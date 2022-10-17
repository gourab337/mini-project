'
' IvAddBookmark.vbs        Sample vbscript for the IvBind COM component.
'
' IvAddBookmark adds a bookmark to a specific camera in an Alarm Server.  
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: IvAddBookmark.vbs <asIP> <camIP> <bookmarktext>" + vbCrlf _
  + "   or: IvAddBookmark.vbs <asIp> <camIP> <bookmarktext> <Timestamp>" + vbCrlf _    
  + " Note: <TimeStamp>     Specifies timestamp to add bookmark with format " + vbCrlf _
  + "                Year(YYYY):Month(01-12):Day(01-31:Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)" + vbCrlf _
  + "                Or specifies relative time in milliseconds." + vbCrlf _
  + "                (Default the current time is used) " + vbCrlf + vbCrLf _    
  + "e.g. IvAddBookmark.vbs 192.168.1.5 192.168.1.1 ""BookmarkText"" 2010:12:20:08:07:00:123" + vbCrLf _
  + "     Add bookmark with text ""BookmarkText"" at 08:07:00:123 20/Dec/2010" + vbCrLf _
  + "     to camera 192.168.1.1 in Alarm Server with IP address 192.168.1.5"
  
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
If ( WScript.Arguments.Count < 3 ) or ( WScript.Arguments.Count > 4 ) Then
    WScript.Echo Usage
    WScript.Quit
End If

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.ivBind2" )

'
' IvBind AddBookmark Parameters 
'
Dim  nvrIpAddress, camIpAddress, bookmarkText, numSec, TimeStamp

nvrIpAddress = WScript.Arguments( 0 )
camIpAddress = WScript.Arguments( 1 )
bookmarkText = WScript.Arguments( 2 )

'
' Check the third and the fourth command-line argument
'
If WScript.Arguments.Count = 4 Then
    TimeStamp = GetTimeStamp( WScript.Arguments( 3 ) )
Else
    TimeStamp = DateAdd( "h", TIME_ZONE_DIFFERENCE, Now )    
End If

On Error Resume Next
'
' Add Bookmark to the Camera in Alarm Server using the date and time of this PC 
' as the timestamp from local machine
'
WScript.Echo TimeStamp

Call ivbind.AddBookmark( nvrIpAddress, camIpAddress, bookmarkText, TimeStamp )

If Err.number <> 0 Then
	WScript.Echo "Error: " + Err.Description
Else	
	WScript.Echo "Added bookmark successfully"
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