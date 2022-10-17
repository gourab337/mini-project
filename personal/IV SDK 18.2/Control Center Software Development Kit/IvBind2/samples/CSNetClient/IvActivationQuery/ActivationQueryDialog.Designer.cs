namespace IvActivationQuery
{
    partial class ActivationQueryDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.alarmAddressLabel = new System.Windows.Forms.Label();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.queryButton = new System.Windows.Forms.Button();
            this.activationRecordsLabel = new System.Windows.Forms.Label();
            this.asIpAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.parameterGroupBox = new System.Windows.Forms.GroupBox();
            this.maxActivationTimeLabel = new System.Windows.Forms.Label();
            this.maxActivationTimePicker = new System.Windows.Forms.DateTimePicker();
            this.minActivationTimeLabel = new System.Windows.Forms.Label();
            this.minActivationTimePicker = new System.Windows.Forms.DateTimePicker();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.activationRecordsGridView = new System.Windows.Forms.DataGridView();
            this.activationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detectorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmRecordId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoneId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extraInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activationRecordsGridView)).BeginInit();
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
            this.disconnectButton.Location = new System.Drawing.Point(561, 65);
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
            // activationRecordsLabel
            // 
            this.activationRecordsLabel.AutoSize = true;
            this.activationRecordsLabel.Location = new System.Drawing.Point(8, 126);
            this.activationRecordsLabel.Name = "activationRecordsLabel";
            this.activationRecordsLabel.Size = new System.Drawing.Size(153, 13);
            this.activationRecordsLabel.TabIndex = 8;
            this.activationRecordsLabel.Text = "Activation Records";
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
            this.consoleTextBox.Location = new System.Drawing.Point(11, 464);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(639, 79);
            this.consoleTextBox.TabIndex = 9;
            // 
            // parameterGroupBox
            // 
            this.parameterGroupBox.Controls.Add(this.maxActivationTimeLabel);
            this.parameterGroupBox.Controls.Add(this.maxActivationTimePicker);
            this.parameterGroupBox.Controls.Add(this.minActivationTimeLabel);
            this.parameterGroupBox.Controls.Add(this.minActivationTimePicker);
            this.parameterGroupBox.Controls.Add(this.alarmAddressLabel);
            this.parameterGroupBox.Controls.Add(this.asIpAddressTextBox);
            this.parameterGroupBox.Location = new System.Drawing.Point(11, 12);
            this.parameterGroupBox.Name = "parameterGroupBox";
            this.parameterGroupBox.Size = new System.Drawing.Size(420, 100);
            this.parameterGroupBox.TabIndex = 5;
            this.parameterGroupBox.TabStop = false;
            this.parameterGroupBox.Text = "Query Parameters";
            // 
            // maxActivationTimeLabel
            // 
            this.maxActivationTimeLabel.AutoSize = true;
            this.maxActivationTimeLabel.Location = new System.Drawing.Point(13, 69);
            this.maxActivationTimeLabel.Name = "maxActivationTimeLabel";
            this.maxActivationTimeLabel.Size = new System.Drawing.Size(130, 13);
            this.maxActivationTimeLabel.TabIndex = 6;
            this.maxActivationTimeLabel.Text = "Maximum Activation Time:";
            // 
            // maxActivationTimePicker
            // 
            this.maxActivationTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.maxActivationTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.maxActivationTimePicker.Location = new System.Drawing.Point(214, 69);
            this.maxActivationTimePicker.Name = "maxActivationTimePicker";
            this.maxActivationTimePicker.ShowCheckBox = true;
            this.maxActivationTimePicker.Size = new System.Drawing.Size(200, 20);
            this.maxActivationTimePicker.TabIndex = 5;
            this.maxActivationTimePicker.Value = new System.DateTime(2016, 1, 1, 0, 0, 0, 0);
            // 
            // minActivationTimeLabel
            // 
            this.minActivationTimeLabel.AutoSize = true;
            this.minActivationTimeLabel.Location = new System.Drawing.Point(13, 44);
            this.minActivationTimeLabel.Name = "minActivationTimeLabel";
            this.minActivationTimeLabel.Size = new System.Drawing.Size(127, 13);
            this.minActivationTimeLabel.TabIndex = 4;
            this.minActivationTimeLabel.Text = "Minimum Activation Time:";
            // 
            // minActivationTimePicker
            // 
            this.minActivationTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.minActivationTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.minActivationTimePicker.Location = new System.Drawing.Point(214, 44);
            this.minActivationTimePicker.Name = "minActivationTimePicker";
            this.minActivationTimePicker.ShowCheckBox = true;
            this.minActivationTimePicker.Size = new System.Drawing.Size(200, 20);
            this.minActivationTimePicker.TabIndex = 3;
            this.minActivationTimePicker.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // consoleLabel
            // 
            this.consoleLabel.AutoSize = true;
            this.consoleLabel.Location = new System.Drawing.Point(8, 448);
            this.consoleLabel.Name = "consoleLabel";
            this.consoleLabel.Size = new System.Drawing.Size(45, 13);
            this.consoleLabel.TabIndex = 10;
            this.consoleLabel.Text = "Console";
            // 
            // activationRecordsGridView
            // 
            this.activationRecordsGridView.AllowUserToAddRows = false;
            this.activationRecordsGridView.AllowUserToDeleteRows = false;
            this.activationRecordsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.activationRecordsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.activationRecordsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.activationID,
            this.detectorId,
            this.activationTime,
            this.alarmRecordId,
            this.zoneId,
            this.alarmTime,
            this.extraInfo});
            this.activationRecordsGridView.Location = new System.Drawing.Point(11, 142);
            this.activationRecordsGridView.Name = "activationRecordsGridView";
            this.activationRecordsGridView.ReadOnly = true;
            this.activationRecordsGridView.ShowEditingIcon = false;
            this.activationRecordsGridView.Size = new System.Drawing.Size(639, 303);
            this.activationRecordsGridView.TabIndex = 11;
            this.activationRecordsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // activationID
            // 
            this.activationID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.activationID.HeaderText = "ID";
            this.activationID.Name = "activationID";
            this.activationID.ReadOnly = true;
            this.activationID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // detectorId
            // 
            this.detectorId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.detectorId.HeaderText = "Detector";
            this.detectorId.Name = "detectorId";
            this.detectorId.ReadOnly = true;
            this.detectorId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // activationTime
            // 
            this.activationTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.activationTime.HeaderText = "Activation Time";
            this.activationTime.Name = "activationTime";
            this.activationTime.ReadOnly = true;
            this.activationTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // alarmRecordId
            // 
            this.alarmRecordId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alarmRecordId.HeaderText = "Alarm Record";
            this.alarmRecordId.Name = "alarmRecordId";
            this.alarmRecordId.ReadOnly = true;
            this.alarmRecordId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // zoneId
            // 
            this.zoneId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.zoneId.HeaderText = "Zone";
            this.zoneId.Name = "zoneId";
            this.zoneId.ReadOnly = true;
            this.zoneId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // alarmTime
            // 
            this.alarmTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alarmTime.HeaderText = "Alarm Time";
            this.alarmTime.Name = "alarmTime";
            this.alarmTime.ReadOnly = true;
            this.alarmTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // extraInfo
            // 
            this.extraInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.extraInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.extraInfo.HeaderText = "Extra Info";
            this.extraInfo.Name = "extraInfo";
            this.extraInfo.ReadOnly = true;
            this.extraInfo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ActivationQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 555);
            this.Controls.Add(this.activationRecordsGridView);
            this.Controls.Add(this.consoleLabel);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.queryButton);
            this.Controls.Add(this.activationRecordsLabel);
            this.Controls.Add(this.consoleTextBox);
            this.Controls.Add(this.parameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationQueryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activation Query Dialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActivationQueryDialog_FormClosing);
            this.Load += new System.EventHandler(this.ActivationQueryDialog_Load);
            this.parameterGroupBox.ResumeLayout(false);
            this.parameterGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activationRecordsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label alarmAddressLabel;
        internal System.Windows.Forms.Button disconnectButton;
        internal System.Windows.Forms.Button queryButton;
        internal System.Windows.Forms.Label activationRecordsLabel;
        internal System.Windows.Forms.MaskedTextBox asIpAddressTextBox;
        internal System.Windows.Forms.TextBox consoleTextBox;
        internal System.Windows.Forms.GroupBox parameterGroupBox;
        private System.Windows.Forms.DateTimePicker minActivationTimePicker;
        private System.Windows.Forms.Label maxActivationTimeLabel;
        private System.Windows.Forms.DateTimePicker maxActivationTimePicker;
        private System.Windows.Forms.Label minActivationTimeLabel;
        internal System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.DataGridView activationRecordsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn activationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn detectorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn activationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmRecordId;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneId;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn extraInfo;
    }
}

