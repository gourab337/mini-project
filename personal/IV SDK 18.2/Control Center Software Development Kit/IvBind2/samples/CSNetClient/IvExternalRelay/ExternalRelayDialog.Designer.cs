namespace IvExternalRelay
{
    partial class ExternalRelayDialog
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
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.localIpAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.startRelayButton = new System.Windows.Forms.Button();
            this.stopRelayButton = new System.Windows.Forms.Button();
            this.externalRelayTextBox = new System.Windows.Forms.TextBox();
            this.settingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.ipAddressLabel);
            this.settingsGroupBox.Controls.Add(this.localIpAddressTextBox);
            this.settingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(247, 59);
            this.settingsGroupBox.TabIndex = 6;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Settings";
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.Location = new System.Drawing.Point(19, 28);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(90, 13);
            this.ipAddressLabel.TabIndex = 0;
            this.ipAddressLabel.Text = "Local IP &Address:";
            // 
            // localIpAddressTextBox
            // 
            this.localIpAddressTextBox.AsciiOnly = true;
            this.localIpAddressTextBox.Location = new System.Drawing.Point(115, 25);
            this.localIpAddressTextBox.Name = "localIpAddressTextBox";
            this.localIpAddressTextBox.Size = new System.Drawing.Size(126, 20);
            this.localIpAddressTextBox.TabIndex = 1;
            this.localIpAddressTextBox.Text = "0.0.0.0";
            // 
            // startRelayButton
            // 
            this.startRelayButton.Location = new System.Drawing.Point(265, 13);
            this.startRelayButton.Name = "startRelayButton";
            this.startRelayButton.Size = new System.Drawing.Size(75, 23);
            this.startRelayButton.TabIndex = 7;
            this.startRelayButton.Text = "Start Relay";
            this.startRelayButton.UseVisualStyleBackColor = true;
            this.startRelayButton.Click += new System.EventHandler(this.startRelayButton_Click);
            // 
            // stopRelayButton
            // 
            this.stopRelayButton.Location = new System.Drawing.Point(265, 48);
            this.stopRelayButton.Name = "stopRelayButton";
            this.stopRelayButton.Size = new System.Drawing.Size(75, 23);
            this.stopRelayButton.TabIndex = 8;
            this.stopRelayButton.Text = "Stop Relay";
            this.stopRelayButton.UseVisualStyleBackColor = true;
            this.stopRelayButton.Click += new System.EventHandler(this.stopRelayButton_Click);
            // 
            // externalRelayTextBox
            // 
            this.externalRelayTextBox.Location = new System.Drawing.Point(13, 78);
            this.externalRelayTextBox.Multiline = true;
            this.externalRelayTextBox.Name = "externalRelayTextBox";
            this.externalRelayTextBox.ReadOnly = true;
            this.externalRelayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.externalRelayTextBox.Size = new System.Drawing.Size(327, 348);
            this.externalRelayTextBox.TabIndex = 9;
            // 
            // ExternalRelayDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 438);
            this.Controls.Add(this.externalRelayTextBox);
            this.Controls.Add(this.stopRelayButton);
            this.Controls.Add(this.startRelayButton);
            this.Controls.Add(this.settingsGroupBox);
            this.Name = "ExternalRelayDialog";
            this.Text = "IvBind External Relay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExternalRelayDialog_FormClosing);
            this.Load += new System.EventHandler(this.ExternalRelayDialog_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox settingsGroupBox;
        internal System.Windows.Forms.Label ipAddressLabel;
        internal System.Windows.Forms.MaskedTextBox localIpAddressTextBox;
        private System.Windows.Forms.Button startRelayButton;
        private System.Windows.Forms.Button stopRelayButton;
        private System.Windows.Forms.TextBox externalRelayTextBox;

    }
}

