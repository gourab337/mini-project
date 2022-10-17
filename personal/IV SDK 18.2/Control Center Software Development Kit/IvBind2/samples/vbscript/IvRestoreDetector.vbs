'
' IvRestoreDetector.vbs    Sample vbscript for the IvBind COM component.
'
' IvRestoreDetector Restore detector with specific name
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script
'
Dim Usage
Usage = "Usage: IvRestoreDetector.vbs <AS IP Address> <Zone Name> <Detector Name>" _
    + vbCrlf + vbCrlf _
    + "e.g. IvRestoreDetector.vbs 192.168.1.1 ""New Zone 1"" ""New Detector 1""" _
    + vbCrLf _
    + "     Restore detector name ""New Detector 1"" belongs to zone ""New Zone 1""" _
    + " from Alarm Server with IP address 192.168.1.1"
  
'
' Parse the command-line arguments
'
If WScript.Arguments.Count <> 3 Then
    WScript.Echo Usage
    WScript.Quit
End If

'
' IvBind Restore Detector params
'
Dim asIpAddress, zoneName, detectorName
asIpAddress = WScript.Arguments( 0 )
zoneName = WScript.Arguments( 1 )
detectorName = WScript.Arguments( 2 )

'
' Create an instance of IvBind COM object 
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2" )

On Error Resume Next

'
' Calling the Restore Detector action 
'
Call ivbind.RestoreDetector( asIpAddress, zoneName, detectorName )

'
' Display error message
'
If Err.number = 0 Then
    WScript.Echo "Restore detector successfully."
Else
    WScript.Echo "Error: " + Err.Description
End If
