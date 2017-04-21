using System;
namespace GabTracker
{
    partial class GabTracker
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

                _backbrush.Dispose();
                _gridpen.Dispose();
                Array.Clear(_arrdata, 0, _arrdata.Length);
                Array.Clear(_arrtmp, 0, _arrtmp.Length);
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.internalTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // internalTimer
            // 
            this.internalTimer.Tick += new System.EventHandler(this.internalTimer_Tick);
            // 
            // GabTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GabTracker";
            this.Size = new System.Drawing.Size(261, 171);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GabTracker_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer internalTimer;
    }
}
