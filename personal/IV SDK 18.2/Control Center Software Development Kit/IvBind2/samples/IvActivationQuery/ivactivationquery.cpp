///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the IvActivationQuery sample application
//!
//!         This application demonstrates the use of the
//!         IvBind2QueryActivationRecords and the IvBind2StopActivationQuery
//!         functions for querying for historical activation records and
//!         monitoring an Alarm Server for new activations.
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

static void CALLBACK OnActivation(
    void* userParam,
    int notificationType,
    IVBIND2_ACTIVATIONRECORDINFO* activationInfo,
    const char* asIpAddress
    );

static void WriteOutNvrFaultInfo( const IVBIND2_NVRFAULTINFO* faultInfo );

static string FaultTypeToString( int type );

static string FileTimeToString( const FILETIME &fileTime );

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
//! \brief  Activation callback function.
//!
//!         Called by the Binding Kit when an activation record notification is
//!         received from the registered Alarm Server. This will print out the
//!         details of the callback parameters to the console.
//!
//! \param userParam        Parameter set when registering for activations
//! \param notificationType The type of notification
//! \param activationInfo   The activation record information object
//! \param asIpAddress      The IP address of the Alarm Server
//!
///////////////////////////////////////////////////////////////////////////////
static void CALLBACK OnActivation(
    void* userParam,
    int notificationType,
    IVBIND2_ACTIVATIONRECORDINFO* activationInfo,
    const char* asIpAddress
    )
{
    string asIpAddr( asIpAddress );
    string actChange = "";
    bool expandActivationInfo = false;

    switch ( notificationType )
    {
    case IVBIND2_NOTIFICATION_CANCELLED:
        cout    << "The Alarm Server "
                << asIpAddr
                << " has cancelled the activation query."
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
        cout    << "Activation with ID "
                << ItemIdToString( activationInfo->uActivationRecordId )
                << " has been deleted from Alarm Server "
                << asIpAddr
                << "."
                << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout    << "Activation prepopulation completed."
                << endl
                << "All historical activation records have been sent."
                << endl;
        break;

    case IVBIND2_NOTIFICATION_UPDATED:
        actChange = " has been updated on";
        expandActivationInfo = true;
        break;

    case IVBIND2_PREPOPULATION_FETCHED:
        actChange = " was previously created on";
        expandActivationInfo = true;
        break;

    case IVBIND2_NOTIFICATION_INSERTED:
        actChange = " has just been created on";
        expandActivationInfo = true;
        break;

    default:
        cout    << "Activation with ID "
                << ItemIdToString( activationInfo->uActivationRecordId )
                << " has been sent as an unknown notification type from "
                << asIpAddr
                << "."
                << endl;
    }

    if ( expandActivationInfo )
    {
        cout    << "Activation ID: "
                << ItemIdToString( activationInfo->uActivationRecordId )
                << actChange
                << " "
                << asIpAddr
                << endl
                << "Activation details:"
                << endl
                << " Detector ID: "
                << ItemIdToString( activationInfo->uDetectorId )
                << endl
                << " Activation Time: "
                << FileTimeToString( activationInfo->ftActivationTime )
                << endl
                << " Zone ID: "
                << ItemIdToString( activationInfo->uZoneId )
                << endl
                << " Alarm Time: "
                << FileTimeToString( activationInfo->ftAlarmTime )
                << endl
                << " Alarm Record ID: "
                << ItemIdToString( activationInfo->uAlarmRecordId )
                << endl
                << " Extra Activation Information: ";

        switch ( activationInfo->nExtraInfoType )
        {
        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_NONE:
            cout << "None" << endl;
            break;

        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_NVRFAULT:
        {
            cout << endl;
            const IVBIND2_NVRFAULTINFO* faultInfo =
                reinterpret_cast< const IVBIND2_NVRFAULTINFO* >(
                    activationInfo->pExtraInfo );
            WriteOutNvrFaultInfo( faultInfo );
        }
        break;

        case IVBIND2_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION:
        {
            cout << endl;
            const IVBIND2_ANNOTATIONMSG* annotationMsg =
                reinterpret_cast< const IVBIND2_ANNOTATIONMSG* >(
                    activationInfo->pExtraInfo );
            cout << "    Type: " << "Activation Annotation" << endl;
            cout << "    Annotation Info: " << annotationMsg->szAnnotationMsg
                 << endl;
        }
        break;

        default:
            cout << "Unknown" << endl;
        }
    }

    cout << endl;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Output the contents of a IVBIND2_NVRFAULTINFO struct to the console
//!
//! \param faultInfo    The fault info struct to output
//!
///////////////////////////////////////////////////////////////////////////////
static void WriteOutNvrFaultInfo( const IVBIND2_NVRFAULTINFO* faultInfo )
{
    cout << "    Number Faults: " << faultInfo->nNumFaults << endl;
    for( int i = 0; i < faultInfo->nNumFaults; ++i )
    {
        cout << "    Fault " << i << ": " 
            << FaultTypeToString( faultInfo->pFaultTypes[ i ] )
            << endl;
    }
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Convert a fault type into a string
//!
//! \param type    The fault info type to convert
//!
///////////////////////////////////////////////////////////////////////////////
static string FaultTypeToString( int type )
{
    switch( type )
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
    cout << "\nUsage: IvActivationQuery <options> <AsIpAddress>" << endl;
    cout << "\nAt least one of the -l or -u arguments must be provided." << endl;
    cout << "\nOptions:" << endl;
    cout << "    -l <Timestamp>     The minimum activation time" << endl;
    cout << "    -u <Timestamp>     The maximum activation time" << endl;
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
    // Register for activations from the Alarm Sever specified on the command line.
    //
    int result = IvBind2QueryActivationRecords(
        options.m_asIpAddress,
        &OnActivation,
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
            cout << "Failed to query for activations from " << asIpAddr <<
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
    
    result = IvBind2StopActivationRecordQuery( options.m_asIpAddress );
    if ( result != IVBIND2_SUCCESS )
    {
        string asIpAddr( options.m_asIpAddress );
        cout << "Failed to stop for query for activations from " << asIpAddr <<
            ": Error code " << result << endl;
        exit( 1 );
    }

    return 0;
}
