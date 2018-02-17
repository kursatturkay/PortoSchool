using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Scheduler;

namespace Test_project_template
{
    public partial class Form1 : Form
    {
        CScheduler sch = new CScheduler();
        private object _lock = new object();


        public Form1()
        {
            InitializeComponent();
        }

        public void InsertText(string s)
        {
            consoleTxtBox.Text += s;
            consoleTxtBox.SelectionStart = consoleTxtBox.Text.Length;
            consoleTxtBox.ScrollToCaret();
        }

        public void InsertLine(string s)
        {
            InsertText(s + "\r\n");
        }

        public void InsertLineWithTimeStamp(string s)
        {
            lock (_lock)
            {
                InsertLine(DateTime.Now.ToString() + " " + s);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sch.ShowScheduler();
        }

        private void testtask1()
        {
            //this.InsertLineWithTimeStamp("Start");
            SetText("Start");
        }
        
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleTxtBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.InsertLineWithTimeStamp("Start");
                //this.textBox1.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sch.StartScheduling();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CSchedulation _s;
            CTask _tmp = new CTask("task 1");
            _tmp.StartCallback = new DTaskStart(testtask1);
            sch.AddNewTask(_tmp);
            _s = new CSchedulation(DateTime.Now, _tmp);
            _s.OccurrenceStopTime = DateTime.Now.AddDays(1);
            _s.Status = EStatus.Inactive;
            sch.AddNewScheduling(_s);


            _tmp = new CTask("task 2");
            _tmp.StartCallback = new DTaskStart(testtask1);
            sch.AddNewTask(_tmp);
            _s = new CSchedulation(DateTime.Now, _tmp);
            _s.OccurrenceStartTime = DateTime.Now.AddDays(-1);
            _s.OccurrenceStopTime = DateTime.Now.AddMinutes(-10);
            _s.OccurrenceType = EOccurrence.EveryMinute;
            sch.AddNewScheduling(_s);

            _tmp = new CTask("task 3");
            _tmp.StartCallback = new DTaskStart(testtask1);
            sch.AddNewTask(_tmp);
            sch.AddNewScheduling(new CSchedulation(DateTime.Now, _tmp));
            sch.AddNewScheduling(new CSchedulation(DateTime.Now, _tmp));

            _tmp = new CTask("task 4");
            _tmp.StartCallback = new DTaskStart(testtask1);
            sch.AddNewTask(_tmp);
            _s = new CSchedulation(DateTime.Now, _tmp);
            _s.OccurrenceStopTime = DateTime.Now.AddDays(1);
            _s.OccurrenceType = EOccurrence.Monthly;
            sch.AddNewScheduling(_s);
            sch.AddNewScheduling(new CSchedulation(DateTime.Now.AddSeconds(25), _tmp));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sch.StopScheduling();
        }

    }
}