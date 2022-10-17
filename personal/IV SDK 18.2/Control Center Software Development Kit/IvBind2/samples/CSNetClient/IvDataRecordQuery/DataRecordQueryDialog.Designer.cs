namespace IvDataRecordQuery
{
    partial class DataRecordQueryDialog
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
            this.serverAddressLabel = new System.Windows.Forms.Label();
            this.queryStopButton = new System.Windows.Forms.Button();
            this.queryStartButton = new System.Windows.Forms.Button();
            this.dataRecordsLabel = new System.Windows.Forms.Label();
            this.serverAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.queryGroupBox = new System.Windows.Forms.GroupBox();
            this.dataFilterLabel = new System.Windows.Forms.Label();
            this.newRecordLabel = new System.Windows.Forms.Label();
            this.dataFilterEdit = new System.Windows.Forms.TextBox();
            this.newRecordCheck = new System.Windows.Forms.CheckBox();
            this.sourceSelectLabel = new System.Windows.Forms.Label();
            this.sourceSelectBox = new System.Windows.Forms.CheckedListBox();
            this.maxTimeLabel = new System.Windows.Forms.Label();
            this.maxTimePicker = new System.Windows.Forms.DateTimePicker();
            this.minTimeLabel = new System.Windows.Forms.Label();
            this.minTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dataRecordsGridView = new System.Windows.Forms.DataGridView();
            this.recordId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.serverDisconnectButton = new System.Windows.Forms.Button();
            this.serverConnectButton = new System.Windows.Forms.Button();
            this.queryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRecordsGridView)).BeginInit();
            this.serverGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverAddressLabel
            // 
            this.serverAddressLabel.AutoSize = true;
            this.serverAddressLabel.Location = new System.Drawing.Point(6, 19);
            this.serverAddressLabel.Name = "serverAddressLabel";
            this.serverAddressLabel.Size = new System.Drawing.Size(48, 13);
            this.serverAddressLabel.TabIndex = 0;
            this.serverAddressLabel.Text = "&Address:";
            // 
            // queryStopButton
            // 
            this.queryStopButton.Location = new System.Drawing.Point(455, 218);
            this.queryStopButton.Name = "queryStopButton";
            this.queryStopButton.Size = new System.Drawing.Size(89, 29);
            this.queryStopButton.TabIndex = 11;
            this.queryStopButton.Text = "Stop";
            this.queryStopButton.UseVisualStyleBackColor = true;
            this.queryStopButton.Click += new System.EventHandler(this.queryStopButton_Click);
            // 
            // queryStartButton
            // 
            this.queryStartButton.Location = new System.Drawing.Point(360, 218);
            this.queryStartButton.Name = "queryStartButton";
            this.queryStartButton.Size = new System.Drawing.Size(89, 29);
            this.queryStartButton.TabIndex = 10;
            this.queryStartButton.Text = "&Start";
            this.queryStartButton.UseVisualStyleBackColor = true;
            this.queryStartButton.Click += new System.EventHandler(this.queryStartButton_Click);
            // 
            // dataRecordsLabel
            // 
            this.dataRecordsLabel.AutoSize = true;
            this.dataRecordsLabel.Location = new System.Drawing.Point(8, 322);
            this.dataRecordsLabel.Name = "dataRecordsLabel";
            this.dataRecordsLabel.Size = new System.Drawing.Size(129, 13);
            this.dataRecordsLabel.TabIndex = 2;
            this.dataRecordsLabel.Text = "Data Records";
            // 
            // serverAddressTextBox
            // 
            this.serverAddressTextBox.AsciiOnly = true;
            this.serverAddressTextBox.Location = new System.Drawing.Point(149, 17);
            this.serverAddressTextBox.Name = "serverAddressTextBox";
            this.serverAddressTextBox.Size = new System.Drawing.Size(200, 20);
            this.serverAddressTextBox.TabIndex = 1;
            this.serverAddressTextBox.Text = "0.0.0.0";
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.Location = new System.Drawing.Point(11, 618);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(550, 66);
            this.consoleTextBox.TabIndex = 5;
            // 
            // queryGroupBox
            // 
            this.queryGroupBox.Controls.Add(this.dataFilterLabel);
            this.queryGroupBox.Controls.Add(this.newRecordLabel);
            this.queryGroupBox.Controls.Add(this.dataFilterEdit);
            this.queryGroupBox.Controls.Add(this.queryStopButton);
            this.queryGroupBox.Controls.Add(this.newRecordCheck);
            this.queryGroupBox.Controls.Add(this.queryStartButton);
            this.queryGroupBox.Controls.Add(this.sourceSelectLabel);
            this.queryGroupBox.Controls.Add(this.sourceSelectBox);
            this.queryGroupBox.Controls.Add(this.maxTimeLabel);
            this.queryGroupBox.Controls.Add(this.maxTimePicker);
            this.queryGroupBox.Controls.Add(this.minTimeLabel);
            this.queryGroupBox.Controls.Add(this.minTimePicker);
            this.queryGroupBox.Location = new System.Drawing.Point(11, 61);
            this.queryGroupBox.Name = "queryGroupBox";
            this.queryGroupBox.Size = new System.Drawing.Size(550, 253);
            this.queryGroupBox.TabIndex = 1;
            this.queryGroupBox.TabStop = false;
            this.queryGroupBox.Text = "Data Query";
            // 
            // dataFilterLabel
            // 
            this.dataFilterLabel.AutoSize = true;
            this.dataFilterLabel.Location = new System.Drawing.Point(6, 167);
            this.dataFilterLabel.Name = "dataFilterLabel";
            this.dataFilterLabel.Size = new System.Drawing.Size(58, 13);
            this.dataFilterLabel.TabIndex = 6;
            this.dataFilterLabel.Text = "Data Filter:";
            // 
            // newRecordLabel
            // 
            this.newRecordLabel.AutoSize = true;
            this.newRecordLabel.Location = new System.Drawing.Point(6, 193);
            this.newRecordLabel.Name = "newRecordLabel";
            this.newRecordLabel.Size = new System.Drawing.Size(134, 13);
            this.newRecordLabel.TabIndex = 8;
            this.newRecordLabel.Text = "Only Return New Records:";
            // 
            // dataFilterEdit
            // 
            this.dataFilterEdit.Location = new System.Drawing.Point(149, 167);
            this.dataFilterEdit.Name = "dataFilterEdit";
            this.dataFilterEdit.Size = new System.Drawing.Size(395, 20);
            this.dataFilterEdit.TabIndex = 7;
            // 
            // newRecordCheck
            // 
            this.newRecordCheck.AutoSize = true;
            this.newRecordCheck.Location = new System.Drawing.Point(149, 193);
            this.newRecordCheck.Name = "newRecordCheck";
            this.newRecordCheck.Size = new System.Drawing.Size(15, 14);
            this.newRecordCheck.TabIndex = 9;
            this.newRecordCheck.UseVisualStyleBackColor = true;
            // 
            // sourceSelectLabel
            // 
            this.sourceSelectLabel.AutoSize = true;
            this.sourceSelectLabel.Location = new System.Drawing.Point(6, 67);
            this.sourceSelectLabel.Name = "sourceSelectLabel";
            this.sourceSelectLabel.Size = new System.Drawing.Size(116, 13);
            this.sourceSelectLabel.TabIndex = 4;
            this.sourceSelectLabel.Text = "External Data Sources:";
            // 
            // sourceSelectBox
            // 
            this.sourceSelectBox.CheckOnClick = true;
            this.sourceSelectBox.FormattingEnabled = true;
            this.sourceSelectBox.Location = new System.Drawing.Point(149, 67);
            this.sourceSelectBox.Name = "sourceSelectBox";
            this.sourceSelectBox.Size = new System.Drawing.Size(395, 94);
            this.sourceSelectBox.TabIndex = 5;
            // 
            // maxTimeLabel
            // 
            this.maxTimeLabel.AutoSize = true;
            this.maxTimeLabel.Location = new System.Drawing.Point(6, 41);
            this.maxTimeLabel.Name = "maxTimeLabel";
            this.maxTimeLabel.Size = new System.Drawing.Size(122, 13);
            this.maxTimeLabel.TabIndex = 2;
            this.maxTimeLabel.Text = "Maximum Creation Time:";
            // 
            // maxTimePicker
            // 
            this.maxTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.maxTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.maxTimePicker.Location = new System.Drawing.Point(149, 41);
            this.maxTimePicker.Name = "maxTimePicker";
            this.maxTimePicker.ShowCheckBox = true;
            this.maxTimePicker.Size = new System.Drawing.Size(200, 20);
            this.maxTimePicker.TabIndex = 3;
            this.maxTimePicker.Value = new System.DateTime(2016, 1, 1, 0, 0, 0, 0);
            // 
            // minTimeLabel
            // 
            this.minTimeLabel.AutoSize = true;
            this.minTimeLabel.Location = new System.Drawing.Point(6, 16);
            this.minTimeLabel.Name = "minTimeLabel";
            this.minTimeLabel.Size = new System.Drawing.Size(119, 13);
            this.minTimeLabel.TabIndex = 0;
            this.minTimeLabel.Text = "Minimum Creation Time:";
            // 
            // minTimePicker
            // 
            this.minTimePicker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.minTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.minTimePicker.Location = new System.Drawing.Point(149, 16);
            this.minTimePicker.Name = "minTimePicker";
            this.minTimePicker.ShowCheckBox = true;
            this.minTimePicker.Size = new System.Drawing.Size(200, 20);
            this.minTimePicker.TabIndex = 1;
            this.minTimePicker.Value = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            // 
            // dataRecordsGridView
            // 
            this.dataRecordsGridView.AllowUserToAddRows = false;
            this.dataRecordsGridView.AllowUserToDeleteRows = false;
            this.dataRecordsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataRecordsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataRecordsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.recordId,
            this.sourceId,
            this.createTime,
            this.data});
            this.dataRecordsGridView.Location = new System.Drawing.Point(11, 338);
            this.dataRecordsGridView.Name = "dataRecordsGridView";
            this.dataRecordsGridView.ReadOnly = true;
            this.dataRecordsGridView.RowHeadersVisible = false;
            this.dataRecordsGridView.ShowEditingIcon = false;
            this.dataRecordsGridView.Size = new System.Drawing.Size(550, 260);
            this.dataRecordsGridView.TabIndex = 3;
            // 
            // recordId
            // 
            this.recordId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.recordId.FillWeight = 10F;
            this.recordId.HeaderText = "ID";
            this.recordId.Name = "recordId";
            this.recordId.ReadOnly = true;
            this.recordId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sourceId
            // 
            this.sourceId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceId.FillWeight = 40F;
            this.sourceId.HeaderText = "Source Name";
            this.sourceId.Name = "sourceId";
            this.sourceId.ReadOnly = true;
            this.sourceId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // createTime
            // 
            this.createTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.createTime.FillWeight = 40F;
            this.createTime.HeaderText = "Time Created";
            this.createTime.Name = "createTime";
            this.createTime.ReadOnly = true;
            this.createTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // data
            // 
            this.data.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.data.HeaderText = "Data";
            this.data.Name = "data";
            this.data.ReadOnly = true;
            this.data.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // consoleLabel
            // 
            this.consoleLabel.AutoSize = true;
            this.consoleLabel.Location = new System.Drawing.Point(8, 602);
            this.consoleLabel.Name = "consoleLabel";
            this.consoleLabel.Size = new System.Drawing.Size(45, 13);
            this.consoleLabel.TabIndex = 4;
            this.consoleLabel.Text = "Console";
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.serverDisconnectButton);
            this.serverGroupBox.Controls.Add(this.serverConnectButton);
            this.serverGroupBox.Controls.Add(this.serverAddressLabel);
            this.serverGroupBox.Controls.Add(this.serverAddressTextBox);
            this.serverGroupBox.Location = new System.Drawing.Point(11, 7);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(550, 48);
            this.serverGroupBox.TabIndex = 0;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "Alarm Server";
            // 
            // serverDisconnectButton
            // 
            this.serverDisconnectButton.Location = new System.Drawing.Point(455, 12);
            this.serverDisconnectButton.Name = "serverDisconnectButton";
            this.serverDisconnectButton.Size = new System.Drawing.Size(89, 29);
            this.serverDisconnectButton.TabIndex = 3;
            this.serverDisconnectButton.Text = "&Disconnect";
            this.serverDisconnectButton.UseVisualStyleBackColor = true;
            this.serverDisconnectButton.Click += new System.EventHandler(this.serverDisconnectButton_Click);
            // 
            // serverConnectButton
            // 
            this.serverConnectButton.Location = new System.Drawing.Point(360, 12);
            this.serverConnectButton.Name = "serverConnectButton";
            this.serverConnectButton.Size = new System.Drawing.Size(89, 29);
            this.serverConnectButton.TabIndex = 2;
            this.serverConnectButton.Text = "&Connect";
            this.serverConnectButton.UseVisualStyleBackColor = true;
            this.serverConnectButton.Click += new System.EventHandler(this.serverConnectButton_Click);
            // 
            // DataRecordQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 689);
            this.Controls.Add(this.serverGroupBox);
            this.Controls.Add(this.consoleLabel);
            this.Controls.Add(this.dataRecordsGridView);
            this.Controls.Add(this.dataRecordsLabel);
            this.Controls.Add(this.consoleTextBox);
            this.Controls.Add(this.queryGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataRecordQueryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Record Query Dialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataRecordQueryDialog_FormClosing);
            this.Load += new System.EventHandler(this.DataRecordQueryDialog_Load);
            this.queryGroupBox.ResumeLayout(false);
            this.queryGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRecordsGridView)).EndInit();
            this.serverGroupBox.ResumeLayout(false);
            this.serverGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label serverAddressLabel;
        internal System.Windows.Forms.Button queryStopButton;
        internal System.Windows.Forms.Button queryStartButton;
        internal System.Windows.Forms.Label dataRecordsLabel;
        internal System.Windows.Forms.MaskedTextBox serverAddressTextBox;
        internal System.Windows.Forms.TextBox consoleTextBox;
        internal System.Windows.Forms.GroupBox queryGroupBox;
        private System.Windows.Forms.DateTimePicker minTimePicker;
        private System.Windows.Forms.Label maxTimeLabel;
        private System.Windows.Forms.DateTimePicker maxTimePicker;
        private System.Windows.Forms.Label minTimeLabel;
        private System.Windows.Forms.DataGridView dataRecordsGridView;
        internal System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.Label dataFilterLabel;
        private System.Windows.Forms.Label newRecordLabel;
        private System.Windows.Forms.TextBox dataFilterEdit;
        private System.Windows.Forms.CheckBox newRecordCheck;
        private System.Windows.Forms.Label sourceSelectLabel;
        private System.Windows.Forms.CheckedListBox sourceSelectBox;
        private System.Windows.Forms.GroupBox serverGroupBox;
        internal System.Windows.Forms.Button serverDisconnectButton;
        internal System.Windows.Forms.Button serverConnectButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordId;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn data;
    }
}

