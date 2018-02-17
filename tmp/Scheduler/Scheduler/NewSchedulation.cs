using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scheduler
{
    internal partial class NewSchedulation : Form
    {
        List<CTask> m_tasks;

        public NewSchedulation(List<CTask> tasks, CSchedulation sch)
        {
            InitializeComponent();
            m_tasks = tasks;

            int _n = 0;
            for (int i = 0; i < m_tasks.Count; i++)
            {
                comboBoxTask.Items.Add(m_tasks[i].Name);
                if (m_tasks[i].Name == sch.Task.Name)
                {
                    _n = i;
                }
            }
            comboBoxTask.SelectedIndex = _n;

            comboBoxOccurrence.SelectedIndex = (int)sch.OccurrenceType;   //once
            dateTimePickerOcStart.Value = sch.OccurrenceStartTime;
            dateTimePickerOcStop.Value = sch.OccurrenceStopTime;
            dateTimePickerStart.Value = sch.TaskStart;

            comboBoxStatus.SelectedIndex = (int)sch.Status;   //active
        }

        public NewSchedulation(List<CTask> tasks)
        {
            InitializeComponent();

            m_tasks = tasks;

            for (int i = 0; i < m_tasks.Count; i++)
			{
                comboBoxTask.Items.Add(m_tasks[i].Name);
            }
            if (m_tasks.Count != 0)
                comboBoxTask.SelectedIndex = 0;

            comboBoxOccurrence.SelectedIndex = 0;   //once
            dateTimePickerOcStart.Enabled = false;
            dateTimePickerOcStop.Enabled = false;

            comboBoxStatus.SelectedIndex = 0;   //active
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            CTask _task = null;
            for (int i = 0; i < m_tasks.Count; i++)
			{
                if(m_tasks[i].Name == comboBoxTask.Items[comboBoxTask.SelectedIndex].ToString())
                {
                    _task = m_tasks[i];
                }
            }

            CSchedulation _t = new CSchedulation(dateTimePickerStart.Value, _task);
            _t.Status = (EStatus)comboBoxStatus.SelectedIndex;
            _t.OccurrenceStartTime = dateTimePickerOcStart.Value;
            _t.OccurrenceStopTime = dateTimePickerOcStop.Value;
            _t.OccurrenceType = (EOccurrence)comboBoxOccurrence.SelectedIndex;
            this.Tag = _t;

            if (_t.OccurrenceStopTime.CompareTo(_t.OccurrenceStartTime) < 0 & _t.OccurrenceType != EOccurrence.Once)
            {
                MessageBox.Show("Occurrence start is AFTER occurrence stop", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxOccurrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOccurrence.SelectedIndex != 0)
            {
                dateTimePickerOcStart.Enabled = true;
                dateTimePickerOcStop.Enabled = true;
            }
            else
            {
                dateTimePickerOcStart.Enabled = false;
                dateTimePickerOcStop.Enabled = false;
            }
        }
    }
}