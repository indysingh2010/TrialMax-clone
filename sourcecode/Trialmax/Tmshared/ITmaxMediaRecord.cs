using System;
using System.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>All TrialMax record classes support this interface</summary>
	public interface ITmaxBaseRecord
	{
		/// <summary>This method is called to get the data type associated with the record</summary>
		/// <returns>The enumerated data type</returns>
		FTI.Shared.Trialmax.TmaxDataTypes GetDataType();

	}

	/// <summary>Objects that support this interface expose properties of a TrialMax media record</summary>
	public interface ITmaxMediaRecord : ITmaxBaseRecord
	{
		/// <summary>This method is called to get the interface to the parent record</summary>
		/// <returns>The interface to the parent record exchange object</returns>
		ITmaxMediaRecord GetParent();

		/// <summary>This method is called to get the media type of the parent record</summary>
		/// <returns>The parent record's media type enumerator</returns>
		TmaxMediaTypes GetParentMediaType();

		/// <summary>This method is called to get the id assigned to the parent record</summary>
		/// <returns>The id assigned to the parent record</returns>
		long GetParentId();

		/// <summary>This method is called to get the id automatically assigned by the database</summary>
		/// <returns>The id assigned by the database</returns>
		long GetAutoId();

	    DateTime? GetCodeDateValue(CTmaxCaseCode caseCode);

	    string GetCodeText(CTmaxCaseCode caseCode);

	    bool? GetCodeBoolValue(CTmaxCaseCode caseCode);

        int? GetCodeIntValue(CTmaxCaseCode caseCode);

	    double? GetCodeDecimalValue(CTmaxCaseCode caseCode);

        string GetCodePickItemValue(CTmaxCaseCode caseCode);

		/// <summary>This method is called to get the unique identifier assigned by the database</summary>
		/// <returns>The unique id assigned by the database</returns>
		string GetUniqueId();

		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		string GetMediaId();

		/// <summary>This method is called to get the attributes assigned to the record</summary>
		/// <returns>The attributes associated with the record</returns>
		long GetAttributes();

		/// <summary>This method is called to get the record's Admitted state</summary>
		/// <returns>The record's Admitted state</returns>
		bool GetAdmitted();

		/// <summary>This method is called to get the record's Mapped state</summary>
		/// <returns>The record's Mapped state</returns>
		bool GetMapped();

		/// <summary>This method is called to get number of children associated with the record</summary>
		/// <returns>The number of children</returns>
		long GetChildCount();

		/// <summary>This method is called to get display order index for the record</summary>
		/// <returns>The order in which the object gets displayed</returns>
		long GetDisplayOrder();

		/// <summary>This method is called to get barcode id for the record</summary>
		/// <returns>The record's barcode identifier</returns>
		long GetBarcodeId();

		/// <summary>This method is called to get the text used to display the record in a TrialMax tree</summary>
		/// <param name="eTextMode">The enumerated text mode identifier</param>
		/// <returns>The appropriate text string</returns>
		string GetText(TmaxTextModes eTextMode);

		/// <summary>This method is called to get the record's barcode</summary>
		/// <param name="bIgnoreMapped">true to ignore any entry in the barcode map</param>
		/// <returns>The barcode that identifies the record</returns>
		string GetBarcode(bool bIgnoreMapped);

		/// <summary>This method is called to get the record's name</summary>
		/// <returns>The name associated with the record</returns>
		string GetName();

		/// <summary>This method is called to get the record's description</summary>
		/// <returns>The description associated with the record</returns>
		string GetDescription();

		/// <summary>This method is called to get the name of the file associated with the record</summary>
		/// <returns>The name of the associated file</returns>
		string GetFileName();

		/// <summary>This method is called to get the path to the file associated with the record</summary>
		/// <returns>The fully qualified path of the associated file</returns>
		string GetFileSpec();

		/// <summary>This method is called to get the media type associated with the record</summary>
		/// <returns>The media type stored in the database</returns>
		FTI.Shared.Trialmax.TmaxMediaTypes GetMediaType();

		/// <summary>This method is called to get the media level associated with the record</summary>
		/// <returns>The media level identified by the record type</returns>
		FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel();
        
		/// <summary>This method is called to get the date the media was added to the database</summary>
		/// <returns>The date the media was created</returns>
		System.DateTime GetCreatedOn();
		
		/// <summary>This method is called to get the id of the user that created the record</summary>
		/// <returns>The user name</returns>
		long GetCreatedBy();
		
		/// <summary>This method is called to get the date the media was last modified</summary>
		/// <returns>The date the media was last updated</returns>
		System.DateTime GetModifiedOn();
		
		/// <summary>This method is called to get the id of the user that last modified the record</summary>
		/// <returns>The user name</returns>
		long GetModifiedBy();
		
		/// <summary>This method is called to get the media relationship that exists between the two records</summary>
		/// <param name="IRecord">The record to be evaluated</param>
		/// <returns>The enumerated media relationship</returns>
		TmaxMediaRelationships GetRelationship(ITmaxMediaRecord IRecord);
		
		/// <summary>This method is called to get the collection of properties associated with the record</summary>
		/// <param name="aProperties">The collection in which to store the properties</param>
		void GetProperties(CTmaxProperties aProperties);
	
		/// <summary>This method is called to set a property value</summary>
		/// <param name="Args">The arguments used to set the property value</param>
		/// <returns>True if successful</returns>
		bool SetProperty(CTmaxSetPropertyArgs Args);
		
		///	<summary>This method retrieves an XML node that represents the record</summary>
		/// <param name="xmlDocument">The XML document that will own the node</param>
		/// <param name="strName">The name to be assigned to the node</param>
		/// <returns>The XML node representation of the record</returns>
		XmlNode GetXmlNode(XmlDocument xmlDocument, string strName);
		
		///	<summary>This method is called to determine if the record media is linked</summary>
		/// <returns>True if linked</returns>
		bool GetLinked();

		/// <summary>This method is called to get the record's SplitScreen attribute</summary>
		/// <returns>The record's SplitScreen attribute</returns>
		bool GetSplitScreen();

		///	<summary>This method is called to retrieve the id of the server/drive alias assigned to the record</summary>
		/// <returns>The record's alias identifier</returns>
		long GetAliasId();
		
		///	<summary>This method retrieves the aliased path for a linked media record</summary>
		/// <returns>The fully qualified aliased path if linked</returns>
		string GetAliasedPath();
	
		///	<summary>This method is called to determine if the media associated with this record can be cleaned</summary>
		///	<param name="bFill">true if it is OK to fill the child collection to make the determination</param>
		/// <returns>True if the media can be cleaned</returns>
		bool GetCanClean(bool bFill);
	
		///	<summary>This method is called to get the values to be exported in the specified column</summary>
		///	<param name="tmaxColumn">The column that identifies the data to be exported</param>
		///	<param name="bRefresh">true to refresh the data when the column is bound to coded data</param>
		/// <param name="tmaxOptions">The user defined options for the export operation</param>
		/// <returns>True if successful</returns>
		bool GetExportValues(CTmaxExportColumn tmaxColumn, bool bRefresh, CTmaxExportOptions tmaxOptions);
	}

	/// <summary>Objects that support this interface expose properties of records stored in the objections database</summary>
	public interface ITmaxBaseObjectionRecord : ITmaxBaseRecord
	{
		/// <summary>This method is called to get the unique identifier assigned by the database</summary>
		/// <returns>The unique id assigned by the database</returns>
		string GetUniqueId();

		/// <summary>This method is called to text used to display the record</summary>
		/// <returns>The text descriptor</returns>
		string GetText();

	}

	/// <summary>Objects that support this interface expose properties of a TrialMax objection record</summary>
	public interface ITmaxObjectionRecord : ITmaxBaseObjectionRecord
	{
		/// <summary>This method is called to get the interface to the associated application object</summary>
		/// <returns>The application object bound to the record</returns>
		CTmaxObjection GetTmaxObjection();

		/// <summary>This method is called to get the interface to the associated deposition record</summary>
		/// <returns>The exchange interface for the associated deposition record</returns>
		ITmaxBaseObjectionRecord GetIOxDeposition();

		/// <summary>This method is called to get the interface to the associated state record</summary>
		/// <returns>The exchange interface for the associated state record</returns>
		ITmaxBaseObjectionRecord GetIOxState();

		/// <summary>This method is called to get the interface to the associated ruling record</summary>
		/// <returns>The exchange interface for the associated ruling record</returns>
		ITmaxBaseObjectionRecord GetIOxRuling();

		/// <summary>This method is called to get the interface to the associated ModifiedBy user record</summary>
		/// <returns>The exchange interface for the associated user record</returns>
		ITmaxBaseObjectionRecord GetIOxModifiedBy();

		/// <summary>This method is called to get the TrialMax Manager MediaId of the related deposition</summary>
		/// <returns>The MediaId of the associated deposition</returns>
		string GetDeposition();

		/// <summary>This method is called to get the descriptor for the owner case</summary>
		/// <returns>The application case descriptor for the objection</returns>
		CTmaxCase GetCase();

	}// public interface ITmaxObjectionRecord : ITmaxBaseObjectionRecord

	/// <summary>Objects that support this interface expose properties of a TrialMax deposition record</summary>
	public interface ITmaxDeposition
	{
		/// <summary>This method is called to get the MediaId of the deposition</summary>
		/// <returns>The deposition's Media Id</returns>
		string GetMediaId();

		/// <summary>This method is called to get the FirstPL value</summary>
		/// <returns>The composition page/line for the first line in the transcript</returns>
		long GetFirstPL();

		/// <summary>This method is called to get the LastPL value</summary>
		/// <returns>The composition page/line for the last line in the transcript</returns>
		long GetLastPL();

		/// <summary>This method is called to get the test used to present the deposition to the user</summary>
		/// <returns>The text used to represent the deposition</returns>
		string ShowAs();
	}

}// namespace FTI.Shared.Trialmax
	
