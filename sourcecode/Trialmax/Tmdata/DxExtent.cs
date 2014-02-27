using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class encapsulates the transcript information for a deposition</summary>
	public class CDxExtent : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXEXTENT_PROP_FIRST_ID			= 11000; // Ensures unique property id
		
		protected const int DXEXTENT_PROP_START				= DXEXTENT_PROP_FIRST_ID + 1;
		protected const int DXEXTENT_PROP_START_PL			= DXEXTENT_PROP_FIRST_ID + 2;
		protected const int DXEXTENT_PROP_START_TUNED		= DXEXTENT_PROP_FIRST_ID + 3;
		protected const int DXEXTENT_PROP_STOP				= DXEXTENT_PROP_FIRST_ID + 4;
		protected const int DXEXTENT_PROP_STOP_PL			= DXEXTENT_PROP_FIRST_ID + 5;
		protected const int DXEXTENT_PROP_STOP_TUNED		= DXEXTENT_PROP_FIRST_ID + 6;
		protected const int DXEXTENT_PROP_MAX_LINE_TIME		= DXEXTENT_PROP_FIRST_ID + 7;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to SecondaryId property</summary>
		private long m_lSecondaryId = 0;
		
		/// <summary>Local member bound to TertiaryId property</summary>
		private long m_lTertiaryId = 0;
		
		/// <summary>Local member bound to HighlighterId property</summary>
		private long m_lHighlighterId = 0;
		
		/// <summary>Local member bound to XmlSegmentId property</summary>
		private long m_lXmlSegmentId = 0;
		
		/// <summary>Local member bound to Start property</summary>
		private double m_dStart = 0;
		
		/// <summary>Local member bound to Stop property</summary>
		private double m_dStop = 0;
		
		/// <summary>Local member bound to MaxLineTime property</summary>
		private double m_dMaxLineTime = -1;
		
		/// <summary>Local member bound to StartPL property</summary>
		private long m_lStartPL = 0;
		
		/// <summary>Local member bound to StopPL property</summary>
		private long m_lStopPL = 0;
		
		/// <summary>Local member bound to StartTuned property</summary>
		private bool m_bStartTuned = false;
		
		/// <summary>Local member bound to StopTuned property</summary>
		private bool m_bStopTuned = false;
		
		/// <summary>Local member bound to Secondary property</summary>
		private CDxSecondary m_dxSecondary = null;
		
		/// <summary>Local member bound to Tertiary property</summary>
		private CDxTertiary m_dxTertiary = null;
		
		/// <summary>Local member bound to Highlighter property</summary>
		private CDxHighlighter m_dxHighlighter = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxExtent() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		public CDxExtent(CDxSecondary dxSecondary) : base()
		{
			m_dxSecondary = dxSecondary;
			if(m_dxSecondary != null)
			m_lSecondaryId = m_dxSecondary.AutoId;
		}
		
		/// <summary>Constructor</summary>
		public CDxExtent(CDxTertiary dxTertiary) : base()
		{
			if((m_dxTertiary = dxTertiary) != null)
			{
				m_lTertiaryId = dxTertiary.AutoId;
				
				if((m_dxSecondary = dxTertiary.Secondary) != null)
				{
					m_lSecondaryId = m_dxSecondary.AutoId;
				}
				
			}
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			if(m_lTertiaryId > 0)
				return FTI.Shared.Trialmax.TmaxMediaLevels.Tertiary;
			else
				return FTI.Shared.Trialmax.TmaxMediaLevels.Secondary;
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This method is called to get the record exchange object for the record's highlighter</summary>
		///	<returns>The interface to the highlighter record</returns>
		public CDxHighlighter GetHighlighter()
		{
			//	Have we already located the highlighter record?
			if(m_dxHighlighter != null)
			{
				//	Has the highlighter record changed?
				if(m_dxHighlighter.AutoId == HighlighterId)
					return m_dxHighlighter;
				else
					m_dxHighlighter = null; // Need to reset the highlighter
			}
			
			//	Do we have a valid id
			if(HighlighterId > 0)
			{
				//	Do we have access to the database?
				if((this.Database != null) && (this.Database.Highlighters != null))
					m_dxHighlighter = this.Database.Highlighters.Find(m_lHighlighterId);
			}
			
			return m_dxHighlighter;
			
		}// public CDxHighlighter GetHighlighter()
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			GetProperties(tmaxProperties, true, true, true, true);
			
		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		/// <param name="tmaxProperties">The collection in which to place the properties</param>
		/// <param name="bStart">true to include start information</param>
		/// <param name="bStartPL">true to include start PL value</param>
		/// <param name="bStop">true to include stop information</param>
		/// <param name="bStopPL">true to include stop PL value</param>
		public void GetProperties(CTmaxProperties tmaxProperties, bool bStart, bool bStartPL, bool bStop, bool bStopPL)
		{
			if(bStart == true)
			{
				tmaxProperties.Add(DXEXTENT_PROP_START, "Start Time", Start, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXEXTENT_PROP_START_TUNED, "Start Tuned", StartTuned, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				if(bStartPL == true)
					tmaxProperties.Add(DXEXTENT_PROP_START_PL, "Start PG:LN", CTmaxToolbox.PLToString(StartPL), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			}
			
			if(bStop == true)
			{
				tmaxProperties.Add(DXEXTENT_PROP_STOP, "Stop Time", Stop, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXEXTENT_PROP_STOP_TUNED, "Stop Tuned", StopTuned, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				if(bStopPL == true)
				{
					tmaxProperties.Add(DXEXTENT_PROP_STOP_PL, "Stop PG:LN", CTmaxToolbox.PLToString(StopPL), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXEXTENT_PROP_MAX_LINE_TIME, "Max Line Time", MaxLineTime, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				}
				
			}
			
			//	Should we add highlighter properties?
			if(GetHighlighter() != null)
				GetHighlighter().GetProperties(tmaxProperties);
			
		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXEXTENT_PROP_START:
				
					tmaxProperty.Value = Start;
					break;
					
				case DXEXTENT_PROP_START_PL:
				
					tmaxProperty.Value = CTmaxToolbox.PLToString(StartPL);
					break;
					
				case DXEXTENT_PROP_START_TUNED:
				
					tmaxProperty.Value = StartTuned;
					break;
					
				case DXEXTENT_PROP_STOP:
				
					tmaxProperty.Value = Stop;
					break;
					
				case DXEXTENT_PROP_STOP_PL:
				
					tmaxProperty.Value = CTmaxToolbox.PLToString(StopPL);
					break;
					
				case DXEXTENT_PROP_STOP_TUNED:
				
					tmaxProperty.Value = StopTuned;
					break;
					
				default:
				
					//	This could be a highlighter property
					if(GetHighlighter() != null)
						GetHighlighter().RefreshProperty(tmaxProperty);
					
					//	No need to call base class because we don't add base class
					//	properties to the caller's collection
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The AutoId of the secondary record that owns this record</summary>
		public long SecondaryId
		{
			get { return m_lSecondaryId; }
			set { m_lSecondaryId = value;	}
		}
		
		/// <summary>The AutoId of the tertiary record that owns this record</summary>
		public long TertiaryId
		{
			get { return m_lTertiaryId; }
			set { m_lTertiaryId = value;	}
		}
		
		/// <summary>The segment identifier as defined in the XML file</summary>
		public long XmlSegmentId
		{
			get { return m_lXmlSegmentId; }
			set { m_lXmlSegmentId = value;	}
		}
		
		/// <summary>The AutoId of the highlighter used to create the designation</summary>
		public long HighlighterId
		{
			get { return m_lHighlighterId; }
			set { m_lHighlighterId = value;	}
		}
		
		/// <summary>The highlighter record exchange object</summary>
		public CDxHighlighter Highlighter
		{
			get
			{
				return GetHighlighter();
			}
			set
			{
				m_dxHighlighter = value;
			}
		}
		
		/// <summary>The start time</summary>
		public double Start
		{
			get { return m_dStart; }
			set { m_dStart = value;	}
		}
		
		/// <summary>The stop time</summary>
		public double Stop
		{
			get { return m_dStop; }
			set { m_dStop = value;	}
		}
		
		/// <summary>The time required to play the longest line (if not tuned)</summary>
		public double MaxLineTime
		{
			get { return m_dMaxLineTime; }
			set { m_dMaxLineTime = value;	}
		}
		
		/// <summary>The start page/line</summary>
		public long StartPL
		{
			get { return m_lStartPL; }
			set { m_lStartPL = value;	}
		}
		
		/// <summary>The stop page/line</summary>
		public long StopPL
		{
			get { return m_lStopPL; }
			set { m_lStopPL = value;	}
		}
		
		/// <summary>Flag to indicate start position has been tuned</summary>
		public bool StartTuned
		{
			get { return m_bStartTuned; }
			set { m_bStartTuned = value;	}
		}
		
		/// <summary>Flag to indicate stop position has been tuned</summary>
		public bool StopTuned
		{
			get { return m_bStopTuned; }
			set { m_bStopTuned = value;	}
		}
		
		/// <summary>The secondary object that owns this record</summary>
		public CDxSecondary Secondary
		{
			get { return m_dxSecondary; }
			set { m_dxSecondary = value; }
		}
		
		/// <summary>The tertiary object that owns this record</summary>
		public CDxTertiary Tertiary
		{
			get { return m_dxTertiary; }
			set { m_dxTertiary = value; }
		}
		
		#endregion Properties
	
	}// class CDxExtent

	/// <summary>This class is used to manage a ArrayList of CDxExtent objects</summary>
	public class CDxExtents : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			SecondaryId,
			TertiaryId,
			XmlSegmentId,
			HighlighterId,
			Start,
			Stop,
			StartTuned,
			StopTuned,
			StartPL,
			StopPL,
			MaxLineTime,
		}

		public const string TABLE_NAME = "Extents";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Secondary property</summary>
		private CDxSecondary m_dxSecondary = null;
		
		/// <summary>Local member bound to Tertiary property</summary>
		private CDxTertiary m_dxTertiary = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxExtents() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxExtents(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

        /// <summary>This function is called to set the database interface</summary>
        public override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
        {
            //	Do the base class processing first
            base.SetDatabase(tmaxDatabase);

            if(this.Database != null)
            {
                if((m_dxFields != null) && ((int)eFields.MaxLineTime < m_dxFields.Count))
                    m_dxFields[(int)(eFields.MaxLineTime)].Index = this.Database.MaxLineTimeIndex;
            }

        }// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)

        /// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="DxExtent">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxExtent Add(CDxExtent DxExtent)
		{
			return (CDxExtent)base.Add(DxExtent);
			
		}// public CDxExtent Add(CDxExtent DxExtent)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxExtents Dispose()
		{
			return (CDxExtents)base.Dispose();
			
		}// public new CDxExtents Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxExtent Find(long lAutoId, bool bBarcode)
		{
			return (CDxExtent)base.Find(lAutoId, bBarcode);
			
		}//	public new CDxExtent Find(long lAutoId, bool bBarcode)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxExtent Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	public new CDxExtent Find(long lAutoId)

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxExtent this[int iIndex]
		{
			get
			{
				return (CDxExtent)base[iIndex];
			}
		
		}// public new CDxExtent this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxExtent GetAt(int iIndex)
		{
			return (CDxExtent)base.GetAt(iIndex);
		
		}// public new CDxExtent GetAt(int iIndex)

		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string	strSQL = "SELECT * FROM " + m_strTableName;
			long	lSecondary = 0;
			long	lTertiary = 0;
			
			//	Does a tertiary record own this collection?
			if(m_dxTertiary != null)
			{
				lTertiary = m_dxTertiary.AutoId;
				
				if(m_dxTertiary.Secondary != null)
				{
					lSecondary = m_dxSecondary.AutoId;
				}
				else				
				{
					Debug.Assert(false);
					
					if(m_dxSecondary != null)
						lSecondary = m_dxSecondary.AutoId;
					else
						return "";
				}
				
			}
			else if(m_dxSecondary != null)
			{
				lSecondary = m_dxSecondary.AutoId;
			}
			else
			{
				Debug.Assert(false);
				return "";
			}
			
			strSQL += " WHERE SecondaryId = ";
			strSQL += lSecondary.ToString();
			strSQL += " AND TertiaryId = ";
			strSQL += lTertiary.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLSelect()

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxExtent	dxExtent = (CDxExtent)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.SecondaryId.ToString() + ",");
			strSQL += (eFields.TertiaryId.ToString() + ",");
			strSQL += (eFields.XmlSegmentId.ToString() + ",");
			strSQL += (eFields.HighlighterId.ToString() + ",");
			strSQL += (eFields.Start.ToString() + ",");
			strSQL += (eFields.Stop.ToString() + ",");
			strSQL += (eFields.StartTuned.ToString() + ",");
			strSQL += (eFields.StopTuned.ToString() + ",");
			strSQL += (eFields.StartPL.ToString() + ",");
			strSQL += (eFields.StopPL.ToString());
			if(this.GetMaxLineTimeEnabled() == true)
				strSQL += ("," + eFields.MaxLineTime.ToString());
			strSQL +=  ")";

			strSQL += " VALUES(";
			strSQL += ("'" + dxExtent.SecondaryId.ToString() + "',");
			strSQL += ("'" + dxExtent.TertiaryId.ToString() + "',");
			strSQL += ("'" + dxExtent.XmlSegmentId.ToString() + "',");
			strSQL += ("'" + dxExtent.HighlighterId.ToString() + "',");
			strSQL += ("'" + dxExtent.Start.ToString() + "',");
			strSQL += ("'" + dxExtent.Stop.ToString() + "',");
			strSQL += ("'" + BoolToSQL(dxExtent.StartTuned) + "',");
			strSQL += ("'" + BoolToSQL(dxExtent.StopTuned) + "',");
			strSQL += ("'" + dxExtent.StartPL.ToString() + "',");
			strSQL += ("'" + dxExtent.StopPL.ToString() + "'");
			if(this.GetMaxLineTimeEnabled() == true)
				strSQL += (",'" + dxExtent.MaxLineTime.ToString() + "'");
			strSQL += ")";
			
			return strSQL;
		
		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxExtent	dxExtent = (CDxExtent)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.SecondaryId.ToString() + " = '" + dxExtent.SecondaryId.ToString() + "',");
			strSQL += (eFields.TertiaryId.ToString() + " = '" + dxExtent.TertiaryId.ToString() + "',");
			strSQL += (eFields.XmlSegmentId.ToString() + " = '" + dxExtent.XmlSegmentId.ToString() + "',");
			strSQL += (eFields.HighlighterId.ToString() + " = '" + dxExtent.HighlighterId.ToString() + "',");
			strSQL += (eFields.Start.ToString() + " = '" + dxExtent.Start.ToString() + "',");
			strSQL += (eFields.Stop.ToString() + " = '" + dxExtent.Stop.ToString() + "',");
			strSQL += (eFields.StartTuned.ToString() + " = '" + BoolToSQL(dxExtent.StartTuned) + "',");
			strSQL += (eFields.StopTuned.ToString() + " = '" + BoolToSQL(dxExtent.StopTuned) + "',");
			strSQL += (eFields.StartPL.ToString() + " = '" + dxExtent.StartPL.ToString() + "',");
			strSQL += (eFields.StopPL.ToString() + " = '" + dxExtent.StopPL.ToString() + "'");

			if(this.GetMaxLineTimeEnabled() == true)
			{
				strSQL += ",";
				strSQL += (eFields.MaxLineTime.ToString() + " = '" + dxExtent.MaxLineTime.ToString() + "'");
			}
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxExtent.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>Called to determine if the MaxLineTime column is available</summary>
		/// <returns>True if available</returns>
		public bool GetMaxLineTimeEnabled()
		{
			if((m_dxFields != null) && ((int)(eFields.MaxLineTime) < m_dxFields.Count))
				return (m_dxFields[(int)eFields.MaxLineTime].Index >= 0);
			else
				return false;
		
		}// public bool GetMaxLineTimeEnabled()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();
			
			//	The MaxLineTime column may or may not exist in the database
			if(this.Database != null)
			{
				if((m_dxFields != null) && ((int)eFields.MaxLineTime < m_dxFields.Count))
					m_dxFields[(int)(eFields.MaxLineTime)].Index = this.Database.MaxLineTimeIndex;
			}

		}// protected override void SetFields()
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			CDxExtent dxExtent = new CDxExtent();
			
			if(dxExtent != null)
			{
				dxExtent.Collection = this;
				dxExtent.Secondary = m_dxSecondary;
				dxExtent.Tertiary = m_dxTertiary;
				
				if(m_dxSecondary != null)
					dxExtent.SecondaryId = m_dxSecondary.AutoId;
				if(m_dxTertiary != null)
					dxExtent.TertiaryId = m_dxTertiary.AutoId;
			}
			
			return ((CBaseRecord)dxExtent);
		
		}// protected override CDxMediaRecord GetNewRecord()
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxExtent dxExtent = (CDxExtent)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxExtent.AutoId;
					m_dxFields[(int)eFields.SecondaryId].Value  = dxExtent.SecondaryId;
					m_dxFields[(int)eFields.TertiaryId].Value  = dxExtent.TertiaryId;
					m_dxFields[(int)eFields.XmlSegmentId].Value  = dxExtent.XmlSegmentId;
					m_dxFields[(int)eFields.HighlighterId].Value  = dxExtent.HighlighterId;
					m_dxFields[(int)eFields.Start].Value  = dxExtent.Start;
					m_dxFields[(int)eFields.Stop].Value  = dxExtent.Stop;
					m_dxFields[(int)eFields.StartTuned].Value = dxExtent.StartTuned;
					m_dxFields[(int)eFields.StopTuned].Value = dxExtent.StopTuned;
					m_dxFields[(int)eFields.StartPL].Value  = dxExtent.StartPL;
					m_dxFields[(int)eFields.StopPL].Value  = dxExtent.StopPL;

					if(this.GetMaxLineTimeEnabled() == true)
						m_dxFields[(int)eFields.MaxLineTime].Value  = dxExtent.MaxLineTime;
				}
				else
				{
					dxExtent.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxExtent.SecondaryId = (int)(m_dxFields[(int)eFields.SecondaryId].Value);
					dxExtent.TertiaryId = (int)(m_dxFields[(int)eFields.TertiaryId].Value);
					dxExtent.XmlSegmentId = (int)(m_dxFields[(int)eFields.XmlSegmentId].Value);
					dxExtent.HighlighterId = (int)(m_dxFields[(int)eFields.HighlighterId].Value);
					dxExtent.Start = (double)(m_dxFields[(int)eFields.Start].Value);
					dxExtent.Stop = (double)(m_dxFields[(int)eFields.Stop].Value);
					dxExtent.StartTuned = (bool)(m_dxFields[(int)eFields.StartTuned].Value);
					dxExtent.StopTuned = (bool)(m_dxFields[(int)eFields.StopTuned].Value);
					dxExtent.StartPL = (int)(m_dxFields[(int)eFields.StartPL].Value);
					dxExtent.StopPL = (int)(m_dxFields[(int)eFields.StopPL].Value);

					if(this.GetMaxLineTimeEnabled() == true)
					{
						//	This allows for NULL fields that may occur if the column was added with existing records
						try   { dxExtent.MaxLineTime = (double)(m_dxFields[(int)eFields.MaxLineTime].Value); }
						catch { dxExtent.MaxLineTime = -1.0; }
					}
				
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

		}// protected override bool Exchange(CDxMediaRecord dxRecord, bool bSetFields)
		
		#endregion Protected Methods
	
		#region Private Methods
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
		}
		
		/// <param name="eField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.AutoId:
				case eFields.SecondaryId:
				case eFields.TertiaryId:
				case eFields.XmlSegmentId:
				case eFields.HighlighterId:
				case eFields.Start:
				case eFields.Stop:
				case eFields.StartPL:
				case eFields.StopPL:
				case eFields.MaxLineTime:
				
					dxField.Value = 0;
					break;
					
				case eFields.StartTuned:
				case eFields.StopTuned:
				
					dxField.Value = false;
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods
	
		#region Properties
		
		/// <summary>The secondary object that owns this record</summary>
		public CDxSecondary Secondary
		{
			get { return m_dxSecondary; }
			set { m_dxSecondary = value; }
		}
		
		/// <summary>The tertiary object that owns this record</summary>
		public CDxTertiary Tertiary
		{
			get { return m_dxTertiary; }
			set { m_dxTertiary = value; }
		}
		
		#endregion Properties
		
	}//	CDxExtents
		
}// namespace FTI.Trialmax.Database
