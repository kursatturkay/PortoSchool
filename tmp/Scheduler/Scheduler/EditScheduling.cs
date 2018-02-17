using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scheduler
{
    internal partial class EditScheduling : Form
    {
        List<CTask> m_task;
        List<CSchedulation> m_schedule;

        public EditScheduling(List<CTask> task, List<CSchedulation> schedule)
        {
            this.m_task = task;
            this.m_schedule = schedule;
            InitializeComponent();
            updateListView();
        }

        /// <summary>
        /// Updates the listview with values from the schedule list
        /// </summary>
        public void updateListView()
        {
            //put columns into the list
            listView1.Items.Clear();
            //listView1.Clear();
            //listView1.Columns.Add("Name", 100);
            //listView1.Columns.Add("Start", 60);
            //listView1.Columns.Add("Stop", 60);
            //listView1.Columns.Add("Status", 60);
            //listView1.Columns.Add("Oc. start", 60);
            //listView1.Columns.Add("Oc. stop", 60);
            //listView1.Columns.Add("Recurrence", 75);

            listView1.BeginUpdate();
            listView1.FullRowSelect = true;

            //Put schedulations into the list
            for (int i = 0; i < m_schedule.Count; i++)
            {
                ListViewItem li = new ListViewItem(m_schedule[i].Task.Name);
                li.UseItemStyleForSubItems = false;
                li.SubItems.Add(m_schedule[i].TaskStart.ToString());
                li.SubItems.Add(m_schedule[i].Status.ToString());
                li.SubItems.Add(m_schedule[i].OccurrenceType.ToString());

                if (m_schedule[i].OccurrenceType == EOccurrence.Once)
                {
                    li.SubItems.Add(m_schedule[i].OccurrenceStartTime.ToString(), Color.LightGray, li.BackColor, li.Font);
                    li.SubItems.Add(m_schedule[i].OccurrenceStopTime.ToString(), Color.LightGray, li.BackColor, li.Font);
                }
                else
                {
                    li.SubItems.Add(m_schedule[i].OccurrenceStartTime.ToString());
                    li.SubItems.Add(m_schedule[i].OccurrenceStopTime.ToString());
                }
                li.SubItems.Add(m_schedule[i].ID.ToString());

                //Set the color of text and background
                //       Green text :  Status = active
                //         Red text :  Status = inactive
                //Yellow background :  Starttime is outside occurrence interval and event is not a "once"-event
                //                     When event is "once" occurrence start/stop is ignored.
                if(m_schedule[i].Status == EStatus.Active)
                {
                    li.ForeColor = Color.Green;
                }
                else
                {
                    li.ForeColor = Color.Red;
                }

                DateTime _now = DateTime.Now;

                if (((m_schedule[i].OccurrenceStartTime.CompareTo(_now) > 0) | (m_schedule[i].OccurrenceStopTime.CompareTo(_now) < 0)) & (m_schedule[i].OccurrenceType != EOccurrence.Once))
                {
                    li.BackColor = Color.Yellow;
                    li.ToolTipText = "Current time is outside occurrence interval";
                }

                listView1.Items.Add(li);
            }

            //UseItemStyleForSubItems has been set to false so I
            //can set individual colors for subitems. This way I can gray
            //occurrence times for "once" schedulations.
            for (int n = 0; n < listView1.Items.Count; n++)
            {
                if (listView1.Items[n].SubItems[4].ForeColor == Color.LightGray)
                {
                    listView1.Items[n].SubItems[0].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[1].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[2].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[3].ForeColor = listView1.Items[n].ForeColor;

                    listView1.Items[n].SubItems[0].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[1].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[2].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[3].BackColor = listView1.Items[n].BackColor;
                }
                else
                {
                    listView1.Items[n].SubItems[0].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[1].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[2].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[3].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[4].ForeColor = listView1.Items[n].ForeColor;
                    listView1.Items[n].SubItems[5].ForeColor = listView1.Items[n].ForeColor;

                    listView1.Items[n].SubItems[0].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[1].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[2].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[3].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[4].BackColor = listView1.Items[n].BackColor;
                    listView1.Items[n].SubItems[5].BackColor = listView1.Items[n].BackColor;
                }
            }

            listView1.EndUpdate();
        }

        /// <summary>
        /// User ok'ed the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// User pressed cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Adds a new schedulation via a dialog-box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            NewSchedulation dlg = new NewSchedulation(m_task);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_schedule.Add((CSchedulation)dlg.Tag);
                updateListView();
            }
        }

        /// <summary>
        /// Removes the selected schedulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < listView1.SelectedItems.Count; n++)
            {
                for (int i = 0; i < m_schedule.Count; i++)
                {
                    if (listView1.SelectedItems[n].SubItems[6].Text == m_schedule[i].ID.ToString())
                    {
                        m_schedule.RemoveAt(i);
                        updateListView();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Edit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                //find the correct entry in schedule
                int _tmp = 0;
                for (int i = 0; i < m_schedule.Count; i++)
                {
                    if (listView1.SelectedItems[0].SubItems[6].Text == m_schedule[i].ID.ToString())
                    {
                        _tmp = i;
                        break;
                    }
                }

                NewSchedulation dlg = new NewSchedulation(m_task, m_schedule[_tmp]);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //m_schedule.Add((CSchedulation)dlg.Tag);
                    m_schedule[_tmp] = ((CSchedulation)dlg.Tag);
                    updateListView();
                }
            }
        }

        /// <summary>
        /// toggle active/inactive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActive_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                //find the correct entry in schedule
                int _tmp = 0;
                for (int i = 0; i < m_schedule.Count; i++)
                {
                    if (listView1.SelectedItems[0].SubItems[6].Text == m_schedule[i].ID.ToString())
                    {
                        _tmp = i;
                        break;
                    }
                }

                if (m_schedule[_tmp].Status == EStatus.Active)
                {
                    m_schedule[_tmp].Status = EStatus.Inactive;
                }
                else
                {
                    m_schedule[_tmp].Status = EStatus.Active;
                }

                updateListView();
            }
        }

        /// <summary>
        /// Clears schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear\r\nthe whole schedule?", "Clear complete schedulation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                m_schedule.Clear();
                updateListView();
            }

        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}