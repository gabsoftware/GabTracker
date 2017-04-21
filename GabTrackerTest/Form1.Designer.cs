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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.gabTracker1 = new GabTracker.GabTracker();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBar1.Location = new System.Drawing.Point(4, 12);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 561);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(683, 552);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gabTracker1
            // 
            this.gabTracker1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gabTracker1.AutoMax = true;
            this.gabTracker1.AutoMaxPercentage = 120D;
            gabTrackerFeed1.Coefficient = 1D;
            gabTrackerFeed1.FillAlpha = ((byte)(100));
            gabTrackerFeed1.FillThickness = 10F;
            gabTrackerFeed1.FillUnder = true;
            gabTrackerFeed1.Legend = "Download speed";
            gabTrackerFeed1.LineThickness = 2F;
            gabTrackerFeed1.Unit = "Ko";
            gabTrackerFeed1.Value = 70D;
            gabTrackerFeed2.Coefficient = 1D;
            gabTrackerFeed2.FillAlpha = ((byte)(100));
            gabTrackerFeed2.FillThickness = 10F;
            gabTrackerFeed2.FillUnder = true;
            gabTrackerFeed2.Legend = "Instant download speed";
            gabTrackerFeed2.LineColor = System.Drawing.Color.RoyalBlue;
            gabTrackerFeed2.LineThickness = 2F;
            gabTrackerFeed2.Unit = "Ko";
            gabTrackerFeed2.Value = 50D;
            gabTrackerFeed3.Coefficient = 1D;
            gabTrackerFeed3.FillAlpha = ((byte)(255));
            gabTrackerFeed3.FillStopOtherFeeds = false;
            gabTrackerFeed3.FillThickness = 1F;
            gabTrackerFeed3.Legend = "Upload speed";
            gabTrackerFeed3.LineColor = System.Drawing.Color.OrangeRed;
            gabTrackerFeed3.LineThickness = 4F;
            gabTrackerFeed3.Unit = "Ko";
            gabTrackerFeed3.Value = 40D;
            gabTrackerFeed4.Coefficient = 1D;
            gabTrackerFeed4.FillAlpha = ((byte)(255));
            gabTrackerFeed4.FillStopOtherFeeds = false;
            gabTrackerFeed4.FillThickness = 1F;
            gabTrackerFeed4.Legend = "Instant upload speed";
            gabTrackerFeed4.LineColor = System.Drawing.Color.Fuchsia;
            gabTrackerFeed4.LineThickness = 4F;
            gabTrackerFeed4.Unit = "Ko";
            gabTrackerFeed4.Value = 20D;
            this.gabTracker1.Feeds.Add(gabTrackerFeed1);
            this.gabTracker1.Feeds.Add(gabTrackerFeed2);
            this.gabTracker1.Feeds.Add(gabTrackerFeed3);
            this.gabTracker1.Feeds.Add(gabTrackerFeed4);
            this.gabTracker1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(0)))));
            this.gabTracker1.GridScroll = true;
            this.gabTracker1.GridThickerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(0)))));
            this.gabTracker1.GridThickerThickness = 1F;
            this.gabTracker1.GridThickness = 1F;
            this.gabTracker1.LegendOpacity = ((byte)(128));
            this.gabTracker1.Location = new System.Drawing.Point(56, 13);
            this.gabTracker1.Margin = new System.Windows.Forms.Padding(4);
            this.gabTracker1.Maximum = 84;
            this.gabTracker1.Name = "gabTracker1";
            this.gabTracker1.RefreshRate = 200;
            this.gabTracker1.Size = new System.Drawing.Size(701, 529);
            this.gabTracker1.TabIndex = 3;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(770, 585);
            this.Controls.Add(this.gabTracker1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.trackBar1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private GabTracker.GabTracker gabTracker1;

    }
}

