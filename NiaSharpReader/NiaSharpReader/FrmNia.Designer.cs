namespace NiaReader
{
    partial class FrmNia
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNia));
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.DataListBox = new System.Windows.Forms.ListBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.ReadSyncButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SampleLabel = new System.Windows.Forms.Label();
            this.SampleRateLabel = new System.Windows.Forms.Label();
            this.PlaybackButton = new System.Windows.Forms.Button();
            this.RecordingCheckBox = new System.Windows.Forms.CheckBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.Location = new System.Drawing.Point(2, 29);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.ScrollGrace = 0;
            this.zedGraphControl.ScrollMaxX = 0;
            this.zedGraphControl.ScrollMaxY = 0;
            this.zedGraphControl.ScrollMaxY2 = 0;
            this.zedGraphControl.ScrollMinX = 0;
            this.zedGraphControl.ScrollMinY = 0;
            this.zedGraphControl.ScrollMinY2 = 0;
            this.zedGraphControl.Size = new System.Drawing.Size(630, 307);
            this.zedGraphControl.TabIndex = 0;
            // 
            // DataListBox
            // 
            this.DataListBox.FormattingEnabled = true;
            this.DataListBox.Location = new System.Drawing.Point(638, 29);
            this.DataListBox.Name = "DataListBox";
            this.DataListBox.Size = new System.Drawing.Size(199, 290);
            this.DataListBox.TabIndex = 6;
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(68, 4);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(52, 23);
            this.StopButton.TabIndex = 7;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ReadSyncButton
            // 
            this.ReadSyncButton.Location = new System.Drawing.Point(12, 4);
            this.ReadSyncButton.Name = "ReadSyncButton";
            this.ReadSyncButton.Size = new System.Drawing.Size(50, 23);
            this.ReadSyncButton.TabIndex = 11;
            this.ReadSyncButton.Text = "Read";
            this.ReadSyncButton.UseVisualStyleBackColor = true;
            this.ReadSyncButton.Click += new System.EventHandler(this.ReadSyncButton_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1;
            // 
            // SampleLabel
            // 
            this.SampleLabel.AutoSize = true;
            this.SampleLabel.Location = new System.Drawing.Point(639, 322);
            this.SampleLabel.Name = "SampleLabel";
            this.SampleLabel.Size = new System.Drawing.Size(108, 13);
            this.SampleLabel.TabIndex = 12;
            this.SampleLabel.Text = "Current Sample Rate:";
            // 
            // SampleRateLabel
            // 
            this.SampleRateLabel.AutoSize = true;
            this.SampleRateLabel.Location = new System.Drawing.Point(791, 322);
            this.SampleRateLabel.Name = "SampleRateLabel";
            this.SampleRateLabel.Size = new System.Drawing.Size(29, 13);
            this.SampleRateLabel.TabIndex = 13;
            this.SampleRateLabel.Text = "1 ms";
            // 
            // PlaybackButton
            // 
            this.PlaybackButton.Location = new System.Drawing.Point(359, 4);
            this.PlaybackButton.Name = "PlaybackButton";
            this.PlaybackButton.Size = new System.Drawing.Size(64, 23);
            this.PlaybackButton.TabIndex = 14;
            this.PlaybackButton.Text = "Playback";
            this.PlaybackButton.UseVisualStyleBackColor = true;
            this.PlaybackButton.Click += new System.EventHandler(this.PlaybackButton_Click);
            // 
            // RecordingCheckBox
            // 
            this.RecordingCheckBox.AutoSize = true;
            this.RecordingCheckBox.Location = new System.Drawing.Point(126, 8);
            this.RecordingCheckBox.Name = "RecordingCheckBox";
            this.RecordingCheckBox.Size = new System.Drawing.Size(61, 17);
            this.RecordingCheckBox.TabIndex = 15;
            this.RecordingCheckBox.Text = "Record";
            this.RecordingCheckBox.UseVisualStyleBackColor = true;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(289, 4);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(64, 23);
            this.LoadButton.TabIndex = 16;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // FrmNia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 338);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.RecordingCheckBox);
            this.Controls.Add(this.PlaybackButton);
            this.Controls.Add(this.SampleRateLabel);
            this.Controls.Add(this.SampleLabel);
            this.Controls.Add(this.zedGraphControl);
            this.Controls.Add(this.ReadSyncButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.DataListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmNia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nia USB Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl;
        private System.Windows.Forms.ListBox DataListBox;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button ReadSyncButton;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label SampleLabel;
        private System.Windows.Forms.Label SampleRateLabel;
        private System.Windows.Forms.Button PlaybackButton;
        private System.Windows.Forms.CheckBox RecordingCheckBox;
        private System.Windows.Forms.Button LoadButton;

    }
}