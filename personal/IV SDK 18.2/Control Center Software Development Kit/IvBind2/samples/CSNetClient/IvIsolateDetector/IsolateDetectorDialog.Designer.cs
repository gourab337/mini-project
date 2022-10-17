namespace IvIsolateDetector
{
    partial class IsolateDetectorDialog
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
            this.detectorNameTextBox = new System.Windows.Forms.TextBox();
            this.detectorNameLabel = new System.Windows.Forms.Label();
            this.isolateDetectorButton = new System.Windows.Forms.Button();
            this.zoneNameTextBox = new System.Windows.Forms.TextBox();
            this.zoneNameLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.asIpAddressLabel = new System.Windows.Forms.Label();
            this.isolateReasonLabel = new System.Windows.Forms.Label();
            this.isolateReasonTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // detectorNameTextBox
            // 
            this.detectorNameTextBox.Location = new System.Drawing.Point(154, 89);
            this.detectorNameTextBox.Name = "detectorNameTextBox";
            this.detectorNameTextBox.Size = new System.Drawing.Size(164, 20);
            this.detectorNameTextBox.TabIndex = 12;
            // 
            // detectorNameLabel
            // 
            this.detectorNameLabel.AutoSize = true;
            this.detectorNameLabel.Location = new System.Drawing.Point(68, 93);
            this.detectorNameLabel.Name = "detectorNameLabel";
            this.detectorNameLabel.Size = new System.Drawing.Size(82, 13);
            this.detectorNameLabel.TabIndex = 11;
            this.detectorNameLabel.Text = "&Detector Name:";
            // 
            // isolateDetectorButton
            // 
            this.isolateDetectorButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.isolateDetectorButton.Location = new System.Drawing.Point(232, 150);
            this.isolateDetectorButton.Name = "isolateDetectorButton";
            this.isolateDetectorButton.Size = new System.Drawing.Size(86, 25);
            this.isolateDetectorButton.TabIndex = 13;
            this.isolateDetectorButton.Text = "&Isolate";
            this.isolateDetectorButton.Click += new System.EventHandler(this.isolateDetectorButton_Click);
            // 
            // zoneNameTextBox
            // 
            this.zoneNameTextBox.Location = new System.Drawing.Point(154, 56);
            this.zoneNameTextBox.Name = "zoneNameTextBox";
            this.zoneNameTextBox.Size = new System.Drawing.Size(164, 20);
            this.zoneNameTextBox.TabIndex = 10;
            // 
            // zoneNameLabel
            // 
            this.zoneNameLabel.AutoSize = true;
            this.zoneNameLabel.Location = new System.Drawing.Point(84, 60);
            this.zoneNameLabel.Name = "zoneNameLabel";
            this.zoneNameLabel.Size = new System.Drawing.Size(66, 13);
            this.zoneNameLabel.TabIndex = 9;
            this.zoneNameLabel.Text = "&Zone Name:";
            // 
            // asIpAddressTextBox
            // 
            this.asIpAddressTextBox.Location = new System.Drawing.Point(154, 23);
            this.asIpAddressTextBox.Name = "asIpAddressTextBox";
            this.asIpAddressTextBox.Size = new System.Drawing.Size(164, 20);
            this.asIpAddressTextBox.TabIndex = 8;
            this.asIpAddressTextBox.Text = "0.0.0.0";
            // 
            // asIpAddressLabel
            // 
            this.asIpAddressLabel.AutoSize = true;
            this.asIpAddressLabel.Location = new System.Drawing.Point(26, 27);
            this.asIpAddressLabel.Name = "asIpAddressLabel";
            this.asIpAddressLabel.Size = new System.Drawing.Size(124, 13);
            this.asIpAddressLabel.TabIndex = 7;
            this.asIpAddressLabel.Text = "Alarm Server IP &Address:";
            // 
            // isolateReasonLabel
            // 
            this.isolateReasonLabel.AutoSize = true;
            this.isolateReasonLabel.Location = new System.Drawing.Point(69, 122);
            this.isolateReasonLabel.Name = "isolateReasonLabel";
            this.isolateReasonLabel.Size = new System.Drawing.Size(81, 13);
            this.isolateReasonLabel.TabIndex = 14;
            this.isolateReasonLabel.Text = "Isolate Reason:";
            // 
            // isolateReasonTextBox
            // 
            this.isolateReasonTextBox.Location = new System.Drawing.Point(154, 119);
            this.isolateReasonTextBox.Name = "isolateReasonTextBox";
            this.isolateReasonTextBox.Size = new System.Drawing.Size(164, 20);
            this.isolateReasonTextBox.TabIndex = 15;
            // 
            // IsolateDetectorDialog
            // 
            this.AcceptButton = this.isolateDetectorButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 182);
            this.Controls.Add(this.isolateReasonTextBox);
            this.Controls.Add(this.isolateReasonLabel);
            this.Controls.Add(this.detectorNameTextBox);
            this.Controls.Add(this.detectorNameLabel);
            this.Controls.Add(this.isolateDetectorButton);
            this.Controls.Add(this.zoneNameTextBox);
            this.Controls.Add(this.zoneNameLabel);
            this.Controls.Add(this.asIpAddressTextBox);
            this.Controls.Add(this.asIpAddressLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IsolateDetectorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Isolate Detector Dialog";
            this.Load += new System.EventHandler(this.IsolateDetectorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox detectorNameTextBox;
        internal System.Windows.Forms.Label detectorNameLabel;
        internal System.Windows.Forms.Button isolateDetectorButton;
        internal System.Windows.Forms.TextBox zoneNameTextBox;
        internal System.Windows.Forms.Label zoneNameLabel;
        internal System.Windows.Forms.TextBox asIpAddressTextBox;
        internal System.Windows.Forms.Label asIpAddressLabel;
        private System.Windows.Forms.Label isolateReasonLabel;
        internal System.Windows.Forms.TextBox isolateReasonTextBox;
    }
}

