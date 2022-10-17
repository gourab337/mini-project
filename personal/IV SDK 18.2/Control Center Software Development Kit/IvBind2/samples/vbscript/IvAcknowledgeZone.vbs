'
' IvAcknowledgeZone.vbs        Sample vbscript for the IvBind COM component.
'
' IvAcknowledgeZone clear zone with specific name
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: IvAcknowledgeZone.vbs <AS IP Address> <Zone Name>" + vbCrlf _
  + "   or: IvAcknowledgeZone.vbs <AS IP Address> <Zone Name> <AcknowledgeMessage>" + vbCrlf + vbCrlf _
  + "  e.g. IvAcknowledgeZone.vbs 192.168.1.1 ""New Zone 1"" ""Acknowledge message""" + vbCrLf _
  + "     Acknowledge zone name ""New Zone 1"" with message ""Acknowledge message""" +  vbCrLf _
  + "     from Alarm Server with IP address 192.168.1.1"
  
'
' Parse the command-line arguments
'

Dim ackMessage

If ( WScript.Arguments.Count < 2 ) or ( WScript.Arguments.Count > 3 ) Then
    WScript.Echo Usage
    WScript.Quit
ElseIf WScript.Arguments.Count = 3 then
    ackMessage =  WScript.Arguments( 2 )
End If

'
' Create an instance of IvBind COM component.
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2" )

'
' IvBind Acknowledge Zone parameters 
'
Dim nvrIpAddress, zoneName

'
' Passing the parameters
'
nvrIpAddress = WScript.Arguments( 0 )
zoneName = WScript.Arguments( 1 )

On Error Resume Next
'
' Calling the AcknowledgeZone action
'
If WScript.Arguments.Count = 2 then
    Call ivbind.AcknowledgeZone( nvrIpAddress, zoneName, vbNullString )
Else
    Call ivbind.AcknowledgeZone( nvrIpAddress, zoneName, ackMessage )
End If

'
' Check the error and display it, if found
'
If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else    
    WScript.Echo "Acknowledge zone successful."
End If
