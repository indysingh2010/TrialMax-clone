; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CMainView
LastTemplate=CDaoRecordset
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "TmaxPresentation.h"
ODLFile=TmaxPresentation.odl
LastPage=0

ClassCount=31
Class1=CApp
Class2=CTMDocument
Class3=CMainView
Class4=CMainFrame
Class5=CSetup
Class6=CToolbars
Class7=CSplashBox
Class8=CTranscriptSet

ResourceCount=7
Resource1=IDD_SETUP
Resource2=IDD_TOOLBARS
Resource3=IDD_TMAXPRESENTATION_FORM
Resource4=IDD_SPLASHBOX
Resource5=IDD_CONFIRM_EXIT
Resource6=IDD_SETLINE
Class9=CMediaSet
Class10=CVersionSet
Class11=CPageSet
Class12=CTreatmentSet
Class13=CVideoSet
Class14=CDesignationSet
Class15=CLinkSet
Class16=CTextSet
Class17=CSetLine
Class18=CShowItemSet
Class19=CBarcodeMapSet
Class20=CColorSet
Class21=CDLTranscriptSet
Class22=CDBPrimary
Class23=CDBDetails
Class24=CDBUsers
Class25=CDBHighlighters
Class26=CDBExtents
Class27=CDBTranscripts
Class28=CDBSecondary
Class29=CDBTertiary
Class30=CDBQuaternary
Class31=CDBBarcodeMap
Resource7=IDR_MAINFRAME

[CLS:CTMDocument]
Type=0
HeaderFile=include\Document.h
ImplementationFile=source\Document.cpp
BaseClass=CDocument
LastObject=CTMDocument

[CLS:CMainView]
Type=0
HeaderFile=include\View.h
ImplementationFile=source\View.cpp
BaseClass=CFormView
LastObject=ID_ZAP_NEXT
Filter=D
VirtualFilter=VWC

[CLS:CMainFrame]
Type=0
HeaderFile=include\Frame.h
ImplementationFile=source\Frame.cpp
BaseClass=CFrameWnd
LastObject=CMainFrame
Filter=T
VirtualFilter=fWC

[DLG:IDD_TMAXPRESENTATION_FORM]
Type=1
Class=CMainView
ControlCount=17
Control1=IDC_TMSTATCTRL,{0C69F0D1-9BB0-4DB0-A600-D98621E8D8B3},1073741824
Control2=IDC_TMLPENCTRL,{7EFCBDC0-F749-4574-8DC1-2E5575DD9808},1073807360
Control3=IDC_DOCUMENTS,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control4=IDC_LINKS,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control5=IDC_MOVIES,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control6=IDC_GRAPHICS,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control7=IDC_PLAYLISTS,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control8=IDC_TEXT,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control9=IDC_DRAWINGTOOLS,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control10=IDC_POWERPOINT,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control11=IDC_LINKPOWER,{2341B5A2-769B-49CC-8652-B8914992AFB1},1073807360
Control12=IDC_TMPOWERCTRL,{BD138FDB-21B2-4CF1-8175-A94182FED781},1073807360
Control13=IDC_TMTEXTCTRL,{AA52288D-2A50-494F-98FE-FFF0D9FBDE56},1073807360
Control14=IDC_TMMOVIECTRL,{D71D2494-B9CA-401F-8E24-1815E077CE64},1073807360
Control15=IDC_TMVIEWCTRL,{5A3A9FC9-D747-4B92-9106-A32C7E6E84A3},1073807360
Control16=IDC_TMGRABCTRL,{4BA3488C-31EC-4619-9D96-1EFE592DD861},1342242816
Control17=IDC_TMSHARE,{CB5D5073-AB77-45F6-B728-1808DDC80026},1342242816

[MNU:IDR_MAINFRAME]
Type=1
Class=CMainView
Command1=ID_FILE_PRINT
Command2=ID_FILE_PRINT
Command3=ID_APP_EXIT
Command4=ID_SHOW_TOOLBAR
Command5=ID_CLEAR
Command6=ID_SPLIT_VERTICAL
Command7=ID_SPLIT_HORIZONTAL
Command8=ID_SHADEONCALLOUT
Command9=ID_STATUSBAR
Command10=ID_CONFIG
Command11=ID_TMTOOL
Command12=ID_FILTERPROPS
Command13=ID_ZOOM
Command14=ID_ZOOMWIDTH
Command15=ID_ZOOM_RESTRICTED
Command16=ID_HIGHLIGHT
Command17=ID_REDACT
Command18=ID_DRAW
Command19=ID_PAN
Command20=ID_CALLOUT
Command21=ID_SELECT
Command22=ID_ROTATE_CW
Command23=ID_ROTATE_CCW
Command24=ID_SELECTTOOL
Command25=ID_DELETEANN
Command26=ID_ERASE
Command27=ID_NEXTMEDIA
Command28=ID_PREVMEDIA
Command29=ID_SCREEN_CAPTURE
Command30=ID_CAPTURE_BARCODES
Command31=ID_PLAY
Command32=ID_PLAYTHROUGH
Command33=ID_VIDEOCAPTION
Command34=ID_DISABLELINKS
Command35=ID_TEXT
Command36=ID_FULLSCREEN
Command37=ID_FIRSTDESIGNATION
Command38=ID_LASTDESIGNATION
Command39=ID_NEXTDESIGNATION
Command40=ID_PREVDESIGNATION
Command41=ID_BACKDESIGNATION
Command42=ID_FWDDESIGNATION
Command43=ID_STARTDESIGNATION
Command44=ID_SETPAGELINE
Command45=ID_SETPAGELINENEXT
Command46=ID_STARTMOVIE
Command47=ID_ENDMOVIE
Command48=ID_BACKMOVIE
Command49=ID_FWDMOVIE
Command50=ID_ZAP_SAVE
Command51=ID_ZAP_SAVE_SPLIT
Command52=ID_ZAP_UPDATE
Command53=ID_ZAP_FIRST
Command54=ID_ZAP_NEXT
Command55=ID_ZAP_PREVIOUS
Command56=ID_ZAP_LAST
Command57=ID_TMAX_HELP
Command58=ID_APP_ABOUT
Command59=ID_NEXT
Command60=ID_PREVIOUS
Command61=ID_FIRSTPAGE
Command62=ID_LASTPAGE
Command63=ID_HORIZONTAL_NEXT
Command64=ID_VERTICAL_NEXT
Command65=ID_SPLITPAGES_FIRST
Command66=ID_SPLIT_PAGES_PREVIOUS
Command67=ID_SPLITPAGES_NEXT
Command68=ID_SPLITPAGES_LAST
Command69=ID_DRAWRED
Command70=ID_DRAWGREEN
Command71=ID_DRAWBLUE
Command72=ID_DRAWYELLOW
Command73=ID_DRAWBLACK
Command74=ID_DRAWWHITE
Command75=ID_DRAWDARKRED
Command76=ID_DRAWDARKGREEN
Command77=ID_DRAWDARKBLUE
Command78=ID_DRAWLIGHTRED
Command79=ID_DRAWLIGHTGREEN
Command80=ID_DRAWLIGHTBLUE
Command81=ID_FREEHAND
Command82=ID_LINE
Command83=ID_ARROW
Command84=ID_ELLPSE
Command85=ID_RECTANGLE
Command86=ID_FILLEDELLIPSE
Command87=ID_FILLEDRECTANGLE
Command88=ID_POLYLINE
Command89=ID_POLYGON
Command90=ID_ANNTEXT
Command91=ID_FIRST_SHOW_ITEM
Command92=ID_NEXT_SHOW_ITEM
Command93=ID_PREV_SHOW_ITEM
Command94=ID_LAST_SHOW_ITEM
Command95=ID_MOUSE_MODE
CommandCount=95

[TB:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_PREVIOUS
Command2=ID_NEXT
Command3=ID_ROTATE_CW
Command4=ID_ROTATE_CCW
Command5=ID_NORMAL
Command6=ID_ZOOM
Command7=ID_HIGHLIGHT
Command8=ID_REDACT
Command9=ID_DRAW
Command10=ID_ERASE
Command11=ID_CONFIG
Command12=ID_APP_EXIT
CommandCount=12

[ACL:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_APP_EXIT
Command2=ID_CAPTURE_BARCODES
Command3=ID_APP_EXIT
CommandCount=3

[DLG:IDD_SPLASHBOX]
Type=1
Class=CSplashBox
ControlCount=1
Control1=IDC_BITMAP,static,1342177806

[CLS:CSplashBox]
Type=0
HeaderFile=..\common\include\splash.h
ImplementationFile=..\common\source\splash.cpp
BaseClass=CDialog
Filter=D
LastObject=CSplashBox
VirtualFilter=dWC

[DLG:IDD_CONFIRM_EXIT]
Type=1
Class=?
ControlCount=4
Control1=IDCANCEL,button,1342242817
Control2=IDOK,button,1342242816
Control3=IDC_STATIC,static,1342177283
Control4=IDC_STATIC,static,1342308353

[DLG:IDD_TOOLBARS]
Type=1
Class=CToolbars
ControlCount=3
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_TMBARS,{5284E5B7-9E77-4200-9E9F-D5F22CB40F2C},1342242816

[CLS:CToolbars]
Type=0
HeaderFile=include\Toolbars.h
ImplementationFile=source\Toolbars.cpp
BaseClass=CDialog
Filter=D
LastObject=CToolbars
VirtualFilter=dWC

[DLG:IDD_SETUP]
Type=1
Class=CSetup
ControlCount=3
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_TMSETUP,{B581682E-5CC0-4E50-BBBC-582D78677E5A},1342242816

[CLS:CSetup]
Type=0
HeaderFile=include\Setup.h
ImplementationFile=source\Setup.cpp
BaseClass=CDialog
Filter=D
VirtualFilter=dWC
LastObject=CSetup

[CLS:CMediaSet]
Type=0
HeaderFile=..\common\include\tmdb45\Mediaset.h
ImplementationFile=..\common\source\tmdb45\Mediaset.cpp
BaseClass=CDaoRecordset
LastObject=CMediaSet
Filter=N
VirtualFilter=x

[DB:CMediaSet]
DB=1
DBType=DAO
ColumnCount=8
Column1=[MediaID], 12, 12
Column2=[GhostMediaId], 4, 4
Column3=[MediaPlayerType], 4, 4
Column4=[MediaName], 12, 50
Column5=[RelativePath], 12, 255
Column6=[FileName], 12, 50
Column7=[UseTranscript], -7, 1
Column8=[FlagsBinary], 4, 4

[CLS:CVersionSet]
Type=0
HeaderFile=..\common\include\tmdb45\Verset.h
ImplementationFile=..\common\source\tmdb45\Verset.cpp
BaseClass=CDaoRecordset
LastObject=CVersionSet
Filter=N
VirtualFilter=x

[DB:CVersionSet]
DB=1
DBType=DAO
ColumnCount=5
Column1=[Key], 4, 4
Column2=[CurrentTrialMaxDbVersion], 12, 15
Column3=[CurrentViewerDbVersion], 12, 15
Column4=[OriginalTrialMaxDbVersion], 12, 15
Column5=[CreatedBy], 12, 255

[CLS:CPageSet]
Type=0
HeaderFile=..\common\include\tmdb45\Pageset.h
ImplementationFile=..\common\source\tmdb45\Pageset.cpp
BaseClass=CDaoRecordset
LastObject=CPageSet

[DB:CPageSet]
DB=1
DBType=DAO
ColumnCount=6
Column1=[MediaID], 12, 12
Column2=[PageID], 4, 4
Column3=[PlaybackOrder], 4, 4
Column4=[DisplayType], 4, 4
Column5=[FileName], 12, 50
Column6=[SlideID], 4, 4

[CLS:CTreatmentSet]
Type=0
HeaderFile=..\common\include\tmdb45\Treatset.h
ImplementationFile=..\common\source\tmdb45\Treatset.cpp
BaseClass=CDaoRecordset
LastObject=CTreatmentSet

[DB:CTreatmentSet]
DB=1
DBType=DAO
ColumnCount=7
Column1=[MediaID], 12, 12
Column2=[PageID], 4, 4
Column3=[TreatmentID], 4, 4
Column4=[Description], 12, 50
Column5=[PlaybackOrder], 4, 4
Column6=[RelativePath], 12, 255
Column7=[FileName], 12, 50

[CLS:CVideoSet]
Type=0
HeaderFile=..\common\include\tmdb45\Videoset.h
ImplementationFile=..\common\source\tmdb45\Videoset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CVideoSet

[DB:CVideoSet]
DB=1
DBType=DAO
ColumnCount=12
Column1=[VideoFileID], 4, 4
Column2=[TranscriptID], 4, 4
Column3=[RelativePath], 12, 255
Column4=[UnitType], 4, 4
Column5=[FileName], 12, 50
Column6=[BeginNum], 4, 4
Column7=[EndNum], 4, 4
Column8=[MinSelStart], 4, 4
Column9=[MaxSelStart], 4, 4
Column10=[BeginTuned], -7, 1
Column11=[EndTuned], -7, 1
Column12=[RootOverride], 12, 255

[CLS:CDesignationSet]
Type=0
HeaderFile=..\common\include\tmdb45\Desgset.h
ImplementationFile=..\common\source\tmdb45\Desgset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDesignationSet

[DB:CDesignationSet]
DB=1
DBType=DAO
ColumnCount=21
Column1=[MediaID], 12, 12
Column2=[DesignationID], 4, 4
Column3=[TranscriptID], 4, 4
Column4=[Description], 12, 50
Column5=[PlaybackOrder], 4, 4
Column6=[ColorID], 4, 4
Column7=[DisplayType], 4, 4
Column8=[StartPage], 4, 4
Column9=[StartLine], 4, 4
Column10=[StopPage], 4, 4
Column11=[StopLine], 4, 4
Column12=[SelStart], 4, 4
Column13=[SelLength], 4, 4
Column14=[StartNum], 4, 4
Column15=[StopNum], 4, 4
Column16=[VideoFileID], 4, 4
Column17=[StartTuned], -7, 1
Column18=[StopTuned], -7, 1
Column19=[HasObjections], -7, 1
Column20=[OverlayRelativePath], 12, 50
Column21=[OverlayFileName], 12, 50

[DB:CDSet]
DB=1
DBType=DAO
ColumnCount=19
Column1=[MediaID], 12, 12
Column2=[DesignationID], 4, 4
Column3=[TranscriptID], 4, 4
Column4=[Description], 12, 50
Column5=[PlaybackOrder], 4, 4
Column6=[ColorID], 4, 4
Column7=[DisplayType], 4, 4
Column8=[StartPage], 4, 4
Column9=[StartLine], 4, 4
Column10=[StopPage], 4, 4
Column11=[StopLine], 4, 4
Column12=[SelStart], 4, 4
Column13=[SelLength], 4, 4
Column14=[StartNum], 4, 4
Column15=[StopNum], 4, 4
Column16=[VideoFileID], 4, 4
Column17=[StartTuned], -7, 1
Column18=[StopTuned], -7, 1
Column19=[HasObjections], -7, 1

[CLS:CLinkSet]
Type=0
HeaderFile=..\common\include\tmdb45\Linkset.h
ImplementationFile=..\common\source\tmdb45\Linkset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CLinkSet

[DB:CLinkSet]
DB=1
DBType=DAO
ColumnCount=10
Column1=[LinkID], 4, 4
Column2=[MediaID], 12, 12
Column3=[DesignationID], 4, 4
Column4=[DisplayType], 4, 4
Column5=[PageNum], 4, 4
Column6=[LineNum], 4, 4
Column7=[TriggerNum], 4, 4
Column8=[ItemBarcode], 12, 50
Column9=[HideLink], -7, 1
Column10=[SplitScreen], -7, 1

[CLS:CTextSet]
Type=0
HeaderFile=..\common\include\tmdb45\Textset.h
ImplementationFile=..\common\source\tmdb45\Textset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CTextSet

[DB:CTextSet]
DB=1
DBType=DAO
ColumnCount=7
Column1=[MediaID], 12, 12
Column2=[DesignationID], 4, 4
Column3=[PageNum], 4, 4
Column4=[LineNum], 4, 4
Column5=[TextLine], 12, 75
Column6=[FirstNum], 4, 4
Column7=[LastNum], 4, 4

[CLS:CTranscriptSet]
Type=0
HeaderFile=..\common\include\tmdb45\Transet.h
ImplementationFile=..\common\source\tmdb45\Transet.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CTranscriptSet

[DB:CTranscriptSet]
DB=1
DBType=DAO
ColumnCount=7
Column1=[TranscriptID], 4, 4
Column2=[TranscriptName], 12, 50
Column3=[TranscriptDate], 11, 8
Column4=[RelativePath], 12, 255
Column5=[BaseFileName], 12, 50
Column6=[OrigCtxFileExtension], 12, 4
Column7=[OrigLogDbFileExtension], 12, 4

[DLG:IDD_SETLINE]
Type=1
Class=CSetLine
ControlCount=7
Control1=IDC_PAGE,edit,1350639744
Control2=IDC_LINE,edit,1350639744
Control3=IDC_STATIC,static,1342308352
Control4=IDC_STATIC,static,1342308352
Control5=IDC_LABEL,static,1342308352
Control6=IDC_MESSAGE,static,1342308352
Control7=IDC_TRANSCRIPTS,combobox,1344340227

[CLS:CSetLine]
Type=0
HeaderFile=include\Setline.h
ImplementationFile=source\Setline.cpp
BaseClass=CDialog
Filter=D
LastObject=CSetLine
VirtualFilter=dWC

[CLS:CShowItemSet]
Type=0
HeaderFile=..\common\include\tmdb45\Showset.h
ImplementationFile=..\common\source\tmdb45\Showset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CShowItemSet

[DB:CShowItemSet]
DB=1
DBType=DAO
ColumnCount=6
Column1=[MediaID], 12, 12
Column2=[ShowItemID], 4, 4
Column3=[Description], 12, 50
Column4=[PlaybackOrder], 4, 4
Column5=[ItemBarcode], 12, 50
Column6=[Hide], -7, 1

[CLS:CBarcodeMapSet]
Type=0
HeaderFile=..\common\include\tmdb45\bcmapset.h
ImplementationFile=..\common\source\tmdb45\bcmapset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CBarcodeMapSet

[DB:CBarcodeMapSet]
DB=1
DBType=DAO
ColumnCount=2
Column1=[ForeignCode], 12, 50
Column2=[Barcode], 12, 50

[CLS:CColorSet]
Type=0
HeaderFile=..\common\include\tmdb45\Colorset.h
ImplementationFile=..\common\source\tmdb45\Colorset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CColorSet

[DB:CColorSet]
DB=1
DBType=DAO
ColumnCount=4
Column1=[ColorID], 4, 4
Column2=[Description], 12, 20
Column3=[ColorRGB], 4, 4
Column4=[PlaintiffDefendant], 12, 1

[CLS:CDLTranscriptSet]
Type=0
HeaderFile=..\common\include\tmdata\Dltranset.h
ImplementationFile=..\common\source\tmdata\Dltranset.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDLTranscriptSet

[DB:CDLTranscriptSet]
DB=1
DBType=DAO
ColumnCount=4
Column1=[Key], 5, 2
Column2=[Page], 5, 2
Column3=[Line], 5, 2
Column4=[LineText], 12, 255

[CLS:CDBPrimary]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbprimary.h
ImplementationFile=..\common\source\tmdbnet\Dbprimary.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBPrimary

[DB:CDBPrimary]
DB=1
DBType=DAO
ColumnCount=17
Column1=[AutoId], 4, 4
Column2=[Children], 4, 4
Column3=[Attributes], 4, 4
Column4=[MediaType], 5, 2
Column5=[MediaId], 12, 255
Column6=[Exhibit], 12, 255
Column7=[RegisterPath], 12, 255
Column8=[AliasId], 4, 4
Column9=[RelativePath], 12, 255
Column10=[Filename], 12, 255
Column11=[Description], -1, 0
Column12=[AltBarcode], 12, 255
Column13=[Name], 12, 255
Column14=[CreatedBy], 4, 4
Column15=[CreatedOn], 11, 8
Column16=[ModifiedBy], 4, 4
Column17=[ModifiedOn], 11, 8

[CLS:CDBDetails]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbdetails.h
ImplementationFile=..\common\source\tmdbnet\Dbdetails.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBDetails

[DB:CDBDetails]
DB=1
DBType=DAO
ColumnCount=9
Column1=[AutoId], 4, 4
Column2=[MasterId], 12, 128
Column3=[DbMajor], 4, 4
Column4=[DbMinor], 4, 4
Column5=[DbBuild], 4, 4
Column6=[Name], 12, 50
Column7=[Description], -1, 0
Column8=[CreatedBy], 4, 4
Column9=[CreatedOn], 11, 8

[CLS:CDBUsers]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbusers.h
ImplementationFile=..\common\source\tmdbnet\Dbusers.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBUsers

[DB:CDBUsers]
DB=1
DBType=DAO
ColumnCount=4
Column1=[AutoId], 4, 4
Column2=[Name], 12, 50
Column3=[Description], -1, 0
Column4=[LastTime], 11, 8

[CLS:CDBHighlighters]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbhighlighters.h
ImplementationFile=..\common\source\tmdbnet\Dbhighlighters.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBHighlighters

[DB:CDBHighlighters]
DB=1
DBType=DAO
ColumnCount=8
Column1=[AutoId], 4, 4
Column2=[Color], 4, 4
Column3=[GroupId], 5, 2
Column4=[Name], 12, 255
Column5=[CreatedBy], 4, 4
Column6=[CreatedOn], 11, 8
Column7=[ModifiedBy], 4, 4
Column8=[ModifiedOn], 11, 8

[CLS:CDBExtents]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbextents.h
ImplementationFile=..\common\source\tmdbnet\Dbextents.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBExtents

[DB:CDBExtents]
DB=1
DBType=DAO
ColumnCount=11
Column1=[AutoId], 4, 4
Column2=[SecondaryId], 4, 4
Column3=[TertiaryId], 4, 4
Column4=[XmlSegmentId], 4, 4
Column5=[HighlighterId], 4, 4
Column6=[Start], 8, 8
Column7=[Stop], 8, 8
Column8=[StartTuned], -7, 1
Column9=[StopTuned], -7, 1
Column10=[StartPL], 4, 4
Column11=[StopPL], 4, 4

[CLS:CDBTranscripts]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbtranscripts.h
ImplementationFile=..\common\source\tmdbnet\Dbtranscripts.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBTranscripts

[DB:CDBTranscripts]
DB=1
DBType=DAO
ColumnCount=8
Column1=[AutoId], 4, 4
Column2=[PrimaryId], 4, 4
Column3=[Deponent], 12, 255
Column4=[DeposedOn], 12, 50
Column5=[Filename], 12, 255
Column6=[FirstPL], 4, 4
Column7=[LastPL], 4, 4
Column8=[LinesPerPage], 5, 2

[CLS:CDBSecondary]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbsecondary.h
ImplementationFile=..\common\source\tmdbnet\Dbsecondary.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBSecondary

[DB:CDBSecondary]
DB=1
DBType=DAO
ColumnCount=20
Column1=[AutoId], 4, 4
Column2=[PrimaryMediaId], 4, 4
Column3=[BarcodeId], 4, 4
Column4=[Children], 4, 4
Column5=[Attributes], 4, 4
Column6=[MediaType], 5, 2
Column7=[SourceType], 5, 2
Column8=[TransitionTime], 5, 2
Column9=[SourceId], 12, 100
Column10=[AliasId], 4, 4
Column11=[LinkedPath], 12, 255
Column12=[Filename], 12, 255
Column13=[MultipageId], 4, 4
Column14=[Description], -1, 0
Column15=[Name], 12, 255
Column16=[DisplayOrder], 4, 4
Column17=[CreatedBy], 4, 4
Column18=[CreatedOn], 11, 8
Column19=[ModifiedBy], 4, 4
Column20=[ModifiedOn], 11, 8

[CLS:CDBTertiary]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbtertiary.h
ImplementationFile=..\common\source\tmdbnet\Dbtertiary.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBTertiary

[DB:CDBTertiary]
DB=1
DBType=DAO
ColumnCount=16
Column1=[AutoId], 4, 4
Column2=[SecondaryMediaId], 4, 4
Column3=[BarcodeId], 4, 4
Column4=[Children], 4, 4
Column5=[Attributes], 4, 4
Column6=[MediaType], 5, 2
Column7=[Filename], 12, 255
Column8=[SourceId], 12, 255
Column9=[SourceType], 5, 2
Column10=[Description], -1, 0
Column11=[Name], 12, 255
Column12=[DisplayOrder], 4, 4
Column13=[CreatedBy], 4, 4
Column14=[CreatedOn], 11, 8
Column15=[ModifiedBy], 4, 4
Column16=[ModifiedOn], 11, 8

[CLS:CDBQuaternary]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbquaternary.h
ImplementationFile=..\common\source\tmdbnet\Dbquaternary.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBQuaternary

[DB:CDBQuaternary]
DB=1
DBType=DAO
ColumnCount=17
Column1=[AutoId], 4, 4
Column2=[TertiaryMediaId], 4, 4
Column3=[BarcodeId], 4, 4
Column4=[Attributes], 4, 4
Column5=[MediaType], 5, 2
Column6=[SourceId], 12, 255
Column7=[SourceType], 5, 2
Column8=[Description], -1, 0
Column9=[Name], 12, 255
Column10=[DisplayOrder], 4, 4
Column11=[StartPL], 4, 4
Column12=[Start], 8, 8
Column13=[StartTuned], -7, 1
Column14=[CreatedBy], 4, 4
Column15=[CreatedOn], 11, 8
Column16=[ModifiedBy], 4, 4
Column17=[ModifiedOn], 11, 8

[CLS:CDBBarcodeMap]
Type=0
HeaderFile=..\common\include\tmdbnet\Dbbarcodemap.h
ImplementationFile=..\common\source\tmdbnet\Dbbarcodemap.cpp
BaseClass=CDaoRecordset
Filter=N
VirtualFilter=x
LastObject=CDBBarcodeMap

[DB:CDBBarcodeMap]
DB=1
DBType=DAO
ColumnCount=2
Column1=[PSTQ], 12, 64
Column2=[ForeignCode], 12, 255

[CLS:CApp]
Type=0
HeaderFile=include\app.h
ImplementationFile=source\app.cpp
BaseClass=CWinApp
Filter=N
VirtualFilter=AC
LastObject=CApp

