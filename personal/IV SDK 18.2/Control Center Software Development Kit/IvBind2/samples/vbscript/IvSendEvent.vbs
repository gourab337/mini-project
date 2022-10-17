'
' IvSendEvent.vbs        Sample vbscript for the IvBind COM component.
'
' IvSendEvent sends an external system event to an Alarm Server.  
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define contants, variables used to validate timestamp
'
Dim SecondsInDay, MillisecondsInDay, MIN_YEAR, MAX_YEAR, MAX_EVENT_NUMBER_RANGE
SecondsInDay = 86400
MillisecondsInDay = 86400000
MIN_YEAR = 1601
MAX_YEAR = 30827
MAX_EVENT_NUMBER_RANGE = 32767

'
' The difference between local time and UTC time
' The value needs to be set to the timezone difference for the system's timezone
'
Dim TIME_ZONE_DIFFERENCE
TIME_ZONE_DIFFERENCE = 0

Dim TimeUsage
TimeUsage = "Please specify correct timestamp value!" 

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: IvSendEvent.vbs <options> /e:<event number> /a:<AS IP Address>" + vbCrlf _
  + "Options:" + vbCrlf _
  + "     /t:<TimeStamp>     Specifies timestamp to send event with format " + vbCrlf _
  + "                        Year(YYYY):Month(01-12):Day(01-31):Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)" + vbCrlf _
  + "                        Or specifies relative time in milliseconds." + vbCrlf _
  + "                        (Default: The current time is used) " + vbCrlf _
  + "     /x:<Annotation>    Specifies the annotation for the event" + vbCrlf _
  + "                        (Default: The event has no annotation)" + vbCrlf _
  + vbCrLf _
  + " e.g. CScript IvSendEvent.vbs /e:123 /a:192.168.1.1 /t:2010:12:20:08:07:00:123" + vbCrLf _
  + "      Send event Id 123 at 08:07:00 20/Dec/2010 to Alarm Server with IP address 192.168.1.1"

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.ivBind2" )

'
' Parse the command-line arguments
'
Dim nvrIpAddress, eventNumber, timeStamp, annotationText
Dim arguments, numValidArguments
Set arguments = WSCript.Arguments.Named

numValidArguments = 0

'
' Get the alarm server ip address
'
If arguments.Exists( "a" ) Then
    nvrIpAddress = arguments.Item( "a" )
    numValidArguments = numValidArguments + 1
Else
    WScript.Echo Usage
    WScript.Quit( 1 )
End If

'
' Get the event number
'
If arguments.Exists( "e" ) Then
    eventNumber = arguments.Item( "e" )
    numValidArguments = numValidArguments + 1
Else
    WScript.Echo Usage
    WScript.Quit( 1 )
End If

'
' Get the timestamp
'
'
If arguments.Exists( "t" ) Then
    timeStamp = GetTimeStamp( arguments.Item( "t" ) )
    numValidArguments = numValidArguments + 1
Else
    timeStamp = DateAdd( "h", TIME_ZONE_DIFFERENCE, Now )
End If

'
' Get annotation text
'
If arguments.Exists( "x" ) Then
    annotationText = arguments.Item( "x" )
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
' Validate value for Event Number
'
If ( not IsNumeric( eventNumber ) ) Or _
        ( CStr( eventNumber ) <> CStr( CLng( eventNumber ) ) ) Or _
        ( CLng( eventNumber )  <= 0 ) Or _
        ( CLng( eventNumber ) > MAX_EVENT_NUMBER_RANGE )  Then

    WScript.Echo "Invalid value for event number!"
    WScript.Quit 
End If

On Error Resume Next
'
' Send event to the AS using the date and time of this PC as the timestamp
'	from local machine
'

If IsEmpty(annotationText) Then
    Call ivbind.SendEvent( nvrIpAddress, eventNumber, vbNullString, timeStamp )
Else
    Call ivbind.SendEvent2( nvrIpAddress, eventNumber, vbNullString, timeStamp, annotationText )
End If

If Err.number <> 0 Then
	WScript.Echo "Error: " + Err.Description
Else	
	WScript.Echo "Event sent successfully"
End If

'
'Get TimeStamp from the command-line
'
'Convert Year(YYYY):Month(01-12):Day(01-31:Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)
'To CDate
'
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
                    WScript.Echo TimeUsage
                    WScript.Quit
                
                Else
                    On Error Resume Next
                    arr( i ) = CLng( arr( i ) )     
                    If ( Err.number <> 0 ) Then
                          WScript.Echo TimeUsage
                          WScript.Quit   
                    End If     
                End if
            Next
            
            If ValidateTimeValue( arr ) then
                Dim myDate
                myDate = CStr( arr( 0 ) ) + "/" + CStr( arr( 1 ) ) + "/" + CStr( arr( 2 ) )
                    
                If IsDate( myDate ) then
                    num = DateAdd( "h", TIME_ZONE_DIFFERENCE, CDate( myDate ) )
                    num = num + ( arr( 3 ) * 3600 + arr( 4 ) * 60 + arr( 5 ) ) / SecondsInDay
                    GetTimeStamp =  num + arr( 6 ) / MillisecondsInDay
                 
                 Else
                    WScript.Echo TimeUsage
                    WScript.Quit                            
                End if
                
            Else
                WScript.Echo TimeUsage
                WScript.Quit        
            End if 
            
        Else
            WScript.Echo TimeUsage
            WScript.Quit        
        End if
    End if
    
End Function

'
'Validate time value
'param arr - contains Year:month:day:hour:minute:second 
'

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

'
'Validate values in valid range
'
Function ValidateValue( value, min, max  )
    ValidateValue = false
    If ( min <= max ) and ( value >= min ) and ( value <= max ) then
        ValidateValue = true
    End if
End Function
