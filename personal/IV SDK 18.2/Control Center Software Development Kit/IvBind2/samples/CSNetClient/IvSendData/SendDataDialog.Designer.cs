namespace IvSendData
{
    partial class SendDataDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sendDataButton = new System.Windows.Forms.Button();
            this.autoTimeStampCheckBox = new System.Windows.Forms.CheckBox();
            this.sendDataTextBox = new System.Windows.Forms.TextBox();
            this.timeStampTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataTextLabel = new System.Windows.Forms.Label();
            this.extSystemTextBox = new System.Windows.Forms.TextBox();
            this.extSystemLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.asLabel = new System.Windows.Forms.Label();
            this.timeStampLabel = new System.Windows.Forms.Label();
            this.sourceNumberTextBox = new System.Windows.Forms.TextBox();
            this.sourceNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sendDataButton
            // 
            this.sendDataButton.Location = new System.Drawing.Point(251, 273);
            this.sendDataButton.Name = "sendDataButton";
            this.sendDataButton.Size = new System.Drawing.Size(96, 26);
            this.sendDataButton.TabIndex = 19;
            this.sendDataButton.Text = "Send Data";
            this.sendDataButton.UseVisualStyleBackColor = true;
            this.sendDataButton.Click += new System.EventHandler(this.sendDataButton_Click);
            // 
            // autoTimeStampCheckBox
            // 
            this.autoTimeStampCheckBox.AutoSize = true;
            this.autoTimeStampCheckBox.Checked = true;
            this.autoTimeStampCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoTimeStampCheckBox.Location = new System.Drawing.Point(15, 128);
            this.autoTimeStampCheckBox.Name = "autoTimeStampCheckBox";
            this.autoTimeStampCheckBox.Size = new System.Drawing.Size(48, 17);
            this.autoTimeStampCheckBox.TabIndex = 18;
            this.autoTimeStampCheckBox.Text = "Aut&o";
            this.autoTimeStampCheckBox.UseVisualStyleBackColor = true;
            // 
            // sendDataTextBox
            // 
            this.sendDataTextBox.AcceptsReturn = true;
            this.sendDataTextBox.AcceptsTab = true;
            this.sendDataTextBox.Location = new System.Drawing.Point(170, 165);
            this.sendDataTextBox.Multiline = true;
            this.sendDataTextBox.Name = "sendDataTextBox";
            this.sendDataTextBox.Size = new System.Drawing.Size(177, 99);
            this.sendDataTextBox.TabIndex = 15;
            this.sendDataTextBox.Text = "Add Data";
            // 
            // timeStampTextBox
            // 
            this.timeStampTextBox.Location = new System.Drawing.Point(170, 128);
            this.timeStampTextBox.Name = "timeStampTextBox";
            this.timeStampTextBox.Size = new System.Drawing.Size(177, 20);
            this.timeStampTextBox.TabIndex = 17;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // dataTextLabel
            // 
            this.dataTextLabel.AutoSize = true;
            this.dataTextLabel.Location = new System.Drawing.Point(12, 168);
            this.dataTextLabel.Name = "dataTextLabel";
            this.dataTextLabel.Size = new System.Drawing.Size(33, 13);
            this.dataTextLabel.TabIndex = 14;
            this.dataTextLabel.Text = "&Data:";
            // 
            // extSystemTextBox
            // 
            this.extSystemTextBox.Location = new System.Drawing.Point(170, 54);
            this.extSystemTextBox.Name = "extSystemTextBox";
            this.extSystemTextBox.Size = new System.Drawing.Size(177, 20);
            this.extSystemTextBox.TabIndex = 13;
            // 
            // extSystemLabel
            // 
            this.extSystemLabel.AutoSize = true;
            this.extSystemLabel.Location = new System.Drawing.Point(12, 56);
            this.extSystemLabel.Name = "extSystemLabel";
            this.extSystemLabel.Size = new System.Drawing.Size(139, 13);
            this.extSystemLabel.TabIndex = 12;
            this.extSystemLabel.Text = "&External System IP Address:";
            // 
            // asIpAddressTextBox
            // 
            this.asIpAddressTextBox.Location = new System.Drawing.Point(170, 17);
            this.asIpAddressTextBox.Name = "asIpAddressTextBox";
            this.asIpAddressTextBox.Size = new System.Drawing.Size(177, 20);
            this.asIpAddressTextBox.TabIndex = 11;
            this.asIpAddressTextBox.Text = "0.0.0.0";
            // 
            // asLabel
            // 
            this.asLabel.AutoSize = true;
            this.asLabel.Location = new System.Drawing.Point(12, 20);
            this.asLabel.Name = "asLabel";
            this.asLabel.Size = new System.Drawing.Size(124, 13);
            this.asLabel.TabIndex = 10;
            this.asLabel.Text = "Alarm Server IP &Address:";
            // 
            // timeStampLabel
            // 
            this.timeStampLabel.AutoSize = true;
            this.timeStampLabel.Location = new System.Drawing.Point(59, 118);
            this.timeStampLabel.Name = "timeStampLabel";
            this.timeStampLabel.Size = new System.Drawing.Size(63, 13);
            this.timeStampLabel.TabIndex = 16;
            this.timeStampLabel.Text = "&TimeStamp:";
            // 
            // sourceNumberTextBox
            // 
            this.sourceNumberTextBox.Location = new System.Drawing.Point(170, 91);
            this.sourceNumberTextBox.Name = "sourceNumberTextBox";
            this.sourceNumberTextBox.Size = new System.Drawing.Size(177, 20);
            this.sourceNumberTextBox.TabIndex = 17;
            this.sourceNumberTextBox.Text = "1";
            // 
            // sourceNumberLabel
            // 
            this.sourceNumberLabel.AutoSize = true;
            this.sourceNumberLabel.Location = new System.Drawing.Point(12, 92);
            this.sourceNumberLabel.Name = "sourceNumberLabel";
            this.sourceNumberLabel.Size = new System.Drawing.Size(84, 13);
            this.sourceNumberLabel.TabIndex = 16;
            this.sourceNumberLabel.Text = "Source Number:";
            // 
            // SendDataDialog
            // 
            this.AcceptButton = this.sendDataButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 309);
            this.Controls.Add(this.sendDataButton);
            this.Controls.Add(this.sourceNumberLabel);
            this.Controls.Add(this.sourceNumberTextBox);
            this.Controls.Add(this.autoTimeStampCheckBox);
            this.Controls.Add(this.sendDataTextBox);
            this.Controls.Add(this.timeStampTextBox);
            this.Controls.Add(this.dataTextLabel);
            this.Controls.Add(this.extSystemTextBox);
            this.Controls.Add(this.extSystemLabel);
            this.Controls.Add(this.asIpAddressTextBox);
            this.Controls.Add(this.asLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendDataDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Data Dialog";
            this.Load += new System.EventHandler(this.SendDataDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button sendDataButton;
        internal System.Windows.Forms.CheckBox autoTimeStampCheckBox;
        internal System.Windows.Forms.TextBox sendDataTextBox;
        internal System.Windows.Forms.TextBox timeStampTextBox;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.Label dataTextLabel;
        internal System.Windows.Forms.TextBox extSystemTextBox;
        internal System.Windows.Forms.Label extSystemLabel;
        internal System.Windows.Forms.TextBox asIpAddressTextBox;
        internal System.Windows.Forms.Label asLabel;
        internal System.Windows.Forms.TextBox sourceNumberTextBox;
        internal System.Windows.Forms.Label sourceNumberLabel;
        internal System.Windows.Forms.Label timeStampLabel;
    }
}

