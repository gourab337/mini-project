///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// AckZoneDialog
//
// This class implements a dialog box which support Acknowledge Zone when an
// event is sent to an IndigoVision Alarm Server (NVR).
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IvAckZone
{    
    public partial class AckZoneDialog : Form
    {
        public AckZoneDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        private void AckZoneDialog_Load(object sender, EventArgs e)
        {
            ivBind = new sikLib2.IvBind2();
        }

        private void ackZoneButton_Click(object sender, EventArgs e)
        {
            string asIpAddr;
            asIpAddr = ipAddressTextBox.Text.Trim();

            if (asIpAddr == string.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Alarm Server IP Address.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                ipAddressTextBox.Focus();
                return;
            }

            string zoneName;
            zoneName = zoneNameTextBox.Text;

            if (zoneName == string.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Zone Name.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                zoneNameTextBox.Focus();
                return;
            }

            string ackMessage;
            ackMessage = ackMessageTextBox.Text;

            try
            {
                //
                // Call the AcknowledgeZone method of the IvBind COM component
                // to Acknowledge Zone
                //
                ivBind.AcknowledgeZone(asIpAddr, zoneName, ackMessage);

                ShowMessageBox(
                    "Acknowledge zone successful.", "IvBind CSNetClient", 
                    MessageBoxIcon.Information
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(
                    ex.Message, "IvBind CSNetClient", 
                    MessageBoxIcon.Error
                    );
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
    }
}
