'
' IvExternalRelay.vbs        Sample vbscript for the IvBind COM component.
'
' IvExternalRelay sets up an external relay and outputs notifications
' when it's pin states are changed
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Define usage display message for this script 
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvExternalRelay.vbs <Local IP Address>" + vbCrLf + vbCrLf _
    + "  e.g. CScript.exe IvExternalRelay.vbs 192.168.1.1 " + vbCrLf _
    + "       Setup a relay on the local IP address 192.168.1.1 " 
  
' Ensure that this script is being invoked using the CScript host executable
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
' Parse the command-line arguments for IP addresses
'
If WScript.Arguments.Count <> 1 Then
    WScript.Echo Usage 
    WScript.Quit( 1 )
End If

On Error Resume Next
'
' Create an external relay  
'
Dim localIpAddress
localIpAddress = WScript.Arguments( 0 )
Call ivbind.ReceiveExternalRelayNotifications( localIpAddress )
If Err.number <> 0 Then
    WScript.Echo "Error: " + Err.Description
Else	
    WScript.Echo "Created external relay succesfully"
End If
    
'
' Wait for a relay state notification
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnRelayStateChange event handler
'
' Outputs the details of the state change to console
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnRelayStateChange( pinInformation )
    On Error Resume Next

    Dim setString 
    setString = " Unset"
    If pinInformation.State Then
        setString = " Set"
    End If

    WScript.Echo "Received Message from " + pinInformation.RemoteIP + ": Pin " _
        + CStr( pinInformation.PinNumber ) + setString

End Sub


