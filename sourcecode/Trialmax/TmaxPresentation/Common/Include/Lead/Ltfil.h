/*************************************************************
   Ltfil.h - file module library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTFIL_H)
#define LTFIL_H

#if !defined(L_LTFIL_API)
   #define L_LTFIL_API
#endif // #if !defined(L_LTFIL_API)

#if !defined(LEAD_DEFINES_ONLY)

#include "Ltkrn.h"
#include "Lttyp.h"

#if !defined(FOR_WINCE)
#include "Lvkrn.h"
//#include "Ltclr.h"
#endif // #if !defined(FOR_WINCE)

#define L_HEADER_ENTRY
#include "Ltpck.h"

#endif // !defined(LEAD_DEFINES_ONLY)

/****************************************************************
   Enums/defines
****************************************************************/

// List of file types supported
#define FILE_UNKNOWN_FORMAT               0

#define FILE_PCX                          1    // ZSoft PCX
#define FILE_GIF                          2    // CompuServe GIF
#define FILE_TIF                          3    // Tagged Image File Format
#define FILE_TGA                          4    // Targa
#define FILE_CMP                          5    // LEAD CMP
#define FILE_BMP                          6    // Windows BMP
#define FROM_BUFFER                       7    // Internal use
#define FILE_BITMAP                       9    // Internal use
#define FILE_JPEG                         10   // Jpeg File Interchange Format
#define FILE_TIF_JPEG                     11   // Jpeg Tag Image File Format
#define FILE_BIN                          12   // Internal use
#define FILE_HANDLE                       13   // Internal use
#define FILE_OS2                          14   // OS/2 BMP
#define FILE_WMF                          15   // Windows Meta File
#define FILE_EPS                          16   // Encapsulated Post Script
#define FILE_TIFLZW                       17   // TIF Format with LZW compression
#define FILE_LEAD                         20   // LEAD  Proprietary
#define FILE_JPEG_411                     21   // JPEG  4:1:1
#define FILE_TIF_JPEG_411                 22   // JPEG  4:1:1
#define FILE_JPEG_422                     23   // JPEG  4:2:2
#define FILE_TIF_JPEG_422                 24   // JPEG  4:2:2
#define FILE_CCITT                        25   // TIFF  CCITT
#define FILE_LEAD1BIT                     26   // LEAD 1 bit, lossless compression
#define FILE_CCITT_GROUP3_1DIM            27   // CCITT Group3 one dimension
#define FILE_CCITT_GROUP3_2DIM            28   // CCITT Group3 two dimensions
#define FILE_CCITT_GROUP4                 29   // CCITT Group4 two dimensions

#define FILE_LEAD1BITA                    31   // old LEAD 1 bit, lossless compression
#define FILE_ABC                          32   // LEAD ABC 1 bit, lossless compression

#define FILE_CALS                         50
#define FILE_MAC                          51
#define FILE_IMG                          52
#define FILE_MSP                          53
#define FILE_WPG                          54
#define FILE_RAS                          55
#define FILE_PCT                          56
#define FILE_PCD                          57
#define FILE_DXF                          58
#define FILE_AVI                          59
#define FILE_WAV                          60
#define FILE_FLI                          61
#define FILE_CGM                          62
#define FILE_EPSTIFF                      63   // EPS with TIFF Preview
#define FILE_EPSWMF                       64   // EPS with Metafile Preview
#define FILE_CMPNOLOSS                    65
#define FILE_FAX_G3_1D                    66
#define FILE_FAX_G3_2D                    67
#define FILE_FAX_G4                       68
#define FILE_WFX_G3_1D                    69
#define FILE_WFX_G4                       70
#define FILE_ICA_G3_1D                    71
#define FILE_ICA_G3_2D                    72
#define FILE_ICA_G4                       73
#define FILE_ICA_IBM_MMR                  117  // MO:DCA IBM MMR CCITT G32D
#define FILE_ICA_UNCOMPRESSED             253
#define FILE_ICA_ABIC                     190  // MO:DCA IOCA files ABIC compression
#define FILE_OS2_2                        74
#define FILE_PNG                          75
#define FILE_PSD                          76
#define FILE_RAWICA_G3_1D                 77
#define FILE_RAWICA_G3_2D                 78
#define FILE_RAWICA_G4                    79
#define FILE_RAWICA_IBM_MMR               118  // Raw IOCA IBM MMR CCITT G32D
#define FILE_RAWICA_UNCOMPRESSED          254
#define FILE_RAWICA_ABIC                  184  // Raw IOCA ABIC
#define FILE_FPX                          80   // FlashPix, no compression
#define FILE_FPX_SINGLE_COLOR             81   // FlashPix, compression 'single color' method
#define FILE_FPX_JPEG                     82   // FlashPix, compression JPEG
#define FILE_FPX_JPEG_QFACTOR             83   // FlashPix, compression JPEG, specify qFactor
#define FILE_BMP_RLE                      84   // compressed Windows BMP
#define FILE_TIF_CMYK                     85   // TIFF no compression,      CMYK data
#define FILE_TIFLZW_CMYK                  86   // TIFF LZW compression,     CMYK data
#define FILE_TIF_PACKBITS                 87   // TIFF PackBits compression, RGB data
#define FILE_TIF_PACKBITS_CMYK            88   // TIFF PackBits compression, CMYK data
#define FILE_DICOM_GRAY                   89
#define FILE_DICOM_COLOR                  90
#define FILE_WIN_ICO                      91
#define FILE_WIN_CUR                      92
#define FILE_TIF_YCC                      93   // TIFF YCbCr color space, no compression
#define FILE_TIFLZW_YCC                   94   // TIFF YCbCr color space, LZW compression
#define FILE_TIF_PACKBITS_YCC             95   // TIFF YCbCr color space, PackBits compression
#define FILE_EXIF                         96   // uncompressed RGB Exif file
#define FILE_EXIF_YCC                     97   // uncompressed YCbCr Exif file
#define FILE_EXIF_JPEG_422                98   // JPEG 4:2:2 compressed Exif file
#define FILE_AWD                          99   // Microsoft Fax format
#define FILE_FASTEST                      100  // for ISIS only! use the data as is, from the ISIS Scanner
#define FILE_EXIF_JPEG_411                101  // JPEG 4:1:1 compressed Exif file

#define FILE_PBM_ASCII                    102
#define FILE_PBM_BINARY                   103
#define FILE_PGM_ASCII                    104
#define FILE_PGM_BINARY                   105
#define FILE_PPM_ASCII                    106
#define FILE_PPM_BINARY                   107
#define FILE_CUT                          108
#define FILE_XPM                          109
#define FILE_XBM                          110
#define FILE_IFF_ILBM                     111
#define FILE_IFF_CAT                      112
#define FILE_XWD                          113
#define FILE_CLP                          114
#define FILE_JBIG                         115
#define FILE_EMF                          116
#define FILE_ANI                          119
#define FILE_LASERDATA                    121  // LaserData CCITT G4
#define FILE_INTERGRAPH_RLE               122  // Intergraph RLE
#define FILE_INTERGRAPH_VECTOR            123  // Intergraph Vector
#define FILE_DWG                          124

#define FILE_DICOM_RLE_GRAY               125
#define FILE_DICOM_RLE_COLOR              126
#define FILE_DICOM_JPEG_GRAY              127
#define FILE_DICOM_JPEG_COLOR             128

#define FILE_CALS4                        129
#define FILE_CALS2                        130
#define FILE_CALS3                        131

#define FILE_XWD10                        132
#define FILE_XWD11                        133
#define FILE_FLC                          134
#define FILE_KDC                          135
#define FILE_DRW                          136
#define FILE_PLT                          137

#define FILE_TIF_CMP                      138
#define FILE_TIF_JBIG                     139
#define FILE_TIF_DXF_R13                  140
#define FILE_TIF_DXF_R12                  176
#define FILE_TIF_UNKNOWN                  141  // TIFF file with unknown compression

#define FILE_SGI                          142
#define FILE_SGI_RLE                      143
#define FILE_VECTOR_DUMP                  144
#define FILE_DWF                          145

#define FILE_RAS_PDF                      146  // Raster PDF uncompressed
#define FILE_RAS_PDF_G3_1D                147  // Raster PDF CCITT G3 1D compression
#define FILE_RAS_PDF_G3_2D                148  // Raster PDF CCITT G3 2D compression
#define FILE_RAS_PDF_G4                   149  // Raster PDF CCITT G4 compression
#define FILE_RAS_PDF_JPEG                 150  // Raster PDF JPEG 24-bit color 4:4:4 or grayscale 8-bit
#define FILE_RAS_PDF_JPEG_422             151  // Raster PDF JPEG 24-bit color 4:2:2
#define FILE_RAS_PDF_JPEG_411             152  // Raster PDF JPEG 24-bit color 4:1:1
#define FILE_RAS_PDF_LZW                  179  // Raster PDF LZW compression
#define FILE_RAS_PDF_JBIG2                188  // Raster PDF JBIG2 compression

#define FILE_RAW                          153  // Raw uncompressed data

#define FILE_RASTER_DUMP                  154
#define FILE_TIF_CUSTOM                   155

#define FILE_RAW_RGB                      156  // Raw RGB
#define FILE_RAW_RLE4                     157  // Raw RLE4 compressed 4-bit data
#define FILE_RAW_RLE8                     158  // Raw RLE8 compressed 8-bit data
#define FILE_RAW_BITFIELDS                159  // Raw BITFIELD data--16 or 32 bit
#define FILE_RAW_PACKBITS                 160  // Raw Packbits compression
#define FILE_RAW_JPEG                     161  // Raw JPEG compression
#define FILE_FAX_G3_1D_NOEOL              162  // Raw CCITT compression--CCITT with no eol
#define FILE_RAW_LZW                      178

#define FILE_JP2                          163  // Jpeg2000 file
#define FILE_J2K                          164  // Jpeg2000 stream
#define FILE_CMW                          165  // Wavelet CMP

#define FILE_TIF_J2K                      166  // TIFF Jpeg2000 stream
#define FILE_TIF_CMW                      167  // TIFF Wavelet CMP
#define FILE_MRC                          168  // T44 (MRC) files
#define FILE_GERBER                       169  // Gerber Vector
#define FILE_WBMP                         170
#define FILE_JPEG_LAB                     171  // JPEG CieLAB 4:4:4
#define FILE_JPEG_LAB_411                 172  // JPEG CieLAB 4:1:1
#define FILE_JPEG_LAB_422                 173  // JPEG CieLAB 4:2:2
#define FILE_GEOTIFF                      174  // GeoTIFF files
#define FILE_TIF_LEAD1BIT                 175  // TIF files LEAD 1-bit compression
#define FILE_TIF_MRC                      177  // TIF files with MRC compression
#define FILE_TIF_ABC                      180  // LEAD ABC 1 bit, lossless compression
#define FILE_NAP                          181
#define FILE_JPEG_RGB                     182  // JPEG RGB 4:4:4
#define FILE_JBIG2                        183  // JBIG-2
#define FILE_ABIC                         185  // ABIC files
#define FILE_TIF_ABIC                     186  // TIFF files ABIC compression
#define FILE_TIF_JBIG2                    187  // TIFF files with JBIG2 compression
#define FILE_TIF_ZIP                      189  // TIFF files with ZIP compression
/* Next available: 192 */

// File formats supported by the OCR module
#define FILE_AMI_PRO_20                   200  // Ami Pro 2.0
#define FILE_AMI_PRO_30                   201  // Ami Pro 3.0
#define FILE_ASCII_SMART                  202  // ASCII Smart
#define FILE_ASCII_STANDARD               203  // ASCII Standard
#define FILE_ASCII_STANDARD_DOS           204  // ASCII Standard (DOS)
#define FILE_ASCII_STRIPPED               205  // ASCII Stripped
#define FILE_DBASE_IV_10                  206  // dBase IV v1.0
#define FILE_DCA_RFT                      207  // DCA/RFT
#define FILE_DCA_RFT_DW_5                 208  // DisplayWrite 5
#define FILE_EXCEL_MAC                    209  // Excel for the Macintosh
#define FILE_EXCEL_30                     210  // Excel 3.0
#define FILE_EXCEL_40                     211  // Excel 4.0
#define FILE_EXCEL_50                     212  // Excel 5.0
#define FILE_EXCEL_OFFICE97               213  // Excel Office 97
#define FILE_FRAMEMAKER                   214  // FrameMaker
#define FILE_HTML_20                      215  // HTML (2.0 specification)
#define FILE_HTML_EDITOR_20               216  // HTML (SoftQuad Editor)
#define FILE_HTML_NETSCAPE_20             217  // HTML (Netscape additions)
#define FILE_INTERLEAF                    218  // Interleaf
#define FILE_LOTUS123                     219  // Lotus 1-2-3
#define FILE_LOTUS_WORD_PRO               220  // Lotus Word Pro
#define FILE_MULTIMATE_ADV_II             221  // MultiMate Advantage II
#define FILE_POSTSCRIPT                   222  // Postscript
#define FILE_PROFESSIONAL_WRITE_20        223  // Professional Write 2.0
#define FILE_PROFESSIONAL_WRITE_22        224  // Professional Write 2.2
#define FILE_QUATTRA_PRO                  225  // Quattra Pro
#define FILE_RTF                          226  // Rich Text Format
#define FILE_RTF_MAC                      227  // Rich Text Format (Macintosh)
#define FILE_RTF_WORD_60                  228  // Rich Text Format (Word 6.0)
#define FILE_WINDOWS_WRITE                229  // Windows Write
#define FILE_WORD_WINDOWS_2X              230  // Word for Windows 2.X
#define FILE_WORD_WINDOWS_60              231  // Word for Windows 6.0
#define FILE_WORD_OFFICE97                232  // Word Office 97
#define FILE_WORDPERFECT_DOS_42           233  // WordPerfect 4.2 (DOS)
#define FILE_WORDPERFECT_WINDOWS          234  // WordPerfect (Windows)
#define FILE_WORDPERFECT_WINDOWS_60       235  // WordPerfect 6.0 (Windows)
#define FILE_WORDPERFECT_WINDOWS_61       236  // WordPerfect 6.1 (Windows)
#define FILE_WORDPERFECT_WINDOWS_7X       237  // WordPerfect 7X (Windows)
#define FILE_WORDSTAR_WINDOWS_1X          238  // WordStar 1.X (Windows)
#define FILE_WORKS                        239  // Works
#define FILE_XDOC                         240  // Xerox XDOC

// Multimedia formats
#define FILE_MOV                          241  // Apple QuickTime
#define FILE_MIDI                         242  // MIDI music file
#define FILE_MPEG1                        243  // MPEG-1 file
#define FILE_AU                           244  // SUN sound file
#define FILE_AIFF                         245  // Apple/SGI sound file
#define FILE_MPEG2                        246  // MPEG-2 file

#define FILE_SVG                          247  // SVG

#define FILE_NITF                         248  // NITF
#define FILE_PTOCA                        249  // PTOCA
#define FILE_SCT                          250
#define FILE_PCL                          251
#define FILE_AFP                          252
#define FILE_SHP                          255  // ESRI
#define FILE_SMP                          256
#define FILE_SMP_G3_1D                    257  // CCITT Group 3 1D
#define FILE_SMP_G3_2D                    258  // CCITT Group 3 2D
#define FILE_SMP_G4                       259  // CCITT Group 4

#define FILE_VWPG                         260  // vector WPG
#define FILE_VWPG1                        328  // vector WPG
#define FILE_CMX                          261

#define FILE_TGA_RLE                      262  // RLE Compressed TGA

#define FILE_KDC_120                      263
#define FILE_KDC_40                       264
#define FILE_KDC_50                       265
#define FILE_DCS                          266

#define FILE_PSP                          267
#define FILE_PSP_RLE                      268

#define FILE_TIFX_JBIG                    269
#define FILE_TIFX_JBIG_T43                270
#define FILE_TIFX_JBIG_T43_ITULAB         271
#define FILE_TIFX_JBIG_T43_GS             272
#define FILE_TIFX_FAX_G4                  273
#define FILE_TIFX_FAX_G3_1D               274
#define FILE_TIFX_FAX_G3_2D               275
#define FILE_TIFX_JPEG                    276

#define FILE_ECW                          277
#define FILE_RAS_RLE                      288
#define FILE_SVG_EMBED_IMAGES             289
#define FILE_DXF_R13                      290
#define FILE_CLP_RLE                      291
#define FILE_DCR                          292

#define FILE_DICOM_J2K_GRAY               293
#define FILE_DICOM_J2K_COLOR              294

#define FILE_FIT                          295
#define FILE_CRW                          296
#define FILE_DWF_TEXT_AS_POLYLINE         297
#define FILE_CIN                          298
#define FILE_PCL_TEXT_AS_POLYLINE         299
#define FILE_EPSPOSTSCRIPT                300
#define FILE_INTERGRAPH_CCITT_G4          301

#define FILE_SFF                          302
#define FILE_IFF_ILBM_UNCOMPRESSED        303
#define FILE_IFF_CAT_UNCOMPRESSED         304
#define FILE_RTF_RASTER                   305
#define FILE_SID                          306
#define FILE_WMZ                          307
#define FILE_DJVU                         308

#define FILE_AFPICA_G3_1D                 309   // MO:DCA IOCA files with AFP prefix and Fax G3 1D compression
#define FILE_AFPICA_G3_2D                 310   // MO:DCA IOCA files with AFP prefix and Fax G3 2D compression
#define FILE_AFPICA_G4                    311   // MO:DCA IOCA files with AFP prefix and Fax G4 compression
#define FILE_AFPICA_UNCOMPRESSED          312   // MO:DCA IOCA files with AFP prefix and uncompressed data
#define FILE_AFPICA_IBM_MMR               313   // MO:DCA IOCA files with AFP prefix and Fax G3 1D IBM MMR modified compression (no EOL)
#define FILE_AFPICA_ABIC                  191   // MO:DCA IOCA files with AFP prefix and ABIC compression

#define FILE_LEAD_MRC                     314
#define FILE_TIF_LEAD_MRC                 315

#define FILE_TXT                          316

#define FILE_PDF_LEAD_MRC                 317

#define FILE_HDP                          318
#define FILE_HDP_GRAY                     319
#define FILE_HDP_CMYK                     320

#define FILE_PNG_ICO                      321
#define FILE_XPS                          322
#define FILE_JPX                          323  // Jpeg2000 extension part 2

#define FILE_XPS_JPEG                     324
#define FILE_XPS_JPEG_422                 325
#define FILE_XPS_JPEG_411                 326

#define FILE_MNG                          327
#define FILE_MNG_GRAY                     329
#define FILE_MNG_JNG                      330
#define FILE_MNG_JNG_411                  331
#define FILE_MNG_JNG_422                  332

#define FILE_RAS_PDF_CMYK                 333  // Raster PDF CMYK data uncompressed
#define FILE_RAS_PDF_LZW_CMYK             334  // Raster PDF CMYK data LZW compression

#define FILE_MIF                          335  // MIF Vector format
#define FILE_E00                          336  // E00 Vector format

#define FILE_TDB                          337  // XP Thumbnail format
#define FILE_TDB_VISTA                    338  // Vista Thumbnail format

#define FILE_SNP                          339  // MS Access Report Snapshots

#define FILE_AFP_IM1                      340  // IM1 uncompressed AFP

#define FILE_XLS                          341  // Excel file format

// File formats defines that are not unique
// Here for backward compatibility
#define FILE_JFIF                         FILE_JPEG
#define FILE_JTIF                         FILE_TIF_JPEG
#define FILE_LEAD1JFIF                    FILE_JPEG_411
#define FILE_LEAD1JTIF                    FILE_TIF_JPEG_411
#define FILE_LEAD2JFIF                    FILE_JPEG_422
#define FILE_LEAD2JTIF                    FILE_TIF_JPEG_422
#define FILE_DXF_R12                      FILE_DXF
#define FILE_EXIF_JPEG                    FILE_EXIF_JPEG_422
#define FILE_TIF_DXF                      FILE_TIF_DXF_R13
#define FILE_RAW_CCITT                    FILE_FAX_G3_1D_NOEOL
#define FILE_JFIF_LAB                     FILE_JPEG_LAB
#define FILE_LEAD1JFIF_LAB                FILE_JPEG_LAB_411
#define FILE_LEAD2JFIF_LAB                FILE_JPEG_LAB_422

// compression type
#define LEAD                  0  // LEAD  Proprietary
#define JFIF                  1  // JPEG  4:4:4
#define JTIF                  2  // JPEG  4:4:4
#define LEAD1JFIF             3  // JPEG  4:1:1
#define LEAD1JTIF             4  // JPEG  4:1:1
#define LEAD2JFIF             5  // JPEG  4:2:2
#define LEAD2JTIF             6  // JPEG  4:2:2
#define LEADJ2K               7  // j2k
#define LEADJP2               8  // jp2


#define LEAD_0                0  // LEAD 1 bit, lossless compression
#define LEAD_1                1  // LEAD 1 bit, excellent compression

#define TIFF_CCITT            3  // TIFF  CCITT
#define TIFF_CCITTG3_FAX1D    4  // CCITT Group3 one dimensional
#define TIFF_CCITTG3_FAX2D    5  // CCITT Group3 two dimensional
#define TIFF_CCITTG4_FAX      6  // CCITT Group4 two dimensional

// flags for L_*Comment
// the following indicate the associated strings are null terminated
#define CMNT_SZARTIST                                    0     // Person who created image
#define CMNT_SZCOPYRIGHT                                 1     // Copyright notice
#define CMNT_SZDATETIME                                  2     // "YYYY:MM:DD HH:MM:SS" format
#define CMNT_SZDESC                                      3     // Description of image
#define CMNT_SZHOSTCOMP                                  4     // Computer/OP System in use
#define CMNT_SZMAKE                                      5     // Manufacturer of Equip. used generate the image
#define CMNT_SZMODEL                                     6     // Model Name/Number of Equipment
#define CMNT_SZNAMEOFDOC                                 7     // Doc name image was scanned from
#define CMNT_SZNAMEOFPAGE                                8     // Page name image was scanned from
#define CMNT_SZSOFTWARE                                  9     // Name & Version of Software Package used to gen the image
#define CMNT_SZPATIENTNAME                               10    // Patient name (DICOM)
#define CMNT_SZPATIENTID                                 11    // Patient ID (DICOM)
#define CMNT_SZPATIENTBIRTHDATE                          12    // Patient birthdate (DICOM)
#define CMNT_SZPATIENTSEX                                13    // Patient sex (DICOM)
#define CMNT_SZSTUDYINSTANCE                             14    // Study instance ID (DICOM)
#define CMNT_SZSTUDYDATE                                 15    // Study date (DICOM)
#define CMNT_SZSTUDYTIME                                 16    // Study time (DICOM)
#define CMNT_SZSTUDYREFERRINGPHYSICIAN                   17    // Referring physician (DICOM)
#define CMNT_SZSERIESMODALITY                            18    // Series modality (DICOM)
#define CMNT_SZSERIESID                                  19    // Series ID (DICOM)
#define CMNT_SZSERIESNUMBER                              20    // Series number (DICOM)

// Exif 1.0 and 1.1 comments
#define CMNT_EXIFVERSION                                 21    // Exif version
#define CMNT_SZDATETIMEORIGINAL                          22    // Date and time the original image image is captured (Exif)
#define CMNT_SZDATETIMEDIGITIZED                         23    // Date and time the file is generated (Exif)
#define CMNT_SHUTTERSPEEDVALUE                           24    // Shutter speed (Exif)
#define CMNT_APERTURE                                    25    // Aperture value (Exif)
#define CMNT_BRIGHTNESS                                  26    // Brightness value (Exif)
#define CMNT_EXPOSUREBIAS                                27    // Exposure bias (Exif)
#define CMNT_MAXAPERTURE                                 28    // Minimum lens f-number (Exif)
#define CMNT_SUBJECTDISTANCE                             29    // Distance from lens to subject (m) (Exif)
#define CMNT_METERINGMODE                                30    // Photometry mode (Exif)
#define CMNT_LIGHTSOURCE                                 31    // Light source (Exif)
#define CMNT_FLASH                                       32    // Flash On/Off (Exif)
#define CMNT_FOCALLENGTH                                 33    // Focal length (Exif)
#define CMNT_EXPOSURETIME                                34    // Exposure  (Exif)
#define CMNT_FNUMBER                                     35    // F-numnber (Exif)
#define CMNT_MAKERNOTE                                   36    // Maker note (Exif)
#define CMNT_USERCOMMENT                                 37    // User comment (Exif)
#define CMNT_SZSUBSECTIME                                38    // Date Time subsec (Exif)
#define CMNT_SZSUBSECTIMEORIGINAL                        39    // Date Time original subsec (Exif)
#define CMNT_SZSUBSECTIMEDIGITIZED                       40    // Date Time digitized subsec (Exif)

// Exif 2.0 comments - comments introduced since Exif 1.1
#define CMNT_SUPPORTEDFLASHPIXVERSION                    158   // Supported FlashPix version (Exif)
#define CMNT_COLORSPACE                                  159   // Color space (Exif)
#define CMNT_EXPOSUREPROGRAM                             160   // Exposure program (Exif)
#define CMNT_SZSPECTRALSENSITIVITY                       161   // Spectral sensitivity (Exif)
#define CMNT_ISOSPEEDRATINGS                             162   // ISO speed ratings (Exif)
#define CMNT_OPTOELECTRICCOEFFICIENT                     163   // Optoelectric coefficient (Exif)
#define CMNT_SZRELATEDSOUNDFILE                          164   // Related audio file (Exif)
#define CMNT_FLASHENERGY                                 165   // Flash energy (Exif)
#define CMNT_SPATIALFREQUENCYRESPONSE                    166   // Spatial frequency response (Exif)
#define CMNT_FOCALPLANEXRESOLUTION                       167   // Focal plane X Resolution (Exif)
#define CMNT_FOCALPLANEYRESOLUTION                       168   // Focal plane Y Resolution (Exif)
#define CMNT_FOCALPLANERESOLUTIONUNIT                    245   // Focal plane Resolution Unit (Exif)
#define CMNT_SUBJECTLOCATION                             169   // Subject location (Exif)
#define CMNT_EXPOSUREINDEX                               170   // Exposure index (Exif)
#define CMNT_SENSINGMETHOD                               171   // Sensing method (Exif)
#define CMNT_FILESOURCE                                  172   // File source (Exif)
#define CMNT_SCENETYPE                                   173   // Scene type (Exif)
#define CMNT_CFAPATTERN                                  174   // CFA Pattern (Exif)

// Exif 2.2 comments - comments introduced since Exif 2.0
#define CMNT_SUBJECTAREA                                 227
#define CMNT_CUSTOMRENDERED                              228
#define CMNT_EXPOSUREMODE                                229
#define CMNT_WHITEBALANCE                                230
#define CMNT_DIGITALZOOMRATIO                            231
#define CMNT_FOCALLENGTHIN35MMFILM                       232
#define CMNT_SCENECAPTURETYPE                            233
#define CMNT_GAINCONTROL                                 234
#define CMNT_CONTRAST                                    235
#define CMNT_SATURATION                                  236
#define CMNT_SHARPNESS                                   237
#define CMNT_DEVICESETTINGDESCRIPTION                    238
#define CMNT_SUBJECTDISTANCERANGE                        239
#define CMNT_SZIMAGEUNIQUEID                             240

/* Exif 2.21 comments - comments introduced since Exif 2.2 */
#define CMNT_GAMMA                                       246

// Exif 1.1 GPS comments
#define CMNT_GPSVERSIONID                                41
#define CMNT_GPSLATITUDEREF                              42
#define CMNT_GPSLATITUDE                                 43
#define CMNT_GPSLONGITUDEREF                             44
#define CMNT_GPSLONGITUDE                                45
#define CMNT_GPSALTITUDEREF                              46
#define CMNT_GPSALTITUDE                                 47
#define CMNT_GPSTIMESTAMP                                48
#define CMNT_GPSSATELLITES                               49
#define CMNT_GPSSTATUS                                   50
#define CMNT_GPSMEASUREMODE                              51
#define CMNT_GPSDOP                                      52
#define CMNT_GPSSPEEDREF                                 53
#define CMNT_GPSSPEED                                    54
#define CMNT_GPSTRACKREF                                 55
#define CMNT_GPSTRACK                                    56
#define CMNT_GPSIMGDIRECTIONREF                          57
#define CMNT_GPSIMGDIRECTION                             58
#define CMNT_GPSMAPDATUM                                 59
#define CMNT_GPSDESTLATITUDEREF                          60
#define CMNT_GPSDESTLATITUDE                             61
#define CMNT_GPSDESTLONGITUDEREF                         62
#define CMNT_GPSDESTLONGITUDE                            63
#define CMNT_GPSDESTBEARINGREF                           64
#define CMNT_GPSDESTBEARING                              65
#define CMNT_GPSDESTDISTANCEREF                          66
#define CMNT_GPSDESTDISTANCE                             67

// Exif 2.2 comments - comments introduced since Exif 2.0
#define CMNT_GPSPROCESSINGMETHOD                         241
#define CMNT_GPSAREAINFORMATION                          242
#define CMNT_GPSDATESTAMP                                243
#define CMNT_GPSDIFFERENTIAL                             244

#define CMNT_FPXSUMMARYINFORMATION                       0x8001
#define CMNT_FPXTITLE                                    68
#define CMNT_FPXSUBJECT                                  69
#define CMNT_FPXAUTHOR                                   70
#define CMNT_FPXKEYWORDS                                 71
#define CMNT_FPXCOMMENTS                                 72
#define CMNT_FPXOLETEMPLATE                              73
#define CMNT_FPXLASTAUTHOR                               74
#define CMNT_FPXREVNUMBER                                75
#define CMNT_FPXEDITTIME                                 76
#define CMNT_FPXLASTPRINTED                              77
#define CMNT_FPXCREATEDTM                                78
#define CMNT_FPXLASTSAVEDTM                              79
#define CMNT_FPXPAGECOUNT                                80
#define CMNT_FPXWORDCOUNT                                81
#define CMNT_FPXCHARCOUNT                                82
#define CMNT_FPXTHUMBNAIL                                83
#define CMNT_FPXAPPNAME                                  84
#define CMNT_FPXSECURITY                                 85
#define CMNT_FPXSUMMARYINFORMATION1                      CMNT_FPXTITLE
#define CMNT_FPXSUMMARYINFORMATION2                      CMNT_FPXSECURITY

#define CMNT_FPXFILESOURCEGROUP                          0x8002
#define CMNT_FPXFILESOURCE                               86
#define CMNT_FPXSCENETYPE                                87
#define CMNT_FPXCREATIONPATH                             88
#define CMNT_FPXNAMEMANRELEASE                           89
#define CMNT_FPXUSERDEFINEDID                            90
#define CMNT_FPXORIGINALSHARPNESSAPPROXIMATION           91
#define CMNT_FPXFILESOURCEGROUP1                         CMNT_FPXFILESOURCE
#define CMNT_FPXFILESOURCEGROUP2                         CMNT_FPXORIGINALSHARPNESSAPPROXIMATION

#define CMNT_FPXINTELLECTUALPROPERTYGROUP                0x8004
#define CMNT_FPXCOPYRIGHT                                92
#define CMNT_FPXLEGALBROKERFORORIGIMAGE                  93
#define CMNT_FPXLEGALBROKERFORDIGITALIMAGE               94
#define CMNT_FPXAUTHORSHIP                               95
#define CMNT_FPXINTELLECTUALPROPNOTES                    96
#define CMNT_FPXINTELLECTUALPROPERTYGROUP1               CMNT_FPXCOPYRIGHT
#define CMNT_FPXINTELLECTUALPROPERTYGROUP2               CMNT_FPXINTELLECTUALPROPNOTES

#define CMNT_FPXCONTENTDESCRIPTIONGROUP                  0x8008
#define CMNT_FPXTESTTARGETINTHEIMAGE                     97
#define CMNT_FPXGROUPCAPTION                             98
#define CMNT_FPXCAPTIONTEXT                              99
#define CMNT_FPXPEOPLEINTHEIMAGE                         100
#define CMNT_FPXTHINGSINIMAGE                            101
#define CMNT_FPXDATEOFORIGINALIMAGE                      102
#define CMNT_FPXEVENTSINTHEIMAGE                         103
#define CMNT_FPXPLACESINTHE                              104
#define CMNT_FPXCONTENTDESCRIPTIONNOTES                  105
#define CMNT_FPXCONTENTDESCRIPTIONGROUP1                 CMNT_FPXTESTTARGETINTHEIMAGE
#define CMNT_FPXCONTENTDESCRIPTIONGROUP2                 CMNT_FPXCONTENTDESCRIPTIONNOTES

#define CMNT_FPXCAMERAINFORMATIONGROUP                   0x8010
#define CMNT_FPXCAMERAMANUFACTURERNAME                   106
#define CMNT_FPXCAMERAMODELNAME                          107
#define CMNT_FPXCAMERASERIALNUMBER                       108
#define CMNT_FPXCAMERAINFORMATIONGROUP1                  CMNT_FPXCAMERAMANUFACTURERNAME
#define CMNT_FPXCAMERAINFORMATIONGROUP2                  CMNT_FPXCAMERASERIALNUMBER

#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP            0x8020
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
#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP1           CMNT_FPXCAPTUREDATE
#define CMNT_FPXPERPICTURECAMERASETTINGSGROUP2           CMNT_FPXPERPICTURENOTES

#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP       0x8040
#define CMNT_FPXSENSINGMETHOD                            128
#define CMNT_FPXFOCALPLANEXRESOLUTION                    129
#define CMNT_FPXFOCALPLANEYRESOLUTION                    130
#define CMNT_FPXFOCALPLANERESOLUTIONUNIT                 131
#define CMNT_FPXSPACIALFREQUENCY                         132
#define CMNT_FPXCFAPATTERN                               133
#define CMNT_FPXSPECTRALSENSITIVITY                      134
#define CMNT_FPXISOSPEEDRATINGS                          135
#define CMNT_FPXOECF                                     136
#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP1      CMNT_FPXSENSINGMETHOD
#define CMNT_FPXDIGITALCAMERACHARACTERIZATIONGROUP2      CMNT_FPXOECF

#define CMNT_FPXFILMDESCRIPTIONGROUP                     0x8080
#define CMNT_FPXFILMBRAND                                137
#define CMNT_FPXFILMCATEGORY                             138
#define CMNT_FPXFILMSIZEX                                139
#define CMNT_FPXFILMSIZEY                                140
#define CMNT_FPXFILMSIZEUNIT                             141
#define CMNT_FPXFILMROLLNUMBER                           142
#define CMNT_FPXFILMFRAMENUMBER                          143
#define CMNT_FPXFILMDESCRIPTIONGROUP1                    CMNT_FPXFILMBRAND
#define CMNT_FPXFILMDESCRIPTIONGROUP2                    CMNT_FPXFILMFRAMENUMBER

#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP     0x8100
#define CMNT_FPXORIGINALSCANNEDIMAGESIZE                 144
#define CMNT_FPXORIGINALDOCUMENTSIZE                     145
#define CMNT_FPXORIGINALMEDIUM                           146
#define CMNT_FPXTYPEOFREFLECTIONORIGINAL                 147
#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP1    CMNT_FPXORIGINALSCANNEDIMAGESIZE
#define CMNT_FPXORIGINALDOCUMENTSCANDESCRIPTIONGROUP2    CMNT_FPXTYPEOFREFLECTIONORIGINAL

#define CMNT_FPXSCANDEVICEPROPERTYGROUP                  0x8200
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
#define CMNT_FPXSCANDEVICEPROPERTYGROUP1                 CMNT_FPXSCANNERMANUFACTURERNAME
#define CMNT_FPXSCANDEVICEPROPERTYGROUP2                 CMNT_FPXSCANNERPIXELSIZE

#define CMNT_SZTITLE                                     175   // Title or caption for image
#define CMNT_SZDISCLAIMER                                176   // Legal Disclaimer
#define CMNT_SZWARNING                                   177   // Warning of nature of content
#define CMNT_MISC                                        178   // Miscellaneous comment
#define CMNT_J2K_BINARY                                  179   // Jpeg 2000 binary comment
#define CMNT_J2K_LATIN                                   180   // Jpeg 2000 latin comment

// IPTC comments
#define IPTC_SEPARATOR                                   1     // '\001' is a separator used for repeatable comments

#define CMNT_IPTC_FIRST                                  181   // The first IPTC comment
#define CMNT_IPTC_VERSION                                181   // The version of IPTC comments (read-only)
#define CMNT_IPTC_OBJECTTYPEREFERENCE                    182
#define CMNT_IPTC_OBJECTATTRIBUTEREFERENCE               183
#define CMNT_IPTC_OBJECTNAME                             184
#define CMNT_IPTC_EDITSTATUS                             185
#define CMNT_IPTC_EDITORIALUPDATE                        186
#define CMNT_IPTC_URGENCY                                187
#define CMNT_IPTC_SUBJECTREFERENCE                       188
#define CMNT_IPTC_CATEGORY                               189
#define CMNT_IPTC_SUPPLEMENTALCATEGORY                   190
#define CMNT_IPTC_FIXTUREIDENTIFIER                      191
#define CMNT_IPTC_KEYWORDS                               192
#define CMNT_IPTC_CONTENTLOCATIONCODE                    193
#define CMNT_IPTC_CONTENTLOCATIONNAME                    194
#define CMNT_IPTC_RELEASEDATE                            195
#define CMNT_IPTC_RELEASETIME                            196
#define CMNT_IPTC_EXPIRATIONDATE                         197
#define CMNT_IPTC_EXPIRATIONTIME                         198
#define CMNT_IPTC_SPECIALINSTRUCTIONS                    199
#define CMNT_IPTC_ACTIONADVISED                          200
#define CMNT_IPTC_REFERENCESERVICE                       201
#define CMNT_IPTC_REFERENCEDATE                          202
#define CMNT_IPTC_REFERENCENUMBER                        203
#define CMNT_IPTC_DATECREATED                            204
#define CMNT_IPTC_TIMECREATED                            205
#define CMNT_IPTC_DIGITALCREATIONDATE                    206
#define CMNT_IPTC_DIGITALCREATIONTIME                    207
#define CMNT_IPTC_ORIGINATINGPROGRAM                     208
#define CMNT_IPTC_PROGRAMVERSION                         209
#define CMNT_IPTC_OBJECTCYCLE                            210
#define CMNT_IPTC_BYLINE                                 211
#define CMNT_IPTC_BYLINETITLE                            212
#define CMNT_IPTC_CITY                                   213
#define CMNT_IPTC_SUBLOCATION                            214
#define CMNT_IPTC_PROVINCE_STATE                         215
#define CMNT_IPTC_PRIMARYLOCATIONCODE                    216
#define CMNT_IPTC_PRIMARYLOCATIONNAME                    217
#define CMNT_IPTC_ORIGINALTRANSMISSIONREFERENCE          218
#define CMNT_IPTC_HEADLINE                               219
#define CMNT_IPTC_CREDIT                                 220
#define CMNT_IPTC_SOURCE                                 221
#define CMNT_IPTC_COPYRIGHT                              222
#define CMNT_IPTC_CONTACT                                223
#define CMNT_IPTC_CAPTION                                224
#define CMNT_IPTC_AUTHOR                                 225
#define CMNT_IPTC_LANGUAGEIDENTIFIER                     226
#define CMNT_IPTC_LAST                                   226   // The last IPTC comment

#define CMNT_LAST                                        247   // Last defined number for comments

#define CMNT_ALL                                         0xFFFF


// Tags
#define TAG_BYTE        1
#define TAG_ASCII       2
#define TAG_SBYTE       6
#define TAG_UNDEFINED   7
#define TAG_SHORT       3
#define TAG_SSHORT      8
#define TAG_LONG        4
#define TAG_SLONG       9
#define TAG_FLOAT       11
#define TAG_RATIONAL    5
#define TAG_SRATIONAL   10
#define TAG_DOUBLE      12

// Flags that make up EXTENSIONLIST.uFlags
#define EXTENSION_STAMP    0x0001   // contains a stamp. Most likely suitable for LCD displays
#define EXTENSION_AUDIO    0x0002   // contains audio data

// Markers type
#define MARKER_SOS   0xDA
#define MARKER_APP0  0xE0
#define MARKER_APP1  0xE1
#define MARKER_APP2  0xE2
#define MARKER_COM   0xFE

#define MARKER_RST0  0xD0
#define MARKER_RST7  0xD7

#define MARKER_SOI   0xD8
#define MARKER_EOI   0xD9

// Flags for L_WriteMetaData
#define METADATA_COMMENTS  0x0001   // Write comments
#define METADATA_TAGS      0x0002   // Write tags
#define METADATA_MARKERS   0x0004   // Write markers
#define METADATA_GEOKEYS   0x0008   // Write GeoKeys
#define METADATA_ALL       0xFFFF   // Write all metadata

// Flags for L_LoadFile
#define LOADFILE_ALLOCATE           0x00000001  // Allocate image memory
#define LOADFILE_STORE              0x00000002  // Auto-store image lines
#define LOADFILE_FIXEDPALETTE       0x00000004  // Allow fixed palettes only
#define LOADFILE_NOINTERLACE        0x00000008  // Don't send interlaced lines
#define LOADFILE_ALLPAGES           0x00000010  // Load all file pages
#define LOADFILE_NOINITBITMAP       0x00000020  // Don't initialize the bitmap handle
#define LOADFILE_COMPRESSED         0x00000040  // Allow compressed 1 bit images
#define LOADFILE_SUPERCOMPRESSED    0x00000080  // Load 1-bit or 24-bit images supercompressed
#define LOADFILE_TILED              0x00000200  // Create tiled bitmap first
#define LOADFILE_NOTILED            0x00000400  // Do not use tiled bitmaps
#define LOADFILE_DISK               0x00000800  // Use Disk (if possible)
#define LOADFILE_NODISK             0x00001000  // Do not use disk

// Flags for L_SaveFile
#define SAVEFILE_FIXEDPALETTE       0x00000001  // Save with Fixed Palette
#define SAVEFILE_OPTIMIZEDPALETTE   0x00000002  // Save with Bitmap's Palette
#define SAVEFILE_MULTIPAGE          0x00000004  // Save as multipage
#define SAVEFILE_GRAYOUTPUT         0x00000008  // Save output bitmap as grayscale

// Preset Q factors (for LEAD CMP only)
#define PQ1    -1    // Perfect quality option 1
#define PQ2    -2    // Perfect quality option 2
#define QFS    -3    // Quality far more important than size
#define QMS    -4    // Quality more important than size
#define QS     -5    // Quality and size are equally important
#define SQS    -6    // Size more important than quality -Sharp
#define SQT    -7    // Size more important than quality - Less Tilling
#define MCQ    -8    // Max Compression, keeping quality as good as possible
#define MC     -9    // Max compression
#define CMP_CUSTOM_QUALITY_FACTOR (MC - 1)

// FILE_ABC and FILE_TIF_ABC quality factor
#define ABCQ_LOSSLESS         0  // Lossless compression. This option compresses a 1-bit file and
                                 // maintains image data unchanged
                                 // Highest quality
#define ABCQ_VIRTUALLOSSLESS  1  // Lossy compression. This option removes image noisy pixels and
                                 // compresses it at a smaller file size than ABCQ_LOSSLESS
#define ABCQ_REMOVEBORDER     2  // Lossy compression. This option removes image border if it exists
                                 // and compresses it at a smaller file size than ABCQ_VIRTUALLOSSLESS
#define ABCQ_ENHANCE          3  // Lossy compression
                                 // This option cleans up the image, removes its border if it exists and
                                 // compresses it at a smaller file size than ABCQ_REMOVEBORDER
#define ABCQ_MODIFIED1        4  // Lossy compression. This option cleans up the image, removes its
                                 // border if it exists and compresses it at a smaller file size than ABCQ_ENHANCE
                                 // However, it may distort some text or straight lines
#define ABCQ_MODIFIED1_FAST   5  // Same as ABCQ_MODIFIED1 without the border remove or image clean processes
#define ABCQ_MODIFIED2        6  // Lossy compression. This option cleans up the image, removes its border
                                 // if it exists and compresses it at a smaller file size than ABCQ_MODIFIED1
                                 // However, it may distort some text or straight lines
#define ABCQ_MODIFIED2_FAST   7  // Same as ABCQ_MODIFIED2 without the border remove or image clean processes
#define ABCQ_MODIFIED3        8  // Lossy compression.  Provdes faster encode and decode speed than ABCQ_MODIFIED1
                                 // or ABCQ_MODIFIED2. Provides the maximum compression
#define ABCQ_MODIFIED3_FAST   9  // Same as ABCQ_MODIFIED3 without the border remove or image clean processes
#define ABCQ_LOSSLESS_FAST    10 // Faster lossless compression than ABCQ_LOSSLESS. This option compresses a 1-bit
                                 // file and maintains image data unchanged.  Highest quality.  Does not produce
                                 // compression ratios as high as ABCQ_LOSSLESS
#define ABCQ_LOSSY_FAST       11 // Lossy compression.  This is the fastest ABC lossy compression option.  Does
                                 // not produce compression ratios as high as the other lossy options

// Flags for L_SETLOADINFOCALLBACK
#define LOADINFO_TOPLEFT         0x00000001  // Image has TOP_LEFT View Perspective
#define LOADINFO_ORDERRGB        0x00000002  // Image has ORDERRGB Color Order
#define LOADINFO_WHITEONBLACK    0x00000004  // Image is white-on-black
#define LOADINFO_LSB             0x00000008  // Image is Least Significant Bit first fill order
#define LOADINFO_TOPLEFT90       0x00000010  // Image has TOP_LEFT90 View Perspective
#define LOADINFO_TOPLEFT270      0x00000020  // Image has TOP_LEFT270 View Perspective
#define LOADINFO_REVERSE         0x00000040  // Reverse (mirror) each line

#define LOADINFO_TOPLEFT180      0x00000080  // Image has TOP_LEFT180 View Perspective
#define LOADINFO_BOTTOMLEFT90    0x00000100  // Image has BOTTOM_LEFT90 View Perspective
#define LOADINFO_BOTTOMLEFT180   0x00000200  // Image has BOTTOM_LEFT180 View Perspective
#define LOADINFO_BOTTOMLEFT270   0x00000400  // Image has BOTTOM_LEFT270 View Perspective
#define LOADINFO_PAD4            0x00000800  // Each line is padded to a multiple of 4 bytes (raw data only)

#define LOADINFO_PALETTE         0x00001000  // For RAW data of 8 bpp or less, a palette is supplied in rgbQuad of LOADINF

#define LOADINFO_BITFIELDS       0x00002000  // For RAW BITFIELDS -- 3 color masks are specified in rgbColorMask of LOADINFO
#define LOADINFO_ORDERGRAY       0x00004000  // Image is grayscale
#define LOADINFO_MOTOROLAORDER   0x00008000  // Image bytes are in Motorola byte order (valid only for 16, 48 and 64-bit)
#define LOADINFO_ORDERROMM       0x00010000  // Image is ROMM
#define LOADINFO_SIGNED          0x00020000  // unsigned raw data

// Flags that make up FILEINFO.Flags
#define FILEINFO_INTERLACED            0x00000001
#define FILEINFO_PROGRESSIVE           0x00000002  // progressive JPEG file
#define FILEINFO_HAS_STAMP             0x00000004
#define FILEINFO_HAS_GLOBALBACKGROUND  0x00000008
#define FILEINFO_HAS_GLOBALPALETTE     0x00000010
#define FILEINFO_HAS_GLOBALLOOP        0x00000020
#define FILEINFO_COMPRESSED            0x00000040  // the image can be loaded compressed
#define FILEINFO_NOPALETTE             0x00000080  // Grayscale TIF without a palette
#define FILEINFO_ROTATED               0x00000100  // Image with an extended ViewPerspective
#define FILEINFO_SIGNED                0x00000200  // Image with signed values for pixels
#define FILEINFO_LOSSLESSJPEG          0x00000400  // Internal flag, indicating that the JPEG file uses the lossless compression
                                                   // This flag will probably be removed in future versions
#define FILEINFO_HAS_ALPHA             0x00000800  // The file has alpha channel information
#define FILEINFO_FORMATVALID           0x00001000  // Only the format type is valid
#define FILEINFO_INFOVALID             0x00002000  // The whole FILEINFO structure is valid
#define FILEINFO_LINK                  0x00004000  // The file is a Windows 9x/NT link
#define FILEINFO_IFDVALID              0x00008000  // The IFD field is valid
#define FILEINFO_USELOADINFO           0x00010000  // Internal--do not use
#define FILEINFO_NO_RESOLUTION         0x00020000  // The file does not contain resolution information

// Values for FILEINFO.ColorSpace
#define COLORSPACE_BGR     0
#define COLORSPACE_YUV     1  // same as YCbCr
#define COLORSPACE_CMYK    2
#define COLORSPACE_CIELAB  3

// Flags for L_FileInfo (not to make up FILEINFO.Flags!)
#define FILEINFO_TOTALPAGES   0x00000001  // fill in FILEINFO.TotalPages

// Flags for the overlay callback functions
#define OVERLAY_LOADCALL   0x0001   // First try loading the overlay. call overlay callback if the load failed
#define OVERLAY_CALLLOAD   0x0002   // First call the overlay callback. Try loading the overlay file from disk
                                    // if the callback did not supply an overlay bitmap
#define OVERLAY_CALLONLY   0x0003   // Call the overlay callback. If there is no callback, there is no overlay
#define OVERLAY_LOADONLY   0x0004   // Attempt to load the overlay bitmap from disk. If the call fails, there is no overlay

// STARTDECOMPRESSDATA flags
#define DECOMPRESS_LSB              LOADINFO_LSB
#define DECOMPRESS_PAD4             LOADINFO_PAD4
#define DECOMPRESS_PALETTE          LOADINFO_PALETTE

#define DECOMPRESS_STRIPS           0  //compressed strip contains complete rows
#define DECOMPRESS_TILES            2

// Flags to indicate if beginning, end of a strip
#define DECOMPRESS_CHUNK_START      1
#define DECOMPRESS_CHUNK_END        2
#define DECOMPRESS_CHUNK_COMPLETE   (DECOMPRESS_CHUNK_START | DECOMPRESS_CHUNK_END),

// New flags for the LOADFILEOPTION structure
#define ELO_REVERSEBITS                0x00000001
#define ELO_GLOBALBACKGROUND           0x00000002
#define ELO_GLOBALPALETTE              0x00000004
#define ELO_GLOBALLOOP                 0x00000008
#define ELO_ROTATED                    0x00000010   // Load files with extended ViewPerspective - do not rotate them
#define ELO_IGNOREVIEWTRANSFORMS       0x00000020   // Load the image without the viewing transformations
#define ELO_IGNORECOLORTRANSFORMS      0x00000040   // Load the image without the color transformations
#define ELO_SIGNED                     0x00000080   // Load images with signed pixels without conversion
#define ELO_DISABLEMMX                 0x00000100   // (JPEG only) Do not use MMX optimized code
#define ELO_DISABLEP3                  0x00000200   // (JPEG only) Do not use P3-specific optimized code
#define ELO_USEIFD                     0x00000400   // (TIFF only) Use the IFD offset
#define ELO_FORCECIELAB                0x00000800   // (JPEG only) The file has CIELAB colorspace
#define ELO_USEBADJPEGPREDICTOR        0x00001000   // (JPEG only) Load lossless JPEG file using an incorrect predictor
#define ELO_IGNOREPHOTOMETRICINTERP    0x00002000   // (TIFF only) Use a default colorspace for the compression and ignore
                                                   // the value of the PhotometricInterpretation tag:
                                                   // For TIFF JPEG files:
                                                   // Use YUV colorspace for 1-3 samples per pixel, CMYK for 4 samples per pixel
                                                   // All other TIFF files:
                                                   //    Use RGB colorspace. Use this to disable the color conversion to RGB during the load process
#define ELO_FORCERGBFILE               0x00004000   // (JPEG only) The file has RGB colorspace.
#define ELO_MULTISPECTRALSCAN          0x00008000   // Use uXXXScan values info when loading NITF files
#define ELO_LOADCORRUPTED              0x00010000   // Attempt to load corrupted files
#define ELO_FORCE_EPS_THUMBNAIL        0x00020000   // (EPS only) Force loading of raster thumbnail data from postscript files
#define ELO_NITF_USE_MAX               0x00040000   // (NITF only) Use the maximum width and height
#define ELO_NITF_USE_MONODARK          0x00080000   // (NITF only) Use the mono dark process (to brightness the result image)
#define ELO_NITF_SHOW_OBJECT           0x00100000   // (NITF only) Make the object CGM available if exists
#define ELO_IGNOREVIEWPERSPECTIVE      0x00200000   // (TIF and Exif only) Ignore the view perspective stored in the file
#define ELO_USEFASTCONVERSION          0x00400000   // (TIF and CMP only) Use fast color conversion for CMYK and CieLAB
#define ELO_FAST                       0x00800000   // Use fast load. used by ABS filter
#define ELO_ALPHAINIT                  0x01000000   // Fill the alpha channel with 1's (0xFF)
#define ELO_COLOR_COMPONENT_ONLY       0x02000000   // (JPX only) When loading a frame, load only the color component
#define ELO_IGNORE_ADOBE_COLOR_TRANSFORM 0x04000000   // Ignore the Adobe marker containing color transformations (APPE)
#define ELO_ALLOW13BITLZWCODE          0x08000000   // Try to decode buggy LZW TIF files that contain 13-bit LZW codes
#define ELO_VECTOR_CONVERTED_UNITS     0x10000000   // Use converted units, instead of the input file's default

// Flags for the SAVEFILEOPTION structure
#define ESO_REVERSEBITS                      0x00000001
#define ESO_NOSUBFILETYPE                    0x00000002
#define ESO_GLOBALBACKGROUND                 0x00000004
#define ESO_GLOBALPALETTE                    0x00000008
#define ESO_INTERLACED                       0x00000010
#define ESO_GLOBALLOOP                       0x00000020
#define ESO_NOPALETTE                        0x00000040  // save grayscale TIF without a palette
#define ESO_SAVEWITHSTAMP                    0x00000080  // save a stamp if format supports it
#define ESO_FIXEDPALETTESTAMP                0x00000100  // save fixed palette stamps
#define ESO_YCCSTAMP                         0x00000200  // save YCbCr stamps
#define ESO_REPLACEPAGE                      0x00000400  // Replace the page specified by PageNumber
#define ESO_INSERTPAGE                       0x00000800  // Insert image before the page specified by PageNumber
#define ESO_JPEGSTAMP                        0x00001000  // save JPEG compressed stamps
#define ESO_DISABLEMMX                       0x00002000  // Do not use MMX optimized code
#define ESO_SAVEOLDJTIF                      0x00004000  // Write old style JTIF files
#define ESO_NOPAGENUMBER                     0x00008000  // Do not save/update the PageNumber tag
#define ESO_DISABLEP3                        0x00010000  // Do not use P3-specific optimized code
#define ESO_USEIFD                           0x00020000  // Use the IFD offset
#define ESO_MOTOROLAORDER                    0x00040000  // When possible, save files in Motorola byte order
#define ESO_WITHOUTTIMESTAMP                 0x00080000  // save without time stamp*/ 

#define ESO_PDF_TEXT_ENCODING_NONE           0x00000000  // PDF, no text encoding
#define ESO_PDF_TEXT_ENCODING_ASCII_BASE85   0x00100000  // PDF, ASCII BASE85 text encoding
#define ESO_PDF_TEXT_ENCODING_ASCII_HEX      0x00200000  // PDF, ASCII HEX text encoding

#define ESO_PDF_TEXT_ENCODING_MASK           0x00300000  // mask of the flags used for PDF text encoding
#define ESO_PAD4                             0x00400000  // Each line is padded to a multiple of 4 bytes (raw data only)
#define ESO_PLT_BEZIER_CURVES                0x00800000  // PLT filter should save bezier curves
#define ESO_PDF_SAVE_USE_BITMAP_DPI          0x01000000  // Use bitmap  DPI in calculating page dimensions when saving a PDF file
#define ESO_PHOTOMETRICINTERPRETATIONVALID   0x02000000
#define ESO_TILEINFOVALID                    0x04000000  // Use TileWidth and TileHeight when saving TIFF files
#define ESO_USEDITHERINGMETHOD               0x08000000  // Use the DitheringMethod in the BITMAPHANDLE
#define ESO_PRESERVEPALETTE                  0x10000000  // Preserve the palette when saving TIFF CCITT files (might decrease compression ratio)
#define ESO_PDF_SAVE_LOW_MEMORY_USAGE        0x20000000  // Try to use less memory when creating a PDF file with JPEG or Fax compression
#define ESO_GENERATEGLOBALPALETTE            0x40000000  // Generate global palette when saving GIF files
#define ESO_USEPREDICTOR                     0x80000000  // Use a predictor when saving LZW data

// Flags making up SAVEFILEOPTION.Flags2
#define ESO2_NITF                            0x00000001  // Save the images compatible with NITF requirements
#define ESO2_SAVEPLANAR                      0x00000002  // Save the TIF CMYK images as planar (instead of chunky)
#define ESO2_NOLZWAUTOCLEAR                  0x00000004  // Do not automatically insert CLEAR codes during LZW compression
#define ESO2_ALPHAINIT                       0x00000010  // Fill the alpha channel with 1's (0xFF)
#define ESO2_ANNDOTNETPROPS                  0x00000020  // For internal use only
#define ESO2_J2K_DISABLE_8BITS               0x00000040  // For internal use only
#define ESO2_XPS_SAVE_USE_BITMAP_DPI         0x00000080  // Use the bitmap's DPI when saving to XPS
#define ESO2_PDFA_PROFILE                    0x00000100  // When saving as PDF, save PDF files as PDF/A
#define ESO2_PDF_V14                         0x00000200  // When saving as PDF, save PDF files as PDF 1.4
#define ESO2_PDF_V15                         0x00000400  // When saving as PDF, save PDF files as PDF 1.5
#define ESO2_ENDWITH3EOL                     0x00000800  // End Fax G3 (1D/2D) files with 3 EOLs (00 01 00 01 00 01) instead of EOL EOFB (00 01 00 10 01)
#if defined(LEADTOOLS_V16_OR_LATER)
#define ESO2_OPTIMIZEDHUFFMAN                0x00001000  // Save JPEG files with optimized Huffman tables
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

// Values that can be used for LOADFILEOPTION.Passes. These values will
// generate default Progressive JPEG/CMP files (usually 10 passes/file) on 
// save and specifies how many passes to generate while decoding 
// Progressive JPEG/CMP files.
#define CALLBACK_ONCE               0  // callback just once at the end
#define CALLBACK_WHEN_MEANINGFUL   -1 // callback with only significant scans
#define CALLBACK_ALWAYS            -2 // callback with all scans

// Flags for the FILEREADCALLBACK function
#define FILEREAD_FIRSTPASS    0x0001
#define FILEREAD_LASTPASS     0x0002
#define FILEREAD_FIRSTROW     0x0004
#define FILEREAD_LASTROW      0x0008
#define FILEREAD_COMPRESSED   0x0010
#define FILEREAD_CLIPVERT     0x0020   // internal flag - data contains only the requested lines
#define FILEREAD_CLIPHORZ     0x0040   // internal flag - data contains only the requested columns

#define LOADCUSTOMFILEOPTION_FLIPTILES    0x0001

// J2K/new CMW Options
// Limitations imposed by the standard
#define J2K_MAX_COMPONENTS_NUM                     3
#define J2K_MAX_DECOMP_LEVEL                       20

// Progressions Order
#define J2K_LAYER_RESOLUTION_COMPONENT_POSITION    0
#define J2K_RESOLUTION_LAYER_COMPONENT_POSITION    1
#define J2K_RESOLUTION_POSITION_COMPONENT_LAYER    2
#define J2K_POSITION_COMPONENT_RESOLUTION_LAYER    3
#define J2K_COMPONENT_POSITION_RESOLUTION_LAYER    4

// Photo cd
#define L_PCD_BASE_OVER_64    0  // 64 x   96
#define L_PCD_BASE_OVER_16    1  // 128 x  192
#define L_PCD_BASE_OVER_4     2  // 256 x  384
#define L_PCD_BASE            3  // 512 x  768
#define L_PCD_4BASE           4  // 1024 x 1536
#define L_PCD_16BASE          5  // 2048 x 3072

// L_2DSetViewMode flags
#define L2D_USE_BEST          0
#define L2D_USE_WIDTH_HEIGHT  1
#define L2D_USE_WIDTH         2
#define L2D_USE_HEIGHT        3

// Lossless rotate functions and defines
#define FILE_TRANSFORM_FLIP         0x0001
#define FILE_TRANSFORM_REVERSE      0x0002

#define FILE_TRANSFORM_ROTATE90     0x0004
#define FILE_TRANSFORM_ROTATE180    0x0008
#define FILE_TRANSFORM_ROTATE270    0x000C
#define FILE_TRANSFORM_ROTATEMASK   0x000C

// PDF Options Flags
#define PDF_DISABLE_CROPPING     0x00000001
#define PDF_DISABLE_CIECOLORS    0x00000010

// FILTERINFO flags
#define FILTERINFO_IGNORED             0x0001
#define FILTERINFO_FIXED               0x0002
#define FILTERINFO_DYNAMIC             0x0000
#define FILTERINFO_LOADMASK            0x0003

#define FILTERINFO_PRESENT             0x0004
#define FILTERINFO_CHECKEDBYFILEINFO   0x0008
#define FILTERINFO_SLOWFILEINFO        0x0010

#define FILTERINFO_FREEALL             0x0020

#define JBIG2_REMOVE_MARKER               0x0001
#define JBIG2_REMOVE_HEADER_SEGMENT       0x0002
#define JBIG2_REMOVE_EOP_SEGMENT          0x0004
#define JBIG2_REMOVE_EOF_SEGMENT          0x0008
#define JBIG2_IMAGE_TPON                  0x0010
#define JBIG2_ENABLE_DICTIONARY           0x0100
#define JBIG2_TEXT_REMOVEUNREPEATEDSYM    0x1000
#define JBIG2_TEXT_KEEPALLSYM             0x2000

#define FLAG_JBIG2_REMOVE_MARKER          JBIG2_REMOVE_MARKER
#define FLAG_JBIG2_REMOVE_HEADER_SEGMENT  JBIG2_REMOVE_HEADER_SEGMENT
#define FLAG_JBIG2_REMOVE_EOP_SEGMENT     JBIG2_REMOVE_EOP_SEGMENT
#define FLAG_JBIG2_REMOVE_EOF_SEGMENT     JBIG2_REMOVE_EOF_SEGMENT
#define FLAG_JBIG2_TPON                   JBIG2_IMAGE_TPON

#if !defined(LEAD_DEFINES_ONLY)

typedef enum
{
   PME_MINISWHITE = 0,  // min value is white
   PME_MINISBLACK = 1,  // min value is black
   PME_RGB        = 2,  // RGB color model
   PME_PALETTE    = 3,  // color map indexed
   PME_MASK       = 4,  // $holdout mask
   PME_SEPARATED  = 5,  // !color separations
   PME_YCBCR      = 6,  // !CCIR 601
   PME_CIELAB     = 8,  // !1976 CIE L*a*b*
}
PHOTMTRICINTERP, *pPHOTMTRICINTERP;

#define FILEPDFOPTIONS_MAX_PASSWORD_LEN   64

// Flags for PDF Encryption
// Printing
#define PDF_SECURITYFLAGS_REV2_PRINTDOCUMENT          0x00000004
#define PDF_SECURITYFLAGS_REV3_PRINTFAITHFUL          0x00000800  // 128 bit only

// Editing
#define PDF_SECURITYFLAGS_REV2_MODIFYDOCUMENT         0x00000008

// Copying
#define PDF_SECURITYFLAGS_REV2_EXTRACTTEXT            0x00000010
#define PDF_SECURITYFLAGS_REV3_EXTRACTTEXTGRAPHICS    0x00000200  // 128 bit only

// Annotation
#define PDF_SECURITYFLAGS_REV2_MODIFYANNOTATION       0x00000020

// Form filling
#define PDF_SECURITYFLAGS_REV3_FILLFORM               0x00000100

// Assembly:
#define PDF_SECURITYFLAGS_REV3_ASSEMBLEDOCUMENT       0x00000400  // 128 bit only

// Ways of controlling JPEG2000 the compression
typedef enum _J2KCOMPRESSIONCONTROL
{
   J2K_COMPRESSION_LOSSLESS   = 0,  // lossless compression
   J2K_COMPRESSION_RATIO      = 1,  // use fCompressionRatio
   J2K_COMPRESSION_TARGETSIZE = 2,  // use uTargetFileSize
   J2K_COMPRESSION_QFACTOR    = 3,  // use qFactor
}
J2KCOMPRESSIONCONTROL, *pJ2KCOMPRESSIONCONTROL;

// Ways of how to set the JPEG2000 Region Of Interest
typedef enum _J2KREGIONOFINTEREST
{
   J2K_USE_LEAD_REGION  = 0,  // Use LEAD bitmap region
   J2K_USE_OPTION_RECT  = 1,  // Use rcROI member of FILEJ2KOPTIONS
}
J2KREGIONOFINTEREST, *pJ2KREGIONOFINTEREST;

#if defined(LEADTOOLS_V16_OR_LATER)
typedef enum
{
   J2KPRECINCTSIZE_FULL             =  0,
   J2KPRECINCTSIZE_UNIFORM_64       =  1,
   J2KPRECINCTSIZE_UNIFORM_128      =  2,
   J2KPRECINCTSIZE_UNIFORM_256      =  3,
   J2KPRECINCTSIZE_UNIFORM_512      =  4,
   J2KPRECINCTSIZE_UNIFORM_1024     =  5,
   J2KPRECINCTSIZE_UNIFORM_2048     =  6,
   J2KPRECINCTSIZE_HIERARCHICAL1_64  =  7,
   J2KPRECINCTSIZE_HIERARCHICAL1_128 =  8,
   J2KPRECINCTSIZE_HIERARCHICAL1_256 =  9,
   J2KPRECINCTSIZE_HIERARCHICAL1_512 = 10,
   J2KPRECINCTSIZE_HIERARCHICAL2_64  = 11,
   J2KPRECINCTSIZE_HIERARCHICAL2_128 = 12,
   J2KPRECINCTSIZE_HIERARCHICAL2_256 = 13,
   J2KPRECINCTSIZE_HIERARCHICAL2_512 = 14,
}
J2KPRECINCTSIZE, *pJ2KJ2KPRECINCTSIZE;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)


#if !defined(FOR_WINCE)
#define AUTOCADFILES_COLORSCHEME_BLACKONWHITE   0
#define AUTOCADFILES_COLORSCHEME_WHITEONBLACK   1
#endif // #if !defined(FOR_WINCE)

#if defined(LEADTOOLS_V16_OR_LATER)
#if !defined(FOR_WINCE)
typedef enum _CHANNELTYPE
{
   ALPHA_CHANNEL = 0,
   RED_CHANNEL,
   GREEN_CHANNEL,
   BLUE_CHANNEL,
   CYAN_CHANNEL,
   MAGENTA_CHANNEL,
   YELLOW_CHANNEL,
   KEY_CHANNEL,
   GRAY_CHANNEL,
   BITMAP_CHANNEL,
   LIGHTNESS_CHANNEL,
   A_CHANNEL,
   B_CHANNEL
} CHANNELTYPE;
#endif // #if !defined(FOR_WINCE)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

typedef enum
{
   // Page width, height and margins are in pixels
   RASTERIZEDOC_UNIT_PIXEL,
   // Page width, height and margins are in inches
   RASTERIZEDOC_UNIT_INCH,
   // Page width, height and margins are in millimeters
   RASTERIZEDOC_UNIT_MILLIMETER
}
RASTERIZEDOC_UNIT;

typedef enum
{
   // Default mode, use original size
   RASTERIZEDOC_SIZEMODE_NONE,
   // Fit in page width and height keeping the aspect ratio
   RASTERIZEDOC_SIZEMODE_FIT,
   // Like FIT and if the original size is smaller, make it bigger
    RASTERIZEDOC_SIZEMODE_FIT_ALWAYS,
   // Make the image width page width and calculate height using aspect ratio
   RASTERIZEDOC_SIZEMODE_FIT_WIDTH,
   // Stretch the image to be exactly page width and height
   RASTERIZEDOC_SIZEMODE_STRETCH
}
RASTERIZEDOC_SIZEMODE;

// Use paper dimensions obtained from user to draw the worksheet as following if the
// sheet dimension is larger than the paper dimensions obtained from user then the
// sheet will be divided to multiple pages each of which has the same page dimension
// obtained form user (the sheet will be rendered as multiple of sheets). If the sheet
// size is leass than page dimensions obtained from user then it will be drawn on
// single page.
// If this flag is not set, then use paper dimensions obtained from user to draw the
// worksheet at if the size of paper dimension is less than the sheet size then the sheet is cropped.
#define XLS_FLAGS_MULTIPAGE_SHEET   0x00000001

// Flags for L_ReadFileTags, L_ReadFileGeoKeys and L_ReadFileComments
#define READFILEMETADATA_NOMEMORY   0x01

/****************************************************************
   Macros
****************************************************************/

#define GET_TAG_SIZE(pTagSize, uType, ulCount) \
   switch(uType)                    \
   {                                \
   case TAG_BYTE:                   \
   case TAG_ASCII:                  \
   case TAG_SBYTE:                  \
   case TAG_UNDEFINED:              \
      *(pTagSize) = 1 * (ulCount);  \
      break;                        \
   case TAG_SHORT:                  \
   case TAG_SSHORT:                 \
      *(pTagSize) = 2 * (ulCount);  \
      break;                        \
   case TAG_LONG:                   \
   case TAG_SLONG:                  \
   case TAG_FLOAT:                  \
      *(pTagSize) = 4 * (ulCount);  \
      break;                        \
   case TAG_RATIONAL:               \
   case TAG_SRATIONAL:              \
   case TAG_DOUBLE:                 \
      *(pTagSize) = 8 * (ulCount);  \
      break;                        \
   default:                         \
      *(pTagSize) = 0;              \
      break;                        \
   }

// Helper macros for dealing with rotated bitmap info view perspectives
#define INFOROTATED(pInfo) ((pInfo)->ViewPerspective == TOP_LEFT90 || (pInfo)->ViewPerspective == TOP_LEFT270 || (pInfo)->ViewPerspective == BOTTOM_LEFT90 || (pInfo)->ViewPerspective == BOTTOM_LEFT270)
#define INFOBASIC(pInfo) ((pInfo)->ViewPerspective == TOP_LEFT || (pInfo)->ViewPerspective == BOTTOM_LEFT)
#define INFOWIDTH(pInfo) (INFOROTATED(pInfo)?(pInfo)->Height:(pInfo)->Width)
#define INFOHEIGHT(pInfo) (INFOROTATED(pInfo)?(pInfo)->Width:(pInfo)->Height)

/****************************************************************
   Forward declerations
****************************************************************/
typedef struct _J2KVIDEOINFO J2KVIDEOINFO; typedef J2KVIDEOINFO* pJ2KVIDEOINFO;
typedef struct _LOADINFO LOADINFO; typedef LOADINFO* pLOADINFO;
typedef struct _FILEINFOA FILEINFOA; typedef FILEINFOA* pFILEINFOA;
#if defined(FOR_UNICODE)
typedef struct _FILEINFO FILEINFO; typedef FILEINFO* pFILEINFO;
#endif // #if defined(FOR_UNICODE)
typedef struct _CUSTOMTILEINFO CUSTOMTILEINFO; typedef CUSTOMTILEINFO* pCUSTOMTILEINFO;
typedef struct _FILEOVERLAYCALLBACKDATAA FILEOVERLAYCALLBACKDATAA; typedef FILEOVERLAYCALLBACKDATAA* pFILEOVERLAYCALLBACKDATAA;
#if defined(FOR_UNICODE)
typedef struct _FILEOVERLAYCALLBACKDATA FILEOVERLAYCALLBACKDATA; typedef FILEOVERLAYCALLBACKDATA* pFILEOVERLAYCALLBACKDATA;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Callback prototypes
****************************************************************/
typedef L_INT (pEXT_CALLBACK FLTJ2KDECOMPRESSFRAME)(L_UCHAR* pInData , L_UCHAR* pOutData, pJ2KVIDEOINFO pJ2KInfo);

typedef L_INT (pEXT_CALLBACK COMPBUFFCALLBACK)(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_SIZE_T uBytes,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK LOADINFOCALLBACK)(
   L_HFILE hFile,
   pLOADINFO pInfo,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK FILEREADCALLBACKA)(
   pFILEINFOA pFileInfo,
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_UINT uFlags,
   L_INT nRow,
   L_INT nLines,
   L_VOID* pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK FILEREADCALLBACK)(
   pFILEINFO pFileInfo,
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_UINT uFlags,
   L_INT nRow,
   L_INT nLines,
   L_VOID* pUserData);
#else
typedef FILEREADCALLBACKA FILEREADCALLBACK;
#endif // #if defined(FOR_UNICODE)

typedef L_INT (pEXT_CALLBACK FILESAVECALLBACK)(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_UINT uRow,
   L_UINT uLines,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK SAVECUSTOMFILECALLBACK)(
   pCUSTOMTILEINFO pTileInfo,
   L_UCHAR* pUncompressedBuffer,
   L_SIZE_T UncompressedBufferSize,
   L_UCHAR* pCompressedBuffer,
   L_SIZE_T CompressedBufferSize ,
   L_SIZE_T* puActualBytesCompressed,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK LEADMARKERCALLBACK)(
   L_UINT uMarker,
   L_UINT uMarkerSize,
   L_UCHAR* pMarkerData,
   L_VOID* pLEADUserData);

typedef L_INT (pEXT_CALLBACK TRANSFORMFILECALLBACK)(
   L_UINT uMarker,
   L_UINT uMarkerSize,
   L_UCHAR* pMarkerData,
   L_VOID* pUserData,
   L_UINT uTransform,
   LEADMARKERCALLBACK pfnLEADCallback,
   L_VOID* pLEADUserData);

typedef L_INT (pEXT_CALLBACK LOADCUSTOMFILECALLBACK)(
   L_UCHAR* pCompressedTileBuffer,
   L_SIZE_T uCompressedBufferSize,
   L_UCHAR* pDeCompressedTileBuffer,
   L_SIZE_T uDeCompressedBufferSize,
   pCUSTOMTILEINFO pTileInfo,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK SAVEBUFFERCALLBACK)(
   L_SIZE_T uRequiredSize,
   L_UCHAR** ppBuffer,
   L_SIZE_T* pdwBufferSize,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK ENUMMARKERSCALLBACK)(
   L_UINT uMarker,
   L_UINT uMarkerSize,
   L_UCHAR* pMarkerData,
   L_VOID* pUserData,
   LEADMARKERCALLBACK pfnLEADCallback,
   L_VOID* pLEADUserData);

typedef L_INT (pEXT_CALLBACK ENUMTAGSCALLBACK)(
   L_UINT16 uTag,
   L_UINT16 uType,
   L_UINT uCount,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK ENUMGEOKEYSCALLBACK)(
   L_UINT16 uTag,
   L_UINT16 uType,
   L_UINT uCount,
   L_VOID* pData,
   L_VOID* pUserData);

typedef L_INT (pEXT_FUNCTION FLTLOADBUFFER)(
   L_UCHAR* pInput,
   L_SIZE_T nLength,
   L_UCHAR* pOutput,
   L_INT nFormat,
   L_INT32 nWidth,
   L_INT32 nHeight,
   L_UINT nBitsPerPixel,
   L_VOID* pLeadHdr,
   L_UINT uFlags);

typedef L_INT (pEXT_FUNCTION FLTSAVEBUFFER)(
   L_UCHAR* pInput,
   L_BITMAPINFOHEADER* pbiInput,
   L_UCHAR* pOutput,
   L_SIZE_T* pdwSize,
   L_INT nQFactor,
   L_UINT uFlags,
   L_UINT nFormat,
   L_UCHAR* pTopBuffer,
   L_INT nTopBufferHeight);

typedef L_INT (pEXT_CALLBACK BITMAPLOADCALLBACK)(
   L_UCHAR* pBuffer,
   L_SIZE_T uSize,
   L_VOID* pUserData);

#if !defined(FOR_WINCE)
typedef L_INT (pEXT_CALLBACK VECTORFONTMAPPERCALLBACK)(
   pVECTORHANDLE pVector,
   LPLOGFONTW pLogFont,
   L_VOID* pUserData);
#endif // #if !defined(FOR_WINCE)

typedef L_INT (pEXT_CALLBACK OVERLAYCALLBACKA)(
   pFILEOVERLAYCALLBACKDATAA pOverlayCallbackData,
   L_VOID* pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK OVERLAYCALLBACK)(
   pFILEOVERLAYCALLBACKDATA pOverlayCallbackData,
   L_VOID* pUserData);
#else
typedef OVERLAYCALLBACKA OVERLAYCALLBACK;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Classes/structures
****************************************************************/

// LOADINFO structure
typedef struct _LOADINFO
{
   L_UINT uStructSize;        // use sizeof(LOADINFO)
   L_INT Format;              // File format: FILE_PCX, ...
   L_INT Width;               // Image width
   L_INT Height;              // Image height
   L_INT BitsPerPixel;        // Bits per pixel
   L_INT XResolution;         // X resolution (DPI)
   L_INT YResolution;         // Y resolution (DPI)
   L_SIZE_T Offset;           // Data offset
   L_UINT Flags;              // Special flags

   // for RAW file filter
   L_RGBQUAD rgbQuad[256];

   // for RAW BITFIELDS - contains R,G,B color masks
   L_UINT rgbColorMask[3];

   // for RAW_PACKBITS
   L_UINT32 uStripSize;       // size of strip after compression
   L_INT nPhotoInt;           // TIFF tag  0-6  TIFF tag
   L_INT nPlanarConfig;       // TIFF tag  1 = Chunky,  2 = Planar format
}
LOADINFO, *pLOADINFO;

typedef struct _FILEINFOA
{
   L_UINT uStructSize;           // use sizeof(FILEINFO)
   L_INT Format;                 // File format: FILE_PCX, ...
   L_CHAR Name[_MAX_FNAME+_MAX_EXT];// File name, including the Ext
   L_INT Width;                  // Image width
   L_INT Height;                 // Image height
   L_INT BitsPerPixel;           // Bits per pixel
   L_SSIZE_T SizeDisk;           // Size of file on disk
   L_SSIZE_T SizeMem;            // Size of image in memory
   L_CHAR Compression[20];      // Compression method name
   L_INT ViewPerspective;        // Image view prespective
   L_INT Order;                  // RGB order
   L_INT PageNumber;             // Page number
   L_INT TotalPages;             // Total number of pages present in the file
   L_INT XResolution;
   L_INT YResolution;
   L_UINT Flags;                 // identifies file subtypes: progressive, interlaced
   L_UINT GlobalLoop;            // Global animation loop count 0 = infinity
   L_INT GlobalWidth;            // Global width
   L_INT GlobalHeight;           // Global height
   L_COLORREF GlobalBackground;  // Global background color (see Flags)
   L_RGBQUAD GlobalPalette[256]; // Global palette (see Flags)
   L_SIZE_T IFD;                 // IFD offset (for TIF files only)
   L_INT Layers;                 // The number of layers in the file
   L_INT ColorSpace;             // The colorspace (RGB, CMYK, CIELAB, etc)
#if defined(LEADTOOLS_V16_OR_LATER)
   L_INT Channels;               // The number of channels in the file
   L_BOOL bIsDocFile;            // Is this a DOC file?
   L_DOUBLE dDocPageWidth;       // Only valid for DOC files, the width in unit
   L_DOUBLE dDocPageHeight;      // Only valid for DOC files, the height in unit
   RASTERIZEDOC_UNIT uDocUnit;   // Only valid for DOC files, dDocPageWidth and dDocPageHeight unit
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
}
FILEINFOA, *pFILEINFOA;
#if defined(FOR_UNICODE)
typedef struct _FILEINFO
{
   L_UINT uStructSize;           // use sizeof(FILEINFO)
   L_INT Format;                 // File format: FILE_PCX, ...
   L_TCHAR Name[_MAX_FNAME+_MAX_EXT];// File name, including the Ext
   L_INT Width;                  // Image width
   L_INT Height;                 // Image height
   L_INT BitsPerPixel;           // Bits per pixel
   L_SSIZE_T SizeDisk;           // Size of file on disk
   L_SSIZE_T SizeMem;            // Size of image in memory
   L_TCHAR Compression[20];      // Compression method name
   L_INT ViewPerspective;        // Image view prespective
   L_INT Order;                  // RGB order
   L_INT PageNumber;             // Page number
   L_INT TotalPages;             // Total number of pages present in the file
   L_INT XResolution;
   L_INT YResolution;
   L_UINT Flags;                 // identifies file subtypes: progressive, interlaced
   L_UINT GlobalLoop;            // Global animation loop count 0 = infinity
   L_INT GlobalWidth;            // Global width
   L_INT GlobalHeight;           // Global height
   L_COLORREF GlobalBackground;  // Global background color (see Flags)
   L_RGBQUAD GlobalPalette[256]; // Global palette (see Flags)
   L_SIZE_T IFD;                 // IFD offset (for TIF files only)
   L_INT Layers;                 // The number of layers in the file
   L_INT ColorSpace;             // The colorspace (RGB, CMYK, CIELAB, etc)
#if defined(LEADTOOLS_V16_OR_LATER)
   L_INT Channels;               // The number of channels in the file
   L_BOOL bIsDocFile;            // Is this a DOC file?
   L_DOUBLE dDocPageWidth;       // Only valid for DOC files, the width in unit
   L_DOUBLE dDocPageHeight;      // Only valid for DOC files, the height in unit
   RASTERIZEDOC_UNIT uDocUnit;   // Only valid for DOC files, the width in unit
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
}
FILEINFO, *pFILEINFO;
#else
typedef FILEINFOA FILEINFO;
typedef pFILEINFOA pFILEINFO;
#endif // #if defined(FOR_UNICODE)

typedef struct _PCDINFO
{
   L_INT resolution[6];
}
PCDINFO, *pPCDINFO;

// Structure for L_CreateThumbnailFromFile
typedef struct _THUMBOPTIONS
{
   L_UINT uStructSize;  // use sizeof(THUMBOPTIONS)
   L_INT nWidth;
   L_INT nHeight;
   L_INT nBits;
   L_UINT uCRFlags;
   L_BOOL bMaintainAspect;
   L_BOOL bForceSize;
   L_COLORREF crBackColor;
   L_BOOL bLoadStamp;
   L_BOOL bResample;
}
THUMBOPTIONS, *pTHUMBOPTIONS;

typedef struct _LOADFILEOPTION
{
   L_UINT uStructSize;  // the size of the LOADFILEOPTION - use sizeof(LOADFILEOPTION)
   L_INT XResolution;
   L_INT YResolution;
   L_UINT Flags;
   L_INT Passes;
   L_INT PageNumber;
   L_UINT GlobalLoop;
   L_SIZE_T IFD;
   L_UINT uRedScan;     // the scan index of the red channel (NITF files only)
   L_UINT uGreenScan;   // the scan index of the green channel (NITF files only)
   L_UINT uBlueScan;    // the scan index of the blue channel (NITF files only)
}
LOADFILEOPTION, *pLOADFILEOPTION;

typedef struct _SAVEFILEOPTIONA
{
   L_UINT uStructSize;   // the size of the SAVEFILEOPTION - use sizeof(SAVEFILEOPTION)
   L_INT Reserved1;
   L_INT Reserved2;
   L_UINT Flags;
   L_INT Passes;
   L_INT PageNumber;
   L_INT GlobalWidth;
   L_INT GlobalHeight;
   L_UINT GlobalLoop;
   L_COLORREF GlobalBackground;
   L_RGBQUAD GlobalPalette[256];
   L_UINT StampWidth;
   L_UINT StampHeight;
   L_UINT StampBits;
#if !defined(FOR_WINCE)
   // MPEG Specific Options -- Not set by user
   L_BOOL Constrained;
   L_BOOL FieldPic;
   L_INT FrameRate;
   L_INT FramesGOP;
   L_INT PelAspectRatio;
   L_INT ProfileID;
   L_INT LevelID;
   L_INT ChromaFormat;
   L_INT VideoFormat;
   L_INT IntraDcPrec;
   L_INT BitRate;
   L_UCHAR UserInfo[255];     // user defined data to be put in mpeg stream
   // end of the MPEG-specific options
#endif // #if !defined(FOR_WINCE)

   L_SIZE_T IFD;
   L_CHAR szPassword[255];   // password for saving encrypted files
   PHOTMTRICINTERP PhotometricInterpretation;
   L_UINT TileWidth;
   L_UINT TileHeight;
   L_UINT Flags2;             // ESO2_XXX flags
}
SAVEFILEOPTIONA, *pSAVEFILEOPTIONA;
#if defined(FOR_UNICODE)
typedef struct _SAVEFILEOPTION
{
   L_UINT uStructSize;   // the size of the SAVEFILEOPTION - use sizeof(SAVEFILEOPTION)
   L_INT Reserved1;
   L_INT Reserved2;
   L_UINT Flags;
   L_INT Passes;
   L_INT PageNumber;
   L_INT GlobalWidth;
   L_INT GlobalHeight;
   L_UINT GlobalLoop;
   L_COLORREF GlobalBackground;
   L_RGBQUAD GlobalPalette[256];
   L_UINT StampWidth;
   L_UINT StampHeight;
   L_UINT StampBits;
#if !defined(FOR_WINCE)
   // MPEG Specific Options -- Not set by user
   L_BOOL Constrained;
   L_BOOL FieldPic;
   L_INT FrameRate;
   L_INT FramesGOP;
   L_INT PelAspectRatio;
   L_INT ProfileID;
   L_INT LevelID;
   L_INT ChromaFormat;
   L_INT VideoFormat;
   L_INT IntraDcPrec;
   L_INT BitRate;
   L_UCHAR UserInfo[255];     // user defined data to be put in mpeg stream
   // end of the MPEG-specific options
#endif // #if !defined(FOR_WINCE)

   L_SIZE_T IFD;
   L_TCHAR szPassword[255];   // password for saving encrypted files
   PHOTMTRICINTERP PhotometricInterpretation;
   L_UINT TileWidth;
   L_UINT TileHeight;
   L_UINT Flags2;             // ESO2_XXX flags
}
SAVEFILEOPTION, *pSAVEFILEOPTION;
#else
typedef SAVEFILEOPTIONA SAVEFILEOPTION;
typedef pSAVEFILEOPTIONA pSAVEFILEOPTION;
#endif // #if defined(FOR_UNICODE)

typedef struct _SAVECUSTOMFILEOPTION
{
   L_UINT uStructSize;                 // Size of the SAVECUSTOMFILEOPTION
   L_UINT uFlags;                      // Custom save options flags
   L_INT nTileWidth;                   // Width of a tile
   L_INT nTileHeight;                  // Height of a tile
   L_INT nCompressionTag;              // Compression Tag
   L_INT nPlanarConfiguration;         // Planar Configuration
   L_INT nPhotoMetricInterpretation;   // Photo Metric Interpretation
}
SAVECUSTOMFILEOPTION, * pSAVECUSTOMFILEOPTION;

typedef struct _LOADCUSTOMFILEOPTION
{
   L_UINT uStructSize;  // Size of Structure
   L_UINT uFlags;       // Custom load options flags
}
LOADCUSTOMFILEOPTION, *pLOADCUSTOMFILEOPTION;

typedef struct _CUSTOMTILEINFO
{
   L_UINT uStructSize;  // Size of Structure
   L_INT nWidth;        // Width of tile to be compressed
   L_INT nHeight;       // Height of tile to be compressed
   L_INT nBitsPerPixel; // Number of bits per pixel
}
CUSTOMTILEINFO, *pCUSTOMTILEINFO;

typedef struct _FILECOMMENTS
{
   L_INT count;
   L_UCHAR** pointer;
   L_UINT* size;
}
FILECOMMENTS, *pFILECOMMENTS;

typedef struct _DIMENSION
{
   L_UINT nWidth;
   L_UINT nHeight;
}
DIMENSION, *pDIMENSION;

typedef struct _FILETRANSFORMS
{
   // AffineMatrix
   L_FLOAT a11, a12, a13, a14, a21, a22, a23, a24, a31, a32, a33, a34, a41, a42, a43, a44;
   // ColorTwistMatrix
   L_FLOAT byy, byc1, byc2, bc1y, bc1c1, bc1c2, bc2y, bc2c1, bc2c2;
   // ContrastAdjustment
   L_FLOAT fContrastAdjustment;
   // FilteringValue (Sharpness Adjustment)
   L_FLOAT fFilteringValue;
}
FILETRANSFORMS, *pFILETRANSFORMS;

// PLT Options
typedef struct _FILEPLTOPTIONS
{
   L_UINT uStructSize;
   L_INT PenWidth[8];
   L_COLORREF PenColor[8];
   L_BOOL bPenColorOverride;
}
FILEPLTOPTIONS, *pFILEPLTOPTIONS;

// RTF Options
typedef struct _FILERTFOPTIONS
{
   L_UINT uStructSize;     // The size of this structure
   L_DOUBLE dTopMarg;
   L_DOUBLE dBottomMarg;
   L_DOUBLE dRightMarg;
   L_DOUBLE dLeftMarg;
   L_DOUBLE dPaperWidth;
   L_DOUBLE dPaperHeight;
   L_INT nXResolution;
   L_INT nYResolution;
   L_COLORREF crBackColor;
}
FILERTFOPTIONS, *pFILERTFOPTIONS;

// TXT Options
typedef struct _FILETXTOPTIONSA
{
   L_UINT uStructSize;
   L_BOOL bEnabled;
   L_DOUBLE dTopMarg;
   L_DOUBLE dBottomMarg;
   L_DOUBLE dRightMarg;
   L_DOUBLE dLeftMarg;
   L_DOUBLE dPaperWidth;
   L_DOUBLE dPaperHeight;
   L_COLORREF crFontColor;
   L_COLORREF crHighlight;
   L_INT nFontSize;
   L_CHAR szFaceName[LF_FACESIZE];
   L_BOOL bBold;
   L_BOOL bItalic;
   L_BOOL bUnderLine;
   L_BOOL bStrikeThrough;
   L_BOOL bUseSystemLocale;
   L_COLORREF crBackColor;
}
FILETXTOPTIONSA, *pFILETXTOPTIONSA;
#if defined(FOR_UNICODE)
typedef struct _FILETXTOPTIONS
{
   L_UINT uStructSize;
   L_BOOL bEnabled;
   L_DOUBLE dTopMarg;
   L_DOUBLE dBottomMarg;
   L_DOUBLE dRightMarg;
   L_DOUBLE dLeftMarg;
   L_DOUBLE dPaperWidth;
   L_DOUBLE dPaperHeight;
   L_COLORREF crFontColor;
   L_COLORREF crHighlight;
   L_INT nFontSize;
   L_TCHAR szFaceName[LF_FACESIZE];
   L_BOOL bBold;
   L_BOOL bItalic;
   L_BOOL bUnderLine;
   L_BOOL bStrikeThrough;
   L_BOOL bUseSystemLocale;
   L_COLORREF crBackColor;
}
FILETXTOPTIONS, *pFILETXTOPTIONS;
#else
typedef FILETXTOPTIONSA FILETXTOPTIONS;
typedef pFILETXTOPTIONSA pFILETXTOPTIONS;
#endif // #if defined(FOR_UNICODE)

// PDF Options
typedef struct _FILEPDFOPTIONSA
{
   L_UINT uStructSize;                                   // The size of this structure
   L_BOOL bUseLibFonts;                                  // Specifies whether to use the library installed fonts or system fonts, default is TRUE
   L_INT nXResolution;                                   // Horizontal display resolution default is current screen resolution
   L_INT nYResolution;                                   // Vertical display resolution default is current screen resolution
   L_INT nDisplayDepth;                                  // Bits per pixel for resulting bitmap, takes 1,4,8,24 default is 24
   L_INT nTextAlpha;                                     // Font Anti-Aliasing,takes 1(no anti-aliasing), 2 and 4 .Default is 4
   L_INT nGraphicsAlpha;                                 // Graphics Anti-Aliasing takes 1,2,4.Default is 1
   L_UCHAR szPassword[FILEPDFOPTIONS_MAX_PASSWORD_LEN];  // Password to be used with encrypted PDF files
   L_UINT uFlags;
   L_BOOL bCallbackEnabled;
   BITMAPLOADCALLBACK pfnLoadCallback;
   L_VOID* pCallbackUserData;
   L_CHAR szOutputFullPath[L_MAXPATH];
   L_BOOL bUseImageData;
}
FILEPDFOPTIONSA, *pFILEPDFOPTIONSA;
#if defined(FOR_UNICODE)
typedef struct _FILEPDFOPTIONS
{
   L_UINT uStructSize;                                   // The size of this structure
   L_BOOL bUseLibFonts;                                  // Specifies whether to use the library installed fonts or system fonts, default is TRUE
   L_INT nXResolution;                                   // Horizontal display resolution default is current screen resolution
   L_INT nYResolution;                                   // Vertical display resolution default is current screen resolution
   L_INT nDisplayDepth;                                  // Bits per pixel for resulting bitmap, takes 1,4,8,24 default is 24
   L_INT nTextAlpha;                                     // Font Anti-Aliasing,takes 1(no anti-aliasing), 2 and 4 .Default is 4
   L_INT nGraphicsAlpha;                                 // Graphics Anti-Aliasing takes 1,2,4.Default is 1
   L_UCHAR szPassword[FILEPDFOPTIONS_MAX_PASSWORD_LEN];  // Password to be used with encrypted PDF files
   L_UINT uFlags;
   L_BOOL bCallbackEnabled;
   BITMAPLOADCALLBACK pfnLoadCallback;
   L_VOID* pCallbackUserData;
   L_TCHAR szOutputFullPath[L_MAXPATH];
   L_BOOL bUseImageData;
}
FILEPDFOPTIONS, *pFILEPDFOPTIONS;
#else
typedef FILEPDFOPTIONSA FILEPDFOPTIONS;
typedef pFILEPDFOPTIONSA pFILEPDFOPTIONS;
#endif // #if defined(FOR_UNICODE)

typedef struct _FILEPDFSAVEOPTIONS
{
   L_UINT uStructSize;  // Size of the structure
   // Encryption
   L_UCHAR szUserPassword[255];
   L_UCHAR szOwnerPassword[255];
   L_BOOL b128bit;
   L_UINT dwEncryptFlags;
}
FILEPDFSAVEOPTIONS, *pFILEPDFSAVEOPTIONS;

// PTK Options
typedef struct _FILEPTKOPTIONS
{
   L_UINT uStructSize;     // The size of this structure
   L_INT nPTKResolution;   // Display resolution default is 96
}
FILEPTKOPTIONS, *pFILEPTKOPTIONS;

// JBIG2 Options
typedef struct _FILEJBIG2OPTIONS
{
   L_UINT uStructSize;

   //image
   L_UINT uImageFlags;
   L_UCHAR ucImageTemplateType;
   L_CHAR ImageGBATX1;
   L_CHAR ImageGBATY1;
   L_CHAR ImageGBATX2;
   L_CHAR ImageGBATY2;
   L_CHAR ImageGBATX3;
   L_CHAR ImageGBATY3;
   L_CHAR ImageGBATX4;
   L_CHAR ImageGBATY4;
   L_UINT uImageQFactor;

   //text
   L_UINT uTextFlags;
   L_UCHAR ucTextTemplateType;
   L_CHAR TextGBATX1;
   L_CHAR TextGBATY1;
   L_CHAR TextGBATX2;
   L_CHAR TextGBATY2;
   L_CHAR TextGBATX3;
   L_CHAR TextGBATY3;
   L_CHAR TextGBATX4;
   L_CHAR TextGBATY4;
   L_UINT uTextMinSymArea;
   L_UINT uTextMinSymWidth;
   L_UINT uTextMinSymHeight;
   L_UINT uTextMaxSymArea;
   L_UINT uTextMaxSymWidth;
   L_UINT uTextMaxSymHeight;
   L_UINT uTextDifThreshold;
   L_UINT uTextQFactor;

   L_UINT uXResolution;
   L_UINT uYResolution;
   L_UINT uFlags;
}
FILEJBIG2OPTIONS, *pFILEJBIG2OPTIONS;

// Bit values for the code blocks
typedef struct _CodBlockStyle
{
   L_UINT bSelectiveACBypass:1;
   L_UINT bResetContextOnBoundaries:1;
   L_UINT bTerminationOnEachPass:1;
   L_UINT bVerticallyCausalContext:1;
   L_UINT bPredictableTermination:1;
   L_UINT bErrorResilienceSymbol:1;
   L_UINT bReserved6:26;
}
CodBlockStyle;

typedef struct _FILEJ2KOPTIONS
{
   L_UINT uStructSize;  // the size of this structure

   //<----------------------
   L_BOOL bUseColorTransform;
   L_BOOL bDerivedQuantization;
   J2KCOMPRESSIONCONTROL uCompressionControl;
   L_FLOAT  fCompressionRatio;
   L_SIZE_T uTargetFileSize;

   // <------------------------ SIZ_MARKER
   L_UINT uXOsiz;                          // Horizontal offset from the origin of the reference grid to the left side of the image area.
   L_UINT uYOsiz;                          // Vertical offset from the origin of the reference grid to the top side of the image area.
   L_UINT uXTsiz;                          // Width of one reference tile with respect to the reference grid.
   L_UINT uYTsiz;                          // Height of one reference tile with respect to the reference grid.
   L_UINT uXTOsiz;                         // Horizontal offset from the origin of the reference grid to the left side of the first tile.
   L_UINT uYTOsiz;                         // Vertical offset from the origin of the reference grid to the top side of the first tile.
   L_UINT uXRsiz[J2K_MAX_COMPONENTS_NUM];  // Horizontal Sub_sampling Value
   L_UINT uYRsiz[J2K_MAX_COMPONENTS_NUM];  // Vertical Sub_sampling Value 

   // <------------------------ COD_MARKER
   L_UINT uDecompLevel;    // Number of decomposition levels, dyadic decomposition (Zero Implies no transform)
   L_UINT uProgressOrder;  // Progressing Order
   L_INT nCodBlockWidth;
   L_INT nCodBlockHeight;
   CodBlockStyle CodBlockStyleFlags;

   // <------------------------ QCD_MARKER
   L_UINT uGuardBits;
   L_INT nDerivedBaseMantissa;
   L_INT nDerivedBaseExponent;

   // <------------------------ Aditional Markers
   L_BOOL bUseSOPMarker;
   L_BOOL bUseEPHMarker;
   
   // <------------------------ ROI
   J2KREGIONOFINTEREST uROIControl;
   L_BOOL bUseROI;
   L_FLOAT fROIWeight;
   L_RECT rcROI;

   // <---- Alpha Channel Custom Compression
   L_BOOL  bAlphaChannelLossless;
   L_UINT  uAlphaChannelActiveBits;

#if defined(LEADTOOLS_V16_OR_LATER)
   // <---- Precinct Size
   J2KPRECINCTSIZE uPrecinctSize;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
}
FILEJ2KOPTIONS, *pFILEJ2KOPTIONS;

// DJV Options
typedef struct _FILEDJVOPTIONS
{
   L_UINT uStructSize;
   L_INT nResolution;
}
FILEDJVOPTIONS, *pFILEDJVOPTIONS;

typedef struct _J2KVIDEOINFO
{
   L_UINT nSize;
   L_INT nVersion;
   L_BOOL bReversible;
   L_BOOL bUseColorTransform;
   L_INT nMaxBytes;
   L_INT nBytePerLine;
   L_INT nBits;
   L_INT nColorSubSampling;   // Type of the color component subsampling
   L_INT nQuality;
   L_UINT uFrameWidth;        // Frame Width
   L_UINT uFrameHeight;       // Frame Height
   L_UINT uDecompLevel;       // Number of decomposition levels, dyadic decomposition (Zero Implies no transform)
   L_INT nTransform;          // Wavelet Transform used ( Type of wavelet filter ex. 9x7 or 5x3 );
   L_INT nViewSize;
   L_INT nSIMD;
}
J2KVIDEOINFO, *pJ2KVIDEOINFO;

// XPS Options
typedef struct _FILEXPSOPTIONS
{
   L_UINT uStructSize;
   L_INT nXResolution;
   L_INT nYResolution;
}
FILEXPSOPTIONS, *pFILEXPSOPTIONS;

typedef struct _FILEXLSOPTIONS
{
   L_UINT      uStructSize;
   L_UINT32    uFlags; // XLS_FLAGS_MULTIPAGE_SHEET
} FILEXLSOPTIONS, *pFILEXLSOPTIONS;

// Load/rasterize options for the following formats:
// Raster Pdf: FILE_RAS_PDF, FILE_RAS_PDF_G3_1D, FILE_RAS_PDF_G3_2D, FILE_RAS_PDF_G4, FILE_RAS_PDF_JPEG, FILE_RAS_PDF_JPEG_422, FILE_RAS_PDF_JPEG_411, FILE_RAS_PDF_LZW, FILE_RAS_PDF_JBIG2
// Text: FILE_TXT
// Rtf: FILE_RTF_RASTER
// Xls: FILE_XLS
typedef struct _RASTERIZEDOCOPTIONS
{
   L_UINT uStructSize;
   // Whether the user of the RASTERIZEDOCOPTIONS is enabled or not
   L_BOOL bEnabled;
   // Suggested page width in unit, default is 8.5
   L_DOUBLE dPageWidth;
   // Suggested page height in unit, default is 11.0
   L_DOUBLE dPageHeight;
   // Margins, default is 0.0
   // Margins are only valid for FILE_TXT and FILE_RTF_RASTER
   L_DOUBLE dLeftMargin;
   L_DOUBLE dTopMargin;
   L_DOUBLE dRightMargin;
   L_DOUBLE dBottomMargin;
   // Page width, page height and margins unit, default is Inch
   RASTERIZEDOC_UNIT uUnit;
   // Resolution, default is 0 = use screen resolution
   L_UINT uXResolution;
   L_UINT uYResolution;
   // Size mode, default is None
   RASTERIZEDOC_SIZEMODE uSizeMode;
}
RASTERIZEDOCOPTIONS, *pRASTERIZEDOCOPTIONS;

typedef struct _STARTDECOMPRESSDATAA
{
   L_UINT uStructSize;                 // for versioning-- use sizeof(STARTDECOMPRESSDATA)
   pBITMAPHANDLE pBitmap;              // Pointer to the bitmap handle to load the image to
   L_UINT uBitmapStructSize;           // size of the structure pointed to by pBitmap, uses sizeof(BITMAPHANDLE)
   L_UINT uStripsOrTiles;              // DECOMPRESS_STRIPS -- indicates that we are decompressing strips of data
                                       // DECOMPRESS_TILES  -- indicates that we are decompressing tiles of data
                                       // The main difference is that strips have a width equal to the bitmap width
                                       // but tiles can have a width that is less than the bitmap width
                                       // Note that the pfnReadCallback (desribed below) is not fired for DECOMPRESS_TILES
   L_UINT uFormat;                     // One of the following formats:
                                       //      FILE_JFIF
                                       //      FILE_RAW_RLE4
                                       //      FILE_RAW_RLE8
                                       //      FILE_RAW_BITFIELDS
                                       //      FILE_RAW_PACKBITS
                                       //      FILE_RAW_CCITT
                                       //      FILE_FAX_G3_1D
                                       //      FILE_FAX_G3_2D
                                       //      FILE_FAX_G4
   L_INT nWidth;                       // Bitmap width
   L_INT nHeight;                      // Bitmap Height
   L_INT nBitsPerPixel;                // Bits per pixel of raw data

   L_INT nViewPerspective;             // View perspective of raw data--one of the following constants:
                                       //      TOP_LEFT
                                       //      TOP_LEFT90
                                       //      TOP_LEFT180
                                       //      TOP_LEFT270
                                       //      BOTTOM_LEFT
                                       //      BOTTOM_LEFT90
                                       //      BOTTOM_LEFT180
                                       //      BOTTOM_LEFT270

   L_INT nRawOrder;                    // Color order of 24-bit raw data (ORDER_RGB, ORDER_BGR)
                                       // Ignored if palettized
   L_INT nLoadOrder;                   // Desired color order after 24-bit image is loaded (Ignored if palettized)
         
   L_INT nXResolution;                 // Horizontal resolution, in dots per inch.
   L_INT nYResolution;                 // Vertical resolution, in dots per inch.

   FILEREADCALLBACKA pfnReadCallback;   // Optional callback function for additional processing.
                                       // If you do not provide a callback function, use NULL as the value of this parameter.
                                       // If you do provide a callback function, use the function pointer as the value of this parameter.
                                       // The callback function must adhere to the function prototype described in FILEREADCALLBACK Function.

   L_RGBQUAD Palette[256];             // Palette for uncompressed data that is 8 bits per pixel or less. 
                                       // Fill in the first 2^BitsPerPixel entries in this palette and include DECOMPRESS_PALETTE in the uFlags field.
   L_UINT uFlags;                      // Any combination of the following flags:
                                       // DECOMPRESS_LSB          -- The least significant bit is filled first.
                                       // DECOMPRESS_PAD4         -- Each line is padded to a multiple of 4 bytes
                                       // DECOMPRESS_PALETTE      -- A palette is supplied in the rgbQuad field
   L_VOID* pUserData;

   L_UINT rgbColorMask[3];             // Valid for FILE_RAW_BITFIELDS only - contains R,G,B color masks
                                       // rgbColorMask[0] is the red mask
                                       // rgbColorMask[1] is the green mask
                                       // rgbColorMask[2] is the blue mask
                                       // For example, with 16-bit data arranged like (RRR RRGG GGGB BBBB), the masks would be
                                       //     rgbColorMask[0] = 0x7C00
                                       //     rgbColorMask[1] = 0x0E30
                                       //     rgbColorMask[2] = 0x001F
                                       // For 32-bit data, the only valid data is (RRRR RRRR GGGG GGGG BBBB BBBB)
                                       //     rgbColorMask[0] = 0xFF0000
                                       //     rgbColorMask[1] = 0x00FF00
                                       //     rgbColorMask[2] = 0x0000FF

   L_INT nPhotoInt;                    // For:
                                       //      FILE_RAW_CCITT, FILE_FAX_G3_1D, FILE_FAX_G3_2D, FILE_FAX_G4
                                       //         0 = WhiteIsZero
                                       //         1 = BlackIsZero
                                       //
                                       //      FILE_RAW_PACKBITS
                                       //         0 = WhiteIsZero
                                       //         1 = BlackIsZero
                                       //         2 = There is no ColorMap (RGB)
                                       //         5 = Separated - CMYK
                                       //         6 = YC b C r color space
                                       //         8 = 1976 CIE L*a*b*

   L_INT nPlanarConfig;                // For FILE_RAW_PACKBITS only
                                       //      1 = Chunky
                                       //      2 = Planar format

   L_UINT uReserved1;                  // Reserved for future use
}
STARTDECOMPRESSDATAA, *pSTARTDECOMPRESSDATAA;
#if defined(FOR_UNICODE)
typedef struct _STARTDECOMPRESSDATA
{
   L_UINT uStructSize;                 // for versioning-- use sizeof(STARTDECOMPRESSDATA)
   pBITMAPHANDLE pBitmap;              // Pointer to the bitmap handle to load the image to
   L_UINT uBitmapStructSize;           // size of the structure pointed to by pBitmap, uses sizeof(BITMAPHANDLE)
   L_UINT uStripsOrTiles;              // DECOMPRESS_STRIPS -- indicates that we are decompressing strips of data
                                       // DECOMPRESS_TILES  -- indicates that we are decompressing tiles of data
                                       // The main difference is that strips have a width equal to the bitmap width
                                       // but tiles can have a width that is less than the bitmap width
                                       // Note that the pfnReadCallback (desribed below) is not fired for DECOMPRESS_TILES
   L_UINT uFormat;                     // One of the following formats:
                                       //      FILE_JFIF
                                       //      FILE_RAW_RLE4
                                       //      FILE_RAW_RLE8
                                       //      FILE_RAW_BITFIELDS
                                       //      FILE_RAW_PACKBITS
                                       //      FILE_RAW_CCITT
                                       //      FILE_FAX_G3_1D
                                       //      FILE_FAX_G3_2D
                                       //      FILE_FAX_G4
   L_INT nWidth;                       // Bitmap width
   L_INT nHeight;                      // Bitmap Height
   L_INT nBitsPerPixel;                // Bits per pixel of raw data

   L_INT nViewPerspective;             // View perspective of raw data--one of the following constants:
                                       //      TOP_LEFT
                                       //      TOP_LEFT90
                                       //      TOP_LEFT180
                                       //      TOP_LEFT270
                                       //      BOTTOM_LEFT
                                       //      BOTTOM_LEFT90
                                       //      BOTTOM_LEFT180
                                       //      BOTTOM_LEFT270

   L_INT nRawOrder;                    // Color order of 24-bit raw data (ORDER_RGB, ORDER_BGR)
                                       // Ignored if palettized
   L_INT nLoadOrder;                   // Desired color order after 24-bit image is loaded (Ignored if palettized)
         
   L_INT nXResolution;                 // Horizontal resolution, in dots per inch.
   L_INT nYResolution;                 // Vertical resolution, in dots per inch.

   FILEREADCALLBACK pfnReadCallback;   // Optional callback function for additional processing.
                                       // If you do not provide a callback function, use NULL as the value of this parameter.
                                       // If you do provide a callback function, use the function pointer as the value of this parameter.
                                       // The callback function must adhere to the function prototype described in FILEREADCALLBACK Function.

   L_RGBQUAD Palette[256];             // Palette for uncompressed data that is 8 bits per pixel or less. 
                                       // Fill in the first 2^BitsPerPixel entries in this palette and include DECOMPRESS_PALETTE in the uFlags field.
   L_UINT uFlags;                      // Any combination of the following flags:
                                       // DECOMPRESS_LSB          -- The least significant bit is filled first.
                                       // DECOMPRESS_PAD4         -- Each line is padded to a multiple of 4 bytes
                                       // DECOMPRESS_PALETTE      -- A palette is supplied in the rgbQuad field
   L_VOID* pUserData;

   L_UINT rgbColorMask[3];             // Valid for FILE_RAW_BITFIELDS only - contains R,G,B color masks
                                       // rgbColorMask[0] is the red mask
                                       // rgbColorMask[1] is the green mask
                                       // rgbColorMask[2] is the blue mask
                                       // For example, with 16-bit data arranged like (RRR RRGG GGGB BBBB), the masks would be
                                       //     rgbColorMask[0] = 0x7C00
                                       //     rgbColorMask[1] = 0x0E30
                                       //     rgbColorMask[2] = 0x001F
                                       // For 32-bit data, the only valid data is (RRRR RRRR GGGG GGGG BBBB BBBB)
                                       //     rgbColorMask[0] = 0xFF0000
                                       //     rgbColorMask[1] = 0x00FF00
                                       //     rgbColorMask[2] = 0x0000FF

   L_INT nPhotoInt;                    // For:
                                       //      FILE_RAW_CCITT, FILE_FAX_G3_1D, FILE_FAX_G3_2D, FILE_FAX_G4
                                       //         0 = WhiteIsZero
                                       //         1 = BlackIsZero
                                       //
                                       //      FILE_RAW_PACKBITS
                                       //         0 = WhiteIsZero
                                       //         1 = BlackIsZero
                                       //         2 = There is no ColorMap (RGB)
                                       //         5 = Separated - CMYK
                                       //         6 = YC b C r color space
                                       //         8 = 1976 CIE L*a*b*

   L_INT nPlanarConfig;                // For FILE_RAW_PACKBITS only
                                       //      1 = Chunky
                                       //      2 = Planar format

   L_UINT uReserved1;                  // Reserved for future use
}
STARTDECOMPRESSDATA, *pSTARTDECOMPRESSDATA;
#else
typedef STARTDECOMPRESSDATAA STARTDECOMPRESSDATA;
typedef pSTARTDECOMPRESSDATAA pSTARTDECOMPRESSDATA;
#endif // #if defined(FOR_UNICODE)

typedef struct _DECOMPRESSDATA
{
   L_UINT uStructSize;     // for versioning-- use sizeof(STARTDECOMPRESSDATA)
   L_UCHAR* pBuffer;       // pointer to raw compressed data
                           //
   L_INT nWidth;           // width of uncompressed strip/tile in bytes
                           //    For strips, this is the width of the image
                           //    For tiles, this is usually a fraction of the image width
   L_INT nHeight;          // height of uncompressed strip/tile in bytes
                           //    If the image consists of one single compressed strip (as with TWAIN), 
                           //    this is the height of the image
                           //
   L_SIZE_T uOffset;       // offset of strip relative to buffer -- usually zero
   L_UINT32 nBufferSize;   // size of strip after compression  
                           //    For TWAIN, this is the buffer size determined by DAT_SETUPMEMXFER
                           //
   L_INT nRow;             // Row offset of tile or strip
   L_INT nCol;             // Column offset of tile
                           //    For strips, this value is ignored
   L_UINT uFlags;          // Flags used for tiles or strips
                           //    DECOMPRESS_STRIP_START
                           //    DECOMPRESS_STRIP_END
                           //    DECOMPRESS_CHUNK_COMPLETE (note: this is DECOMPRESS_STRIP_START | DECOMPRESS_STRIP_END)
                           // If pBuffer does not point to a full strip or tile you set uFlags to DECOMPRESS_STRIP_START 
                           //    when the strip/tile begins and DECOMPRESS_STRIP_END when the strip/tile ends
                           // If pBuffer points to a complete strip/tile, then set this to DECOMPRESS_CHUNK_COMPLETE
   L_INT nReserved1;       // Reserved for future use
}
DECOMPRESSDATA, *pDECOMPRESSDATA;

// Typedefs and defines used with extensions functions
typedef struct _LTEXTENSION
{
   L_WCHAR* pName;      // the extension name (as Unicode)
   L_SIZE_T uDataSize;  // the size of the extension data (0xFFFFFFFF if the extension is a stream)
   L_UCHAR* pData;      // extension data. NULL for streams (if uDataSize is 0xFFFFFFFF)
   CLSID* pClsid;       // CLSID describing the extension. Only valid if uDataSize is 0xFFFFFFFF
   L_UCHAR ucDefault;   // internal use
}
LTEXTENSION, *pEXTENSION;

typedef struct _EXTENSIONLIST
{
   L_UINT uStructSize;     // size of the whole extension list
   L_UINT uFlags;          // describes the type data contained in the extensions
   L_UINT uCount;          // the number of extensions present in aList
   LTEXTENSION aList[1];   // the list of extensions
}
EXTENSIONLIST, *pEXTENSIONLIST;

typedef struct _FILTERINFO
{
   L_UINT uStructSize;        // Structure size
   L_WCHAR szName[8];         // Filter name.
   L_SIZE_T uSize;            // Size of szName in words
   L_WCHAR* pszExtensionList; // The extension list of files of this type. This speeds up the detection of the file format.
   L_UINT uFlags;             // FILTERINFO_XXX flags
}
FILTERINFO, *pFILTERINFO;

typedef struct _LEADFILETAG
{
   L_UINT16 uTag;
   L_UINT16 uType;
   L_UINT uCount;
   L_UINT uDataSize;
   L_SIZE_T uDataOffset;
}
LEADFILETAG, *pLEADFILETAG;

typedef struct _LEADFILECOMMENT
{
   L_UINT uType;
   L_UINT uDataSize;
   L_SIZE_T uDataOffset;
}
LEADFILECOMMENT, *pLEADFILECOMMENT;

#if !defined(FOR_WINCE)

//* PSD layers functions and defines
typedef struct _LAYERINFOA
{
   L_UINT uStructSize;  // the size of LAYERINFO - use sizeof(LAYERINFO)
   L_INT nLayerLeft;
   L_INT nLayerTop;
   L_UCHAR uOpacity;
   L_UCHAR uClipping;
   L_UCHAR szBlendModeKey[4];
   pBITMAPHANDLE pMaskBitmap;
   L_CHAR szName[MAX_PATH];
}
LAYERINFOA, *pLAYERINFOA;
#if defined(FOR_UNICODE)
typedef struct _LAYERINFO
{
   L_UINT uStructSize;  // the size of LAYERINFO - use sizeof(LAYERINFO)
   L_INT nLayerLeft;
   L_INT nLayerTop;
   L_UCHAR uOpacity;
   L_UCHAR uClipping;
   L_UCHAR szBlendModeKey[4];
   pBITMAPHANDLE pMaskBitmap;
   L_TCHAR szName[MAX_PATH];
}
LAYERINFO, *pLAYERINFO;
#else
typedef LAYERINFOA LAYERINFO;
typedef pLAYERINFOA pLAYERINFO;
#endif // #if defined(FOR_UNICODE)

// The overlay callback is called to provide the overlay bitmap
typedef struct _FILEOVERLAYCALLBACKDATAA
{
   L_UINT uStructSize;
   L_CHAR* pszFilename;
   L_INT nPageNumber;
   L_BOOL bInfo;
   L_INT nInfoWidth;
   L_INT nInfoHeight;
   L_INT nInfoXResolution;
   L_INT nInfoYResolution;
   pBITMAPHANDLE pLoadBitmap;
}
FILEOVERLAYCALLBACKDATAA, *pFILEOVERLAYCALLBACKDATAA;
#if defined(FOR_UNICODE)
typedef struct _FILEOVERLAYCALLBACKDATA
{
   L_UINT uStructSize;
   L_TCHAR* pszFilename;
   L_INT nPageNumber;
   L_BOOL bInfo;
   L_INT nInfoWidth;
   L_INT nInfoHeight;
   L_INT nInfoXResolution;
   L_INT nInfoYResolution;
   pBITMAPHANDLE pLoadBitmap;
}
FILEOVERLAYCALLBACKDATA, *pFILEOVERLAYCALLBACKDATA;
#else
typedef FILEOVERLAYCALLBACKDATAA FILEOVERLAYCALLBACKDATA;
typedef pFILEOVERLAYCALLBACKDATAA pFILEOVERLAYCALLBACKDATA;
#endif // #if defined(FOR_UNICODE)

#if defined(LEADTOOLS_V16_OR_LATER)
typedef struct _CHANNELINFOA
{
   L_UINT      uStructSize;
   L_CHAR      szName[64];
   CHANNELTYPE ChannelType;
}CHANNELINFOA, *pCHANNELINFOA;
#if defined(FOR_UNICODE)
typedef struct _CHANNELINFO
{
   L_UINT      uStructSize;
   L_TCHAR     szName[64];
   CHANNELTYPE ChannelType;
}CHANNELINFO, *pCHANNELINFO;
#else
typedef CHANNELINFOA CHANNELINFO;
typedef pCHANNELINFOA pCHANNELINFO;
#endif // #if defined(FOR_UNICODE)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#endif // #if !defined(FOR_WINCE)

/****************************************************************
   Function prototypes
****************************************************************/
#if !defined(FOR_MANAGED) || defined(FOR_MANAGED_CODEC)

// functions1
L_LTFIL_API L_INT EXT_FUNCTION L_CompressBuffer(
   L_UCHAR* pBuffer);

L_LTFIL_API L_INT EXT_FUNCTION L_EndCompressBuffer(
   L_VOID);

L_LTFIL_API L_INT EXT_FUNCTION L_GetComment(
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength);

L_LTFIL_API L_INT EXT_FUNCTION L_GetLoadResolution(
   L_INT nFormat,
   L_UINT* pWidth,
   L_UINT* pHeight,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetTag(
   L_UINT16 uTag,
   L_UINT16* pType,
   L_UINT* pCount,
   L_VOID* pData);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentMemory(
   L_UCHAR* pBuffer,
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTagMemory(
   L_UCHAR* pBuffer,
   L_UINT16 uTag,
   L_UINT16* pType,
   L_UINT* pCount,
   L_VOID* pData,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SetComment(
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength);

L_LTFIL_API LOADINFOCALLBACK EXT_FUNCTION L_SetLoadInfoCallback(
   LOADINFOCALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTFIL_API L_VOID* EXT_FUNCTION L_GetLoadInfoCallbackData(
   L_VOID);

L_LTFIL_API L_INT EXT_FUNCTION L_SetLoadResolution(
   L_INT nFormat,
   L_UINT nWidth,
   L_UINT nHeight);

L_LTFIL_API L_INT EXT_FUNCTION L_SetTag(
   L_UINT16 uTag,
   L_UINT16 uType,
   L_UINT uCount,
   L_VOID* pData);

L_LTFIL_API L_INT EXT_FUNCTION L_SetSaveResolution(
   L_UINT uCount,
   pDIMENSION pResolutions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetSaveResolution(
   L_UINT* puCount,
   pDIMENSION pResolutions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetDefaultLoadFileOption(
   pLOADFILEOPTION pLoadOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_GetJ2KOptions(
   pFILEJ2KOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_GetDefaultJ2KOptions(
   pFILEJ2KOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetJ2KOptions(
   pFILEJ2KOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_MarkerCallbackProxy(
   LEADMARKERCALLBACK pfnCallback,
   L_UINT uMarker,
   L_UINT uMarkerSize,
   L_UCHAR* pMarkerData,
   L_VOID* pLEADUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_FreeExtensions(
   pEXTENSIONLIST pExtensionList);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadExtensionStamp(
   pEXTENSIONLIST pExtensionList,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_GetExtensionAudio(
   pEXTENSIONLIST pExtensionList,
   L_INT nStream,
   L_UCHAR** ppBuffer,
   L_SIZE_T* puSize);

L_LTFIL_API L_INT EXT_FUNCTION L_StopDecompressBuffer(
   L_HGLOBAL hDecompress);

L_LTFIL_API L_INT EXT_FUNCTION L_DecompressBuffer(
   L_HGLOBAL hDecompress,
   pDECOMPRESSDATA pDecompressData);

L_LTFIL_API L_INT EXT_FUNCTION L_GetJBIG2Options(
   pFILEJBIG2OPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetJBIG2Options(
   pFILEJBIG2OPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_CreateMarkers(
   L_HANDLE* phMarkers);

L_LTFIL_API L_INT EXT_FUNCTION L_FreeMarkers(
   L_VOID* hMarkers);

L_LTFIL_API L_INT EXT_FUNCTION L_SetMarkers(
   L_VOID* hMarkers,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_GetMarkers(
   L_VOID** phMarkers,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_EnumMarkers(
   L_VOID* hMarkers,
   L_UINT uFlags,
   ENUMMARKERSCALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_DeleteMarker(
   L_VOID* hMarkers,
   L_UINT uMarker,
   L_INT nCount);

L_LTFIL_API L_INT EXT_FUNCTION L_InsertMarker(
   L_VOID* hMarkers,
   L_UINT uIndex,
   L_UINT uMarker,
   L_UINT uMarkerSize,
   L_UCHAR* pMarkerData);

L_LTFIL_API L_INT EXT_FUNCTION L_CopyMarkers(
   L_VOID** phMarkersDst,
   L_HANDLE hMarkersSrc);

L_LTFIL_API L_INT EXT_FUNCTION L_GetMarkerCount(
   L_VOID* hMarkers,
   L_UINT* puCount);

L_LTFIL_API L_INT EXT_FUNCTION L_GetMarker(
   L_VOID* hMarkers,
   L_UINT uIndex,
   L_UINT* puMarker,
   L_UINT* puMarkerSize,
   L_UCHAR* pMarkerData);

L_LTFIL_API L_INT EXT_FUNCTION L_DeleteMarkerIndex(
   L_VOID* hMarkers,
   L_UINT uIndex);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveData(
   L_VOID* pStruct,
   L_UCHAR* pDataBuffer,
   L_SIZE_T ulBytes);

L_LTFIL_API L_INT EXT_FUNCTION L_StopSaveData(
   L_VOID* pStruct);

L_LTFIL_API L_INT EXT_FUNCTION L_SetGeoKey(
   L_UINT16 uTag,
   L_UINT uType,
   L_UINT uCount,
   L_VOID* pData);

L_LTFIL_API L_INT EXT_FUNCTION L_GetGeoKey(
   L_UINT16 uTag,
   L_UINT* puType,
   L_UINT* puCount,
   L_VOID* pData);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentOffset(
   L_HFILE fd,
   L_SSIZE_T nOffsetBegin,
   L_SSIZE_T nBytesToLoad,
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetLoadStatus(L_VOID);

L_LTFIL_API L_INT EXT_FUNCTION L_DecodeABIC(
   L_UCHAR *pInputData,
   L_SSIZE_T nLength,
   L_UCHAR **ppOutputData,
   L_INT nAlign,
   L_INT nWidth,
   L_INT nHeight,
   L_BOOL bBiLevel);

L_LTFIL_API L_INT EXT_FUNCTION L_EncodeABIC(
   L_UCHAR* pInputData,
   L_INT nAlign,
   L_INT nWidth,
   L_INT nHeight,
   L_UCHAR** ppOutputData,
   L_SSIZE_T* pnLength,
   L_BOOL bBiLevel);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPNGTRNS( L_UCHAR * pData, L_UINT uSize );
L_LTFIL_API L_UINT EXT_FUNCTION L_GetPNGTRNS( L_UCHAR * pData );

L_LTFIL_API L_INT EXT_FUNCTION L_GetFilterListInfo(
   pFILTERINFO* ppFilterList,
   L_UINT* pFilterCount);

L_LTFIL_API L_INT EXT_FUNCTION L_GetFilterInfo(
   L_WCHAR* pFilterName,
   pFILTERINFO pFilterInfo,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_FreeFilterInfo(
   pFILTERINFO pFilterInfo,
   L_UINT uFilterCount,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_SetFilterInfo(
   pFILTERINFO pFilterInfo,
   L_UINT uFilterCount,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_DeletePageA(
   L_CHAR* pszFile,
   L_INT nPage,
   L_UINT uFlags,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadLoadResolutionsA(
   L_CHAR* pszFile,
   pDIMENSION pDimensions,
   L_INT* pDimensionCount,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FileConvertA(
   L_CHAR* pszFileSrc,
   L_CHAR* pszFileDst,
   L_INT nType,
   L_INT nWidth,
   L_INT nHeight,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   pLOADFILEOPTION pLoadOptions,
   pSAVEFILEOPTIONA pSaveOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_FileInfoA(
   L_CHAR* pszFile,
   pFILEINFOA pFileInfo,
   L_UINT uStructSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FileInfoMemoryA(
   L_UCHAR* pBuffer,
   pFILEINFOA pFileInfo,
   L_UINT uStructSize,
   L_SSIZE_T nBufferSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetFileCommentSizeA(
   L_CHAR* pszFile,
   L_UINT uType,
   L_UINT* uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapListA(
   L_CHAR*lpszFile,
   pHBITMAPLIST phList,
   L_INT nBitsTo,
   L_INT nColorOrder,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapMemoryA(
   L_UCHAR* pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileTileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMemoryTileA(
   L_UCHAR*   pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileOffsetA(
   L_HFILE fd,
   L_SSIZE_T nOffsetBegin,
   L_SSIZE_T nBytesToLoad,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMemoryA(
   L_UCHAR* pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentA(
   L_CHAR* pszFile,
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentExtA(
   L_CHAR* pszFile,
   L_UINT uType,
   pFILECOMMENTS pComments,
   L_UCHAR* pBuffer,
   L_UINT* uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTagA(
   L_CHAR* pFile,
   L_UINT16 uTag,
   L_UINT16* pType,
   L_UINT* pCount,
   L_VOID* pData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileStampA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapBufferA(
   L_UCHAR* pBuffer,
   L_SIZE_T uInitialBufferSize,
   L_SIZE_T* puFinalFileSize,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   SAVEBUFFERCALLBACK pfnSaveBufferCB,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapListA(
   L_CHAR* lpszFile,
   HBITMAPLIST hList,
   L_INT nFormat,
   L_INT nBits,
   L_INT nQFactor,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapMemoryA(
   L_HGLOBAL* phHandle,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_SIZE_T* puSize,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileBufferA(
   L_UCHAR* pBuffer,
   L_SIZE_T uInitialBufferSize,
   L_SIZE_T* puFinalFileSize,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnFileSaveCB,
   SAVEBUFFERCALLBACK pfnSaveBufferCB,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileMemoryA(
   L_HANDLE* hHandle,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pFunction,
   L_VOID* lpUserData,
   L_SIZE_T* uSize,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileTileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nCol,
   L_INT nRow,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileOffsetA(
   L_HFILE fd,
   L_SSIZE_T nOffsetBegin,
   L_SSIZE_T* nSizeWritten,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartCompressBufferA(
   pBITMAPHANDLE pBitmap,
   COMPBUFFCALLBACK pfnCallback,
   L_SIZE_T uInputBytes,
   L_SIZE_T uOutputBytes,
   L_UCHAR* pOutputBuffer,
   L_INT nOutputType,
   L_INT nQFactor,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartFeedLoadA(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   L_HGLOBAL* phLoad,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_FeedLoadA(
   L_HGLOBAL hLoad,
   L_UCHAR* pBuffer,
   L_SIZE_T dwBufferSize);

L_LTFIL_API L_INT EXT_FUNCTION L_StopFeedLoadA(
   L_HGLOBAL hLoad);

L_LTFIL_API L_INT EXT_FUNCTION L_StartFeedInfoA(
   L_VOID** phInfo,
   pFILEINFOA pFileInfo,
   L_UINT uStructSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FeedInfoA(
   L_VOID* hInfo,
   L_UCHAR* pBuffer,
   L_SIZE_T dwBufferSize);

L_LTFIL_API L_INT EXT_FUNCTION L_StopFeedInfoA(L_VOID* hInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_StartFeedInfo(
   L_VOID** phInfo,
   pFILEINFO pFileInfo,
   L_UINT uStructSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FeedInfo(
   L_VOID* hInfo,
   L_UCHAR* pBuffer,
   L_SIZE_T dwBufferSize);

L_LTFIL_API L_INT EXT_FUNCTION L_StopFeedInfo(L_VOID* hInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileCommentExtA(
   L_CHAR*pszFile,
   L_UINT uType,
   pFILECOMMENTS pComments,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileStampA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetDefaultSaveFileOptionA(
   pSAVEFILEOPTIONA pSaveOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileTagA(
   L_CHAR* pszFile,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileCommentA(
   L_CHAR* pszFile,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_CreateThumbnailFromFileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   pTHUMBOPTIONS pThumbOptions,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_TransformFileA(
   L_CHAR* pszFileSrc,
   L_CHAR* pszFileDst,
   L_UINT uTransform,
   TRANSFORMFILECALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileExtensionsA(
   L_CHAR* pszFile,
   pEXTENSIONLIST* ppExtensionList,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartDecompressBufferA(
   L_HGLOBAL *phDecompress,
   pSTARTDECOMPRESSDATAA pStartDecompressData);

L_LTFIL_API L_INT EXT_FUNCTION L_IgnoreFiltersA(
   L_CHAR* pszFilters);

L_LTFIL_API L_INT EXT_FUNCTION L_PreLoadFiltersA(
   L_INT nFixedFilters,
   L_INT nCachedFilters,
   L_CHAR* pszFilters);

L_LTFIL_API L_SSIZE_T EXT_FUNCTION L_GetIgnoreFiltersA(
   L_CHAR* pszFilters,
   L_SIZE_T uSize);

L_LTFIL_API L_SSIZE_T EXT_FUNCTION L_GetPreLoadFiltersA(
   L_CHAR* pszFilters,
   L_SIZE_T uSize,
   L_INT* pnFixedFilters,
   L_INT* pnCachedFilters);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMarkersA(
   L_CHAR* pszFilename,
   L_VOID** phMarkers,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileMetaDataA(
   L_CHAR* pFile,
   L_UINT uFlags,
   pSAVEFILEOPTIONA pSaveFileOption);

L_LTFIL_API L_INT EXT_FUNCTION L_StartSaveDataA(
   L_VOID** ppStruct,
   L_CHAR* pszFileName,
   L_INT nCompression,
   L_INT nPlanarConfig,
   L_INT nOrder,           // ORDER_BGR or ORDER_RGB
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_RGBQUAD* pPalette,
   L_UINT uPaletteEntries,
   L_INT XResolution,
   L_INT YResolution,
   L_BOOL bSaveMulti,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetTXTOptionsA(
   pFILETXTOPTIONSA pTxtOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetTXTOptionsA(
   pFILETXTOPTIONSA pTxtOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_CompactFileA(
   L_CHAR* pszSrcFile,
   L_CHAR* pszDstFile,
   L_UINT uPages,
   pLOADFILEOPTION pLoadFileOption,
   pSAVEFILEOPTIONA pSaveFileOption);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileCMYKArrayA(
   L_CHAR* pszFile,
   pBITMAPHANDLE* ppBitmapArray,
   L_UINT uArrayCount,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadFileOption,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileCMYKArrayA(
   L_CHAR* pszFile, 
   pBITMAPHANDLE* ppBitmapArray,
   L_UINT uBitmapArrayCount,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_EnumFileTagsA(
   L_CHAR* pszFile,
   L_UINT uFlags,
   ENUMTAGSCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_DeleteTagA(
   L_CHAR* pszFile,
   L_INT nPage,
   L_UINT uTag,
   L_UINT uFlags,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileGeoKeyA(
   L_CHAR* pszFile,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileGeoKeyA(
   L_CHAR* pszFile,
   L_UINT16 uTag,
   L_UINT* puType,
   L_UINT* pCount,
   L_VOID* pData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_EnumFileGeoKeysA(
   L_CHAR* pszFile,
   L_UINT uFlags,
   ENUMGEOKEYSCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SetOverlayCallbackA(
   OVERLAYCALLBACKA pfnCallback,
   L_VOID* pUserData,
   L_UINT uFlags
);

L_LTFIL_API L_INT EXT_FUNCTION L_GetOverlayCallbackA(
   OVERLAYCALLBACKA *ppfnCallback,
   L_VOID** ppUserData,
   L_UINT* puFlags
);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTagsA(
   L_CHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puTagCount,
   pLEADFILETAG* ppTags,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileGeoKeysA(
   L_CHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puTagCount,
   pLEADFILETAG* ppTags,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentsA(
   L_CHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puCommentCount,
   pLEADFILECOMMENT* ppComments,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

#if defined(FOR_UNICODE)
L_LTFIL_API L_INT EXT_FUNCTION L_DeletePage(
   L_TCHAR* pszFile,
   L_INT nPage,
   L_UINT uFlags,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadLoadResolutions(
   L_TCHAR* pszFile,
   pDIMENSION pDimensions,
   L_INT* pDimensionCount,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FileConvert(
   L_TCHAR* pszFileSrc,
   L_TCHAR* pszFileDst,
   L_INT nType,
   L_INT nWidth,
   L_INT nHeight,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   pLOADFILEOPTION pLoadOptions,
   pSAVEFILEOPTION pSaveOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_FileInfo(
   L_TCHAR* pszFile,
   pFILEINFO pFileInfo,
   L_UINT uStructSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FileInfoMemory(
   L_UCHAR* pBuffer,
   pFILEINFO pFileInfo,
   L_UINT uStructSize,
   L_SSIZE_T nBufferSize,
   L_UINT uFlags,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetFileCommentSize(
   L_TCHAR* pszFile,
   L_UINT uType,
   L_UINT* uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmap(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapList(
   L_TCHAR*lpszFile,
   pHBITMAPLIST phList,
   L_INT nBitsTo,
   L_INT nColorOrder,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapMemory(
   L_UCHAR* pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileTile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMemoryTile(
   L_UCHAR*   pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileOffset(
   L_HFILE fd,
   L_SSIZE_T nOffsetBegin,
   L_SSIZE_T nBytesToLoad,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMemory(
   L_UCHAR* pBuffer,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileComment(
   L_TCHAR* pszFile,
   L_UINT uType,
   L_UCHAR* pComment,
   L_UINT uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileCommentExt(
   L_TCHAR* pszFile,
   L_UINT uType,
   pFILECOMMENTS pComments,
   L_UCHAR* pBuffer,
   L_UINT* uLength,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTag(
   L_TCHAR* pFile,
   L_UINT16 uTag,
   L_UINT16* pType,
   L_UINT* pCount,
   L_VOID* pData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileStamp(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmap(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapBuffer(
   L_UCHAR* pBuffer,
   L_SIZE_T uInitialBufferSize,
   L_SIZE_T* puFinalFileSize,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   SAVEBUFFERCALLBACK pfnSaveBufferCB,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapList(
   L_TCHAR* lpszFile,
   HBITMAPLIST hList,
   L_INT nFormat,
   L_INT nBits,
   L_INT nQFactor,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapMemory(
   L_HGLOBAL* phHandle,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_SIZE_T* puSize,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileBuffer(
   L_UCHAR* pBuffer,
   L_SIZE_T uInitialBufferSize,
   L_SIZE_T* puFinalFileSize,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnFileSaveCB,
   SAVEBUFFERCALLBACK pfnSaveBufferCB,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileMemory(
   L_HANDLE* hHandle,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pFunction,
   L_VOID* lpUserData,
   L_SIZE_T* uSize,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileTile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nCol,
   L_INT nRow,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileOffset(
   L_HFILE fd,
   L_SSIZE_T nOffsetBegin,
   L_SSIZE_T* nSizeWritten,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartCompressBuffer(
   pBITMAPHANDLE pBitmap,
   COMPBUFFCALLBACK pfnCallback,
   L_SIZE_T uInputBytes,
   L_SIZE_T uOutputBytes,
   L_UCHAR* pOutputBuffer,
   L_INT nOutputType,
   L_INT nQFactor,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartFeedLoad(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   L_HGLOBAL* phLoad,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_FeedLoad(
   L_HGLOBAL hLoad,
   L_UCHAR* pBuffer,
   L_SIZE_T dwBufferSize);

L_LTFIL_API L_INT EXT_FUNCTION L_StopFeedLoad(
   L_HGLOBAL hLoad);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileCommentExt(
   L_TCHAR*pszFile,
   L_UINT uType,
   pFILECOMMENTS pComments,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileStamp(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetDefaultSaveFileOption(
   pSAVEFILEOPTION pSaveOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileTag(
   L_TCHAR* pszFile,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileComment(
   L_TCHAR* pszFile,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_CreateThumbnailFromFile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   pTHUMBOPTIONS pThumbOptions,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_TransformFile(
   L_TCHAR* pszFileSrc,
   L_TCHAR* pszFileDst,
   L_UINT uTransform,
   TRANSFORMFILECALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileExtensions(
   L_TCHAR* pszFile,
   pEXTENSIONLIST* ppExtensionList,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_StartDecompressBuffer(
   L_HGLOBAL *phDecompress,
   pSTARTDECOMPRESSDATA pStartDecompressData);

#if defined(LEADTOOLS_V16_OR_LATER)
L_LTFIL_API L_BOOL EXT_FUNCTION L_EnableFastFileInfo(L_BOOL bEnable);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

L_LTFIL_API L_INT EXT_FUNCTION L_IgnoreFilters(
   L_TCHAR* pszFilters);

L_LTFIL_API L_INT EXT_FUNCTION L_PreLoadFilters(
   L_INT nFixedFilters,
   L_INT nCachedFilters,
   L_TCHAR* pszFilters);

L_LTFIL_API L_SSIZE_T EXT_FUNCTION L_GetIgnoreFilters(
   L_TCHAR* pszFilters,
   L_SIZE_T uSize);

L_LTFIL_API L_SSIZE_T EXT_FUNCTION L_GetPreLoadFilters(
   L_TCHAR* pszFilters,
   L_SIZE_T uSize,
   L_INT* pnFixedFilters,
   L_INT* pnCachedFilters);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadMarkers(
   L_TCHAR* pszFilename,
   L_VOID** phMarkers,
   L_UINT uFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileMetaData(
   L_TCHAR* pFile,
   L_UINT uFlags,
   pSAVEFILEOPTION pSaveFileOption);

L_LTFIL_API L_INT EXT_FUNCTION L_StartSaveData(
   L_VOID** ppStruct,
   L_TCHAR* pszFileName,
   L_INT nCompression,
   L_INT nPlanarConfig,
   L_INT nOrder,           // ORDER_BGR or ORDER_RGB
   L_UINT uWidth,
   L_UINT uHeight,
   L_INT nBitsPerPixel,
   L_RGBQUAD* pPalette,
   L_UINT uPaletteEntries,
   L_INT XResolution,
   L_INT YResolution,
   L_BOOL bSaveMulti,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetTXTOptions(
   pFILETXTOPTIONS pTxtOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetTXTOptions(
   pFILETXTOPTIONS pTxtOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_CompactFile(
   L_TCHAR* pszSrcFile,
   L_TCHAR* pszDstFile,
   L_UINT uPages,
   pLOADFILEOPTION pLoadFileOption,
   pSAVEFILEOPTION pSaveFileOption);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadFileCMYKArray(
   L_TCHAR* pszFile,
   pBITMAPHANDLE* ppBitmapArray,
   L_UINT uArrayCount,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_UINT uFlags,
   FILEREADCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadFileOption,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveFileCMYKArray(
   L_TCHAR* pszFile, 
   pBITMAPHANDLE* ppBitmapArray,
   L_UINT uBitmapArrayCount,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   FILESAVECALLBACK pfnCallback,
   L_VOID* pUserData,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_EnumFileTags(
   L_TCHAR* pszFile,
   L_UINT uFlags,
   ENUMTAGSCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_DeleteTag(
   L_TCHAR* pszFile,
   L_INT nPage,
   L_UINT uTag,
   L_UINT uFlags,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileGeoKey(
   L_TCHAR* pszFile,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileGeoKey(
   L_TCHAR* pszFile,
   L_UINT16 uTag,
   L_UINT* puType,
   L_UINT* pCount,
   L_VOID* pData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_EnumFileGeoKeys(
   L_TCHAR* pszFile,
   L_UINT uFlags,
   ENUMGEOKEYSCALLBACK pfnCallback,
   L_VOID* pUserData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SetOverlayCallback(
   OVERLAYCALLBACK pfnCallback,
   L_VOID* pUserData,
   L_UINT uFlags
);

L_LTFIL_API L_INT EXT_FUNCTION L_GetOverlayCallback(
   OVERLAYCALLBACK *ppfnCallback,
   L_VOID** ppUserData,
   L_UINT* puFlags
);
#else
#define L_DeletePage L_DeletePageA
#define L_ReadLoadResolutions L_ReadLoadResolutionsA
#define L_FileConvert L_FileConvertA
#define L_FileInfo L_FileInfoA
#define L_FileInfoMemory L_FileInfoMemoryA
#define L_GetFileCommentSize L_GetFileCommentSizeA
#define L_LoadBitmap L_LoadBitmapA
#define L_LoadBitmapList L_LoadBitmapListA
#define L_LoadBitmapMemory L_LoadBitmapMemoryA
#define L_LoadFile L_LoadFileA
#define L_LoadFileTile L_LoadFileTileA
#define L_LoadMemoryTile L_LoadMemoryTileA
#define L_LoadFileOffset L_LoadFileOffsetA
#define L_LoadMemory L_LoadMemoryA
#define L_ReadFileComment L_ReadFileCommentA
#define L_ReadFileCommentExt L_ReadFileCommentExtA
#define L_ReadFileTag L_ReadFileTagA
#define L_ReadFileStamp L_ReadFileStampA
#define L_SaveBitmap L_SaveBitmapA
#define L_SaveBitmapBuffer L_SaveBitmapBufferA
#define L_SaveBitmapList L_SaveBitmapListA
#define L_SaveBitmapMemory L_SaveBitmapMemoryA
#define L_SaveFile L_SaveFileA
#define L_SaveFileBuffer L_SaveFileBufferA
#define L_SaveFileMemory L_SaveFileMemoryA
#define L_SaveFileTile L_SaveFileTileA
#define L_SaveFileOffset L_SaveFileOffsetA
#define L_StartCompressBuffer L_StartCompressBufferA
#define L_StartFeedLoad L_StartFeedLoadA
#define L_FeedLoad L_FeedLoadA
#define L_StopFeedLoad L_StopFeedLoadA
#define L_StartFeedInfo L_StartFeedInfoA
#define L_FeedInfo L_FeedInfoA
#define L_StopFeedInfo L_StopFeedInfoA
#define L_WriteFileCommentExt L_WriteFileCommentExtA
#define L_WriteFileStamp L_WriteFileStampA
#define L_GetDefaultSaveFileOption L_GetDefaultSaveFileOptionA
#define L_WriteFileTag L_WriteFileTagA
#define L_WriteFileComment L_WriteFileCommentA
#define L_CreateThumbnailFromFile L_CreateThumbnailFromFileA
#define L_TransformFile L_TransformFileA
#define L_ReadFileExtensions L_ReadFileExtensionsA
#define L_StartDecompressBuffer L_StartDecompressBufferA
#define L_IgnoreFilters L_IgnoreFiltersA
#define L_PreLoadFilters L_PreLoadFiltersA
#define L_GetIgnoreFilters L_GetIgnoreFiltersA
#define L_GetPreLoadFilters L_GetPreLoadFiltersA
#define L_LoadMarkers L_LoadMarkersA
#define L_WriteFileMetaData L_WriteFileMetaDataA
#define L_StartSaveData L_StartSaveDataA
#define L_GetFilterListInfo L_GetFilterListInfoA
//#define L_GetFilterInfo L_GetFilterInfoA
//#define L_FreeFilterInfo L_FreeFilterInfoA
//#define L_SetFilterInfo L_SetFilterInfoA
#define L_GetTXTOptions L_GetTXTOptionsA
#define L_SetTXTOptions L_SetTXTOptionsA
#define L_CompactFile L_CompactFileA
#define L_LoadFileCMYKArray L_LoadFileCMYKArrayA
#define L_SaveFileCMYKArray L_SaveFileCMYKArrayA
#define L_EnumFileTags L_EnumFileTagsA
#define L_DeleteTag L_DeleteTagA
#define L_WriteFileGeoKey L_WriteFileGeoKeyA
#define L_ReadFileGeoKey L_ReadFileGeoKeyA
#define L_EnumFileGeoKeys L_EnumFileGeoKeysA
#define L_SetOverlayCallback L_SetOverlayCallbackA
#define L_GetOverlayCallback L_GetOverlayCallbackA
#define L_ReadFileTags L_ReadFileTagsA
#define L_ReadFileGeoKeys L_ReadFileGeoKeysA
#define L_ReadFileComments L_ReadFileCommentsA

#endif // #if defined(FOR_UNICODE)

// These functions not ported to Windows CE
#if !defined(FOR_WINCE)
// functions2
L_LTFIL_API L_INT EXT_FUNCTION L_GetWMFResolution(
   L_INT* lpXResolution,
   L_INT* lpYResolution);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPCDResolution(
   L_INT nResolution);

L_LTFIL_API L_INT EXT_FUNCTION L_SetWMFResolution(
   L_INT nXResolution,
   L_INT nYResolution);

L_LTFIL_API L_INT EXT_FUNCTION L_2DSetViewport(
   L_INT nWidth,
   L_INT nHeight);

L_LTFIL_API L_INT EXT_FUNCTION L_2DGetViewport(
   L_INT* pnWidth,
   L_INT* pnHeight);

L_LTFIL_API L_INT EXT_FUNCTION L_2DSetViewMode(
   L_INT nViewMode);

L_LTFIL_API L_INT EXT_FUNCTION L_2DGetViewMode(
   L_VOID);

L_LTFIL_API L_INT EXT_FUNCTION L_VecFeedLoad(
   L_HANDLE hLoad,
   L_UCHAR* pInBuffer,
   L_SIZE_T dwBufferSize);

L_LTFIL_API L_INT EXT_FUNCTION L_VecStopFeedLoad(
   L_HANDLE hLoad);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPLTOptions(
   pFILEPLTOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPLTOptions(
   pFILEPLTOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetRTFOptions(
   pFILERTFOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetRTFOptions(
   pFILERTFOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPTKOptions(
   pFILEPTKOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPTKOptions(
   pFILEPTKOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPDFSaveOptions(
   pFILEPDFSAVEOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPDFSaveOptions(
   pFILEPDFSAVEOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetDJVOptions(
   pFILEDJVOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetDJVOptions(
   pFILEDJVOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetXPSOptions(
   pFILEXPSOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetXPSOptions(
   pFILEXPSOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetXLSOptions(
   pFILEXLSOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetXLSOptions(
   pFILEXLSOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetRasterizeDocOptions(
   pRASTERIZEDOCOPTIONS pRasterizeDocOptions,
   L_UINT uStructSize);
L_LTFIL_API L_INT EXT_FUNCTION L_SetRasterizeDocOptions(
   pRASTERIZEDOCOPTIONS pRasterizeDocOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetAutoCADFilesColorScheme(
   L_UINT* dwFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_SetAutoCADFilesColorScheme(
   L_UINT dwFlags);

L_LTFIL_API L_INT EXT_FUNCTION L_VecAddFontMapper(
   VECTORFONTMAPPERCALLBACK pMapper,
   L_VOID* pUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_VecRemoveFontMapper(
   VECTORFONTMAPPERCALLBACK pMapper);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapResizeA(
   L_CHAR* pszFile,             // name of the file to load 
   pBITMAPHANDLE pSmallBitmap,   // pointer to the target bitmap handle 
   L_UINT uStructSize,
   L_INT nDestWidth,             // new width of the image 
   L_INT nDestHeight,            // new height of the image 
   L_INT nDestBits,              // new bits per pixel for the image 
   L_UINT uFlags,                // SIZE_NORMAL, SIZE_RESAMPLE SIZE_BICUBIC	
   L_INT nOrder,                 // color order for 16-, 24-, 32-, 48, and 64-bit bitmaps 
   pLOADFILEOPTION pLoadOptions, // pointer to optional extended load options 
   pFILEINFOA pFileInfo);         // pointer to a structure 

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTransformsA(
   L_CHAR* pszFile,
   pFILETRANSFORMS pTransforms,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileTransformsA(
   L_CHAR* pszFile,
   pFILETRANSFORMS pTransforms,
   L_INT nFlags,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPCDResolutionA(
   L_CHAR* pszFile,
   pPCDINFO pPCDInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveCustomFileA(
   L_CHAR* pszFilename,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   pSAVEFILEOPTIONA pSaveOptions,
   pSAVECUSTOMFILEOPTION pSaveCustomFileOption,
   SAVECUSTOMFILECALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadCustomFileA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACKA pfnFileReadCallback,
   L_VOID* pFileReadCallbackUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo,
   pLOADCUSTOMFILEOPTION pLoadCustomFileOption,
   LOADCUSTOMFILECALLBACK pfnLoadCustomFileCallback,
   L_VOID* pCustomCallbackUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_VecLoadFileA(
   L_CHAR* pszFile,
   pVECTORHANDLE pVector,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecLoadMemoryA(
   L_UCHAR* pBuffer,
   pVECTORHANDLE pVector,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecStartFeedLoadA(
   pVECTORHANDLE pVector,
   L_HANDLE*phLoad,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFOA pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecSaveFileA(
   L_CHAR* pszFile,
   pVECTORHANDLE pVector,
   L_INT nFormat,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_VecSaveMemoryA(
   L_HANDLE*hHandle,
   pVECTORHANDLE pVector,
   L_INT nFormat,
   L_SIZE_T* uSize,
   pSAVEFILEOPTIONA pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPDFInitDirA(
   L_CHAR* pszInitDir,
   L_SIZE_T uBufSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPDFInitDirA(
   L_CHAR* pszInitDir);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPDFOptionsA(
   pFILEPDFOPTIONSA pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPDFOptionsA(
   pFILEPDFOPTIONSA pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadLayerA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_INT nLayer,
   pLAYERINFOA pLayerInfo,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapWithLayersA(
   L_CHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   HBITMAPLIST hLayers,
   pLAYERINFOA pLayerInfo,
   L_INT nLayers,
   pSAVEFILEOPTIONA pSaveOptions);

#if defined(LEADTOOLS_V16_OR_LATER)
L_LTFIL_API L_INT EXT_FUNCTION L_LoadChannelA(L_CHAR* pszFile,            /* name of a file */ 
                                              pBITMAPHANDLE pBitmap,      /* pointer to the target bitmap handle */
                                              L_UINT uStructSize,         /* size in bytes, of the structure pointed to by pBitmap */
                                              L_INT nBitsPerPixel,
                                              L_INT nOrder,
                                              L_INT nChannel,             /* index of the channel to load */
                                              pCHANNELINFOA pChannelInfo, /* pointer to CHANNELINFO structure */
                                              pLOADFILEOPTION pLoadOptions);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#if defined(FOR_UNICODE)
L_LTFIL_API L_INT EXT_FUNCTION L_LoadBitmapResize(
   L_TCHAR* pszFile,             // name of the file to load 
   pBITMAPHANDLE pSmallBitmap,   // pointer to the target bitmap handle 
   L_UINT uStructSize,
   L_INT nDestWidth,             // new width of the image 
   L_INT nDestHeight,            // new height of the image 
   L_INT nDestBits,              // new bits per pixel for the image 
   L_UINT uFlags,                // SIZE_NORMAL, SIZE_RESAMPLE SIZE_BICUBIC	
   L_INT nOrder,                 // color order for 16-, 24-, 32-, 48, and 64-bit bitmaps 
   pLOADFILEOPTION pLoadOptions, // pointer to optional extended load options 
   pFILEINFO pFileInfo);         // pointer to a structure 

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTransforms(
   L_TCHAR* pszFile,
   pFILETRANSFORMS pTransforms,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_WriteFileTransforms(
   L_TCHAR* pszFile,
   pFILETRANSFORMS pTransforms,
   L_INT nFlags,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPCDResolution(
   L_TCHAR* pszFile,
   pPCDINFO pPCDInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveCustomFile(
   L_TCHAR* pszFilename,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   L_UINT uFlags,
   pSAVEFILEOPTION pSaveOptions,
   pSAVECUSTOMFILEOPTION pSaveCustomFileOption,
   SAVECUSTOMFILECALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadCustomFile(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_UINT uFlags,
   FILEREADCALLBACK pfnFileReadCallback,
   L_VOID* pFileReadCallbackUserData,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo,
   pLOADCUSTOMFILEOPTION pLoadCustomFileOption,
   LOADCUSTOMFILECALLBACK pfnLoadCustomFileCallback,
   L_VOID* pCustomCallbackUserData);

L_LTFIL_API L_INT EXT_FUNCTION L_VecLoadFile(
   L_TCHAR* pszFile,
   pVECTORHANDLE pVector,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecLoadMemory(
   L_UCHAR* pBuffer,
   pVECTORHANDLE pVector,
   L_SSIZE_T nBufferSize,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecStartFeedLoad(
   pVECTORHANDLE pVector,
   L_HANDLE*phLoad,
   pLOADFILEOPTION pLoadOptions,
   pFILEINFO pFileInfo);

L_LTFIL_API L_INT EXT_FUNCTION L_VecSaveFile(
   L_TCHAR* pszFile,
   pVECTORHANDLE pVector,
   L_INT nFormat,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_VecSaveMemory(
   L_HANDLE*hHandle,
   pVECTORHANDLE pVector,
   L_INT nFormat,
   L_SIZE_T* uSize,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPDFInitDir(
   L_TCHAR* pszInitDir,
   L_SIZE_T uBufSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPDFInitDir(
   L_TCHAR* pszInitDir);

L_LTFIL_API L_INT EXT_FUNCTION L_GetPDFOptions(
   pFILEPDFOPTIONS pOptions,
   L_UINT uStructSize);

L_LTFIL_API L_INT EXT_FUNCTION L_SetPDFOptions(
   pFILEPDFOPTIONS pOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_LoadLayer(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_INT nOrder,
   L_INT nLayer,
   pLAYERINFO pLayerInfo,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_SaveBitmapWithLayers(
   L_TCHAR* pszFile,
   pBITMAPHANDLE pBitmap,
   L_INT nFormat,
   L_INT nBitsPerPixel,
   L_INT nQFactor,
   HBITMAPLIST hLayers,
   pLAYERINFO pLayerInfo,
   L_INT nLayers,
   pSAVEFILEOPTION pSaveOptions);

L_LTFIL_API L_BOOL EXT_FUNCTION L_TagsSupported(
   L_INT nFormat);

L_LTFIL_API L_BOOL EXT_FUNCTION L_GeoKeysSupported(
   L_INT nFormat);

L_LTFIL_API L_BOOL EXT_FUNCTION L_CommentsSupported(
   L_INT nFormat);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileTags(
   L_TCHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puTagCount,
   pLEADFILETAG* ppTags,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileGeoKeys(
   L_TCHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puTagCount,
   pLEADFILETAG* ppTags,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FreeFileTags(
   L_UINT uTagCount,
   pLEADFILETAG pTags,
   L_SIZE_T uDataSize,
   L_UCHAR* pData);

L_LTFIL_API L_INT EXT_FUNCTION L_ReadFileComments(
   L_TCHAR* pszFile,
   L_UINT uFlags,
   L_UINT* puCommentCount,
   pLEADFILECOMMENT* ppComments,
   L_SIZE_T* puDataSize,
   L_UCHAR** ppData,
   pLOADFILEOPTION pLoadOptions);

L_LTFIL_API L_INT EXT_FUNCTION L_FreeFileComments(
   L_UINT uCommentCount,
   pLEADFILECOMMENT pComments,
   L_SIZE_T uDataSize,
   L_UCHAR* pData);

#if defined(LEADTOOLS_V16_OR_LATER)
L_LTFIL_API L_INT EXT_FUNCTION L_LoadChannel(L_TCHAR* pszFile,           /* name of a file */ 
                                             pBITMAPHANDLE pBitmap,      /* pointer to the target bitmap handle */
                                             L_UINT uStructSize,         /* size in bytes, of the structure pointed to by pBitmap */
                                             L_INT nBitsPerPixel,
                                             L_INT nOrder,
                                             L_INT nChannel,             /* index of the channel to load */
                                             pCHANNELINFO pChannelInfo,  /* pointer to CHANNELINFO structure */
                                             pLOADFILEOPTION pLoadOptions);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#else
#define L_LoadBitmapResize L_LoadBitmapResizeA
#define L_ReadFileTransforms L_ReadFileTransformsA
#define L_WriteFileTransforms L_WriteFileTransformsA
#define L_GetPCDResolution L_GetPCDResolutionA
#define L_SaveCustomFile L_SaveCustomFileA
#define L_LoadCustomFile L_LoadCustomFileA
#define L_VecLoadFile L_VecLoadFileA
#define L_VecLoadMemory L_VecLoadMemoryA
#define L_VecStartFeedLoad L_VecStartFeedLoadA
#define L_VecSaveFile L_VecSaveFileA
#define L_VecSaveMemory L_VecSaveMemoryA
#define L_GetPDFInitDir L_GetPDFInitDirA
#define L_SetPDFInitDir L_SetPDFInitDirA
#define L_GetPDFOptions L_GetPDFOptionsA
#define L_SetPDFOptions L_SetPDFOptionsA
#define L_LoadLayer L_LoadLayerA
#define L_SaveBitmapWithLayers L_SaveBitmapWithLayersA
#if defined(LEADTOOLS_V16_OR_LATER)
#define L_LoadChannel L_LoadChannelA
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#endif // #if defined(FOR_UNICODE)

#endif // #if !defined(FOR_WINCE)

#endif // #if !defined(FOR_MANAGED) || defined(FOR_MANAGED_CODEC)

#if defined(FOR_WINCE)
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterCMP(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterCMW(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterBMP(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterTIF(L_VOID);   // TIF requires FAX
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterFAX(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterGIF(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterJ2K(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterTGA(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterPNG(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterPCX(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterXBM(L_VOID);
   L_LTFIL_API L_VOID EXT_FUNCTION L_RegisterXPM(L_VOID);
#endif // #if defined(FOR_WINCE)

#if defined(FOR_JUSTLIB)
   #if defined(__cplusplus)
      #define USE_FILTER(name)                     \
      extern "C" L_INT fltInfo##name(L_VOID*ptr);  \
      void USE_FILTER_##name()                     \
      {                                            \
         fltInfo##name(NULL);                      \
      }
   #else
      #define USE_FILTER(name)                     \
      extern L_INT fltInfo##name(L_VOID*ptr);      \
      void USE_FILTER_##name()                     \
      {                                            \
         fltInfo##name(NULL);                      \
      }
   #endif // #if defined(__cplusplus)
#endif // #if defined(FOR_JUSTLIB)

#endif // #if !defined(LEAD_DEFINES_ONLY)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTFIL_H)
