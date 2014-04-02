/*************************************************************
   Ltaut.h - Automation runtime library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTAUT_H)
#define LTAUT_H

#if !defined(L_LTAUT_API)
   #define L_LTAUT_API
#endif // #if !defined(L_LTAUT_API)

#include "Lttyp.h"
#include "Lterr.h"
#include "Lvkrn.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

//.....................................................................................................................
// CONSTANTS 
//.....................................................................................................................
#define DEF_AUTOMATION_UNDO_LEVEL         16
#define AUTOMATION_PAINT_MIN_ZOOM        -32
#define AUTOMATION_PAINT_MAX_ZOOM         32

#if defined (LEADTOOLS_V16_OR_LATER)
typedef enum AUTOMATION_MODE
{
   AUTOMATION_MODE_PAINT,
   AUTOMATION_MODE_VECTOR,
   AUTOMATION_MODE_ANNOTATION

} AUTOMATIONMODE, *pAUTOMATIONMODE ;

#else
typedef enum _AUTOMATION_MODE
{
   AUTOMATION_MODE_PAINT,
   AUTOMATION_MODE_VECTOR,
   AUTOMATION_MODE_ANNOTATION

} AUTOMATIONMODE, *pAUTOMATIONMODE ;

#endif // #if defined (LEADTOOLS_V16_OR_LATER)

typedef enum AUTOMATIONSELECT
{
   AUTOMATION_SELECT_NONE,
   AUTOMATION_SELECT_ALL

} AUTOMATIONSELECT, *pAUTOMATIONSELECT ;

//.....................................................................................................................
// TYPES
//.....................................................................................................................

//.....................................................................................................................
// PAINTING TYPES
//.....................................................................................................................
#if defined (LEADTOOLS_V16_OR_LATER)
typedef struct CONTAINERPAINTDATA
{
   pBITMAPHANDLE pBitmap ;
   HPALETTE      hRestrictionPalette ;

} CONTAINERPAINTDATA, *pCONTAINERPAINTDATA ; 
#else
typedef struct _CONTAINERPAINTDATA
{
   pBITMAPHANDLE pBitmap ;
   HPALETTE      hRestrictionPalette ;

} CONTAINERPAINTDATA, *pCONTAINERPAINTDATA ; 
#endif // #if defined (LEADTOOLS_V16_OR_LATER)

//.....................................................................................................................
// VECTOR Properties
//.....................................................................................................................
typedef struct _AUTOMATIONVECTORPROPERTIES
{
   L_INT          nSize;
   L_UINT32       dwMask;
   VECTORPEN      Pen;
   VECTORBRUSH    Brush;
   VECTORFONT     Font;
}
AUTOMATIONVECTORPROPERTIES, *pAUTOMATIONVECTORPROPERTIES; 

// Masks for AUTOMATIONVECTORPROPERTIES
typedef enum AUTOMATION_VECTOR_MASK
{
   AUTOMATION_VECTOR_PEN   = 1,
   AUTOMATION_VECTOR_BRUSH = 2,
   AUTOMATION_VECTOR_FONT  = 4
}
AUTOMATION_VECTOR_MASK, *pAUTOMATION_VECTOR_MASK;


//.....................................................................................................................
// Automation handle.
//.....................................................................................................................
typedef L_VOID *           pAUTOMATIONHANDLE ;
typedef pAUTOMATIONHANDLE* ppAUTOMATIONHANDLE ;

typedef struct _INTERNALAUTOMATIONHANDLE
{
   //{ GENERAL STUFF

      L_INT               nSize ; 
      L_UINT32            dwSignature ; 
      AUTOMATIONMODE      nAutomationMode ; 
      L_VOID *            pContainerList ;
      pCONTAINERHANDLE    hActiveContainer ;
      pCONTAINERCALLBACK  pContainerCallback ;
      pTOOLBARHANDLE      pToolbar ;
      pTOOLBARCALLBACK    pToolbarCallback ;
      L_INT               nCurrentTool;
      L_UINT              uUndoLevel ;

   //} GENERAL STUFF

   //{ PAINTING STUFF

      L_VOID * pPaintAutomationObject ; 

   //} PAINTING STUFF

   //{ VECTOR STUFF

      L_VOID  *pVectorAutomationObject;

   //} VECTOR STUFF

   //{ ANNOTATION STUFF

   //} ANNOTATION STUFF

} INTERNALAUTOMATIONHANDLE, *pINTERNALAUTOMATIONHANDLE ;

//.....................................................................................................................
// USER CALLBACK FUNCTION
//.....................................................................................................................
typedef L_INT ( pEXT_CALLBACK pAUTCONTAINERCALLBACK ) ( CONTAINEREVENTTYPE nEventType,
                                                        L_VOID  *pEventData, 
                                                        L_VOID  *pUserData ) ;

typedef L_INT ( pEXT_CALLBACK pAUTOMATIONENUMCONTAINERPROC ) ( pCONTAINERHANDLE pContainer, L_VOID  *pUserData ) ;

//.....................................................................................................................
// VARIABLES 
//.....................................................................................................................

//.....................................................................................................................
// FUNCTIONS 
//.....................................................................................................................

//.....................................................................................................................
// EXPORTED FUNCTIONS 
//.....................................................................................................................

// General functions.
L_LTAUT_API L_INT EXT_FUNCTION L_AutIsValid            ( pAUTOMATIONHANDLE pAutomation ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutInit               ( ppAUTOMATIONHANDLE ppAutomation ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutCreate             ( pAUTOMATIONHANDLE pAutomation, AUTOMATIONMODE nMode, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutFree               ( pAUTOMATIONHANDLE pAutomation ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetUndoLevel       ( pAUTOMATIONHANDLE pAutomation, L_UINT uLevel ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetUndoLevel       ( pAUTOMATIONHANDLE pAutomation, L_UINT *puLevel ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutCanUndo            ( pAUTOMATIONHANDLE pAutomation, L_BOOL *pfCanUndo ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutCanRedo            ( pAUTOMATIONHANDLE pAutomation, L_BOOL *pfCanRedo ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutUndo               ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutRedo               ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetUndoEnabled     ( pAUTOMATIONHANDLE pAutomation, L_BOOL bEnabled );
L_LTAUT_API L_INT EXT_FUNCTION L_AutAddUndoNode        ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags );
L_LTAUT_API L_INT EXT_FUNCTION L_AutSelect             ( pAUTOMATIONHANDLE pAutomation, AUTOMATIONSELECT nSelect, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutClipboardDataReady ( pAUTOMATIONHANDLE pAutomation, L_BOOL *pfReady ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutCut                ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutCopy               ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutPaste              ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutDelete             ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutPrint              ( pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags ) ;

// Container Functions.
L_LTAUT_API L_INT EXT_FUNCTION L_AutAddContainer       ( pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer , L_VOID  *pModeData ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutRemoveContainer    ( pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetActiveContainer ( pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetActiveContainer ( pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE *ppContainer ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutEnumContainers     ( pAUTOMATIONHANDLE pAutomation, pAUTOMATIONENUMCONTAINERPROC pEnumProc, L_VOID  *pUserData ) ;

// Painting Functionts.
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetPaintPropertyA  ( pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, const L_VOID  *pProperty ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetPaintPropertyA  ( pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, L_VOID  *pProperty ) ;
#if defined(FOR_UNICODE)
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetPaintProperty  ( pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, const L_VOID  *pProperty ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetPaintProperty  ( pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, L_VOID  *pProperty ) ;
#else
#define L_AutSetPaintProperty L_AutSetPaintPropertyA
#define L_AutGetPaintProperty L_AutGetPaintPropertyA
#endif //#if defined(FOR_UNICODE)
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetPaintBkColor   ( pAUTOMATIONHANDLE pAutomation, COLORREF rcBKColor ) ;
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetPaintBkColor   ( pAUTOMATIONHANDLE pAutomation, COLORREF *prcBKColor ) ;

// Vector Functions.
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetVectorProperty( pAUTOMATIONHANDLE pAutomation, const pAUTOMATIONVECTORPROPERTIES pProps);
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetVectorProperty( const pAUTOMATIONHANDLE pAutomation, pAUTOMATIONVECTORPROPERTIES pProps);
L_LTAUT_API L_INT EXT_FUNCTION L_AutEditVectorObject( pAUTOMATIONHANDLE, const pVECTOROBJECT );

//Toolbar Functions.
L_LTAUT_API L_INT EXT_FUNCTION L_AutSetToolbar ( pAUTOMATIONHANDLE pAutomation, pTOOLBARHANDLE pToolbar );
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetToolbar ( pAUTOMATIONHANDLE pAutomation, pTOOLBARHANDLE *ppToolbar ) ;

L_LTAUT_API L_INT EXT_FUNCTION L_AutSetCurrentTool( pAUTOMATIONHANDLE pAutomation, L_INT nTool );
L_LTAUT_API L_INT EXT_FUNCTION L_AutGetCurrentTool( pAUTOMATIONHANDLE pAutomation, L_INT *pnTool );

#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif // #if !defined(LTAUT_H)
