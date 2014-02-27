/*
   LTFIL.H - file module header file
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef _LTFIL_H_
#define _LTFIL_H_

#include "ltkrn.h"
#include "lvkrn.h"
#include "ldkrn.h"

#define _HEADER_ENTRY_
#include "ltpck.h"

/*
   flags for L_LoadFile
*/
#define LOADFILE_ALLOCATE     0x0001   /* Allocate image memory */
#define LOADFILE_STORE        0x0002   /* Auto-store image lines */
#define LOADFILE_FIXEDPALETTE 0x0004   /* Allow fixed palettes only */
#define LOADFILE_NOINTERLACE  0x0008   /* Don't send interlaced lines */
#define LOADFILE_ALLPAGES     0x0010   /* Load all file pages */
#define LOADFILE_NOINITBITMAP 0x0020   /* Don't initialize the bitmap handle */
#define LOADFILE_COMPRESSED   0x0040   /* Allow compressed 1 bit images */

/*
   flags for L_SaveFile
*/
#define SAVEFILE_FIXEDPALETTE      0x0001 /* Save with Fixed Palette */
#define SAVEFILE_OPTIMIZEDPALETTE  0x0002 /* Save with Bitmap's Palette */
#define SAVEFILE_MULTIPAGE         0x0004 /* Save as multipage */

/*
   list of file types supported
*/
#define FILE_PCX                  1 /* ZSoft PCX */
#define FILE_GIF                  2 /* CompuServe GIF */
#define FILE_TIF                  3 /* Tagged Image File Format */
#define FILE_TGA                  4 /* Targa */
#define FILE_CMP                  5 /* LEAD CMP */
#define FILE_BMP                  6 /* Windows BMP */
#define FROM_BUFFER               7 /* Internal use */
#define FILE_BITMAP               9 /* Internal use */
#define FILE_JFIF                10 /* Jpeg File Interchange Format */
#define FILE_JTIF                11 /* Jpeg Tag Image File Format */
#define FILE_BIN                 12 /* Internal use */
#define FILE_HANDLE              13 /* Internal use */
#define FILE_OS2                 14 /* OS/2 BMP */
#define FILE_WMF                 15 /* Windows Meta File */
#define FILE_EPS                 16 /* Encapsulated Post Script */
#define FILE_TIFLZW              17 /* TIF Format with LZW compression */
#define FILE_LEAD                20 /* LEAD  Proprietary */
#define FILE_LEAD1JFIF           21 /* JPEG  4:1:1 */
#define FILE_LEAD1JTIF           22 /* JPEG  4:1:1 */
#define FILE_LEAD2JFIF           23 /* JPEG  4:2:2 */
#define FILE_LEAD2JTIF           24 /* JPEG  4:2:2 */
#define FILE_CCITT               25 /* TIFF  CCITT */
#define FILE_LEAD1BIT            26 /* LEAD 1 bit, lossless compression */
#define FILE_CCITT_GROUP3_1DIM   27 /* CCITT Group3 one dimension */
#define FILE_CCITT_GROUP3_2DIM   28 /* CCITT Group3 two dimensions */
#define FILE_CCITT_GROUP4        29 /* CCITT Group4 two dimensions */
#define FILE_LEAD_NOLOSS         30 /* LEAD  Proprietary Lossless */
#define FILE_LEAD1BITA           31 /* old LEAD 1 bit, lossless compression */

#define FILE_CALS                50
#define FILE_MAC                 51
#define FILE_IMG                 52
#define FILE_MSP                 53
#define FILE_WPG                 54
#define FILE_RAS                 55
#define FILE_PCT                 56
#define FILE_PCD                 57
#define FILE_DXF                 58
#define FILE_AVI                 59
#define FILE_WAV                 60
#define FILE_FLI                 61
#define FILE_CGM                 62
#define FILE_EPSTIFF             63 /* EPS with TIFF Preview */
#define FILE_EPSWMF              64 /* EPS with Metafile Preview */
#define FILE_CMPNOLOSS           65
#define FILE_FAX_G3_1D           66
#define FILE_FAX_G3_2D           67
#define FILE_FAX_G4              68
#define FILE_WFX_G3_1D           69
#define FILE_WFX_G4              70
#define FILE_ICA_G3_1D           71
#define FILE_ICA_G3_2D           72
#define FILE_ICA_G4              73
#define FILE_OS2_2               74
#define FILE_PNG                 75
#define FILE_PSD                 76
#define FILE_RAWICA_G3_1D        77
#define FILE_RAWICA_G3_2D        78
#define FILE_RAWICA_G4           79
#define FILE_FPX                 80  /* FlashPix, no compression */
#define FILE_FPX_SINGLE_COLOR    81  /* FlashPix, compression 'single color' method */
#define FILE_FPX_JPEG            82  /* FlashPix, compression JPEG */
#define FILE_FPX_JPEG_QFACTOR    83  /* FlashPix, compression JPEG, specify qFactor */
#define FILE_BMP_RLE             84  /* compressed Windows BMP */
#define FILE_TIF_CMYK            85  /* TIFF no compression,      CMYK data */
#define FILE_TIFLZW_CMYK         86  /* TIFF LZW compression,     CMYK data */
#define FILE_TIF_PACKBITS        87  /* TIFF PackBits compression, RGB data */
#define FILE_TIF_PACKBITS_CMYK   88  /* TIFF PackBits compression, CMYK data */
#define FILE_DICOM_GRAY          89
#define FILE_DICOM_COLOR         90
#define FILE_WIN_ICO             91
#define FILE_WIN_CUR             92
#define FILE_TIF_YCC             93 /* TIFF YCbCr color space, no compression */
#define FILE_TIFLZW_YCC          94 /* TIFF YCbCr color space, LZW compression */
#define FILE_TIF_PACKBITS_YCC    95 /* TIFF YCbCr color space, PackBits compression */
#define FILE_EXIF                96 /* uncompressed RGB Exif file */
#define FILE_EXIF_YCC            97 /* uncompressed YCbCr Exif file */
#define FILE_EXIF_JPEG           98 /* JPEG 4:2:2 compressed Exif file */
#define FILE_AWD                 99 /* Microsoft Fax format */
#define FILE_FASTEST             100 /* for ISIS only! use the data as is, from the ISIS Scanner */
#define FILE_EXIF_JPEG_411       101 /* JPEG 4:1:1 compressed Exif file */

#define FILE_PBM_ASCII           102
#define FILE_PBM_BINARY          103
#define FILE_PGM_ASCII           104
#define FILE_PGM_BINARY          105
#define FILE_PPM_ASCII           106
#define FILE_PPM_BINARY          107
#define FILE_CUT                 108
#define FILE_XPM                 109
#define FILE_XBM                 110
#define FILE_IFF_ILBM            111
#define FILE_IFF_CAT             112
#define FILE_XWD                 113
#define FILE_CLP                 114
#define FILE_JBIG                115
#define FILE_EMF                 116
#define FILE_ICA_IBM_MMR         117 /* MO:DCA IBM MMR CCITT G32D */
#define FILE_RAWICA_IBM_MMR      118 /* IOCA IBM MMR CCITT G32D */
#define FILE_ANI                 119
#define FILE_LASERDATA           121   /* LaserData CCITT G4 */
#define FILE_INTERGRAPH_RLE      122   /* Intergraph RLE */
#define FILE_INTERGRAPH_VECTOR   123   /* Intergraph Vector */
#define FILE_DWG                 124

#define FILE_DICOM_RLE_GRAY      125
#define FILE_DICOM_RLE_COLOR     126
#define FILE_DICOM_JPEG_GRAY     127
#define FILE_DICOM_JPEG_COLOR    128

#define FILE_CALS4               129
#define FILE_CALS2               130
#define FILE_CALS3               131

#define FILE_XWD10               132
#define FILE_XWD11               133
#define FILE_FLC                 134
#define FILE_KDC                 135
#define FILE_DRW                 136
#define FILE_PLT                 137

#define FILE_TIF_CMP             138
#define FILE_TIF_JBIG            139
#define FILE_TIF_DXF             140
#define FILE_TIF_UNKNOWN         141   /* TIFF file with unknown compression */

#define FILE_SGI                 142
#define FILE_SGI_RLE             143
#define FILE_VECTOR_DUMP         144
#define FILE_DWF                 145

/* under development */
#define FILE_RAS_PDF             146   /* Raster PDF uncompressed */
#define FILE_RAS_PDF_G3_1D       147   /* Raster PDF CCITT G3 1D compression */
#define FILE_RAS_PDF_G3_2D       148   /* Raster PDF CCITT G3 2D compression */
#define FILE_RAS_PDF_G4          149   /* Raster PDF CCITT G4 compression */
#define FILE_RAS_PDF_JPEG        150   /* Raster PDF JPEG 24-bit color 4:4:4 or grayscale 8-bit */
#define FILE_RAS_PDF_JPEG_422    151   /* Raster PDF JPEG 24-bit color 4:2:2 */
#define FILE_RAS_PDF_JPEG_411    152   /* Raster PDF JPEG 24-bit color 4:1:1 */

#define FILE_RAW                 153   /* Raw uncompressed data */

#define FILE_RASTER_DUMP         154
#define FILE_TIFF_CUSTOM         155

#define FILE_RAW_RGB             156   /* Raw RGB */
#define FILE_RAW_RLE4            157   /* Raw RLE4 compressed 4-bit data */
#define FILE_RAW_RLE8            158   /* Raw RLE8 compressed 8-bit data */
#define FILE_RAW_BITFIELDS       159   /* Raw BITFIELD data--16 or 32 bit */
#define FILE_RAW_PACKBITS        160   /* Raw Packbits compression */
#define FILE_RAW_JPEG            161   /* Raw JPEG compression */
#define FILE_FAX_G3_1D_NOEOL     162  /* Raw CCITT compression--CCITT with no eol */
#define FILE_RAW_CCITT           (FILE_FAX_G3_1D_NOEOL)


/* File formats supported by the OCR module */
#define FILE_AMI_PRO_20             200   /* Ami Pro 2.0 */
#define FILE_AMI_PRO_30             201   /* Ami Pro 3.0 */
#define FILE_ASCII_SMART            202   /* ASCII Smart */
#define FILE_ASCII_STANDARD         203   /* ASCII Standard */
#define FILE_ASCII_STANDARD_DOS     204   /* ASCII Standard (DOS) */
#define FILE_ASCII_STRIPPED         205   /* ASCII Stripped */
#define FILE_DBASE_IV_10            206   /* dBase IV v1.0 */
#define FILE_DCA_RFT                207   /* DCA/RFT */
#define FILE_DCA_RFT_DW_5           208   /* DisplayWrite 5 */
#define FILE_EXCEL_MAC              209   /* Excel for the Macintosh */
#define FILE_EXCEL_30               210   /* Excel 3.0 */
#define FILE_EXCEL_40               211   /* Excel 4.0 */
#define FILE_EXCEL_50               212   /* Excel 5.0 */
#define FILE_EXCEL_OFFICE97         213   /* Excel Office 97 */
#define FILE_FRAMEMAKER             214   /* FrameMaker */
#define FILE_HTML_20                215   /* HTML (2.0 specification) */
#define FILE_HTML_EDITOR_20         216   /* HTML (SoftQuad Editor) */
#define FILE_HTML_NETSCAPE_20       217   /* HTML (Netscape additions) */
#define FILE_INTERLEAF              218   /* Interleaf */
#define FILE_LOTUS123               219   /* Lotus 1-2-3 */
#define FILE_LOTUS_WORD_PRO         220   /* Lotus Word Pro */
#define FILE_MULTIMATE_ADV_II       221   /* MultiMate Advantage II */
#define FILE_POSTSCRIPT             222   /* Postscript */
#define FILE_PROFESSIONAL_WRITE_20  223   /* Professional Write 2.0 */
#define FILE_PROFESSIONAL_WRITE_22  224   /* Professional Write 2.2 */
#define FILE_QUATTRA_PRO            225   /* Quattra Pro */
#define FILE_RTF                    226   /* Rich Text Format */
#define FILE_RTF_MAC                227   /* Rich Text Format (Macintosh) */
#define FILE_RTF_WORD_60            228   /* Rich Text Format (Word 6.0) */
#define FILE_WINDOWS_WRITE          229   /* Windows Write */
#define FILE_WORD_WINDOWS_2X        230   /* Word for Windows 2.X */
#define FILE_WORD_WINDOWS_60        231   /* Word for Windows 6.0 */
#define FILE_WORD_OFFICE97          232   /* Word Office 97 */
#define FILE_WORDPERFECT_DOS_42     233   /* WordPerfect 4.2 (DOS) */
#define FILE_WORDPERFECT_WINDOWS    234   /* WordPerfect (Windows) */
#define FILE_WORDPERFECT_WINDOWS_60 235   /* WordPerfect 6.0 (Windows) */
#define FILE_WORDPERFECT_WINDOWS_61 236   /* WordPerfect 6.1 (Windows) */
#define FILE_WORDPERFECT_WINDOWS_7X 237   /* WordPerfect 7X (Windows) */
#define FILE_WORDSTAR_WINDOWS_1X    238   /* WordStar 1.X (Windows) */
#define FILE_WORKS                  239   /* Works */
#define FILE_XDOC                   240   /* Xerox XDOC */

/* Other formats */
#define FILE_MOV                    241   /* Apple QuickTime */
#define FILE_MIDI                   242   /* MIDI music file */
#define FILE_MPEG1                  243   /* MPEG-1 file */
#define FILE_AU                     244   /* SUN sound file */
#define FILE_AIFF                   245   /* Apple/SGI sound file */
#define FILE_MPEG2                  246   /* MPEG-2 file */


/*
    compression type
*/
#define LEAD                      0 /* LEAD  Proprietary */
#define JFIF                      1 /* JPEG  4:4:4 */
#define JTIF                      2 /* JPEG  4:4:4 */
#define LEAD1JFIF                 3 /* JPEG  4:1:1 */
#define LEAD1JTIF                 4 /* JPEG  4:1:1 */
#define LEAD2JFIF                 5 /* JPEG  4:2:2 */
#define LEAD2JTIF                 6 /* JPEG  4:2:2 */

#define LEAD_0                    0 /* LEAD 1 bit, lossless compression */
#define LEAD_1                    1 /* LEAD 1 bit, excellent compression */

#define TIFF_CCITT                3 /* TIFF  CCITT */
#define TIFF_CCITTG3_FAX1D        4 /* CCITT Group3 one dimensional */
#define TIFF_CCITTG3_FAX2D        5 /* CCITT Group3 two dimensional */
#define TIFF_CCITTG4_FAX          6 /* CCITT Group4 two dimensional */

/*
   preset Q factors (for LEAD CMP only)
*/
#define PQ1           -1        /* Perfect quality option 1 */
#define PQ2           -2        /* Perfect quality option 2 */
#define QFS           -3        /* Quality far more important than size */
#define QMS           -4        /* Quality more important than size */
#define QS            -5        /* Quality and size are equally important */
#define SQS           -6        /* Size more important than quality -Sharp */
#define SQT           -7        /* Size more important than quality - Less
                                   Tilling */
#define MCQ           -8        /* Max Compression, keeping quality as good
                                   as possible */
#define MC            -9        /* Max compression */

/*
   flags for L_*Comment
   the following indicate the associated strings are nul terminated
*/
#define CMNT_SZARTIST               0  /* Person who created image. */
#define CMNT_SZCOPYRIGHT            1  /* Copyright notice. */
#define CMNT_SZDATETIME             2  /* "YYYY:MM:DD HH:MM:SS" format */
#define CMNT_SZDESC                 3  /* Description of image */
#define CMNT_SZHOSTCOMP             4  /* Computer/OP System in use. */
#define CMNT_SZMAKE                 5  /* Manufacturer of Equip. used to
                                          generate the image. */
#define CMNT_SZMODEL                6  /* Model Name/Number of Equip. ... */
#define CMNT_SZNAMEOFDOC            7  /* Doc name image was scanned from. */
#define CMNT_SZNAMEOFPAGE           8  /* Page name image was scanned from. */
#define CMNT_SZSOFTWARE             9  /* Name & Version of Software Packages
                                          used to gen the image. */
#define CMNT_SZPATIENTNAME          10 /* Patient name (DICOM) */
#define CMNT_SZPATIENTID            11 /* Patient ID (DICOM) */
#define CMNT_SZPATIENTBIRTHDATE     12 /* Patient birthdate (DICOM) */
#define CMNT_SZPATIENTSEX           13 /* Patient sex (DICOM) */
#define CMNT_SZSTUDYINSTANCE        14 /* Study instance ID (DICOM) */
#define CMNT_SZSTUDYDATE            15 /* Study date (DICOM) */
#define CMNT_SZSTUDYTIME            16 /* Study time (DICOM) */
#define CMNT_SZSTUDYREFERRINGPHYSICIAN 17 /* Referring physician (DICOM) */
#define CMNT_SZSERIESMODALITY       18 /* Series modality (DICOM) */
#define CMNT_SZSERIESID             19 /* Series ID (DICOM) */
#define CMNT_SZSERIESNUMBER         20 /* Series number (DICOM) */

/* Exif 1.0 and 1.1 comments */
#define CMNT_EXIFVERSION            21 /* Exif version */
#define CMNT_SZDATETIMEORIGINAL     22 /* Date and time the original image image is captured (Exif) */
#define CMNT_SZDATETIMEDIGITIZED    23 /* Date and time the file is generated (Exif) */
#define CMNT_SHUTTERSPEEDVALUE      24 /* Shutter speed (Exif) */
#define CMNT_APERTURE               25 /* Aperture value (Exif) */
#define CMNT_BRIGHTNESS             26 /* Brightness value (Exif) */
#define CMNT_EXPOSUREBIAS           27 /* Exposure bias (Exif) */
#define CMNT_MAXAPERTURE            28 /* Minimum lens f-number (Exif) */
#define CMNT_SUBJECTDISTANCE        29 /* Distance from lens to subject (m) (Exif) */
#define CMNT_METERINGMODE           30 /* Photometry mode (Exif) */
#define CMNT_LIGHTSOURCE            31 /* Light source (Exif) */ 
#define CMNT_FLASH                  32 /* Flash On/Off (Exif) */ 
#define CMNT_FOCALLENGTH            33 /* Focal length (Exif) */ 
#define CMNT_EXPOSURETIME           34 /* Exposure  (Exif) */
#define CMNT_FNUMBER                35 /* F-numnber (Exif) */
#define CMNT_MAKERNOTE              36 /* Maker note (Exif) */
#define CMNT_USERCOMMENT            37 /* User comment (Exif) */
#define CMNT_SZSUBSECTIME           38 /* Date Time subsec (Exif) */
#define CMNT_SZSUBSECTIMEORIGINAL   39 /* Date Time original subsec (Exif) */
#define CMNT_SZSUBSECTIMEDIGITIZED  40 /* Date Time digitized subsec (Exif) */

/* Exif 2.0 comments - comments introduced since Exif 1.1 */
#define CMNT_SUPPORTEDFLASHPIXVERSION  158   /* Supported FlashPix version (Exif) */
#define CMNT_COLORSPACE                159   /* Color space (Exif) */
#define CMNT_EXPOSUREPROGRAM           160   /* Exposure program (Exif) */
#define CMNT_SZSPECTRALSENSITIVITY     161   /* Spectral sensitivity (Exif) */
#define CMNT_ISOSPEEDRATINGS           162   /* ISO speed ratings (Exif) */
#define CMNT_OPTOELECTRICCOEFFICIENT   163   /* Optoelectric coefficient (Exif) */
#define CMNT_SZRELATEDSOUNDFILE        164   /* Related audio file (Exif) */
#define CMNT_FLASHENERGY               165   /* Flash energy (Exif) */
#define CMNT_SPATIALFREQUENCYRESPONSE  166   /* Spatial frequency response (Exif) */
#define CMNT_FOCALPLANEXRESOLUTION     167   /* Focal plane X Resolution (Exif) */
#define CMNT_FOCALPLANEYRESOLUTION     168   /* Focal plane Y Resolution (Exif) */
#define CMNT_SUBJECTLOCATION           169   /* Subject location (Exif) */
#define CMNT_EXPOSUREINDEX             170   /* Exposure index (Exif) */
#define CMNT_SENSINGMETHOD             171   /* Sensing method (Exif) */
#define CMNT_FILESOURCE                172   /* File source (Exif) */
#define CMNT_SCENETYPE                 173   /* Scene type (Exif) */
#define CMNT_CFAPATTERN                174   /* CFA Pattern (Exif) */

/* Exif 1.1 GPS comments */
#define CMNT_GPSVERSIONID           41
#define CMNT_GPSLATITUDEREF         42
#define CMNT_GPSLATITUDE            43
#define CMNT_GPSLONGITUDEREF        44
#define CMNT_GPSLONGITUDE           45
#define CMNT_GPSALTITUDEREF         46
#define CMNT_GPSALTITUDE            47
#define CMNT_GPSTIMESTAMP           48
#define CMNT_GPSSATELLITES          49
#define CMNT_GPSSTATUS              50
#define CMNT_GPSMEASUREMODE         51
#define CMNT_GPSDOP                 52
#define CMNT_GPSSPEEDREF            53
#define CMNT_GPSSPEED               54
#define CMNT_GPSTRACKREF            55
#define CMNT_GPSTRACK               56
#define CMNT_GPSIMGDIRECTIONREF     57
#define CMNT_GPSIMGDIRECTION        58
#define CMNT_GPSMAPDATUM            59
#define CMNT_GPSDESTLATITUDEREF     60
#define CMNT_GPSDESTLATITUDE        61 
#define CMNT_GPSDESTLONGITUDEREF    62 
#define CMNT_GPSDESTLONGITUDE       63 
#define CMNT_GPSDESTBEARINGREF      64 
#define CMNT_GPSDESTBEARING         65 
#define CMNT_GPSDESTDISTANCEREF     66 
#define CMNT_GPSDESTDISTANCE        67 


#define CMNT_FPXSUMMARYINFORMATION                    0x8001
#define CMNT_FPXTITLE                                     68
#define CMNT_FPXSUBJECT                                   69
#define CMNT_FPXAUTHOR                                    70
#define CMNT_FPXKEYWORDS                                  71
#define CMNT_FPXCOMMENTS                                  72
#define CMNT_FPXOLETEMPLATE                               73
#define CMNT_FPXLASTAUTHOR                                74
#define CMNT_FPXREVNUMBER                                 75
#define CMNT_FPXEDITTIME                                  76
#define CMNT_FPXLASTPRINTED                               77
#define CMNT_FPXCREATEDTM                                 78
#define CMNT_FPXLASTSAVEDTM                               79
#define CMNT_FPXPAGECOUNT                                 80
#define CMNT_FPXWORDCOUNT                                 81
#define CMNT_FPXCHARCOUNT                                 82
#define CMNT_FPXTHUMBNAIL                                 83
#define CMNT_FPXAPPNAME                                   84
#define CMNT_FPXSECURITY                                  85
#define CMNT_FPXSUMMARYINFORMATION1                   CMNT_FPXTITLE
#define CMNT_FPXSUMMARYINFORMATION2                   CMNT_FPXSECURITY
                                                      
#define CMNT_FPXFILESOURCEGROUP                       0x8002
#define CMNT_FPXFILESOURCE                                86
#define CMNT_FPXSCENETYPE                                 87
#define CMNT_FPXCREATIONPATH                              88
#define CMNT_FPXNAMEMANRELEASE                            89
#define CMNT_FPXUSERDEFINEDID                             90
#define CMNT_FPXORIGINALSHARPNESSAPPROXIMATION            91
#define CMNT_FPXFILESOURCEGROUP1                      CMNT_FPXFILESOURCE
#define CMNT_FPXFILESOURCEGROUP2                      CMNT_FPXORIGINALSHARPNESSAPPROXIMATION

#define CMNT_FPXINTELLECTUALPROPERTYGROUP             0x8004
#define CMNT_FPXCOPYRIGHT                                 92
#define CMNT_FPXLEGALBROKERFORORIGIMAGE                   93
#define CMNT_FPXLEGALBROKERFORDIGITALIMAGE                94
#define CMNT_FPXAUTHORSHIP                                95
#define CMNT_FPXINTELLECTUALPROPNOTES                     96
#define CMNT_FPXINTELLECTUALPROPERTYGROUP1            CMNT_FPXCOPYRIGHT
#define CMNT_FPXINTELLECTUALPROPERTYGROUP2            CMNT_FPXINTELLECTUALPROPNOTES

#define CMNT_FPXCONTENTDESCRIPTIONGROUP               0x8008
#define CMNT_FPXTESTTARGETINTHEIMAGE                      97
#define CMNT_FPXGROUPCAPTION                              98
#define CMNT_FPXCAPTIONTEXT                               99
#define CMNT_FPXPEOPLEINTHEIMAGE                         100
#define CMNT_FPXTHINGSINIMAGE                            101
#define CMNT_FPXDATEOFORIGINALIMAGE                      102
#define CMNT_FPXEVENTSINTHEIMAGE                         103
#define CMNT_FPXPLACESINTHE                              104
#define CMNT_FPXCONTENTDESCRIPTIONNOTES                  105
#define CMNT_FPXCONTENTDESCRIPTIONGROUP1              CMNT_FPXTESTTARGETINTHEIMAGE
#define CMNT_FPXCONTENTDESCRIPTIONGROUP2              CMNT_FPXCONTENTDESCRIPTIONNOTES

#define CMNT_FPXCAMERAINFORMATIONGROUP                0x8010
#define CMNT_FPXCAMERAMANUFACTURERNAME                   106
#define CMNT_FPXCAMERAMODELNAME                          107
#define CMNT_FPXCAMERASERIALNUMBER                       108
#define CMNT_FPXCAMERAINFORMATIONGROUP1               CMNT_FPXCAMERAMANUFACTURERNAME
#define CMNT_FPXCAMERAINFORMATIONGROUP2               CMNT_FPXCAMERASERIALNUMBER

#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP         0x8020
#define CMNT_FPXCAPTUREDATE                              109
#define CMNT_FPXEXPOSURETIME                             110
#define CMNT_FPXFNUMBER                                  111
#define CMNT_FPXEXPOSUREPROGRAM                          112
#define CMNT_FPXBRIGHTNESSVALUE                          113
#define CMNT_FPXEXPOSUREBIASVALUE                        114
#define CMNT_FPXSUBJECTDISTANCE                          115
#define CMNT_FPXMETERINGMODE                             116
#define CMNT_FPXSCENEILLUMINANT                          117
#define CMNT_FPXFOCALLENGTH                              118
#define CMNT_FPXMAXIMUMAPERATUREVALUE                    119
#define CMNT_FPXFLASH                                    120
#define CMNT_FPXFLASHENERGY                              121
#define CMNT_FPXFLASHRETURN                              122
#define CMNT_FPXBACKLIGHT                                123
#define CMNT_FPXSUBJECTLOCATION                          124
#define CMNT_FPXEXPOSUREINDEX                            125
#define CMNT_FPXSPECIALEFFECTSOPTICALFILTER              126
#define CMNT_FPXPERPICTURENOTES                          127
#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP1        CMNT_FPXCAPTUREDATE
#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP2        CMNT_FPXPERPICTURENOTES

#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP    0x8040
#define CMNT_FPXSENSINGMETHOD                            128
#define CMNT_FPXFOCALPLANEXRESOLUTION                    129
#define CMNT_FPXFOCALPLANEYRESOLUTION                    130
#define CMNT_FPXFOCALPLANERESOLUTIONUNIT                 131
#define CMNT_FPXSPACIALFREQUENCY                         132
#define CMNT_FPXCFAPATTERN                               133
#define CMNT_FPXSPECTRALSENSITIVITY                      134
#define CMNT_FPXISOSPEEDRATINGS                          135
#define CMNT_FPXOECF                                     136
#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP1   CMNT_FPXSENSINGMETHOD
#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP2   CMNT_FPXOECF

#define CMNT_FPXFILMDESCRIPTIONGROUP                  0x8080
#define CMNT_FPXFILMBRAND                                137
#define CMNT_FPXFILMCATEGORY                             138
#define CMNT_FPXFILMSIZEX                                139
#define CMNT_FPXFILMSIZEY                                140
#define CMNT_FPXFILMSIZEUNIT                             141
#define CMNT_FPXFILMROLLNUMBER                           142
#define CMNT_FPXFILMFRAMENUMBER                          143
#define CMNT_FPXFILMDESCRIPTIONGROUP1                 CMNT_FPXFILMBRAND
#define CMNT_FPXFILMDESCRIPTIONGROUP2                 CMNT_FPXFILMFRAMENUMBER

#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP  0x8100
#define CMNT_FPXORIGINALSCANNEDIMAGESIZE                 144
#define CMNT_FPXORIGINALDOCUMENTSIZE                     145
#define CMNT_FPXORIGINALMEDIUM                           146
#define CMNT_FPXTYPEOFREFLECTIONORIGINAL                 147
#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP1 CMNT_FPXORIGINALSCANNEDIMAGESIZE
#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP2 CMNT_FPXTYPEOFREFLECTIONORIGINAL

#define CMNT_FPXSCANDEVICEPROPERTYGROUP               0x8200
#define CMNT_FPXSCANNERMANUFACTURERNAME                  148
#define CMNT_FPXSCANNERMODELNAME                         149
#define CMNT_FPXSCANNERSERIALNUMBER                      150
#define CMNT_FPXSCANSOFTWARE                             151
#define CMNT_FPXSCANSOFTWAREREVISIONDATE                 152
#define CMNT_FPXSERVICEBUREAUORGNAME                     153
#define CMNT_FPXSCANOPERATORID                           154
#define CMNT_FPXSCANDATE                                 155
#define CMNT_FPXLASTMODIFIEDDATE                         156
#define CMNT_FPXSCANNERPIXELSIZE                         157
#define CMNT_FPXSCANDEVICEPROPERTYGROUP1              CMNT_FPXSCANNERMANUFACTURERNAME
#define CMNT_FPXSCANDEVICEPROPERTYGROUP2              CMNT_FPXSCANNERPIXELSIZE

#define CMNT_SZTITLE                                     175 /* Title or caption for image */
#define CMNT_SZDISCLAIMER                                176 /* Legal Disclaimer */
#define CMNT_SZWARNING                                   177 /* Warning of nature of content */
#define CMNT_MISC                                        178 /* Miscellaneous comment */

#define CMNT_LAST                   178 /* Last defined number for comments. */

#define CMNT_ALL                 0xFFFF

#define TAG_BYTE                    1
#define TAG_ASCII                   2
#define TAG_SBYTE                   6
#define TAG_UNDEFINED               7
#define TAG_SHORT                   3
#define TAG_SSHORT                  8
#define TAG_LONG                    4
#define TAG_SLONG                   9
#define TAG_FLOAT                   11
#define TAG_RATIONAL                5
#define TAG_SRATIONAL               10
#define TAG_DOUBLE                  12

#define GET_TAG_SIZE(pTagSize,uType,ulCount) \
   switch(uType)                    \
   {                                \
   case TAG_BYTE:                   \
   case TAG_ASCII:                  \
   case TAG_SBYTE:                  \
   case TAG_UNDEFINED:              \
      *(pTagSize) = 1L * (ulCount); \
      break;                        \
   case TAG_SHORT:                  \
   case TAG_SSHORT:                 \
      *(pTagSize) = 2L * (ulCount); \
      break;                        \
   case TAG_LONG:                   \
   case TAG_SLONG:                  \
   case TAG_FLOAT:                  \
      *(pTagSize) = 4L * (ulCount); \
      break;                        \
   case TAG_RATIONAL:               \
   case TAG_SRATIONAL:              \
   case TAG_DOUBLE:                 \
      *(pTagSize) = 8L * (ulCount); \
      break;                        \
   default:                         \
      *(pTagSize) = 0;              \
      break;                        \
   }

/*
    flags for L_SETLOADINFOCALLBACK
*/
#define LOADINFO_TOPLEFT      0x00000001 /* Image has TOP_LEFT View
                                            Perspective */
#define LOADINFO_ORDERRGB     0x00000002 /* Image has ORDERRGB Color Order */
#define LOADINFO_WHITEONBLACK 0x00000004 /* Image is white-on-black */
#define LOADINFO_LSB          0x00000008 /* Image is Least Significant Bit
                                            first fill order */
#define LOADINFO_TOPLEFT90    0x00000010 /* Image has TOP_LEFT90 View 
                                            Perspective */
#define LOADINFO_TOPLEFT270   0x00000020 /* Image has TOP_LEFT270 View 
                                            Perspective */
#define LOADINFO_REVERSE      0x00000040 /* Reverse (mirror) each line */

#define LOADINFO_TOPLEFT180   0x00000080 /* Image has TOP_LEFT180 View 
                                            Perspective */
#define LOADINFO_BOTTOMLEFT90    0x00000100 /* Image has BOTTOM_LEFT90 View 
                                            Perspective */
#define LOADINFO_BOTTOMLEFT180   0x00000200 /* Image has BOTTOM_LEFT180 View 
                                            Perspective */
#define LOADINFO_BOTTOMLEFT270   0x00000400 /* Image has BOTTOM_LEFT270 View 
                                            Perspective */
#define LOADINFO_PAD4            0x00000800 /* Each line is padded to a multiple of 4 bytes (raw data only) */

#define LOADINFO_PALETTE         0x00001000 /* For RAW data of 8 bpp or less, a palette is supplied in rgbQuad of LOADINFO*/

#define LOADINFO_ORDERGRAY       0x00004000 /* Image is grayscale */
#define LOADINFO_MOTOROLAORDER   0x00008000 /* Image bytes are in Motorola byte order (valid only for 16, 48 and 64-bit) */
#define LOADINFO_BITFIELDS       0x00002000 /* For RAW BITFIELDS -- 3 color masks are specified in rgbColorMask of LOADINFO */

/*
   LOADINFO structure
*/
typedef struct _OLDLOADINFO
{
   L_INT Format;                /* File format: FILE_PCX, ... */
   L_INT Width;                 /* Image width */
   L_INT Height;                /* Image height */
   L_INT BitsPerPixel;          /* Bits per pixel */
   L_INT XResolution;           /* X resolution (DPI) */
   L_INT YResolution;           /* Y resolution (DPI) */
   L_UINT32 Offset;             /* Data offset */
   L_UINT32 Flags;              /* Special flags */
} OLDLOADINFO, L_FAR * pOLDLOADINFO;


typedef struct _LOADINFO
{
   L_INT Format;                /* File format: FILE_PCX, ... */
   L_INT Width;                 /* Image width */
   L_INT Height;                /* Image height */
   L_INT BitsPerPixel;          /* Bits per pixel */
   L_INT XResolution;           /* X resolution (DPI) */
   L_INT YResolution;           /* Y resolution (DPI) */
   L_UINT32 Offset;             /* Data offset */
   L_UINT32 Flags;              /* Special flags */

  //for RAW file filter */
   RGBQUAD        rgbQuad[256]; 

   //for RAW BITFIELDS - contains R,G,B color masks */
   L_UINT32       rgbColorMask[3];    

   // for RAW_PACKBITS */
   L_UINT32       uStripSize;             // size of strip after compression
   L_INT          nPhotoInt;              // TIFF tag  0-6  TIFF tag
   L_INT          nPlanarConfig;          // TIFF tag  1 = Chunky,  2 = Planar format
} LOADINFO, L_FAR * pLOADINFO;


/*
   flags that make up FILEINFO.Flags
*/
#define FILEINFO_INTERLACED            0x00000001
#define FILEINFO_PROGRESSIVE           0x00000002  /* progressive JPEG file */
#define FILEINFO_HAS_STAMP             0x00000004
#define FILEINFO_HAS_GLOBALBACKGROUND  0x00000008
#define FILEINFO_HAS_GLOBALPALETTE     0x00000010
#define FILEINFO_HAS_GLOBALLOOP        0x00000020
#define FILEINFO_COMPRESSED            0x00000040  /* the image can be loaded compressed */
#define FILEINFO_NOPALETTE             0x00000080  /* Grayscale TIF without a palette */
#define FILEINFO_ROTATED               0x00000100  /* Image with an extended ViewPerspective */
#define FILEINFO_SIGNED                0x00000200  /* Image with signed values for pixels */
#define FILEINFO_LOSSLESSJPEG          0x00000400  /* Internal flag, indicating that the JPEG file uses the lossless compression.
                                                      This flag will probably be removed in future versions */
#define FILEINFO_HAS_ALPHA             0x00000800  /* The file has alpha channel information */
#define FILEINFO_FORMATVALID           0x00001000  /* Only the format type is valid */
#define FILEINFO_INFOVALID             0x00002000  /* The whole FILEINFO structure is valid */
#define FILEINFO_LINK                  0x00004000  /* The file is a Windows 9x/NT link */
#define FILEINFO_IFDVALID              0x00008000  /* The IFD field is valid */


/*
   Flags for L_FileInfo (not to make up FILEINFO.Flags!)
*/

#define FILEINFO_TOTALPAGES            0x00000001  /* fill in FILEINFO.TotalPages */

/*
   structure used by L_FileInfo
*/
typedef struct struct_file_information
{
   L_INT    Format;              /* File format: FILE_PCX, ... */
   L_CHAR   Name[16];            /* File name, including the Ext */
   L_INT    Width;               /* Image width */
   L_INT    Height;              /* Image height */
   L_INT    BitsPerPixel;        /* Bits per pixel */
   L_INT32  SizeDisk;            /* Size of file on disk */
   L_INT32  SizeMem;             /* Size of image in memory */
   L_CHAR   Compression[20];     /* Compression method name */
   L_INT    ViewPerspective;     /* Image view prespective */
   L_INT    Order;               /* RGB order */
   L_INT    PageNumber;          /* Page number */
   L_INT    TotalPages;          /* Total number of pages present in the file */
   L_INT    XResolution;
   L_INT    YResolution;
   L_UINT32 Flags;               /* identifies file subtypes: progressive, interlaced */
   L_UINT   GlobalLoop;          /* Global animation loop count 0 = infinity */
   L_INT    GlobalWidth;         /* Global width */
   L_INT    GlobalHeight;        /* Global height */
   COLORREF GlobalBackground;    /* Global background color (see Flags) */
   RGBQUAD  GlobalPalette[256];  /* Global palette (see Flags) */
   L_UINT32 IFD;                 /* IFD offset (for TIF files only) */
} FILEINFO, L_FAR * pFILEINFO;

/* helper macros for dealing with rotated bitmap info view perspectives */
#define INFOROTATED(pInfo) ((pInfo)->ViewPerspective == TOP_LEFT90 || (pInfo)->ViewPerspective == TOP_LEFT270 || (pInfo)->ViewPerspective == BOTTOM_LEFT90 || (pInfo)->ViewPerspective == BOTTOM_LEFT270)
#define INFOBASIC(pInfo) ((pInfo)->ViewPerspective == TOP_LEFT || (pInfo)->ViewPerspective == BOTTOM_LEFT)
#define INFOWIDTH(pInfo) (INFOROTATED(pInfo)?(pInfo)->Height:(pInfo)->Width)
#define INFOHEIGHT(pInfo) (INFOROTATED(pInfo)?(pInfo)->Width:(pInfo)->Height)

/*
   photo cd
*/
#define L_PCD_BASE_OVER_64   0  /* 64 x   96 */
#define L_PCD_BASE_OVER_16   1  /* 128 x  192 */
#define L_PCD_BASE_OVER_4    2  /* 256 x  384 */
#define L_PCD_BASE           3  /* 512 x  768 */
#define L_PCD_4BASE          4  /* 1024 x 1536 */
#define L_PCD_16BASE         5  /* 2048 x 3072 */

typedef struct pcdinfo
{
   L_INT resolution[6];
} PCDINFO, L_FAR * pPCDINFO;

/* structure for L_CreateThumbnailFromFile */
typedef struct thumboptions
{
   L_INT          nWidth;
   L_INT          nHeight;
   L_INT          nBits;
   L_UINT32       uCRFlags;
   L_BOOL         bMaintainAspect;
   L_BOOL         bForceSize;
   COLORREF       crBackColor;
   L_BOOL         bLoadStamp;
   L_BOOL         bResample;
} THUMBOPTIONS, L_FAR *pTHUMBOPTIONS;

/* New flags for the LOADFILEOPTION structure */
#define ELO_REVERSEBITS          0x00000001
#define ELO_GLOBALBACKGROUND     0x00000002
#define ELO_GLOBALPALETTE        0x00000004
#define ELO_GLOBALLOOP           0x00000008
#define ELO_ROTATED              0x00000010 /* Load files with extended ViewPerspective - do not rotate them */
#define ELO_IGNOREVIEWTRANSFORMS 0x00000020 /* Load the image without the viewing transformations */
#define ELO_IGNORECOLORTRANSFORMS 0x00000040 /* Load the image without the color transformations */
#define ELO_SIGNED               0x00000080 /* Load images with signed pixels without conversion */
#define ELO_DISABLEMMX           0x00000100 /* Do not use MMX optimized code */
#define ELO_DISABLEP3            0x00000200 /* Do not use P3-specific optimized code */
#define ELO_USEIFD               0x00000400 /* Use the IFD offset */
#define ELO_FORCECIELAB          0x00000800 /* Use CIE LAB*/

/* 
   Flags for the SAVEFILEOPTION structure 
*/
#define ESO_REVERSEBITS          0x00000001
#define ESO_NOSUBFILETYPE        0x00000002
#define ESO_GLOBALBACKGROUND     0x00000004
#define ESO_GLOBALPALETTE        0x00000008
#define ESO_INTERLACED           0x00000010
#define ESO_GLOBALLOOP           0x00000020
#define ESO_NOPALETTE            0x00000040 /* save grayscale TIF without a palette */
#define ESO_SAVEWITHSTAMP        0x00000080 /* save a stamp if format supports it */
#define ESO_FIXEDPALETTESTAMP    0x00000100 /* save fixed palette stamps */
#define ESO_YCCSTAMP             0x00000200 /* save YCbCr stamps */
#define ESO_REPLACEPAGE          0x00000400 /* Replace the page specified by PageNumber */
#define ESO_INSERTPAGE           0x00000800 /* Insert image before the page specified by PageNumber */
#define ESO_JPEGSTAMP            0x00001000 /* save JPEG compressed stamps */
#define ESO_DISABLEMMX           0x00002000 /* Do not use MMX optimized code */
#define ESO_SAVEOLDJTIF          0x00004000 /* Write old style JTIF files */
#define ESO_NOPAGENUMBER         0x00008000 /* Do not save/update the PageNumber tag */
#define ESO_DISABLEP3            0x00010000 /* Do not use P3-specific optimized code */
#define ESO_USEIFD               0x00020000 /* Use the IFD offset */
#define ESO_MOTOROLAORDER        0x00040000 /* When possible, save files in Motorola byte order */
#define ESO_WITHOUTTIMESTAMP     0x00080000 /* save without time stamp*/ 

#define ESO_PDF_TEXT_ENCODING_NONE          0x00000000  /* PDF, no text encoding */
#define ESO_PDF_TEXT_ENCODING_ASCII_BASE85  0x00100000  /* PDF, ASCII BASE85 text encoding */
#define ESO_PDF_TEXT_ENCODING_ASCII_HEX     0x00200000  /* PDF, ASCII HEX text encoding */

#define ESO_PDF_TEXT_ENCODING_MASK           0x00300000  /* mask of the flags used for PDF text encoding */
#define ESO_PAD4                             0x00400000  /* Each line is padded to a multiple of 4 bytes (raw data only) */
#define ESO_PLT_BEZIER_CURVES                0x00800000  /* PLT filter should save bezier curves */
#define ESO_PDF_SAVE_USE_BITMAP_DPI          0x01000000  /* Use bitmap  DPI in calculating page dimensions when saving a PDF file . */





/*
   Values that can be used for LOADFILEOPTION.Passes. These values will
   generate default Progressive JPEG/CMP files (usually 10 passes/file) on 
   save and specifies how many passes to generate while decoding 
   Progressive JPEG/CMP files.
*/
#define CALLBACK_ONCE                0 /* callback just once at the end */
#define CALLBACK_WHEN_MEANINGFUL    -1 /* callback with only significant scans */
#define CALLBACK_ALWAYS             -2 /* callback with all scans */

typedef struct _LOADFILEOPTION
{
   L_UINT32 lSize;                     /* the size of the LOADFILEOPTION - use sizeof(LOADFILEOPTION) */
   L_INT    XResolution;
   L_INT    YResolution;
   L_UINT32 Flags;
   L_INT    Passes;
   L_INT    PageNumber;
   L_UINT   GlobalLoop;
   L_UINT32 IFD;
} LOADFILEOPTION, L_FAR * pLOADFILEOPTION;

typedef struct _SAVEFILEOPTION
{
   L_UINT32 lSize;                     /* the size of the SAVEFILEOPTION - use sizeof(SAVEFILEOPTION) */
   L_INT    Reserved1;
   L_INT    Reserved2;
   L_UINT32 Flags;
   L_INT    Passes;
   L_INT    PageNumber;
   L_INT    GlobalWidth;
   L_INT    GlobalHeight;
   L_UINT   GlobalLoop;
   COLORREF GlobalBackground;
   RGBQUAD  GlobalPalette[256];
   L_UINT   StampWidth;
   L_UINT   StampHeight;
   L_UINT   StampBits;

   // MPEG Specific Options -- Not set by user
   // These options can only be set with the Multimedia Toolkit

   L_BOOL   Constrained;
   L_BOOL   FieldPic;
   L_INT    FrameRate;
   L_INT    FramesGOP;
   L_INT    PelAspectRatio;
   L_INT    ProfileID;
   L_INT    LevelID;
   L_INT    ChromaFormat;
   L_INT    VideoFormat;
   L_INT    IntraDcPrec;
   L_INT32  BitRate; 
   L_CHAR   UserInfo[255];    // user defined data to be put in mpeg stream
   L_UINT32 IFD;
} SAVEFILEOPTION, L_FAR * pSAVEFILEOPTION;


typedef struct _SAVECUSTOMFILEOPTION
{
   L_UINT32 lSize                      ;/* Size of the SAVECUSTOMFILEOPTION */
   L_UINT32 uFlags                     ;/* Custom save options flags */
   L_INT 	nTileWidth                 ;/* Width of a tile */
   L_INT 	nTileHeight                ;/* Height of a tile*/
   L_INT 	nCompressionTag            ;/* Compression Tag*/
	L_INT 	nPlanarConfiguration       ;/* Planar Configuration */
	L_INT	   nPhotoMetricInterpretation ;/* Photo Metric Interpretation*/
} SAVECUSTOMFILEOPTION, L_FAR * pSAVECUSTOMFILEOPTION;



/*
   flags for the FILEREADCALLBACK function
*/

#define FILEREAD_FIRSTPASS    0x0001
#define FILEREAD_LASTPASS     0x0002
#define FILEREAD_FIRSTROW     0x0004
#define FILEREAD_LASTROW      0x0008
#define FILEREAD_COMPRESSED   0x0010
#define FILEREAD_CLIPVERT     0x0020   // internal flag - data contains only the requested lines
#define FILEREAD_CLIPHORZ     0x0040   // internal flag - data contains only the requested columns

typedef struct _FILECOMMENTS
{
   L_INT           count;
   L_UCHAR L_FAR **pointer;
   L_UINT L_FAR   *size;
} FILECOMMENTS, L_FAR *LPFILECOMMENTS;

/*
   flags for the L_SaveFile and L_SaveFileOffset function
*/

typedef struct _DIMENSION
{
   L_UINT32 nWidth;
   L_UINT32 nHeight;
} DIMENSION, L_FAR*pDIMENSION;

typedef struct _FILETRANSFORMS {
   //AffineMatrix
   L_FLOAT a11, a12, a13, a14, a21, a22, a23, a24, a31, a32, a33, a34, a41, a42, a43, a44;
   //ColorTwistMatrix
   L_FLOAT byy, byc1, byc2, bc1y, bc1c1, bc1c2, bc2y, bc2c1, bc2c2;
   //ContrastAdjustment
   L_FLOAT fContrastAdjustment;
   //FilteringValue (Sharpness Adjustment)
   L_FLOAT fFilteringValue;
} FILETRANSFORMS, L_FAR  *pFILETRANSFORMS;

/*
   L_2DSetViewMode flags
*/
#define L2D_USE_BEST          0
#define L2D_USE_WIDTH_HEIGHT  1
#define L2D_USE_WIDTH         2
#define L2D_USE_HEIGHT        3

/*
   PLT Options
*/
typedef struct _FILEPLTOPTIONS
{
   L_INT             Size;
   L_INT             PenWidth[ 8 ];
   COLORREF          PenColor[ 8 ];
}
FILEPLTOPTIONS, L_FAR *pFILEPLTOPTIONS;


/*
   PDF Options
*/
#define FILEPDFOPTIONS_MAX_PASSWORD_LEN 64
typedef struct _FILEPDFOPTIONS
{
   L_INT    nSize                                        ;/*   The size of this structure */
   L_BOOL   bUseLibFonts                                 ;/*   Specifies whether to use the library installed fonts or system fonts, 
                                                               default is TRUE */
   L_INT    nXResolution                                 ;/*   Horizontal display resolution default is 96 */
   L_INT    nYResolution                                 ;/*   Vertical display resolution default is 96 */
   L_INT    nDisplayDepth                                ;/*   Bits per pixel for resulting bitmap, takes 1,4,8,24 default is 24 */
   L_INT    nTextAlpha                                   ;/*   Font Anti-Aliasing,takes 1(no anti-aliasing), 2 and 4 .Default is 4 */
   L_INT    nGraphicsAlpha                               ;/*   Graphics Anti-Aliasing takes 1,2,4.Default is 1 */
   L_CHAR   szPassword[FILEPDFOPTIONS_MAX_PASSWORD_LEN]  ;/*   Password to be used with encrypted PDF files */   
}
FILEPDFOPTIONS , L_FAR * pFILEPDFOPTIONS;


/*
   callbacks
*/
typedef L_INT (pEXT_CALLBACK COMPBUFFCALLBACK) (pBITMAPHANDLE, L_UCHAR L_FAR *, L_UINT, L_VOID L_FAR *);
typedef L_INT (pEXT_CALLBACK LOADINFOCALLBACK) (L_INT, pLOADINFO, L_VOID L_FAR*);
typedef L_INT (pEXT_CALLBACK FILEREADCALLBACK) (pFILEINFO, pBITMAPHANDLE, L_UCHAR L_FAR *, L_UINT, L_INT, L_INT, L_VOID L_FAR *);
typedef L_INT (pEXT_CALLBACK FILESAVECALLBACK) (pBITMAPHANDLE, L_UCHAR L_FAR *, L_UINT, L_UINT, L_VOID L_FAR *);
typedef L_INT (pEXT_CALLBACK CUSTOMFILESAVECALLBACK)(pBITMAPHANDLE ,L_UCHAR* ,L_UINT, L_UINT *,PRECT , L_VOID *);

#define DECOMPRESS_LSB           (LOADINFO_LSB)
#define DECOMPRESS_PAD4          (LOADINFO_PAD4)
#define DECOMPRESS_PALETTE       (LOADINFO_PALETTE)

#define DECOMPRESS_STRIPS        (0)
#define DECOMPRESS_TILES         (1)


typedef struct _STARTDECOMPRESSDATA
{
   L_INT32          nSize;            // For versioning
   pBITMAPHANDLE    pBitmap;
   L_UINT           uStripsOrTiles;   // DECOMPRESS_STRIPS, DECOMPRESS_TILES
   L_UINT           uFormat;	        // FILE_RAW_RGB; FILE_RAW_RLE4; FILE_RAW_RLE8; FILE_RAW_BITFIELDS; FILE_RAW_PACKBITS
   L_INT            nWidth;           //
   L_INT            nHeight;          //
   L_INT            nBitsPerPixel;    //
   L_INT            nViewPerspective; //
   L_INT            nOrder;           // ORDER_RGB, ORDER_BGR
   L_INT            nXResolution;
   L_INT            nYResolution;
   FILEREADCALLBACK pfnReadCallback;  // optional callback function
   RGBQUAD          Palette[256];     //
   L_UINT32         uFlags;           // DECOMPRESS_LSB, DECOMPRESS_PAD4, DECOMPRESS_WHITEONBLACK, DECOMPRESS_PALETTE
   L_VOID          *pUserData;        //
   
   //Valid for FILE_RAW_BITFIELDS - contains R,G,B color masks */
   L_UINT32         rgbColorMask[3];  
   
   //Valid for FILE_RAW_PACKBITS */
   L_INT            nPhotoInt;              // TIFF tag  0-6  TIFF tag
   L_INT            nPlanarConfig;   // TIFF tag  1 = Chunky,  2 = Planar format

   //Reserved for future use
   L_UINT32         uReserved1;
   
} STARTDECOMPRESSDATA, *pSTARTDECOMPRESSDATA;


typedef struct _DECOMPRESSDATA
{
   L_INT32        nSize;            //for versioning
   L_UCHAR       *pBuffer;          //pointer to raw data
   L_INT          nWidth;           //Width of uncompressed strip/tile
   L_INT          nHeight;          //Height of uncompressed strip/tile
   L_UINT32       uOffset;          //Offset of strip relative to buffer
   L_INT          nBufferSize;      //Size of strip after compression
   L_INT          nRow;             //Row offset of tile
   L_INT          nCol;             //Col offset of tile
   L_INT          nReserved1;       //Reserved for future use
} DECOMPRESSDATA, *pDECOMPRESSDATA;

/*
   function prototypes
*/

L_INT EXT_FUNCTION L_StartDecompressBuffer(
                                           HGLOBAL *phDecompress,
                                           pSTARTDECOMPRESSDATA pStartCompressData
                                           );

L_INT EXT_FUNCTION L_StopDecompressBuffer(HGLOBAL hDecompress);


L_INT EXT_FUNCTION L_DecompressBuffer(
                                      HGLOBAL hDecompress,
                                      pDECOMPRESSDATA pDecompressData
                                      );

L_INT EXT_FUNCTION L_CompressBuffer (L_UCHAR L_FAR * pBuffer);

L_INT EXT_FUNCTION L_DeletePage(L_CHAR L_FAR*pszFile, 
                                L_INT nPage,
                                L_UINT uFlags,
                                pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_EndCompressBuffer (L_VOID);

L_INT EXT_FUNCTION L_ReadLoadResolutions (L_CHAR L_FAR *pszFile, 
                                     pDIMENSION pDimensions, 
                                     L_INT L_FAR *pDimensionCount,
                                     pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_ReadFileTransforms (L_CHAR L_FAR *pszFile,
                                         pFILETRANSFORMS pTransforms,
                                         pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_WriteFileTransforms (L_CHAR L_FAR *pszFile,
                                          pFILETRANSFORMS pTransforms,
                                          L_INT nFlags,
                                          pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_FeedLoad (HGLOBAL hLoad,
                                L_UCHAR L_FAR * pBuffer,
                                L_UINT32 dwBufferSize);

L_INT EXT_FUNCTION L_FileConvert (L_CHAR L_FAR * pszFileSrc,
                                   L_CHAR L_FAR * pszFileDst,
                                   L_INT nType,
                                   L_INT nWidth,
                                   L_INT nHeight,
                                   L_INT nBitsPerPixel,
                                   L_INT nQFactor,
                                   pLOADFILEOPTION pLoadOptions,
                                   pSAVEFILEOPTION pSaveOptions,
                                   pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_FileInfo (L_CHAR L_FAR * pszFile,
                                pFILEINFO pFileInfo,
                                L_UINT uFlags,
                                pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_FileInfoMemory (L_CHAR L_FAR * pBuffer,
                                      pFILEINFO pFileInfo,
                                      L_INT32 nBufferSize,
                                      L_UINT uFlags,
                                      pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_GetComment (L_UINT uType,
                                  L_UCHAR L_FAR * pComment,
                                  L_UINT uLength);

L_INT EXT_FUNCTION L_GetLoadResolution (L_INT nFormat,
                                   L_UINT32 L_FAR *pWidth,
                                   L_UINT32 L_FAR *pHeight,
                                   pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_GetFileCommentSize (L_CHAR L_FAR *pszFile,
                                       L_UINT uType,
                                       L_UINT L_FAR *uLength,
                                       pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_GetPCDResolution (L_CHAR L_FAR * pszFile,
                                        pPCDINFO pPCDInfo);

L_INT EXT_FUNCTION L_GetWMFResolution (L_INT L_FAR* lpResolution);

L_INT32 EXT_FUNCTION L_GetTag (L_UINT16 uTag,
                              L_UINT16 L_FAR*pType,
                              L_UINT32 L_FAR*pCount,
                              L_VOID L_FAR*pData);

L_INT EXT_FUNCTION L_IgnoreFilters (L_CHAR L_FAR*pszFilters);

L_INT EXT_FUNCTION L_LoadBitmap (L_CHAR L_FAR * pszFile,
                                  pBITMAPHANDLE pBitmap,
                                  L_INT nBitsPerPixel,
                                  L_INT nOrder,
                                  pLOADFILEOPTION pLoadOptions,
                                  pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadBitmapList (LPSTR lpszFile,
                                     pHBITMAPLIST phList,
                                     L_INT nBitsTo,
                                     L_INT nColorOrder,
                                     pLOADFILEOPTION pLoadOptions,
                                     pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadBitmapMemory (L_CHAR L_FAR * pBuffer,
                                        pBITMAPHANDLE pBitmap,
                                        L_INT nBitsPerPixel,
                                        L_INT nOrder,
                                        L_INT32 nBufferSize,
                                        pLOADFILEOPTION pLoadOptions,
                                        pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadFile (L_CHAR L_FAR * pszFile,
                                pBITMAPHANDLE pBitmap,
                                L_INT nBitsPerPixel,
                                L_INT nOrder,
                                L_UINT uFlags,
                                FILEREADCALLBACK pfnCallback,
                                L_VOID L_FAR * pUserData,
                                pLOADFILEOPTION pLoadOptions,
                                pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadFileTile (L_CHAR L_FAR * pszFile,
                                   pBITMAPHANDLE pBitmap,
                                   L_INT nCol,
                                   L_INT nRow,
                                   L_UINT uWidth,
                                   L_UINT uHeight,
                                   L_INT nBitsPerPixel,
                                   L_INT nOrder,
                                   L_UINT uFlags,
                                   FILEREADCALLBACK pfnCallback,
                                   L_VOID L_FAR * pUserData,
                                   pLOADFILEOPTION pLoadOptions,
                                   pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadFileOffset (L_INT fd,
                                      L_INT32 nOffsetBegin,
                                      L_INT32 nBytesToLoad,
                                      pBITMAPHANDLE pBitmap,
                                      L_INT nBitsPerPixel,
                                      L_INT nOrder,
                                      L_UINT uFlags,
                                      FILEREADCALLBACK pfnCallback,
                                      L_VOID L_FAR * pUserData,
                                      pLOADFILEOPTION pLoadOptions,
                                      pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_LoadMemory (L_CHAR L_FAR * pBuffer,
                                  pBITMAPHANDLE pBitmap,
                                  L_INT nBitsPerPixel,
                                  L_INT nOrder,
                                  L_UINT uFlags,
                                  FILEREADCALLBACK pfnCallback,
                                  L_VOID L_FAR * pUserData,
                                  L_INT32 nBufferSize,
                                  pLOADFILEOPTION pLoadOptions,
                                  pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_PreLoadFilters (L_INT nFixedFilters, 
                                     L_INT nCachedFilters,
                                     L_CHAR L_FAR*pszFilters);

L_INT EXT_FUNCTION L_ReadFileComment (L_CHAR L_FAR *pszFile,
                                       L_UINT uType,
                                       L_UCHAR L_FAR *pComment,
                                       L_UINT uLength,
                                       pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_ReadFileCommentExt (L_CHAR L_FAR *pszFile,
                                       L_UINT uType,
                                       LPFILECOMMENTS pComments,
                                       L_UCHAR L_HUGE *pBuffer,
                                       L_UINT L_FAR *uLength,
                                       pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_ReadFileCommentMemory (L_CHAR L_FAR * pBuffer,
                                       L_UINT uType,
                                       L_UCHAR L_FAR * pComment,
                                       L_UINT uLength,
                                       L_INT32 nBufferSize,
                                       pLOADFILEOPTION pLoadOptions);

L_INT32 EXT_FUNCTION L_ReadFileTag (L_CHAR L_FAR * pFile,
                                 L_UINT16 uTag,
                                 L_UINT16 L_FAR*pType,
                                 L_UINT32 L_FAR*pCount,
                                 L_VOID L_FAR*pData,
                                 pLOADFILEOPTION pLoadOptions);

L_INT32 EXT_FUNCTION L_ReadFileTagMemory (L_CHAR L_FAR * pBuffer,
                                 L_UINT16 uTag,
                                 L_UINT16 L_FAR*pType,
                                 L_UINT32 L_FAR*pCount,
                                 L_VOID L_FAR*pData,
                                 L_INT32  nBufferSize,
                                 pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_ReadFileStamp (L_CHAR L_FAR * pszFile,
                                    pBITMAPHANDLE pBitmap,
                                    pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_SaveBitmap (L_CHAR L_FAR * pszFile,
                                  pBITMAPHANDLE pBitmap,
                                  L_INT nFormat,
                                  L_INT nBitsPerPixel,
                                  L_INT nQFactor,
                                  pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveBitmapList (LPSTR lpszFile,
                                  HBITMAPLIST hList,
                                  L_INT nFormat,
                                  L_INT nBits,
                                  L_INT nQFactor,
                                  pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveBitmapMemory (HGLOBAL L_FAR * phHandle,
                                        pBITMAPHANDLE pBitmap,
                                        L_INT nFormat,
                                        L_INT nBitsPerPixel,
                                        L_INT nQFactor,
                                        L_UINT32 L_FAR * puSize,
                                        pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveFile (L_CHAR L_FAR * pszFile,
                                pBITMAPHANDLE pBitmap,
                                L_INT nFormat,
                                L_INT nBitsPerPixel,
                                L_INT nQFactor,
                                L_UINT uFlags,
                                FILESAVECALLBACK pfnCallback,
                                L_VOID L_FAR * pUserData,
                                pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveCustomFile
(
   L_CHAR L_FAR*           pszFilename          ,
   pBITMAPHANDLE           pInBitmap            ,
   L_INT                   nFormat              ,
   L_INT                   nBitsTo              ,
   L_INT                   nQFactor             ,
	L_UINT                  uFlags               ,
   CUSTOMFILESAVECALLBACK  pfnCallback          ,
	L_VOID L_FAR*           pUserData            ,
	pSAVECUSTOMFILEOPTION   pSaveCustomFileOption,
   pSAVEFILEOPTION         pSaveOptions
);

L_INT EXT_FUNCTION L_SaveFileMemory(HANDLE L_FAR *hHandle,
                                   pBITMAPHANDLE pBitmap,
                                   L_INT nFormat,
                                   L_INT nBitsPerPixel,
                                   L_INT nQFactor,
                                   L_UINT uFlags,
                                   FILESAVECALLBACK pFunction,
                                   L_VOID L_FAR * lpUserData,
                                   L_UINT32 L_FAR *uSize,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveFileTile (L_CHAR L_FAR * pszFile,
                                   pBITMAPHANDLE pBitmap,
                                   L_INT nCol,
                                   L_INT nRow,
                                   FILESAVECALLBACK pfnCallback,
                                   L_VOID L_FAR * pUserData,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SaveFileOffset (L_INT fd,
                                      L_INT32 nOffsetBegin,
                                      L_INT32 L_FAR * nSizeWritten,
                                      pBITMAPHANDLE pBitmap,
                                      L_INT nFormat,
                                      L_INT nBitsPerPixel,
                                      L_INT nQFactor,
                                      L_UINT uFlags,
                                      FILESAVECALLBACK pfnCallback,
                                      L_VOID L_FAR * pUserData,
                                      pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SetComment (L_UINT uType,
                                  L_UCHAR L_FAR * pComment,
                                  L_UINT uLength);

LOADINFOCALLBACK EXT_FUNCTION L_SetLoadInfoCallback (LOADINFOCALLBACK pfnCallback, 
                                                     L_VOID L_FAR*pUserData);

L_INT EXT_FUNCTION L_SetLoadResolution (L_INT nFormat,
                                  L_UINT32 nWidth,
                                  L_UINT32 nHeight);

L_INT EXT_FUNCTION L_SetPCDResolution (L_INT nResolution);

L_INT EXT_FUNCTION L_SetWMFResolution (L_INT nResolution);

L_INT EXT_FUNCTION L_SetTag( L_UINT16 uTag, 
                                 L_UINT16 uType,
                                 L_UINT32 uCount,
                                 L_VOID L_FAR*pData);

L_INT EXT_FUNCTION L_StartCompressBuffer (pBITMAPHANDLE pBitmap,
                                           COMPBUFFCALLBACK pfnCallback,
                                           L_UINT32 uInputBytes,
                                           L_UINT uOutputBytes,
                                           L_CHAR L_FAR * pOutputBuffer,
                                           L_INT nOutputType,
                                           L_INT nQFactor,
                                           L_VOID L_FAR * pUserData,
                                           pSAVEFILEOPTION pSaveOptions);


L_INT EXT_FUNCTION L_StartFeedLoad (pBITMAPHANDLE pBitmap,
                                     L_INT nBitsPerPixel,
                                     L_INT nOrder,
                                     L_UINT uFlags,
                                     FILEREADCALLBACK pfnCallback,
                                     L_VOID L_FAR * pUserData,
                                     HGLOBAL L_FAR * phLoad,
                                     pLOADFILEOPTION pLoadOptions,
                                     pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_StopFeedLoad (HGLOBAL hLoad);

L_INT EXT_FUNCTION L_WriteFileCommentExt (L_CHAR L_FAR *pszFile,
                                       L_UINT uType,
                                       LPFILECOMMENTS pComments,
                                       pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_WriteFileStamp (L_CHAR L_FAR * pszFile,
                                    pBITMAPHANDLE pBitmap,
                                    pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_SetSaveResolution (L_UINT uCount, 
                                        pDIMENSION pResolutions);

L_INT EXT_FUNCTION L_GetSaveResolution (L_UINT L_FAR*puCount, 
                                        pDIMENSION pResolutions);

L_INT EXT_FUNCTION L_2DSetViewport(L_INT nWidth, 
                                   L_INT nHeight);

L_INT EXT_FUNCTION L_2DGetViewport(L_INT L_FAR*pnWidth, 
                                   L_INT L_FAR*pnHeight);

L_INT EXT_FUNCTION L_2DSetViewMode(L_INT nViewMode);

L_INT EXT_FUNCTION L_2DGetViewMode(L_VOID);

L_INT EXT_FUNCTION L_VecLoadFile(LPSTR pszFile, 
                                  pVECTORHANDLE pVector,
                                  pLOADFILEOPTION pLoadOptions,
                                  pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_VecLoadMemory (L_CHAR L_FAR * pBuffer,
                                    pVECTORHANDLE pVector,
                                    L_INT32 nBufferSize,
                                    pLOADFILEOPTION pLoadOptions,
                                    pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_VecStartFeedLoad ( pVECTORHANDLE pVector,
                                       HANDLE L_FAR *phLoad,
                                       pLOADFILEOPTION pLoadOptions,
                                       pFILEINFO pFileInfo);

L_INT EXT_FUNCTION L_VecFeedLoad(HANDLE hLoad,
                                L_UCHAR L_FAR *pInBuffer,
                                L_UINT32 dwBufferSize);

L_INT EXT_FUNCTION L_VecStopFeedLoad(HANDLE hLoad);

L_INT EXT_FUNCTION L_VecSaveFile(LPSTR pszFile, 
                                   pVECTORHANDLE pVector, 
                                   L_INT nFormat,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_VecSaveMemory(HANDLE L_FAR *hHandle,
                                   pVECTORHANDLE pVector,
                                   L_INT nFormat,
                                   L_UINT32 L_FAR *uSize,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_GetDefaultLoadFileOption (pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_GetDefaultSaveFileOption (pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_WriteFileTag(L_CHAR L_FAR*pszFile,
                                  pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_WriteFileComment(L_CHAR L_FAR*pszFile,
                                  pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_DocLoadFile(LPSTR pszFile, 
                                  pDOCHANDLE pDocument,
                                  pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_DocLoadMemory (L_CHAR L_FAR * pBuffer,
                                    pDOCHANDLE pDocument,
                                    L_INT32 nBufferSize,
                                    pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_DocStartFeedLoad ( pDOCHANDLE pDocument,
                                       HANDLE L_FAR *phLoad,
                                       pLOADFILEOPTION pLoadOptions);

L_INT EXT_FUNCTION L_DocFeedLoad(HANDLE hLoad,
                                L_UCHAR L_FAR *pInBuffer,
                                L_UINT32 dwBufferSize);

L_INT EXT_FUNCTION L_DocStopFeedLoad(HANDLE hLoad);

L_INT EXT_FUNCTION L_DocSaveFile(LPSTR pszFile, 
                                   pDOCHANDLE pDocument, 
                                   L_INT nFormat,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_DocSaveMemory(HANDLE L_FAR *hHandle,
                                   pDOCHANDLE pDocument,
                                   L_INT nFormat,
                                   L_UINT32 L_FAR *uSize,
                                   pSAVEFILEOPTION pSaveOptions);

L_INT EXT_FUNCTION L_GetPLTOptions( pFILEPLTOPTIONS );
L_INT EXT_FUNCTION L_SetPLTOptions( const pFILEPLTOPTIONS );

L_INT EXT_FUNCTION L_CreateThumbnailFromFile( L_CHAR L_FAR * pszFile,
                                              pBITMAPHANDLE pBitmap,
                                              const pTHUMBOPTIONS pThumbOptions,
                                              FILEREADCALLBACK pfnCallback,
                                              L_VOID L_FAR * pUserData,
                                              pLOADFILEOPTION pLoadOptions,
                                              pFILEINFO pFileInfo );

L_INT EXT_FUNCTION L_GetPDFOptions( pFILEPDFOPTIONS, L_INT nSize );
L_INT EXT_FUNCTION L_SetPDFOptions( const pFILEPDFOPTIONS );

#undef _HEADER_ENTRY_
#include "ltpck.h"

#endif                          /* _LTFIL_H_ */
