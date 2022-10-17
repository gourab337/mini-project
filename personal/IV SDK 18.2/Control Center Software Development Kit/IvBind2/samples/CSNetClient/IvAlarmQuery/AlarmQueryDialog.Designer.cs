namespace IvAlarmQuery
{
    partial class AlarmQueryDialog
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
            this.queryButton = new System.Windows.Forms.Button();
            this.alarmRecordsLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.parameterGroupBox = new System.Windows.Forms.GroupBox();
            this.maxAlarmRaisedTimeLabel = new System.Windows.Forms.Label();
            this.maxAlarmTimePicker = new System.Windows.Forms.DateTimePicker();
            this.minAlarmRaisedTimeLabel = new System.Windows.Forms.Label();
            this.minAlarmTimePicker = new System.Windows.Forms.DateTimePicker();
            this.alarmRecordsGridView = new System.Windows.Forms.DataGridView();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.activationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoneId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ownerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeRaised = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clearedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alarmRecordsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // alarmAddressLabel
            // 
            this.alarmAddressLabel.AutoSize = true;
            this.alarmAddressLabel.Location = new System.Drawing.Point(13, 21);
            this.alarmAddressLabel.Name = "alarmAddressLabel";
            this.alarmAddressLabel.Size = new System.Drawing.Size(111, 13);
            this.alarmAddressLabel.TabIndex = 0;
            this.alarmAddressLabel.Text = "&Alarm Server Address:";
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(561, 72);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(89, 29);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "&Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // queryButton
            // 
            this.queryButton.Location = new System.Drawing.Point(561, 25);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(89, 29);
            this.queryButton.TabIndex = 7;
            this.queryButton.Text = "&Query";
            this.queryButton.UseVisualStyleBackColor = true;
            this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
            // 
            // alarmRecordsLabel
            // 
            this.alarmRecordsLabel.AutoSize = true;
            this.alarmRecordsLabel.Location = new System.Drawing.Point(8, 127);
            this.alarmRecordsLabel.Name = "alarmRecordsLabel";
            this.alarmRecordsLabel.Size = new System.Drawing.Size(132, 13);
            this.alarmRecordsLabel.TabIndex = 8;
            this.alarmRecordsLabel.Text = "Alarm Records";
            // 
            // asIpAddressTextBox
            // 
            this.asIpAddressTextBox.AsciiOnly = true;
            this.asIpAddressTextBox.Location = new System.Drawing.Point(214, 18);
            this.asIpAddressTextBox.Name = "asIpAddressTextBox";
            this.asIpAddressTextBox.Size = new System.Drawing.Size(200, 20);
            this.asIpAddressTextBox.TabIndex = 1;
            this.asIpAddressTextBox.Text = "0.0.0.0";
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.Location = new System.Drawing.Point(11, 482);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(639, 66);
            this.consoleTextBox.TabIndex = 9;
            // 
            // parameterGroupBox
            // 
            this.parameterGroupBox.Controls.Add(this.maxAlarmRaisedTimeLabel);
            this.parameterGroupBox.Controls.Add(this.maxAlarmTimePicker);
            this.parameterGroupBox.Controls.Add(this.minAlarmRaisedTimeLabel);
            this.parameterGroupBox.Controls.Add(this.minAlarmTimePicker);
            this.parameterGroupBox.Controls.Add(this.alarmAddressLabel);
            this.parameterGroupBox.Controls.Add(this.asIpAddressTextBox);
            this.parameterGroupBox.Location = new System.Drawing.Point(11, 12);
            this.parameterGroupBox.Name = "parameterGroupBox";
            this.parameterGroupBox.Size = new System.Drawing.Size(420, 100);
            this.parameterGroupBox.TabIndex = 5;
            this.parameterGroupBox.TabStop = false;
            this.parameterGroupBox.Text = "Query Parameters";
            // 
            // maxAlarmRaisedTimeLabel
            // 
            this.maxAlarmRaisedTimeLabel.AutoSize = true;
            this.maxAlarmRaisedTimeLabel.Location = new System.Drawing.Point(13, 69);
            this.maxAlarmRaisedTimeLabel.Name = "maxAlarmRaisedTimeLabel";
            this.maxAlarmRaisedTimeLabel.Size = new System.Drawing.Size(145, 13);
            this.maxAlarmRaisedTimeLabel.TabIndex = 6;
            this.maxAlarmRaisedTimeLabel.Text = "Maximum Alarm Raised Time:";
            // 
            // maxAlarmTimePicker
            // 
            this.maxAlarmTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.maxAlarmTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.maxAlarmTimePicker.Location = new System.Drawing.Point(214, 69);
            this.maxAlarmTimePicker.Name = "maxAlarmTimePicker";
            this.maxAlarmTimePicker.ShowCheckBox = true;
            this.maxAlarmTimePicker.Size = new System.Drawing.Size(200, 20);
            this.maxAlarmTimePicker.TabIndex = 5;
            this.maxAlarmTimePicker.Value = new System.DateTime(2016, 1, 1, 0, 0, 0, 0);
            // 
            // minAlarmRaisedTimeLabel
            // 
            this.minAlarmRaisedTimeLabel.AutoSize = true;
            this.minAlarmRaisedTimeLabel.Location = new System.Drawing.Point(13, 44);
            this.minAlarmRaisedTimeLabel.Name = "minAlarmRaisedTimeLabel";
            this.minAlarmRaisedTimeLabel.Size = new System.Drawing.Size(142, 13);
            this.minAlarmRaisedTimeLabel.TabIndex = 4;
            this.minAlarmRaisedTimeLabel.Text = "Minimum Alarm Raised Time:";
            // 
            // minAlarmTimePicker
            // 
            this.minAlarmTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.minAlarmTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.minAlarmTimePicker.Location = new System.Drawing.Point(214, 44);
            this.minAlarmTimePicker.Name = "minAlarmTimePicker";
            this.minAlarmTimePicker.ShowCheckBox = true;
            this.minAlarmTimePicker.Size = new System.Drawing.Size(200, 20);
            this.minAlarmTimePicker.TabIndex = 3;
            this.minAlarmTimePicker.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // alarmRecordsGridView
            // 
            this.alarmRecordsGridView.AllowUserToAddRows = false;
            this.alarmRecordsGridView.AllowUserToDeleteRows = false;
            this.alarmRecordsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.alarmRecordsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.alarmRecordsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.activationID,
            this.zoneId,
            this.alarmState,
            this.ownerId,
            this.timeRaised,
            this.clearedTime});
            this.alarmRecordsGridView.Location = new System.Drawing.Point(11, 143);
            this.alarmRecordsGridView.Name = "alarmRecordsGridView";
            this.alarmRecordsGridView.ReadOnly = true;
            this.alarmRecordsGridView.ShowEditingIcon = false;
            this.alarmRecordsGridView.Size = new System.Drawing.Size(639, 320);
            this.alarmRecordsGridView.TabIndex = 12;
            // 
            // consoleLabel
            // 
            this.consoleLabel.AutoSize = true;
            this.consoleLabel.Location = new System.Drawing.Point(8, 466);
            this.consoleLabel.Name = "consoleLabel";
            this.consoleLabel.Size = new System.Drawing.Size(45, 13);
            this.consoleLabel.TabIndex = 13;
            this.consoleLabel.Text = "Console";
            // 
            // activationID
            // 
            this.activationID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.activationID.FillWeight = 50F;
            this.activationID.HeaderText = "ID";
            this.activationID.Name = "activationID";
            this.activationID.ReadOnly = true;
            this.activationID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // zoneId
            // 
            this.zoneId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.zoneId.FillWeight = 50F;
            this.zoneId.HeaderText = "Zone";
            this.zoneId.Name = "zoneId";
            this.zoneId.ReadOnly = true;
            this.zoneId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // alarmState
            // 
            this.alarmState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alarmState.HeaderText = "State";
            this.alarmState.Name = "alarmState";
            this.alarmState.ReadOnly = true;
            this.alarmState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ownerId
            // 
            this.ownerId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ownerId.FillWeight = 50F;
            this.ownerId.HeaderText = "Owner";
            this.ownerId.Name = "ownerId";
            this.ownerId.ReadOnly = true;
            this.ownerId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // timeRaised
            // 
            this.timeRaised.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.timeRaised.HeaderText = "Time Raised";
            this.timeRaised.Name = "timeRaised";
            this.timeRaised.ReadOnly = true;
            this.timeRaised.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clearedTime
            // 
            this.clearedTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clearedTime.HeaderText = "Time Cleared";
            this.clearedTime.Name = "clearedTime";
            this.clearedTime.ReadOnly = true;
            this.clearedTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AlarmQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 560);
            this.Controls.Add(this.consoleLabel);
            this.Controls.Add(this.alarmRecordsGridView);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.queryButton);
            this.Controls.Add(this.alarmRecordsLabel);
            this.Controls.Add(this.consoleTextBox);
            this.Controls.Add(this.parameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmQueryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm Query Dialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlarmQueryDialog_FormClosing);
            this.Load += new System.EventHandler(this.AlarmQueryDialog_Load);
            this.parameterGroupBox.ResumeLayout(false);
            this.parameterGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alarmRecordsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label alarmAddressLabel;
        internal System.Windows.Forms.Button disconnectButton;
        internal System.Windows.Forms.Button queryButton;
        internal System.Windows.Forms.Label alarmRecordsLabel;
        internal System.Windows.Forms.MaskedTextBox asIpAddressTextBox;
        internal System.Windows.Forms.TextBox consoleTextBox;
        internal System.Windows.Forms.GroupBox parameterGroupBox;
        private System.Windows.Forms.DateTimePicker minAlarmTimePicker;
        private System.Windows.Forms.Label maxAlarmRaisedTimeLabel;
        private System.Windows.Forms.DateTimePicker maxAlarmTimePicker;
        private System.Windows.Forms.Label minAlarmRaisedTimeLabel;
        private System.Windows.Forms.DataGridView alarmRecordsGridView;
        internal System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn activationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneId;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ownerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeRaised;
        private System.Windows.Forms.DataGridViewTextBoxColumn clearedTime;
    }
}

