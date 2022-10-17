'
' IvZoneMonitor.vbs        Sample vbscript for the IvBind COM component.
'
' IvZoneMonitor outputs zone notifications from specified zone server to the console.  
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvZoneMonitor.vbs <AS IP Address>" + vbCrLf + vbCrLf _
    + "  e.g. CScript.exe IvZoneMonitor.vbs 192.168.1.1 " + vbCrLf _
    + "       Monitor zones in Alarm Server with IP address 192.168.1.1 " 
  
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
' Register with Zone Server for zone notifications
'
Dim nvrIpAddress
nvrIpAddress = WScript.Arguments( 0 )
Call ivbind.ReceiveZoneNotifications( nvrIpAddress )
If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else	
    WScript.Echo "Register zone notification successfully"
End If
    
'
' Wait for an Zone notification
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZonePrepopulate event handler
'
' Outputs all properties of the prepopulated zone to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZonePrepopulate( zone )
    On Error Resume Next
    Dim ZoneMessage

    '
    ' Obtain the zone server (NVR) properties
    '
    ZoneMessage = "Zone notification prepopulation received from server " _
        + zone.ASIpAddress +  vbCrLf _
        
    '
    ' Obtain the zone source properties
    '
    With zone
        ZoneMessage = ZoneMessage + "Zone details:" +                   vbCrLf _
        + " Id = " +                ShowCorrectId( .Id ) +              vbCrLf _
        + " Name = " +              CStr( .Name ) +                     vbCrLf _
        + " OwnerId = " +           ShowCorrectId( .OwnerId ) +         vbCrLf _
        + " ScheduleId = " +        ShowCorrectId( .ScheduleId ) +      vbCrLf _
        + " AlarmRecordId = " +     ShowCorrectId( .AlarmRecordId ) +   vbCrLf _
        + " Priority = " +          CStr( .Priority ) +                 vbCrLf _
        + " State = " +             .StateDesc +                        vbCrLf _
        + " TimeRaised = " +        ShowCorrectTime( CStr( .TimeRaised ) ) + vbCrLf _
        + " TimeAcknowledged = " + ShowCorrectTime( CStr( .TimeAcknowledged ) ) + vbCrLf 
    End With

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZonePrepopulateComplete event handler
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZonePrepopulateComplete( asIpAddr )
    On Error Resume Next
    Dim ZoneMessage
    
    ZoneMessage = "Zone prepopulation completed from server " _
        + asIpAddr + vbCrLf         

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZoneNew event handler
'
' Outputs all properties of the new zone to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZoneNew( zone )
    On Error Resume Next
    Dim ZoneMessage

    '
    ' Obtain the zone server (NVR) properties
    '
    ZoneMessage = "Zone notification insertion received from server " _
        + zone.ASIpAddress +  vbCrLf _
        
    '
    ' Obtain the zone source properties
    '
    With zone
        ZoneMessage = ZoneMessage + "Zone details:" +                 vbCrLf _
        + " Id = " +                ShowCorrectId( .Id ) +            vbCrLf _
        + " Name = " +              CStr( .Name ) +                   vbCrLf _
        + " OwnerId = " +           ShowCorrectId( .OwnerId  ) +      vbCrLf _
        + " ScheduleId = " +        ShowCorrectId( .ScheduleId ) +    vbCrLf _
        + " AlarmRecordId = " +     ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Priority = " +          CStr( .Priority ) +               vbCrLf _
        + " State = " +             .StateDesc +                      vbCrLf _
        + " TimeRaised = " + ShowCorrectTime( CStr( .TimeRaised ) ) + vbCrLf _
        + " TimeAcknowledged = " + ShowCorrectTime( CStr( .TimeAcknowledged ) ) + vbCrLf 
    End With

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZoneUpdate event handler
'
' Outputs all properties of the updated zone to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZoneUpdate( zone )
    On Error Resume Next
    Dim ZoneMessage

    '
    ' Obtain the zone server (NVR) properties
    '
    ZoneMessage = "Zone notification update received from server " _
        + zone.ASIpAddress +  vbCrLf _
        
    '
    ' Obtain the zone source properties
    '
    With zone
        ZoneMessage = ZoneMessage + "Zone details:" +                   vbCrLf _
        + " Id = " +                ShowCorrectId( .Id ) +              vbCrLf _
        + " Name = " +              CStr( .Name ) +                     vbCrLf _
        + " OwnerId = " +           ShowCorrectId( .OwnerId ) +         vbCrLf _
        + " ScheduleId = " +        ShowCorrectId( .ScheduleId ) +      vbCrLf _
        + " AlarmRecordId = " +     ShowCorrectId( .AlarmRecordId ) +   vbCrLf _
        + " Priority = " +          CStr( .Priority ) +                 vbCrLf _
        + " State = " +             .StateDesc +                        vbCrLf _
        + " TimeRaised = " + ShowCorrectTime( CStr( .TimeRaised ) ) +   vbCrLf _
        + " TimeAcknowledged = " + ShowCorrectTime( CStr( .TimeAcknowledged ) ) + vbCrLf 
    End With

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZoneDelete event handler
'
' Outputs all properties of the deleted zone to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZoneDelete( zone )
    On Error Resume Next
    Dim ZoneMessage

    With zone
    ZoneMessage = "Zone with Id = " + ShowCorrectId( .Id ) + _
            " is deleted from server " + zone.ASIpAddress +  vbCrLf
    End With

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZoneRequestCancel event handler
'
' Outputs all properties of the cancelled zone to the console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZoneRequestCancel( asIpAddr )
    On Error Resume Next
    Dim ZoneMessage

    '
    ' Obtain the zone server (NVR) properties
    '
    ZoneMessage = "Zone notification is cancelled from server " _
        + asIpAddr + vbCrLf         

    WScript.Echo ZoneMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnZoneServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnZoneServerDisconnect( asIpAddr )
    WScript.Echo "The zone monitor is disconnected from server " _
        + asIpAddr + "."
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Check if the input timeValue is the default datetime
' If Yes, show N/A instead of timeValue
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowCorrectTime( timeValue )
    ShowCorrectTime = "N/A"
    timeValue = Trim( timeValue )
    If IsDate( timeValue ) Then
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
