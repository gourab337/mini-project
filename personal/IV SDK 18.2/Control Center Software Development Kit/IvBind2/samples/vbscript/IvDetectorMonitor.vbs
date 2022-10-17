'
' IvDetectorMonitor.vbs        Sample vbscript for the IvBind COM component.
'
' IvDetectorMonitor outputs detector notifications from specified detector server to the console.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Detector type constants
'
Const IVBIND2_DETECTORTYPE_UNKNOWN = 0
Const IVBIND2_DETECTORTYPE_BASICANALYTICS = 1
Const IVBIND2_DETECTORTYPE_DIGITALINPUT = 2
Const IVBIND2_DETECTORTYPE_EXTERNAL = 3
Const IVBIND2_DETECTORTYPE_NETWORKFAULT = 4
Const IVBIND2_DETECTORTYPE_VIDEOFAULT = 5
Const IVBIND2_DETECTORTYPE_DOUBLEKNOCK = 6
Const IVBIND2_DETECTORTYPE_UNHANDLEDALARM = 7
Const IVBIND2_DETECTORTYPE_DEVICEFAULT = 8
Const IVBIND2_DETECTORTYPE_NONE = 9
Const IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT = 10
Const IVBIND2_DETECTORTYPE_ONVIFEVENT = 11

'
' Extra info type constants
'
Const IVBIND2_DETECTOREXTRAINFOTYPE_NONE = 0
Const IVBIND2_DETECTOREXTRAINFOTYPE_UNKNOWN = 1
Const IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULTINFO = 2
Const IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON = 3
Const IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION = 4

'
' Nvr fault type constants
'
Const IVBIND2_NVRFAULTTYPE_UNKNOWN = 0
Const IVBIND2_NVRFAULTTYPE_RECORDINGFAILURE = 1
Const IVBIND2_NVRFAULTTYPE_LICENSEFAILURE = 2
Const IVBIND2_NVRFAULTTYPE_RAIDDEGRADED = 3
Const IVBIND2_NVRFAULTTYPE_REDUNDANTPOWERFAIL = 4
Const IVBIND2_NVRFAULTTYPE_REDUNDANTNETWORKFAIL = 5
Const IVBIND2_NVRFAULTTYPE_DEVICEOFFLINE = 6
Const IVBIND2_NVRFAULTTYPE_UPSONBATTERY = 7
Const IVBIND2_NVRFAULTTYPE_FANFAILURE = 8
Const IVBIND2_NVRFAULTTYPE_SYSTEMOVERTEMP = 9
Const IVBIND2_NVRFAULTTYPE_DISKOVERTEMP = 10
Const IVBIND2_NVRFAULTTYPE_STORAGEARRAYMONITORINGFAILURE = 11
Const IVBIND2_NVRFAULTTYPE_STORAGEARRAYDISKFAILURE = 12
Const IVBIND2_NVRFAULTTYPE_STORAGEARRAYREDUNDANCYFAILURE = 13
Const IVBIND2_NVRFAULTTYPE_STORAGEARRAYENCLOSUREFAILURE = 14
Const IVBIND2_NVRFAULTTYPE_LOWDISKSPACE = 15

Const IVBIND2_ONVIFEVENTTYPE_UNKNOWN           = 0
Const IVBIND2_ONVIFEVENTTYPE_BASICANALYTICS    = 1
Const IVBIND2_ONVIFEVENTTYPE_DIGITALINPUT      = 2
Const IVBIND2_ONVIFEVENTTYPE_ADVANCEDANALYTICS = 3
Const IVBIND2_ONVIFEVENTTYPE_CYBERVIGILANT     = 4

'
' Define usage display message for this script
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvDetectorMonitor.vbs <AS IP Address>" + vbCrLf _
    + "  e.g. CScript.exe IvDetectorMonitor.vbs 192.168.1.1 " + vbCrLf  + vbCrLf _
    + "       Monitor detectors in Alarm Server with IP address 192.168.1.1"

' Ensure that this script is being invoked using the CScript host executable
If InStr( LCase( wscript.FullName ), "wscript" ) > 0 Then
    WScript.Echo "Please run this script using " _
                + "the Console script engine CScript.exe" + vbCrLf _
                + vbCrLf + Usage + vbCrLf
    WScript.Quit( 1 )
End If

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2", "IvBind2_" )

'
' Parse the command-line arguments for IP addresses
'
If WScript.Arguments.Count <> 1 Then
    WScript.Echo Usage 
    WScript.Quit( 1 )
End If

On Error Resume Next
'
' Register with Detector Server for detector notifications
'
Dim nvrIpAddress
nvrIpAddress = WScript.Arguments( 0 )
Call ivbind.ReceiveDetectorNotifications( nvrIpAddress )
If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else
    WScript.Echo "Register detector notification successfully"
End If

'
' Wait for an Detector notification
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorPrepopulate event handler
'
' Outputs all properties of the prepopulated detector to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorPrepopulate( detector )
    On Error Resume Next
    Dim DetectorMessage

    '
    ' Obtain the detector server (NVR) properties
    '
    DetectorMessage = "Detector notification prepopulation received from server " _
        + detector.ASIpAddress +  vbCrLf
        
    '
    ' Obtain the detector source properties
    '
    DetectorMessage = DetectorMessage + GetDetectorMessage( detector )

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorPrepopulateComplete event handler
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorPrepopulateComplete( asIpAddr )
    On Error Resume Next
    Dim DetectorMessage
    
    DetectorMessage = "Detector prepopulation completed from server " _
        + asIpAddr + vbCrLf         

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorNew event handler
'
' Outputs all properties of the new detector to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorNew( detector )
    On Error Resume Next
    Dim DetectorMessage

    '
    ' Obtain the detector server (NVR) properties
    '
    DetectorMessage = "Detector notification insertion received from server " _
        + detector.ASIpAddress +  vbCrLf
        
    '
    ' Obtain the detector source properties
    '
    DetectorMessage = DetectorMessage + GetDetectorMessage( detector )

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorUpdate event handler
'
' Outputs all properties of the updated detector to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorUpdate( detector )
    On Error Resume Next
    Dim DetectorMessage

    '
    ' Obtain the detector server (NVR) properties
    '
    DetectorMessage = "Detector notification update received from server " _
        + detector.ASIpAddress +  vbCrLf
        
    '
    ' Obtain the detector source properties
    '
    DetectorMessage = DetectorMessage + GetDetectorMessage( detector )

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorDelete event handler
'
' Outputs all properties of the deleted detector to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorDelete( detector )
    On Error Resume Next
    Dim DetectorMessage

    With detector
        DetectorMessage = "Detector with Id = " + ShowCorrectId( .Id ) + _
            " is deleted from server " + detector.ASIpAddress + vbCrLf
    End With

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorRequestCancel event handler
'
' Outputs all properties of the cancelled detector to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorRequestCancel( asIpAddr )
    On Error Resume Next
    Dim DetectorMessage

    '
    ' Obtain the detector server (NVR) properties
    '
    DetectorMessage = "Detector notification is cancelled from server " _
        + asIpAddr + vbCrLf         

    WScript.Echo DetectorMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDetectorServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDetectorServerDisconnect( asIpAddr )
    WScript.Echo "The detector monitor is disconnected from server " _
        + asIpAddr + "."
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing a detector
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function GetDetectorMessage( detector )
    GetDetectorMessage = _
            "Detector details:" + vbCrLf + _
            " Id = " + ShowCorrectId(detector.Id) + vbCrLf + _
            " Name = " + detector.Name + vbCrLf + _
            " ZoneId = " + ShowCorrectId(detector.ZoneId) + vbCrLf + _
            " DwellTime = " + CStr( detector.DwellTime ) + vbCrLf + _
            " Alarmable = " + CStr( detector.Alarmable ) + vbCrLf + _
            " InAlarm = " + CStr( detector.InAlarm ) + vbCrLf + _
            " State = " + detector.StateDesc + vbCrLf + _
            " TimeActivated = " + ShowCorrectTime(detector.TimeActivated) + vbCrLf + _
            " ActivateSource: " + vbCrLf + _
            ShowSource(detector.ActivateSource) + _
            " DeactivateSource:" + vbCrLf + _
            ShowSource(detector.DeactivateSource) + _
            " ExtraInfo:" + vbCrLf + _
            ShowExtraInfo(detector.ExtraInfo)
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Check if the input timeValue is the default datetime
' If Yes, show N/A instead of timeValue
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowCorrectTime( timeValue )
    ShowCorrectTime = "N/A"
    timeValue = Trim( timeValue )
    If IsDate( timeValue ) then
        If CDate( timeValue ) = CDate( "1/1/1970" ) Then
            ShowCorrectTime = "N/A"
        Else
            ShowCorrectTime = CStr( timeValue )
        End If
    End If
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' If idValue is zero, show "N/A" instead
' If idValue is the max value of the datatype, show "Invalid" instead
' Notice that the datatype for IDs is 64-bits unsigned integral type, but
' vbscript does not support it.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowCorrectId( idValue )
    Const ZERO_ID_STR = "0"
    Const INVALID_ID_STR = "18446744073709551615" '&HFFFFFFFFFFFFFFFF

    Dim idStrValue
    idStrValue = CStr( idValue )

    If idStrValue = ZERO_ID_STR Then
        ShowCorrectId = "N/A"
    ElseIf idStrValue = INVALID_ID_STR Then
        ShowCorrectId = "Invalid"
    Else
        ShowCorrectId = idStrValue
    End If
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Show information from Clear/Trigger Source
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowSource( sourceInfo )
    dim returnValue

    returnValue = _
            "  Type: " + DetectorTypeToString(sourceInfo.Type) + vbCrLf

    Select Case sourceInfo.Type
        Case IVBIND2_DETECTORTYPE_DIGITALINPUT
            returnValue = returnValue + _
                "  IP Address: " + sourceInfo.IpAddress + vbCrLf + _
                "  Pin Number: " + CStr( sourceInfo.Input ) + vbCrLf + _
                "  Normal PinState: " + CStr( sourceInfo.PinState ) + vbCrLf
        Case IVBIND2_DETECTORTYPE_BASICANALYTICS
            returnValue = returnValue + _
                "  IP Address: " + sourceInfo.IpAddress + vbCrLf
        Case IVBIND2_DETECTORTYPE_EXTERNAL
            returnValue = returnValue + _
                "  External System IP Address: " + sourceInfo.IpAddress + vbCrLf + _
                "  External Input number: " + CStr( sourceInfo.Input ) + vbCrLf
        Case IVBIND2_DETECTORTYPE_NETWORKFAULT
            returnValue = returnValue + _
                "  IP Address: " + sourceInfo.IpAddress + vbCrLf
        Case IVBIND2_DETECTORTYPE_VIDEOFAULT
            returnValue = returnValue + _
                "  IP Address: " + sourceInfo.IpAddress + vbCrLf
        Case IVBIND2_DETECTORTYPE_DOUBLEKNOCK:
            returnValue = returnValue + _
                "  First Detector Id: " + CStr( sourceInfo.FirstDetectorId ) + vbCrLf + _
                "  Second Detector Id: " + CStr( sourceInfo.SecondDetectorId ) + vbCrLf
        Case IVBIND2_DETECTORTYPE_UNHANDLEDALARM
            returnValue = returnValue + _
                "  Zone Id: " + CStr( sourceInfo.ZoneId ) + vbCrLf + _
                "  Timeout: " + CStr( sourceInfo.Timeout ) + "ms" + vbCrLf
        Case IVBIND2_DETECTORTYPE_DEVICEFAULT
            returnValue = returnValue + _
                "  IP Address: " + sourceInfo.IpAddress + vbCrLf
        Case IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT
            returnValue = returnValue + _
                "  Service Id: " + sourceInfo.ServiceId + vbCrLf + _
                "  Access URL: " + sourceInfo.AccessUrl + vbCrLf
        Case IVBIND2_DETECTORTYPE_ONVIFEVENT
            returnValue = returnValue + _
                "  Detector Service Id: " + sourceInfo.ServiceId + vbCrLf + _
                "  Event Detector Type: " + _
                OnvifEventDetectorTypeToString( sourceInfo.EventType ) + vbCrLf
    End Select

    ShowSource = returnValue
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Convert Onvif Event Detector type to a string
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function OnvifEventDetectorTypeToString (nType)
    Select Case nType
         Case IVBIND2_ONVIFEVENTTYPE_UNKNOWN
             OnvifEventDetectorTypeToString = "Unknown"
         Case IVBIND2_ONVIFEVENTTYPE_BASICANALYTICS
             OnvifEventDetectorTypeToString = "Basic Analytics"
         Case IVBIND2_ONVIFEVENTTYPE_DIGITALINPUT
             OnvifEventDetectorTypeToString = "Digital Input"
         Case IVBIND2_ONVIFEVENTTYPE_ADVANCEDANALYTICS
             OnvifEventDetectorTypeToString = "Advanced Analytics"
         Case IVBIND2_ONVIFEVENTTYPE_CYBERVIGILANT
             OnvifEventDetectorTypeToString = "CyberVigilant"
    End Select
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Convert Detector type to a string
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function DetectorTypeToString( nType )
    Select Case nType
        Case IVBIND2_DETECTORTYPE_BASICANALYTICS
            DetectorTypeToString = "Basic Analytics"

        Case IVBIND2_DETECTORTYPE_DIGITALINPUT
            DetectorTypeToString = "Digital Input"

        Case IVBIND2_DETECTORTYPE_EXTERNAL
            DetectorTypeToString = "External"

        Case IVBIND2_DETECTORTYPE_NETWORKFAULT
            DetectorTypeToString = "Network Fault"

        Case IVBIND2_DETECTORTYPE_VIDEOFAULT
            DetectorTypeToString = "Video Fault"

        Case IVBIND2_DETECTORTYPE_DOUBLEKNOCK
            DetectorTypeToString = "Double Knock"

        Case IVBIND2_DETECTORTYPE_UNHANDLEDALARM
            DetectorTypeToString = "Unhandled Alarm"

        Case IVBIND2_DETECTORTYPE_NONE
            DetectorTypeToString = "None"

        Case IVBIND2_DETECTORTYPE_DEVICEFAULT
            DetectorTypeToString = "Device Fault"

        Case IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT
            DetectorTypeToString = "ONVIF Network Fault"

        Case IVBIND2_DETECTORTYPE_ONVIFEVENT
            DetectorTypeToString = "ONVIF Event"

        Case Else
            DetectorTypeToString = "Unknown type"

    End Select
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing the extra info passed in
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowExtraInfo( extraInfo )
    dim returnValue, index

    returnValue = "  Type: " + ExtraInfoTypeToString( extraInfo.Type ) + vbCrLf
    
    Select Case extraInfo.Type
        case IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULTINFO
            returnValue = returnValue + "  Number Faults: " + _
                CStr( extraInfo.NumFaults ) + vbCrLf
            For index = 0 to ( extraInfo.NumFaults - 1 ) 
                returnValue = returnValue + "   Fault " + CStr( index ) + _
                    ": " + FaultTypeToString( extraInfo.GetFaultType( index ) ) + _
                    vbCrLf
            Next
        case IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON
            returnValue = returnValue + " Isolate Reason: " + extraInfo.IsolateReason
        case IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION
            returnValue = returnValue + "  Activation Annotation: " + extraInfo.AnnotationMsg
    End Select

    ShowExtraInfo = returnValue
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing an extra info type
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ExtraInfoTypeToString( nType )
    Select Case nType
        Case IVBIND2_DETECTOREXTRAINFOTYPE_NONE
            ExtraInfoTypeToString = "No Extra Info"
        Case IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULTINFO
            ExtraInfoTypeToString = "Nvr Fault Info"
        Case IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON
            ExtraInfoTypeToString = "Isolate Reason"
        Case IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION
            ExtraInfoTypeToString = "Activation Annotation"
        Case Else
            ExtraInfoTypeToString = "Unknown Extra Info"
    End Select
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing a fault type
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function FaultTypeToString( nType )
    Select Case nType
        Case IVBIND2_NVRFAULTTYPE_RECORDINGFAILURE
            FaultTypeToString = "Recording Failure"
        Case IVBIND2_NVRFAULTTYPE_LICENSEFAILURE
            FaultTypeToString = "License Failure"
        Case IVBIND2_NVRFAULTTYPE_RAIDDEGRADED
            FaultTypeToString = "Raid Degraded"
        Case IVBIND2_NVRFAULTTYPE_REDUNDANTPOWERFAIL
            FaultTypeToString = "Redundant Power Failure"
        Case IVBIND2_NVRFAULTTYPE_REDUNDANTNETWORKFAIL
            FaultTypeToString = "Redundant Network Failure"
        Case IVBIND2_NVRFAULTTYPE_DEVICEOFFLINE
            FaultTypeToString = "Device Offline"
        Case IVBIND2_NVRFAULTTYPE_UPSONBATTERY
            FaultTypeToString = "UPS On Battery"
        Case IVBIND2_NVRFAULTTYPE_FANFAILURE
            FaultTypeToString = "Fan Failure"
        Case IVBIND2_NVRFAULTTYPE_SYSTEMOVERTEMP
            FaultTypeToString = "System Over Temperature"
        Case IVBIND2_NVRFAULTTYPE_DISKOVERTEMP
            FaultTypeToString = "Disk Over Temperature"
        Case IVBIND2_NVRFAULTTYPE_STORAGEARRAYMONITORINGFAILURE
            FaultTypeToString = "Storage Array Monitoring Failure"
        Case IVBIND2_NVRFAULTTYPE_STORAGEARRAYDISKFAILURE
            FaultTypeToString = "Storage Array Disk Failure"
        Case IVBIND2_NVRFAULTTYPE_STORAGEARRAYREDUNDANCYFAILURE
            FaultTypeToString = "Storage Array Redundancy Failure"
        Case IVBIND2_NVRFAULTTYPE_STORAGEARRAYENCLOSUREFAILURE
            FaultTypeToString = "Storage Array Enclosure Failure"
        Case IVBIND2_NVRFAULTTYPE_LOWDISKSPACE
            FaultTypeToString = "Disk Space Too Low"
        Case Else
            FaultTypeToString = "Unknown"
    End Select
End Function
