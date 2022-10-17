'
' IvIsolateDetector.vbs    Sample vbscript for the IvBind COM component.
'
' IvIsolateDetector Isolate detector with specific name
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script
'
Dim Usage
Usage = "Usage: IvIsolateDetector.vbs <AS IP Address> <Zone Name> <Detector Name> <Isolate Reason>" _
    + vbCrlf + vbCrlf _
    + "e.g. IvIsolateDetector.vbs 192.168.1.1 ""New Zone 1"" ""New Detector 1"" ""Faulty Detector""" _
    + vbCrLf _
    + "     Isolate detector ""New Detector 1"" in zone ""New Zone 1""" _
    + " from the Alarm Server with IP address 192.168.1.1, because of ""Faulty Detector"""
  
'
' Parse the command-line arguments
'
If WScript.Arguments.Count <> 4 Then
    WScript.Echo Usage
    WScript.Quit
End If

'
' IvBind Isolate Detector params
'
Dim asIpAddress, zoneName, detectorName, isolateReason
asIpAddress = WScript.Arguments( 0 )
zoneName = WScript.Arguments( 1 )
detectorName = WScript.Arguments( 2 )
isolateReason = WScript.Arguments( 3 )

'
' Create an instance of IvBind COM object 
'
Dim ivbind
Set ivbind = WScript.CreateObject( "SIK2.IvBind2" )

On Error Resume Next

'
' Calling the Isolate Detector action 
'
Call ivbind.IsolateDetector2( asIpAddress, zoneName, detectorName, isolateReason )

'
' Display error message
'
If Err.number = 0 Then
    WScript.Echo "Isolate detector successfully."
Else
    WScript.Echo "Error: " + Err.Description
End If
