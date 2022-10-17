///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the IvAlarmQuery sample application
//!
//!         This application demonstrates the use of the
//!         IvBind2QueryAlarmRecords and the IvBind2StopAlarmQuery
//!         functions for querying for historical alarm records and
//!         monitoring an Alarm Server for new alarms.
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <signal.h>
#include <iostream>
#include <string>
#include <sstream>
#include <vector>

static const WORD MIN_YEAR = 1601;
static const WORD MAX_YEAR = 30827;

using namespace std;

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief Class used to parse and store command line options
//!
///////////////////////////////////////////////////////////////////////////////
class CmdOptions
{
public:
    CmdOptions( int argc, char* argv[] );

public:
    char*    m_asIpAddress;
    bool     m_minimumTimeSupplied;
    FILETIME m_minimumTime;
    bool     m_maximumTimeSupplied;
    FILETIME m_maximumTime;

private:
    bool StringToFileTime(
        const std::string& time,
        FILETIME& fileTime
        );
    void Tokenize(
        const std::string& str,
        std::vector< std::string >& tokens,
        const std::string& delimiters
        );
    bool StringDateToSysTime(
        const std::vector< std::string >& date,
        SYSTEMTIME& fileTime
        );
    bool ValidateTimeValue(
        WORD value,
        WORD minValue,
        WORD maxValue
        );
    bool IsLeapYear( WORD year );
    bool ValidateYearMonthDayValue(
        WORD year,
        WORD month,
        WORD day
        );
};

static void PrintUsage();

static void CALLBACK OnAlarm(
    void* userParam,
    int notificationType,
    IVBIND2_ALARMRECORDINFO* alarmInfo,
    const char* asIpAddress
    );

static string FileTimeToString( const FILETIME &fileTime );

static string AlarmStateToString( const int state );

static string ItemIdToString( const UINT64& idValue );

static void SignalHandler( int signal );

static bool IsTimeValid( const FILETIME& ft );

// Static flag used to stop the program
static bool sg_stop = false;

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
CmdOptions::CmdOptions( int argc, char* argv[] ) :
m_asIpAddress( NULL ),
m_minimumTimeSupplied( false ),
m_maximumTimeSupplied( false )
{
    memset( &m_minimumTime, 0, sizeof( FILETIME ) );
    memset( &m_maximumTime, 0, sizeof( FILETIME ) );

    int nArg = 0;
    std::string strTime;
    while ( ++nArg < argc )
    {
        const char *pszArg = argv[ nArg ];

        if ( pszArg[ 0 ] == '-' || pszArg[ 0 ] == '/' )
        {
            if ( strcmp( &pszArg[ 1 ], "l" ) == 0
                || strcmp( &pszArg[ 1 ], "u" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    cout << "-" << pszArg[ 1 ]
                        << ": Please specify a time stamp"
                        << endl;
                    PrintUsage();
                    exit( 1 );
                }

                strTime = argv[ nArg ];
                FILETIME tempTime;
                if ( !StringToFileTime( strTime, tempTime ) )
                {
                    cout << "-" << pszArg[ 1 ]
                        << ": Please specify a valid time stamp"
                        << endl;
                    PrintUsage();
                    exit( 1 );
                }
                if ( strcmp( &pszArg[ 1 ], "l" ) == 0 )
                {
                    m_minimumTime = tempTime;
                    m_minimumTimeSupplied = true;
                }
                else
                {
                    m_maximumTime = tempTime;
                    m_maximumTimeSupplied = true;
                }
            }
            else
            {
                cout << pszArg << ": Unknown argument" << endl;
                PrintUsage();
                exit( 1 );
            }
        }
        else
        {
            if ( m_asIpAddress == NULL )
            {
                m_asIpAddress = argv[ nArg ];
            }
            else
            {
                cout << pszArg << ": Unexpected argument" << endl;
                PrintUsage();
                exit( 1 );
            }
        }
    }

    if ( m_asIpAddress == NULL )
    {
        cout << "Missing Alarm Server IP address" << endl;
        PrintUsage();
        exit( 1 );
    }
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Convert string argument to FILETIME
//! 
//! \param  time      String representing the time to convert
//! \param  fileTime  Converted time
//!
//! \return True if successfully converted
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::StringToFileTime(
    const std::string& time,
    FILETIME& fileTime
    )
{
    //
    // Convert to SysTime object
    //
    std::vector< std::string > tokens;
    Tokenize( time, tokens, ":" );
    if ( tokens.size() != 7 )
    {
        // Invalid time format
        return false;
    }

    SYSTEMTIME sysTime;
    bool success = StringDateToSysTime( tokens, sysTime );

    if ( success )
    {
        //
        // Convert local time to UTC
        //
        TIME_ZONE_INFORMATION timeZoneInfo;
        SYSTEMTIME utcTime = { 0 };

        GetTimeZoneInformation( &timeZoneInfo );
        TzSpecificLocalTimeToSystemTime( 
            &timeZoneInfo,
            &sysTime,
            &utcTime
            );
        SystemTimeToFileTime( &utcTime, &fileTime );
    }

    return success;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Tokenize string with delimiter
//! 
//! \param  str         String to be tokenized
//! \param  tokens      Vector of tokens parsed from the string
//! \param  delimiters  String containing characters to be used as delimiters
//!
///////////////////////////////////////////////////////////////////////////////
void CmdOptions::Tokenize(
    const std::string& str,
    std::vector< std::string >& tokens,
    const std::string& delimiters
    )
{
    // Skip delimiters at beginning
    string::size_type lastPos = str.find_first_not_of( delimiters, 0 );

    // Find first "non-delimiter"
    string::size_type pos = str.find_first_of( delimiters, lastPos );

    while (string::npos != pos || string::npos != lastPos)
    {
        // Found a token, add it to the vector.
        tokens.push_back( str.substr( lastPos, pos - lastPos ) );

        // Skip delimiters.  Note the "not_of"
        lastPos = str.find_first_not_of( delimiters, pos );

        // Find next "non-delimiter"
        pos = str.find_first_of( delimiters, lastPos );
    }
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Convert string to SYSTEMTIME
//! 
//! \param  date    Data string to convert
//! \param  sysTime Converted time
//!
//! \return Convert successfully or not.
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::StringDateToSysTime(
    const std::vector< std::string >& date,
    SYSTEMTIME& sysTime
    )
{
    bool success = true;

    sysTime.wYear   = atoi( date.at( 0 ).c_str() );
    sysTime.wMonth  = atoi( date.at( 1 ).c_str() );
    sysTime.wDay    = atoi( date.at( 2 ).c_str() );
    sysTime.wHour   = atoi( date.at( 3 ).c_str() );
    sysTime.wMinute = atoi( date.at( 4 ).c_str() );
    sysTime.wSecond = atoi( date.at( 5 ).c_str() );
    sysTime.wMilliseconds = atoi( date.at( 6 ).c_str() );

    success = ValidateYearMonthDayValue(
        sysTime.wYear, 
        sysTime.wMonth, 
        sysTime.wDay 
        );
    success = ValidateTimeValue( sysTime.wHour, 0, 23 ) && success;
    success = ValidateTimeValue( sysTime.wMinute, 0, 59 ) && success;
    success = ValidateTimeValue( sysTime.wSecond, 0, 59 ) && success;
    success = ValidateTimeValue( sysTime.wMilliseconds, 0, 999 ) && success;

    return success;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Check whether a particluar year is a leap year or not
//! 
//! \param  year     Year
//!
//! \return true if it is leap year
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::IsLeapYear( WORD year )
{
    return ( ( year % 400 == 0 ) || 
           ( ( year % 100 != 0 ) && ( year % 4 == 0 ) ) );
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Validate year, month and day time value
//!
//! \param  year      Year
//! \param  month     Month
//! \param  day       Day
//!
//! \return true if day corresponds correctly to month and year
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::ValidateYearMonthDayValue(
    WORD year,
    WORD month,
    WORD day
    )
{
    bool success = true;

    success = ValidateTimeValue( year, MIN_YEAR, MAX_YEAR );
    success = ValidateTimeValue( month, 1, 12 ) && success;
    success = ValidateTimeValue( day, 1, 31 ) && success;

    if ( !success )
    {
        return success;
    }
    else
    {
        switch ( month )
        {
        case 2:
            success = ( day <= 28 ) ||
                      ( ( day == 29 ) && ( IsLeapYear( year ) ) );
            break;
        case 4:
        case 6:
        case 9:
        case 11:
            success = ( day <= 30 );
            break;
        default:
            success = true;
        }
    }

    return success;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Validate time value
//! 
//! \param  value     Time value need checked
//! \param  minValue  Min value
//! \param  maxValue  Max value
//!
//! \return true if value belong to range minValue-maxValue; otherwise false
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::ValidateTimeValue(
    WORD value,
    WORD minValue,
    WORD maxValue
    )
{
    bool success = false;
    if ( ( minValue <= maxValue ) &&
         ( value >= minValue ) && 
         ( value <= maxValue ) )
    {
        success = true;
    }

    return success;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Alarm callback function.
//!
//!         Called by the Binding Kit when a alarm record notification is
//!         received from the registered alarm server. This will print out the
//!         details of the callback parameters to the console.
//!
//! \param userParam        Parameter set when registering for alarms
//! \param notificationType The type of notification
//! \param alarmInfo        The alarm information object
//! \param asIpAddress      The IP address of the alarm server
//!
///////////////////////////////////////////////////////////////////////////////
static void CALLBACK OnAlarm(
    void* userParam,
    int notificationType,
    IVBIND2_ALARMRECORDINFO* alarmInfo,
    const char* asIpAddress
    )
{
    string asIpAddr( asIpAddress );
    string alarmChange = "";
    bool expandAlarmInfo = false;

    switch ( notificationType )
    {
    case IVBIND2_NOTIFICATION_CANCELLED:
        cout    << "The Alarm Server "
                << asIpAddr
                << " has cancelled the alarm query."
                << endl;

        // Stop running the monitor
        sg_stop = true;
        break;

    case IVBIND2_NOTIFICATION_DISCONNECTED:
        cout    << "The connection to Alarm Server "
                << asIpAddr
                << " has been lost."
                << endl;

        // Stop running the monitor
        sg_stop = true;
        break;

    case IVBIND2_NOTIFICATION_DELETED:
        cout    << "Alarm with ID "
                << ItemIdToString( alarmInfo->uAlarmRecordId )
                << " has been deleted from Alarm Server "
                << asIpAddr
                << "."
                << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout    << "Alarm prepopulation completed."
                << endl
                << "All historical alarm records have been sent."
                << endl;
        break;

    case IVBIND2_NOTIFICATION_UPDATED:
        alarmChange = " has been updated on";
        expandAlarmInfo = true;
        break;

    case IVBIND2_PREPOPULATION_FETCHED:
        alarmChange = " was previously raised on";
        expandAlarmInfo = true;
        break;

    case IVBIND2_NOTIFICATION_INSERTED:
        alarmChange = " has just been raised on";
        expandAlarmInfo = true;
        break;

    default:
        cout    << "Alarm with ID "
                << ItemIdToString( alarmInfo->uAlarmRecordId )
                << " has been sent as an unknown notification type from "
                << asIpAddr
                << "."
                << endl;
    }

    if ( expandAlarmInfo )
    {
        cout    << "Alarm ID: "
                << ItemIdToString( alarmInfo->uAlarmRecordId )
                << alarmChange
                << " "
                << asIpAddr
                << endl
                << "Alarm details:"
                << endl
                << " State: "
                << AlarmStateToString( alarmInfo->nAlarmState )
                << endl
                << " Zone ID: "
                << ItemIdToString( alarmInfo->uZoneId )
                << endl
                << " Owner ID: "
                << ItemIdToString( alarmInfo->uOwnerId )
                << endl
                << " Time Raised: "
                << FileTimeToString( alarmInfo->ftTimeRaised )
                << endl;

        if ( alarmInfo->nAlarmState == IVBIND2_ALARMSTATE_CLEARED )
        {
            if ( IsTimeValid( alarmInfo->ftTimeCleared ) )
            {
                cout    << " Cleared Time: "
                        << FileTimeToString( alarmInfo->ftTimeCleared )
                        << endl;
            }
            else
            {
                cout << "Cleared Time: " << "N/A" << endl;
            }
        }
    }

    cout << endl;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Validate FILETIME to see if it is 1970-01-01 00:00:00 or not 
//!
//! \param ft   The time to check
//!
//! \return true if the filetime is valid
//!
///////////////////////////////////////////////////////////////////////////////
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

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Convert an Alarm State to a string
//!
//! \param state     The Alarm State to convert
//!
//! \return The state as a string
//!
///////////////////////////////////////////////////////////////////////////////
static string AlarmStateToString( const int state )
{
    switch( state )
    {
    case IVBIND2_ALARMSTATE_NEW:
        return "New";
    case IVBIND2_ALARMSTATE_ACKNOWLEDGED:
        return "Acknowledged";
    case IVBIND2_ALARMSTATE_CLEARED:
        return "Cleared";
    default:
        return "Unknown";
    }
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Format an ItemId for output on the console
//!
//! \param idValue     The ItemId to convert
//!
//! \return The ID as a human readable string. If the ID was zero, "N/A" is
//!         returned
//!
///////////////////////////////////////////////////////////////////////////////
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

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Format a timestamp for output on the console
//!
//! \param fileTime     The timestamp to format
//!
//! \return The timestamp as a human readable string
//!
///////////////////////////////////////////////////////////////////////////////
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

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Displays usage information
//!
///////////////////////////////////////////////////////////////////////////////
static void PrintUsage()
{
    cout << "\nUsage: IvAlarmQuery <options> <AsIpAddress>" << endl;
    cout << "\nAt least one of the -l or -u arguments must be provided." << endl;
    cout << "\nOptions:" << endl;
    cout << "    -l <Timestamp>     The minimum alarm raised time" << endl;
    cout << "    -u <Timestamp>     The maximum alarm raised time" << endl;
    cout << "\nTimestamp Format:" << endl;
    cout << "    \"Year(YYYY):Month(01-12):Day(01-31):" << endl;
    cout << "    Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)\"" << endl;
    cout << endl;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Signal handler
//!
//! \param signal   Ingored. Any caught signal will stop the monitor.
//!
///////////////////////////////////////////////////////////////////////////////
static void SignalHandler( int signal )
{
    sg_stop = true;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Program entry function
//!
///////////////////////////////////////////////////////////////////////////////
int main( int argc, char *argv[] )
{
    CmdOptions options = CmdOptions( argc, argv );

    //
    // Register for alarms from the Alarm Sever specified on the command line.
    //
    int result = IvBind2QueryAlarmRecords(
        options.m_asIpAddress,
        &OnAlarm,
        NULL,
        options.m_minimumTimeSupplied ? &options.m_minimumTime : NULL,
        options.m_maximumTimeSupplied ? &options.m_maximumTime : NULL
        );
    if ( result != IVBIND2_SUCCESS )
    {
        if ( result == IVBIND2_ERROR_BADPARAM ) 
        {
            cout << "One or more of your supplied parameters are invalid"
                << endl;
            PrintUsage();
            exit( 1 );
        }
        else
        {
            string asIpAddr( options.m_asIpAddress );
            cout << "Failed to query for alarms from " << asIpAddr <<
                ": Error code " << result << endl;
            exit( 1 );
        }
    } 

    //
    // Sit around until Ctrl-C is pressed
    //
    signal( SIGINT, SignalHandler );
    signal( SIGTERM, SignalHandler );

    while ( !sg_stop )
    {
        Sleep( 100 );
    }
    
    result = IvBind2StopAlarmRecordQuery( options.m_asIpAddress );
    if ( result != IVBIND2_SUCCESS )
    {
        string asIpAddr( options.m_asIpAddress );
        cout << "Failed to stop query for alarms from " << asIpAddr <<
            ": Error code " << result << endl;
        exit( 1 );
    }

    return 0;
}
