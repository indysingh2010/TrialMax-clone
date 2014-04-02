namespace FTI.Shared.Trialmax
{
	/// <summary>Objects that can be displayed in a TrialMax list view control</summary>
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

}// namespace FTI.Shared.Trialmax
	
