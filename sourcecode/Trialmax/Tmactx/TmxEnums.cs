using System;
using System.Windows.Forms;

namespace FTI.Trialmax.ActiveX
{
	public enum TmxEvents
	{
		Action = 1,
		StateChange,
		PositionChange,
		QueryContinue,
		CreatedCallout,
		SavedPage,
		StartEditAnnText,
		FinishEditAnnText,		
	}
	
	public enum TmxActions
	{
		None = 0,
		SaveZap,
		UpdateZap,
		Play,
		Stop,
		Pause,
		Resume,
		ShowCallouts,
		HideCallouts,
		RotateImage,
		First,
		Last,
		Previous,
		Next,
		GoTo,
	}
	
	/// <summary>Cue operations</summary>
	public enum TmxCueModes
	{
		First = 0,
		Last,
		Start,
		Stop,
		Absolute,
		Relative,
	}
	
	public enum TmxStates
	{
		Invalid = 0,
		Unitialized,
		Unloaded,
		Loaded,
		Playing,
		Paused,
		Stopped,
	}
	
	/// <summary>Tm_view annotation actions</summary>
	public enum TmxViewActions
	{
		None = 0,
		Zoom,
		Draw,
		Redact,
		Highlight,
		Select,
		Callout,
		Pan,
	}
	
	/// <summary>Tm_view annotation colors</summary>
	public enum TmxViewColors
	{
		Black = 0,
		Red,
		Green,
		Blue,
		Yellow,
		Magenta,
		Cyan,
		Grey,
		White,
		DarkRed,
		DarkGreen,
		DarkBlue,
		LightRed,
		LightGreen,
		LightBlue,
	}
	
	/// <summary>Tm_view annotation tools</summary>
	public enum TmxViewTools
	{
		Freehand = 0,
		Line,
		Arrow,
		Ellipse,
		Rectangle,
		FilledEllipse,
		FilledRectangle,
		Polyline,
		Polygon,
		Text,
	}
	
	/// <summary>Tm_movie cue operations</summary>
	public enum TmxMovieCueRequests
	{
		First = 0,
		Last,
		Start,
		Stop,
		Absolute,
		Relative,
	}
	
	/// <summary>Tm_movie playback states</summary>
	public enum TmxMovieStates
	{
		NotReady = 0,
		Ready,
		Playing,
		Paused,
		Stopped,
	}

	/// <summary>Tmshare command identifiers</summary>
	public enum TmxShareCommands
	{
		None = 0,
		Open,
		AddTreatment,
		AddToBinder,
		UpdateTreatment,
	}
	

}// namespace FTI.Trialmax.ActiveX
