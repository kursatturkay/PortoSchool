using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scheduler
{

    /// <summary>
    /// First you fill in tasks that can be performed
    /// and finally you fill in the scheduling of tasks
    /// either by doing it in code or by letting the user
    /// do it through the EditScheduling dialog-box.
    /// At some time you need to call the StartScheduling method
    /// to start the scheduling. This you can do at any time
    /// you please. You dont necessarily have to do it _after_
    /// you add tasks and schedules.
    /// 
    /// If this is to be used as solely for the benefit of the developer
    /// then you dont need to use the AddNewScheduledTask-method
    /// as this method is only used for the EditScheduling dialog-box.
    /// </summary>
    public class CScheduler
    {
        #region Attributter

        private bool m_isRunning = true;

        /// <summary>
        /// This is used as syncronization-object in
        /// the tasks list.
        /// </summary>
        private object m_lockTasks = new object();

        private List<CTask> m_tasks = new List<CTask>();
        /// <summary>
        /// The list of tasks that can be preformed
        /// </summary>
        private List<CTask> Tasks
        {
            get
            {
                return m_tasks;
            }
        }


        /// <summary>
        /// This is used as syncronization-object in
        /// the schedulation list.
        /// </summary>
        private object m_lockSchedule = new object();

        private List<CSchedulation> m_schedule = new List<CSchedulation>();
        /// <summary>
        /// This is a list of points in time where
        /// tasks should be performed.
        /// </summary>
        private List<CSchedulation> Schedule
        {
            get
            {
                lock (m_lockSchedule)
                {
                    return m_schedule;
                }
            }
        }

        Thread m_scheduleThread;

        #endregion


        public CScheduler()
        {
            m_scheduleThread = new Thread(new ThreadStart(scheduleRunner));
        }

        /// <summary>
        /// Add a new task that can be executed at some point
        /// in time. The list of tasks added will only be used when
        /// showing the EditScheduling dialog-box.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool AddNewTask(CTask task)
        {
            if (task.StartCallback == null)
            {
                throw new Exception("Error! No start-callback function set");
            }

            for (int i = 0; i < m_tasks.Count; i++)
            {
                if (m_tasks[i].Name == task.Name)
                {
                    throw new Exception("Error! Taskname already exists");
                }
            }

            m_tasks.Add(task);
            return true;
        }

        /// <summary>
        /// Add a new point in time to do a task
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public bool AddNewScheduling(CSchedulation schedule)
        {
            //sanity check on occurrence dates
            if (schedule.OccurrenceStopTime.CompareTo(schedule.OccurrenceStartTime) < 0)
            {
                throw new Exception("Error! Occurrence stop is _before_ occurrence start");
            }

            m_schedule.Add(schedule);
            return true;
        }

        /// <summary>
        /// Purges all schedulations
        /// </summary>
        public void ClearSchedule()
        {
            m_schedule.Clear();
        }

        /// <summary>
        /// Purges all tasks
        /// </summary>
        public void ClearTasks()
        {
            m_tasks.Clear();
        }

        /// <summary>
        /// This start the execution of tasks according.
        /// This method will return immediatly because it
        /// starts it own thread for the schedulation.
        /// </summary>
        public void StartScheduling()
        {
            //Restart the thread if it has run and then stopped.
            if (m_scheduleThread.ThreadState == ThreadState.Stopped)
            {
                m_scheduleThread = new Thread(new ThreadStart(scheduleRunner));
            }

            if (m_scheduleThread.ThreadState == ThreadState.Unstarted)
            {
                m_scheduleThread.Start();
            }
        }

        /// <summary>
        /// This stops the execution of tasks.
        /// It suspends the thread that takes
        /// care of the schedulation.
        /// </summary>
        public void StopScheduling()
        {
            m_isRunning = false;
            //if (m_scheduleThread.ThreadState != ThreadState.Unstarted)
            //{
            //    m_scheduleThread.Suspend();
            //}
        }

        /// <summary>
        /// Used to show the GUI for adding and showing schedulations
        /// </summary>
        public void ShowScheduler()
        {
            //We make copies of the lists and their objects so
            //we can press the Cancel button
            List<CTask> _ta = new List<CTask>();
            List<CSchedulation> _sc = new List<CSchedulation>();
            for (int i = 0; i < m_tasks.Count; i++)
            {
                _ta.Add(new CTask(m_tasks[i]));
            }
            for (int i = 0; i < m_schedule.Count; i++)
            {
                _sc.Add(new CSchedulation(m_schedule[i]));
            }

            EditScheduling dlg = new EditScheduling(_ta, _sc);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
            }
            else
            {
                lock (m_schedule)
                {
                    m_schedule.Clear();
                    m_tasks.Clear();
                    for (int i = 0; i < _ta.Count; i++)
                    {
                        m_tasks.Add(new CTask(_ta[i]));
                    }
                    for (int i = 0; i < _sc.Count; i++)
                    {
                        m_schedule.Add(new CSchedulation(_sc[i]));
                    }

                }
            }

            //EditScheduling dlg = new EditScheduling(m_tasks, m_schedule);
            //dlg.ShowDialog();

        }


        /// <summary>
        /// Signal the thread in this method to exit by setting
        /// m_isRunning to false.
        /// </summary>
        private void scheduleRunner()
        {
            m_isRunning = true;
            while (m_isRunning)
            {
                System.Threading.Thread.Sleep(200);
                lock (m_schedule)
                {
                    DateTime _now = DateTime.Now;               //Get the point in time.
                    //go through all schedulations and check
                    for (int i = 0; i < m_schedule.Count; i++)
                    {
                        //Is the ocurrence time sat at all and if it is, is it before the current time
                        if (m_schedule[i].Status == EStatus.Active)
                        {
                            switch (m_schedule[i].OccurrenceType)
                            {
                                #region Once

                                case EOccurrence.Once:
                                    //-- check start time
                                    if ((m_schedule[i].TaskStart.CompareTo(_now) < 0) & (m_schedule[i].TaskStart.AddSeconds(0.38).CompareTo(_now) > 0))
                                    {
                                        if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                        {
                                            m_schedule[i].LastAlarm = _now;
                                            if (m_schedule[i].Task.StartCallback != null)
                                                m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                        }
                                    }
                                    break;
                                #endregion

                                #region Minute

                                case EOccurrence.EveryMinute:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.Second == _now.Second)
                                        {
                                            if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                            {
                                                m_schedule[i].LastAlarm = _now;
                                                if (m_schedule[i].Task.StartCallback != null)
                                                    m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                #region Hour

                                case EOccurrence.Hourly:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.Minute == _now.Minute)
                                        {
                                            if (m_schedule[i].TaskStart.Second == _now.Second)
                                            {
                                                if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                                {
                                                    m_schedule[i].LastAlarm = _now;
                                                    if (m_schedule[i].Task.StartCallback != null)
                                                        m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                #region Daily

                                case EOccurrence.Daily:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.Hour == _now.Hour)
                                        {
                                            if (m_schedule[i].TaskStart.Minute == _now.Minute)
                                            {
                                                if (m_schedule[i].TaskStart.Second == _now.Second)
                                                {
                                                    if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                                    {
                                                        m_schedule[i].LastAlarm = _now;
                                                        if (m_schedule[i].Task.StartCallback != null)
                                                            m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                #region Week

                                case EOccurrence.Weekly:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.DayOfWeek == _now.DayOfWeek)
                                        {
                                            if (m_schedule[i].TaskStart.Hour == _now.Hour)
                                            {
                                                if (m_schedule[i].TaskStart.Minute == _now.Minute)
                                                {
                                                    if (m_schedule[i].TaskStart.Second == _now.Second)
                                                    {
                                                        if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                                        {
                                                            m_schedule[i].LastAlarm = _now;
                                                            if (m_schedule[i].Task.StartCallback != null)
                                                                m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                #region Month

                                case EOccurrence.Monthly:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.Day == _now.Day)
                                        {
                                            if (m_schedule[i].TaskStart.Hour == _now.Hour)
                                            {
                                                if (m_schedule[i].TaskStart.Minute == _now.Minute)
                                                {
                                                    if (m_schedule[i].TaskStart.Second == _now.Second)
                                                    {
                                                        if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                                        {
                                                            m_schedule[i].LastAlarm = _now;
                                                            if (m_schedule[i].Task.StartCallback != null)
                                                                m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                #region Year

                                case EOccurrence.Yearly:
                                    if ((m_schedule[i].OccurrenceStartTime.CompareTo(_now) < 0) & (m_schedule[i].OccurrenceStopTime.CompareTo(_now) > 0))
                                    {
                                        if (m_schedule[i].TaskStart.DayOfYear == _now.DayOfYear)
                                        {
                                            if (m_schedule[i].TaskStart.Hour == _now.Hour)
                                            {
                                                if (m_schedule[i].TaskStart.Minute == _now.Minute)
                                                {
                                                    if (m_schedule[i].TaskStart.Second == _now.Second)
                                                    {
                                                        if (_now.Subtract(m_schedule[i].LastAlarm) > new TimeSpan(0, 0, 1))
                                                        {
                                                            m_schedule[i].LastAlarm = _now;
                                                            if (m_schedule[i].Task.StartCallback != null)
                                                                m_schedule[i].Task.StartCallback.BeginInvoke(null, null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                #endregion

                                default:
                                    System.Windows.Forms.MessageBox.Show("Fell through switch in schedule Thread", "Error!");
                                    break;
                            }
                        }
                    }
                }
            }
        }

    }
}
