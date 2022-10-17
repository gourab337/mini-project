'
' IvActivationQuery.vbs  Sample vbscript for the IvBind COM component.
'
' IvActivationQuery outputs activation record notifications from a
' specified alarm server to the console.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit
On Error Resume Next

'
' VB Script doesn't support Enumerations so any enums must be redeclared as
' constant integers before they can be used:
' Extra info type constants
'
Const IVBIND2_ACTIVATIONEXTRAINFOTYPE_NONE = 0
Const IVBIND2_ACTIVATIONEXTRAINFOTYPE_UNKNOWN = 1
Const IVBIND2_ACTIVATIONEXTRAINFOTYPE_NVRFAULT = 2
Const IVBIND2_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION = 4

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

'
' Usage information for this script 
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvActivationQuery.vbs <options> /a:<AS IP Address>" + vbCrLf + vbCrLf _
    + "At least one of the /l or /u arguments must be provided" + vbCrLf _
    + "Options:" + vbCrLf _
    + "     /l:<Timestamp>      The minimum activation time" + vbCrLf _
    + "     /u:<Timestamp>      The maximum activation time" + vbCrLf _
    + "Timestamp Format:" + vbCrLf _
    + "     Year(YYYY):Month(01-12):Day(01-31):Hour(00-23):Minute(00-59):" _
    + "Second(00-59):Millisecond(000-999)" + vbCrLf _
    + vbCrLf _
    + "  e.g. CScript.exe IvActivationQuery.vbs /l:2015:03:05:12:00:00:000 /a:192.168.1.1" + vbCrLf _
    + "       Query Activations raised after noon on the 5th of march 2015 " + vbCrLf _
    + "       on Alarm Server 192.168.1.1."

'
' Define constants for validating command line timestamp values
'
Dim TimeErrorMsg
TimeErrorMsg = "Please specify correct timestamp value!" + vbCrLf + vbCrLf
Dim SecondsInDay, MillisecondsInDay, MIN_YEAR, MAX_YEAR
SecondsInDay = 86400
MillisecondsInDay = 86400000
MIN_YEAR = 1601
MAX_YEAR = 30827

'
' The difference between local time and UTC time
' To correctly convert timestamps specified on the command line to UTC,
' this value needs to be set to the difference in hours for the system's timezone
' compared to UTC. It also needs to be changed to suit daylight saving time.
'
Dim TIME_ZONE_DIFFERENCE
TIME_ZONE_DIFFERENCE = 0

'
' Ensure that this script is being invoked using the CScript host executable
'
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
' Parse the command-line arguments
'
Dim asIpAddress, minActTime, maxActTime
Dim arguments, numValidArguments
Set arguments = WSCript.Arguments.Named
Set numValidArguments = 0

'
' Get the Alarm Server IP Address
'
If arguments.Exists( "a" ) Then
    asIpAddress = arguments.Item( "a" )
    numValidArguments = numValidArguments + 1
Else
    WScript.Echo Usage
    WScript.Quit( 1 )
End If

'
' Get the minimum and maximum time bounds
'
If arguments.Exists( "l" ) Then
    minActTime = GetTimeStamp( arguments.Item( "l" ) )
    numValidArguments = numValidArguments + 1
Else
    ' No lower bound on the activation time
    minActTime = 0
End If
If arguments.Exists( "u" ) Then
    maxActTime = GetTimeStamp( arguments.Item( "u" ) )
    numValidArguments = numValidArguments + 1
Else
    ' No upper bound on the activation time
    maxActTime = 0
End If

'
' Check no unused arguments provided
'
If numValidArguments <> WScript.Arguments.Count Then
    WScript.Echo "Invalid argument provided"
    WScript.Echo Usage
    WScript.Quit( 1 )
End If

'
' Create an Activation Record Query using the Binding Kit
'
Call ivbind.QueryActivationRecords( asIpAddress, minActTime, maxActTime )
If Err.number <> 0 Then
    WScript.Echo "QueryActivationRecords returned error: " + Err.Description
    WScript.Quit( 1 )
Else
    WScript.Echo "Successfully created activation record query"
End If

'
' Wait for Activation Records
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordPrepopulate event handler
'
' Outputs all properties of the activation record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordPrepopulate( activation )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "Historic activation record notification received " _
        + "from Alarm Server " + activation.ASIpAddress + vbCrLf

    With activation
        ActivationMessage = ActivationMessage + "Activation Record details:" + vbCrLf _
        + " Activation Record ID = " + ShowCorrectId( .ActivationRecordId ) + vbCrLf _
        + " Detector ID = " + ShowCorrectId( .DetectorId ) + vbCrLf _
        + " Activation Time = " + ShowCorrectTime( CStr( .ActivationTime ) ) + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " Alarm Time = " + ShowCorrectTime( CStr( .AlarmTime ) ) + vbCrLf _
        + " Extra Info  = " + ShowExtraInfo( .ExtraInfo ) + vbCrLf
    End With

    WScript.Echo ActivationMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordPrepopulateComplete event handler
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordPrepopulateComplete( asIpAddr )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "Activation record prepopulation completed from " _
        + "server " + asIpAddr + vbCrLf _
        + "All historical activation records have been sent." + vbCrLf

    WScript.Echo ActivationMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordNew event handler
'
' Outputs all properties of the new activation record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordNew( activation )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "Activation record insertion notification received " _
        + "from server " + activation.ASIpAddress + vbCrLf

    With activation
        ActivationMessage = ActivationMessage + "Activation Record details:" + vbCrLf _
        + " Activation Record ID = " + ShowCorrectId( .ActivationRecordId ) + vbCrLf _
        + " Detector ID = " + ShowCorrectId( .DetectorId ) + vbCrLf _
        + " Activation Time = " + ShowCorrectTime( CStr( .ActivationTime ) ) + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " Alarm Time = " + ShowCorrectTime( CStr( .AlarmTime ) ) + vbCrLf _
        + " Extra Info  = " + ShowExtraInfo( .ExtraInfo ) + vbCrLf
    End With

    WScript.Echo ActivationMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' IvBind2_OnActivationRecordUpdate event handler
'
' Outputs all properties of the updated activation to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordUpdate( activation )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "Activation record update notification received from " _
        + "server " + activation.ASIpAddress + vbCrLf

    With activation
        ActivationMessage = ActivationMessage + "Activation Record details:" + vbCrLf _
        + " Activation Record ID = " + ShowCorrectId( .ActivationRecordId ) + vbCrLf _
        + " Detector ID = " + ShowCorrectId( .DetectorId ) + vbCrLf _
        + " Activation Time = " + ShowCorrectTime( CStr( .ActivationTime ) ) + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " Alarm Time = " + ShowCorrectTime( CStr( .AlarmTime ) ) + vbCrLf _
        + " Extra Info  = " + ShowExtraInfo( .ExtraInfo ) + vbCrLf
    End With

    WScript.Echo ActivationMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordDelete event handler
'
' Outputs all properties of the deleted activation to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordDelete( activation )
    On Error Resume Next
    Dim ActivationMessage

    With activation
        ActivationMessage = "Activation record with Id = " _
            + ShowCorrectId( .ActivationRecordId ) _
            + " has been deleted from server " + .ASIpAddress + vbCrLf
    End With

    WScript.Echo ActivationMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordRequestCancel event handler
'
' Outputs cancellation message then exits.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordRequestCancel( asIpAddr )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "Activation query with server " _
        + asIpAddr + " has been canceled." + vbCrLf 
    WScript.Echo ActivationMessage
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnActivationRecordServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnActivationRecordServerDisconnect( asIpAddr )
    On Error Resume Next
    Dim ActivationMessage

    ActivationMessage = "The connection to Alarm Server " _
        + asIpAddr + " has been lost." + vbCrLf 
    WScript.Echo ActivationMessage
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing the extra info passed in
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ShowExtraInfo( extraInfo )
    On Error Resume Next
    Dim message, index

    message = vbCrLf + "  Type: " + ExtraInfoTypeToString( extraInfo.Type ) + vbCrLf

    Select Case extraInfo.Type
        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION
            message = message + "  Annotation Message: " + _
                extraInfo.AnnotationMsg + vbCrLf
        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_NVRFAULT
            message = message + "  Number Faults: " + _
                CStr( extraInfo.NumFaults ) + vbCrLf
            For index = 0 to ( extraInfo.NumFaults - 1 ) 
                message = message + "   Fault " + CStr( index ) + _
                    ": " + FaultTypeToString( extraInfo.GetFaultType( index ) ) + _
                    vbCrLf
            Next
        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_UNKNOWN
          message = message + "Unknown" +  vbCrLf
    End Select

    ShowExtraInfo = message
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing an extra info type
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ExtraInfoTypeToString( infoType )
    Select Case infoType
        Case IVBIND2_ACTIVATIONEXTRAINFOTYPE_NONE
            ExtraInfoTypeToString = "No Extra Info"
        Case IVBIND2_ACTIVATIONEXTRAINFOTYPE_NVRFAULT
            ExtraInfoTypeToString = "NVR Fault Info"
        Case IVBIND2_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION
            ExtraInfoTypeToString = "Annotation Info"
        Case Else
            ExtraInfoTypeToString = "Unknown"
    End Select
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Get a string describing a fault type
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function FaultTypeToString( infoType )
    Select Case infoType
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

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Check if the input timeValue is the default datetime
' If Yes, show N/A instead of timeValue
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
'Get TimeStamp from the command-line
'
'Convert Year(YYYY):Month(01-12):Day(01-31):Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)
'To CDate
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function GetTimeStamp( str )
    Dim num, arr, i, u

    str = Trim( str )

    If IsNumeric( str ) then
        num = DateAdd( "h", TIME_ZONE_DIFFERENCE, Now )
        num = num + CDbl( str ) / MillisecondsInDay
        GetTimeStamp = num 
    Else
        arr = Split( str, ":", -1, 1 )
        u = Ubound( arr )

        'Check TimeStamp format
        If ( u = 6 ) Then

            For i = 0 to u
                If arr( i ) = vbNullString Then
                    WScript.Echo TimeErrorMsg + Usage
                    WScript.Quit

                Else
                    On Error Resume Next
                    arr( i ) = CLng( arr( i ) )

                    If ( Err.number <> 0 ) Then
                          WScript.Echo TimeErrorMsg + Usage
                          WScript.Quit
                    End If 
                End if
            Next

            If ValidateTimeValue( arr ) Then
                Dim myDate
                myDate = CStr( arr( 0 ) ) + "/" + CStr( arr( 1 ) ) + "/" + CStr( arr( 2 ) )
                    
                If IsDate( myDate ) then
                    num = DateAdd( "h", TIME_ZONE_DIFFERENCE, CDate( myDate ) )
                    num = num + ( arr( 3 ) * 3600 + arr( 4 ) * 60 + arr( 5 ) ) / SecondsInDay
                    GetTimeStamp = Cdbl(CDate( num ) ) + arr( 6 ) / MillisecondsInDay
                Else
                    WScript.Echo TimeErrorMsg + Usage
                    WScript.Quit
                End if

            Else
                WScript.Echo TimeErrorMsg + Usage
                WScript.Quit
            End if 

        Else
            WScript.Echo TimeErrorMsg + Usage
            WScript.Quit
        End if
    End if

End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'Validate time value
'param arr - contains Year:month:day:hour:minute:second 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ValidateTimeValue( arr )
    ValidateTimeValue = false
    If ( ValidateValue( arr( 0 ), MIN_YEAR, MAX_YEAR ) ) and _
        ( ValidateValue( arr( 1 ), 1, 12 ) ) and _
        ( ValidateValue( arr( 2 ), 1, 31 ) ) and _
        ( ValidateValue( arr( 3 ), 0, 23 ) ) and _
        ( ValidateValue( arr( 4 ), 0, 59 ) ) and _
        ( ValidateValue( arr( 5 ), 0, 59 ) ) and _
        ( ValidateValue( arr( 6 ), 0, 999 ) ) then
        
        ValidateTimeValue = true
    End if
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'Validate values
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ValidateValue( value, min, max )
    ValidateValue = false
    If ( min <= max ) and ( value >= min ) and ( value <= max ) then
        ValidateValue = true
    End if
End Function
