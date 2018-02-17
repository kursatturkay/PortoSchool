using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public enum EOccurrence
    {
        Once,
        EveryMinute,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum EStatus
    {
        Active = 0,
        Inactive = 1
    }

    /// <summary>
    /// This represents a point in time where you want
    /// a particular task executed. The class has a start and stop time
    /// where the task should be executed.
    /// If the task is reoccurring and a timespan limit on the occurrence.
    /// </summary>
    public class CSchedulation
    {

        #region Attributter

        private DateTime m_lastAlarm = DateTime.MinValue;
        /// <summary>
        /// The last time an alarm was executed
        /// as a cause of this schedulation.
        /// </summary>
        public DateTime LastAlarm
        {
            get { return m_lastAlarm; }
            set { m_lastAlarm = value; }
        }

        private Guid m_id;
        /// <summary>
        /// An unique id that can be used
        /// to identify an instance of
        /// this class.
        /// </summary>
        public Guid ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private DateTime m_taskStart = DateTime.Now;
        /// <summary>
        /// The point in time where the task should
        /// be executed.
        /// </summary>
        public DateTime TaskStart
        {
            get
            {
                return m_taskStart;
            }
            set
            {
                m_taskStart = value;
            }
        }

        /// <summary>
        /// How often this task should be executed
        /// </summary>
        public EOccurrence OccurrenceType = EOccurrence.Once;

        /// <summary>
        /// The status of this schedulation
        /// </summary>
        public EStatus Status = EStatus.Active;

        private DateTime m_occurrenceStartTime = DateTime.Now;
        /// <summary>
        /// The starting point in time where scheduling of
        /// this task should begin.
        /// This can be left null for no restrictions.
        /// </summary>
        public DateTime OccurrenceStartTime
        {
            get { return m_occurrenceStartTime; }
            set { m_occurrenceStartTime = value; }
        }

        private DateTime m_occurrenceStopTime = DateTime.Now;
        /// <summary>
        /// The starting point in time where scheduling of
        /// this task should end. After this time its status
        /// will be changed to Inactive.
        /// This can be left null for no restrictions.
        /// </summary>
        public DateTime OccurrenceStopTime
        {
            get { return m_occurrenceStopTime; }
            set { m_occurrenceStopTime = value; }
        }

        /// <summary>
        /// The task to be performed
        /// </summary>
        public CTask Task = null;

        #endregion

        public CSchedulation(DateTime start, CTask task)
        {
            this.Task = task;
            TaskStart = start;
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rhs"></param>
        public CSchedulation(CSchedulation rhs)
        {
            this.Task = rhs.Task;
            this.TaskStart = rhs.TaskStart;
            this.OccurrenceStopTime = rhs.OccurrenceStopTime;
            this.OccurrenceStartTime = rhs.OccurrenceStartTime;
            this.Status = rhs.Status;
            this.OccurrenceType = rhs.OccurrenceType;
            this.ID = rhs.ID;
            this.LastAlarm = rhs.LastAlarm;
        }

    }
}
