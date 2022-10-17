namespace IvClearZone
{
    partial class ClearZoneDialog
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
            this.clearZoneButton = new System.Windows.Forms.Button();
            this.clearMessageTextBox = new System.Windows.Forms.TextBox();
            this.clearMessageLabel = new System.Windows.Forms.Label();
            this.zoneNameTextBox = new System.Windows.Forms.TextBox();
            this.zoneNameLabel = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.alarmServerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // clearZoneButton
            // 
            this.clearZoneButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.clearZoneButton.Location = new System.Drawing.Point(243, 135);
            this.clearZoneButton.Name = "clearZoneButton";
            this.clearZoneButton.Size = new System.Drawing.Size(86, 23);
            this.clearZoneButton.TabIndex = 10;
            this.clearZoneButton.Text = "&Clear";
            this.clearZoneButton.Click += new System.EventHandler(this.clearZoneButton_Click);
            // 
            // clearMessageTextBox
            // 
            this.clearMessageTextBox.Location = new System.Drawing.Point(156, 90);
            this.clearMessageTextBox.Name = "clearMessageTextBox";
            this.clearMessageTextBox.Size = new System.Drawing.Size(173, 20);
            this.clearMessageTextBox.TabIndex = 8;
            // 
            // clearMessageLabel
            // 
            this.clearMessageLabel.AutoSize = true;
            this.clearMessageLabel.Location = new System.Drawing.Point(69, 93);
            this.clearMessageLabel.Name = "clearMessageLabel";
            this.clearMessageLabel.Size = new System.Drawing.Size(80, 13);
            this.clearMessageLabel.TabIndex = 11;
            this.clearMessageLabel.Text = "Clear &Message:";
            // 
            // zoneNameTextBox
            // 
            this.zoneNameTextBox.Location = new System.Drawing.Point(156, 55);
            this.zoneNameTextBox.Name = "zoneNameTextBox";
            this.zoneNameTextBox.Size = new System.Drawing.Size(173, 20);
            this.zoneNameTextBox.TabIndex = 7;
            // 
            // zoneNameLabel
            // 
            this.zoneNameLabel.AutoSize = true;
            this.zoneNameLabel.Location = new System.Drawing.Point(83, 58);
            this.zoneNameLabel.Name = "zoneNameLabel";
            this.zoneNameLabel.Size = new System.Drawing.Size(66, 13);
            this.zoneNameLabel.TabIndex = 9;
            this.zoneNameLabel.Text = "Zone &Name:";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(156, 20);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(173, 20);
            this.ipAddressTextBox.TabIndex = 5;
            this.ipAddressTextBox.Text = "0.0.0.0";
            // 
            // alarmServerLabel
            // 
            this.alarmServerLabel.AutoSize = true;
            this.alarmServerLabel.Location = new System.Drawing.Point(25, 23);
            this.alarmServerLabel.Name = "alarmServerLabel";
            this.alarmServerLabel.Size = new System.Drawing.Size(124, 13);
            this.alarmServerLabel.TabIndex = 6;
            this.alarmServerLabel.Text = "Alarm Server IP &Address:";
            // 
            // ClearZoneDialog
            // 
            this.AcceptButton = this.clearZoneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 174);
            this.Controls.Add(this.clearZoneButton);
            this.Controls.Add(this.clearMessageTextBox);
            this.Controls.Add(this.clearMessageLabel);
            this.Controls.Add(this.zoneNameTextBox);
            this.Controls.Add(this.zoneNameLabel);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.alarmServerLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClearZoneDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clear Zone Dialog";
            this.Load += new System.EventHandler(this.ClearZoneDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button clearZoneButton;
        internal System.Windows.Forms.TextBox clearMessageTextBox;
        internal System.Windows.Forms.Label clearMessageLabel;
        internal System.Windows.Forms.TextBox zoneNameTextBox;
        internal System.Windows.Forms.Label zoneNameLabel;
        internal System.Windows.Forms.TextBox ipAddressTextBox;
        internal System.Windows.Forms.Label alarmServerLabel;
    }
}

