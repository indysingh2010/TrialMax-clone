using System.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>Objects that represent a record in the database should support this interface</summary>
	/// <remarks>
	/// This interface allows for inclusion of references to database record classes
	/// without having to reference the tmdata assembly
	/// </remarks>
	public interface ITmaxRecord
	{
		/// <summary>This method is called to get the interface to the parent record</summary>
		/// <returns>The interface to the parent record exchange object</returns>
		ITmaxRecord GetParent();

		/// <summary>This method is called to get the media type of the parent record</summary>
		/// <returns>The parent record's media type enumerator</returns>
		TmaxMediaTypes GetParentMediaType();

		/// <summary>This method is called to get the id automatically assigned by the database</summary>
		/// <returns>The id assigned by the database</returns>
		long GetAutoId();

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

		/// <summary>This method is called to get the data type associated with the record</summary>
		/// <returns>The enumerated data type</returns>
		FTI.Shared.Trialmax.TmaxDataTypes GetDataType();

		/// <summary>This method is called to get the media type associated with the record</summary>
		/// <returns>The media type stored in the database</returns>
		FTI.Shared.Trialmax.TmaxMediaTypes GetMediaType();

		/// <summary>This method is called to get the media level associated with the record</summary>
		/// <returns>The media level identified by the record type</returns>
		FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel();

		/// <summary>This method is called to get the date the media was added to the database</summary>
		/// <returns>The date the media was created</returns>
		System.DateTime GetCreatedOn();
		
		/// <summary>This method is called to get the date the media was last modified</summary>
		/// <returns>The date the media was last updated</returns>
		System.DateTime GetModifiedOn();
		
		/// <summary>This method is called to get the media relationship that exists between the two records</summary>
		/// <param name="IRecord">The record to be evaluated</param>
		/// <returns>The enumerated media relationship</returns>
		TmaxMediaRelationships GetRelationship(ITmaxRecord IRecord);
		
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
		/// <returns>True if successful</returns>
		bool GetExportValues(CTmaxExportColumn tmaxColumn, bool bRefresh);
	}

	/// <summary>Objects that represent a designation</summary>
	/// <remarks>
	/// This interface allows controls to query CXmlDesignation and CDxDesignation
	/// objects for the designation extents without knowing the object type
	/// </remarks>
	/// <summary>Objects displayed in a TrialMax list view control must support this interface</summary>
	public interface ITmaxListViewCtrl
	{
		/// <summary>This method is called to fill the array list with the names of all the column headers</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>An array of names used to create the columns</returns>
		string[] GetColumnNames(int iDisplayMode);
	
		/// <summary>This method is called to fill the array list with the values to be displayed in the columns</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>An array of values used to populate the columns</returns>
		string[] GetValues(int iDisplayMode);
	
		/// <summary>This method is called to get the index of the image to display in the row</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>An image to be displayed in the row for the object</returns>
		int GetImageIndex(int iDisplayMode);
	
	}// public interface ITmaxListViewCtrl

	/// <summary>Objects displayed in a TrialMax property grid control must support this interface</summary>
	public interface ITmaxPropGridCtrl
	{
		/// <summary>This method is called to get the id of the property</summary>
		/// <returns>The id to be assigned to the property</returns>
		long GetId();
	
		/// <summary>This method is called to compare two objects for sorting</summary>
		/// <returns>0 if equal, -1 if less than, 1 if greater than</returns>
		int Compare(ITmaxPropGridCtrl ICompare);
	
		/// <summary>This method is called to get the name used to identify the property in the grid</summary>
		/// <returns>An property name</returns>
		string GetName();
	
		/// <summary>This method is called to display the value of the property in the grid</summary>
		/// <returns>An value of the property as a text string</returns>
		string GetValue();
	
		/// <summary>This method is called to get the editor to use for this property</summary>
		/// <returns>The enumerated editor identifier</returns>
		TmaxPropGridEditors GetEditor();
	
		/// <summary>This method is called to attach a user defined tag to be attached to the property</summary>
		/// <returns>An optional tag to be attached and supplied with events</returns>
		object GetTag();
	
		/// <summary>This method is called to get the category to which the property belongs</summary>
		/// <returns>An optional category that owns the property</returns>
		object GetCategory();
		
		/// <summary>This method is called to set the value of the property</summary>
		/// <returns>True if successful</returns>
		bool SetValue(string strValue);
	
	}// public interface ITmaxPropGridCtrl

	/// <summary>Objects displayed in a TrialMax script combobox control must support this interface</summary>
	public interface ITmaxScriptBoxCtrl
	{
		/// <summary>This method is called to get the name used to identify the object</summary>
		/// <returns>An object's name</returns>
		string GetName();
	
		/// <summary>This method is called to display the object's media id</summary>
		/// <returns>An value of the media id</returns>
		string GetMediaId();
	
	}// public interface ITmaxScriptBoxCtrl

}// namespace FTI.Shared.Trialmax
	
