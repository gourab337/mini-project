namespace IvAddBookmark2
{
    partial class AddBookmark2Dialog
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
            this.addBookmark2Button = new System.Windows.Forms.Button();
            this.autoTimeStampCheckBox = new System.Windows.Forms.CheckBox();
            this.bookmark2TextBox = new System.Windows.Forms.TextBox();
            this.timeStampTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer( this.components );
            this.bookmark2TextLabel = new System.Windows.Forms.Label();
            this.camServiceIdTextBox = new System.Windows.Forms.TextBox();
            this.camServiceIdLabel = new System.Windows.Forms.Label();
            this.nvrIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.nvrServerLabel = new System.Windows.Forms.Label();
            this.timeStampLabel = new System.Windows.Forms.Label();
            this.securityLevelTextBox = new System.Windows.Forms.TextBox();
            this.securityLevelLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addBookmark2Button
            // 
            this.addBookmark2Button.Location = new System.Drawing.Point( 242, 203 );
            this.addBookmark2Button.Name = "addBookmark2Button";
            this.addBookmark2Button.Size = new System.Drawing.Size( 96, 26 );
            this.addBookmark2Button.TabIndex = 19;
            this.addBookmark2Button.Text = "Add Bookmark";
            this.addBookmark2Button.UseVisualStyleBackColor = true;
            this.addBookmark2Button.Click += new System.EventHandler( this.addBookmark2Button_Click );
            // 
            // autoTimeStampCheckBox
            // 
            this.autoTimeStampCheckBox.AutoSize = true;
            this.autoTimeStampCheckBox.Checked = true;
            this.autoTimeStampCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoTimeStampCheckBox.Location = new System.Drawing.Point( 71, 162 );
            this.autoTimeStampCheckBox.Name = "autoTimeStampCheckBox";
            this.autoTimeStampCheckBox.Size = new System.Drawing.Size( 48, 17 );
            this.autoTimeStampCheckBox.TabIndex = 18;
            this.autoTimeStampCheckBox.Text = "Aut&o";
            this.autoTimeStampCheckBox.UseVisualStyleBackColor = true;
            // 
            // bookmark2TextBox
            // 
            this.bookmark2TextBox.Location = new System.Drawing.Point( 135, 87 );
            this.bookmark2TextBox.Name = "bookmark2TextBox";
            this.bookmark2TextBox.Size = new System.Drawing.Size( 177, 20 );
            this.bookmark2TextBox.TabIndex = 15;
            this.bookmark2TextBox.Text = "Message Add";
            // 
            // timeStampTextBox
            // 
            this.timeStampTextBox.Location = new System.Drawing.Point( 135, 162 );
            this.timeStampTextBox.Name = "timeStampTextBox";
            this.timeStampTextBox.Size = new System.Drawing.Size( 177, 20 );
            this.timeStampTextBox.TabIndex = 17;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler( this.Timer1_Tick );
            // 
            // bookmark2TextLabel
            // 
            this.bookmark2TextLabel.AutoSize = true;
            this.bookmark2TextLabel.Location = new System.Drawing.Point( 44, 91 );
            this.bookmark2TextLabel.Name = "bookmark3TextLabel";
            this.bookmark2TextLabel.Size = new System.Drawing.Size( 78, 13 );
            this.bookmark2TextLabel.TabIndex = 14;
            this.bookmark2TextLabel.Text = "&Bookmark text:";
            // 
            // camServiceIdTextBox
            // 
            this.camServiceIdTextBox.Location = new System.Drawing.Point( 135, 52 );
            this.camServiceIdTextBox.Name = "camServiceIdTextBox";
            this.camServiceIdTextBox.Size = new System.Drawing.Size( 177, 20 );
            this.camServiceIdTextBox.TabIndex = 13;
            // 
            // camServiceIdLabel
            // 
            this.camServiceIdLabel.AutoSize = true;
            this.camServiceIdLabel.Location = new System.Drawing.Point( 22, 55 );
            this.camServiceIdLabel.Name = "camServiceIdLabel";
            this.camServiceIdLabel.Size = new System.Drawing.Size( 97, 13 );
            this.camServiceIdLabel.TabIndex = 12;
            this.camServiceIdLabel.Text = "&Camera Service Id:";
            // 
            // nvrIpAddressTextBox
            // 
            this.nvrIpAddressTextBox.Location = new System.Drawing.Point( 135, 17 );
            this.nvrIpAddressTextBox.Name = "nvrIpAddressTextBox";
            this.nvrIpAddressTextBox.Size = new System.Drawing.Size( 177, 20 );
            this.nvrIpAddressTextBox.TabIndex = 11;
            this.nvrIpAddressTextBox.Text = "0.0.0.0";
            // 
            // nvrServerLabel
            // 
            this.nvrServerLabel.AutoSize = true;
            this.nvrServerLabel.Location = new System.Drawing.Point( 35, 20 );
            this.nvrServerLabel.Name = "nvrServerLabel";
            this.nvrServerLabel.Size = new System.Drawing.Size( 87, 13 );
            this.nvrServerLabel.TabIndex = 10;
            this.nvrServerLabel.Text = "NVR IP &Address:";
            // 
            // timeStampLabel
            // 
            this.timeStampLabel.AutoSize = true;
            this.timeStampLabel.Location = new System.Drawing.Point( 59, 118 );
            this.timeStampLabel.Name = "timeStampLabel";
            this.timeStampLabel.Size = new System.Drawing.Size( 63, 13 );
            this.timeStampLabel.TabIndex = 16;
            this.timeStampLabel.Text = "&TimeStamp:";
            // 
            // securityLevelTextBox
            // 
            this.securityLevelTextBox.Location = new System.Drawing.Point( 135, 125 );
            this.securityLevelTextBox.Name = "securityLevelTextBox";
            this.securityLevelTextBox.Size = new System.Drawing.Size( 177, 20 );
            this.securityLevelTextBox.TabIndex = 17;
            this.securityLevelTextBox.Text = "1";
            // 
            // securityLevelLabel
            // 
            this.securityLevelLabel.AutoSize = true;
            this.securityLevelLabel.Location = new System.Drawing.Point( 42, 128 );
            this.securityLevelLabel.Name = "securityLevelLabel";
            this.securityLevelLabel.Size = new System.Drawing.Size( 77, 13 );
            this.securityLevelLabel.TabIndex = 16;
            this.securityLevelLabel.Text = "Security Level:";
            // 
            // AddBookmark3Dialog
            // 
            this.AcceptButton = this.addBookmark2Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 372, 254 );
            this.Controls.Add( this.addBookmark2Button );
            this.Controls.Add( this.securityLevelLabel );
            this.Controls.Add( this.securityLevelTextBox );
            this.Controls.Add( this.autoTimeStampCheckBox );
            this.Controls.Add( this.bookmark2TextBox );
            this.Controls.Add( this.timeStampTextBox );
            this.Controls.Add( this.bookmark2TextLabel );
            this.Controls.Add( this.camServiceIdTextBox );
            this.Controls.Add( this.camServiceIdLabel );
            this.Controls.Add( this.nvrIpAddressTextBox );
            this.Controls.Add( this.nvrServerLabel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBookmark3Dialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Bookmark Dialog";
            this.Load += new System.EventHandler( this.AddBookmarkDialog_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button addBookmark2Button;
        internal System.Windows.Forms.CheckBox autoTimeStampCheckBox;
        internal System.Windows.Forms.TextBox bookmark2TextBox;
        internal System.Windows.Forms.TextBox timeStampTextBox;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.Label bookmark2TextLabel;
        internal System.Windows.Forms.TextBox camServiceIdTextBox;
        internal System.Windows.Forms.Label camServiceIdLabel;
        internal System.Windows.Forms.TextBox nvrIpAddressTextBox;
        internal System.Windows.Forms.Label nvrServerLabel;
        internal System.Windows.Forms.TextBox securityLevelTextBox;
        internal System.Windows.Forms.Label securityLevelLabel;
        internal System.Windows.Forms.Label timeStampLabel;
    }
}

