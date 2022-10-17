using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IvSendData
{

    public partial class SendDataDialog : Form
    {
        public SendDataDialog()
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
        private void SendDataDialog_Load( object sender, EventArgs e )
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
        /// Validates user input and attempts to send data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendDataButton_Click( object sender, EventArgs e )
        {
            //
            // Validate AS IP Address
            //
            string asIpAddr;
            asIpAddr = asIpAddressTextBox.Text.Trim();

            if ( asIpAddr == string.Empty )
            {
                ShowMessageBox(
                    "Please enter value for Alarm Server IP Address.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                asIpAddressTextBox.Focus();
                return;
            }

            //
            // Validate external system IP address
            //
            string extSystemIpAddr;
            extSystemIpAddr = extSystemTextBox.Text.Trim();

            if ( extSystemIpAddr == string.Empty )
            {
                ShowMessageBox(
                    "Please enter value for External System IP Address.", "Warning",
                    MessageBoxIcon.Exclamation
                    );

                extSystemTextBox.Focus();
                return;
            }

            //
            // Validate source number
            //
            int sourceNumber;
            sourceNumber = Convert.ToInt32( sourceNumberTextBox.Text.Trim() );
            if ( sourceNumber < 1 || sourceNumber > 32767 )
            {
                ShowMessageBox(
                    "Please enter valid value for source number. (1 to 32767)", "Warning",
                    MessageBoxIcon.Exclamation
                    );

                sourceNumberTextBox.Focus();
                return;
            }

            //
            // Retrieve data
            //
            string dataText;
            dataText = sendDataTextBox.Text;

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
                // Call the SendData method of the IvBind COM component
                //
                ivBind.SendData (
                    asIpAddr,
                    extSystemIpAddr,
                    sourceNumber,
                    timeStamp,
                    dataText
                    );
                ShowMessageBox(
                    "Send Data successful.", "IvBind CSNetClient", 
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
    

