/*****************************************************************************/
/*!
*  \file
*  \brief   Header file for the IndigoVision Binding Kit 2.x API
*/
/*****************************************************************************/
/* Copyright (c) IndigoVision Limited.                                  */
/*****************************************************************************/

#ifndef IVBIND2_H_
#define IVBIND2_H_
#ifdef _WIN32

#include <winsock2.h>
#include <windows.h>
#else
#include <inttypes.h>
#endif

/*
** Error codes
*/
#define IVBIND2_SUCCESS                     0
#define IVBIND2_ERROR_FAILURE              -1
#define IVBIND2_ERROR_PARTIALFAILURE       -2
#define IVBIND2_ERROR_NOTREG               -3
#define IVBIND2_ERROR_ALREADYREG           -4
#define IVBIND2_ERROR_BADPARAM             -5
#define IVBIND2_ERROR_REJECTED             -6
#define IVBIND2_ERROR_NORESPONSE           -7
#define IVBIND2_ERROR_INVALIDDETECTORTYPE  -8
#define IVBIND2_ERROR_NOZONE               -9
#define IVBIND2_ERROR_NODETECTOR          -10
#define IVBIND2_ERROR_ADDRESSUNAVALIABLE  -11
#define IVBIND2_ERROR_ADDRESSINUSE        -12

/*
** Warning codes
*/
#define IVBIND2_WARNING_NOACKPOSSIBLE     -64
#define IVBIND2_WARNING_NOCLEARPOSSIBLE   -65
#define IVBIND2_WARNING_NOUNSETPOSSIBLE   -66
#define IVBIND2_WARNING_NOSETPOSSIBLE     -67
#define IVBIND2_WARNING_NOISOLATEPOSSIBLE -68
#define IVBIND2_WARNING_NORESTOREPOSSIBLE -69

/*
** Zone state
*/
#define IVBIND2_ZONESTATE_UNKNOWN           0
#define IVBIND2_ZONESTATE_SET               1
#define IVBIND2_ZONESTATE_UNSET             2
#define IVBIND2_ZONESTATE_ALARM             4
#define IVBIND2_ZONESTATE_ACKNOWLEDGED      8

/*
** Alarm state
*/
#define IVBIND2_ALARMSTATE_UNKNOWN          0
#define IVBIND2_ALARMSTATE_NEW              1
#define IVBIND2_ALARMSTATE_ACKNOWLEDGED     2
#define IVBIND2_ALARMSTATE_CLEARED          4

/*
** Detector state
*/
#define IVBIND2_DETECTORSTATE_UNKNOWN       0
#define IVBIND2_DETECTORSTATE_ENABLED       1
#define IVBIND2_DETECTORSTATE_DISABLED      2
#define IVBIND2_DETECTORSTATE_TRIGGERED     4
#define IVBIND2_DETECTORSTATE_TAMPERED      8

/*
** Notification type (zone, detector or alarm)
*/
#define IVBIND2_NOTIFICATION_INSERTED       1
#define IVBIND2_NOTIFICATION_UPDATED        2
#define IVBIND2_NOTIFICATION_DELETED        4
#define IVBIND2_PREPOPULATION_FETCHED       8
#define IVBIND2_NOTIFICATION_CANCELLED     16
#define IVBIND2_NOTIFICATION_DISCONNECTED  32
#define IVBIND2_PREPOPULATION_COMPLETED    64

/*
** Detector types
*/
#define IVBIND2_DETECTORTYPE_UNKNOWN            0
#define IVBIND2_DETECTORTYPE_BASICANALYTICS     1
#define IVBIND2_DETECTORTYPE_DIGITALINPUT       2
#define IVBIND2_DETECTORTYPE_EXTERNAL           3
#define IVBIND2_DETECTORTYPE_NETWORKFAULT       4
#define IVBIND2_DETECTORTYPE_VIDEOFAULT         5
#define IVBIND2_DETECTORTYPE_DOUBLEKNOCK        6
#define IVBIND2_DETECTORTYPE_UNHANDLEDALARM     7
#define IVBIND2_DETECTORTYPE_DEVICEFAULT        8
#define IVBIND2_DETECTORTYPE_NONE               9
#define IVBIND2_DETECTORTYPE_ONVIFNETWORKFAULT  10
#define IVBIND2_DETECTORTYPE_ONVIFEVENT         11

/*
** Detector Extra Info Types
*/
#define IVBIND2_DETECTOREXTRAINFOTYPE_UNKNOWN               0
#define IVBIND2_DETECTOREXTRAINFOTYPE_NONE                  1
#define IVBIND2_DETECTOREXTRAINFOTYPE_NVRFAULT              2
#define IVBIND2_DETECTOREXTRAINFOTYPE_ISOLATEREASON         3
#define IVBIND2_DETECTOREXTRAINFOTYPE_ACTIVATIONANNOTATION  4

/*
** Activation Extra Info Types
*/
#define IVBIND2_ACTIVATIONEXTRAINFOTYPE_UNKNOWN               0
#define IVBIND2_ACTIVATIONEXTRAINFOTYPE_NONE                  1
#define IVBIND2_ACTIVATIONEXTRAINFOTYPE_NVRFAULT              2
#define IVBIND2_ACTIVATIONEXTRAINFOTYPE_ACTIVATIONANNOTATION  4

/*
** NVR Fault Types
*/
#define IVBIND2_NVRFAULTTYPE_UNKNOWN                        0
#define IVBIND2_NVRFAULTTYPE_RECORDINGFAILURE               1
#define IVBIND2_NVRFAULTTYPE_LICENSEFAILURE                 2
#define IVBIND2_NVRFAULTTYPE_RAIDDEGRADED                   3
#define IVBIND2_NVRFAULTTYPE_REDUNDANTPOWERFAIL             4
#define IVBIND2_NVRFAULTTYPE_REDUNDANTNETWORKFAIL           5
#define IVBIND2_NVRFAULTTYPE_DEVICEOFFLINE                  6
#define IVBIND2_NVRFAULTTYPE_UPSONBATTERY                   7
#define IVBIND2_NVRFAULTTYPE_FANFAILURE                     8
#define IVBIND2_NVRFAULTTYPE_SYSTEMOVERTEMP                 9
#define IVBIND2_NVRFAULTTYPE_DISKOVERTEMP                   10
#define IVBIND2_NVRFAULTTYPE_STORAGEARRAYMONITORINGFAILURE  11
#define IVBIND2_NVRFAULTTYPE_STORAGEARRAYDISKFAILURE        12
#define IVBIND2_NVRFAULTTYPE_STORAGEARRAYREDUNDANCYFAILURE  13
#define IVBIND2_NVRFAULTTYPE_STORAGEARRAYENCLOSUREFAILURE   14
#define IVBIND2_NVRFAULTTYPE_LOWDISKSPACE                   15

/*
** ONVIF Event Types
*/
#define IVBIND2_ONVIFEVENTTYPE_UNKNOWN           0
#define IVBIND2_ONVIFEVENTTYPE_BASICANALYTICS    1
#define IVBIND2_ONVIFEVENTTYPE_DIGITALINPUT      2
#define IVBIND2_ONVIFEVENTTYPE_ADVANCEDANALYTICS 3
#define IVBIND2_ONVIFEVENTTYPE_CYBERVIGILANT     4

/*
** Data constraint constants
*/
#define IVBIND2_ISOLATEREASON_MAXLENGTH  1500
#define IVBIND2_ONVIFSERVICEID_MAXLENGTH 128
#define IVBIND2_ANNOTATIONMSG_MAXLENGTH 256

/*
** General defines
*/
#ifdef _WIN32
#define IVBIND2API __stdcall
#else
#define IVBIND2API
#define CALLBACK

typedef int      BOOL;
typedef int      INT;
typedef uint16_t UINT16;
typedef int64_t  INT64;
typedef uint64_t UINT64;
typedef int64_t  FILETIME;
typedef void*    PVOID;
#endif

typedef struct tagIVBIND2_DETECTORSOURCEDESC
{
    INT         nDetectorType;
    char        szIpAddress[ 16 ];
    INT         nInput;
    BOOL        bNormalPinState;
}
IVBIND2_DETECTORSOURCEDESC;

typedef struct tagIVBIND2_ZONEINFO
{
    INT         nZoneNotificationType;
    UINT64      uZoneId;
    UINT64      uOwnerId;
    UINT64      uScheduleId;
    UINT64      uAlarmRecordId;
    char        szZoneName[ 256 ];
    INT         nPriority;
    INT         nZoneState;
    FILETIME    ftTimeRaised;
    FILETIME    ftTimeAcknowledged;
}
IVBIND2_ZONEINFO;

typedef struct tagIVBIND2_ZONEINFO2
{
    UINT64      uZoneId;
    UINT64      uOwnerId;
    UINT64      uScheduleId;
    UINT64      uAlarmRecordId;
    char        szZoneName[ 256 ];
    INT         nPriority;
    INT         nZoneState;
    FILETIME    ftTimeRaised;
    FILETIME    ftTimeAcknowledged;
}
IVBIND2_ZONEINFO2;

typedef struct tagIVBIND2_ALARMRECORDINFO
{
    UINT64      uAlarmRecordId;
    UINT64      uZoneId;
    UINT64      uOwnerId;
    INT         nAlarmState;
    FILETIME    ftTimeRaised;
    FILETIME    ftTimeCleared;
}
IVBIND2_ALARMRECORDINFO;

typedef struct tagIVBIND2_DETECTORINFO
{
    INT         nDetectorNotificationType;
    UINT64      uDetectorId;
    char        szDetectorName[ 256 ];
    INT         nDetectorState;
    BOOL        bAlarmable;
    INT64       uDwellTime;
    UINT64      uZoneId;
    BOOL        bInAlarm;
    FILETIME    ftActivationTime;
    IVBIND2_DETECTORSOURCEDESC dsClearSource;
    IVBIND2_DETECTORSOURCEDESC dsTriggerSource;
}
IVBIND2_DETECTORINFO;

typedef struct tagIVBIND2_DETECTORINFO2
{
    UINT64      uDetectorId;
    char        szDetectorName[ 256 ];
    INT         nDetectorState;
    BOOL        bAlarmable;
    INT64       uDwellTime;
    UINT64      uZoneId;
    BOOL        bInAlarm;
    FILETIME    ftActivationTime;
    INT         nDeactivateSourceType;
    PVOID       pDeactivateSource;
    INT         nActivateSourceType;
    PVOID       pActivateSource;
    INT         nExtraInfoType;
    PVOID       pExtraInfo;
}
IVBIND2_DETECTORINFO2;

typedef struct tagIVBIND2_ACTIVATIONRECORDINFO
{
    UINT64      uActivationRecordId;
    UINT64      uDetectorId;
    FILETIME    ftActivationTime;
    UINT64      uAlarmRecordId;
    UINT64      uZoneId;
    FILETIME    ftAlarmTime;
    INT         nExtraInfoType;
    PVOID       pExtraInfo;
}
IVBIND2_ACTIVATIONRECORDINFO;

typedef struct tagIVBIND2_BASICANALYTICSSOURCEDESC
{
    char        szIpAddress[ 16 ];
    int         nBasicAnalyticsSource;
} IVBIND2_BASICANALYTICSSOURCEDESC;

typedef struct tagIVBIND2_DIGITALINPUTSOURCEDESC
{
    char        szIpAddress[ 16 ];
    INT         nInput;
    BOOL        bNormalPinState;
} IVBIND2_DIGITALINPUTSOURCEDESC;

typedef struct tagIVBIND2_EXTERNALSOURCEDESC
{
    char        szIpAddress[ 16 ];
    INT         nInput;
}
IVBIND2_EXTERNALSOURCEDESC;

typedef struct tagIVBIND2_NETWORKFAULTSOURCEDESC
{
    char        szIpAddress[ 16 ];
}
IVBIND2_NETWORKFAULTSOURCEDESC;

typedef struct tagIVBIND2_VIDEOFAULTSOURCEDESC
{
    char        szIpAddress[ 16 ];
}
IVBIND2_VIDEOFAULTSOURCEDESC;

typedef struct tagIVBIND2_DOUBLEKNOCKSOURCEDESC
{
    UINT64      uFirstDetectorId;
    UINT64      uSecondDetectorId;
} IVBIND2_DOUBLEKNOCKSOURCEDESC;

typedef struct tagIVBIND2_UNHANDLEDALARMSOURCEDESC
{
    UINT64      uZoneId;
    UINT64      uTimeout;
} IVBIND2_UNHANDLEDALARMSOURCEDESC;

typedef struct tagIVBIND2_DEVICEFAULTSOURCEDESC
{
    char        szIpAddress[ 16 ];
}
IVBIND2_DEVICEFAULTSOURCEDESC;

typedef struct tagIVBIND2_ONVIFNETWORKFAULTSOURCEDESC
{
    char        szServiceId[ IVBIND2_ONVIFSERVICEID_MAXLENGTH ];
}
IVBIND2_ONVIFNETWORKFAULTSOURCEDESC;

typedef struct tagIVBIND2_ONVIFEVENTSOURCEDESC
{
    int         nEventType;
    char        szServiceId[ IVBIND2_ONVIFSERVICEID_MAXLENGTH ];
}
IVBIND2_ONVIFEVENTSOURCEDESC;

typedef struct tagIVBIND2_NVRFAULTINFO
{
    INT         nNumFaults;
    INT*        pFaultTypes;
}
IVBIND2_NVRFAULTINFO;

typedef struct tagIVBIND2_ISOLATEREASON
{
    char szIsolateReason[ IVBIND2_ISOLATEREASON_MAXLENGTH ];
}
IVBIND2_ISOLATEREASON;

typedef struct tagIVBIND2_ANNOTATIONMSG
{
    char szAnnotationMsg[ IVBIND2_ANNOTATIONMSG_MAXLENGTH ];
} IVBIND2_ANNOTATIONMSG;

typedef struct tagIVBIND2_PINSTATEINFO
{
    UINT16      nPinNumber;
    BOOL        bPinState;
}
IVBIND2_PINSTATEINFO;

typedef struct tagIVBIND2_EXTERNALDATASOURCEINFO
{
    UINT64 uDataSourceId;
    char   szDataSourceName[ 256 ];
    char   szIpAddress[ 16 ];
    INT    nSourceNumber;
} IVBIND2_EXTERNALDATASOURCEINFO;

typedef struct tagIVBIND2_DATARECORDINFO
{
    UINT64   uDataRecordId;
    UINT64   uDataSourceId;
    FILETIME ftTime;
    char     szData[ 320 ];
} IVBIND2_DATARECORDINFO;

typedef void ( CALLBACK* IVBIND2_ZONECALLBACK )(
    void*             pUserParam,
    IVBIND2_ZONEINFO* pZoneInfo,
    const char*       pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_ZONECALLBACK2 )(
    void*              pUserParam,
    INT                nZoneNotificationType,
    IVBIND2_ZONEINFO2* pZoneInfo,
    const char*        pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_ALARMRECORDCALLBACK )(
    void*                       pUserParam,
    INT                         nAlarmNotificationType,
    IVBIND2_ALARMRECORDINFO*    pAlarmInfo,
    const char*                 pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_DETECTORCALLBACK )(
    void*                 pUserParam,
    IVBIND2_DETECTORINFO* pDetectorInfo,
    const char*           pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_DETECTORCALLBACK2 )(
    void*                    pUserParam,
    INT                      nDetectorNotificationType,
    IVBIND2_DETECTORINFO2*   pDetectorInfo,
    const char*              pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_ACTIVATIONRECORDCALLBACK )(
    void*                           pUserParam,
    INT                             nActivationNotificationType,
    IVBIND2_ACTIVATIONRECORDINFO*   pAlarmInfo,
    const char*                     pszAsIpAddress
    );

typedef void ( CALLBACK* IVBIND2_RELAYCALLBACK )(
    void*                 pUserParam,
    IVBIND2_PINSTATEINFO* pPinStateInfo,
    const char*           pszRemoteIpAddress,
    BOOL*                 pbError
    );

typedef void( CALLBACK* IVBIND2_EXTERNALDATASOURCECALLBACK )(
    void*                           pUserParam,
    INT                             nSourceNotificationType,
    IVBIND2_EXTERNALDATASOURCEINFO* pSourceInfo,
    const char*                     pszAsIpAddress );

typedef void( CALLBACK* IVBIND2_DATARECORDCALLBACK )(
    void*                   pUserParam,
    INT                     nDataNotificationType,
    IVBIND2_DATARECORDINFO* pDataInfo,
    const char*             pszAsIpAddress );

#ifdef __cplusplus
extern "C" {
#endif

    int IVBIND2API IvBind2SendEvent(
        const char*     pszAsIpAddress,
        int             nEventNum,
        const char*     pszLocalIpAddress,
        const FILETIME* pftTimeStamp
        );

    int IVBIND2API IvBind2SendEvent2(
        const char*     pszAsIpAddress,
        int             nEventNum,
        const char*     pszLocalIpAddress,
        const FILETIME* pftTimeStamp,
        const char*     pszAnnotation );

    int IVBIND2API IvBind2AddBookmark(
        const char*      pszNvrIpAddress,
        const char*      pszCameraIpAddress,
        const char*      pszBookmarkText,
        const FILETIME*  pftTimeStamp
        );

    int IVBIND2API IvBind2AddBookmark2(
        const char*      pszNvrIpAddress,
        const char*      pszCamServiceId,
        const char*      pszBookmarkText,
        const FILETIME*  pftTimeStamp,
        unsigned         pftSecurityLevel
        );

    int IVBIND2API IvBind2AckZone(
        const char*      pszAsIpAddress,
        const char*      pszZoneName,
        const char*      pszAckDescription
        );

    int IVBIND2API IvBind2ClearZone(
        const char*      pszAsIpAddress,
        const char*      pszZoneName,
        const char*      pszClearDescription
        );

    int IVBIND2API IvBind2SetZone(
        const char*      pszAsIpAddress,
        const char*      pszZoneName
        );

    int IVBIND2API IvBind2UnsetZone(
        const char*      pszAsIpAddress,
        const char*      pszZoneName
        );

    int IVBIND2API IvBind2IsolateDetector(
        const char*      pszAsIpAddress,
        const char*      pszZoneName,
        const char*      pszDetectorName
        );

    int IVBIND2API IvBind2RestoreDetector(
        const char*      pszAsIpAddress,
        const char*      pszZoneName,
        const char*      pszDetectorName
        );

    int IVBIND2API IvBind2IsolateDetector2(
        const char*      pszAsIpAddress,
        const char*      pszZoneName,
        const char*      pszDetectorName,
        const char*      pszIsolateReason
        );

    int IVBIND2API IvBind2RegisterZoneChanges(
        const char*           pszIpAddress,
        IVBIND2_ZONECALLBACK  pfnUserCallback,
        void*                 pUserParam
        );

    int IVBIND2API IvBind2RegisterZoneChanges2(
        const char*           pszIpAddress,
        IVBIND2_ZONECALLBACK2 pfnUserCallback,
        void*                 pUserParam
        );

    int IVBIND2API IvBind2UnregisterZoneChanges(
        const char* pszIpAddress
        );

    int IVBIND2API IvBind2QueryAlarmRecords(
        const char*                 pszIpAddress,
        IVBIND2_ALARMRECORDCALLBACK pfnUserCallback,
        void*                       pUserParam,
        const FILETIME*             pftMinimumTime,
        const FILETIME*             pftMaximumTime
        );

    int IVBIND2API IvBind2StopAlarmRecordQuery(
        const char* pszIpAddress
        );

    int IVBIND2API IvBind2RegisterDetectorChanges(
        const char*               pszIpAddress,
        IVBIND2_DETECTORCALLBACK  pfnUserCallback,
        void*                     pUserParam
        );

    int IVBIND2API IvBind2RegisterDetectorChanges2(
        const char*                 pszLocalIpAddress,
        IVBIND2_DETECTORCALLBACK2   pfnUserCallback,
        void*                       pUserParam
        );

    int IVBIND2API IvBind2UnregisterDetectorChanges(
        const char* pszIpAddress
        );

    int IVBIND2API IvBind2QueryActivationRecords(
        const char*                         pszIpAddress,
        IVBIND2_ACTIVATIONRECORDCALLBACK    pfnUserCallback,
        void*                               pUserParam,
        const FILETIME*                     pftMinimumTime,
        const FILETIME*                     pftMaximumTime
        );

    int IVBIND2API IvBind2StopActivationRecordQuery(
        const char* pszIpAddress
        );

    int IVBIND2API IvBind2RegisterRelayChanges(
        const char*             pszLocalIpAddress,
        IVBIND2_RELAYCALLBACK   pfnUserCallback,
        void*                   pUserParam
        );

    int IVBIND2API IvBind2UnregisterRelayChanges(
        );

    int IVBIND2API IvBind2SendData(
        const char*         pszAsIpAddress,
        const char*         pszExtSystemIpAddress,
        int                 nSourceNumber,
        const FILETIME*     pftTimeStamp,
        const char*         pszData
        );

    int IVBIND2API IvBind2QueryExternalDataSources(
        const char*                        pszIpAddress,
        IVBIND2_EXTERNALDATASOURCECALLBACK pfnUserCallback,
        void*                              pUserParam );

    int IVBIND2API
    IvBind2StopExternalDataSourceQuery( const char* pszIpAddress );

    int IVBIND2API IvBind2QueryDataRecords(
        const char*                pszIpAddress,
        IVBIND2_DATARECORDCALLBACK pfnUserCallback,
        void*                      pUserParam,
        const FILETIME*            pftMinimumTime,
        const FILETIME*            pftMaximumTime,
        const UINT64*              pSourceIdArray,
        int                        nSourceIdCount,
        const char*                pszDataFilter,
        BOOL                       fLiveQueryOnly );

    int IVBIND2API IvBind2StopDataRecordQuery( const char* pszIpAddress );

    int IVBIND2API
        IvBind2SetDnaProxy( const char* pszIpAddress, unsigned short nPort );

#ifdef __cplusplus
}
#endif

#endif /* IVBIND2_H_ */
