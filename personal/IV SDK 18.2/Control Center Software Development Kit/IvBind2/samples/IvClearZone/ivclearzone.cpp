///////////////////////////////////////////////////////////////////////////////
//!
//! \file   ivclearzone.cpp
//! \brief  Main file for the IvClearZone sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2003-2010 IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <vector>
#include <iostream>

using namespace std;

//
// ClearZoneOptions class, used to parse and store command line options
//
class ClearZoneOptions
{
public:
    ClearZoneOptions( int argc, char* argv[] );

public:
    std::string       m_strAsIpAddress;
    std::string       m_strzoneName;
    char*             m_annotation;
};

static void PrintUsage();

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
ClearZoneOptions::ClearZoneOptions( int argc, char *argv[] ) 
    : m_strAsIpAddress(),
    m_strzoneName(),
    m_annotation( NULL )    
{
    int nArg = 0;

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
            //
            // Check if there is string with leading "-" right after current parameter
            //

            if ( ( nArg + 1 ) < argc ) 
            {
                const char *checkMinus = argv[ nArg + 1 ];

                if (( checkMinus[ 0 ] == '-' ) || ( checkMinus[ 0 ] == '/' ))
                {
                    std::cout << pszArg << ": Please specify a valid value" << std::endl;
                    exit( 1 );
                }
            }
            if ( strcmp( &pszArg[ 1 ], "n" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    cout << "-n: Please specify a zone name" << endl;
                    exit( 1 );
                }

                if ( strcmp( argv[ nArg ], "-d" ) == 0 )
                {
                    cout << "-n: Please specify a zone name" << endl;
                    exit( 1 );
                }
                else
                {
                    m_strzoneName = argv[ nArg ];
                }
                
            }
            else if ( strcmp( &pszArg[ 1 ], "d" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    cout << "-d: Please specify clear message" << endl;
                    exit( 1 );
                }

                if ( strcmp( argv[ nArg ], "-n" ) == 0 )
                {
                    cout << "-d: Please specify clear message" << endl;
                    exit( 1 );
                }
                else
                {
                    m_annotation = argv[ nArg ];
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
            if ( m_strAsIpAddress.empty() )
            {
                m_strAsIpAddress = pszArg;
            }
            else
            {
                cout << pszArg << ": Unexpected argument" << endl;
                PrintUsage();
                exit( 1 );
            }
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Displays usage information
//!
///////////////////////////////////////////////////////////////////////////////
static void PrintUsage()
{
    cout << "\nUsage: IvClearZone <options> -n <ZoneName> <AsIpAddress>" << endl;    
    cout << "\nOptions:" << endl;
    cout << "   -d <ClearMessage>        Specifies zone clear message" << endl;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Program entry point
//!
//! IvClearZone <options> -n <ZoneName> <AsIpAddr>
//!
//! Options:
//!  -d <ClearMessage>         Specifies zone clear message
//!
///////////////////////////////////////////////////////////////////////////////
int main( int argc, char *argv[] )
{
    //
    // Parse the command line options
    //
    ClearZoneOptions options( argc, argv );

    if ( options.m_strAsIpAddress.empty() ||
         ( options.m_strzoneName.empty() ) )
    {
        PrintUsage();
        return 1;
    }

    int nResult = IvBind2ClearZone(
                    options.m_strAsIpAddress.c_str(),                    
                    options.m_strzoneName.c_str(),
                    options.m_annotation
                    );

    switch ( nResult )
    {
    case IVBIND2_SUCCESS:
        cout << "Zone cleared successfully." << endl;
        break;

    case IVBIND2_ERROR_FAILURE:
        cout << "Zone clear failed." << endl;
        break;

    case IVBIND2_ERROR_PARTIALFAILURE:
        cout << "Zone clear partially failed." << endl;
        break;

    case IVBIND2_ERROR_NOZONE:
        cout << "Zone does not exist." << endl;
        break;

    case IVBIND2_WARNING_NOCLEARPOSSIBLE:
        cout << "Zone is not in alarm therefore zone cannot be cleared." << endl;
        break;

    default:
        cout << "Zone clear failed with error code "
            << nResult << "." << endl;
    }
    
    return nResult;
}