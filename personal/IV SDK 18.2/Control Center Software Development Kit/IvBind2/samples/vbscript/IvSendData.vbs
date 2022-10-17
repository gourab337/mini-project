'
' IvSendData.vbs        Sample vbscript for the IvBind COM component.
'
' IvSendData sends data from a external system to an Alarm Server.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: IvSendData.vbs <alarm server IP address> <external system IP> <source num> <data>" + vbCrlf _
  + "   or: IvSendData.vbs <alarm server IP address> <external system IP> <source num> <data> <timestamp>" + vbCrlf _    
  + " Note: <source num> has to be a number in the range 1 to 32767 (including 1 and 32767)" + vbCrlf _
  + " Note: <TimeStamp>  specifies timestamp to send data with format " + vbCrlf _
  + "                      Year(YYYY):Month(01-12):Day(01-31:Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)" + vbCrlf _
  + "                      Or specifies relative time in milliseconds." + vbCrlf _
  + "                      (Default the current time is used) " + vbCrlf + vbCrLf _    
  + "e.g. IvSendData.vbs 192.168.1.6 192.168.1.5 12345 testdata 2010:12:20:08:07:00:123" + vbCrLf _
  + "     Sends the data 'testdata' at 08:07:00:123 20/Dec/2010 from external system 192.168.1.5" + vbCrLf _
  + "     and source number 12345 to Alarm Server with IP address 192.168.1.6"
 
'
' Define constants, messages to validate TimeStamp inputted value
'
Dim TimeUsage
TimeUsage = "Please specify correct timestamp value!" 
Dim SecondsInDay, MillisecondsInDay, MIN_YEAR, MAX_YEAR
SecondsInDay = 86400
MillisecondsInDay = 86400000
MIN_YEAR = 1601
MAX_YEAR = 30827

'
' The difference between local time and UTC time
' The value needs to be set to the timezone difference for the system's timezone
' Also needs to be changed to suit daylight saving time
'
Dim TIME_ZONE_DIFFERENCE
TIME_ZONE_DIFFERENCE = 0
  
'
' Parse the command-line arguments
'
If ( WScript.Arguments.Count < 4 ) or ( WScript.Arguments.Count > 5 ) Then
    WScript.Echo Usage
    WScript.Quit
End If

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.ivBind2" )

'
' IvBind Send Data Parameters 
'
Dim  serverIpAddress, extSystemIpAddress, sourceNum, data, TimeStamp

serverIpAddress = WScript.Arguments( 0 )
extSystemIpAddress = WScript.Arguments( 1 )
sourceNum = WScript.Arguments( 2 )
data = WScript.Arguments( 3 )


If WScript.Arguments.Count = 5 Then
    TimeStamp = GetTimeStamp( WScript.Arguments( 4 ) )
Else
    TimeStamp = DateAdd( "h", TIME_ZONE_DIFFERENCE, Now )    
End If

On Error Resume Next

'
' Send the data using the parameters
'
Call ivbind.SendData( serverIpAddress, extSystemIpAddress, sourceNum, TimeStamp, data )

If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else
    WScript.Echo "Sent data successfully"
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
            
            If ValidateTimeValue( arr ) Then
                Dim myDate
                myDate = CStr( arr( 0 ) ) + "/" + CStr( arr( 1 ) ) + "/" + CStr( arr( 2 ) )
                    
                If IsDate( myDate ) then
                    num = DateAdd( "h", TIME_ZONE_DIFFERENCE, CDate( myDate ) )
                    num = num + ( arr( 3 ) * 3600 + arr( 4 ) * 60 + arr( 5 ) ) / SecondsInDay
                    GetTimeStamp = Cdbl(CDate( num ) ) + arr( 6 ) / MillisecondsInDay
                 
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
'Validate values
'

Function ValidateValue( value, min, max  )
    ValidateValue = false
    If ( min <= max ) and ( value >= min ) and ( value <= max ) then
        ValidateValue = true
    End if
End Function
