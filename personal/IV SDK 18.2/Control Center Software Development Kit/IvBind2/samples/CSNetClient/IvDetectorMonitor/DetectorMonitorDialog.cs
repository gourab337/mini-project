///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// DetectorMonitorDialog
//
// This class implements a dialog which monitors IndigoVision Alarm
// notifications.
//
///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

using System;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace IvDetectorMonitor
{
    public partial class DetectorMonitorDialog : Form
    {
        public DetectorMonitorDialog()
        {
            InitializeComponent();
        }

        readonly static DateTime DATE_1970 = DateTime.Parse("1/1/1970");

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
        // DetectorMonitorDialog_Load
        //
        // Registers for alarm notifications with the NVR specified in the IP Address
        // text box and adds the IP address to the AlarmServers List Box.
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void DetectorMonitorDialog_Load(object sender, EventArgs e)
        {
            ivBind = new sikLib2.IvBind2();
            ivBind.OnDetectorPrepopulate += 
                new sikLib2._IIvBind2Events_OnDetectorPrepopulateEventHandler(
                                ivBind_OnDetectorPrepopulate
                                );
            ivBind.OnDetectorPrepopulateComplete +=
                new sikLib2._IIvBind2Events_OnDetectorPrepopulateCompleteEventHandler(
                                ivBind_OnDetectorPrepopulateComplete
                                );
            ivBind.OnDetectorNew += 
                new sikLib2._IIvBind2Events_OnDetectorNewEventHandler(ivBind_OnDetectorNew);
            ivBind.OnDetectorUpdate += 
                new sikLib2._IIvBind2Events_OnDetectorUpdateEventHandler(ivBind_OnDetectorUpdate);
            ivBind.OnDetectorDelete += 
                new sikLib2._IIvBind2Events_OnDetectorDeleteEventHandler(ivBind_OnDetectorDelete);
            ivBind.OnDetectorRequestCancel += 
                new sikLib2._IIvBind2Events_OnDetectorRequestCancelEventHandler(
                                ivBind_OnDetectorRequestCancel
                                );
            ivBind.OnDetectorServerDisconnect += 
                new sikLib2._IIvBind2Events_OnDetectorServerDisconnectEventHandler(
                                ivBind_OnDetectorServerDisconnect
                                );
            disconnectButton.Enabled = false;
        }
        
        private void DetectorMonitorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ivBind != null && isMonitoring)
                {
                    ivBind.StopDetectorNotifications(asIpAddressTextBox.Text.Trim());
                }
            }
            catch (Exception)
            {
            }
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

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Show correct time value
        //
        // If TimeValue equal default value (1/1/1970), we must show N/A instead
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private string ShowCorrectTime(DateTime InputTime)
        {
            string returnValue;
            if (InputTime == DATE_1970)
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
        // Show correct detector ID
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
        // Get string describing a detector
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private string GetDetectorMessage(sikLib2.detector detector)
        {
            return
                    "Detector details:" + "\r\n" +
                    " Id = " + ShowCorrectId(detector.Id) + "\r\n" +
                    " Name = " + detector.Name + "\r\n" +
                    " ZoneId = " + ShowCorrectId(detector.ZoneId) + "\r\n" +
                    " DwellTime = " + detector.DwellTime.ToString() + "\r\n" +
                    " Alarmable = " + detector.Alarmable.ToString() + "\r\n" +
                    " InAlarm = " + detector.InAlarm.ToString() + "\r\n" +
                    " State = " + detector.StateDesc + "\r\n" +
                    " TimeActivated = " + ShowCorrectTime(detector.TimeActivated) + "\r\n" +
                    " ActivateSource: " + "\r\n" +
                    ShowSource(detector.ActivateSource) +
                    " DeactivateSource:" + "\r\n" +
                    ShowSource(detector.DeactivateSource) +
                    " ExtraInfo: " + "\r\n" +
                    ShowExtraInfo(detector.ExtraInfo);
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorPrepopulate event handler
        //
        // Outputs all properties of the prepopulated detector to the console
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorPrepopulate(sikLib2.detector detector)
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
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //
                DetectorMessage = "Detector notification prepopulation received from server " + 
                                    detector.ASIpAddress + "\r\n";

                //
                // Obtain the detector source properties
                //
                DetectorMessage =
                    DetectorMessage + GetDetectorMessage(detector);

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorPrepopulateComplete event handler
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorPrepopulateComplete(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                DetectorMessage = "Detector prepopulation completed from server " +
                                    asIpAddr + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(
                    new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox),
                    new object[] { DetectorMessage }
                    );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorNew event handler
        //
        // Outputs all properties of the new detector to the console
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorNew(sikLib2.detector detector)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //
                DetectorMessage = "Detector notification insertion received from server " + 
                                    detector.ASIpAddress + "\r\n";

                //
                // Obtain the detector source properties
                //                
                DetectorMessage =
                    DetectorMessage + GetDetectorMessage(detector);

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorUpdate event handler
        //
        // Outputs all properties of the updated detector to the console
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorUpdate(sikLib2.detector detector)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //
                DetectorMessage = "Detector notification update received from server " + 
                                    detector.ASIpAddress + "\r\n";

                //
                // Obtain the detector source properties
                //                
                DetectorMessage =
                    DetectorMessage + GetDetectorMessage(detector);

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorDelete event handler
        //
        // Outputs all properties of the deleted detector to the console
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorDelete(sikLib2.detector detector)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //                
                DetectorMessage = "Detector with Id = " + ShowCorrectId(detector.Id) + 
                                  " is deleted from server " + detector.ASIpAddress + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
                        );
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // OnDetectorCancel event handler
        //
        // Outputs all properties of the cancelled detector to the console
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorRequestCancel(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //
                DetectorMessage = "Detector notification is cancelled from server " + 
                                    asIpAddr + "\r\n";

                //
                // Delegate updating of the NotificationsTextBox
                //
                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
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
        // OnDetectorDisconnect event handler
        //
        // Outputs a disconnected message, then exit the monitor.
        //
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void ivBind_OnDetectorServerDisconnect(string asIpAddr)
        {
            if (!isMonitoring)
            {
                // User has canceled the query, drop the notification.
                return;
            }

            try
            {
                string DetectorMessage;

                //
                // Obtain the detector server (NVR) properties
                //
                DetectorMessage = "The detector monitor is disconnected from server " + asIpAddr + ".";

                this.BeginInvoke(new UpdateNotificationsTextBoxHandler(UpdateNotificationsTextBox), 
                        new object[] { DetectorMessage }
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
        // Show information from Clear/Trigger Source
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private object ShowSource(sikLib2.DetectorSourceBase sourceInfo)
        {
            string returnValue;

            returnValue =
                    "  Type: " + DetectorTypeToString(sourceInfo.Type) + "\r\n";

            switch ( sourceInfo.Type )
            {
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DIGITALINPUT:
                    sikLib2.DigitalInputSource physSource = (sikLib2.DigitalInputSource )sourceInfo;
                    returnValue +=
                        "  IP Address: " + physSource.IpAddress + "\r\n" +
                        "  Pin Number: " + physSource.Input.ToString() + "\r\n" +
                        "  Normal PinState: " + physSource.PinState.ToString() + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_BASICANALYTICS:
                    sikLib2.BasicAnalyticsSource analyticsSource = (sikLib2.BasicAnalyticsSource)sourceInfo;
                    returnValue += "  IP Address: " + analyticsSource.IpAddress + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_EXTERNAL:
                    sikLib2.ExternalSource externalSource = (sikLib2.ExternalSource)sourceInfo;
                    returnValue +=
                        "  External System IP Address: " + externalSource.IpAddress + "\r\n" +
                        "  External Input number: " + externalSource.Input.ToString() + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_NETWORKFAULT:
                    sikLib2.NetworkFaultSource networkFaultSource = (sikLib2.NetworkFaultSource)sourceInfo;
                    returnValue +=
                        "  IP Address: " + networkFaultSource.IpAddress + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_VIDEOFAULT:
                    sikLib2.VideoFaultSource videoFaultSource = (sikLib2.VideoFaultSource)sourceInfo;
                    returnValue +=
                        "  IP Address: " + videoFaultSource.IpAddress + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DOUBLEKNOCK:
                    sikLib2.DoubleKnockSource logicalAndSource = (sikLib2.DoubleKnockSource)sourceInfo;
                    returnValue +=
                        "  First Detector Id: " + logicalAndSource.FirstDetectorId + "\r\n" +
                        "  Second Detector Id: " + logicalAndSource.SecondDetectorId + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_UNHANDLEDALARM:
                    sikLib2.UnhandledAlarmSource alarmSource = (sikLib2.UnhandledAlarmSource)sourceInfo;
                    returnValue +=
                        "  Zone Id: " + alarmSource.ZoneId + "\r\n" +
                        "  Timeout: " + alarmSource.Timeout + "ms\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DEVICEFAULT:
                    sikLib2.DeviceFaultSource deviceFaultSource = (sikLib2.DeviceFaultSource)sourceInfo;
                    returnValue +=
                        "  IP Address: " + deviceFaultSource.IpAddress + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_ONVIFNETWORKFAULT:
                    sikLib2.OnvifNetworkFaultSource onvifNetworkFaultSource = (sikLib2.OnvifNetworkFaultSource)sourceInfo;
                    returnValue +=
                        "  Service Id: " + onvifNetworkFaultSource.ServiceId + "\r\n";
                    break;
                case sikLib2.DetectorType.SIK_DETECTORTYPE_ONVIFEVENT:
                    sikLib2.OnvifEventSource onvifEventSource = (sikLib2.OnvifEventSource)sourceInfo;
                    returnValue +=
                        "  Service Id: " + onvifEventSource.ServiceId + "\r\n" +
                        "  Event Type: ";
                    switch ( onvifEventSource.EventType )
                    {
                        case sikLib2.OnvifEventType.SIK_ONVIFEVENTTYPE_UNKNOWN:
                            returnValue += "Unknown";
                            break;
                        case sikLib2.OnvifEventType.SIK_ONVIFEVENTTYPE_BASICANALYTICS:
                            returnValue += "Basic Analytics";
                            break;
                        case sikLib2.OnvifEventType.SIK_ONVIFEVENTTYPE_DIGITALINPUT:
                            returnValue += "Digital Input";
                            break;
                        case sikLib2.OnvifEventType.SIK_ONVIFEVENTTYPE_ADVANCEDANALYTICS:
                            returnValue += "Advanced Analytics";
                            break;
                        case sikLib2.OnvifEventType.SIK_ONVIFEVENTTYPE_CYBERVIGILANT:
                            returnValue += "CyberVigilant";
                            break;
                    }
                    returnValue += "\r\n";
                    break;
            }
            return returnValue;
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Get a string describing the extra info passed in
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private string ShowExtraInfo(sikLib2.DetectorExtraInfoBase extraInfo)
        {
            string returnValue;

            returnValue =
                    "  Type: " + ExtraInfoTypeToString(extraInfo.Type) + "\r\n";

            switch (extraInfo.Type)
            {
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_NVRFAULTINFO:
                    sikLib2.NvrFaultInfo faultInfo = (sikLib2.NvrFaultInfo)extraInfo;
                    returnValue += "  Number Faults: " + faultInfo.NumFaults + "\r\n";
                    for (int i = 0; i < faultInfo.NumFaults; ++i)
                    {
                        returnValue += "   Fault " + i + ": " + 
                            FaultTypeToString( faultInfo.GetFaultType( i ) ) + "\r\n";
                    }
                    break;
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_ISOLATEREASON:
                    sikLib2.IsolationAnnotationInfo isolationInfo = (sikLib2.IsolationAnnotationInfo)extraInfo;
                    returnValue += "   " + isolationInfo.IsolateReason + "\r\n";
                    break;

                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION:
                    sikLib2.ActivationAnnotationInfo annotationInfo =
                        (sikLib2.ActivationAnnotationInfo)extraInfo;
                    returnValue += "   " + annotationInfo.AnnotationMsg + "\r\n";
                    break;
   }
   return returnValue;
        }

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Convert Detector type to a string
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private object DetectorTypeToString(sikLib2.DetectorType type)
        {
            switch (type)
            {
                case sikLib2.DetectorType.SIK_DETECTORTYPE_NONE:
                    return "None";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DIGITALINPUT:
                    return "Digital Input";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_BASICANALYTICS:
                    return "Basic Analytics";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_EXTERNAL:
                    return "External";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_NETWORKFAULT:
                    return "Network Fault";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_VIDEOFAULT:
                    return "Video Fault";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DOUBLEKNOCK:
                    return "Double Knock";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_UNHANDLEDALARM:
                    return "Unhandled Alarm";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_DEVICEFAULT:
                    return "Device Fault";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_ONVIFNETWORKFAULT:
                    return "ONVIF Network Fault";
                case sikLib2.DetectorType.SIK_DETECTORTYPE_ONVIFEVENT:
                    return "ONVIF Event";
                default:
                    return "Unknown";
            }
        }

        private string ExtraInfoTypeToString(sikLib2.DetectorExtraInfoType type)
        {
            switch (type)
            {
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_NONE:
                    return "None";
                default:
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_UNKNOWN:
                    return "Unknown";
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_NVRFAULTINFO:
                    return "Nvr Fault Info";
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_ISOLATEREASON:
                    return "Isolation Reason";
                case sikLib2.DetectorExtraInfoType.SIK_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION:
                    return "Activation Annotation";
   }
        }

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
                    return "Disk space too low";
                }
        }

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // UpdateNotificationsTextBox
        //
        // Appends the passed String to the DetectorMonitorForm alarm notifications
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

        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // Button: Monitor Alarm Server
        //
        // Registers for detector notifications with the NVR specified in the IP Address
        // text box
        //
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        private void monitorButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dnaProxyIPInput.Text.Trim() != "")
                {
                    ivBind.SetDnaProxy(dnaProxyIPInput.Text.Trim(), (ushort)dnaProxyPortInput.Value);
                }

                ivBind.ReceiveDetectorNotifications(asIpAddressTextBox.Text.Trim());
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
                ivBind.StopDetectorNotifications(asIpAddressTextBox.Text);

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
