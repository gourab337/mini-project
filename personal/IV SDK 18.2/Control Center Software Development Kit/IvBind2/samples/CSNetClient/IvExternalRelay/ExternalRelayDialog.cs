///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// ExternalRelayDialog
//
// This class implements a dialog box which allows you to run an external
// relay and outputs any incoming pin state changes to a text box
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IvExternalRelay
{
    public partial class ExternalRelayDialog : Form
    {
        public ExternalRelayDialog()
        {
            InitializeComponent();

            listMutex = new Mutex();
            stateChangeList = new List< StateChangeListEntry >();
        }

        private struct StateChangeListEntry
        {
            public String  remoteIp;
            public int     pinNumber;
            public bool    pinSet;
        }

        sikLib2.IvBind2             ivbind;
        bool                        relayRunning;

        Thread                      updateThread;
        Mutex                       listMutex;
        List<StateChangeListEntry>  stateChangeList;

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ExternalRelayDialog_Load
        //
        // Creates IvBind instance and sets up our event handlers
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ExternalRelayDialog_Load(object sender, EventArgs e)
        {
            ivbind = new sikLib2.IvBind2();
            ivbind.OnRelayStateChange +=
                new sikLib2._IIvBind2Events_OnRelayStateChangeEventHandler(
                    OnRelayStateChange
                    );
            stopRelayButton.Enabled = false;
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // FormClosing event handler
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ExternalRelayDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopRelay();
            }
            catch (System.Exception)
            {

            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Start Relay Button Click event handler
        //
        // Starts the relay if it's not already running
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void startRelayButton_Click(object sender, EventArgs e)
        {
            try
            {
                StartRelay();
            }
            catch ( System.Exception ex )
            {
                MessageBox.Show(
                    this,
                    "Error starting relay: " + ex.ToString(),
                    "Error"
                    ); 
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Stop Relay Button Click event handler
        //
        // Stops the relay if it's running
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void stopRelayButton_Click(object sender, EventArgs e)
        {
            try
            {
                StopRelay();
            }
            catch ( System.Exception ex )
            {
                MessageBox.Show(
                    this,
                    "Error stopping relay: " + ex.ToString(),
                    "Error"
                    ); 
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Starts the external relay & UI update thread running
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void StartRelay()
        {
            if (ivbind != null && !relayRunning)
            {
                updateThread = new Thread(
                    new ThreadStart(this.UpdateTextBoxThreadFunc)
                    );
                
                updateThread.Start();

                ivbind.ReceiveExternalRelayNotifications(
                    localIpAddressTextBox.Text
                    );

                relayRunning = true;
                startRelayButton.Enabled = false;
                stopRelayButton.Enabled = true;
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Stops the external relay & UI update thread
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void StopRelay()
        {
            if (ivbind != null && relayRunning)
            {
                ivbind.StopExternalRelayNotifications();
                relayRunning = false;
                startRelayButton.Enabled = true;
                stopRelayButton.Enabled = false;

                updateThread.Abort();
                updateThread.Join();
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnRelayStateChange event handler
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void OnRelayStateChange(ref sikLib2.PinState pinState)
        {
            if (pinState == null)
            {
                return;
            }
            try
            {
                listMutex.WaitOne();
                stateChangeList.Add(
                    new StateChangeListEntry
                    {
                        pinNumber = pinState.PinNumber,
                        pinSet = pinState.State,
                        remoteIp = pinState.RemoteIP
                    }
                    );
            }
            finally
            {
                listMutex.ReleaseMutex();
            }
        }

        delegate void UpdateTextBoxHandler(String newContents);

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Runs in a separate thread, and builds up the contents
        // of the externalRelayTextBox
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void UpdateTextBoxThreadFunc()
        {
            while (true)
            {
                Thread.Sleep(1000);
                List<StateChangeListEntry> ourList;
                List<StateChangeListEntry> newList = 
                    new List<StateChangeListEntry>();
                try
                {
                    listMutex.WaitOne();
                    if (stateChangeList.Count == 0)
                    {
                        continue;
                    }
                    ourList = stateChangeList;
                    stateChangeList = newList;
                }
                finally
                {
                    listMutex.ReleaseMutex();
                }
                StringBuilder builder =
                        new StringBuilder(externalRelayTextBox.Text);
                foreach (StateChangeListEntry entry in ourList)
                {
                    builder.Append("Received Message From ");
                    builder.Append(entry.remoteIp);
                    builder.Append(": Pin ");
                    builder.Append(entry.pinNumber.ToString());
                    if (entry.pinSet)
                    {
                        builder.Append(", Set\r\n");
                    }
                    else
                    {
                        builder.Append(", Unset\r\n");
                    }
                }
                UpdateTextBoxHandler textBoxHandler = DoUpdateTextBox;
                Object[] args = { builder.ToString() };
                this.BeginInvoke( textBoxHandler, args );
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Updates the contents of the text box with the string generated
        // on the separate thread.
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void DoUpdateTextBox(String newContents)
        {
            externalRelayTextBox.Text = newContents;
        }
    }
}
