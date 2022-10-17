using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace IvDataRecordQuery
{
    /// <summary>
    /// A dialog that demonstrates use of the external data source and data
    /// record query functionality in the binding kit.
    /// </summary>
    public partial class DataRecordQueryDialog : Form
    {
        public DataRecordQueryDialog()
        {
            InitializeComponent();
        }

        // Use the IndigoVision Software Development Kit IvBind component.
        public sikLib2.IvBind2 ivBind;

        // Tracks whether the external data source query is running or not
        public bool sourceQueryRunning;

        // Tracks whether the data record query is running or not
        public bool recordQueryRunning;

        // Binding source used to store data for data record grid view
        public BindingSource dataRecordBindings = new BindingSource();

        // Delegate notify the the GUI thread of a message to be added to the
        // console
        delegate void UpdateConsoleDelegate(string message);

        // Delegate used when notifying the GUI thread of a prepopulated or
        // inserted external data source
        delegate void AddSourceDelegate(sikLib2.ExternalDataSource source);

        // Delegate used when notifying the GUI thread of an updated external
        // data source
        delegate void UpdateSourceDelegate(sikLib2.ExternalDataSource source);

        // Delegate used when notifying the GUI thread of a deleted external
        // data source
        delegate void DeleteSourceDelegate(UInt64 sourceId);

        // Delegate used when notifying the GUI thread of a prepopulated or
        // inserted data record
        delegate void AddDataRecordDelegate(sikLib2.DataRecord record);

        // Delegate used when notifying the GUI thread of a deleted data record
        delegate void DeleteDataRecordDelegate(UInt64 recordId);

        // Delegate used when notifying the GUI thread that it should reset the
        // form
        delegate void ResetFormDelegate();

        /// <summary>
        /// Class used to store items in the source list
        /// </summary>
        public class SourceListItem
        {
            public SourceListItem(sikLib2.ExternalDataSource source)
            {
                this.source = source;
            }

            public sikLib2.ExternalDataSource source;

            public override string ToString()
            {
                return source.DataSourceName;
            }
        }

        /// <summary>
        /// Class used to store items in the data record grid
        /// </summary>
        public class DataRecordGridItem
        {
            public DataRecordGridItem(
                sikLib2.DataRecord record,
                sikLib2.ExternalDataSource source
                )
            {
                this.record = record;
                this.source = source;
            }

            public sikLib2.DataRecord record;
            public sikLib2.ExternalDataSource source;

            public string Id
            {
                get
                {
                    if (record.DataRecordId == 0)
                    {
                        return "N/A";
                    }
                    else if (record.DataRecordId == UInt64.MaxValue)
                    {
                        return "Invalid";
                    }

                    return record.DataRecordId.ToString();
                }
            }

            public string Source
            {
                get
                {
                    if (source == null)
                    {
                        return "Unknown";
                    }
                    return source.DataSourceName;
                }
            }

            public string Time
            {
                get
                {
                    if (record.Time == DateTime.Parse("1/1/1970"))
                    {
                        return "N/A";
                    }

                    var localTime = TimeZone.CurrentTimeZone.ToLocalTime(record.Time);
                    return localTime.ToString();
                }
            }

            public string Data
            {
                get
                {
                    return record.Data;
                }
            }
        }

        /// <summary>
        /// Called when the dialog displays for the first time. Initialise the
        /// IvBind library, notification callbacks and GUI controls.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void DataRecordQueryDialog_Load(object sender, EventArgs e)
        {
            //
            // Initialise Binding Kit and configure event handlers
            //
            ivBind = new sikLib2.IvBind2();

            //
            // External data source events
            //
            ivBind.OnExternalDataSourcePrepopulate +=
                new sikLib2._IIvBind2Events_OnExternalDataSourcePrepopulateEventHandler(
                    ivBind_OnExternalDataSourcePrepopulate
                    );
            ivBind.OnExternalDataSourcePrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnExternalDataSourcePrepopulateCompleteEventHandler(
                    ivBind_OnExternalDataSourcePrepopulateComplete
                    );
            ivBind.OnExternalDataSourceNew +=
                new sikLib2._IIvBind2Events_OnExternalDataSourceNewEventHandler(
                    ivBind_OnExternalDataSourceNew
                    );
            ivBind.OnExternalDataSourceUpdate +=
                new sikLib2._IIvBind2Events_OnExternalDataSourceUpdateEventHandler(
                    ivBind_OnExternalDataSourceUpdate
                    );
            ivBind.OnExternalDataSourceDelete +=
                new sikLib2._IIvBind2Events_OnExternalDataSourceDeleteEventHandler(
                    ivBind_OnExternalDataSourceDelete
                    );
            ivBind.OnExternalDataSourceRequestCancel +=
                new sikLib2._IIvBind2Events_OnExternalDataSourceRequestCancelEventHandler(
                    ivBind_OnExternalDataSourceRequestCancel
                    );
            ivBind.OnExternalDataSourceServerDisconnect +=
                new sikLib2._IIvBind2Events_OnExternalDataSourceServerDisconnectEventHandler(
                    ivBind_OnExternalDataSourceServerDisconnect
                    );

            //
            // Data record events
            //
            ivBind.OnDataRecordPrepopulate +=
                new sikLib2._IIvBind2Events_OnDataRecordPrepopulateEventHandler(
                    ivBind_OnDataRecordPrepopulate
                    );
            ivBind.OnDataRecordPrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnDataRecordPrepopulateCompleteEventHandler(
                    ivBind_OnDataRecordPrepopulateComplete
                    );
            ivBind.OnDataRecordNew +=
                new sikLib2._IIvBind2Events_OnDataRecordNewEventHandler(
                    ivBind_OnDataRecordNew
                    );
            ivBind.OnDataRecordUpdate +=
                new sikLib2._IIvBind2Events_OnDataRecordUpdateEventHandler(
                    ivBind_OnDataRecordUpdate
                    );
            ivBind.OnDataRecordDelete +=
                new sikLib2._IIvBind2Events_OnDataRecordDeleteEventHandler(
                    ivBind_OnDataRecordDelete
                    );
            ivBind.OnDataRecordRequestCancel +=
                new sikLib2._IIvBind2Events_OnDataRecordRequestCancelEventHandler(
                    ivBind_OnDataRecordRequestCancel
                    );
            ivBind.OnDataRecordServerDisconnect +=
                new sikLib2._IIvBind2Events_OnDataRecordServerDisconnectEventHandler(
                    ivBind_OnDataRecordServerDisconnect
                    );

            //
            // Setup controls
            //
            DateTime currentTime = DateTime.Now;
            minTimePicker.Value = currentTime.AddDays(-7);
            maxTimePicker.Value = currentTime.AddDays(7);

            // Bind data record grid columns to properties on DataRecordGridItem
            // class
            dataRecordsGridView.DataSource = dataRecordBindings;
            dataRecordsGridView.Columns[0].DataPropertyName = "Id";
            dataRecordsGridView.Columns[1].DataPropertyName = "Source";
            dataRecordsGridView.Columns[2].DataPropertyName = "Time";
            dataRecordsGridView.Columns[3].DataPropertyName = "Data";

            EnableControls();
        }

        /// <summary>
        /// Called when the dialog is closed. Stops any active queries.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void DataRecordQueryDialog_FormClosing(
            object sender,
            FormClosingEventArgs e
            )
        {
            if (ivBind != null)
            {
                if (sourceQueryRunning)
                {
                    try
                    {
                        ivBind.StopExternalDataSourceQuery(
                            serverAddressTextBox.Text.Trim()
                            );
                    }
                    catch (Exception)
                    {
                    }
                }

                if (recordQueryRunning)
                {
                    try
                    {
                        ivBind.StopDataRecordQuery(
                            serverAddressTextBox.Text.Trim()
                            );
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Called when the connect button is clicked. Starts the external data
        /// source query and updates GUI controls to match new state.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void serverConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear console text box ready for messages relating to new
                // connection
                consoleTextBox.Clear();

                ivBind.QueryExternalDataSources(
                    serverAddressTextBox.Text.Trim()
                    );

                sourceQueryRunning = true;

                EnableControls();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Called when the disconnect button is clicked. Stops any running
        /// queries and updates GUI controls to match new state.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void serverDisconnectButton_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        /// <summary>
        /// Called when the start button is clicked. Starts the data record
        /// query and updates GUI controls to match new state.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void queryStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime minTime = DateTime.FromBinary(0);
                if (minTimePicker.Checked)
                {
                    minTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        minTimePicker.Value
                        );
                }

                DateTime maxTime = DateTime.FromBinary(0);
                if (maxTimePicker.Checked)
                {
                    maxTime = TimeZone.CurrentTimeZone.ToUniversalTime(
                        maxTimePicker.Value
                        );
                }

                List<UInt64> sourceList = new List<UInt64>();
                foreach (SourceListItem item in sourceSelectBox.CheckedItems)
                {
                    sourceList.Add(item.source.DataSourceId);
                }

                ivBind.QueryDataRecords(
                    serverAddressTextBox.Text.Trim(),
                    minTime,
                    maxTime,
                    sourceList.ToArray(),
                    dataFilterEdit.Text,
                    newRecordCheck.Checked
                    );

                recordQueryRunning = true;

                EnableControls();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Called when the stop button is clicked. Stops the data record query
        /// and updates GUI controls to match new state.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event details</param>
        private void queryStopButton_Click(object sender, EventArgs e)
        {
            try
            {
                recordQueryRunning = false;

                ivBind.StopDataRecordQuery(serverAddressTextBox.Text.Trim());

                dataRecordsGridView.Rows.Clear();
                EnableControls();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for OnExternalDataSourcePrepopulate event. Adds a
        /// message to the console to indicate reception of the notification and
        /// updates the source selection list.
        /// </summary>
        /// <remarks>
        /// This function will be called repeatedly at a very high rate as the
        /// query results are processed. As it creates work on the UI thread, we
        /// need to be careful to leave some idle time to keep the UI
        /// responsive.
        /// </remarks>
        /// <param name="source">The external data source details</param>
        private void ivBind_OnExternalDataSourcePrepopulate(
            sikLib2.ExternalDataSource source
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            // Keep the UI responsive when processing the query results
            // with a forced sleep for each notification.
            Thread.Sleep(10); // ms

            try
            {
                string message = "External data source prepopulation" +
                    " notification received from " + source.ASIpAddress +
                    "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new AddSourceDelegate(AddSource),
                    new object[] { source }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnExternalDataSourcePrepopulateComplete event.
        /// Adds a message to the console to indicate reception of the
        /// notification.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnExternalDataSourcePrepopulateComplete(
            string asIpAddress
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source prepopulation" +
                    "  completed from server " + asIpAddress + "\r\n"
                    + "All existing external data sources have been sent.\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
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
        /// Event handler for the OnExternalDataSourceNew event. Adds a message
        /// to the console to indicate reception of the notification and updates
        /// the source selection list.
        /// </summary>
        /// <param name="source">The external data source details</param>
        private void ivBind_OnExternalDataSourceNew(
            sikLib2.ExternalDataSource source
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source insertion notification" +
                    " received from " + source.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new AddSourceDelegate(AddSource),
                    new object[] { source }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnExternalDataSourceUpdate event. Adds a
        /// message to the console to indicate reception of the notification and
        /// updates the source selection list.
        /// </summary>
        /// <param name="source">The external data source details</param>
        private void ivBind_OnExternalDataSourceUpdate(
            sikLib2.ExternalDataSource source
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source update notification" +
                    " received from " + source.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new UpdateSourceDelegate(UpdateSource),
                    new object[] { source }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnExternalDataSourceDelete event. Adds a
        /// message to the console to indicate reception of the notification and
        /// updates the source selection list.
        /// </summary>
        /// <param name="source">The external data source details</param>
        private void ivBind_OnExternalDataSourceDelete(
            sikLib2.ExternalDataSource source
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source delete notification" +
                    " received from " + source.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new DeleteSourceDelegate(DeleteSource),
                    source.DataSourceId
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnExternalDataSourceRequestCancel event. This
        /// adds a message to the console and resets the dialog to its initial
        /// state.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnExternalDataSourceRequestCancel(
            string asIpAddress
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source query with server "
                    + asIpAddress + " has been cancelled\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(new ResetFormDelegate(ResetForm));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnExternalDataSourceServerDisconnect event.
        /// This adds a message to the console and resets the dialog to its
        /// initial state.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnExternalDataSourceServerDisconnect(
            string asIpAddress
            )
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "External data source connection to" +
                    " Alarm Server " + asIpAddress + " has been lost\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(new ResetFormDelegate(ResetForm));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for OnDataRecordPrepopulate event. Adds a message to
        /// the console to indicate reception of the notification and updates
        /// the data record grid.
        /// </summary>
        /// <remarks>
        /// This function will be called repeatedly at a very high rate as the
        /// query results are processed. As it creates work on the UI thread, we
        /// need to be careful to leave some idle time to keep the UI
        /// responsive.
        /// </remarks>
        /// <param name="record">The data record details</param>
        private void ivBind_OnDataRecordPrepopulate(sikLib2.DataRecord record)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            // Keep the UI responsive when processing the query results
            // with a forced sleep for each notification.
            Thread.Sleep(10); // ms

            try
            {
                string message = "Data record prepopulation notification" +
                    " received from " + record.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new AddDataRecordDelegate(AddDataRecord),
                    new object[] { record }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnDataRecordPrepopulateComplete event. Adds a
        /// message to the console to indicate reception of the notification.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnDataRecordPrepopulateComplete(string asIpAddress)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "Data record prepopulation completed from" +
                    " server " + asIpAddress + "\r\n"
                    + "All historic data records have been sent.\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
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
        /// Event handler for the OnDataRecordNew event. Adds a message to the
        /// console to indicate reception of the notification and updates the
        /// data record grid.
        /// </summary>
        /// <param name="record">The data record details</param>
        private void ivBind_OnDataRecordNew(sikLib2.DataRecord record)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "Data record insertion notification" +
                    " received from " + record.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new AddDataRecordDelegate(AddDataRecord),
                    new object[] { record }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnDataRecordUpdate event. Data records are
        /// never updated so this handler should never be called.
        /// </summary>
        /// <param name="record">The data record details</param>
        private void ivBind_OnDataRecordUpdate(sikLib2.DataRecord record)
        {
            Debug.Assert(false);
        }

        /// <summary>
        /// Event handler for the OnDataRecordDelete event. Adds a message to
        /// the console to indicate reception of the notification and updates
        /// the data record grid.
        /// </summary>
        /// <param name="record">The data record details</param>
        private void ivBind_OnDataRecordDelete(sikLib2.DataRecord record)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "Data record delete notification received"
                    + " from " + record.ASIpAddress + "\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(
                    new DeleteDataRecordDelegate(DeleteDataRecord),
                    record.DataRecordId
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnDataRecordRequestCancel event. This adds a
        /// message to the console and resets the dialog to its initial state.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnDataRecordRequestCancel(string asIpAddress)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "Data record query with server " + asIpAddress
                    + " has been cancelled\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(new ResetFormDelegate(ResetForm));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the OnDataRecordServerDisconnect event. This adds
        /// a message to the console and resets the dialog to its initial state.
        /// </summary>
        /// <param name="asIpAddress">IP address of the Alarm Server</param>
        private void ivBind_OnDataRecordServerDisconnect(string asIpAddress)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                string message = "Data record connection to Alarm Server "
                    + asIpAddress + " has been lost\r\n";

                BeginInvoke(
                    new UpdateConsoleDelegate(
                        UpdateConsole
                        ),
                    new object[] { message }
                    );

                BeginInvoke(new ResetFormDelegate(ResetForm));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Utility method for displaying a message box
        /// </summary>
        /// <param name="text">text to be displayed</param>
        /// <param name="icon">icon to be used</param>
        private void ShowMessageBox(
            string text,
            System.Windows.Forms.MessageBoxIcon icon =
                System.Windows.Forms.MessageBoxIcon.Error
            )
        {
            MessageBox.Show(this, text, "Alert", MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Utility method to reset the form back to its initial state. This
        /// will cause any running queries to be cancelled.
        /// </summary>
        private void ResetForm()
        {
            try
            {
                //
                // Stop any running queries
                //
                string serverIpAddr = serverAddressTextBox.Text.Trim();

                if (sourceQueryRunning)
                {
                    sourceQueryRunning = false;
                    ivBind.StopExternalDataSourceQuery(serverIpAddr);
                }

                if (recordQueryRunning)
                {
                    recordQueryRunning = false;
                    ivBind.StopDataRecordQuery(serverIpAddr);
                }

                //
                // Update controls
                //
                sourceSelectBox.Items.Clear();
                dataRecordsGridView.Rows.Clear();
                EnableControls();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Appends the passed String to the console text box.
        /// </summary>
        /// <param name="message">
        /// The text to append to the notification box
        /// </param>
        private void UpdateConsole(string message)
        {
            consoleTextBox.AppendText(message);
            consoleTextBox.AppendText("\r\n");
            consoleTextBox.Update();
            consoleTextBox.Refresh();
        }

        /// <summary>
        /// Utility method called when a new external data source entry is to
        /// be added. This adds the new source to the source selection list.
        /// </summary>
        /// <param name="source">The external data source details</param>
        private void AddSource(sikLib2.ExternalDataSource source)
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                sourceSelectBox.Items.Add(new SourceListItem(source));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Utility method called when an existing external data source entry is
        /// to be updated. This updates the entry for the source in the source
        /// selection list and updates any items in the data record grid that
        /// use the source.
        /// </summary>
        /// <param name="source">The external data source details</param>
        private void UpdateSource(sikLib2.ExternalDataSource source)
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                foreach (SourceListItem item in sourceSelectBox.Items)
                {
                    if (item.source.DataSourceId == source.DataSourceId)
                    {
                        item.source = source;
                        break; // Break as there should only be one match
                    }
                }

                foreach (DataRecordGridItem item in dataRecordBindings.List)
                {
                    if (item.source.DataSourceId == source.DataSourceId)
                    {
                        item.source = source;
                    }
                }

                // Force redraw of source select list and data record grid so
                // they reflect any changes
                sourceSelectBox.Refresh();
                dataRecordsGridView.Refresh();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Utility method called when an external data source entry is to
        /// be deleted. This removes the source from the source selection list
        /// and updates any items in the data record grid that use the source.
        /// </summary>
        /// <param name="sourceId">The external data source ID</param>
        private void DeleteSource(UInt64 sourceId)
        {
            if (!sourceQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                foreach (SourceListItem item in sourceSelectBox.Items)
                {
                    if (item.source.DataSourceId == sourceId)
                    {
                        sourceSelectBox.Items.Remove(item);
                        break; // Break as there should only be one match
                    }
                }

                foreach (DataRecordGridItem item in dataRecordBindings.List)
                {
                    if (item.source.DataSourceId == sourceId)
                    {
                        item.source = null;
                    }
                }

                // Force redraw of data record grid to reflect any changes
                dataRecordsGridView.Refresh();
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Insert new record into data grid ordered by the records ID.
        /// This needs to cope with the fact that during prepopulation
        /// records will be received in decreasing order of the records
        /// creation time which will generally be in decreasing order of
        /// ID. Where as when new records are created they will always be
        /// received in increasing ID order.
        /// </summary>
        /// <param name="record">The record details</param>
        /// <param name="source">The external data source details</param>
        private void InsertIntoGrid(sikLib2.DataRecord record,
            sikLib2.ExternalDataSource source)
        {
            if (dataRecordBindings.Count > 0)
            {
                DataRecordGridItem item = (DataRecordGridItem)
                    dataRecordBindings[dataRecordBindings.Count - 1];
                if (record.DataRecordId > item.record.DataRecordId)
                {
                    // The new records ID is greater than the last one
                    // currently in the list so just add it. During normal
                    // operation this will be the usual case.
                    dataRecordBindings.Add(
                        new DataRecordGridItem(record, source));
                }
                else
                {
                    // The new record ID needs to be inserted into the list
                    // at the correct location. During prepopulation the
                    // usual case will be for the new record to be inserted
                    // at the very start of the list.
                    for (int n = 0; n < dataRecordBindings.Count; ++n)
                    {
                        item = (DataRecordGridItem)dataRecordBindings[n];
                        if (record.DataRecordId <= item.record.DataRecordId)
                        {
                            dataRecordBindings.Insert(n,
                                new DataRecordGridItem(record, source));
                            break;
                        }
                    }
                }
            }
            else
            {
                // List is empty so just add new item
                dataRecordBindings.Add(
                    new DataRecordGridItem(record, source));
            }
        }

        /// <summary>
        /// Utility method called when a new data record entry is to be added.
        /// This adds an entry to the data record grid.
        /// </summary>
        /// <param name="record">The record details</param>
        private void AddDataRecord(sikLib2.DataRecord record)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            try
            {
                sikLib2.ExternalDataSource source = null;

                // Find the source object used by the data record
                foreach (SourceListItem item in sourceSelectBox.Items)
                {
                    if (item.source.DataSourceId == record.DataSourceId)
                    {
                        source = item.source;
                        break; // Break as there should only be one match
                    }
                }

                InsertIntoGrid(record, source);
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Utility method called when a data record entry is to be deleted.
        /// This removes the corresponding item from the data record grid.
        /// </summary>
        /// <param name="recordId">The data records ID</param>
        private void DeleteDataRecord(UInt64 recordId)
        {
            if (!recordQueryRunning)
            {
                return; // User has cancelled the query, drop the notification.
            }

            foreach (DataRecordGridItem item in dataRecordBindings.List)
            {
                if (item.record.DataRecordId == recordId)
                {
                    dataRecordBindings.Remove(item);
                    break; // Break as there should only be one match
                }
            }
        }

        /// <summary>
        /// Utility method to enable/disable controls so they match the current
        /// state.
        /// </summary>
        private void EnableControls()
        {
            serverAddressLabel.Enabled = !sourceQueryRunning;
            serverAddressTextBox.Enabled = !sourceQueryRunning;
            serverConnectButton.Enabled = !sourceQueryRunning;
            serverDisconnectButton.Enabled = sourceQueryRunning;
            minTimeLabel.Enabled = sourceQueryRunning && !recordQueryRunning;
            minTimePicker.Enabled = sourceQueryRunning && !recordQueryRunning;
            maxTimeLabel.Enabled = sourceQueryRunning && !recordQueryRunning;
            maxTimePicker.Enabled = sourceQueryRunning && !recordQueryRunning;
            sourceSelectLabel.Enabled =
                sourceQueryRunning && !recordQueryRunning;
            sourceSelectBox.Enabled = sourceQueryRunning && !recordQueryRunning;
            dataFilterLabel.Enabled = sourceQueryRunning && !recordQueryRunning;
            dataFilterEdit.Enabled = sourceQueryRunning && !recordQueryRunning;
            newRecordLabel.Enabled = sourceQueryRunning && !recordQueryRunning;
            newRecordCheck.Enabled = sourceQueryRunning && !recordQueryRunning;
            queryStartButton.Enabled =
                sourceQueryRunning && !recordQueryRunning;
            queryStopButton.Enabled = sourceQueryRunning && recordQueryRunning;
        }
    }
}
