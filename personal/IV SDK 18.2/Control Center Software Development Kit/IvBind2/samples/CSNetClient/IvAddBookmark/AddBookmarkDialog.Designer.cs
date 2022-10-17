namespace IvAddBookmark
{
    partial class AddBookmarkDialog
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
            this.addBookmarkButton = new System.Windows.Forms.Button();
            this.autoTimeStampCheckBox = new System.Windows.Forms.CheckBox();
            this.bookmarkTextBox = new System.Windows.Forms.TextBox();
            this.timeStampTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bookmarkTextLabel = new System.Windows.Forms.Label();
            this.camIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.camIPAddressLabel = new System.Windows.Forms.Label();
            this.nvrIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.nvrServerLabel = new System.Windows.Forms.Label();
            this.timeStampLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addBookmarkButton
            // 
            this.addBookmarkButton.Location = new System.Drawing.Point(216, 174);
            this.addBookmarkButton.Name = "addBookmarkButton";
            this.addBookmarkButton.Size = new System.Drawing.Size(96, 26);
            this.addBookmarkButton.TabIndex = 19;
            this.addBookmarkButton.Text = "Add Boo&kmark";
            this.addBookmarkButton.UseVisualStyleBackColor = true;
            this.addBookmarkButton.Click += new System.EventHandler(this.addBookmarkButton_Click);
            // 
            // autoTimeStampCheckBox
            // 
            this.autoTimeStampCheckBox.AutoSize = true;
            this.autoTimeStampCheckBox.Checked = true;
            this.autoTimeStampCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoTimeStampCheckBox.Location = new System.Drawing.Point(74, 134);
            this.autoTimeStampCheckBox.Name = "autoTimeStampCheckBox";
            this.autoTimeStampCheckBox.Size = new System.Drawing.Size(48, 17);
            this.autoTimeStampCheckBox.TabIndex = 18;
            this.autoTimeStampCheckBox.Text = "Aut&o";
            this.autoTimeStampCheckBox.UseVisualStyleBackColor = true;
            // 
            // bookmarkTextBox
            // 
            this.bookmarkTextBox.Location = new System.Drawing.Point(135, 87);
            this.bookmarkTextBox.Name = "bookmarkTextBox";
            this.bookmarkTextBox.Size = new System.Drawing.Size(177, 20);
            this.bookmarkTextBox.TabIndex = 15;
            this.bookmarkTextBox.Text = "Message Add";
            // 
            // timeStampTextBox
            // 
            this.timeStampTextBox.Location = new System.Drawing.Point(135, 132);
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
            // bookmarkTextLabel
            // 
            this.bookmarkTextLabel.AutoSize = true;
            this.bookmarkTextLabel.Location = new System.Drawing.Point(44, 91);
            this.bookmarkTextLabel.Name = "bookmarkTextLabel";
            this.bookmarkTextLabel.Size = new System.Drawing.Size(78, 13);
            this.bookmarkTextLabel.TabIndex = 14;
            this.bookmarkTextLabel.Text = "&Bookmark text:";
            // 
            // camIpAddressTextBox
            // 
            this.camIpAddressTextBox.Location = new System.Drawing.Point(135, 52);
            this.camIpAddressTextBox.Name = "camIpAddressTextBox";
            this.camIpAddressTextBox.Size = new System.Drawing.Size(177, 20);
            this.camIpAddressTextBox.TabIndex = 13;
            this.camIpAddressTextBox.Text = "0.0.0.0";
            // 
            // camIPAddressLabel
            // 
            this.camIPAddressLabel.AutoSize = true;
            this.camIPAddressLabel.Location = new System.Drawing.Point(22, 55);
            this.camIPAddressLabel.Name = "camIPAddressLabel";
            this.camIPAddressLabel.Size = new System.Drawing.Size(100, 13);
            this.camIPAddressLabel.TabIndex = 12;
            this.camIPAddressLabel.Text = "&Camera IP Address:";
            // 
            // nvrIpAddressTextBox
            // 
            this.nvrIpAddressTextBox.Location = new System.Drawing.Point(135, 17);
            this.nvrIpAddressTextBox.Name = "nvrIpAddressTextBox";
            this.nvrIpAddressTextBox.Size = new System.Drawing.Size(177, 20);
            this.nvrIpAddressTextBox.TabIndex = 11;
            this.nvrIpAddressTextBox.Text = "0.0.0.0";
            // 
            // nvrServerLabel
            // 
            this.nvrServerLabel.AutoSize = true;
            this.nvrServerLabel.Location = new System.Drawing.Point(35, 20);
            this.nvrServerLabel.Name = "nvrServerLabel";
            this.nvrServerLabel.Size = new System.Drawing.Size(87, 13);
            this.nvrServerLabel.TabIndex = 10;
            this.nvrServerLabel.Text = "NVR IP &Address:";
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
            // AddBookmarkDialog
            // 
            this.AcceptButton = this.addBookmarkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 211);
            this.Controls.Add(this.addBookmarkButton);
            this.Controls.Add(this.autoTimeStampCheckBox);
            this.Controls.Add(this.bookmarkTextBox);
            this.Controls.Add(this.timeStampTextBox);
            this.Controls.Add(this.bookmarkTextLabel);
            this.Controls.Add(this.camIpAddressTextBox);
            this.Controls.Add(this.camIPAddressLabel);
            this.Controls.Add(this.nvrIpAddressTextBox);
            this.Controls.Add(this.nvrServerLabel);
            this.Controls.Add(this.timeStampLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBookmarkDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Bookmark Dialog";
            this.Load += new System.EventHandler(this.AddBookmarkDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button addBookmarkButton;
        internal System.Windows.Forms.CheckBox autoTimeStampCheckBox;
        internal System.Windows.Forms.TextBox bookmarkTextBox;
        internal System.Windows.Forms.TextBox timeStampTextBox;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.Label bookmarkTextLabel;
        internal System.Windows.Forms.TextBox camIpAddressTextBox;
        internal System.Windows.Forms.Label camIPAddressLabel;
        internal System.Windows.Forms.TextBox nvrIpAddressTextBox;
        internal System.Windows.Forms.Label nvrServerLabel;
        internal System.Windows.Forms.Label timeStampLabel;
    }
}

