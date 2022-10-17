'
' IvExternalDataSourceQuery.vbs  Sample vbscript for the IvBind COM component.
'
' IvExternalDataSourceQuery outputs external data source notifications from a
' specified alarm server to the console.
' Ensure that the IvBind COM component has been registered with Windows.
'
Option Explicit

'
' Usage information for this script
'
Dim Usage
Usage = _
    "Usage: CScript.exe IvExternalDataSourceQuery.vbs /a:<AS IP Address>"

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
Dim asIpAddress
Dim arguments, numValidArguments
Set arguments = WSCript.Arguments.Named

numValidArguments = 0

'
' Get the Alarm Server IP Address
'
If arguments.Exists( "a" ) Then
    asIpAddress = arguments.Item( "a" )
    numValidArguments = numValidArguments + 1
Else
    WScript.Echo Usage
    WScript.Quit( 1 )
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
' Create a External Data Source Query using the Binding Kit
'
Call ivbind.QueryExternalDataSources( asIpAddress )
If Err.number <> 0 Then
    WScript.Echo "QueryExternalDataSources returned error: " + Err.Description
    WScript.Quit( 1 )
Else
    WScript.Echo "Successfully created External Data Source query"
End If

'
' Wait for External Data Sources
'
Do While True
    WScript.sleep 5000
Loop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnExternalDataSourcePrepopulate event handler
'
' Outputs all properties of the external data source to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourcePrepopulate( source )
    Dim message

    message = "Existing External Data Source notification received " _
        + "from Alarm Server " + source.ASIpAddress + vbCrLf _
        + ExteralDataSourceToString( source )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnExternalDataSourcePrepopulateComplete event handler
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourcePrepopulateComplete( asIpAddr )
    Dim message

    message = "External Data Source prepopulation completed from server " _
        + asIpAddr + vbCrLf _
        + "All existing External Data Sources have been sent." + vbCrLf

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnAlarmRecordNew event handler
'
' Outputs all properties of the new external data source to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourceNew( source )
    Dim message

    message = "External Data Source insertion notification received " _
        + "from server " + source.ASIpAddress + vbCrLf _
        + ExteralDataSourceToString( source )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' IvBind2_OnExternalDataSourceUpdate event handler
'
' Outputs all properties of the updated external data source to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourceUpdate( source )
    Dim message

    message = "External Data Source update notification received from " _
        + "server " + source.ASIpAddress + vbCrLf _
        + ExteralDataSourceToString( source )

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnExternalDataSourceDelete event handler
'
' Outputs all properties of the deleted external data source to the console
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourceDelete( source )
    Dim message

    message = "External Data Source with ID = " _
        + ShowCorrectId( source.DataSourceId ) _
        + " has been deleted from Alarm Server " _
        + source.ASIpAddress + vbCrLf

    WScript.Echo message
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnExternalDataSourcRequestCancel event handler
'
' Outputs cancellation message then exits.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourcRequestCancel( asIpAddr )
    Dim message

    message = "External Data Source query with Alarm Server " _
        + asIpAddr + " has been cancelled." + vbCrLf
    WScript.Echo message
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' OnExternalDataSourceServerDisconnect event handler
'
' Outputs a disconnected message, then exit the monitor.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub IvBind2_OnExternalDataSourceServerDisconnect( asIpAddr )
    Dim message

    message = "The connection to Alarm Server " _
        + asIpAddr + " has been lost." + vbCrLf
    WScript.Echo message
    WScript.Quit( 1 )
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Output an ID number
'
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
' Generate a string containing the details of an external data source
'
' Returns a string containing the external data source details
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function ExteralDataSourceToString( source )
    With source
        ExteralDataSourceToString = "External Data Source details:" + vbCrLf _
            + " Data Source ID = " + ShowCorrectId( .DataSourceId ) + vbCrLf _
            + " Data Source Name = " + .DataSourceName + vbCrLf _
            + " IP Address = " + .IpAddress + vbCrLf _
            + " Source Number = " + CStr( .SourceNumber ) + vbCrLf
    End With
End Function
