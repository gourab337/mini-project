using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IvAddBookmark2
{

    public partial class AddBookmark2Dialog : Form
    {
        public AddBookmark2Dialog()
        {
            InitializeComponent();
        }
        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        const string DATETIME_FORMAT_STRING = "dd/MM/yyyy HH:mm:ss.fff";

        /// <summary>
        /// Adds sikLib on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBookmarkDialog_Load( object sender, EventArgs e )
        {
            ivBind = new sikLib2.IvBind2();
        }

        /// <summary>
        /// Displays the current time in the timestamp text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick( object sender, EventArgs e )
        {
            if ( autoTimeStampCheckBox.Checked )
            {
                timeStampTextBox.Text = DateTime.Now.ToString( DATETIME_FORMAT_STRING );
            }
        }

        /// <summary>
        /// Wraps MessageBox.Show to display a simple message
        /// </summary>
        /// <param name="text">text to be displayed in message box</param>
        /// <param name="title">title of message box</param>
        /// <param name="icon">icon to be used</param>
        private void ShowMessageBox(
            string text, string title, 
            System.Windows.Forms.MessageBoxIcon icon
            )
        {
            MessageBox.Show ( this, text, title, MessageBoxButtons.OK, icon );
        }

        /// <summary>
        /// Button event handler
        /// Validates user input and attempts to add a bookmark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBookmark2Button_Click( object sender, EventArgs e )
        {
            //
            // Validate NVR IP Address
            //
            string nvrIpAddr;
            nvrIpAddr = nvrIpAddressTextBox.Text.Trim();

            if ( nvrIpAddr == string.Empty )
            {
                ShowMessageBox(
                    "Please enter value for NVR Server IP Address.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                nvrIpAddressTextBox.Focus();
                return;
            }
            //
            // Validate Camera Service ID
            //
            string camServiceId;
            camServiceId = camServiceIdTextBox.Text.Trim();

            if ( camServiceId == string.Empty )
            {
                ShowMessageBox(
                    "Please enter value for Camera ServiceId.", "Warning",
                    MessageBoxIcon.Exclamation
                    );

                camServiceIdTextBox.Focus();
                return;
            }
            uint securityLevel;
            securityLevel = Convert.ToUInt32( securityLevelTextBox.Text.Trim() );
            if ( securityLevel < 1 || securityLevel > 5 )
            {
                ShowMessageBox(
                    "Please enter valid value for security level. (1 to 5)", "Warning",
                    MessageBoxIcon.Exclamation
                    );

                securityLevelTextBox.Focus();
                return;
            }

            //
            // Retrieve bookmark text
            //
            string bookmark2Text;
            bookmark2Text = bookmark2TextBox.Text;

            //
            // Validate timestamp
            //
            string timeStampStr;
            DateTime timeStamp;
            timeStampStr = timeStampTextBox.Text.Trim();

            //
            // If user does not specify timestamp, use the current time
            //
            if ( timeStampStr == string.Empty )
            {
                timeStampStr = DateTime.Now.ToString( DATETIME_FORMAT_STRING );
                timeStampTextBox.Text = timeStampStr;
            }

            //
            // If timestamp is a integer number, treat it as relative milliseconds
            //
            int timeStampInMs;

            if ( int.TryParse( timeStampStr, out timeStampInMs ) )
            {
                //
                // Number of milliseconds must be an integer number
                //
                if ( timeStampInMs != double.Parse( timeStampStr ) )
                {
                    ShowMessageBox(
                        "Please enter valid milliseconds for TimeStamp.", "Warning", 
                        MessageBoxIcon.Exclamation
                        );

                    timeStampTextBox.Focus();
                    return;
                }

                timeStamp = DateTime.UtcNow.AddMilliseconds( timeStampInMs );
            }
            else
            {
                try
                {
                    System.Globalization.CultureInfo currentCultureInfo = 
                        new System.Globalization.CultureInfo(
                            System.Globalization.CultureInfo.CurrentCulture.Name
                            );
                    //
                    // Convert input value with specific format in current timezone
                    //
                    timeStamp = DateTime.ParseExact(
                                            timeStampStr, DATETIME_FORMAT_STRING, currentCultureInfo, 
                                            System.Globalization.DateTimeStyles.None
                                            );

                    timeStamp = TimeZone.CurrentTimeZone.ToUniversalTime( timeStamp );
                }
                catch ( Exception )
                {
                    ShowMessageBox(
                        "Please enter valid value for TimeStamp.", "Warning", 
                        MessageBoxIcon.Exclamation
                        );

                    timeStampTextBox.Focus();
                    return;
                }
            }

            try
            {
                //
                // Call the AddBookmark method of the IvBind COM component
                //
                ivBind.AddBookmark2 (nvrIpAddr, camServiceId, bookmark2Text, timeStamp, securityLevel );
                ShowMessageBox(
                    "Add Bookmark successful.", "IvBind CSNetClient", 
                    MessageBoxIcon.Information
                    );
            }
            catch ( Exception ex )
            {
                ShowMessageBox( ex.Message, "IvBind CSNetClient", MessageBoxIcon.Error );
            }
        }
    }
}
    

