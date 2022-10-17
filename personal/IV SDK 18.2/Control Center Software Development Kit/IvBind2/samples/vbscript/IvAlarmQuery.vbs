'
' IvAlarmQuery.vbs  Sample vbscript for the IvBind COM component.
'
' IvAlarmQuery outputs alarm record notifications from a specified alarm
' server to the console.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit
On Error Resume Next

'
' Usage information for this script 
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvAlarmQuery.vbs <options> /a:<AS IP Address>" + vbCrLf + vbCrLf _
    + "At least one of the /l or /u arguments must be provided" + vbCrLf _
    + "Options:" + vbCrLf _
    + "     /l:<Timestamp>      The minimum alarm raised time" + vbCrLf _
    + "     /u:<Timestamp>      The maximum alarm raised time" + vbCrLf _
    + "Timestamp Format:" + vbCrLf _
    + "     Year(YYYY):Month(01-12):Day(01-31):Hour(00-23):Minute(00-59):" _
    + "Second(00-59):Millisecond(000-999)" + vbCrLf _
    + vbCrLf _
    + "  e.g. CScript.exe IvAlarmQuery.vbs /l:2015:03:05:12:00:00:000 /a:192.168.1.1" + vbCrLf _
    + "       Query Alarms raised after noon on the 5th of march 2015 " + vbCrLf _
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
Dim asIpAddress, minimumAlarmTime, maximumAlarmTime
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
    minimumAlarmTime = GetTimeStamp( arguments.Item( "l" ) )
    numValidArguments = numValidArguments + 1
Else
    ' No lower bound on the alarm raised time
    minimumAlarmTime = 0
End If
If arguments.Exists( "u" ) Then
    maximumAlarmTime = GetTimeStamp( arguments.Item( "u" ) )
    numValidArguments = numValidArguments + 1
Else
    ' No upper bound on the alarm raised time
    maximumAlarmTime = 0
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
' Create a Alarm Record Query using the Binding Kit
'
Call ivbind.QueryAlarmRecords( asIpAddress, minimumAlarmTime, maximumAlarmTime )
If Err.number <> 0 Then
    WScript.Echo "QueryAlarmRecords returned error: " + Err.Description
    WScript.Quit( 1 )
Else
    WScript.Echo "Successfully created alarm record query"
End If

'
' Wait for Alarm Records
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordPrepopulate event handler
'
' Outputs all properties of the alarm record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordPrepopulate( alarm )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "Historic Alarm Record notification received from Alarm " _
        + "Server " + alarm.ASIpAddress + vbCrLf

    With alarm
        AlarmMessage = AlarmMessage + "Alarm Record details:" + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " State = " + .StateDesc + vbCrLf _
        + " Owner ID = " + ShowCorrectId( .OwnerId ) + vbCrLf _
        + " Time Raised = " + ShowCorrectTime( CStr( .TimeRaised ) ) + vbCrLf _
        + " Time Cleared = " + ShowCorrectTime( CStr( .TimeCleared ) ) + vbCrLf
    End With

    WScript.Echo AlarmMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordPrepopulateComplete event handler
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordPrepopulateComplete( asIpAddr )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "Alarm record prepopulation completed from server " _
        + asIpAddr + vbCrLf _
        + "All historical alarm records have been sent." + vbCrLf

    WScript.Echo AlarmMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordNew event handler
'
' Outputs all properties of the new alarm record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordNew( alarm )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "Alarm record insertion notification received from " _
        + "server " + alarm.ASIpAddress + vbCrLf

    With alarm
        AlarmMessage = AlarmMessage + "Alarm Record details:" + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " State = " + .StateDesc + vbCrLf _
        + " Owner ID = " + ShowCorrectId( .OwnerId ) + vbCrLf _
        + " Time Raised = " + ShowCorrectTime( CStr( .TimeRaised ) ) + vbCrLf _
        + " Time Cleared = " + ShowCorrectTime( CStr( .TimeCleared ) ) + vbCrLf
    End With

    WScript.Echo AlarmMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' IvBind2_OnAlarmRecordUpdate event handler
'
' Outputs all properties of the updated zone to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordUpdate( alarm )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "Alarm record update notification received from " _
        + "server " + alarm.ASIpAddress + vbCrLf

    With alarm
        AlarmMessage = AlarmMessage + "Alarm Record details:" + vbCrLf _
        + " Alarm Record ID = " + ShowCorrectId( .AlarmRecordId ) + vbCrLf _
        + " Zone ID = " + ShowCorrectId( .ZoneId ) + vbCrLf _
        + " State = " + .StateDesc + vbCrLf _
        + " Owner ID = " + ShowCorrectId( .OwnerId ) + vbCrLf _
        + " Time Raised = " + ShowCorrectTime( CStr( .TimeRaised ) ) + vbCrLf _
        + " Time Cleared = " + ShowCorrectTime( CStr( .TimeCleared ) ) + vbCrLf
    End With

    WScript.Echo AlarmMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordDelete event handler
'
' Outputs all properties of the deleted zone to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordDelete( alarm )
    On Error Resume Next
    Dim AlarmMessage

    With alarm
        AlarmMessage = "Alarm record with Id = " _
            + ShowCorrectId( .AlarmRecordId ) _
            + " has been deleted from server " + alarm.ASIpAddress +  vbCrLf
    End With

    WScript.Echo AlarmMessage
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordRequestCancel event handler
'
' Outputs cancellation message then exits.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordRequestCancel( asIpAddr )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "Alarm query with server " _
        + asIpAddr + " has been canceled." + vbCrLf 
    WScript.Echo AlarmMessage
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordServerDisconnect( asIpAddr )
    On Error Resume Next
    Dim AlarmMessage

    AlarmMessage = "The connection to alarm server " _
        + asIpAddr + " has been lost." + vbCrLf 
    WScript.Echo AlarmMessage
    WScript.Quit( 1 )
End Sub

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
