namespace InternetChecks
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            infoToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            testToolStripMenuItem = new ToolStripMenuItem();
            timer1_CheckNetStatus = new System.Windows.Forms.Timer(components);
            timer2_ContextMenuUpdate = new System.Windows.Forms.Timer(components);
            timer3_Fading = new System.Windows.Forms.Timer(components);
            timer4_DelayFading = new System.Windows.Forms.Timer(components);
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Check Net Alive";
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { infoToolStripMenuItem, exitToolStripMenuItem, testToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(241, 133);
            contextMenuStrip1.Closed += contextMenuStrip1_Closed;
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // infoToolStripMenuItem
            // 
            infoToolStripMenuItem.Enabled = false;
            infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            infoToolStripMenuItem.Size = new Size(240, 32);
            infoToolStripMenuItem.Text = "info";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(240, 32);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(240, 32);
            testToolStripMenuItem.Text = "test";
            testToolStripMenuItem.Visible = false;
            testToolStripMenuItem.Click += testToolStripMenuItem_Click;
            // 
            // timer1_CheckNetStatus
            // 
            timer1_CheckNetStatus.Interval = 60000;
            timer1_CheckNetStatus.Tick += timer1_Tick;
            // 
            // timer2_ContextMenuUpdate
            // 
            timer2_ContextMenuUpdate.Interval = 500;
            timer2_ContextMenuUpdate.Tick += timer2_Tick;
            // 
            // timer3_Fading
            // 
            timer3_Fading.Interval = 5;
            timer3_Fading.Tick += timer3_Tick;
            // 
            // timer4_DelayFading
            // 
            timer4_DelayFading.Interval = 5000;
            timer4_DelayFading.Tick += timer4_DelayFading_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(224, 118);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer timer1_CheckNetStatus;
        private ToolStripMenuItem infoToolStripMenuItem;
        
        private System.Windows.Forms.Timer timer2_ContextMenuUpdate;
        private ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.Timer timer3_Fading;
        private System.Windows.Forms.Timer timer4_DelayFading;
    }
}
