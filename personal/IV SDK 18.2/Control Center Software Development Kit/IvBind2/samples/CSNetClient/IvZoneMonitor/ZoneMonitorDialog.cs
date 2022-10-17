///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// ZoneMonitorDialog
//
// This class implements a dialog which monitors IndigoVision Alarm
// notifications.
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace IvZoneMonitor
{
    public partial class ZoneMonitorDialog : Form
    {
        public ZoneMonitorDialog()
        {
            InitializeComponent();
        }

        //
        // Declare use of the IndigoVision Software Development Kit IvBind
        // component.
        //
        public sikLib2.IvBind2 ivBind;

        //
        // This indicates we are monitoring an AS or not
        //
        private bool isMonitoring;

        //
        // Declare the UpdateNotificationsTextBoxHandler as a .Net Delegate.
        //
        delegate void UpdateNotificationsTextBoxHandler(string message);

        //
        // Declare the ResetAppStatusHandler as a .Net Delegate.
        //
        delegate void ResetAppStatusHandler();

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ZoneMonitorForm_Load
        //
        // Registers for alarm notifications with the NVR specified in the IP Address
        // text box and adds the IP address to the AlarmServers List Box.
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ZoneMonitorDialog_Load(object sender, EventArgs e)
        {
            ivBind = new sikLib2.IvBind2();
            ivBind.OnZonePrepopulate += 
                new sikLib2._IIvBind2Events_OnZonePrepopulateEventHandler(
                                ivBind_OnZonePrepopulate
                                );
            ivBind.OnZonePrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnZonePrepopulateCompleteEventHandler(
                                ivBind_OnZonePrepopulateComplete
                                );
            ivBind.OnZoneNew += 
                new sikLib2._IIvBind2Events_OnZoneNewEventHandler(ivBind_OnZoneNew);
            ivBind.OnZoneUpdate += 
                new sikLib2._IIvBind2Events_OnZoneUpdateEventHandler(ivBind_OnZoneUpdate);
            ivBind.OnZoneDelete += 
                new sikLib2._IIvBind2Events_OnZoneDeleteEventHandler(ivBind_OnZoneDelete);
            ivBind.OnZoneRequestCancel += 
                new sikLib2._IIvBind2Events_OnZoneRequestCancelEventHandler(
                                ivBind_OnZoneRequestCancel
                                );
            ivBind.OnZoneServerDisconnect += 
                new sikLib2._IIvBind2Events_OnZoneServerDisconnectEventHandler(
                                ivBind_OnZoneServerDisconnect
                                );
            disconnectButton.Enabled = false;
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ShowMessageBox
        //
        // Wraps MessageBox.Show to display to set most parameters
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ShowMessageBox(string text, System.Windows.Forms.MessageBoxIcon icon)
        {
            MessageBox.Show(this, text, "IvBind CSNetClient", MessageBoxButtons.OK, icon);
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // UpdateNotificationsTextBox
        //
        // Appends the passed String to the ZoneMonitorForm alarm notifications
        // text box.
        // This function is used delegated to do so from the IvBind event handlers
        // which exist on a separate thread to that which created the text box.
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        public void UpdateNotificationsTextBox(string message)
        {
            this.notificationsTextBox.AppendText(message);
            this.notificationsTextBox.AppendText("\r\n");
            this.notificationsTextBox.Update();
            this.notificationsTextBox.Refresh();
        }

        private void ZoneMonitorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ivBind != null && isMonitoring)
                {
                    ivBind.StopZoneNotifications(asIpAddressTextBox.Text.Trim());
                }
            }
            catch (Exception)
            {
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Show correct time value
        //
        // If TimeValue equal default value (1/1/1970), we must show N/A instead
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private string ShowCorrectTime(DateTime InputTime)
        {
            string returnValue;
            if (InputTime == DateTime.Parse("1/1/1970"))
            {
                returnValue = "N/A";
            }
            else
            {
                InputTime = TimeZone.CurrentTimeZone.ToLocalTime(InputTime);
                returnValue = InputTime.ToString();
            }
            return returnValue;
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Show correct Id value
        //
        // If Id is zero, we show 'N/A'
        // If Id is the max value of the datatype, we show 'Invalid'
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private string ShowCorrectId(UInt64 Id)
        {
            string returnValue;
            if (Id == 0)
            {
                returnValue = "N/A";
            }
            else if (Id == UInt64.MaxValue)
            {
                returnValue = "Invalid";
            }
            else
            {
                returnValue = Id.ToString();
            }
            return returnValue;
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZonePrepopulate event handler
        //
        // Outputs all properties of the prepopulated zone to the console
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZonePrepopulate(sikLib2.zone zone)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            // Keep the UI responsive when processing the query results
            // with a forced sleep for each notification.
            Thread.Sleep(10); // ms

            try
            {
                string ZoneMessage;

                //
                // Obtain the zone server (NVR) properties
                //
                ZoneMessage = 
                    "Zone notification prepopulation received from server " + 
                    zone.ASIpAddress + "\r\n";

                //
                // Obtain the zone source properties
                //                
                ZoneMessage = ZoneMessage + "Zone details:" + "\r\n" + 
                    " Id = " + ShowCorrectId(zone.Id) + "\r\n" + 
                    " Name = " + zone.Name + "\r\n" + 
                    " OwnerId = " + ShowCorrectId(zone.OwnerId) + "\r\n" +
                    " ScheduleId = " + ShowCorrectId(zone.ScheduleId) + "\r\n" +
                    " AlarmRecordId = " + ShowCorrectId(zone.AlarmRecordId) + "\r\n" +
                    " Priority = " + zone.Priority.ToString() + "\r\n" + 
                    " State = " + zone.StateDesc + "\r\n" + 
                    " TimeRaised = " + ShowCorrectTime(zone.TimeRaised) + "\r\n" + 
                    " TimeAcknowledged = " + ShowCorrectTime(zone.TimeAcknowledged) + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(
                    new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                    new object[] { ZoneMessage }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZonePrepopulateComplete event handler
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZonePrepopulateComplete(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string ZoneMessage;

                ZoneMessage = "Zone prepopulation completed from server "
                    + asIpAddr + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(
                    new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox),
                    new object[] { ZoneMessage }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZoneNew event handler
        //
        // Outputs all properties of the new zone to the console
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZoneNew(sikLib2.zone zone)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string ZoneMessage;

                //
                // Obtain the zone server (NVR) properties
                //
                ZoneMessage = "Zone notification insertion received from server " + 
                                zone.ASIpAddress + "\r\n";

                //
                // Obtain the zone source properties
                //                
                ZoneMessage = ZoneMessage + 
                    "Zone details:" + "\r\n" + 
                    " Id = " + ShowCorrectId(zone.Id) + "\r\n" + 
                    " Name = " + zone.Name + "\r\n" + 
                    " OwnerId = " + ShowCorrectId(zone.OwnerId) + "\r\n" +
                    " ScheduleId = " + ShowCorrectId(zone.ScheduleId) + "\r\n" + 
                    " AlarmRecordId = " + ShowCorrectId(zone.AlarmRecordId) + "\r\n" +
                    " Priority = " + zone.Priority.ToString() + "\r\n" +
                    " State = " + zone.StateDesc + "\r\n" + 
                    " TimeRaised = " + ShowCorrectTime(zone.TimeRaised) + "\r\n" + 
                    " TimeAcknowledged = " + ShowCorrectTime(zone.TimeAcknowledged) + "\r\n";
                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(
                                    UpdateNotificationsTextBox), new object[] { ZoneMessage }
                                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZoneUpdate event handler
        //
        // Outputs all properties of the updated zone to the console
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZoneUpdate(sikLib2.zone zone)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string ZoneMessage;

                //
                // Obtain the zone server (NVR) properties
                //
                ZoneMessage = "Zone notification update received from server " + 
                                zone.ASIpAddress + "\r\n";

                //
                // Obtain the zone source properties
                //                
                ZoneMessage = ZoneMessage + "Zone details:" + "\r\n" + " Id = " +
                                ShowCorrectId(zone.Id) + "\r\n" + " Name = " + zone.Name +
                                "\r\n" + " OwnerId = " + ShowCorrectId(zone.OwnerId) + "\r\n" +
                                " ScheduleId = " + ShowCorrectId(zone.ScheduleId) + "\r\n" +
                                " AlarmRecordId = " + ShowCorrectId(zone.AlarmRecordId) +
                                "\r\n" + " Priority = " + zone.Priority.ToString() + "\r\n" +
                                " State = " + zone.StateDesc + "\r\n" + " TimeRaised = " +
                                ShowCorrectTime(zone.TimeRaised) + "\r\n" +
                                " TimeAcknowledged = " + ShowCorrectTime(zone.TimeAcknowledged) + 
                                "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { ZoneMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZoneDelete event handler
        //
        // Outputs all properties of the deleted zone to the console
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZoneDelete(sikLib2.zone zone)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string ZoneMessage;

                //
                // Obtain the Zone server (NVR) properties
                //                
                ZoneMessage = "Zone with Id = " + ShowCorrectId(zone.Id) + 
                                " is deleted from server " + zone.ASIpAddress + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { ZoneMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZoneCancel event handler
        //
        // Outputs all properties of the cancelled zone to the console
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZoneRequestCancel(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;

            }
            try
            {
                string ZoneMessage;

                //
                // Obtain the zone server (NVR) properties
                //
                ZoneMessage = "Zone notification is cancelled from server " + asIpAddr + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { ZoneMessage }
                        );

                //
                // Reset App status to as ready for new monitoring
                //
                this.BeginInvoke(new ResetAppStatusHandler(ResetAppStatus));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnZoneDisconnect event handler
        //
        // Outputs a disconnected message, then exit the monitor.
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnZoneServerDisconnect(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string ZoneMessage;

                //
                // Update the Zone Message
                //
                ZoneMessage = "The zone monitor is disconnected from server " + asIpAddr + ".";

                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { ZoneMessage }
                        );

                //
                // Reset App status to as ready for new monitoring
                //
                this.BeginInvoke(new ResetAppStatusHandler(ResetAppStatus));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ResetAppStatus
        //
        // Run garbage collector, then reset components' status.
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ResetAppStatus()
        {
            // UI updates from the alarm record notifications could still
            // be on the message queue, (raised by the BeginInvoke function).
            Application.DoEvents();

            // Hint that garbage collector should run
            System.GC.Collect();

            isMonitoring = false;
            asIpAddressTextBox.Enabled = true;
            monitorButton.Enabled = true;
            disconnectButton.Enabled = false;
        }

        private void monitorButton_Click(object sender, EventArgs e)
        {
            try
            {
                //
                // Clear content of the text box
                //
                this.notificationsTextBox.Clear();

                if (dnaProxyIPInput.Text.Trim() != "")
                {
                    ivBind.SetDnaProxy(dnaProxyIPInput.Text.Trim(), (ushort)dnaProxyPortInput.Value);
                }

                ivBind.ReceiveZoneNotifications(asIpAddressTextBox.Text.Trim());
                isMonitoring = true;

                asIpAddressTextBox.Enabled = false;
                monitorButton.Enabled = false;
                disconnectButton.Enabled = true;

            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            isMonitoring = false;

            try
            {
                ivBind.StopZoneNotifications(asIpAddressTextBox.Text.Trim());
                ResetAppStatus();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void dnaProxyIPInput_TextChanged(object sender, EventArgs e)
        {
            const string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            Regex check = new Regex(Pattern);

            if (check.IsMatch(dnaProxyIPInput.Text.Trim(), 0) || dnaProxyIPInput.Text.Trim() == "")
            {
                dnaProxyIPInput.BackColor = System.Drawing.Color.White;
            }
            else
                dnaProxyIPInput.BackColor = System.Drawing.Color.LightPink;
        }
    }
}
