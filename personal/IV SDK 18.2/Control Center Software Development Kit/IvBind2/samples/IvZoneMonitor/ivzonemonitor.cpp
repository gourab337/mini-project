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
static void CALLBACK OnZoneChange( 
                          void              *pUserParam,
                          int                nNotificationType,
                          IVBIND2_ZONEINFO2 *pZoneInfo,
                          const char        *pszAsIpAddress
                          );

static string FileTimeToString( const FILETIME &fileTime );
static string ZoneStateToString( const int nState );
static void WriteOutInfo( 
                 const int nNotificationType,
                 const IVBIND2_ZONEINFO2 *pZoneInfo,
                 const string& sWriteOut,
                 const char* pszAsIpAddress
                 );

static string ItemIdToString( const UINT64& idValue );

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
        cout << "\nUsage: IvZoneMonitor <IpAddress> " << endl;
        exit( 1 );
    }

    const char *pszArg = argv[ 1 ];
    if ( pszArg[ 0 ] == '?')
    {
        cout << "\nUsage: IvZoneMonitor <IpAddress> " << endl;
        exit( 1 );
    }
    //
    // Register for zone change from the Alarm Sever specified on the command line.
    //
    int nResult = IvBind2RegisterZoneChanges2( argv[ 1 ], &OnZoneChange, 0 );
    if ( nResult != IVBIND2_SUCCESS )
    {
        if ( nResult == IVBIND2_ERROR_BADPARAM ) 
        {
            cout << "Please specify a valid Alarm Server IP Address. " << endl;
            exit( 1 );
        }
        else
        {
            cout << "Failed to register for zone changes from " << argv[ 1 ] <<
                ": Error code " << nResult << endl;
            exit( 1 );
        }
    } 

    //
    //  Sit around until Ctrl-C is pressed
    //    
    signal( SIGINT, SignalHandler );
    signal( SIGTERM, SignalHandler );

    while ( !sg_fStop )
    {
        Sleep( 100 );
    }
    
    nResult = IvBind2UnregisterZoneChanges( argv[ 1 ] );
    if ( nResult != IVBIND2_SUCCESS )
    {
        cout << "Failed to unregister for zone changes from " << argv[ 1 ] <<
            ": Error code " << nResult << endl;
    }

    return 0;
}

//
// Event callback function
//
static void CALLBACK OnZoneChange( 
                          void              *pUserParam,
                          int                nNotificationType,
                          IVBIND2_ZONEINFO2 *pZoneInfo,                        
                          const char        *pszAsIpAddress
                          )
{
    switch ( nNotificationType )
    {
        case IVBIND2_NOTIFICATION_INSERTED:
            WriteOutInfo(
                nNotificationType, 
                pZoneInfo, 
                " is inserted into Alarm Server." ,
                pszAsIpAddress
                );
            break;

        case IVBIND2_NOTIFICATION_DELETED:
            WriteOutInfo( 
                nNotificationType, 
                pZoneInfo, 
                "" ,
                pszAsIpAddress
                );
            break;

        case IVBIND2_NOTIFICATION_CANCELLED:
            WriteOutInfo( 
                nNotificationType, 
                pZoneInfo, 
                "" ,
                pszAsIpAddress
                );
            break;

        case IVBIND2_PREPOPULATION_FETCHED:
            WriteOutInfo( 
                nNotificationType, 
                pZoneInfo, 
                " is prepopulated from Alarm Server " ,
                pszAsIpAddress
                );
            break;

        case IVBIND2_PREPOPULATION_COMPLETED:
            WriteOutInfo(
                nNotificationType, 
                pZoneInfo,
                "",
                pszAsIpAddress
                );
            break;
                
        case IVBIND2_NOTIFICATION_UPDATED:
            WriteOutInfo( 
                nNotificationType, 
                pZoneInfo, 
                " is updated from Alarm Server " ,
                pszAsIpAddress
                );
            break;

        case IVBIND2_NOTIFICATION_DISCONNECTED:
            {
                WriteOutInfo(
                    nNotificationType, 
                    pZoneInfo,
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
                pZoneInfo,
                " is unknown.",
                pszAsIpAddress
                );
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
//  Writeout zone information
//
static void WriteOutInfo( 
                 const int nNotificationType,
                 const IVBIND2_ZONEINFO2 *pZoneInfo,
                 const string& sWriteOut,
                 const char* pszAsIpAddress
                 )
{
    string asIpAddr( pszAsIpAddress );
    string name;
    
    if ( pZoneInfo != NULL )
    {
        name = std::string( pZoneInfo->szZoneName );
    }

    switch ( nNotificationType )
    {
    case IVBIND2_NOTIFICATION_DELETED:
        cout << "Zone with ID "
             << ItemIdToString( pZoneInfo->uZoneId )
             << " is deleted from Alarm Server "
             << asIpAddr
             << endl;
        break;

    case IVBIND2_NOTIFICATION_CANCELLED:
        cout << "Zone notification is cancelled from Alarm Server " 
             << asIpAddr << endl;
        break;

    case IVBIND2_NOTIFICATION_DISCONNECTED:
        cout
            << "The zone monitor is disconnected from the Alarm Server."
            << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout
            << "Zone prepopulation completed."
            << endl;
        break;

    default:
        cout << "Zone: "
             << name
             << " with ID "
             << ItemIdToString( pZoneInfo->uZoneId )
             << sWriteOut
             << " "
             << asIpAddr
             << endl
             << "Zone details: "
             << endl
             << " State: "
             << ZoneStateToString( pZoneInfo->nZoneState )
             << endl
             << " Priority: "
             << pZoneInfo->nPriority
             << endl
             << " Schedule Id: "
             << ItemIdToString( pZoneInfo->uScheduleId )
             << endl
             << " AlarmRecord Id: "
             << ItemIdToString( pZoneInfo->uAlarmRecordId )
             << endl
             << " Assigned User: "
             << ItemIdToString( pZoneInfo->uOwnerId )
             << endl;
        
        if ( pZoneInfo->nZoneState == IVBIND2_ZONESTATE_ALARM )
        {
            if ( IsTimeValid( pZoneInfo->ftTimeRaised ) )
            {
                cout << "Raised Time: "
                    << FileTimeToString( pZoneInfo->ftTimeRaised ) 
                    << endl;
            }
            else
            {
                cout << "Raised Time: " << "N/A" << endl;
            }
        }
        if ( pZoneInfo->nZoneState == IVBIND2_ZONESTATE_ACKNOWLEDGED )
        {
            if ( IsTimeValid( pZoneInfo->ftTimeAcknowledged ) )
            {
                cout << "Acknowledged Time: " 
                     << FileTimeToString( pZoneInfo->ftTimeAcknowledged ) 
                     << endl;
            }
            else
            {
                cout << "Acknowledged Time: " << "N/A" << endl;
            }
        }
    }

    cout << endl;
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
// Converts a zone state to a string
//
static string ZoneStateToString( const int nState )
{
    switch( nState )
    {
    case IVBIND2_ZONESTATE_SET:
        return "Set";
    case IVBIND2_ZONESTATE_UNSET:
        return "Unset";
    case IVBIND2_ZONESTATE_ALARM:
        return "Alarmed";
    case IVBIND2_ZONESTATE_ACKNOWLEDGED:
        return "Acknowledged";
    default:
        return "Unknown";
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
// Signal handler. Handles the Ctrl-C keypress.
//
static void SignalHandler( int nSignal )
{
    sg_fStop = true;
}
