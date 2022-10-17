namespace IvSendEvent
{
    partial class SendEventDialog
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
            this.eventNumberSpinBox = new System.Windows.Forms.NumericUpDown();
            this.eventNumberLabel = new System.Windows.Forms.Label();
            this.nvrServerLabel = new System.Windows.Forms.Label();
            this.timeStampLabel = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.autoTimeStampCheckBox = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.localIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.localIPLabel = new System.Windows.Forms.Label();
            this.timeStampTextBox = new System.Windows.Forms.TextBox();
            this.sendEventButton = new System.Windows.Forms.Button();
            this.annotationTextBox = new System.Windows.Forms.TextBox();
            this.annotationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.eventNumberSpinBox)).BeginInit();
            this.SuspendLayout();
            // 
            // eventNumberSpinBox
            // 
            this.eventNumberSpinBox.Location = new System.Drawing.Point(142, 87);
            this.eventNumberSpinBox.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.eventNumberSpinBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eventNumberSpinBox.Name = "eventNumberSpinBox";
            this.eventNumberSpinBox.Size = new System.Drawing.Size(190, 20);
            this.eventNumberSpinBox.TabIndex = 5;
            this.eventNumberSpinBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.eventNumberSpinBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // eventNumberLabel
            // 
            this.eventNumberLabel.AutoSize = true;
            this.eventNumberLabel.Location = new System.Drawing.Point(57, 89);
            this.eventNumberLabel.Name = "eventNumberLabel";
            this.eventNumberLabel.Size = new System.Drawing.Size(78, 13);
            this.eventNumberLabel.TabIndex = 4;
            this.eventNumberLabel.Text = "Event &Number:";
            // 
            // nvrServerLabel
            // 
            this.nvrServerLabel.AutoSize = true;
            this.nvrServerLabel.Location = new System.Drawing.Point(12, 18);
            this.nvrServerLabel.Name = "nvrServerLabel";
            this.nvrServerLabel.Size = new System.Drawing.Size(124, 13);
            this.nvrServerLabel.TabIndex = 0;
            this.nvrServerLabel.Text = "Alarm Server IP &Address:";
            // 
            // timeStampLabel
            // 
            this.timeStampLabel.AutoSize = true;
            this.timeStampLabel.Location = new System.Drawing.Point(74, 120);
            this.timeStampLabel.Name = "timeStampLabel";
            this.timeStampLabel.Size = new System.Drawing.Size(61, 13);
            this.timeStampLabel.TabIndex = 6;
            this.timeStampLabel.Text = "Timestamp:";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(142, 15);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(190, 20);
            this.ipAddressTextBox.TabIndex = 1;
            this.ipAddressTextBox.Text = "0.0.0.0";
            // 
            // autoTimeStampCheckBox
            // 
            this.autoTimeStampCheckBox.AutoSize = true;
            this.autoTimeStampCheckBox.Checked = true;
            this.autoTimeStampCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoTimeStampCheckBox.Location = new System.Drawing.Point(87, 140);
            this.autoTimeStampCheckBox.Name = "autoTimeStampCheckBox";
            this.autoTimeStampCheckBox.Size = new System.Drawing.Size(48, 17);
            this.autoTimeStampCheckBox.TabIndex = 7;
            this.autoTimeStampCheckBox.Text = "Aut&o";
            this.autoTimeStampCheckBox.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // localIpAddressTextBox
            // 
            this.localIpAddressTextBox.Location = new System.Drawing.Point(142, 51);
            this.localIpAddressTextBox.Name = "localIpAddressTextBox";
            this.localIpAddressTextBox.Size = new System.Drawing.Size(190, 20);
            this.localIpAddressTextBox.TabIndex = 3;
            this.localIpAddressTextBox.Text = "0.0.0.0";
            // 
            // localIPLabel
            // 
            this.localIPLabel.AutoSize = true;
            this.localIPLabel.Location = new System.Drawing.Point(46, 54);
            this.localIPLabel.Name = "localIPLabel";
            this.localIPLabel.Size = new System.Drawing.Size(90, 13);
            this.localIPLabel.TabIndex = 2;
            this.localIPLabel.Text = "&Local IP Address:";
            // 
            // timeStampTextBox
            // 
            this.timeStampTextBox.Location = new System.Drawing.Point(142, 138);
            this.timeStampTextBox.Name = "timeStampTextBox";
            this.timeStampTextBox.Size = new System.Drawing.Size(190, 20);
            this.timeStampTextBox.TabIndex = 8;
            this.timeStampTextBox.WordWrap = false;
            // 
            // sendEventButton
            // 
            this.sendEventButton.Location = new System.Drawing.Point(251, 300);
            this.sendEventButton.Name = "sendEventButton";
            this.sendEventButton.Size = new System.Drawing.Size(81, 25);
            this.sendEventButton.TabIndex = 11;
            this.sendEventButton.Text = "&Send";
            this.sendEventButton.UseVisualStyleBackColor = true;
            this.sendEventButton.Click += new System.EventHandler(this.sendEventButton_Click);
            // 
            // annotationTextBox
            // 
            this.annotationTextBox.Location = new System.Drawing.Point(142, 174);
            this.annotationTextBox.MaxLength = 256;
            this.annotationTextBox.Multiline = true;
            this.annotationTextBox.Name = "annotationTextBox";
            this.annotationTextBox.Size = new System.Drawing.Size(190, 104);
            this.annotationTextBox.TabIndex = 10;
            // 
            // annotationLabel
            // 
            this.annotationLabel.AutoSize = true;
            this.annotationLabel.Location = new System.Drawing.Point(75, 177);
            this.annotationLabel.Name = "annotationLabel";
            this.annotationLabel.Size = new System.Drawing.Size(61, 13);
            this.annotationLabel.TabIndex = 9;
            this.annotationLabel.Text = "Annotation:";
            // 
            // SendEventDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 337);
            this.Controls.Add(this.annotationLabel);
            this.Controls.Add(this.annotationTextBox);
            this.Controls.Add(this.eventNumberSpinBox);
            this.Controls.Add(this.eventNumberLabel);
            this.Controls.Add(this.nvrServerLabel);
            this.Controls.Add(this.timeStampLabel);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.autoTimeStampCheckBox);
            this.Controls.Add(this.localIpAddressTextBox);
            this.Controls.Add(this.localIPLabel);
            this.Controls.Add(this.timeStampTextBox);
            this.Controls.Add(this.sendEventButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendEventDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Event Dialog";
            this.Load += new System.EventHandler(this.SendEventDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eventNumberSpinBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.NumericUpDown eventNumberSpinBox;
        internal System.Windows.Forms.Label eventNumberLabel;
        internal System.Windows.Forms.Label nvrServerLabel;
        internal System.Windows.Forms.Label timeStampLabel;
        internal System.Windows.Forms.TextBox ipAddressTextBox;
        internal System.Windows.Forms.CheckBox autoTimeStampCheckBox;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.TextBox localIpAddressTextBox;
        internal System.Windows.Forms.Label localIPLabel;
        internal System.Windows.Forms.TextBox timeStampTextBox;
        internal System.Windows.Forms.Button sendEventButton;
        internal System.Windows.Forms.TextBox annotationTextBox;
        internal System.Windows.Forms.Label annotationLabel;
    }
}

