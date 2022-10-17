///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// RestoreDetectorDialog
//
// This class implements a dialog box which supports restore detector(s) in
// an IndigoVision Alarm Server (NVR).
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IvRestoreDetector
{
    public partial class RestoreDetectorDialog : Form
    {
        public RestoreDetectorDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        private void RestoreDetectorDialog_Load(object sender, EventArgs e)
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

        private void restoreDetectorButton_Click(object sender, EventArgs e)
        {
            string asIpAddr;
            asIpAddr = asIpAddressTextBox.Text.Trim();

            if (asIpAddr == string.Empty)
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

            if (zoneName == string.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Zone Name.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                zoneNameTextBox.Focus();
                return;
            }

            string detectorName;
            detectorName = detectorNameTextBox.Text;

            if (detectorName == string.Empty)
            {
                ShowMessageBox(
                    "Please enter value for Detector Name.", "Warning", 
                    MessageBoxIcon.Exclamation
                    );

                detectorNameTextBox.Focus();
                return;
            }

            try
            {
                //
                // Call the RestoreDetector method of the IvBind COM component
                //
                ivBind.RestoreDetector(asIpAddr, zoneName, detectorName);

                ShowMessageBox(
                    "Restore detector successful.", "IvBind CSNetClient", 
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
