///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the data record query sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <iostream>
#include <signal.h>
#include <sstream>
#include <string>
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
    typedef std::vector< UINT64 > SourceList;

public:
    CmdOptions( int argc, char* argv[] );

public:
    const char* m_asIpAddress;
    bool        m_minimumTimeSupplied;
    FILETIME    m_minimumTime;
    bool        m_maximumTimeSupplied;
    FILETIME    m_maximumTime;
    SourceList  m_sourceList;
    std::string m_dataFilter;
    bool        m_liveQueryOnly;

private:
    void Tokenize(
        const string&     str,
        vector< string >& tokens,
        const string&     delimiters );

    bool StringToFileTime( const std::string& time, FILETIME& fileTime );

    bool StringDateToSysTime(
        const std::vector< std::string >& date,
        SYSTEMTIME&                       fileTime );

    bool ValidateTimeValue( WORD value, WORD minValue, WORD maxValue );

    bool IsLeapYear( WORD year );

    bool ValidateYearMonthDayValue( WORD year, WORD month, WORD day );

    bool StringToSourceList(
        const std::string& sourceStr,
        SourceList&        sourceList );
};

static void PrintUsage();

static void CALLBACK OnDataRecord(
    void*                   userParam,
    int                     notificationType,
    IVBIND2_DATARECORDINFO* dataRecord,
    const char*             asIpAddress );

static string ItemIdToString( const UINT64& idValue );

static string FileTimeToString( const FILETIME& fileTime );

static void SignalHandler( int signal );

// Static flag used to stop the program
static bool sg_stop = false;

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
CmdOptions::CmdOptions( int argc, char* argv[] )
: m_asIpAddress( NULL ),
  m_minimumTimeSupplied( false ),
  m_maximumTimeSupplied( false ),
  m_liveQueryOnly( false )
{
    int argIndex = 0;

    memset( &m_minimumTime, 0, sizeof( FILETIME ) );
    memset( &m_maximumTime, 0, sizeof( FILETIME ) );

    while ( ++argIndex < argc )
    {
        const char* arg = argv[ argIndex ];

        if ( arg[ 0 ] == '-' || arg[ 0 ] == '/' )
        {
            if ( strcmp( &arg[ 1 ], "l" ) == 0 ||
                 strcmp( &arg[ 1 ], "u" ) == 0 )
            {
                FILETIME timeStamp;

                argIndex++;

                if ( argIndex >= argc )
                {
                    cout << "-" << arg[ 1 ] << ": Please specify a time stamp"
                         << endl;
                    PrintUsage();
                    exit( 1 );
                }

                if ( !StringToFileTime( argv[ argIndex ], timeStamp ) )
                {
                    cout << "-" << arg[ 1 ]
                         << ": Please specify a valid time stamp" << endl;
                    PrintUsage();
                    exit( 1 );
                }

                if ( strcmp( &arg[ 1 ], "l" ) == 0 )
                {
                    m_minimumTime = timeStamp;
                    m_minimumTimeSupplied = true;
                }
                else
                {
                    m_maximumTime = timeStamp;
                    m_maximumTimeSupplied = true;
                }
            }
            else if ( strcmp( &arg[ 1 ], "s" ) == 0 )
            {
                argIndex++;

                if ( argIndex >= argc )
                {
                    cout << "-" << arg[ 1 ] << ": Please specify a source list"
                         << endl;
                    PrintUsage();
                    exit( 1 );
                }

                if ( !StringToSourceList( argv[ argIndex ], m_sourceList ) )
                {
                    cout << "-" << arg[ 1 ]
                         << ": Please specify a valid source list" << endl;
                    PrintUsage();
                    exit( 1 );
                }
            }
            else if ( strcmp( &arg[ 1 ], "d" ) == 0 )
            {
                argIndex++;

                if ( argIndex >= argc )
                {
                    cout << "-" << arg[ 1 ] << ": Please specify a data filter"
                         << endl;
                    PrintUsage();
                    exit( 1 );
                }

                m_dataFilter = argv[ argIndex ];
            }
            else if ( strcmp( &arg[ 1 ], "n" ) == 0 )
            {
                m_liveQueryOnly = true;
            }
            else
            {
                cout << arg << ": Unknown argument" << endl;
                PrintUsage();
                exit( 1 );
            }
        }
        else if ( m_asIpAddress == NULL )
        {
            m_asIpAddress = arg;
        }
        else
        {
            cout << arg << ": Unexpected argument" << endl;
            PrintUsage();
            exit( 1 );
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
//! \brief  Tokenize string with delimiter
//!
//! \param  str         String to be tokenized
//! \param  tokens      Vector of tokens parsed from the string
//! \param  delimiters  String containing characters to be used as delimiters
//!
///////////////////////////////////////////////////////////////////////////////
void CmdOptions::Tokenize(
    const string&     str,
    vector< string >& tokens,
    const string&     delimiters )
{
    // Skip delimiters at beginning
    string::size_type lastPos = str.find_first_not_of( delimiters, 0 );

    // Find first "non-delimiter"
    string::size_type pos = str.find_first_of( delimiters, lastPos );

    while ( string::npos != pos || string::npos != lastPos )
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
//! \brief  Convert string argument to FILETIME
//!
//! \param  time      String representing the time to convert
//! \param  fileTime  Converted time
//!
//! \return True if successfully converted
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::StringToFileTime( const std::string& time, FILETIME& fileTime )
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
    bool       success = StringDateToSysTime( tokens, sysTime );

    if ( success )
    {
        //
        // Convert local time to UTC
        //
        TIME_ZONE_INFORMATION timeZoneInfo;
        SYSTEMTIME            utcTime = { 0 };

        GetTimeZoneInformation( &timeZoneInfo );
        TzSpecificLocalTimeToSystemTime( &timeZoneInfo, &sysTime, &utcTime );
        SystemTimeToFileTime( &utcTime, &fileTime );
    }

    return success;
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
    SYSTEMTIME&                       sysTime )
{
    bool success = true;

    sysTime.wYear = atoi( date.at( 0 ).c_str() );
    sysTime.wMonth = atoi( date.at( 1 ).c_str() );
    sysTime.wDay = atoi( date.at( 2 ).c_str() );
    sysTime.wHour = atoi( date.at( 3 ).c_str() );
    sysTime.wMinute = atoi( date.at( 4 ).c_str() );
    sysTime.wSecond = atoi( date.at( 5 ).c_str() );
    sysTime.wMilliseconds = atoi( date.at( 6 ).c_str() );

    success = ValidateYearMonthDayValue(
        sysTime.wYear, sysTime.wMonth, sysTime.wDay );
    success = ValidateTimeValue( sysTime.wHour, 0, 23 ) && success;
    success = ValidateTimeValue( sysTime.wMinute, 0, 59 ) && success;
    success = ValidateTimeValue( sysTime.wSecond, 0, 59 ) && success;
    success = ValidateTimeValue( sysTime.wMilliseconds, 0, 999 ) && success;

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
bool CmdOptions::ValidateTimeValue( WORD value, WORD minValue, WORD maxValue )
{
    bool success = false;
    if ( ( minValue <= maxValue ) && ( value >= minValue ) &&
         ( value <= maxValue ) )
    {
        success = true;
    }

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
    return (
        ( year % 400 == 0 ) || ( ( year % 100 != 0 ) && ( year % 4 == 0 ) ) );
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
bool CmdOptions::ValidateYearMonthDayValue( WORD year, WORD month, WORD day )
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
            success =
                ( day <= 28 ) || ( ( day == 29 ) && ( IsLeapYear( year ) ) );
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
//! \brief  Convert string argument to list of source IDs
//!
//! \param  sourceStr   A string containing the comma separated list of
//!                     external source IDs
//! \param  sourceList  The resulting list of source IDs
//!
//! \return True if successfully converted
//!
///////////////////////////////////////////////////////////////////////////////
bool CmdOptions::StringToSourceList(
    const std::string& sourceStr,
    SourceList&        sourceList )
{
    std::vector< std::string > tokens;

    Tokenize( sourceStr, tokens, "," );
    if ( tokens.empty() )
    {
        return false;
    }

    for ( std::size_t index = 0; index < tokens.size(); ++index )
    {
        std::istringstream stream( tokens[ index ] );
        UINT64             sourceId;

        if ( !( stream >> sourceId ) )
        {
            return false;
        }

        sourceList.push_back( sourceId );
    }

    return true;
}

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Data record callback function.
//!
//!         Called by the Binding Kit when a data record notification is
//!         received from the registered alarm server. This will print out the
//!         details of the callback parameters to the console.
//!
//! \param userParam        Parameter set when registering for alarms
//! \param notificationType The type of notification
//! \param dataRecord       The data record object
//! \param asIpAddress      The IP address of the alarm server
//!
///////////////////////////////////////////////////////////////////////////////
static void CALLBACK OnDataRecord(
    void*                   userParam,
    int                     notificationType,
    IVBIND2_DATARECORDINFO* dataRecord,
    const char*             asIpAddress )
{
    string asIpAddr( asIpAddress );
    string change = "";
    bool   expand = false;

    switch ( notificationType )
    {
    case IVBIND2_NOTIFICATION_CANCELLED:
        cout << "The Alarm Server " << asIpAddr
             << " has cancelled the data record query." << endl;

        // Stop running the monitor
        sg_stop = true;
        break;

    case IVBIND2_NOTIFICATION_DISCONNECTED:
        cout << "The connection to Alarm Server " << asIpAddr
             << " has been lost." << endl;

        // Stop running the monitor
        sg_stop = true;
        break;

    case IVBIND2_NOTIFICATION_DELETED:
        cout << "Data record with ID "
             << ItemIdToString( dataRecord->uDataRecordId )
             << " has been deleted from Alarm Server " << asIpAddr << "."
             << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout << "Data record prepopulation completed." << endl
             << "All historic data records have been sent." << endl;
        break;

    case IVBIND2_NOTIFICATION_UPDATED:
        change = " has been updated on";
        expand = true;
        break;

    case IVBIND2_PREPOPULATION_FETCHED:
        change = " was previously created on";
        expand = true;
        break;

    case IVBIND2_NOTIFICATION_INSERTED:
        change = " has just been created on";
        expand = true;
        break;

    default:
        cout << "Data record with ID "
             << ItemIdToString( dataRecord->uDataRecordId )
             << " has been sent as an unknown notification type from "
             << asIpAddr << "." << endl;
    }

    if ( expand )
    {
        cout << "Data record ID: "
             << ItemIdToString( dataRecord->uDataRecordId ) << change << " "
             << asIpAddr << endl
             << "Data record details:" << endl
             << " Source ID: " << ItemIdToString( dataRecord->uDataSourceId )
             << endl
             << " Created: " << FileTimeToString( dataRecord->ftTime ) << endl
             << " Data: " << dataRecord->szData << endl;
    }

    cout << endl;
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
static string FileTimeToString( const FILETIME& fileTime )
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
    cout << "\nUsage: IvDataRecordQuery <AsIpAddress>" << endl;
    cout << "\nIf -n is not provided then at least one of -l or -u must be."
         << endl;
    cout << "\nOptions:" << endl;
    cout << "    -l <Timestamp>     Only data records created after this time "
            "will be returned"
         << endl;
    cout << "    -u <Timestamp>     Only data records created before this time "
            "will be returned"
         << endl;
    cout << "    -s <SourceList>    Only data records created by one of the "
            "supplied sources will be returned."
         << endl;
    cout << "    -d <String>        Only data records where the data contains "
            "the supplied string will be returned"
         << endl;
    cout << "    -n                 Only data records created after the query "
            "is started will be returned"
         << endl;
    cout << "\nSource List Format:" << endl;
    cout << "    \"SourceID1,SourceID2,SourceID3,.....,SourceIDX\"" << endl;
    cout << "\nTimestamp Format:" << endl;
    cout << "    \"Year(YYYY):Month(01-12):Day(01-31):" << endl;
    cout << "    Hour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)\""
         << endl;
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
int main( int argc, char* argv[] )
{
    CmdOptions options = CmdOptions( argc, argv );

    //
    // Register for updates about data records from the Alarm Sever specified
    // on the command line.
    //
    int result = IvBind2QueryDataRecords(
        options.m_asIpAddress,
        &OnDataRecord,
        NULL,
        options.m_minimumTimeSupplied ? &options.m_minimumTime : NULL,
        options.m_maximumTimeSupplied ? &options.m_maximumTime : NULL,
        options.m_sourceList.empty() ? NULL : options.m_sourceList.data(),
        options.m_sourceList.size(),
        options.m_dataFilter.empty() ? NULL : options.m_dataFilter.c_str(),
        options.m_liveQueryOnly );
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
            cout << "Failed to query for data records from " << asIpAddr
                 << ": Error code " << result << endl;
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

    result = IvBind2StopDataRecordQuery( options.m_asIpAddress );
    if ( result != IVBIND2_SUCCESS )
    {
        string asIpAddr( options.m_asIpAddress );
        cout << "Failed to stop query for data records from " << asIpAddr
             << ": Error code " << result << endl;
        exit( 1 );
    }

    return 0;
}
