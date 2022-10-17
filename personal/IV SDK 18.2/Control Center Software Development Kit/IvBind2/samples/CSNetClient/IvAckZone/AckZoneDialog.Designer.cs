namespace IvAckZone
{
    partial class AckZoneDialog
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
            this.ackZoneButton = new System.Windows.Forms.Button();
            this.ackMessageTextBox = new System.Windows.Forms.TextBox();
            this.ackMessageLabel = new System.Windows.Forms.Label();
            this.zoneNameTextBox = new System.Windows.Forms.TextBox();
            this.zoneNameLabel = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.alarmServerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ackZoneButton
            // 
            this.ackZoneButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ackZoneButton.Location = new System.Drawing.Point(225, 137);
            this.ackZoneButton.Name = "ackZoneButton";
            this.ackZoneButton.Size = new System.Drawing.Size(95, 24);
            this.ackZoneButton.TabIndex = 13;
            this.ackZoneButton.Text = "Acknowl&edge";
            this.ackZoneButton.Click += new System.EventHandler(this.ackZoneButton_Click);
            // 
            // ackMessageTextBox
            // 
            this.ackMessageTextBox.Location = new System.Drawing.Point(156, 100);
            this.ackMessageTextBox.Name = "ackMessageTextBox";
            this.ackMessageTextBox.Size = new System.Drawing.Size(164, 20);
            this.ackMessageTextBox.TabIndex = 12;
            // 
            // ackMessageLabel
            // 
            this.ackMessageLabel.AutoSize = true;
            this.ackMessageLabel.Location = new System.Drawing.Point(31, 103);
            this.ackMessageLabel.Name = "ackMessageLabel";
            this.ackMessageLabel.Size = new System.Drawing.Size(121, 13);
            this.ackMessageLabel.TabIndex = 11;
            this.ackMessageLabel.Text = "Acknowledge &Message:";
            // 
            // zoneNameTextBox
            // 
            this.zoneNameTextBox.Location = new System.Drawing.Point(156, 63);
            this.zoneNameTextBox.Name = "zoneNameTextBox";
            this.zoneNameTextBox.Size = new System.Drawing.Size(164, 20);
            this.zoneNameTextBox.TabIndex = 10;
            // 
            // zoneNameLabel
            // 
            this.zoneNameLabel.AutoSize = true;
            this.zoneNameLabel.Location = new System.Drawing.Point(86, 66);
            this.zoneNameLabel.Name = "zoneNameLabel";
            this.zoneNameLabel.Size = new System.Drawing.Size(66, 13);
            this.zoneNameLabel.TabIndex = 9;
            this.zoneNameLabel.Text = "Zone &Name:";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(156, 26);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(164, 20);
            this.ipAddressTextBox.TabIndex = 8;
            this.ipAddressTextBox.Text = "0.0.0.0";
            // 
            // alarmServerLabel
            // 
            this.alarmServerLabel.AutoSize = true;
            this.alarmServerLabel.Location = new System.Drawing.Point(28, 29);
            this.alarmServerLabel.Name = "alarmServerLabel";
            this.alarmServerLabel.Size = new System.Drawing.Size(124, 13);
            this.alarmServerLabel.TabIndex = 7;
            this.alarmServerLabel.Text = "Alarm Server IP &Address:";
            // 
            // AckZoneDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 179);
            this.Controls.Add(this.ackZoneButton);
            this.Controls.Add(this.ackMessageTextBox);
            this.Controls.Add(this.ackMessageLabel);
            this.Controls.Add(this.zoneNameTextBox);
            this.Controls.Add(this.zoneNameLabel);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.alarmServerLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AckZoneDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acknowledge Zone Dialog";
            this.Load += new System.EventHandler(this.AckZoneDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ackZoneButton;
        internal System.Windows.Forms.TextBox ackMessageTextBox;
        internal System.Windows.Forms.Label ackMessageLabel;
        internal System.Windows.Forms.TextBox zoneNameTextBox;
        internal System.Windows.Forms.Label zoneNameLabel;
        internal System.Windows.Forms.TextBox ipAddressTextBox;
        internal System.Windows.Forms.Label alarmServerLabel;

    }
}

