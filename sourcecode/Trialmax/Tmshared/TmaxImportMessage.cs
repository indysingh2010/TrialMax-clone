using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with a search result</summary>
	public class CTmaxImportMessage : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to LineNumber property</summary>
		private int m_iLineNumber = 0;
		
		/// <summary>Local member bound to Message property</summary>
		private string m_strMessage = "";

		/// <summary>Local member bound to Result property</summary>
		private TmaxImportResults m_eResult = TmaxImportResults.Invalid;

		/// <summary>Local member bound to Level property</summary>
		private TmaxMessageLevels m_eLevel = TmaxMessageLevels.Text;

		/// <summary>Local member bound to MediaType property</summary>
		private TmaxMediaTypes m_eMediaType = TmaxMediaTypes.Unknown;

		/// <summary>Local member bound to DataType property</summary>
		private TmaxDataTypes m_eDataType = TmaxDataTypes.Unknown;

		/// <summary>Local member bound to TmaxRecord property</summary>
		private ITmaxBaseRecord m_tmaxRecord = null;

		/// <summary>Local member bound to ImageIndex property</summary>
		private int m_iImageIndex = -1;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxImportMessage()
		{
		}
	
		/// <summary>Called to get the text representation of the specified result</summary>
		/// <param name="eResult">The enumerated result identifier</param>
		/// <returns>The text representation of the enumerated value</returns>
		static public string AsString(TmaxImportResults eResult)
		{
			switch(eResult)
			{
				case TmaxImportResults.Added:		
					return "Added";
				case TmaxImportResults.Updated:
					return "Updated";
				case TmaxImportResults.AddFailed:
					return "Add Failed";
				case TmaxImportResults.UpdateFailed:
					return "Update Failed";
				case TmaxImportResults.Conflict:
					return "Conflict";
				case TmaxImportResults.Ignored:
					return "Ignored";
				case TmaxImportResults.Invalid:
					return "Invalid";
				default:
					return eResult.ToString();
			}
			
		}// static public string AsString(TmaxImportResults eResult)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxMessage">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxImportMessage tmaxMessage, long lMode)
		{
			//	Compare the line numbers
			if(m_iLineNumber == tmaxMessage.LineNumber)
			{	
				return 0;
			}
			else
			{
				return (m_iLineNumber < tmaxMessage.LineNumber) ? -1 : 1;
			}
					
		}// public int Compare(CTmaxImportMessage tmaxMessage, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxImportMessage)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)

		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			TmaxImportMessageModes eMode = TmaxImportMessageModes.AsciiMedia;

			try { eMode = (TmaxImportMessageModes)iDisplayMode; }
			catch { eMode = TmaxImportMessageModes.AsciiMedia; }

			switch(eMode)
			{
				case TmaxImportMessageModes.XmlScripts:

					return new string[] { "Filename", "Result", "Type", "Summary" };

				case TmaxImportMessageModes.AsciiObjections:

					return new string[] { "Filename", "Line", "Result", "Summary" };

				case TmaxImportMessageModes.AsciiMedia:
				default:

					return new string[] { "Filename", "Line", "Result", "Type", "Summary" };

			}// switch(eMode)			

		}// string[] ITmaxListViewCtrl.GetColumnNames()

		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			TmaxImportMessageModes eMode = TmaxImportMessageModes.AsciiMedia;
			
			try { eMode = (TmaxImportMessageModes)iDisplayMode; }
			catch { eMode = TmaxImportMessageModes.AsciiMedia; }

			switch(eMode)
			{
				case TmaxImportMessageModes.XmlScripts:
				
					aValues = new string[4];
					aValues[0] = this.Filename;
					aValues[1] = AsString(this.Result);
					if(this.MediaType != TmaxMediaTypes.Unknown)
						aValues[2] = this.MediaType.ToString();
					else if(this.DataType != TmaxDataTypes.Unknown)
						aValues[2] = this.DataType.ToString();
					else
						aValues[2] = "";
					aValues[3] = this.Message.Replace("\n", " ");
					break;
					
				case TmaxImportMessageModes.AsciiObjections:
				
					aValues = new string[4];
					aValues[0] = this.Filename;
					if(this.LineNumber > 0)
						aValues[1] = this.LineNumber.ToString();
					else
						aValues[1] = "";
					aValues[2] = AsString(this.Result);
					aValues[3] = this.Message.Replace("\n", " ");
					break;

				case TmaxImportMessageModes.AsciiMedia:
				default:

					aValues = new string[5];
					aValues[0] = this.Filename;
					
					if(this.LineNumber > 0)
						aValues[1] = this.LineNumber.ToString();
					else
						aValues[1] = "";

					aValues[2] = AsString(this.Result);

					if(this.MediaType != TmaxMediaTypes.Unknown)
						aValues[3] = this.MediaType.ToString();
					else if(this.DataType != TmaxDataTypes.Unknown)
						aValues[3] = this.DataType.ToString();
					else
						aValues[3] = "";
						
					aValues[4] = this.Message.Replace("\n", " ");
					break;

			}// switch(eMode)			

			return aValues;

		}// string[] ITmaxListViewCtrl.GetValues()

		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			return this.ImageIndex;
		}

		/// <summary>This function is called to set the database record associated with the message</summary>
		/// <param name="tmaxRecord">The interface to the database record</param>
		public void SetTmaxRecord(ITmaxBaseRecord tmaxRecord)
		{
			if((m_tmaxRecord = tmaxRecord) != null)
			{
			
			}

		}// void SetTmaxRecord(ITmaxBaseRecord tmaxRecord)

		/// <summary>This function is called to get the media type associated with this message</summary>
		/// <returns>The enumerated media type identifier</returns>
		public TmaxMediaTypes GetMediaType()
		{
			if(m_eMediaType == TmaxMediaTypes.Unknown)
			{
				//	Do we have a media record?
				if((TmaxRecord != null) && (TmaxRecord.GetDataType() == TmaxDataTypes.Media))
				{
					try { m_eMediaType = ((ITmaxMediaRecord)(TmaxRecord)).GetMediaType(); }
					catch {};
					
				}

			}// if(m_eMediaType == TmaxMediaTypes.Unknown)
			
			return m_eMediaType;

		}// public TmaxMediaTypes GetMediaType()

		/// <summary>This function is called to get the data type associated with this message</summary>
		/// <returns>The enumerated data type identifier</returns>
		public TmaxDataTypes GetDataType()
		{
			if(m_eDataType == TmaxDataTypes.Unknown)
			{
				//	Do we have a database record?
				if(TmaxRecord != null)
					m_eDataType = TmaxRecord.GetDataType();

			}// if(m_eDataType == TmaxDataTypes.Unknown)

			return m_eDataType;

		}// public TmaxDataTypes GetDataType()
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The filename associated with this message</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}
		
		/// <summary>The line number associated with this message</summary>
		public int LineNumber
		{
			get { return m_iLineNumber; }
			set { m_iLineNumber = value; }
		}
		
		/// <summary>The text associated with this message</summary>
		public string Message
		{
			get { return m_strMessage; }
			set { m_strMessage = value; }
		}

		/// <summary>The result associated with this message</summary>
		public TmaxImportResults Result
		{
			get { return m_eResult; }
			set { m_eResult = value; }
		}

		/// <summary>The error level associated with the message</summary>
		public TmaxMessageLevels Level
		{
			get { return m_eLevel; }
			set { m_eLevel = value; }
		}

		/// <summary>The database record associated with the message</summary>
		public ITmaxBaseRecord TmaxRecord
		{
			get { return m_tmaxRecord; }
			set { SetTmaxRecord(value); }
		}

		/// <summary>The media type with the message</summary>
		public TmaxMediaTypes MediaType
		{
			get { return GetMediaType(); }
			set { m_eMediaType = value; }
		}

		/// <summary>The data type with the message</summary>
		public TmaxDataTypes DataType
		{
			get { return GetDataType(); }
			set { m_eDataType = value; }
		}

		/// <summary>The image index associated with this message</summary>
		public int ImageIndex
		{
			get { return m_iImageIndex; }
			set { m_iImageIndex = value; }
		}

		#endregion Properties
		
	}// public class CTmaxImportMessage

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxImportMessages : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxImportMessages() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxMessage">CTmaxImportMessage object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxImportMessage Add(CTmaxImportMessage tmaxMessage)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxMessage as object);

				return tmaxMessage;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxImportMessage Add(CTmaxImportMessage tmaxMessage)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxMessage">The filter object to be removed</param>
		public void Remove(CTmaxImportMessage tmaxMessage)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxMessage as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxImportMessage tmaxMessage)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxMessage">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxImportMessage tmaxMessage)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxMessage as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxImportMessage this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxImportMessage);
			}
		}

		#endregion Public Methods
		
	}//	public class CTmaxImportMessages
		
}// namespace FTI.Shared.Trialmax
