///////////////////////////////////////////////////////////////////////////////
//!
//! \file   ivunsetzone.cpp
//! \brief  Main file for the IvUnsetZone sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2010 IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <iostream>

//
// UnsetZoneOptions class, used to parse and store command line options
//
class UnsetZoneOptions
{
public:
    UnsetZoneOptions( int argc, char* argv[] );

public:
    std::string m_asIpAddress;
    std::string m_zoneName;
};

static void PrintUsage();

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
UnsetZoneOptions::UnsetZoneOptions( int argc, char* argv[] ) :
    m_asIpAddress(),
    m_zoneName()
{
    int nArg = 0;

    while ( ++nArg < argc )
    {
        const char* pszArg = argv[ nArg ];

        if ( pszArg[ 0 ] == '?')
        {
            PrintUsage();
            exit( 1 );
        }

        if ( pszArg[ 0 ] == '-' || pszArg[ 0 ] == '/' )
        {
            if ( strcmp( &pszArg[ 1 ], "n" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-n: Please specify zone name." << std::endl;
                    exit( 1 );
                }

                m_zoneName = argv[ nArg ];
            }
            else
            {
                std::cout << pszArg << ": Unknown argument." << std::endl;
                PrintUsage();
                exit( 1 );
            }
        }
        else
        {
            if ( !m_asIpAddress.empty() )
            {
                std::cout << pszArg << ": Unexpected argument." << std::endl;
                PrintUsage();
                exit( 1 );
            }

            m_asIpAddress = pszArg;
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
    std::cout
        << "\nUsage: IvUnsetZone -n <ZoneName> <AsIpAddress>"
        << std::endl;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Program entry point
//!
//! IvSetZone -n <ZoneName> <AsIpAddr>
//!
///////////////////////////////////////////////////////////////////////////////
int main( int argc, char* argv[] )
{
    //
    // Parse the command line options
    //
    UnsetZoneOptions options( argc, argv );

    if ( options.m_asIpAddress.empty() || options.m_zoneName.empty() )
    {
        PrintUsage();
        return 1;
    }

    int result = IvBind2UnsetZone(
                        options.m_asIpAddress.c_str(),
                        options.m_zoneName.c_str()
                        );

    switch( result )
    {
    case IVBIND2_SUCCESS:
        std::cout << "Zone unset successfully." << std::endl;
        break;

    case IVBIND2_ERROR_FAILURE:
        std::cout << "Zone unset failed." << std::endl;
        break;

    case IVBIND2_ERROR_PARTIALFAILURE:
        std::cout << "Zone unset partially failed." << std::endl;
        break;

    case IVBIND2_ERROR_NOZONE:
        std::cout << "Zone does not exist." << std::endl;
        break;

    case IVBIND2_WARNING_NOUNSETPOSSIBLE:
        std::cout
            << "The zone cannot be unset as it is already "
            << "in an unset state."
            << std::endl;
        break;

    default:
        std::cout
            << "Zone unset failed with error code " << result << "."
            << std::endl;
    }
    
    return 0;
}
