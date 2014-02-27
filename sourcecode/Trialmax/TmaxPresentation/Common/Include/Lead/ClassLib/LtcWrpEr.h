/*--------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|---------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcprnt.h                                                        |
| DESC      :                                                                 |
+--------------------------------------*/

#ifndef  _LEAD_WRAPPER_ERRORS_H_
#define  _LEAD_WRAPPER_ERRORS_H_

/*--------------------------------------+
| DEFINES                                                                     |
+--------------------------------------*/

//---------------------------------------

//-WRAPPER SPECIFIC ERRORS--------------------------
#define END_LEAD_ERROR                             -917
#define WRAPPER_START_ERROR                        -2000

#define WRPERR_BITMAP_NOT_ALLOCATED                -2000     // The bitmap has not been allocated
#define WRPERR_INVALID_CLASS                       -2001     // The class object is invalid or has not been initialized
#define WRPERR_INVALID_PARAMETERS                  -2002     // One or more invalid parameters were specified
#define WRPERR_BITMAP_BPP                          -2003     // The bitmap's bits per pixel is invalid for this operation
#define WRPERR_BITMAP_DITHER_NOT_STARTED           -2004     // LBitmapBase::StartDithering must be called first
#define WRPERR_BITMAP_ALREADY_ALLOCATED            -2005     // The bitmap has already been allocated
#define WRPERR_BITMAP_PREPARE_FAILED               -2006     // Bitmap prepare failed
#define WRPERR_BITMAP_ITEM_IN_LIST                 -2007     // Bitmap is an item from list
#define WRPERR_BITMAP_CONVERT                      -2008     // Error converting bitmap
#define WRPERR_INET_ITEM_NOTFOUND                  -2009     // Internet item not found
#define WRPERR_INET_NOMORE_SPACE                   -2010     // Internet no more space
#define WRPERR_INET_ALREADY_CONNECTED              -2011     // Internet already connected
#define WRPERR_MMCAPTURE_WINDOW_ALREADY_CREATED    -2012     // Multimedia capture window already created
#define WRPERR_MMEDIA_ALREADY_CREATED              -2013     // Multimedia already created
#define WRPERR_PLAYBACK_ALREADY_CREATED            -2014     // Playback already created
#define WRPERR_LIST_ALREADY_CREATED                -2015     // List already created
#define WRPERR_LIST_NOT_CREATED                    -2016     // List not created
#define WRPERR_CLASS_NOT_READY                     -2017     // Class not ready
#define WRPERR_BUFFER_NO_MEMORY                    -2018     // Buffer no memory
#define WRPERR_BUFFER_ERRSIZE                      -2019     // Invalid buffer size
#define WRPERR_BUFFER_INVALID_HANDLE               -2020     // Invalid buffer handle
#define WRPERR_FILE_FEEDLOAD_NOT_STARTED           -2021     // Feedload not started
#define WRPERR_MEMFILE_COMP_NOT_STARTED            -2022     // Compression engine not started
#define WRPERR_ANN_INVALID_FILE                    -2023     // Invalid annonation file
#define WRPERR_ANN_INVALID_FILEMEM                 -2024     // Invalid annonation file memory
#define WRPERR_ANN_INVALID_OBJECT                  -2025     // Invalid annonation object
#define WRPERR_ANN_DESTROYTOOLBAR_FAILED           -2026     // Cannot destroy toolbar
#define WRPERR_ANN_ALLOCATED_MEMORY                -2027     // Error allocating memory
#define WRPERR_BUFFER_NOTVALID                     -2028     // Invalid buffer
#define WRPERR_BUFFER_REALLOCATE                   -2029     // Error reallocating buffer
#define WRPERR_BUFFER_COPY                         -2030     // Error copying buffer
#define WRPERR_BUFFER_RESIZE_NOT_STARTED           -2031     // Resize Engine not started
#define WRPERR_BUFFER_LOCKED                       -2032     // Buffer locked
#define WRPERR_BITMAPWND_PANNOTCREATED             -2033     // Pan window not created
#define WRPERR_BITMAPWND_REGISTER                  -2034     // Error registering window
#define WRPERR_BITMAPWND_CREATEWINDOW              -2035     // Error creating window
#define WRPERR_ANIMATION_ALREADY_STARTED           -2036     // Animation playback already started
#define WRPERR_ANIMATION_IS_RUNNING                -2037     // Animation is running
#define WRPERR_ANNWND_CANT_CREATE_OBJECT           -2038     // Cannot create annotation window
#define WRPERR_LTKRN_DLL_NOT_LOADED                -2039     // ltkrn dll not loaded
#define WRPERR_LTDIS_DLL_NOT_LOADED                -2040     // ltdis dll not loaded
#define WRPERR_LTFIL_DLL_NOT_LOADED                -2041     // ltfil dll not loaded
#define WRPERR_LTIMG_DLL_NOT_LOADED                -2042     // ltimg dll not loaded
#define WRPERR_LTEFX_DLL_NOT_LOADED                -2043     // ltefx dll not loaded
#define WRPERR_LTDLG_DLL_NOT_LOADED                -2044     // ltdlg dll not loaded
#define WRPERR_LTTWN_DLL_NOT_LOADED                -2045     // lttwn dll not loaded
#define WRPERR_LTSCR_DLL_NOT_LOADED                -2046     // ltscr dll not loaded
#define WRPERR_LTANN_DLL_NOT_LOADED                -2047     // ltann dll not loaded
#define WRPERR_LTNET_DLL_NOT_LOADED                -2048     // ltnet dll not loaded
#define WRPERR_BITMAPLIST_ITEM_OPERATION_ERROR     -2049     // Bitmaplist item operation error
#define WRPERR_BITMAPLIST_NOT_CREATED              -2050     // Bitmaplist not created
#define WRPERR_ANIMATION_INVALID_FILE              -2051     // Invalid Animation File
#define WRPERR_NOT_LEAD_BITMAP                     -2052     // Not a LEAD Bitmap
#define WRPERR_BITMAP_IS_ALIST_MEMBER              -2053     // Bitmap is a bitmaplist member
#define WRPERR_WINDOW_NOT_CREATED                  -2054     // Window not created
#define WRPERR_OPERATION_NOT_ALLOWED               -2055     // Operation not Allowed
#define WRPERR_OPERATION_CANCELED                  -2056     // Operation Canceled
#define WRPERR_LTTMB_DLL_NOT_LOADED                -2057     // lttmb dll not loaded
#define WRPERR_LTLST_DLL_NOT_LOADED                -2058     // ltlst dll not loaded
#define WRPERR_IMAGELISTCONTROL_CREATE             -2059     // ImageList Control not created
#define WRPERR_LVKRN_DLL_NOT_LOADED                -2060     // lvkrn dll not loaded
#define WRPERR_NO_VECTOR                           -2061     // Vector is not loaded
#define WRPERR_VECTOR_NOT_ALLOCATED                -2062     // Vector is not allocated
#define WRPERR_LVDLG_DLL_NOT_LOADED                -2063     // lvdlg dll not loaded
#define WRPERR_FEATURE_NOT_SUPPORTED_IN_DIRECTX    -2064     // Feature not supported in DirectX
#define WRPERR_FEATURE_NOT_SUPPORTED               -2065     // Feature not supported
#define WRPERR_VECTOR_NOT_ASSOCIATED               -2066     // Vector not associated
#define WRPERR_VECTOR_INVALID_LAYER                -2067     // Invalid Vector Layer
#define WRPERR_VECTOR_INVALID_OBJECT               -2068     // Invalid Vector Object
#define WRPERR_VECTOR_INVALID_OBJECT_TYPE          -2069     // Invalid Vector Object Type
#define WRPERR_VECTOR_INVALID_OBJECT_DESC          -2070     // Invalid Vector Object Descriptor
#define WRPERR_VECTOR_LOCK_ERROR                   -2071     // Vector is locked
#define WRPERR_LTBAR_DLL_NOT_LOADED                -2072     // ltbar.dll not loaded
#define WRPERR_LTAUT_DLL_NOT_LOADED                -2073     // ltaut.dll not loaded
#define WRPERR_LTCON_DLL_NOT_LOADED                -2074     // ltcon.dll not loaded
#define WRPERR_LDKRN_DLL_NOT_LOADED                -2075     // ldkrn.dll not loaded
#define WRPERR_LTTLB_DLL_NOT_LOADED                -2076     // lttlb.dll not loaded
#define WRPERR_LTPNT_DLL_NOT_LOADED                -2077     // ltpnt.dll not loaded
#define WRPERR_LTPDG_DLL_NOT_LOADED                -2078     // ltpdg.dll not loaded
#define WRPERR_VECTOR_INVALID_GROUP                -2079     // Invalid Vector Group
#define WRPERR_LTWEB_DLL_NOT_LOADED                -2080     // ltweb dll not loaded
#define WRPERR_LTSGM_DLL_NOT_LOADED                -2081     // ltsgm dll not loaded

#define WRPERR_LTDLGKRN_DLL_NOT_LOADED             -2082     // ltdlgkrn dll not loaded
#define WRPERR_LTDLGCLR_DLL_NOT_LOADED             -2083     // ltdlgclr dll not loaded
#define WRPERR_LTDLGWEB_DLL_NOT_LOADED             -2084     // ltdlgweb dll not loaded
#define WRPERR_LTDLGIMG_DLL_NOT_LOADED             -2085     // ltdlgimg dll not loaded
#define WRPERR_LTDLGEFX_DLL_NOT_LOADED             -2086     // ltdlgefx dll not loaded
#define WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED          -2087     // ltdlgimgdoc dll not loaded
#define WRPERR_LTDLGFILE_DLL_NOT_LOADED            -2088     // ltdlgfile dll not loaded
#define WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED          -2089     // ltdlgimgefx dll not loaded
#define WRPERR_LTZMV_DLL_NOT_LOADED                -2090     // ltzmv dll not loaded
#define WRPERR_LTIMGOPT_DLL_NOT_LOADED             -2091     // ltimgopt dll not loaded
#define WRPERR_LCMRC_DLL_NOT_LOADED                -2092     // lcmrc dll not loaded
#define WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED   -2093     // ltivw dll not loaded
#define WRPERR_IMAGEVIEWERCONTROL_NOT_CREATED      -2094     // Image Viewer Control not created
#define WRPERR_IMAGEVIEWERCONTROL_ALREADY_CREATED  -2095     // Image Viewer Control already created
#define WRPERR_LTCLR_DLL_NOT_LOADED                -2096     // ltclr dll not loaded
#define WRPERR_LTIMGCOR_DLL_NOT_LOADED             -2097     // ltimgcor dll not loaded
#define WRPERR_LTIMGCLR_DLL_NOT_LOADED             -2098     // ltimgclr dll not loaded
#define WRPERR_LTIMGSFX_DLL_NOT_LOADED             -2099     // ltimgsfx dll not loaded
#define WRPERR_LTIMGEFX_DLL_NOT_LOADED             -2100     // ltimgefx dll not loaded
#define WRPERR_LTNTF_DLL_NOT_LOADED                -2101     // ltntf dll not loaded
#define WRPERR_NITF_NOT_CREATED                    -2102     // NITF handle not created
#define WRPERR_LTWIA_DLL_NOT_LOADED                -2103     // ltwia dll not loaded
#define WRPERR_END_ERROR                           -2104
#endif //_LEAD_WRAPPER_ERRORS_H_ 
/*================================================================= EOF =====*/

