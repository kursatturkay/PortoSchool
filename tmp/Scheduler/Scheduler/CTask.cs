using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class CTask
    {
        #region Attributter

        private string m_name;
        /// <summary>
        /// The name describing the task to
        /// be executed
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private DTaskStart m_startCallback = null;
        /// <summary>
        /// The callback to execute when
        /// starting this task
        /// </summary>
        public DTaskStart StartCallback
        {
            get
            {
                return m_startCallback;
            }
            set
            {
                m_startCallback = value;
            }
        }
	
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the task. Don't give tasks
        /// the same name!</param>
        /// <param name="startCallback">The callback to be executed on starttime is reached</param>
        public CTask(string name, DTaskStart startCallback)
        {
            this.Name = name;
            this.StartCallback = startCallback;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the task. Don't give tasks
        /// the same name!</param>
        public CTask(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rhs"></param>
        public CTask(CTask rhs)
        {
            this.m_name = rhs.m_name;
            this.m_startCallback = rhs.m_startCallback;
        }

    }

    public delegate void DTaskStart();

}
