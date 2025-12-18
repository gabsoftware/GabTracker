namespace GabTrackerTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GabTracker.GabTrackerFeed gabTrackerFeed1 = new GabTracker.GabTrackerFeed();
            GabTracker.GabTrackerFeed gabTrackerFeed2 = new GabTracker.GabTrackerFeed();
            GabTracker.GabTrackerFeed gabTrackerFeed3 = new GabTracker.GabTrackerFeed();
            GabTracker.GabTrackerFeed gabTrackerFeed4 = new GabTracker.GabTrackerFeed();
            trackBar1 = new System.Windows.Forms.TrackBar();
            button1 = new System.Windows.Forms.Button();
            gabTracker1 = new GabTracker.GabTracker();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // trackBar1
            // 
            trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            trackBar1.Location = new System.Drawing.Point(4, 12);
            trackBar1.Maximum = 100;
            trackBar1.Name = "trackBar1";
            trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            trackBar1.Size = new System.Drawing.Size(56, 561);
            trackBar1.TabIndex = 1;
            trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(683, 552);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // gabTracker1
            // 
            gabTracker1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gabTracker1.AutoMax = true;
            gabTracker1.AutoMaxPercentage = 120D;
            gabTrackerFeed1.Coefficient = 1D;
            gabTrackerFeed1.FillAlpha = 100;
            gabTrackerFeed1.FillThickness = 10F;
            gabTrackerFeed1.FillUnder = true;
            gabTrackerFeed1.Legend = "Download speed";
            gabTrackerFeed1.LineThickness = 2F;
            gabTrackerFeed1.Unit = "Ko";
            gabTrackerFeed1.Value = 70D;
            gabTrackerFeed2.Coefficient = 1D;
            gabTrackerFeed2.FillAlpha = 100;
            gabTrackerFeed2.FillThickness = 10F;
            gabTrackerFeed2.FillUnder = true;
            gabTrackerFeed2.Legend = "Instant download speed";
            gabTrackerFeed2.LineColor = System.Drawing.Color.RoyalBlue;
            gabTrackerFeed2.LineThickness = 2F;
            gabTrackerFeed2.Unit = "Ko";
            gabTrackerFeed2.Value = 50D;
            gabTrackerFeed3.Coefficient = 1D;
            gabTrackerFeed3.FillAlpha = 100;
            gabTrackerFeed3.FillStopOtherFeeds = false;
            gabTrackerFeed3.FillSuperpose = true;
            gabTrackerFeed3.FillThickness = 1F;
            gabTrackerFeed3.FillUnder = true;
            gabTrackerFeed3.Legend = "Upload speed";
            gabTrackerFeed3.LineColor = System.Drawing.Color.OrangeRed;
            gabTrackerFeed3.LineThickness = 4F;
            gabTrackerFeed3.Unit = "Ko";
            gabTrackerFeed3.Value = 40D;
            gabTrackerFeed4.Coefficient = 1D;
            gabTrackerFeed4.FillAlpha = 255;
            gabTrackerFeed4.FillStopOtherFeeds = false;
            gabTrackerFeed4.FillThickness = 1F;
            gabTrackerFeed4.Legend = "Instant upload speed";
            gabTrackerFeed4.LineColor = System.Drawing.Color.Fuchsia;
            gabTrackerFeed4.LineThickness = 4F;
            gabTrackerFeed4.Unit = "Ko";
            gabTrackerFeed4.Value = 20D;
            gabTracker1.Feeds.Add(gabTrackerFeed1);
            gabTracker1.Feeds.Add(gabTrackerFeed2);
            gabTracker1.Feeds.Add(gabTrackerFeed3);
            gabTracker1.Feeds.Add(gabTrackerFeed4);
            gabTracker1.GridScroll = true;
            gabTracker1.GridThickerThickness = 1F;
            gabTracker1.GridThickness = 1F;
            gabTracker1.LegendOpacity = 128;
            gabTracker1.Location = new System.Drawing.Point(56, 13);
            gabTracker1.Margin = new System.Windows.Forms.Padding(4);
            gabTracker1.Maximum = 84;
            gabTracker1.Name = "gabTracker1";
            gabTracker1.RefreshRate = 200;
            gabTracker1.Size = new System.Drawing.Size(701, 529);
            gabTracker1.TabIndex = 3;
            // 
            // Form1
            // 
            ClientSize = new System.Drawing.Size(770, 585);
            Controls.Add(gabTracker1);
            Controls.Add(button1);
            Controls.Add(trackBar1);
            Name = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private GabTracker.GabTracker gabTracker1;

    }
}

