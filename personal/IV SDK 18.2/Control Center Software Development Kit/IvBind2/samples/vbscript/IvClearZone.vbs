'
' IvClearZone.vbs        Sample vbscript for the IvBind COM component.
'
' IvClearZone clear zone with specific name
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: IvClearZone.vbs <AS IP Address> <Zone Name>" + vbCrlf _
  + "   or: IvClearZone.vbs <AS IP Address> <Zone Name> <ClearMessage>" + vbCrlf + vbCrlf _
  + "e.g. IvClearZone.vbs 192.168.1.1 ""New Zone 1"" ""MessageCleared""" + vbCrLf _
  + "     Clear zone name ""New Zone 1"" with message ""MessageCleared"" " + vbCrLf _
  + "           from Alarm Server with IP address 192.168.1.1"
  
'
' Parse the command-line arguments
'
Dim clrMessage

If ( WScript.Arguments.Count < 2 ) or ( WScript.Arguments.Count > 3 ) Then
    WScript.Echo Usage
    WScript.Quit
ElseIf WScript.Arguments.Count = 3 then
    clrMessage =  WScript.Arguments( 2 )
End If

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2" )

'
' IvBind Clear Zone Parameters 
'
Dim nvrIpAddress, zoneName

'
' Mapping the arguments from parameters
'
nvrIpAddress = WScript.Arguments( 0 )
zoneName = WScript.Arguments( 1 )

On Error Resume Next
'
' Calling the Clear Zone action 
'
If WScript.Arguments.Count = 2 then
    Call ivbind.ClearZone( nvrIpAddress, zoneName, vbNullString )
Else
    Call ivbind.ClearZone( nvrIpAddress, zoneName, clrMessage )
End If

'
' Check the error and display it, if found
'
If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else    
    WScript.Echo "Clear zone successful."
End If
