using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IvActivationQuery
{
    /// <summary>
    /// A dialog that demonstrates use of the activation query functionality in
    /// the binding kit. This can be used to query an IndigoVision Alarm
    /// Server for historical and current activation records.
    /// </summary>
    public partial class ActivationQueryDialog : Form
    {
        public ActivationQueryDialog()
        {
            InitializeComponent();
        }

        // Use the IndigoVision Software Development Kit IvBind component.
        public sikLib2.IvBind2 ivBind;

        // Tracks whether the query is running or not
        private bool isQueryRunning;

        // Declare the UpdateConsole as a .Net Delegate.
        delegate void UpdateConsoleHandler(string message);

        // Declare the AddActivationToGridView as a .Net Delegate.
        delegate void AddActivationToGridViewHandler(
            sikLib2.ActivationRecord actRec
            );

        // Declare the RemoveActivationFromGridView as a .Net Delegate.
        delegate void RemoveActivationFromGridViewHandler(
            UInt64 activationId
            );

        // Declare the ResetAppStatusHandler as a .Net Delegate.
        delegate void ResetAppStatusHandler();

        /// <summary>
        /// Initialise the IvBind library and our activation record notification
        /// callbacks when the dialog is displayed for the first time.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void ActivationQueryDialog_Load(object sender, EventArgs e)
        {
            //
            // Initialise Binding Kit and configure event handlers
            //
            ivBind = new sikLib2.IvBind2();
            ivBind.OnActivationRecordPrepopulate +=
                new sikLib2._IIvBind2Events_OnActivationRecordPrepopulateEventHandler(
                    ivBind_OnActivationRecordPrepopulate
                    );
            ivBind.OnActivationRecordPrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnActivationRecordPrepopulateCompleteEventHandler(
                    ivBind_OnActivationRecordPrepopulateComplete
                    );
            ivBind.OnActivationRecordNew +=
                new sikLib2._IIvBind2Events_OnActivationRecordNewEventHandler(
                    ivBind_OnActivationRecordNew
                    );
            ivBind.OnActivationRecordDelete +=
                new sikLib2._IIvBind2Events_OnActivationRecordDeleteEventHandler(
                    ivBind_OnActivationRecordDelete
                    );
            ivBind.OnActivationRecordRequestCancel +=
                new sikLib2._IIvBind2Events_OnActivationRecordRequestCancelEventHandler(
                    ivBind_OnActivationRecordRequestCancel
                    );
            ivBind.OnActivationRecordServerDisconnect +=
                new sikLib2._IIvBind2Events_OnActivationRecordServerDisconnectEventHandler(
                    ivBind_OnActivationRecordServerDisconnect
                    );

            //
            // Initialise the interface
            // - disable the disconnect button
            // - set useful default timestamps for min and max activation time
            //
            disconnectButton.Enabled = false;
            DateTime currentLocalTime = DateTime.Now;
            minActivationTimePicker.Value = currentLocalTime.AddDays(-7);
            maxActivationTimePicker.Value = currentLocalTime.AddDays(7);
        }

        /// <summary>
        /// Utility method for displaying a message box
        /// </summary>
        /// <param name="text">text to be displayed</param>
        /// <param name="icon">icon to be used</param>
        private void ShowMessageBox(
            string text,
            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.Error
            )
        {
            MessageBox.Show(this, text, "Alarm Query Alert", MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Appends the passed String to the console text box.
        /// </summary>
        /// <remarks>
        /// This function is used delegated to do call from the IvBind event handlers
        /// which exist on a separate thread to that which created the text box.
        /// </remarks>
        /// <param name="message">The text to append to the console</param>
        public void UpdateConsole(string message)
        {
            this.consoleTextBox.AppendText(message);
            this.consoleTextBox.AppendText("\r\n");
            this.consoleTextBox.Update();
            this.consoleTextBox.Refresh();
        }

        /// <summary>
        /// Called when the dialog is closed. Stops any active query.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void ActivationQueryDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ivBind != null && isQueryRunning)
                {
                    ivBind.StopActivationRecordQuery(asIpAddressTextBox.Text.Trim());
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Convert a time from the Binding Kit to a string representation.
        /// </summary>
        /// <remarks>
        /// If TimeValue equal default value (1/1/1970), we must show N/A instead.
        /// </remarks>
        /// <param name="inputTime">the time to convert</param>
        /// <returns>String representation of the time</returns>
        private string ShowCorrectTime(DateTime inputTime)
        {
            string returnValue;
            if (inputTime == DateTime.Parse("1/1/1970"))
            {
                returnValue = "N/A";
            }
            else
            {
                inputTime = TimeZone.CurrentTimeZone.ToLocalTime(inputTime);
                returnValue = inputTime.ToString();
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an ItemId from the Binding Kit to a string representation.
        /// </summary>
        /// <remarks>
        /// If Id is zero, we show 'N/A'.
        /// If Id is the max value of the datatype, we show 'Invalid'.
        /// </remarks>
        /// <param name="id">the ID to convert</param>
        /// <returns>String representation of ID</returns>
        private string ShowCorrectId(UInt64 id)
        {
            string returnValue;
            if (id == 0)
            {
                returnValue = "N/A";
            }
            else if (id == UInt64.MaxValue)
            {
                returnValue = "Invalid";
            }
            else
            {
                returnValue = id.ToString();
            }
            return returnValue;
        }

        /// <summary>
        /// Add a new activation record to the GridView.
        /// </summary>
        /// <param name="actRec">Activation record details</param>
        private void AddActivationToGridView(sikLib2.ActivationRecord actRec)
        {
            string activationRecordId = ShowCorrectId(actRec.ActivationRecordId);
            string detectorId = ShowCorrectId(actRec.DetectorId);
            string activationTime = ShowCorrectTime(actRec.ActivationTime);
            string alarmRecordId = ShowCorrectId(actRec.AlarmRecordId);
            string zoneId = ShowCorrectId(actRec.ZoneId);
            string alarmTime = ShowCorrectTime(actRec.AlarmTime);
            string extraInfo = ExtraInfoToString(actRec.ExtraInfo);

            activationRecordsGridView.Rows.Add(
                activationRecordId,
                detectorId,
                activationTime,
                alarmRecordId,
                zoneId,
                alarmTime,
                extraInfo
                );
        }

        /// <summary>
        /// Remove an existing activation record from the grid view.
        /// </summary>
        /// <param name="activationId">Id of the record to remove</param>
        private void RemoveActivationFromGridView(
            UInt64 activationId
            )
        {
            // Note that production code would require a better UI-data
            // lookup mechanism to scale with the number of activations.
            foreach (DataGridViewRow row in activationRecordsGridView.Rows)
            {
                try
                {
                    if (UInt64.Parse(row.Cells[0].Value.ToString())
                        == activationId)
                    {
                        activationRecordsGridView.Rows.Remove(row);
                        break;
                    }
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// Convert an activation extra info object to a string.
        /// </summary>
        /// <param name="extraInfo">the extra info object to convert</param>
        /// <returns>a human readable string representing the extra info</returns>
        private string ExtraInfoToString(
            sikLib2.ActivationExtraInfoBase extraInfo
            )
        {
            string returnValue;

            switch (extraInfo.Type)
            {
                case sikLib2.ActivationExtraInfoType.SIK_ACTIVATIONEXTRAINFOTYPE_NONE:
                    returnValue = "No Extra Info";
                    break;

                case sikLib2.ActivationExtraInfoType.SIK_ACTIVATIONEXTRAINFOTYPE_NVRFAULTINFO:
                    returnValue = "NVR Fault Information\r\n";
                    sikLib2.IActivationNvrFaultInfo faultInfo =
                        (sikLib2.IActivationNvrFaultInfo) extraInfo;
                    returnValue += "Number Faults: " + faultInfo.NumFaults;
                    for (int i = 0; i < faultInfo.NumFaults; ++i)
                    {
                        returnValue += "\r\nFault " + i + ": " +
                            FaultTypeToString(faultInfo.GetFaultType(i));
                    }
                    break;

                case sikLib2.ActivationExtraInfoType.SIK_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION:
                   sikLib2.IAnnotationInfo annotationInfo =
                        ( sikLib2.IAnnotationInfo ) extraInfo;
                   returnValue = "Annotation Info: " + annotationInfo.AnnotationMsg;
                   break;

                default:
                    returnValue = "Unknown";
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// Convert a NVR fault type to a string
        /// </summary>
        /// <param name="faultType">fault type to convert</param>
        /// <returns>human readable string representation of fault type</returns>
        private string FaultTypeToString(sikLib2.NvrFaultType faultType)
        {
            switch (faultType)
            {
                default:
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_UNKNOWN:
                    return "Unknown";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_RECORDINGFAILURE:
                    return "Recording Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_LICENSEFAILURE:
                    return "License Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_RAIDDEGRADED:
                    return "Raid Degraded";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_REDUNDANTPOWERFAIL:
                    return "Redundant Power Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_REDUNDANTNETWORKFAIL:
                    return "Redundant Network Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_DEVICEOFFLINE:
                    return "Device Offline";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_UPSONBATTERY:
                    return "UPS On Battery";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_FANFAILURE:
                    return "Fan Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_SYSTEMOVERTEMP:
                    return "System Over Temperature";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_DISKOVERTEMP:
                    return "Disk Over Temperature";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_STORAGEARRAYMONITORINGFAILURE:
                    return "Storage Array Monitoring Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_STORAGEARRAYDISKFAILURE:
                    return "Storage Array Disk Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_STORAGEARRAYREDUNDANCYFAILURE:
                    return "Storage Array Redundancy Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_STORAGEARRAYENCLOSUREFAILURE:
                    return "Storage Array Enclosure Failure";
                case sikLib2.NvrFaultType.SIK_NVRFAULTTYPE_LOWDISKSPACE:
                    return "Disk Space Too Low";
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordPrepopulate event. Adds the
        /// new activation to the grid view.
        /// </summary>
        /// <remarks>
        /// This function will be called repeatedly at a very high rate
        /// as the query results are processed. As it creates work on
        /// the UI thread, we need to be careful to leave some idle time
        /// to keep the UI responsive.
        /// </remarks>
        /// <param name="alarmRec">the activation record details</param>
        private void ivBind_OnActivationRecordPrepopulate(
            sikLib2.ActivationRecord actRec
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            // Keep the UI responsive when processing the query results
            // with a forced sleep for each notification.
            Thread.Sleep(10); // ms

            try
            {
                // Delegate updating of the GridView with the new activation
                this.BeginInvoke(
                    new AddActivationToGridViewHandler(
                        AddActivationToGridView
                        ),
                    new object[] { actRec }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordPrepopulateComplete event.
        /// Adds message to the console to indicate this event occurred.
        /// </summary>
        /// <param name="asIpAddr">IP address of the alarm server</param>
        private void ivBind_OnActivationRecordPrepopulateComplete(
            string asIpAddr
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Activation record prepopulation completed " +
                    "from server " + asIpAddr + "\r\n"
                    + "All historical activation records have been sent." + "\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordNew event. Adds a new
        /// activation to the grid view.
        /// </summary>
        /// <param name="actRec">Details of activation record</param>
        private void ivBind_OnActivationRecordNew(
            sikLib2.ActivationRecord actRec
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Activation record with ID " +
                    ShowCorrectId(actRec.ActivationRecordId) +
                    " has been added.\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Delegate updating of the GridView with the new activation
                this.BeginInvoke(
                    new AddActivationToGridViewHandler(
                        AddActivationToGridView
                        ),
                    new object[] { actRec }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordDelete event. Outputs
        /// a message to the console and removes the record from the grid view.
        /// </summary>
        /// <param name="alarmRec">Activation Record details</param>
        private void ivBind_OnActivationRecordDelete(
            sikLib2.ActivationRecord actRec
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Activation record with ID " +
                    ShowCorrectId(actRec.ActivationRecordId) +
                    " has been deleted from server " +
                    actRec.ASIpAddress + "\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Delegate updating of the GridView with the new activation
                this.BeginInvoke(
                    new RemoveActivationFromGridViewHandler(
                        RemoveActivationFromGridView
                        ),
                    new object[] { actRec.ActivationRecordId }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordRequestCancel event.
        /// This signals that the query has been canceled by the server and
        /// clears up the dialog.
        /// </summary>
        /// <param name="asIpAddr">IP address of the Alarm Server</param>
        private void ivBind_OnActivationRecordRequestCancel(
            string asIpAddr
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Activation query with server " + asIpAddr +
                    " has been canceled\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Tidy up UI ready for next query
                this.BeginInvoke(new ResetAppStatusHandler(ResetAppStatus));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the OnActivationRecordServerDisconnect event.
        /// </summary>
        /// <param name="asIpAddr">IP address of the Alarm Server</param>
        private void ivBind_OnActivationRecordServerDisconnect(
            string asIpAddr
            )
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Connection to Alarm Server " + asIpAddr
                    + " has been lost\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Tidy up UI ready for next query
                this.BeginInvoke(new ResetAppStatusHandler(ResetAppStatus));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Reset the dialog when a query stops.
        /// </summary>
        private void ResetAppStatus()
        {
            // UI updates from the notifications could still be on
            // the message queue, (raised by the BeginInvoke function).
            Application.DoEvents();

            // Hint that garbage collector should run
            System.GC.Collect();

            isQueryRunning = false;
            asIpAddressTextBox.Enabled = true;
            queryButton.Enabled = true;
            disconnectButton.Enabled = false;
        }

        /// <summary>
        /// Handle click events on the query button. This will start the
        /// activation record query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear UI contents
                this.consoleTextBox.Clear();
                this.activationRecordsGridView.Rows.Clear();

                //
                // Get the Query time parameters
                //
                DateTime minTime = DateTime.FromBinary(0);
                if (minActivationTimePicker.Checked)
                {
                    minTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        minActivationTimePicker.Value
                        );
                }
                DateTime maxTime = DateTime.FromBinary(0);
                if (maxActivationTimePicker.Checked)
                {
                    maxTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        maxActivationTimePicker.Value
                        );
                }

                // Start the query
                ivBind.QueryActivationRecords(
                    asIpAddressTextBox.Text.Trim(),
                    minTime,
                    maxTime
                    );

                isQueryRunning = true;
                asIpAddressTextBox.Enabled = false;
                queryButton.Enabled = false;
                disconnectButton.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Handle click event from disconnect button. Stops the live query.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            isQueryRunning = false;

            try
            {
                ivBind.StopActivationRecordQuery(
                    asIpAddressTextBox.Text.Trim()
                    );
                ResetAppStatus();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
