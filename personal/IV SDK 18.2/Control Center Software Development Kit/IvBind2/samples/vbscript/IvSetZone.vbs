'
' IvSetZone.vbs    Sample vbscript for the IvBind COM component.
'
' IvSetZone set zone with specific name
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script
'
Dim Usage
Usage = "Usage: IvSetZone.vbs <AS IP Address> <Zone Name>" + vbCrlf + vbCrlf _
    + "e.g. IvSetZone.vbs 192.168.1.1 ""New Zone 1""" + vbCrLf _
    + "     Set zone name ""New Zone 1"" from Alarm Server with IP address 192.168.1.1"
  
'
' Parse the command-line arguments
'
If WScript.Arguments.Count <> 2 Then
    WScript.Echo Usage
    WScript.Quit
End If

'
' IvBind Set Zone params
'
Dim asIpAddress, zoneName
asIpAddress = WScript.Arguments( 0 )
zoneName = WScript.Arguments( 1 )

'
' Create an instance of IvBind COM object 
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2" )

On Error Resume Next

'
' Calling the Set Zone action 
'
Call ivbind.SetZone( asIpAddress, zoneName )

'
' Display error message
'
If Err.number = 0 Then
    WScript.Echo "Set zone successfully."
Else
    WScript.Echo "Error: " + Err.Description
End If
