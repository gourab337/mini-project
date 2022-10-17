namespace IvZoneMonitor
{
    partial class ZoneMonitorDialog
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
            this.alarmAddressLabel = new System.Windows.Forms.Label();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.monitorButton = new System.Windows.Forms.Button();
            this.zoneNotificationsLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.notificationsTextBox = new System.Windows.Forms.TextBox();
            this.nvrGroupBox = new System.Windows.Forms.GroupBox();
            this.dnaProxyGroupBox = new System.Windows.Forms.GroupBox();
            this.dnaProxyPortInput = new System.Windows.Forms.NumericUpDown();
            this.dnaProxyPortLabel = new System.Windows.Forms.Label();
            this.dnaProxyIPInput = new System.Windows.Forms.MaskedTextBox();
            this.dnaProxyIpAddressLabel = new System.Windows.Forms.Label();
            this.nvrGroupBox.SuspendLayout();
            this.dnaProxyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dnaProxyPortInput)).BeginInit();
            this.SuspendLayout();
            // 
            // alarmAddressLabel
            // 
            this.alarmAddressLabel.AutoSize = true;
            this.alarmAddressLabel.Location = new System.Drawing.Point(13, 21);
            this.alarmAddressLabel.Name = "alarmAddressLabel";
            this.alarmAddressLabel.Size = new System.Drawing.Size(61, 13);
            this.alarmAddressLabel.TabIndex = 0;
            this.alarmAddressLabel.Text = "IP &Address:";
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(14, 82);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(89, 29);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "&Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // monitorButton
            // 
            this.monitorButton.Location = new System.Drawing.Point(109, 82);
            this.monitorButton.Name = "monitorButton";
            this.monitorButton.Size = new System.Drawing.Size(84, 29);
            this.monitorButton.TabIndex = 7;
            this.monitorButton.Text = "&Monitor";
            this.monitorButton.UseVisualStyleBackColor = true;
            this.monitorButton.Click += new System.EventHandler(this.monitorButton_Click);
            // 
            // zoneNotificationsLabel
            // 
            this.zoneNotificationsLabel.AutoSize = true;
            this.zoneNotificationsLabel.Location = new System.Drawing.Point(12, 121);
            this.zoneNotificationsLabel.Name = "zoneNotificationsLabel";
            this.zoneNotificationsLabel.Size = new System.Drawing.Size(93, 13);
            this.zoneNotificationsLabel.TabIndex = 8;
            this.zoneNotificationsLabel.Text = "Zone Notifications";
            // 
            // asIpAddressTextBox
            // 
            this.asIpAddressTextBox.AsciiOnly = true;
            this.asIpAddressTextBox.Location = new System.Drawing.Point(82, 18);
            this.asIpAddressTextBox.Name = "asIpAddressTextBox";
            this.asIpAddressTextBox.Size = new System.Drawing.Size(137, 20);
            this.asIpAddressTextBox.TabIndex = 1;
            this.asIpAddressTextBox.Text = "0.0.0.0";
            // 
            // notificationsTextBox
            // 
            this.notificationsTextBox.Location = new System.Drawing.Point(10, 137);
            this.notificationsTextBox.Multiline = true;
            this.notificationsTextBox.Name = "notificationsTextBox";
            this.notificationsTextBox.ReadOnly = true;
            this.notificationsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notificationsTextBox.Size = new System.Drawing.Size(478, 368);
            this.notificationsTextBox.TabIndex = 9;
            // 
            // nvrGroupBox
            // 
            this.nvrGroupBox.Controls.Add(this.alarmAddressLabel);
            this.nvrGroupBox.Controls.Add(this.asIpAddressTextBox);
            this.nvrGroupBox.Location = new System.Drawing.Point(12, 12);
            this.nvrGroupBox.Name = "nvrGroupBox";
            this.nvrGroupBox.Size = new System.Drawing.Size(236, 49);
            this.nvrGroupBox.TabIndex = 5;
            this.nvrGroupBox.TabStop = false;
            this.nvrGroupBox.Text = "Alarm Server (NVR-AS)";
            // 
            // dnaProxyGroupBox
            // 
            this.dnaProxyGroupBox.Controls.Add(this.dnaProxyPortInput);
            this.dnaProxyGroupBox.Controls.Add(this.dnaProxyPortLabel);
            this.dnaProxyGroupBox.Controls.Add(this.dnaProxyIPInput);
            this.dnaProxyGroupBox.Controls.Add(this.dnaProxyIpAddressLabel);
            this.dnaProxyGroupBox.Location = new System.Drawing.Point(254, 12);
            this.dnaProxyGroupBox.Name = "dnaProxyGroupBox";
            this.dnaProxyGroupBox.Size = new System.Drawing.Size(236, 83);
            this.dnaProxyGroupBox.TabIndex = 10;
            this.dnaProxyGroupBox.TabStop = false;
            this.dnaProxyGroupBox.Text = "DNA Proxy (Optional)";
            // 
            // dnaProxyPortInput
            // 
            this.dnaProxyPortInput.Location = new System.Drawing.Point(82, 48);
            this.dnaProxyPortInput.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.dnaProxyPortInput.Name = "dnaProxyPortInput";
            this.dnaProxyPortInput.Size = new System.Drawing.Size(137, 20);
            this.dnaProxyPortInput.TabIndex = 4;
            this.dnaProxyPortInput.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // dnaProxyPortLabel
            // 
            this.dnaProxyPortLabel.AutoSize = true;
            this.dnaProxyPortLabel.Location = new System.Drawing.Point(13, 50);
            this.dnaProxyPortLabel.Name = "dnaProxyPortLabel";
            this.dnaProxyPortLabel.Size = new System.Drawing.Size(29, 13);
            this.dnaProxyPortLabel.TabIndex = 2;
            this.dnaProxyPortLabel.Text = "Port:";
            // 
            // dnaProxyIPInput
            // 
            this.dnaProxyIPInput.Location = new System.Drawing.Point(82, 19);
            this.dnaProxyIPInput.Name = "dnaProxyIPInput";
            this.dnaProxyIPInput.Size = new System.Drawing.Size(137, 20);
            this.dnaProxyIPInput.TabIndex = 1;
            this.dnaProxyIPInput.TextChanged += new System.EventHandler(this.dnaProxyIPInput_TextChanged);
            // 
            // dnaProxyIpAddressLabel
            // 
            this.dnaProxyIpAddressLabel.AutoSize = true;
            this.dnaProxyIpAddressLabel.Location = new System.Drawing.Point(15, 22);
            this.dnaProxyIpAddressLabel.Name = "dnaProxyIpAddressLabel";
            this.dnaProxyIpAddressLabel.Size = new System.Drawing.Size(61, 13);
            this.dnaProxyIpAddressLabel.TabIndex = 0;
            this.dnaProxyIpAddressLabel.Text = "IP Address:";
            // 
            // ZoneMonitorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 518);
            this.Controls.Add(this.dnaProxyGroupBox);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.monitorButton);
            this.Controls.Add(this.zoneNotificationsLabel);
            this.Controls.Add(this.notificationsTextBox);
            this.Controls.Add(this.nvrGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZoneMonitorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zone Monitor Dialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ZoneMonitorDialog_FormClosing);
            this.Load += new System.EventHandler(this.ZoneMonitorDialog_Load);
            this.nvrGroupBox.ResumeLayout(false);
            this.nvrGroupBox.PerformLayout();
            this.dnaProxyGroupBox.ResumeLayout(false);
            this.dnaProxyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dnaProxyPortInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label alarmAddressLabel;
        internal System.Windows.Forms.Button disconnectButton;
        internal System.Windows.Forms.Button monitorButton;
        internal System.Windows.Forms.Label zoneNotificationsLabel;
        internal System.Windows.Forms.MaskedTextBox asIpAddressTextBox;
        internal System.Windows.Forms.TextBox notificationsTextBox;
        internal System.Windows.Forms.GroupBox nvrGroupBox;
        private System.Windows.Forms.GroupBox dnaProxyGroupBox;
        private System.Windows.Forms.Label dnaProxyPortLabel;
        private System.Windows.Forms.MaskedTextBox dnaProxyIPInput;
        private System.Windows.Forms.Label dnaProxyIpAddressLabel;
        private System.Windows.Forms.NumericUpDown dnaProxyPortInput;
    }
}

