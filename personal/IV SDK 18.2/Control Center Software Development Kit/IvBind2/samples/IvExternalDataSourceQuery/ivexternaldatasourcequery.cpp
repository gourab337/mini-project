///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the external data source query sample application
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
    const char* m_asIpAddress;

private:
    void Tokenize(
        const string&     str,
        vector< string >& tokens,
        const string&     delimiters );
};

static void PrintUsage();

static void CALLBACK OnExternalDataSource(
    void*                           userParam,
    int                             notificationType,
    IVBIND2_EXTERNALDATASOURCEINFO* sourceInfo,
    const char*                     asIpAddress );

static string ItemIdToString( const UINT64& idValue );

static void SignalHandler( int signal );

// Static flag used to stop the program
static bool sg_stop = false;

///////////////////////////////////////////////////////////////////////////////
//!
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
CmdOptions::CmdOptions( int argc, char* argv[] ) : m_asIpAddress( NULL )
{
    int     argIndex = 0;
    string  strTime;
    while ( ++argIndex < argc )
    {
        const char* arg = argv[ argIndex ];

        if ( m_asIpAddress == NULL )
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
//! \brief  External data source callback function.
//!
//!         Called by the Binding Kit when an external data source notification
//!         is received from the registered alarm server. This will print out
//!         the details of the callback parameters to the console.
//!
//! \param userParam        Parameter set when registering for alarms
//! \param notificationType The type of notification
//! \param sourceInfo       The external data source object
//! \param asIpAddress      The IP address of the alarm server
//!
///////////////////////////////////////////////////////////////////////////////
static void CALLBACK OnExternalDataSource(
    void*                           userParam,
    int                             notificationType,
    IVBIND2_EXTERNALDATASOURCEINFO* sourceInfo,
    const char*                     asIpAddress )
{
    string asIpAddr( asIpAddress );
    string change = "";
    bool   expand = false;

    switch ( notificationType )
    {
    case IVBIND2_NOTIFICATION_CANCELLED:
        cout << "The Alarm Server " << asIpAddr
             << " has cancelled the external data source query." << endl;

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
        cout << "External data source with ID "
             << ItemIdToString( sourceInfo->uDataSourceId )
             << " has been deleted from Alarm Server " << asIpAddr << "."
             << endl;
        break;

    case IVBIND2_PREPOPULATION_COMPLETED:
        cout << "External data source prepopulation completed." << endl
             << "All current external data sources have been sent." << endl;
        break;

    case IVBIND2_NOTIFICATION_UPDATED:
        change = " has been updated on";
        expand = true;
        break;

    case IVBIND2_PREPOPULATION_FETCHED:
        change = " was previously raised on";
        expand = true;
        break;

    case IVBIND2_NOTIFICATION_INSERTED:
        change = " has just been raised on";
        expand = true;
        break;

    default:
        cout << "External data source with ID "
             << ItemIdToString( sourceInfo->uDataSourceId )
             << " has been sent as an unknown notification type from "
             << asIpAddr << "." << endl;
    }

    if ( expand )
    {
        cout << "External data source ID: "
             << ItemIdToString( sourceInfo->uDataSourceId ) << change << " "
             << asIpAddr << endl
             << "External data source details:" << endl
             << " Name: " << sourceInfo->szDataSourceName << endl
             << " Source IP Address: " << sourceInfo->szIpAddress << endl
             << " Source Number: "
             << ItemIdToString( sourceInfo->nSourceNumber ) << endl;
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
//! \brief  Displays usage information
//!
///////////////////////////////////////////////////////////////////////////////
static void PrintUsage()
{
    cout << "\nUsage: IvExternalDataSourceQuery <AsIpAddress>" << endl;
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
    // Register for updates about external data sources from the Alarm Sever
    // specified on the command line.
    //
    int result = IvBind2QueryExternalDataSources(
        options.m_asIpAddress, &OnExternalDataSource, NULL );
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
            cout << "Failed to query for external data sources from "
                 << asIpAddr << ": Error code " << result << endl;
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

    result = IvBind2StopExternalDataSourceQuery( options.m_asIpAddress );
    if ( result != IVBIND2_SUCCESS )
    {
        string asIpAddr( options.m_asIpAddress );
        cout << "Failed to stop query for external data sources from "
             << asIpAddr << ": Error code " << result << endl;
        exit( 1 );
    }

    return 0;
}
