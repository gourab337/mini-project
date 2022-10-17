///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the addbookmark sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2008 IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <vector>
#include <set>
#include <iostream>
#include <sstream>
#include <time.h>

static const LONGLONG FILETIME_TICKS_PER_MILLISECOND = 10000;
static const WORD MIN_YEAR = 1601;
static const WORD MAX_YEAR = 30827;

//
//  List of constant parameters
//
std::string params[] = { 
        std::string( "-nvr" ), 
        std::string( "-cam" ), 
        std::string( "-d" ), 
        std::string( "-t" ) 
        };
static const std::set< std::string > PARAMS( params, params+4 );

//
// SendOptions class, used to parse and store command line options
//
class SendOptions
{
public:
    SendOptions( int argc, char* argv[] );
    bool StringToFileTime(
            const std::string& time,
            FILETIME& fileTime
            );

public:
    std::string  m_strNvrIpAddr;
    std::string  m_strCamIpAddr;
    const char  *m_bookmarkText;
    FILETIME     m_time;

private:
    
private:
    //
    //  Split time field in command line 
    //
    void Tokenize(
            const std::string& str,
            std::vector< std::string >& tokens,
            const std::string& delimiters
            );

    //
    //  Validate time value
    //
    bool ValidateTimeValue(
            WORD value,
            WORD minValue,
            WORD maxValue
            );

    //
    //  Validate month and day
    //
    bool ValidateYearMonthDayValue(
            WORD year,
            WORD month,
            WORD day
            );

    //
    //  Validate year
    //
    bool IsLeapYear( WORD year );

    //
    //  Convert string date format to system time
    //
    bool StringDateToSysTime(
            const std::vector< std::string >& date,
            SYSTEMTIME& fileTime
            );

    //
    //  Convert milliseconds to file time
    //
    void MillisecondToFileTime( 
            const long long& milliseconds,
            FILETIME& fileTime
            );
};

static void PrintUsage();

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Program entry point
//!
//! IvAddBookmark <options> -nvr <nvrIP> -cam <CamIP> 
//!
//! Options:
//!  "    -d <BookmarkText>    Specifies the bookmarktext"
//!  "    -t <TimeStamp>       Specifies the timestamp (UTC) of bookmark (optional)
//! 
///////////////////////////////////////////////////////////////////////////////

int main( int argc, char *argv[] )
{
    //
    // Parse the command line options
    //
    SendOptions options( argc, argv );


    if ( options.m_strNvrIpAddr.empty() || options.m_strCamIpAddr.empty() )
    {
        PrintUsage();
        return -1;
    }

    //
    // Send the bookmark with the camreIP address,NVR IP address,user ID,
    // bookmark number and text
    //
    int nResult = IvBind2AddBookmark(
                    options.m_strNvrIpAddr.c_str(),
                    options.m_strCamIpAddr.c_str(),
                    options.m_bookmarkText,
                    &options.m_time
                    );

    if ( nResult == IVBIND2_SUCCESS )
    {
        std::cout << "Added bookmark successfully" << std::endl;
        return 0;
    }
    else
    {
        std::cout << "Adding bookmark with error code " << nResult << std::endl;
        return -1;
    }  
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
SendOptions::SendOptions( int argc, char *argv[] ) :
m_bookmarkText( "" ),
m_strNvrIpAddr(),
m_strCamIpAddr()
{
    int nArg = 0;
    std::string strTime;
    GetSystemTimeAsFileTime( &m_time );

    while ( ++nArg < argc )
    {
        const char *pszArg = argv[ nArg ];
        if ( pszArg[ 0 ] == '?')
        {
            PrintUsage();
            exit( 1 );
        }

        if ( pszArg[ 0 ] == '-' || pszArg[ 0 ] == '/' )
        {
            if ( strcmp( &pszArg[ 1 ], "d" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-d: Please specify a bookmark string" << std::endl;
                    exit( 1 );
                }
                
                if ( PARAMS.find( argv[ nArg ] ) == PARAMS.end() )
                {
                    m_bookmarkText = argv[ nArg ];
                }
                else
                {
                    std::cout << "-d: Please specify a bookmark string " << std::endl;
                    exit( 1 );
                }
            }

            else if ( strcmp( &pszArg[ 1 ], "cam" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-cam: Please specify camera IP address" << std::endl;
                    exit( 1 );
                }

                char* nextArg = argv[ nArg ];
                if ( nextArg[ 0 ] == '-' || nextArg[ 0 ] == '/' )
                {
                    std::cout << "-cam: Please specify camera IP address" << std::endl;
                    exit( 1 );
                }

                m_strCamIpAddr = nextArg;
            }

            else if ( strcmp( &pszArg[ 1 ], "nvr" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-nvr: Please specify NVR IP address" << std::endl;
                    exit( 1 );
                }

                char* nextArg = argv[ nArg ];
                if ( nextArg[ 0 ] == '-' || nextArg[ 0 ] == '/' )
                {
                    std::cout << "-nvr: Please specify NVR IP address" << std::endl;
                    exit( 1 );
                }

                m_strNvrIpAddr = argv[ nArg ];
            }

            else if ( strcmp( &pszArg[ 1 ], "t" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-t: Please specify the time stamp" << std::endl;
                    exit( 1 );
                }

                strTime = argv[ nArg ];
                if ( !StringToFileTime( strTime, m_time ) )
                {
                    {
                        std::cout << "-t: Please specify correct time" << std::endl;
                        exit( 1 );
                    }
                }
            }
            else
            {
                std::cout << pszArg << ": Unexpected argument" << std::endl;
                PrintUsage();
                exit( 1 );
            }
        }
        else
        {
            std::cout << pszArg << ": Unexpected argument" << std::endl;
            PrintUsage();
            exit( 1 );
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Convert string argument to FILETIME
//! 
//! \param  time    Timestamp argument
//!
//! \return Convert successfully or not.
//!
///////////////////////////////////////////////////////////////////////////////
bool SendOptions::StringToFileTime(
                           const std::string& time,
                           FILETIME& fileTime
                           )
{
    bool success = true;
    SYSTEMTIME sysTime;    
    std::vector< std::string > tokens;

    Tokenize( time, tokens, ":" );
    if ( tokens.size() != 7 )
    {
        //
        //  Convert string to int
        //
        std::istringstream buffer( time );
        long long milliseconds;
        char c;

        //
        //  Checks that there aren't any left-over characters
        //
        if ( !( buffer >> milliseconds ) || buffer.get( c ) )
        {            
            return false;
        }
        MillisecondToFileTime( milliseconds, fileTime );
    }
    else
    {
        success = StringDateToSysTime( tokens, sysTime );
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
                &utcTime );
            SystemTimeToFileTime( &utcTime, &fileTime );
        }
    }

    return success;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Convert string to SYSTEMTIME
//! 
//! \param  time    Time
//!
//! \return Convert successfully or not.
//!
///////////////////////////////////////////////////////////////////////////////
bool SendOptions::StringDateToSysTime(
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
//! \brief  Convert different milliseconds to FILETIME
//! 
//! \param  milliseconds    Different milliseconds
//! \param  fileTime        Filetime
//!
///////////////////////////////////////////////////////////////////////////////
void SendOptions::MillisecondToFileTime( 
        const long long& milliseconds,
        FILETIME& fileTime
        )
{
   ULONGLONG qwResult;

   // Copy the time into a quadword.
   qwResult = ( ( (ULONGLONG) fileTime.dwHighDateTime ) << 32 ) 
                    + fileTime.dwLowDateTime;

   // Add relative milliseconds to filetime
   qwResult += milliseconds * FILETIME_TICKS_PER_MILLISECOND;

   // Copy the result back into the FILETIME structure.
   fileTime.dwLowDateTime  = (DWORD)( qwResult & 0xFFFFFFFF );
   fileTime.dwHighDateTime = (DWORD)( qwResult >> 32 );
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Check whether it's leap year or not
//! 
//! \param  year     Year
//!
//! \return true if it is leap year
//!
///////////////////////////////////////////////////////////////////////////////
bool SendOptions::IsLeapYear( WORD year )
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
bool SendOptions::ValidateYearMonthDayValue(
                      WORD year,
                      WORD month,
                      WORD day
                      )
{
    bool success = false;

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
bool SendOptions::ValidateTimeValue(
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
//! \brief  Tokenize string with delimiter
//! 
//! \param  str         String to be tokenized
//! \param  tokens      Returned tokens 
//! \param  delimiter   Delimiter
//!
///////////////////////////////////////////////////////////////////////////////
void SendOptions::Tokenize(
                      const std::string& str,
                      std::vector< std::string >& tokens,
                      const std::string& delimiters = " "
                      )
{
    // Skip delimiters at beginning
    std::string::size_type lastPos = str.find_first_not_of(delimiters, 0);

    // Find first "non-delimiter"
    std::string::size_type pos     = str.find_first_of(delimiters, lastPos);

    while (std::string::npos != pos || std::string::npos != lastPos)
    {
        // Found a token, add it to the vector.
        tokens.push_back(str.substr(lastPos, pos - lastPos));

        // Skip delimiters.  Note the "not_of"
        lastPos = str.find_first_not_of(delimiters, pos);

        // Find next "non-delimiter"
        pos = str.find_first_of(delimiters, lastPos);
    }
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Displays usage information
//!
///////////////////////////////////////////////////////////////////////////////
static void PrintUsage()
{
    std::cout << "\nUsage: IvAddBookmark <options> -nvr <nvrIP> -cam <camIP>" << std::endl;
    std::cout << "\nOptions:" << std::endl;
    std::cout << "    -d <BookmarkText>   Specifies the bookmark string" << std::endl;
    std::cout << "    -t <TimeStamp>      Specifies a timestamp (UTC) for sent event with format " << std::endl;
    std::cout << "\t\t\t\"Year(YYYY):Month(01-12):Day(01-31):" << std::endl;
    std::cout << "\t\t\tHour(00-23):Minute(00-59):Second(00-59):Millisecond(000-999)\"" << std::endl;
    std::cout << "\t\t\tOr specifies relative time in milliseconds." << std::endl;
    std::cout << "\t\t\t(Default the current time is used) ";
    std::cout << std::endl;
}
