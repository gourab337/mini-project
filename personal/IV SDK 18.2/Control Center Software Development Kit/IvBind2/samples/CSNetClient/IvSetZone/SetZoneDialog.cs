///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// SetZoneDialog
//
// This class implements a dialog box which supports Set zone(s) in an
// IndigoVision Alarm Server (NVR).
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IvSetZone
{
    public partial class SetZoneDialog : Form
    {
        public SetZoneDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        private void SetZoneDialog_Load(object sender, EventArgs e)
        {
            ivBind = new sikLib2.IvBind2();
        }

        private void setZoneButton_Click(object sender, EventArgs e)
        {
            string asIpAddr;
            asIpAddr = asIpAddressTextBox.Text.Trim();

            if (asIpAddr == String.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Alarm Server IP Address.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                asIpAddressTextBox.Focus();
                return;
            }

            string zoneName;
            zoneName = zoneNameTextBox.Text;

            if (zoneName == String.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Zone Name.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                zoneNameTextBox.Focus();
                return;
            }

            try
            {
                //
                // Call the SetZone method of the IvBind COM component
                //
                ivBind.SetZone(asIpAddr, zoneName);

                ShowMessageBox(
                    "Set zone successful.", "IvBind CSNetClient", 
                    MessageBoxIcon.Information
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(
                    ex.Message, "IvBind CSNetClient", MessageBoxIcon.Error
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
