namespace FTI.Shared.Trialmax
{
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
	
