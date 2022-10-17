using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IvAlarmQuery
{
    /// <summary>
    /// A dialog that demonstrates use of the alarm query functionality in
    /// the binding kit. This can be used to query an IndigoVision Alarm
    /// Server for historical and current alarm records.
    /// </summary>
    public partial class AlarmQueryDialog : Form
    {
        public AlarmQueryDialog()
        {
            InitializeComponent();
        }

        // Use the IndigoVision Software Development Kit IvBind component.
        public sikLib2.IvBind2 ivBind;

        // Tracks whether the query is running or not
        private bool isQueryRunning;

        // Declare the UpdateConsole as a .Net Delegate.
        delegate void UpdateConsoleHandler(string message);

        // Declare the AddAlarmToGridView as a .Net Delegate.
        delegate void AddAlarmToGridViewHandler(
            sikLib2.AlarmRecord alarm
            );

        // Declare the RemoveAlarmFromGridView as a .Net Delegate.
        delegate void RemoveAlarmFromGridViewHandler(
            UInt64 alarmId
            );

        // Declare the ModifyAlarmInGridView as a .Net Delegate.
        delegate void ModifyAlarmInGridViewHandler(
            sikLib2.AlarmRecord alarm
            );

        // Declare the ResetAppStatusHandler as a .Net Delegate.
        delegate void ResetAppStatusHandler();

        /// <summary>
        /// Initialise the IvBind library and our alarm record notification
        /// callbacks when the dialog is displayed for the first time.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void AlarmQueryDialog_Load(object sender, EventArgs e)
        {
            //
            // Initialise Binding Kit and configure event handlers
            //
            ivBind = new sikLib2.IvBind2();
            ivBind.OnAlarmRecordPrepopulate +=
                new sikLib2._IIvBind2Events_OnAlarmRecordPrepopulateEventHandler(
                    ivBind_OnAlarmRecordPrepopulate
                    );
            ivBind.OnAlarmRecordPrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnAlarmRecordPrepopulateCompleteEventHandler(
                    ivBind_OnAlarmRecordPrepopulateComplete
                    );
            ivBind.OnAlarmRecordNew +=
                new sikLib2._IIvBind2Events_OnAlarmRecordNewEventHandler(
                    ivBind_OnAlarmRecordNew
                    );
            ivBind.OnAlarmRecordUpdate +=
                new sikLib2._IIvBind2Events_OnAlarmRecordUpdateEventHandler(
                    ivBind_OnAlarmRecordUpdate
                    );
            ivBind.OnAlarmRecordDelete +=
                new sikLib2._IIvBind2Events_OnAlarmRecordDeleteEventHandler(
                    ivBind_OnAlarmRecordDelete
                    );
            ivBind.OnAlarmRecordRequestCancel +=
                new sikLib2._IIvBind2Events_OnAlarmRecordRequestCancelEventHandler(
                    ivBind_OnAlarmRecordRequestCancel
                    );
            ivBind.OnAlarmRecordServerDisconnect +=
                new sikLib2._IIvBind2Events_OnAlarmRecordServerDisconnectEventHandler(
                    ivBind_OnAlarmRecordServerDisconnect
                    );

            //
            // Initialise the interface
            // - disable the disconnect button
            // - set useful timestamps for min and max alarm raised time
            //
            disconnectButton.Enabled = false;
            DateTime currentLocalTime = DateTime.Now;
            minAlarmTimePicker.Value = currentLocalTime.AddDays(-7);
            maxAlarmTimePicker.Value = currentLocalTime.AddDays(7);
        }

        /// <summary>
        /// Utility method for displaying a message box
        /// </summary>
        /// <param name="text">text to be displayed</param>
        /// /// <param name="icon">icon to be used</param>
        private void ShowMessageBox(
            string text,
            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.Error
            )
        {
            MessageBox.Show(this, text, "Alarm Query Alert", MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Appends the passed String to the AlarmQueryForm console text box.
        /// </summary>
        /// <remarks>
        /// This function is used delegated to do so from the IvBind event handlers
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
        private void AlarmQueryDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ivBind != null && isQueryRunning)
                {
                    ivBind.StopAlarmRecordQuery(asIpAddressTextBox.Text.Trim());
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
        /// Add a new alarm record to the GridView.
        /// </summary>
        /// <param name="alarm">Alarm record details</param>
        private void AddAlarmToGridView(sikLib2.AlarmRecord alarm)
        {
            string alarmRecordId = ShowCorrectId(alarm.AlarmRecordId);
            string zoneId = ShowCorrectId(alarm.ZoneId);
            string state = alarm.StateDesc;
            string ownerId = ShowCorrectId(alarm.OwnerId);
            string timeRaised = ShowCorrectTime(alarm.TimeRaised);
            string timeCleared = ShowCorrectTime(alarm.TimeCleared);

            alarmRecordsGridView.Rows.Add(
                alarmRecordId,
                zoneId,
                state,
                ownerId,
                timeRaised,
                timeCleared
                );
        }

        /// <summary>
        /// Remove an existing alarm record from the grid view.
        /// </summary>
        /// <param name="alarmId">Id of the record to remove</param>
        private void RemoveAlarmFromGridView(
            UInt64 alarmId
            )
        {
            // Note that production code would require a better UI-data
            // lookup mechanism to scale with the number of alarms.
            foreach (DataGridViewRow row in alarmRecordsGridView.Rows)
            {
                try
                {
                    if (UInt64.Parse(row.Cells[0].Value.ToString())
                        == alarmId)
                    {
                        alarmRecordsGridView.Rows.Remove(row);
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
        /// Remove existing entry and add a new up to date one
        /// (at the bottom of the table)
        /// </summary>
        /// <param name="alarm">updated alarm record details</param>
        private void ModifyAlarmInGridView(
            sikLib2.AlarmRecord alarm
            )
        {
            RemoveAlarmFromGridView(alarm.AlarmRecordId);
            AddAlarmToGridView(alarm);
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordPrepopulate event. Adds
        /// new alarm to the grid view, without adding a message to the
        /// console.
        /// <remarks>
        /// This function will be called repeatedly at a very high rate
        /// as the query results are processed. As it creates work on
        /// the UI thread, we need to be careful to leave some idle time
        /// to keep the UI responsive.
        /// </remarks>
        /// </summary>
        /// <param name="alarmRec">the alarm record details</param>
        private void ivBind_OnAlarmRecordPrepopulate(
            sikLib2.AlarmRecord alarmRec
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
                // Delegate updating the GridView with the alarm
                this.BeginInvoke(
                    new AddAlarmToGridViewHandler(
                        AddAlarmToGridView
                        ),
                    new object[] { alarmRec }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordPrepopulateComplete event.
        /// Adds a message to the console to indicate reception of the
        /// notification.
        /// </summary>
        /// <param name="asIpAddr">IP address of the alarm server</param>
        private void ivBind_OnAlarmRecordPrepopulateComplete(string asIpAddr)
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Alarm record prepopulation completed from server "
                    + asIpAddr + "\r\n"
                    + "All historical alarm records have been sent." + "\r\n";

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
        /// Event handler for the OnAlarmRecordNew event. Adds
        /// message to console and adds the alarm record details to the
        /// grid view.
        /// </summary>
        /// <param name="alarmRec">Details of alarm record</param>
        private void ivBind_OnAlarmRecordNew(sikLib2.AlarmRecord alarmRec)
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Alarm record insertion notification" +
                    " received from " + alarmRec.ASIpAddress + "\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(UpdateConsole),
                    new object[] { message }
                    );

                // Delegate updating the GridView with the new alarm
                this.BeginInvoke(
                    new AddAlarmToGridViewHandler(
                        AddAlarmToGridView
                        ),
                    new object[] { alarmRec }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordUpdate event. Adds
        /// message to console and updates the alarm record details in the
        /// grid view.
        /// </summary>
        /// <param name="alarmRec">Details of the alarm record</param>
        private void ivBind_OnAlarmRecordUpdate(sikLib2.AlarmRecord alarmRec)
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Alarm record update notification " +
                    "received from " + alarmRec.ASIpAddress + "\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Delegate updating the GridView
                this.BeginInvoke(
                    new ModifyAlarmInGridViewHandler(
                        ModifyAlarmInGridView
                        ),
                    new object[] { alarmRec }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordDelete event. Adds
        /// a message to th console and removes the alarm from the
        /// grid view.
        /// </summary>
        /// <param name="alarmRec">Alarm record details</param>
        private void ivBind_OnAlarmRecordDelete(sikLib2.AlarmRecord alarmRec)
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Alarm record with ID " +
                    ShowCorrectId(alarmRec.AlarmRecordId) +
                    " has been deleted from server " +
                    alarmRec.ASIpAddress + "\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Delegate updating the GridView
                this.BeginInvoke(
                    new RemoveAlarmFromGridViewHandler(
                        RemoveAlarmFromGridView
                        ),
                    new object[] { alarmRec.AlarmRecordId }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordRequestCancel event.
        /// This adds a message to the console and clears the grid view.
        /// </summary>
        /// <param name="asIpAddr">IP address of the Alarm Server</param>
        private void ivBind_OnAlarmRecordRequestCancel(string asIpAddr)
        {
            if (!isQueryRunning)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string message = "Alarm query with server " + asIpAddr
                    + " has been canceled\r\n";

                // Delegate updating of the ConsoleTextBox
                this.BeginInvoke(
                    new UpdateConsoleHandler(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                // Reset App status to as ready for new monitoring
                this.BeginInvoke(new ResetAppStatusHandler(ResetAppStatus));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the OnAlarmRecordServerDisconnect event
        /// /// This adds a message to the console and clears the grid view.
        /// </summary>
        /// <param name="asIpAddr">IP address of the Alarm Server</param>
        private void ivBind_OnAlarmRecordServerDisconnect(string asIpAddr)
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

                // Reset App status to as ready for new monitoring
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
            // UI updates from the alarm record notifications could still
            // be on the message queue, (raised by the BeginInvoke function).
            Application.DoEvents();

            // Hint that garbage collector should run
            System.GC.Collect();

            isQueryRunning = false;
            asIpAddressTextBox.Enabled = true;
            queryButton.Enabled = true;
            disconnectButton.Enabled = false;
        }

        /// <summary>
        /// Handle click events on the query button. This will start the alarm
        /// record query.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear the console and alarm record grid views
                this.consoleTextBox.Clear();
                this.alarmRecordsGridView.Rows.Clear();

                //
                // Get the Query time parameters
                //
                DateTime minTime = DateTime.FromBinary(0);
                if (minAlarmTimePicker.Checked)
                {
                    minTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        minAlarmTimePicker.Value
                        );
                }
                DateTime maxTime = DateTime.FromBinary(0);
                if (maxAlarmTimePicker.Checked)
                {
                    maxTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        maxAlarmTimePicker.Value
                        );
                }

                // Start the query
                ivBind.QueryAlarmRecords(
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
                ivBind.StopAlarmRecordQuery(asIpAddressTextBox.Text.Trim());
                ResetAppStatus();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }
    }
}
