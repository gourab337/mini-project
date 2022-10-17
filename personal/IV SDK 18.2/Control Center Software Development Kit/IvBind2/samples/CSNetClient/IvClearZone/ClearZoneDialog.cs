///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// ClearZoneDialog
//
// This class implements a dialog box which support Clear Zone''s acknowledgeable
// status when an event is sent to an IndigoVision Alarm Server (NVR).
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IvClearZone
{
    public partial class ClearZoneDialog : Form
    {
        public ClearZoneDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        private void ClearZoneDialog_Load(object sender, EventArgs e)
        {
            ivBind = new sikLib2.IvBind2();
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

        private void clearZoneButton_Click(object sender, EventArgs e)
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

            string clrMessage;
            clrMessage = clearMessageTextBox.Text;

            try
            {
                //
                // Call the ClearZone method of the IvBind COM component
                // to clear a zone
                //
                ivBind.ClearZone(asIpAddr, zoneName, clrMessage);

                ShowMessageBox(
                    "Clear zone successful.", "IvBind CSNetClient", 
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
    }
}
