using System.Windows.Forms;
using System.ComponentModel;
using System;
using System.Drawing;
namespace WinFormsUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            lock (_lock)
            {
                this.logText = new RichTextBox();
            }
            this.logFileGroup     = new GroupBox();
            this.openLogFileBtn   = new Button();
            this.closeLogFileBtn  = new Button();
            this.connectionGroup  = new GroupBox();
            this.connectBtn       = new Button();
            this.disconnectBtn    = new Button();
            this.measurementGroup = new GroupBox();
            this.startBtn         = new Button();
            this.breakBtn         = new Button();
            this.logFileGroup.SuspendLayout();
            this.connectionGroup.SuspendLayout();
            this.measurementGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // logText
            // 
            lock (_lock)
            {
                this.logText.Location = new Point(18, 22);
                this.logText.Name     = "logText";
                this.logText.ReadOnly = true;
                this.logText.Size     = new Size(776, 338);
                this.logText.TabIndex = 0;
                this.logText.Text     = "";
            }
            // 
            // logFileGroup
            // 
            this.logFileGroup.Controls.Add(this.openLogFileBtn);
            this.logFileGroup.Controls.Add(this.closeLogFileBtn);
            this.logFileGroup.Location = new Point(18, 381);
            this.logFileGroup.Name     = "logFileGroup";
            this.logFileGroup.Size     = new Size(776, 80);
            this.logFileGroup.TabIndex = 1;
            this.logFileGroup.TabStop  = false;
            this.logFileGroup.Text     = "Log File";
            // 
            // openLogFileBtn
            // 
            this.openLogFileBtn.Location                = new Point(96, 26);
            this.openLogFileBtn.Name                    = "openLogFileBtn";
            this.openLogFileBtn.Size                    = new Size(138, 41);
            this.openLogFileBtn.TabIndex                = 0;
            this.openLogFileBtn.Text                    = "Open";
            this.openLogFileBtn.UseVisualStyleBackColor = true;
            this.openLogFileBtn.Click                  += new EventHandler(this.OpenLogFileBtn_Click);
            // 
            // closeLogFileBtn
            // 
            this.closeLogFileBtn.Location                = new Point(555, 26);
            this.closeLogFileBtn.Name                    = "closeLogFileBtn";
            this.closeLogFileBtn.Size                    = new Size(138, 41);
            this.closeLogFileBtn.TabIndex                = 0;
            this.closeLogFileBtn.Text                    = "Close";
            this.closeLogFileBtn.UseVisualStyleBackColor = true;
            this.closeLogFileBtn.Click                  += new System.EventHandler(this.CloseLogFileBtn_Click);
            // 
            // connectionGroup
            // 
            this.connectionGroup.Controls.Add(this.connectBtn);
            this.connectionGroup.Controls.Add(this.disconnectBtn);
            this.connectionGroup.Location = new Point(18, 485);
            this.connectionGroup.Name     = "connectionGroup";
            this.connectionGroup.Size     = new Size(776, 80);
            this.connectionGroup.TabIndex = 1;
            this.connectionGroup.TabStop  = false;
            this.connectionGroup.Text     = "Connection";
            // 
            // connectBtn
            // 
            this.connectBtn.Location                = new Point(96, 26);
            this.connectBtn.Name                    = "connectBtn";
            this.connectBtn.Size                    = new Size(138, 41);
            this.connectBtn.TabIndex                = 0;
            this.connectBtn.Text                    = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click                  += new EventHandler(this.ConnectBtn_Click);
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Location                = new Point(555, 26);
            this.disconnectBtn.Name                    = "disconnectBtn";
            this.disconnectBtn.Size                    = new Size(138, 41);
            this.disconnectBtn.TabIndex                = 0;
            this.disconnectBtn.Text                    = "Disconnect";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click                  += new EventHandler(this.DisconnectBtn_Click);
            // 
            // measurementGroup
            // 
            this.measurementGroup.Controls.Add(this.startBtn);
            this.measurementGroup.Controls.Add(this.breakBtn);
            this.measurementGroup.Location = new Point(18, 588);
            this.measurementGroup.Name     = "measurementGroup";
            this.measurementGroup.Size     = new Size(776, 80);
            this.measurementGroup.TabIndex = 1;
            this.measurementGroup.TabStop  = false;
            this.measurementGroup.Text     = "Measurement";
            // 
            // startBtn
            // 
            this.startBtn.Location                = new Point(96, 26);
            this.startBtn.Name                    = "startBtn";
            this.startBtn.Size                    = new Size(138, 41);
            this.startBtn.TabIndex                = 0;
            this.startBtn.Text                    = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click                  += new EventHandler(this.StartBtn_Click);
            // 
            // breakBtn
            // 
            this.breakBtn.Location                = new Point(555, 26);
            this.breakBtn.Name                    = "breakBtn";
            this.breakBtn.Size                    = new Size(138, 41);
            this.breakBtn.TabIndex                = 0;
            this.breakBtn.Text                    = "Break";
            this.breakBtn.UseVisualStyleBackColor = true;
            this.breakBtn.Click                  += new EventHandler(this.BreakBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode       = AutoScaleMode.Font;
            this.ClientSize          = new Size(810, 684);
            lock (_lock)
            {
                this.Controls.Add(this.logText);
            }

            this.Controls.Add(this.measurementGroup);
            this.Controls.Add(this.logFileGroup);
            this.Controls.Add(this.connectionGroup);
            this.Name = "MainForm";
            this.Text = "IAS0410 Logger";
            this.logFileGroup.ResumeLayout(false);
            this.connectionGroup.ResumeLayout(false);
            this.measurementGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox logText;
        private GroupBox logFileGroup;
        private Button closeLogFileBtn;
        private Button openLogFileBtn;
        private GroupBox connectionGroup;
        private Button connectBtn;
        private Button disconnectBtn;
        private GroupBox measurementGroup;
        private Button startBtn;
        private Button breakBtn;
    }
}