///////////////////////////////////////////////////////////////////////////////
//!
//! \file   ivrestoredetector.cpp
//! \brief  Main file for the IvRestoreDetector sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2010 IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <iostream>

//
// RestoreDetectorOptions class, used to parse and store command line options
//
class RestoreDetectorOptions
{
public:
    RestoreDetectorOptions( int argc, char* argv[] );

public:
    std::string m_asIpAddress;
    std::string m_zoneName;
    std::string m_detectorName;
};

static void PrintUsage();

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Builds an options structure from the command line
//!
///////////////////////////////////////////////////////////////////////////////
RestoreDetectorOptions::RestoreDetectorOptions( int argc, char* argv[] ) :
    m_asIpAddress(),
    m_zoneName(),
    m_detectorName()
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
            if ( strcmp( &pszArg[ 1 ], "z" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-n: Please specify zone name." << std::endl;
                    exit( 1 );
                }

                m_zoneName = argv[ nArg ];
            }
            else if ( strcmp( &pszArg[ 1 ], "d" ) == 0 )
            {
                nArg++;

                if ( nArg >= argc )
                {
                    std::cout << "-n: Please specify detector name." << std::endl;
                    exit( 1 );
                }

                m_detectorName = argv[ nArg ];
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
        << "\nUsage: IvRestoreDetector "
        << "-z <ZoneName> -d <DetectorName> <AsIpAddress>"
        << std::endl;
}

///////////////////////////////////////////////////////////////////////////////
//! 
//! \brief  Program entry point
//!
//! IvRestoreDetector -z <ZoneName> -d <DetectorName> <AsIpAddress>
//!
///////////////////////////////////////////////////////////////////////////////
int main( int argc, char* argv[] )
{
    //
    // Parse the command line options
    //
    RestoreDetectorOptions options( argc, argv );

    if (
        options.m_asIpAddress.empty() ||
        options.m_zoneName.empty() ||
        options.m_detectorName.empty()
        )
    {
        PrintUsage();
        return 1;
    }

    int result = IvBind2RestoreDetector(
                        options.m_asIpAddress.c_str(),
                        options.m_zoneName.c_str(),
                        options.m_detectorName.c_str()
                        );

    switch( result )
    {
    case IVBIND2_SUCCESS:
        std::cout << "Detector restored successfully." << std::endl;
        break;

    case IVBIND2_WARNING_NORESTOREPOSSIBLE:
        std::cout << "Detector is not in an isolated state "
            << "therefore detector cannot be restored." << std::endl;
        break;

    case IVBIND2_ERROR_FAILURE:
        std::cout << "Detector restore failed." << std::endl;
        break;

    case IVBIND2_ERROR_PARTIALFAILURE:
        std::cout << "Detector restore partially failed." << std::endl;
        break;

    case IVBIND2_ERROR_NODETECTOR:
        std::cout << "Detector does not exist." << std::endl;
        break;

    default:
        std::cout
            << "Detector restore failed with error code " << result << "."
            << std::endl;
    }
    
    return 0;
}
