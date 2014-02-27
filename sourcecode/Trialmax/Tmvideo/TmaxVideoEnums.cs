using System;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>These enumerators identify the application views</summary>
	public enum TmaxVideoViews
	{
		Transcript = 0,
		Tree,
		Tuner,
		Results,
		MaxViews,
	};
		
	/// <summary>Enumerations to identify commands fired by the child views</summary>
	public enum TmaxVideoCommands
	{
		Activate,			//	Activate the requested object
		Add,				//	Add the specified object
		Delete,				//	Delete the specified object
		Reorder,			//	Reorder the specified objects
		Update,				//	One or more properties were updated
		EditDesignation,	//	Edit extents of a specified designation
		SetPreferences,		//	Set the application preferences
		LoadResult,			//	Load the views using the specified search result
		Find,				//	Search for text
		Import,				//	Import designations from file
		Export,				//	Export designations to file
	}
		
	/// <summary>Enumerations to identify pages in the SetPreferences form</summary>
	public enum TmaxVideoPreferencesPages
	{
		General,		//	General application setup
		Highlighters,	//	Application highlighters
	}
		
	
}// namespace FTI.Trialmax.TMVV.Tmvideo