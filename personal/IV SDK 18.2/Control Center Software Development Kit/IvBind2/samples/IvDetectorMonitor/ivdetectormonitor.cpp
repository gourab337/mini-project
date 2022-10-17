///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the ivzonemonitor sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2003-2010 IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <signal.h>
#include <iostream>
#include <string>
#include <sstream>

using namespace std;

//
// Forward function declarations
//
static void CALLBACK OnDetectorChange(
                          void                  *pUserParam,
                          int                    nNotificationType,
                          IVBIND2_DETECTORINFO2 *pDetectorInfo,
                          const char            *pszAsIpAddress
                          );

static string FileTimeToString( const FILETIME &fileTime );
static string DetectorAlarmToString( const int& nInAlarm );
static string DetectorStateToString( int nState );
static string DetectorTypeToString( int nType );
static string ExtraInfoTypeToString( int nType );
static string FaultTypeToString( int nType );
static void WriteOutInfo(
                 const int nNotificationType,
                 const IVBIND2_DETECTORINFO2 *pDetectorInfo,
                 const string& sWriteOut,
                 const char* pszAsIpAddress
                 );
static void WriteOutSourceDescription(
                  const int nSourceType,
                  const void* detSourceDesc
                  );
static void WriteOutExtraInfo( int extraInfoType, const void* extraInfo );

static string ItemIdToString( const UINT64& idValue );

static bool IsTimeValid( const FILETIME& ft );
static void SignalHandler( int nSignal );

//
// Static flag used to stop the program
//
static bool sg_fStop = false;

//
// Program entry function
//
int main( int argc, char *argv[] )
{
    if ( argc != 2 )
    {
        cout << "\nUsage: IvDetectorMonitor <IpAddress> " << endl;
        exit( 1 );
    }

    const char *pszArg = argv[ 1 ];

    if ( pszArg[ 0 ] == '?')
    {
        cout << "\nUsage: IvDetectorMonitor <IpAddress> " << endl;
        exit( 1 );
    }

    //
    // Register for detector changes from the Alarm Sever specified on the command line.
    //
    int nResult = IvBind2RegisterDetectorChanges2( argv[ 1 ], &OnDetectorChange, 0 );

    if ( nResult != IVBIND2_SUCCESS )
    {
        if ( nResult == IVBIND2_ERROR_BADPARAM )
        {
            cout << "Please specify a valid Alarm Server IP Address. " << endl;
            exit( 1 );
        }
        else
        {
            cout << "Failed to register for detector changes from "
                 << argv[ 1 ]
                 << ": Error code "
                 << nResult
                 << endl;
            exit( 1 );
        }
    }

    //
    // Sit around until Ctrl-C is pressed
    //
    signal( SIGINT, SignalHandler );
    signal( SIGTERM, SignalHandler );

    while ( !sg_fStop )
    {
        Sleep( 100 );
    }

    nResult = IvBind2UnregisterDetectorChanges( argv[ 1 ] );
    if ( nResult != IVBIND2_SUCCESS )
    {
        cout << "Failed to unregister for detector changes from " << argv[ 1 ] <<
            ": Error code " << nResult << endl;
    }

    return 0;
}

//
// Event callback function
//
static void CALLBACK OnDetectorChange(
                          void                  *pUserParam,
                          int                    nNotificationType,
                          IVBIND2_DETECTORINFO2 *pDetectorInfo,      
                          const char            *pszAsIpAddress
                          )
{
    string asIpAddr( pszAsIpAddress );

    switch ( nNotificationType )
    {
    case IVBIND2_NOTIFICATION_INSERTED:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            " is inserted into Alarm Server ",
            pszAsIpAddress
            );
        break;

    case IVBIND2_NOTIFICATION_DELETED:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            "",
            pszAsIpAddress
            );
        break;

    case IVBIND2_NOTIFICATION_CANCELLED:
        WriteOutInfo( 
            nNotificationType,
            pDetectorInfo,
            "",
            pszAsIpAddress
            );
        break;

    case IVBIND2_PREPOPULATION_FETCHED:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            " is prepopulated from Alarm Server",
            pszAsIpAddress
            );
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            "",
            pszAsIpAddress
            );
        break;

    case IVBIND2_NOTIFICATION_UPDATED:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            " is updated in Alarm Server",
            pszAsIpAddress
            );
        break;

    case IVBIND2_NOTIFICATION_DISCONNECTED:
        {
            WriteOutInfo(
                nNotificationType,
                pDetectorInfo,
                "",
                pszAsIpAddress
                );

            //
            // Stop running the monitor
            //
            sg_fStop = true;
        }
        break;

    default:
        WriteOutInfo(
            nNotificationType,
            pDetectorInfo,
            " is unknown.",
            pszAsIpAddress
            );
    }
}

//
//  Write out information for default case
//
static void WriteOutInfoDefault(
                const IVBIND2_DETECTORINFO2 *pDetectorInfo,
                const string& sWriteOut,
                const string& asIpAddr
                )
{
    std::string name( pDetectorInfo->szDetectorName );

    cout << "Detector: "
         << name
         << " with ID "
         << ItemIdToString( pDetectorInfo->uDetectorId )
         << sWriteOut
         << " "
         << asIpAddr
         << " in zone with ID "
         << ItemIdToString( pDetectorInfo->uZoneId )
         << endl;

    cout << "Detector detailed information: " << endl
         << "State: "
         << DetectorStateToString( pDetectorInfo->nDetectorState )
         << endl
         << "Activate source: " << endl;
    WriteOutSourceDescription( 
        pDetectorInfo->nActivateSourceType,
        pDetectorInfo->pActivateSource
        );

    cout << "Deactivate source: " << endl;
    WriteOutSourceDescription(
        pDetectorInfo->nDeactivateSourceType,
        pDetectorInfo->pDeactivateSource
        );

    cout << "Alarmable: "
         << pDetectorInfo->bAlarmable
         << endl
         << "In alarm: "
         << pDetectorInfo->bInAlarm
         << endl
         << "Dwell time: "
         << pDetectorInfo->uDwellTime
         << endl;
    cout << "Activated Time: ";
    if ( IsTimeValid( pDetectorInfo->ftActivationTime ) )
    {
        cout << FileTimeToString( pDetectorInfo->ftActivationTime ) << endl;
    }
    else
    {
        cout << "N/A" << endl;
    }

    cout << "Extra Info: " << endl;
    WriteOutExtraInfo(
        pDetectorInfo->nExtraInfoType,
        pDetectorInfo->pExtraInfo
        );
}

//
//  Writeout detector information
//
static void WriteOutInfo(
                 const int nNotificationType,
                 const IVBIND2_DETECTORINFO2 *pDetectorInfo,
                 const string& sWriteOut,
                 const char* pszAsIpAddress
                 )
{
    std::string asIpAddr( pszAsIpAddress );

    switch ( nNotificationType )
    {
    case IVBIND2_NOTIFICATION_DELETED:
        cout << "Detector with ID "
             << ItemIdToString( pDetectorInfo->uDetectorId )
             << " is deleted from Alarm Server "
             << asIpAddr
             << endl;
        break;

    case IVBIND2_NOTIFICATION_CANCELLED:
        cout << "Detector notification is cancelled from Alarm Server "
             << asIpAddr
             << endl;
        break;

    case IVBIND2_NOTIFICATION_DISCONNECTED:
        cout
            << "The detector monitor is disconnected from the Alarm Server."
            << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout << "Detector prepopulation completed."
            << endl;
        break;

    default:
        WriteOutInfoDefault(
            pDetectorInfo,
            sWriteOut,
            asIpAddr
            );
    }

    cout << endl;
}


//
//  Write out detector source description
//
static void WriteOutSourceDescription(
                                const int nSourceType,
                                const void* detSourceDesc      
                                )
{
    if ( nSourceType == IVBIND2_DETECTORTYPE_UNKNOWN )
    {
        cout << " Unknown" << endl;
    }
    else
    {
        cout << " Type: " << DetectorTypeToString( nSourceType ) << endl;
        if ( nSourceType == IVBIND2_DETECTORTYPE_BASICANALYTICS )
        {
            const IVBIND2_BASICANALYTICSSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_BASICANALYTICSSOURCEDESC* >(
                    detSourceDesc );

            cout << " Detector address: " << sourceDesc->szIpAddress << endl
                 << " Analytics source: " << sourceDesc->nBasicAnalyticsSource
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_DIGITALINPUT )
        {
            const IVBIND2_DIGITALINPUTSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_DIGITALINPUTSOURCEDESC* >(
                    detSourceDesc );

            cout << " Detector address: "
                 << sourceDesc->szIpAddress
                 << endl
                 << " Input: "
                 << sourceDesc->nInput
                 << endl
                 << " Normal PinState: "
                 << sourceDesc->bNormalPinState
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_EXTERNAL )
        {
            const IVBIND2_EXTERNALSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_EXTERNALSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector address: "
                 << sourceDesc->szIpAddress
                 << endl
                 << " Input: "
                 << sourceDesc->nInput
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_NETWORKFAULT )
        {
            const IVBIND2_NETWORKFAULTSOURCEDESC* sourceDesc = 
                reinterpret_cast< const IVBIND2_NETWORKFAULTSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector address: "
                 << sourceDesc->szIpAddress
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_VIDEOFAULT )
        {
            const IVBIND2_VIDEOFAULTSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_VIDEOFAULTSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector address: " << sourceDesc->szIpAddress << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_DOUBLEKNOCK )
        {
            const IVBIND2_DOUBLEKNOCKSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_DOUBLEKNOCKSOURCEDESC* >(
                    detSourceDesc );

            cout << " First Detector Id: "
                 << sourceDesc->uFirstDetectorId
                 << endl
                 << " Second Detector Id: "
                 << sourceDesc->uSecondDetectorId
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_UNHANDLEDALARM )
        {
            const IVBIND2_UNHANDLEDALARMSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_UNHANDLEDALARMSOURCEDESC* >(
                    detSourceDesc );

            cout << " Zone Id: "
                 << sourceDesc->uZoneId
                 << endl
                 << " Timeout: "
                 << sourceDesc->uTimeout
                 << "ms" << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_DEVICEFAULT )
        {
            const IVBIND2_DEVICEFAULTSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_DEVICEFAULTSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector address: "
                 << sourceDesc->szIpAddress
                 << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT )
        {
            const IVBIND2_ONVIFNETWORKFAULTSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_ONVIFNETWORKFAULTSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector service id: "
                << sourceDesc->szServiceId
                << endl;
        }
        else if ( nSourceType == IVBIND2_DETECTORTYPE_ONVIFEVENT )
        {
            const IVBIND2_ONVIFEVENTSOURCEDESC* sourceDesc =
                reinterpret_cast< const IVBIND2_ONVIFEVENTSOURCEDESC* >(
                    detSourceDesc
                    );

            cout << " Detector service id: "
                << sourceDesc->szServiceId
                << endl
                << " Event detector type: ";
 
            switch ( sourceDesc->nEventType )
            {
            case IVBIND2_ONVIFEVENTTYPE_DIGITALINPUT:
                cout << "Digital Input";
                break;
            case IVBIND2_ONVIFEVENTTYPE_BASICANALYTICS:
                cout << "Basic Analytics";
                break;
            case IVBIND2_ONVIFEVENTTYPE_ADVANCEDANALYTICS:
                cout << "Advanced Analytics";
                break;
            case IVBIND2_ONVIFEVENTTYPE_CYBERVIGILANT:
                cout << "CyberVigilant";
                break;
            case IVBIND2_ONVIFEVENTTYPE_UNKNOWN:
                cout << "Unknown";
                break;
            }

            cout << endl;
        }
    }
}

//
//  Write out detector extra info
//
static void WriteOutExtraInfo( int nExtraInfoType, const void* pExtraInfo )
{
    cout << "  Type: "
         << ExtraInfoTypeToString( nExtraInfoType )
         << endl;
    if ( nExtraInfoType == IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULT )
    {
        const IVBIND2_NVRFAULTINFO* faultInfo =
            reinterpret_cast< const IVBIND2_NVRFAULTINFO* >( pExtraInfo );

        cout << "  Number Faults: " << faultInfo->nNumFaults << endl;
        for( int i = 0; i < faultInfo->nNumFaults; ++i )
        {
            cout << "  Fault " << i << ": "
                 << FaultTypeToString( faultInfo->pFaultTypes[ i ] )
                 << endl;
        }
    }
    else if ( nExtraInfoType == IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON )
    {
        const IVBIND2_ISOLATEREASON* isolateInfo =
            reinterpret_cast< const IVBIND2_ISOLATEREASON* >( pExtraInfo );
        cout << "  Isolate Reason: " << isolateInfo->szIsolateReason << endl;
    }
    else if (
        nExtraInfoType == IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION )
    {
        const IVBIND2_ANNOTATIONMSG* annotationMsg =
            reinterpret_cast< const IVBIND2_ANNOTATIONMSG* >( pExtraInfo );
        cout << "  Annotation Info: " << annotationMsg->szAnnotationMsg << endl;
    }
}

//
// If idValue is zero, return "N/A" instead
// If idValue is the max value of the datatype, return "Invalid" instead
//
static string ItemIdToString( const UINT64& idValue )
{
    if ( idValue == 0 )
    {
        return string( "N/A" );
    }
    else if ( idValue == ULLONG_MAX )
    {
        return string( "Invalid" );
    }
    else
    {
        ostringstream strStream;
        strStream << idValue;
        return strStream.str();
    }
}

//
// Formats a timestamp for output
//
static string FileTimeToString( const FILETIME &fileTime )
{
    if ( fileTime.dwHighDateTime != 0 )
    {
        TIME_ZONE_INFORMATION timeZoneInfo;
        SYSTEMTIME            sysTime, localTime;
        char                  szTime[ 128 ];

        FileTimeToSystemTime( &fileTime, &sysTime );

        GetTimeZoneInformation( &timeZoneInfo );
        SystemTimeToTzSpecificLocalTime( &timeZoneInfo, &sysTime, &localTime );

        sprintf(
            szTime,
            "%04d-%02d-%02d %02d:%02d:%02d.%03d",
            (int) localTime.wYear,
            (int) localTime.wMonth,
            (int) localTime.wDay,
            (int) localTime.wHour,
            (int) localTime.wMinute,
            (int) localTime.wSecond,
            (int) localTime.wMilliseconds
            );

        return string( szTime );
    }
    else
    {
        return "(null)";
    }
}

//
//  Validate time (whether time is 1970-01-01 00:00:00 or not )
//
static bool IsTimeValid( const FILETIME& ft )
{
    //
    //  Create file time of 1970-01-01 00:00:00
    //
    time_t t = 0;
    FILETIME invalidFt;
    LONGLONG ll = Int32x32To64(t, 10000000) + 116444736000000000;
    invalidFt.dwLowDateTime  = (DWORD) ll;
    invalidFt.dwHighDateTime = (DWORD) (ll >> 32);

    if ( CompareFileTime( &invalidFt, &ft ) == 0 )
    {
        return false;
    }
    else
    {
        return true;
    }

}

//
// Converts a detector state to a string for output
//
static string DetectorStateToString( int nState )
{
    switch( nState )
    {
        case IVBIND2_DETECTORSTATE_DISABLED:
            return "Disabled";
        case IVBIND2_DETECTORSTATE_ENABLED:
            return "Enabled";
        case IVBIND2_DETECTORSTATE_TRIGGERED:
            return "Triggered";
        case IVBIND2_DETECTORSTATE_TAMPERED:
            return "Tampered";
        default:
            return "Unknown";
    }
}

//
// Formats an detector type for output
//
static string DetectorTypeToString( int nType )
{
    switch ( nType )
    {
    case IVBIND2_DETECTORTYPE_BASICANALYTICS:
        return "Basic Analytics";

    case IVBIND2_DETECTORTYPE_DIGITALINPUT:
        return "Digital Input";

    case IVBIND2_DETECTORTYPE_EXTERNAL:
        return "External";

    case IVBIND2_DETECTORTYPE_NETWORKFAULT:
        return "Network Fault";

    case IVBIND2_DETECTORTYPE_VIDEOFAULT:
        return "Video Fault";

    case IVBIND2_DETECTORTYPE_DOUBLEKNOCK:
        return "Double Knock";

    case IVBIND2_DETECTORTYPE_UNHANDLEDALARM:
        return "Unhandled Alarm";

    case IVBIND2_DETECTORTYPE_DEVICEFAULT:
        return "Device Fault";

    case IVBIND2_DETECTORTYPE_NONE:
        return "None";

    case IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT:
        return "ONVIF Network Fault";

    case IVBIND2_DETECTORTYPE_ONVIFEVENT:
        return "ONVIF Event";

    default:
        return "Unknown type";
    }
}

//
// Convert an extra info type into a string
//
static string ExtraInfoTypeToString( int nType )
{
    switch( nType )
    {
    case IVBIND2_DETECTOREXTRAINFOTYPE_NONE:
        return "None";
    case IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULT:
        return "Nvr Fault Info";
    case IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON:
        return "Isolate Annotation";
    case IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION:
        return "Activation Annotation";
    default:
        return "Unknown";
    }
}

//
// Convert a fault type into a string
//
static string FaultTypeToString( int nType )
{
    switch( nType )
    {
    case IVBIND2_NVRFAULTTYPE_RECORDINGFAILURE:
        return "Recording Failure";
    case IVBIND2_NVRFAULTTYPE_LICENSEFAILURE:
        return "License Failure";
    case IVBIND2_NVRFAULTTYPE_RAIDDEGRADED:
        return "Raid Degraded";
    case IVBIND2_NVRFAULTTYPE_REDUNDANTPOWERFAIL:
        return "Redundant Power Failure";
    case IVBIND2_NVRFAULTTYPE_REDUNDANTNETWORKFAIL:
        return "Redundant Network Failure";
    case IVBIND2_NVRFAULTTYPE_DEVICEOFFLINE:
        return "Device Offline";
    case IVBIND2_NVRFAULTTYPE_UPSONBATTERY:
        return "UPS On Battery";
    case IVBIND2_NVRFAULTTYPE_FANFAILURE:
        return "Fan Failure";
    case IVBIND2_NVRFAULTTYPE_SYSTEMOVERTEMP:
        return "System Over Temperature";
    case IVBIND2_NVRFAULTTYPE_DISKOVERTEMP:
        return "Disk Over Temperature";
    case IVBIND2_NVRFAULTTYPE_STORAGEARRAYMONITORINGFAILURE:
        return "Storage Array Monitoring Failure";
    case IVBIND2_NVRFAULTTYPE_STORAGEARRAYDISKFAILURE:
        return "Storage Array Disk Failure";
    case IVBIND2_NVRFAULTTYPE_STORAGEARRAYREDUNDANCYFAILURE:
        return "Storage Array Redundancy Failure";
    case IVBIND2_NVRFAULTTYPE_STORAGEARRAYENCLOSUREFAILURE:
        return "Storage Array Enclosure Failure";
    case IVBIND2_NVRFAULTTYPE_LOWDISKSPACE:
        return "Disk Space Too Low";
    default:
        return "Unknown";
    }
}

//
// Signal handler. Handles the Ctrl-C keypress.
//
static void SignalHandler( int nSignal )
{
    sg_fStop = true;
}
