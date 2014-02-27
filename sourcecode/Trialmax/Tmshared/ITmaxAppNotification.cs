namespace FTI.Shared.Trialmax
{
	/// <summary>Objects that support this interface can request application notification for specific actions</summary>
	public interface ITmaxAppNotification
	{
		/// <summary>This method is called by the application when something is added to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren);
	
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems);
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems);
	
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem);

		/// <summary>This method is called by the application when Objections are added to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren);

		/// <summary>This method is called by the application to when Objections get deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems);

		/// <summary>This method is called by the application when multiple Objections have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems);

	}// public interface ITmaxPropGridCtrl

}// namespace FTI.Shared.Trialmax
	
