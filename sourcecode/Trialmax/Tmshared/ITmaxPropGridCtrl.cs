namespace FTI.Shared.Trialmax
{
	/// <summary>Objects displayed in a TrialMax property grid control must support this interface</summary>
	public interface ITmaxPropGridCtrl
	{
		/// <summary>This method is called to get the id of the property</summary>
		/// <returns>The id to be assigned to the property</returns>
		long GetId();

		/// <summary>This method is called to compare two objects for sorting</summary>
		/// <param name="ICompare">The object to be compared</param>
		/// <param name="lSortOn">The sort mode identifier assigned to the property grid</param>
		/// <returns>0 if equal, -1 if less than, 1 if greater than</returns>
		int Compare(ITmaxPropGridCtrl ICompare, long lSortOn);

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
		/// <param name="strValue">The new value to be assigned to the property</param>
		/// <returns>True if successful</returns>
		bool SetValue(string strValue);

		/// <summary>This method is called to set the value of the multi-level property</summary>
		/// <param name="tmaxParent">The new parent pick list</param>
		/// <param name="strValue">The new value to be assigned to the property</param>
		/// <returns>True if successful</returns>
		bool SetValue(CTmaxPickItem tmaxParent, string strValue);

		/// <summary>This method is called to get the collection of drop list values for the property</summary>
		/// <returns>True if successful</returns>
		System.Collections.ICollection GetDropListValues();

		/// <summary>This method is called to get the case code bound to the property</summary>
		/// <returns>True if successful</returns>
		FTI.Shared.Trialmax.CTmaxCaseCode GetCaseCode();

		/// <summary>This method is called to get the pick list item bound to the property</summary>
		/// <returns>True if successful</returns>
		FTI.Shared.Trialmax.CTmaxPickItem GetPickItem();

		/// <summary>This method is called to determine if the property should be visible in the grid</summary>
		/// <returns>True if visible</returns>
		bool GetVisible();

		/// <summary>This method is called to set the visibility of the object in the grid</summary>
		/// <param name="bVisibile">True if visible</param>
		void SetVisible(bool bVisibile);

	}// public interface ITmaxPropGridCtrl

}// namespace FTI.Shared.Trialmax
	
