'
' IvDataRecordQuery.vbs  Sample vbscript for the IvBind COM component.
'
' IvDataRecordQuery outputs data record notifications from a specified Alarm
' Server to the console.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Usage information for this script
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvDataRecordQuery.vbs <options> /a:<AS IP Address>" _
    + vbCrLf + vbCrLf _
    + "If /n is not provided then at least one of /l or /u must be." + vbCrLf _
    + "Options:" + vbCrLf _
    + "     /l:<Timestamp>  Only Data Records created after this time will " _
    + "be returned" + vbCrLf _
    + "     /u:<Timestamp>  Only Data Records created before this time will " _
    + "be returned" + vbCrLf _
    + "     /s:<SourceIDs>  Only Data Records created by one of these " _
    + "External Data Sources will be returned" + vbCrLf _
    + "     /d:<DataFilter> Only Data Records that have data that contains" _
    + "the supplied string will be returned" + vbCrLf _
    + "Timestamp Format:" + vbCrLf _
    + "     /n              Only Data Records created after the query is " _
    + "started will be returned" + vbCrLf _
    + "     Year(YYYY):Month(01-12):Day(01-31):Hour(00-23):Minute(00-59):" _
    + "Second(00-59):Millisecond(000-999)" + vbCrLf _
    + vbCrLf _
    + "  e.g. CScript.exe IvDataRecordQuery.vbs /l:2015:03:05:12:00:00:000 " _
    + "/a:192.168.1.1" + vbCrLf _
    + "     Query Data Records created after noon on the 5th of march 2015 " _
    + "on Alarm Server 192.168.1.1."

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
' To correctly convert timestamps specified on the command line to UTC, this
' value needs to be set to the difference in hours for the system's timezone
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
Dim asIpAddress, minTime, maxTime, sourceIds, dataFilter, liveOnly
Dim arguments, numValidArguments
Set arguments = WSCript.Arguments.Named

minTime = 0
maxTime = 0
dataFilter = ""
liveOnly = False
numValidArguments = 0

'
' Process arguments
'
If arguments.Exists( "a" ) Then
    asIpAddress = arguments.Item( "a" )
    numValidArguments = numValidArguments + 1
Else
    WScript.Echo Usage
    WScript.Quit( 1 )
End If

If arguments.Exists( "l" ) Then
    minTime = GetTimeStamp( arguments.Item( "l" ) )
    numValidArguments = numValidArguments + 1
End If

If arguments.Exists( "u" ) Then
    maxTime = GetTimeStamp( arguments.Item( "u" ) )
    numValidArguments = numValidArguments + 1
End If

If arguments.Exists( "s" ) Then
    sourceIds = Split( arguments.Item( "s" ), "," )
    numValidArguments = numValidArguments + 1
End If

If arguments.Exists( "d" ) Then
    dataFilter = arguments.Item( "d" )
    numValidArguments = numValidArguments + 1
End If

If arguments.Exists( "n" ) Then
    liveOnly = True
    numValidArguments = numValidArguments + 1
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
' Create a Data Record Query using the Binding Kit
'
Call ivbind.QueryDataRecords( asIpAddress, minTime, maxTime, sourceIds, _
    dataFilter, liveOnly )
If Err.number <> 0 Then
    WScript.Echo "QueryDataRecords returned error: " + Err.Description
    WScript.Quit( 1 )
Else
    WScript.Echo "Successfully created data record query"
End If

'
' Wait for Data Records
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordPrepopulate event handler
'
' Outputs all properties of the data record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordPrepopulate( record )
    Dim message

    message = "Historic Data Record notification received from Alarm Server " _
        + record.ASIpAddress + vbCrLf  + DataRecordToString( record )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordPrepopulateComplete event handler
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordPrepopulateComplete( asIpAddr )
    Dim message

    message = "Data Record prepopulation completed from Alarm Server " _
        + asIpAddr + vbCrLf _
        + "All historical data records have been sent." + vbCrLf

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordNew event handler
'
' Outputs all properties of the new data record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordNew( record )
    Dim message

    message = "Data Record insertion notification received from Alarm Server " _
        + record.ASIpAddress + vbCrLf + DataRecordToString( record )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' IvBind2_OnDataRecordUpdate event handler
'
' Outputs all properties of the updated data record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordUpdate( record )
    Dim message

    message = "Data Record update notification received from Alarm Server " _
        + record.ASIpAddress + vbCrLf + DataRecordToString( record )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordDelete event handler
'
' Outputs all properties of the deleted data record to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnAlarmRecordDelete( record )
    Dim message

    message = "Data Record with ID = " _
            + ShowCorrectId( record.AlarmRecordId ) _
            + " has been deleted from Alarm Server " _
            + record.ASIpAddress + vbCrLf

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordRequestCancel event handler
'
' Outputs cancellation message then exits.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordRequestCancel( asIpAddr )
    Dim message

    message = "Data Record query with Alarm Server " + asIpAddr _
        + " has been cancelled." + vbCrLf
    WScript.Echo message
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnDataRecordServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnDataRecordServerDisconnect( asIpAddr )
    Dim message

    message = "The connection to Alarm Server " + asIpAddr _
        + " has been lost." + vbCrLf
    WScript.Echo message
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
'Convert YYYY:MM:DD:hh:mm:ss:fff string to CDate
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
                myDate = CStr( arr( 0 ) ) + "/" + CStr( arr( 1 ) ) + "/" _
                    + CStr( arr( 2 ) )

                If IsDate( myDate ) then
                    num = DateAdd( "h", TIME_ZONE_DIFFERENCE, CDate( myDate ) )
                    num = num _
                        + ( arr( 3 ) * 3600 + arr( 4 ) * 60 + arr( 5 ) ) _
                        / SecondsInDay
                    GetTimeStamp = Cdbl(CDate( num ) ) + arr( 6 ) _
                        / MillisecondsInDay
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
'param arr - contains Year:month:day:hour:minute:second:millisecond
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

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Generate a string containing the details of an data record
'
' Returns a string containing the data record details
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function DataRecordToString( record )
    With record
        DataRecordToString = "Data Record details:" + vbCrLf _
            + " Data Record ID = " + ShowCorrectId( .DataRecordId ) + vbCrLf _
            + " Data Source ID = " + ShowCorrectId( .DataSourceId ) + vbCrLf _
            + " Creation Time = " + ShowCorrectTime( CStr( .Time ) ) + vbCrLf _
            + " Data = " + CStr( .Data ) + vbCrLf
    End With
End Function
