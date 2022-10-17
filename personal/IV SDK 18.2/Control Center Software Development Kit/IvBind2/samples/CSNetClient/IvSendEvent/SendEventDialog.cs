///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// SendEventDialog
//
// This class implements a dialog box which sends an External System event
// to an IndigoVision Alarm Server ( NVR ).
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IvSendEvent
{
    public partial class SendEventDialog : Form
    {
        public SendEventDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        const string DATETIME_FORMAT_STRING = "dd/MM/yyyy HH:mm:ss.fff";

        private void SendEventDialog_Load(object sender, EventArgs e)
        {
            timer1.Start();
            ivBind = new sikLib2.IvBind2();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (autoTimeStampCheckBox.Checked)
            {
                timeStampTextBox.Text = DateTime.Now.ToString(DATETIME_FORMAT_STRING);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ShowMessageBox
        //
        // Wraps MessageBox.Show to display a simple message
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ShowMessageBox(
                        string text, string title, 
                        System.Windows.Forms.MessageBoxIcon icon
                        )
        {
            MessageBox.Show(this, text, title, MessageBoxButtons.OK, icon);
        }

        private void sendEventButton_Click(object sender, EventArgs e)
        {
            //
            // Validate as IP Address
            //
            string nvrIpAddr;
            nvrIpAddr = ipAddressTextBox.Text.Trim();

            if (nvrIpAddr == string.Empty)
            {
                ShowMessageBox(
                    "Please enter value for NVR Server IP Address.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                ipAddressTextBox.Focus();
                return;
            }

            //
            // Validate Local IP Address
            //
            string localIpAddr;
            localIpAddr = localIpAddressTextBox.Text.Trim();

            if (localIpAddr == string.Empty)
            {
                localIpAddr = null;
            }

            //
            // Retrieve event number
            //
            int eventNumber;
            eventNumber = int.Parse(eventNumberSpinBox.Text.Trim());

            //
            // Validate timestamp
            //
            string timeStampStr;
            DateTime timeStamp;
            timeStampStr = timeStampTextBox.Text.Trim();

            //
            // If user does not specify timestamp, use the current time
            //
            if (timeStampStr == string.Empty)
            {
                timeStampStr = DateTime.Now.ToString(DATETIME_FORMAT_STRING);
                timeStampTextBox.Text = timeStampStr;
            }

            //
            // If timestamp is a integer number, treat it as relative milliseconds
            //
            int timeStampInMs;

            if (int.TryParse(timeStampStr, out timeStampInMs))
            {                
                //
                // Number of milliseconds must be an integer number
                //
                if (timeStampInMs != double.Parse(timeStampStr))
                {
                    ShowMessageBox(
                        "Please enter valid milliseconds for TimeStamp.", "Warning", 
                        MessageBoxIcon.Exclamation
                        );

                    timeStampTextBox.Focus();
                    return;
                }

                timeStamp = DateTime.UtcNow.AddMilliseconds(timeStampInMs);
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
                    // Convert input value with specific format in Current timezone
                    //
                    timeStamp = DateTime.ParseExact(
                                            timeStampStr, DATETIME_FORMAT_STRING, currentCultureInfo, 
                                            System.Globalization.DateTimeStyles.None
                                            );

                    timeStamp = TimeZone.CurrentTimeZone.ToUniversalTime(timeStamp);
                }
                catch (Exception)
                {
                    ShowMessageBox(
                        "Please enter valid value for TimeStamp.", "Warning", 
                        MessageBoxIcon.Exclamation
                        );

                    timeStampTextBox.Focus();
                    return;
                }
            }

            //
            // Retrieve annotation
            //
            string annotation = annotationTextBox.Text;

            try
            {
                //
                // Call the SendEvent2 method of the IvBind COM component to send
                // the external system event
                //
                ivBind.SendEvent2(
                    nvrIpAddr,
                    (ushort)eventNumber,
                    localIpAddr,
                    timeStamp,
                    annotation );

                ShowMessageBox(
                    "Send event successful.", "IvBind CSNetClient", 
                    MessageBoxIcon.Information
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, "IvBind CSNetClient", MessageBoxIcon.Error);
            }
        }
    }
}
