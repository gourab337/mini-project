namespace IvSetZone
{
    partial class SetZoneDialog
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
            this.setZoneButton = new System.Windows.Forms.Button();
            this.zoneNameTextBox = new System.Windows.Forms.TextBox();
            this.zoneNameLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.asIpAddressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // setZoneButton
            // 
            this.setZoneButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.setZoneButton.Location = new System.Drawing.Point(232, 110);
            this.setZoneButton.Name = "setZoneButton";
            this.setZoneButton.Size = new System.Drawing.Size(86, 25);
            this.setZoneButton.TabIndex = 9;
            this.setZoneButton.Text = "&Set";
            this.setZoneButton.Click += new System.EventHandler(this.setZoneButton_Click);
            // 
            // zoneNameTextBox
            // 
            this.zoneNameTextBox.Location = new System.Drawing.Point(154, 64);
            this.zoneNameTextBox.Name = "zoneNameTextBox";
            this.zoneNameTextBox.Size = new System.Drawing.Size(164, 20);
            this.zoneNameTextBox.TabIndex = 8;
            // 
            // zoneNameLabel
            // 
            this.zoneNameLabel.AutoSize = true;
            this.zoneNameLabel.Location = new System.Drawing.Point(86, 67);
            this.zoneNameLabel.Name = "zoneNameLabel";
            this.zoneNameLabel.Size = new System.Drawing.Size(66, 13);
            this.zoneNameLabel.TabIndex = 7;
            this.zoneNameLabel.Text = "Zone &Name:";
            // 
            // asIpAddressTextBox
            // 
            this.asIpAddressTextBox.Location = new System.Drawing.Point(154, 20);
            this.asIpAddressTextBox.Name = "asIpAddressTextBox";
            this.asIpAddressTextBox.Size = new System.Drawing.Size(164, 20);
            this.asIpAddressTextBox.TabIndex = 6;
            this.asIpAddressTextBox.Text = "0.0.0.0";
            // 
            // asIpAddressLabel
            // 
            this.asIpAddressLabel.AutoSize = true;
            this.asIpAddressLabel.Location = new System.Drawing.Point(23, 24);
            this.asIpAddressLabel.Name = "asIpAddressLabel";
            this.asIpAddressLabel.Size = new System.Drawing.Size(124, 13);
            this.asIpAddressLabel.TabIndex = 5;
            this.asIpAddressLabel.Text = "Alarm Server IP &Address:";
            // 
            // SetZoneDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 155);
            this.Controls.Add(this.setZoneButton);
            this.Controls.Add(this.zoneNameTextBox);
            this.Controls.Add(this.zoneNameLabel);
            this.Controls.Add(this.asIpAddressTextBox);
            this.Controls.Add(this.asIpAddressLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetZoneDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Zone Dialog";
            this.Load += new System.EventHandler(this.SetZoneDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button setZoneButton;
        internal System.Windows.Forms.TextBox zoneNameTextBox;
        internal System.Windows.Forms.Label zoneNameLabel;
        internal System.Windows.Forms.TextBox asIpAddressTextBox;
        internal System.Windows.Forms.Label asIpAddressLabel;

    }
}

