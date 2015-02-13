using System;

namespace FTI.Shared.Trialmax
{
	/// <summary>Enumerations to identify TrialMax product components</summary>
	public enum TmaxComponents
	{
		TrialMax,	//	Applications,Assemblies,ActiveX,Configuration
		FTIORE,		//	FTI Objections Report Engine
		WMEncoder,	//	Windows Media Encoder (WMV)
		VideoViewer,//	TrialMax video viewer / script builder
	}
		
	/// <summary>Enumerations to identify TrialMax applications</summary>
	public enum TmaxApplications
	{
		TmaxManager,
		VideoViewer,
	}
		
	/// <summary>Enumerations to identify TrialMax registry keys</summary>
	public enum TmaxRegistryKeys
	{
		Root,		//	Top-level root key for all TrialMax entries in the registry
		Product,	//	Product component descriptors and information
		Activate,	//	Product activation key
		LastCase,	//	Path to last case (added by installer for demo database)
	}
		
	/// <summary>These enumerators identify the application panes</summary>
	public enum TmaxAppPanes
	{
		Errors = 0,
		Diagnostics,
		Media,
		Source,
		Binders,
		Viewer,
		Properties,
		Scripts,
		Transcripts,
		Tuner,
		Results,
		Help,
		Versions,
		Codes,
		FilteredTree,
		Objections,
		ObjectionProperties,
		ScriptReview,
		MaxPanes,
	};
		
	/// <summary>Enumerations to identify source types for registration</summary>
	public enum RegSourceTypes
	{
		NoSource,
		Document,
		Powerpoint,
		Recording,
		Deposition,
		Adobe,
		AllFiles,
		MultiPageTIFF,
	}
	
	/// <summary>Enumerations to establish methods for transferring source files on registration</summary>
	public enum RegFileTransfers
	{
		Copy,
		Move,
		Link,
	}
	
	/// <summary>Enumerations to establish methods for morphing source folder names prior to registration</summary>
	public enum RegMorphMethods
	{
		None,
		Offset,
		Prefix,
		Suffix,
	}
	
	/// <summary>Enumerations to establish methods for resolving name conflicts on registration</summary>
	public enum RegConflictResolutions
	{
		Prompt,
		Prefix,
		Suffix,
		Automatic,
	}
	
	/// <summary>Enumerations to select media creation options on registration</summary>
	public enum RegMediaCreations
	{
		OnePerFolder,
		Split,
		Merge,
	}
		
	/// <summary>Enumerations to select assignments of folder name on registration</summary>
	public enum RegFolderAssignments
	{
		MediaId,
		MediaName,
		Description,
	}
		
	/// <summary>Enumerations to select adjustments applied to automatically assigned Media Ids</summary>
	public enum RegMediaIdAdjustments
	{
		TruncateOnSpace,
		TruncateOnHyphen,
		StripZerosFirst,
		StripZerosAll,
	}
		
	/// <summary>Enumerations to select adjustments applied to automatically assigned Foreign Barcodes</summary>
	public enum RegForeignBarcodeAdjustments
	{
		AssignFromFilename,
		TruncateOnSpace,
		TruncateOnHyphen,
		StripZerosFirst,
		StripZerosAll,
	}
		
	/// <summary>Enumerations to define flags used during registration</summary>
	public enum RegFlags
	{
		IncludeSubfolders,
		PauseOnError,
		ShowOnDrop,
		UpdateSuperNodes,
		AssignFilenameToBarcode,
		MultiPageTiff,
	}
		
	/// <summary>Enumerated custom Windows message identifiers</summary>
	public enum TmaxWindowMessages
	{
		Invalid = (FTI.Shared.Win32.User.WM_USER + 0x00),
		Command = (FTI.Shared.Win32.User.WM_USER + 0x01),
		InstanceCommandLine = (FTI.Shared.Win32.User.WM_USER + 0x02),
		Max = (FTI.Shared.Win32.User.WM_USER + 0xFF),
	}

	/// <summary>Enumerations to identify TrialMax commands</summary>
	public enum TmaxCommands
	{
		StartDrag = 1,
		CompleteDrag,
		AcceptDrop,
		RegisterSource,
		Open,
		Add,
		Delete,
		Reorder,
		Update,
		Activate,
		Copy,
		Synchronize,
		Edit,
		Duplicate,
		SetCaseOptions,
		Print,
		Find,
		SetSearchResult,
		Import,
		Export,
		Merge,
		Rotate,
		Move,
		Help,
		SetCodes,
		SetFilter,
		SetPrimariesOrder,
		EditDesignation,
		RefreshCodes,
		AddNotification,
		EndNotification,
		QueryDatabase,
		HideCaseCodes,
		BulkUpdate,
		SetTargetBinder,
		SetDeposition,
		Navigate,
		NavigatorChanged,
        Preferences
	}
	
	/// <summary>Enumerations to identify parameters passed with TrialMax command events</summary>
	///	<remarks>The ToString() member of the enumeration should be used to reference the object in the collection</remarks>
	public enum TmaxCommandParameters
	{
		RegSourceType = 1,	//	Registration source type enumeration
		RegSourceCount,		//	Total number of files requesting registration
		Before,				//	True to insert item before existing media, false to insert after
		Activate,			//	True to activate the item after processing the command
		Viewer,				//	True to open the item in the viewer pane
		Builder,			//	True to open the item in the appropriate builder
		Tuner,				//	True to open the item in the video tuner
		Presentation,		//	True to open the item in Presentation application
		Properties,			//	True to open the item in the properties pane
		Codes,				//	True to open the item in the codes pane
		Transcripts,		//	True to open the item in the transcripts pane
		Recurse,			//	True to recurse into child collections
		SyncMediaTree,		//	True to sync the physical tree during activation
		SyncFilterTree,		//	True to sync the filter tree during activation
		SyncXml,			//	True to sync the associated XML file
		CounterClockwise,	//	True to rotate page counterclockwise
		UpdateFromFile,		//	True to update the record using the file selected by the user
		Shortcut,			//	True to add a shortcut to an existing record
		HelpManual,			//	Show the user's manual
		HelpContact,		//	Show the contact information
		HelpAbout,			//	Show the About information
		ImportFormat,		//	Enumerated import format for Import operations
		MergeImported,		//	True to merge imported files into one
		ExportFormat,		//	Enumerated export format for Export operations
		DrillChildren,		//	Drill down into child collections
		CodesAction,		//	Database action for SetCodes command
		SetFilterFlags,		//	Flags used to control the SetFilter command
		SetFilterText,		//	Text used by the SetFilter command
		StartPL,			//	Start page/line for Edit Designation command
		StopPL,				//	Stop page/line for Edit Designation command
		EditMethod,			//	Method for editing the specified designation
		Highlighter,		//	Highlighter to use for editing designations
		InitialPage,		//	Page to be displayed when setting preferences
		Filtered,			//	True to indicate Filtered primaries for SetPrimariesOrder command
		NoDuplicates,		//	Don't allow duplicates for the operation
		Objections,			//	The operation applies to objections
		NavigatorIndex,		//	The current index of the navigator
		NavigatorTotal,		//	The total number of records in the navigator's collection
		NavigatorRequest,	//	The request to move the navigator
		NavigatorMode,		//	The operating mode of the navigator
	}

	/// <summary>Enumerations to identify TrialMax hotkeys</summary>
	public enum TmaxHotkeys
	{
		None = 0,
		OpenHelp,				//	F1
		CaseOptions,			//	F2
		FindNext,				//	F3
		ViewProperties,			//	F4
		OpenPresentation,		//	F5
		ViewBuilder,			//	F6
		ViewTuner,				//	F7
		ViewMediaViewer,		//	F8
		ReloadCase,				//	F9
		ViewCodes,				//	F10
		ScreenCapture,			//	F11
		SetFilter,				//	F12
		Copy,					//	Control-Insert, Control-C
		Paste,					//	Shift-Insert, Control-V
		Delete,					//	Shift-Delete
		AddObjection,			//	Control-A
		AddToBinder,			//	Control-B
		Find,					//	Control-F
		OpenLast,				//	Control-L
		FileNew,				//	Control-N
		FileOpen,				//	Control-O
		Print,					//	Control-P
		AddToScript,			//	Control-S
		RefreshTreatments,		//	Control-T
		GoToBarcode,			//	Control-X
		GoTo,					//	Control-G
		Save,					//	Control-S
		BlankPresentation,		//	Control-F5
		FastFilter,				//	Control-F12
		RepeatObjection,		//	Control--
	}
	
	/// <summary>Enumerations to identify database record types</summary>
	public enum TmaxDataTypes
	{
		Unknown,
		Media,
		Binder,
		Highlighter,
		CaseCode,
		Code,
		PickItem,
		Objection,
	}
		
	/// <summary>Enumerations for search filter comparisons</summary>
	public enum TmaxFilterComparisons
	{
		LessThan,
		LessThanEquals,
		Equals,
		GreaterThanEquals,
		GreaterThan,
		StartsWith,
		Contains,
		EndsWith,
		AnyValue,
	}
		
	/// <summary>Enumerations for search filter modifiers</summary>
	public enum TmaxFilterModifiers
	{
		None,
		NOT,
	}
		
	/// <summary>Enumerations for search filter operators</summary>
	public enum TmaxFilterOperators
	{
		AND,
		OR,
	}
		
	/// <summary>Enumerations of flags used for SetFilter command</summary>
	[System.Flags] public enum TmaxSetFilterFlags
	{
		Advanced = 0x0001,
		PromptUser = 0x0002,
		Exclude = 0x0004,
	}
	
	/// <summary>Enumerations to identify fields used for fast filtering</summary>
	public enum TmaxFastFilterFields
	{
		All,
		Names,
		Descriptions,
	}
		
	/// <summary>Enumerations to identify code types</summary>
	public enum TmaxCodeTypes
	{
		Unknown,
		Integer,
		Decimal,
		Text,
		Memo,
		Boolean,
		Date,
		PickList,
	}
		
	/// <summary>Enumerations to identify constant codes that also serve as properties</summary>
	public enum TmaxCodedProperties
	{
		Invalid,
		Description,
		Exhibit,
		Admitted,
	}
		
	/// <summary>Enumerations to identify action for SetCodes command</summary>
	public enum TmaxCodeActions
	{
		Unknown,
		Add,
		Delete,
		Update,
	}
		
	/// <summary>Enumerations to identify attributes assigned to case codes</summary>
	[System.Flags] public enum TmaxCaseCodeAttributes
	{
		AllowMultiple = 0x0001,
		Hidden = 0x0002,
	}
	
	/// <summary>Enumerations to identify primary media types</summary>
	public enum TmaxMediaTypes
	{
		Unknown,
		Document,
		Powerpoint,
		Recording,
		Script,
		Page,
		Segment,
		Slide,
		Scene,
		Treatment,
		Clip,
		Deposition,
		Designation,
		Link,
	}
	
	/// <summary>Enumerations to identify media levels</summary>
	public enum TmaxMediaLevels
	{
		None,
		Primary,
		Secondary,
		Tertiary,
		Quaternary,
	}
	
	/// <summary>Enumerations to identify media relationships</summary>
	public enum TmaxMediaRelationships
	{
		None,
		Same,
		Parent,
		Child,
		Sibling,
		Grandchild,
		Grandparent,
	}
	
	/// <summary>Enumerations to identify image indexes in the TrialMax media bar commands</summary>
	public enum TmaxMediaBarCommands
	{
		Invalid = 0,
		RotateCW,
		RotateCCW,
		Normal,
		Zoom,
		ZoomWidth,
		Callout,
		Highlight,
		Redact,
		Draw,
		Erase,
		EraseLast,
		Arrow,
		Ellipse,
		Freehand,
		Line,
		Polygon,
		Rectangle,
		Text,
		FilledEllipse,
		FilledPolygon,
		ShadedCallouts,
		Select,
		SaveZap,
		UpdateZap,
		Red,
		Green,
		Blue,
		DarkRed,
		DarkGreen,
		DarkBlue,
		LightRed,
		LightGreen,
		LightBlue,
		Yellow,
		PureBlack,
		PureWhite,
		Play,
		Pause,
		PlayStart,
		PlayEnd,
		PlayBack,
		PlayFwd,
		Pan,
        BlankPresentation,
		Next,
		Previous,
		GoTo,
        NudgeLeft,
        NudgeRight,
        SaveNudge,
	}
	
	/// <summary>Enumerations to identify groups for highlighter assignments</summary>
	public enum TmaxHighlighterGroups
	{
		Plaintiff = 0,
		Defendant,
		Other,
	}
		
	/// <summary>Enumerations to identify event item states</summary>
	public enum TmaxItemStates
	{
		Pending = 0,
		Processed,
		Deleted,
	}
	
	/// <summary>Enumerations to identify property categories</summary>
	public enum TmaxPropertyCategories
	{
		Media = 0,
		Database,
	}
	
	/// <summary>Enumerations to identify property grid control editors</summary>
	public enum TmaxPropGridEditors
	{
		None = 0,
		Text,
		Integer,
		Double,
		Date,
		Boolean,
		Memo,
		Custom,
		DropList,
		Combobox,
		MultiLevel,
	}
	
	/// <summary>Enumerations to identify predefined reports</summary>
	public enum TmaxReports
	{
		Unknown = 0,
		Scripts,
		Transcript,
		Exhibits,
		Objections,
	}
	
	/// <summary>Enumerations to define export formats</summary>
	public enum TmaxExportFormats
	{
		Unknown = 0,
		BarcodeMap,
		AsciiMedia,
		Video,
		Codes,
		CodesDatabase,
		LoadFile,
		Transcript,
		XmlScript,
		AsciiPickList,
		XmlCaseCodes,
		XmlBinder,
	}
		
	/// <summary>Enumerations to define export delimiters</summary>
	public enum TmaxExportDelimiters
	{
		Tab = 0,
		Comma,
		Pipe,
	}
		
	/// <summary>Enumerations to define export concatenation characters</summary>
	public enum TmaxExportConcatenators
	{
		Comma = 0,
		Semicolon,
		Pipe,
		Tilde,
		HardReturn,
		User,
	}
		
	/// <summary>Enumerations to define export CRLF Replacements</summary>
	public enum TmaxExportCRLF
	{
		None = 0,
		Space,
		Pipe,
		HTML,
		Summation,
		User,
	}
		
	/// <summary>Enumerations to define export columns</summary>
	public enum TmaxExportColumns
	{
		Invalid = 0,
		Barcode,
		Name,
	}
		
	/// <summary>Enumerations to define import formats</summary>
	public enum TmaxImportFormats
	{
		Unknown = 0,
		AsciiMedia,
		BarcodeMap,
		Codes,
		CodesDatabase,
		XmlScript,
		AsciiPickList,
		XmlCaseCodes,
		//Add enum value for .txt format
        TextCaseCodes,
		AsciiBinder,
		XmlBinder,
	}
		
	/// <summary>Enumerations to define properties that can be imported from a codes file</summary>
	/// <remarks>NOTE: Coded properties are not included in this enumeration</remarks>
	public enum TmaxImportProperties
	{
		Invalid = 0,
		Barcode,
		Name,
	}
		
	/// <summary>Enumerations to define import delimiters</summary>
	public enum TmaxImportDelimiters
	{
		Tab = 0,
		Pipe,
		Comma,
		Expression,
	}
		
	/// <summary>Enumerations to define import concatenation characters</summary>
	public enum TmaxImportConcatenators
	{
		Comma = 0,
		Semicolon,
		Pipe,
		Tilde,
		User,
	}
		
	/// <summary>Enumerations to define import CRLF Substitions</summary>
	public enum TmaxImportCRLF
	{
		None = 0,
		Space,
		Pipe,
		HTML,
		Summation,
		User,
	}

	/// <summary>Enumerations to define methods for importing objections</summary>
	public enum TmaxImportObjectionMethods
	{
		IgnoreAll = 0,
		UpdateExisting,
		IgnoreExisting,
		AddAll,
	}

	/// <summary>Enumerations to define possible results of an import attempt</summary>
	public enum TmaxImportResults
	{
		Invalid = 0,
		Added,
		Updated,
		Ignored,
		Conflict,
		AddFailed,
		UpdateFailed,
	}

	/// <summary>Enumerations to define display modes for import messages</summary>
	public enum TmaxImportMessageModes
	{
		AsciiMedia = 0,
		XmlScripts,
		AsciiObjections,
	}

	/// <summary>Enumerations to define XML script formats</summary>
	public enum TmaxXmlScriptFormats
	{
		Unknown = 0,
		Manager,
		VideoViewer,
	}
		
	/// <summary>Enumerations to define methods for merging designations</summary>
	public enum TmaxDesignationMergeMethods
	{
		None = 0,
		AdjacentLines,	//	Adjacent lines on same page
		AdjacentPages,	//	Adjacent pages
	}
		
	/// <summary>Enumerations to define export formats</summary>
	public enum TmaxDesignationEditMethods
	{
		Unknown = 0,
		Extents,
		SplitBefore,
		SplitAfter,
		Exclude,
	}

	/// <summary>Enumerations to define Objection report boolean options</summary>
	public enum TmaxOREFlags
	{
		ShowObjection,
		ShowRuling,
		SplitResponse,
	}

	/// <summary>Enumerations to define requests that can be issued to a navigator</summary>
	public enum TmaxNavigatorRequests
	{
		Invalid,
		First,
		Last,
		Next,
		Previous,
		Absolute,
		QueryPosition,
		SetMode,
	}

	/// <summary>Enumerations to identify attributes common to all media records</summary>
	///
	///	<remarks>The common flags start at the top and work down</remarks>
	[System.Flags] public enum TmaxCommonAttributes
	{
		Mapped = 0x8000,
		Admitted = 0x4000,
	}
	
	/// <summary>Enumerations to identify primary attributes</summary>
	[System.Flags] public enum TmaxPrimaryAttributes
	{
		Merged = 0x0001,
		Unused2 = 0x0002,	//	Used to be Split attribute
		Unused4 = 0x0004,
		Playlist = 0x0008,
	}
	
	/// <summary>Enumerations to identify secondary attributes</summary>
	public enum TmaxSecondaryAttributes
	{
		HighResPage = 0x0001,
		Hidden = 0x0002,
		AutoTransition = 0x0004,
	}
	
	/// <summary>Enumerations to identify tertiary attributes</summary>
	[System.Flags] public enum TmaxTertiaryAttributes
	{
		HasShortcuts = 0x0001,		// Media is source for multiple records
		ScrollText = 0x0002,		// Scroll text on video designations
		SplitScreen = 0x0004,		// Split-screen treatment
		SplitHorizontal = 0x0008,	// Horizontal split screen
		SplitRight = 0x0010,		// Vertical right or Horizontal bottom for split screen
	}
	
	/// <summary>Enumerations to identify quaternary attributes</summary>
	[System.Flags] public enum TmaxQuaternaryAttributes
	{
		SplitLink = 0x0001,	// Split screen link
		HideLink = 0x0002,	// Hide link
		HideVideo = 0x0004,	// Hide video playback
		HideText = 0x0008,	// Hide scrolling text
	}
	
	/// <summary>Enumerations to identify binder attributes</summary>
	///
	///	<remarks>Before adding to this enumeration check out the effect on CDxBinderEntries::GetSQLSelect()</remarks>
	public enum TmaxBinderAttributes
	{
		IsMedia = 0x0001,
	}
	
	/// <summary>Enumerations to identify pick item types</summary>
	public enum TmaxPickItemTypes
	{
		Unknown = 0,
		MultiLevel,		//	A collection of pick lists
		StringList,		//	A pick list of strings
		Value,			//	An assignable value
	}
	
	/// <summary>Enumerations to identify attributes assigned to pick list items</summary>
	[System.Flags] public enum TmaxPickItemAttributes
	{
		CaseSensitive = 0x0001,
		UserAdditions = 0x0002,
	}
	
	/// <summary>Case options enumerations to identify various flags</summary>
	public enum TmaxCaseFlags
	{
		AllowEditAliases,
	}
	
	/// <summary>Case options enumerations to identify the media path overrides</summary>
	public enum TmaxCaseFolders
	{
		Documents,
		Treatments,
		PowerPoints,
		Recordings,
		Transcripts,
		Videos,
		Clips,
		Slides,
		Objections,
	}
	
	/// <summary>Enumerations to control text displayed for records in Physical and Virtual trees</summary>
	[System.Flags] public enum TmaxTextModes
	{
		MediaId = 1,
		Barcode = 2,
		Name = 4,
		DisplayOrder = 8,
		Exhibit = 16,
		Filename = 32,
	}

	/// <summary>Enumerations to control database validations</summary>
	[System.Flags] public enum TmaxDatabaseValidations
	{
		Document = 0x0001,
		PowerPoint = 0x0002,
		Recording = 0x0004,
		Transcripts = 0x0008,
		Video = 0x0010,
		Scripts = 0x0020,
		Binders = 0x0040,
		BarcodeMap = 0x0080,
		TransferCodes = 0x0100,
		CreateDesignations = 0x0200,
	}

	/// <summary>Enumerations to control database validations</summary>
	public enum TmaxHolePunchLocations
	{
		None = 0x0000,
		Left = 0x0001,
		Right = 0x0002,
		Top = 0x0004,
		Bottom = 0x0008,
	}

	/// <summary>Enumerations to control page rotations</summary>
	public enum TmaxRotations
	{
		None,
		Clockwise,
		CounterClockwise,
	}

	/// <summary>Enumerations for error levels associated with CTmaxMessage objects</summary>
	public enum TmaxMessageLevels
	{
		Text,
		Information,
		Warning,
		CriticalError,
		FatalError,
	}
	
	/// <summary>Enumerations for form-specific status message types</summary>
	public enum TmaxStatusMessageTypes
	{
		Export,
		Import,
	}

    /// <summary>Enumerations for form-specific status message types</summary>
    public enum TmaxPDFOutputType
    {
        Autodetect,
        ForceColor,
        ForceBW,
    }
	
}// namespace FTI.Shared.Trialmax