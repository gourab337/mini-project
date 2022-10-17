///////////////////////////////////////////////////////////////////////////////
//!
//! \file
//! \brief  Main file for the ivexternalrelay sample application
//!
///////////////////////////////////////////////////////////////////////////////
// Copyright (c) IndigoVision Limited.
///////////////////////////////////////////////////////////////////////////////

#include "ivbind2.h"
#include <signal.h>
#include <iostream>
#include <string>
#include <sstream>
#include <memory>
#include <vector>

using namespace std;

//
// Structures & typedefs used by the program
//
struct PinStateChangeInfo
{
    std::string remoteIp;
    int         pinNum;
    bool        pinState;
};

typedef std::vector< PinStateChangeInfo >   PinStateChangeList;
typedef std::auto_ptr< PinStateChangeList > PinStateChangeListPtr;

struct UserParamStruct
{
    CRITICAL_SECTION                    criticalSection;
    std::auto_ptr< PinStateChangeList > pStateChangeList;
};

//
// Forward function declarations
//
static void CALLBACK OnPinChange( 
                          void                  *pUserParam,
                          IVBIND2_PINSTATEINFO  *pPinStateInfo,
                          const char            *pszRemoteIpAddress,
                          BOOL                  *pbError
                          );

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
        cout << "\nUsage: IvExternalRelay <LocalIpAddress> " << endl;
        exit( 1 );
    }

    const char *pszArg = argv[ 1 ];
    if ( pszArg[ 0 ] == '?' )
    {
        cout << "\nUsage: IvExternalRelay <LocalIpAddress> " << endl;
        exit( 1 );
    }

    UserParamStruct userParam;
    userParam.pStateChangeList.reset( new PinStateChangeList );
    ::InitializeCriticalSection( &userParam.criticalSection );


    //
    // Register for relay changes on the ip address specified on the command line
    //
    int nResult = 
        IvBind2RegisterRelayChanges( argv[1], &OnPinChange, &userParam );
    
    if ( nResult != IVBIND2_SUCCESS )
    {
        if ( nResult == IVBIND2_ERROR_BADPARAM ) 
        {
            cout << "Please specify a valid local IP Address. " << endl;
            exit( 1 );
        }
        else
        {
            cout << "Failed to register for relay changes on " << argv[ 1 ] <<
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
        Sleep( 1000 );
        PinStateChangeListPtr ourPtr( new PinStateChangeList );
        
        ::EnterCriticalSection( &userParam.criticalSection );
        std::swap( ourPtr, userParam.pStateChangeList );
        ::LeaveCriticalSection( &userParam.criticalSection );
        
        PinStateChangeList::const_iterator 
            it = ourPtr->begin(),
            itEnd = ourPtr->end();

        for( ; it != itEnd ; ++it )
        {
            cout << "Pin " << it->pinNum << " was changed to ";
            if( it->pinState )
            {
                cout << "on";
            }
            else
            {
                cout << "off";
            }
            cout << " by client " << it->remoteIp << endl;
        }
    }
    
    nResult = IvBind2UnregisterRelayChanges();
    if ( nResult != IVBIND2_SUCCESS )
    {
        cout << "Failed to unregister for relay changes from " << argv[ 1 ] <<
            ": Error code " << nResult << endl;
    }

    ::DeleteCriticalSection( &userParam.criticalSection );

	return 0;
}

//
// Event callback function
//
static void CALLBACK OnPinChange( 
                        void                 *pUserParam,
                        IVBIND2_PINSTATEINFO *pPinStateInfo,
                        const char           *pszRemoteIpAddress,
                        BOOL                 *pbError
                        )
{
    if( pUserParam == NULL || pPinStateInfo == NULL )
    {
        return;
    }

    UserParamStruct* userParam = static_cast< UserParamStruct* >( pUserParam );

    ::EnterCriticalSection( &userParam->criticalSection );

    userParam->pStateChangeList->resize( 
        userParam->pStateChangeList->size() + 1 
        );
    PinStateChangeInfo& pinState = userParam->pStateChangeList->back();
    pinState.pinNum = pPinStateInfo->nPinNumber;
    pinState.pinState = pPinStateInfo->bPinState == TRUE;
    pinState.remoteIp = pszRemoteIpAddress;
    *pbError = FALSE;
    ::LeaveCriticalSection( &userParam->criticalSection );
}

//
// Signal handler. Handles the Ctrl-C keypress.
//
static void SignalHandler( int nSignal )
{
    sg_fStop = true;
}
