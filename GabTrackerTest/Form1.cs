using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GabTrackerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            /*Random r = new Random();
            this.gabTracker1.Feeds[0].Value = trackBar1.Value;
            this.gabTracker1.Feeds[1].Value = trackBar1.Value + r.Next(-12, 12);*/
            this.gabTracker1.Feeds[2].Value = trackBar1.Value;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Random r = new Random();
            GabTracker.GabTrackerFeed[] feeds = this.gabTracker1.Feeds;
            Array.Resize(ref feeds, feeds.Length + 1);
            feeds[feeds.Length - 1] = new GabTracker.GabTrackerFeed();
            gabTracker1.Feeds = feeds;
            gabTracker1.Feeds[gabTracker1.Feeds.Length - 1].Value = r.Next(1, 99);*/
            Random r = new Random();
            GabTracker.GabTrackerFeed feed = new GabTracker.GabTrackerFeed();
            feed.Value = r.Next(1, 99);
            gabTracker1.Feeds.Add(feed);
        }
    }
}
