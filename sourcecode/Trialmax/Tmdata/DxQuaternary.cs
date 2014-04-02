using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class encapsulates the information used to exchange a QuaternaryMedia record
	/// </summary>
	public class CDxQuaternary : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXQUATERNARY_PROP_FIRST_ID			= DX_MEDIA_RECORD_PROP_LAST + 4000;
		
		protected const int DXQUATERNARY_PROP_MEDIA_TYPE		= DXQUATERNARY_PROP_FIRST_ID + 1;
		protected const int DXQUATERNARY_PROP_MEDIA_PATH		= DXQUATERNARY_PROP_FIRST_ID + 2;
		protected const int DXQUATERNARY_PROP_FILENAME			= DXQUATERNARY_PROP_FIRST_ID + 3;
		protected const int DXQUATERNARY_PROP_DISPLAY_ORDER		= DXQUATERNARY_PROP_FIRST_ID + 4;
		protected const int DXQUATERNARY_PROP_SPLIT_LINK		= DXQUATERNARY_PROP_FIRST_ID + 5;
		protected const int DXQUATERNARY_PROP_HIDE_LINK			= DXQUATERNARY_PROP_FIRST_ID + 6;
		protected const int DXQUATERNARY_PROP_START				= DXQUATERNARY_PROP_FIRST_ID + 7;
		protected const int DXQUATERNARY_PROP_START_TUNED		= DXQUATERNARY_PROP_FIRST_ID + 8;
		protected const int DXQUATERNARY_PROP_PL				= DXQUATERNARY_PROP_FIRST_ID + 9;
		protected const int DXQUATERNARY_PROP_SOURCE_ID			= DXQUATERNARY_PROP_FIRST_ID + 10;
		protected const int DXQUATERNARY_PROP_SOURCE_TYPE		= DXQUATERNARY_PROP_FIRST_ID + 11;
		protected const int DXQUATERNARY_PROP_SOURCE_BARCODE	= DXQUATERNARY_PROP_FIRST_ID + 12;
		protected const int DXQUATERNARY_PROP_SOURCE_FILENAME	= DXQUATERNARY_PROP_FIRST_ID + 13;
		protected const int DXQUATERNARY_PROP_SOURCE_PATH		= DXQUATERNARY_PROP_FIRST_ID + 14;
		protected const int DXQUATERNARY_PROP_HIDE_TEXT			= DXQUATERNARY_PROP_FIRST_ID + 15;
		protected const int DXQUATERNARY_PROP_HIDE_VIDEO		= DXQUATERNARY_PROP_FIRST_ID + 16;

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Tertiary property</summary>
		protected CDxTertiary m_dxTertiary = null;
		
		/// <summary>Local member bound to TertiaryMediaId property</summary>
		protected long m_lTertiaryMediaId = 0;
		
		/// <summary>Local member bound to Start property</summary>
		private double m_dStart = 0;
		
		/// <summary>Local member bound to StartPL property</summary>
		private long m_lStartPL = 0;
		
		/// <summary>Local member bound to StartTuned property</summary>
		private bool m_bStartTuned = false;
		
		/// <summary>Local member bound to SourceId property</summary>
		private string m_strSourceId = "";
		
		/// <summary>Local member bound to SourceType property</summary>
		private TmaxMediaTypes m_eSourceType = TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to Source property</summary>
		private CDxMediaRecord m_dxSource = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxQuaternary() : base()
		{
		}
		
		/// <summary>Overloaded constructor</summary>
		///	<param name="dxTertiary">The tertiary exchange object that owns this quaternary object</param>
		public CDxQuaternary(CDxTertiary dxTertiary) : base()
		{
			m_dxTertiary = dxTertiary;
		}
		
		/// <summary>This method is called to get the record exchange object for the source media</summary>
		///	<param name="dxTertiary">The record exchange object that this quaternary object references</param>
		public CDxMediaRecord GetSource()
		{
			//	Have we already located the source record?
			if(m_dxSource != null) return m_dxSource;
			
			//	Do we have a valid id
			if((SourceId != null) && (SourceId.Length > 0))
			{
				//	Do we have access to the database?
				if(this.Database != null)
					m_dxSource = this.Database.GetRecordFromId(SourceId, true);
			}
			
			return m_dxSource;
			
		}// public CDxMediaRecord GetSource()
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.Quaternary;
		}
		
		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		public override string GetMediaId()
		{
			if((Tertiary != null) &&
			   (Tertiary.Secondary != null) && 
			   (Tertiary.Secondary.Primary != null))
				return Tertiary.Secondary.Primary.GetMediaId();
			else
				return "";
		}

		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public override CDxMediaRecord GetParent()
		{
			return m_dxTertiary;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	Add the base class properties first
			base.GetProperties(tmaxProperties);
			
			tmaxProperties.Add(DXQUATERNARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_MEDIA_PATH, "Media Path", GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_FILENAME, "Filename", GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_SPLIT_LINK, "Split Link", SplitLink, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_HIDE_LINK, "Hide Link", HideLink, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_HIDE_TEXT, "Hide Text", HideText, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_HIDE_VIDEO, "Hide Video", HideVideo, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_START, "Start", Start, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_START_TUNED, "Tuned", StartTuned, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			
			if((m_dxTertiary != null) && (m_dxTertiary.MediaType == TmaxMediaTypes.Designation))
				tmaxProperties.Add(DXQUATERNARY_PROP_PL, "PG:LN", CTmaxToolbox.PLToString(StartPL), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
			//	Add the source properties
			GetSourceProperties(tmaxProperties);
			
		}// public override void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This function is called to add properties for the source record to the specified collection</summary>
		/// <param name="tmaxProperties">The collection in which to place the property descriptors</param>
		public void GetSourceProperties(CTmaxProperties tmaxProperties)
		{
			CDxMediaRecord dxSource = null;
			
			tmaxProperties.Add(DXQUATERNARY_PROP_SOURCE_ID, "Source PSTQ", SourceId, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXQUATERNARY_PROP_SOURCE_TYPE, "Source Type", SourceType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

			//	Get the source record
			if((dxSource = GetSource()) == null) return;
			
			//	What type of media is the source?
			switch(dxSource.MediaType)
			{
				case TmaxMediaTypes.Page:
				case TmaxMediaTypes.Slide:
				case TmaxMediaTypes.Treatment:
							
					tmaxProperties.Add(DXQUATERNARY_PROP_SOURCE_BARCODE, "Source Barcode", dxSource.GetBarcode(false), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXQUATERNARY_PROP_SOURCE_FILENAME, "Source Filename", dxSource.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXQUATERNARY_PROP_SOURCE_PATH, "Source Path", dxSource.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					break;
					
				default:
							
					break;
								
			}// switch(dxSource.MediaType)
					
		}// public void GetSourceProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXQUATERNARY_PROP_MEDIA_PATH:
				
					tmaxProperty.Value = GetFileSpec();
					break;
					
				case DXQUATERNARY_PROP_FILENAME:
				
					tmaxProperty.Value = GetFileName();
					break;
					
				case DXQUATERNARY_PROP_DISPLAY_ORDER:
				
					tmaxProperty.Value = m_lDisplayOrder;
					break;
					
				case DXQUATERNARY_PROP_SPLIT_LINK:
				
					tmaxProperty.Value = SplitLink;
					break;
					
				case DXQUATERNARY_PROP_HIDE_LINK:
				
					tmaxProperty.Value = HideLink;
					break;
					
				case DXQUATERNARY_PROP_START:
				
					tmaxProperty.Value = Start;
					break;

				case DXQUATERNARY_PROP_HIDE_TEXT:

					tmaxProperty.Value = HideText;
					break;

				case DXQUATERNARY_PROP_HIDE_VIDEO:

					tmaxProperty.Value = HideVideo;
					break;

				case DXQUATERNARY_PROP_START_TUNED:
				
					tmaxProperty.Value = StartTuned;
					break;
					
				case DXQUATERNARY_PROP_PL:
				
					tmaxProperty.Value = CTmaxToolbox.PLToString(StartPL);
					break;
					
				case DXQUATERNARY_PROP_SOURCE_ID:
				
					tmaxProperty.Value = SourceId;
					break;
					
				case DXQUATERNARY_PROP_SOURCE_TYPE:
				
					tmaxProperty.Value = SourceType;
					break;
					
				case DXQUATERNARY_PROP_SOURCE_BARCODE:
				
					if(GetSource() != null)
						tmaxProperty.Value = this.Database.GetBarcode(GetSource(), false);
					break;
					
				case DXQUATERNARY_PROP_SOURCE_FILENAME:
				
					if(GetSource() != null)
						tmaxProperty.Value = GetSource().GetFileName();
					break;
					
				case DXQUATERNARY_PROP_SOURCE_PATH:
				
					if(GetSource() != null)
						tmaxProperty.Value = GetSource().GetFileSpec();
					break;
					
				//	These properties are read-only
				case DXQUATERNARY_PROP_MEDIA_TYPE:
				
					break;
					
				default:
				
					base.RefreshProperty(tmaxProperty);
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public override string GetText()
		{
			return GetDefaultName();
		}
		
		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public override bool GetCanClean(bool bFill)
		{
			//	Must be a ling
			switch(this.MediaType)
			{
				case TmaxMediaTypes.Link:
				
					//	Use the source record
					if((this.HideLink == false) && (this.GetSource() != null))
						return (this.GetSource().GetCanClean(bFill));
					break;
			}
			
			return false;
			
		}// public override bool GetCanClean(bool bFill)
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		/// <param name="eMode">The desired TrialMax text mode</param>
		/// <returns>The text that represents this record</returns>
		public override string GetText(TmaxTextModes eMode)
		{
			string strText = "";
			
			strText = GetDefaultName();

			//	Use the barcode for the source
			if(HideLink == true)
			{
				strText += " - Hide";
			}
			else
			{
				if(GetSource() != null)
				{
					if(strText.Length > 0)
						strText += " - ";
						
					strText += this.Database.GetBarcode(GetSource(), false);
				}
			}
				
			return strText;
		}
					
		/// <summary>This method sets the object properties using the specified XML link descriptor</summary>
		/// <param name="xmlLink">The source XML link</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(CXmlLink xmlLink)
		{
			CDxMediaRecord dxSource = null;
			
			Start      = xmlLink.Start;
			StartTuned = xmlLink.StartTuned;
			StartPL    = xmlLink.PL;
			HideLink   = xmlLink.Hide;
			HideText   = xmlLink.HideText;
			HideVideo  = xmlLink.HideVideo;
			SplitLink  = (HideLink == true) ? false : xmlLink.Split;
			SourceId   = xmlLink.SourceDbId;
			
			if(m_dxTertiary.MediaType == TmaxMediaTypes.Designation)
				Name = CTmaxToolbox.PLToString(StartPL);
			else
				Name = CTmaxToolbox.SecondsToString(Start);
			
			//	Has this record been added to the database yet?
			if(this.Database != null)
			{
				//	Get the source specified in the XML
				if((HideLink == false) && (SourceId.Length > 0))
				{
					if((dxSource = this.Database.GetRecordFromId(SourceId, false)) != null)
					{
						Source = dxSource;
						SourceType = dxSource.MediaType;
					}
					else
					{
						Source = null;
						SourceType = TmaxMediaTypes.Unknown;
						return false;
					}
					
				}
				else
				{
					Source = null;
					SourceId = "";
					SourceType = TmaxMediaTypes.Unknown;
				}
				
			}

			return true;
				
		}// public bool SetProperties(CXmlLink xmlLink)
		
		/// <summary>This method sets the XML attributes using the current property values</summary>
		/// <param name="xmlLink">The target XML link</param>
		/// <returns>true if successful</returns>
		public bool SetAttributes(CXmlLink xmlLink)
		{
			xmlLink.DatabaseId = this.Database.GetUniqueId(this);
			xmlLink.Start = Start;
			xmlLink.StartTuned = StartTuned;
			xmlLink.PL = StartPL;
			xmlLink.Hide = HideLink;
			xmlLink.HideText = HideText;
			xmlLink.HideVideo = HideVideo;
			xmlLink.Split = SplitLink;
			xmlLink.SourceDbId = SourceId;

			if(GetSource() != null)
				xmlLink.SourceMediaId = this.Database.GetBarcode(GetSource(), false);
			else
				xmlLink.SourceMediaId = xmlLink.SourceDbId;
				
			if(StartPL > 0)
			{
				xmlLink.Page = CTmaxToolbox.PLToPage(StartPL);
				xmlLink.Line = CTmaxToolbox.PLToLine(StartPL);
			}
			else
			{
				xmlLink.Page = 0;
				xmlLink.Line = 0;
			}
			
			return true;
				
		}// public bool SetAttributes(CXmlLink xmlLink)
		
		//	Get the default name for this record
		public override string GetDefaultName()
		{
			//	Is this a link?
			if(MediaType == TmaxMediaTypes.Link)
			{
				//	Is it a link to a designation?
				if((m_dxTertiary != null) && (m_dxTertiary.MediaType == TmaxMediaTypes.Designation))
				{
					return CTmaxToolbox.PLToString(StartPL);
				}
				else
				{
					return CTmaxToolbox.SecondsToString(Start);
				}
			}
			else
			{
				return Name;
			}
			
		}
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The id of the tertiary media parent</summary>
		public long TertiaryMediaId
		{
			get { return m_lTertiaryMediaId; }
			set { m_lTertiaryMediaId = value; }
		}
		
		/// <summary>The tertiary object that owns this record</summary>
		public CDxTertiary Tertiary
		{
			get	{ return m_dxTertiary; }
			set { m_dxTertiary = value; }
		}
		
		/// <summary>The start time</summary>
		public double Start
		{
			get { return m_dStart; }
			set { m_dStart = value;	}
		}
		
		/// <summary>Flag to indicate start position has been tuned</summary>
		public bool StartTuned
		{
			get { return m_bStartTuned; }
			set { m_bStartTuned = value;	}
		}
		
		/// <summary>The start page/line</summary>
		public long StartPL
		{
			get { return m_lStartPL; }
			set { m_lStartPL = value;	}
		}
		
		/// <summary>The record exchange object for the source media</summary>
		public CDxMediaRecord Source
		{
			get { return GetSource(); }
			set { m_dxSource = value;	}
		}
		
		/// <summary>The PST identifier for the source media</summary>
		public string SourceId
		{
			get { return m_strSourceId; }
			set { m_strSourceId = value;	}
		}
		
		/// <summary>The media type for the source media</summary>
		public TmaxMediaTypes SourceType
		{
			get { return m_eSourceType; }
			set { m_eSourceType = value;	}
		}
		
		/// <summary>Flag to indicate link should be hidden</summary>
		public bool HideLink
		{
			get { return ((m_lAttributes & (long)TmaxQuaternaryAttributes.HideLink) != 0); }
			set 
			{ 
				if(value == true)
					m_lAttributes |= (long)TmaxQuaternaryAttributes.HideLink;
				else
					m_lAttributes &= ~((long)TmaxQuaternaryAttributes.HideLink);
			
			}
		
		}
		
		/// <summary>Flag to indicate link should be shown split screen</summary>
		public bool SplitLink
		{
			get { return ((m_lAttributes & (long)TmaxQuaternaryAttributes.SplitLink) != 0);  }
			set 
			{ 
				if(value == true)
					m_lAttributes |= (long)TmaxQuaternaryAttributes.SplitLink;
				else
					m_lAttributes &= ~((long)TmaxQuaternaryAttributes.SplitLink);
			}
		
		}

		/// <summary>Flag to indicate video playback should be hidden</summary>
		public bool HideVideo
		{
			get { return ((m_lAttributes & (long)TmaxQuaternaryAttributes.HideVideo) != 0); }
			set
			{
				if(value == true)
					m_lAttributes |= (long)TmaxQuaternaryAttributes.HideVideo;
				else
					m_lAttributes &= ~((long)TmaxQuaternaryAttributes.HideVideo);

			}

		}

		/// <summary>Flag to indicate video playback should be hidden</summary>
		public bool HideText
		{
			get { return ((m_lAttributes & (long)TmaxQuaternaryAttributes.HideText) != 0); }
			set
			{
				if(value == true)
					m_lAttributes |= (long)TmaxQuaternaryAttributes.HideText;
				else
					m_lAttributes &= ~((long)TmaxQuaternaryAttributes.HideText);

			}

		}

		#endregion Properties
	
	}// class CDxQuaternary

	/// <summary>
	/// This class is used to manage a ArrayList of CDxQuaternary objects
	/// </summary>
	public class CDxQuaternaries : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			TertiaryMediaId,
			BarcodeId,
			Attributes,
			MediaType,
			SourceId,
			SourceType,
			Description,
			Name,
			DisplayOrder,
			StartPL,
			Start,
			StartTuned,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "QuaternaryMedia";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Tertiary property</summary>
		private CDxTertiary m_dxTertiary = null;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxQuaternaries() : base()
		{
			m_strTableName = TABLE_NAME;
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxQuaternaries(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxQuaternary">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxQuaternary Add(CDxQuaternary dxQuaternary)
		{
			return (CDxQuaternary)base.Add(dxQuaternary);
			
		}// Add(CDxQuaternary dxQuaternary)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxQuaternaries Dispose()
		{
			return (CDxQuaternaries)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxQuaternary Find(long lAutoId, bool bBarcode)
		{
			return (CDxQuaternary)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxQuaternary Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the object at the desired index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxQuaternary this[int iIndex]
		{
			get
			{
				return (CDxQuaternary)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxQuaternary GetAt(int iIndex)
		{
			return (CDxQuaternary)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxQuaternary	dxQuaternary = (CDxQuaternary)dxRecord;
			string			strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.TertiaryMediaId.ToString() + ",");
			strSQL += (eFields.BarcodeId.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.MediaType.ToString() + ",");
			strSQL += (eFields.SourceId.ToString() + ",");
			strSQL += (eFields.SourceType.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.DisplayOrder.ToString() + ",");
			strSQL += (eFields.StartPL.ToString() + ",");
			strSQL += (eFields.Start.ToString() + ",");
			strSQL += (eFields.StartTuned.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + dxQuaternary.TertiaryMediaId.ToString() + "',");
			strSQL += ("'" + dxQuaternary.BarcodeId.ToString() + "',");
			strSQL += ("'" + dxQuaternary.Attributes.ToString() + "',");
			strSQL += ("'" + ((int)dxQuaternary.MediaType).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxQuaternary.SourceId) + "',");
			strSQL += ("'" + ((int)dxQuaternary.SourceType).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxQuaternary.Description) + "',");
			strSQL += ("'" + SQLEncode(dxQuaternary.Name) + "',");
			strSQL += ("'" + dxQuaternary.DisplayOrder.ToString() + "',");
			strSQL += ("'" + dxQuaternary.StartPL.ToString() + "',");
			strSQL += ("'" + dxQuaternary.Start.ToString() + "',");
			strSQL += ("'" + BoolToSQL(dxQuaternary.StartTuned) + "',");
			strSQL += ("'" + dxQuaternary.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxQuaternary.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxQuaternary.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxQuaternary.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxQuaternary	dxQuaternary = (CDxQuaternary)dxRecord;
			string			strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.TertiaryMediaId.ToString() + " = '" + dxQuaternary.TertiaryMediaId.ToString() + "',");
			strSQL += (eFields.BarcodeId.ToString() + " = '" + dxQuaternary.BarcodeId.ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxQuaternary.Attributes.ToString() + "',");
			strSQL += (eFields.MediaType.ToString() + " = '" + ((int)dxQuaternary.MediaType).ToString() + "',");
			strSQL += (eFields.SourceId.ToString() + " = '" + SQLEncode(dxQuaternary.SourceId) + "',");
			strSQL += (eFields.SourceType.ToString() + " = '" + ((int)dxQuaternary.SourceType).ToString() + "',");
			strSQL += (eFields.Description.ToString() + " = '" + SQLEncode(dxQuaternary.Description) + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxQuaternary.Name) + "',");
			strSQL += (eFields.DisplayOrder.ToString() + " = '" + dxQuaternary.DisplayOrder.ToString() + "',");
			strSQL += (eFields.StartPL.ToString() + " = '" + dxQuaternary.StartPL.ToString() + "',");
			strSQL += (eFields.Start.ToString() + " = '" + dxQuaternary.Start.ToString() + "',");
			strSQL += (eFields.StartTuned.ToString() + " = '" + BoolToSQL(dxQuaternary.StartTuned) + "',");
			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxQuaternary.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxQuaternary.CreatedOn.ToString() + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxQuaternary.ModifiedBy.ToString() + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxQuaternary.ModifiedOn.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxQuaternary.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to flush all records belonging to the collection
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = "DELETE FROM ";
			
			strSQL += TableName;
			strSQL += " WHERE TertiaryMediaId = ";
			strSQL += m_dxTertiary.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}
		
		/// <summary>
		/// This method is called to get the SQL statement required to select the desired records
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			
			if(m_dxTertiary != null)
			{
				strSQL += " WHERE TertiaryMediaId = ";
				strSQL += m_dxTertiary.AutoId.ToString();
				strSQL += " ORDER BY ";
				strSQL += eFields.DisplayOrder.ToString();
				strSQL += ";";
			}
			else
			{
				Debug.Assert(false);
			}
			
			return strSQL;
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			CDxQuaternary dxQuaternary = new CDxQuaternary();
			
			if(dxQuaternary != null)
			{
				dxQuaternary.Collection = this;
				dxQuaternary.Tertiary = m_dxTertiary;
				
				if(m_dxTertiary != null)
					dxQuaternary.TertiaryMediaId = m_dxTertiary.AutoId;
					
			}
			return ((CBaseRecord)dxQuaternary);
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxQuaternary dxQuaternary = (CDxQuaternary)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					//	Make sure we're using the default name for links
					if(dxQuaternary.MediaType == TmaxMediaTypes.Link)
						dxQuaternary.Name = dxQuaternary.GetDefaultName();
						
					m_dxFields[(int)eFields.AutoId].Value  = dxQuaternary.AutoId;
					m_dxFields[(int)eFields.TertiaryMediaId].Value  = dxQuaternary.TertiaryMediaId;
					m_dxFields[(int)eFields.BarcodeId].Value  = dxQuaternary.BarcodeId;
					m_dxFields[(int)eFields.Attributes].Value = dxQuaternary.Attributes;
					m_dxFields[(int)eFields.MediaType].Value = dxQuaternary.MediaType;
					m_dxFields[(int)eFields.SourceId].Value = dxQuaternary.SourceId;
					m_dxFields[(int)eFields.SourceType].Value = dxQuaternary.SourceType;
					m_dxFields[(int)eFields.Description].Value = dxQuaternary.Description;
					m_dxFields[(int)eFields.Name].Value = dxQuaternary.Name;
					m_dxFields[(int)eFields.DisplayOrder].Value = dxQuaternary.DisplayOrder;
					m_dxFields[(int)eFields.StartPL].Value = dxQuaternary.StartPL;
					m_dxFields[(int)eFields.Start].Value = dxQuaternary.Start;
					m_dxFields[(int)eFields.StartTuned].Value = dxQuaternary.StartTuned;
					m_dxFields[(int)eFields.CreatedBy].Value = dxQuaternary.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxQuaternary.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxQuaternary.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxQuaternary.ModifiedOn;
				}
				else
				{
					dxQuaternary.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxQuaternary.TertiaryMediaId = (int)(m_dxFields[(int)eFields.TertiaryMediaId].Value);
					dxQuaternary.BarcodeId = (int)(m_dxFields[(int)eFields.BarcodeId].Value);
					dxQuaternary.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxQuaternary.MediaType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)(m_dxFields[(int)eFields.MediaType].Value));
					dxQuaternary.SourceId = (string)(m_dxFields[(int)eFields.SourceId].Value);
					dxQuaternary.SourceType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)m_dxFields[(int)eFields.SourceType].Value);
					dxQuaternary.Description = (string)(m_dxFields[(int)eFields.Description].Value);
					dxQuaternary.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxQuaternary.DisplayOrder = (int)(m_dxFields[(int)eFields.DisplayOrder].Value);
					dxQuaternary.StartPL = (int)(m_dxFields[(int)eFields.StartPL].Value);
					dxQuaternary.Start = (double)(m_dxFields[(int)eFields.Start].Value);
					dxQuaternary.StartTuned = (bool)(m_dxFields[(int)eFields.StartTuned].Value);
					dxQuaternary.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxQuaternary.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxQuaternary.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxQuaternary.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
					
					//	Make sure we're using the default name for links
					if(dxQuaternary.MediaType == TmaxMediaTypes.Link)
						dxQuaternary.Name = dxQuaternary.GetDefaultName();
				}
				return true;
			}
            catch(OleDbException oleEx)
            {
                FireError("Exchange",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_EXCHANGE_FIELDS_EX,TableName,bSetFields),oleEx,GetErrorItems(dxRecord));
            }
            catch(System.Exception Ex)
            {
                FireError("Exchange",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_EXCHANGE_FIELDS_EX,TableName,bSetFields),Ex,GetErrorItems(dxRecord));
            }
			
			return false;
			
		}// Exchange()
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
		}
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="eField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.AutoId:
				case eFields.TertiaryMediaId:
				case eFields.BarcodeId:
				case eFields.Attributes:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				case eFields.Start:
				case eFields.StartPL:
				case eFields.DisplayOrder:
				
					dxField.Value = 0;
					break;
					
				case eFields.StartTuned:
				
					dxField.Value = false;
					break;
					
				case eFields.MediaType:
				case eFields.SourceType:
				
					dxField.Value = TmaxMediaTypes.Unknown;
					break;
					
				case eFields.Name:
				case eFields.SourceId:
				case eFields.Description:
				
					dxField.Value = "";
					break;
					
				case eFields.CreatedOn:
				case eFields.ModifiedOn:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>
		/// This is the primary media object that owns the tertiary collection
		/// </summary>
		public CDxTertiary Tertiary
		{
			get
			{
				return m_dxTertiary;
			}
			set
			{
				m_dxTertiary = value;
			}
			
		}// Tertiary property
		
		#endregion Properties
		
		
	}//	CDxQuaternaries
		
}// namespace FTI.Trialmax.Database
