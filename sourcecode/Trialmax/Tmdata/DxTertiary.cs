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
	/// This class encapsulates the information used to exchange a SecondaryMedia record
	/// </summary>
	public class CDxTertiary : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXTERTIARY_PROP_FIRST_ID		= DX_MEDIA_RECORD_PROP_LAST + 3000;
		
		protected const int DXTERTIARY_PROP_BARCODE			= DXTERTIARY_PROP_FIRST_ID + 1;
		protected const int DXTERTIARY_PROP_MEDIA_TYPE		= DXTERTIARY_PROP_FIRST_ID + 2;
		protected const int DXTERTIARY_PROP_MEDIA_PATH		= DXTERTIARY_PROP_FIRST_ID + 3;
		protected const int DXTERTIARY_PROP_FILENAME		= DXTERTIARY_PROP_FIRST_ID + 4;
		protected const int DXTERTIARY_PROP_DISPLAY_ORDER	= DXTERTIARY_PROP_FIRST_ID + 5;
		protected const int DXTERTIARY_PROP_CHILDREN		= DXTERTIARY_PROP_FIRST_ID + 6;
		protected const int DXTERTIARY_PROP_BARCODE_ID		= DXTERTIARY_PROP_FIRST_ID + 7;
		protected const int DXTERTIARY_PROP_SOURCE_FILENAME	= DXTERTIARY_PROP_FIRST_ID + 8;
		protected const int DXTERTIARY_PROP_SOURCE_PATH		= DXTERTIARY_PROP_FIRST_ID + 9;
		protected const int DXTERTIARY_PROP_DURATION		= DXTERTIARY_PROP_FIRST_ID + 10;
		protected const int DXTERTIARY_PROP_FOREIGN_BARCODE	= DXTERTIARY_PROP_FIRST_ID + 11;
		protected const int DXTERTIARY_PROP_MAPPED			= DXTERTIARY_PROP_FIRST_ID + 12;
		protected const int DXTERTIARY_PROP_HAS_SHORTCUTS	= DXTERTIARY_PROP_FIRST_ID + 13;
		protected const int DXTERTIARY_PROP_SIBLING_ID		= DXTERTIARY_PROP_FIRST_ID + 14;
		protected const int DXTERTIARY_PROP_SPLIT_SCREEN_ID = DXTERTIARY_PROP_FIRST_ID + 15;
		protected const int DXTERTIARY_PROP_SPLIT_HORZ_ID	= DXTERTIARY_PROP_FIRST_ID + 16;
		protected const int DXTERTIARY_PROP_SPLIT_RIGHT_ID	= DXTERTIARY_PROP_FIRST_ID + 17;
		
		#endregion Constants
	
		#region Private Members
		
		/// <summary>Local member bound to Quaternaries property</summary>
		private CDxQuaternaries m_dxQuaternaries = new CDxQuaternaries();
		
		/// <summary>Local member bound to Secondary property</summary>
		protected CDxSecondary m_dxSecondary = null;
		
		/// <summary>Local member bound to Extents property</summary>
		private CDxExtents m_dxExtents = null;
		
		/// <summary>Local member bound to SecondaryMediaId property</summary>
		protected long m_lSecondaryMediaId = 0;
		
		/// <summary>Local member bound to Filename property</summary>
		protected string m_strFilename = "";
		
		/// <summary>Local member bound to SourceId property</summary>
		private string m_strSourceId = "";
		
		/// <summary>Local member bound to SourceType property</summary>
		private TmaxMediaTypes m_eSourceType = TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to Source property</summary>
		private CDxMediaRecord m_dxSource = null;

		/// <summary>Local member bound to SiblingId property</summary>
		private string m_strSiblingId = "";

		/// <summary>Local member bound to Sibling property</summary>
		private CDxTertiary m_dxSibling = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxTertiary() : base()
		{
			Initialize(null);
		}
		
		/// <summary>Overloaded constructor</summary>
		///	<param name="dxSecondary">The secondary exchange object that owns this tertiary object</param>
		public CDxTertiary(CDxSecondary dxSecondary) : base()
		{
			Initialize(dxSecondary);
		}
		
		/// <summary>This method is called to add the specified transcript</summary>
		/// <param name="dxExtent">Extent media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxExtent dxExtent)
		{
			if(m_dxExtents == null)
				CreateExtents();
				
			if(m_dxExtents != null)
				return (m_dxExtents.Add(dxExtent) != null);
			else
				return false;
		}

		/// <summary>This method is called to add a quaternary media object to the local collection</summary>
		/// <param name="dxQuaternary">Quaternary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxQuaternary dxQuaternary)
		{
			if(m_dxQuaternaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxQuaternaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct secondary id
				dxQuaternary.Tertiary = this;
				dxQuaternary.TertiaryMediaId = AutoId;
				
				return (m_dxQuaternaries.Add(dxQuaternary) != null);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to fill the secondaries collection</summary>
		/// <returns>true if successful</returns>
		override public bool Fill()
		{
			bool bSuccessful = false;
			
			if(m_dxQuaternaries != null)
			{
				//	Make sure the tertiary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxQuaternaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Clear the existing objects
				m_dxQuaternaries.Clear();
				
				//	Is this a designation or clip?
				if((this.MediaType == TmaxMediaTypes.Designation) ||
				   (this.MediaType == TmaxMediaTypes.Clip))
				{
					//	Have we already assigned the sorter?
					if(m_dxQuaternaries.Comparer == null)
					{
						m_dxQuaternaries.Comparer = new CTmaxRecordSorter();
						m_dxQuaternaries.KeepSorted = false;
					}
					
					if((bSuccessful = m_dxQuaternaries.Fill()) == true)
					{					
						//	Now sort the collection based on start position
						m_dxQuaternaries.Sort();
					}
				
				}
				else
				{
					bSuccessful = m_dxQuaternaries.Fill();
				}
			
				//	Were we able to fill the child collection?
				if(bSuccessful == true)
				{
					//	Just in case something messed up..........
					if(this.ChildCount != m_dxQuaternaries.Count)
					{
						//Debug.Assert(this.ChildCount == m_dxQuaternaries.Count);
						
						if(Collection != null)
						{
							this.ChildCount = m_dxQuaternaries.Count;
							Collection.Update(this);
						}
						
					}// if(this.ChildCount != m_dxSecondaries.Count)
					
				}// if(bSuccessful == true)
				
			}// if(m_dxQuaternaries != null)
			
			return bSuccessful;
		
		}// public bool Fill()
		
		/// <summary>This method is called to update a quaternary media object</summary>
		/// <param name="dxQuaternary">Quaternary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Update(CDxQuaternary dxQuaternary)
		{
			if(m_dxQuaternaries != null)
			{
				//	Make sure the tertiary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxQuaternaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct primary id
				dxQuaternary.TertiaryMediaId = AutoId;
				
				return m_dxQuaternaries.Update(dxQuaternary);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to delete a quaternary media object</summary>
		/// <param name="dxQuaternary">Quaternary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Delete(CDxQuaternary dxQuaternary)
		{
			int iIndex = -1;
			
			Debug.Assert(m_dxQuaternaries != null);
			if(m_dxQuaternaries == null) return false;
			
			//	Make sure this secondary exists in the collection
			if((iIndex = m_dxQuaternaries.IndexOf(dxQuaternary)) < 0) return false;
			
			//	Make sure the tertiary collection is using the correct database
			if(m_dxCollection != null)
			{
                m_dxQuaternaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
			}
			
			if(m_dxQuaternaries.Delete(dxQuaternary) == true)
			{
				//	Update the display order of remaining records
				for(int i = iIndex; i < m_dxQuaternaries.Count; i++)
				{
					m_dxQuaternaries[i].DisplayOrder = (i + 1);
					m_dxQuaternaries.Update(m_dxQuaternaries[i]);
				}
					
				//	Update this record
				if(Collection != null)
				{
					this.ChildCount = m_dxQuaternaries.Count;
					Collection.Update(this);
				}
					
				return true;
				
			}
			
			//	Must have been an error
			return false;
		
		}// Delete(CDxQuaternary dxQuaternary)
		
		/// <summary>This function is called to delete the specified child record</summary>
		public override bool Delete(CDxMediaRecord dxChild)
		{
			return Delete((CDxQuaternary)dxChild);
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.Tertiary;
		}
		
		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		public override string GetMediaId()
		{
			if((Secondary != null) && (Secondary.Primary != null))
				return Secondary.Primary.GetMediaId();
			else
				return "";
		}

		/// <summary>This function is called to get the collection of child records</summary>
		/// <returns>The child collection if it exists</returns>
		public override CDxMediaRecords GetChildCollection()
		{
			return m_dxQuaternaries;
		}
		
		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public override CDxMediaRecord GetParent()
		{
			return m_dxSecondary;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	Add the base class properties first
			base.GetProperties(tmaxProperties);
			
			//	We want these to appear first in the list for treatments
			if(m_eMediaType == TmaxMediaTypes.Treatment)
			{
				tmaxProperties.Add(DXTERTIARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXTERTIARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
				tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
			}
			
			//	These are added for all types
			tmaxProperties.Add(DXTERTIARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTERTIARY_PROP_MEDIA_PATH, "Media Path", GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTERTIARY_PROP_FILENAME, "Filename", GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTERTIARY_PROP_SOURCE_FILENAME, "Source Filename", Secondary.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTERTIARY_PROP_SOURCE_PATH, "Source Path", Secondary.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				
			//	Now add any additional type-specific properties
			switch(m_eMediaType)
			{
				case TmaxMediaTypes.Treatment:
					
					tmaxProperties.Add(DXTERTIARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_BARCODE_ID, "Barcode Id", m_lBarcodeId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_SIBLING_ID, "Sibling Id", this.SiblingId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_SPLIT_SCREEN_ID, "Split Screen", this.SplitScreen, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_SPLIT_HORZ_ID, "Split Horizontal", this.SplitHorizontal, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_SPLIT_RIGHT_ID, "Split Right", this.SplitRight, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					break;
					
				case TmaxMediaTypes.Clip:
				
					tmaxProperties.Add(DXTERTIARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					if(GetExtent() != null)
						GetExtent().GetProperties(tmaxProperties, true, false, true, false);
					tmaxProperties.Add(DXTERTIARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_HAS_SHORTCUTS, "HasShortcuts", HasShortcuts, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					break;
					
				case TmaxMediaTypes.Designation:
				
					tmaxProperties.Add(DXTERTIARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					if(GetExtent() != null)
						GetExtent().GetProperties(tmaxProperties, true, true, true, true);
					tmaxProperties.Add(DXTERTIARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXTERTIARY_PROP_HAS_SHORTCUTS, "HasShortcuts", HasShortcuts, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					break;
					
				default:
				
					Debug.Assert(false);
					break;
					
			}// switch(m_eMediaType)
			
		}// public override void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXTERTIARY_PROP_BARCODE:
				
					tmaxProperty.Value = this.Database.GetBarcode(this, true);
					break;
					
				case DXTERTIARY_PROP_MEDIA_PATH:
				
					tmaxProperty.Value = GetFileSpec();
					break;
					
				case DXTERTIARY_PROP_FILENAME:
				
					tmaxProperty.Value = GetFileName();
					break;
					
				case DXTERTIARY_PROP_DISPLAY_ORDER:
				
					tmaxProperty.Value = m_lDisplayOrder;
					break;
					
				case DXTERTIARY_PROP_SOURCE_FILENAME:
				
					tmaxProperty.Value = Secondary.GetFileName();
					break;
					
				case DXTERTIARY_PROP_SOURCE_PATH:
				
					tmaxProperty.Value = Secondary.GetFileSpec();
					break;
					
				case DXTERTIARY_PROP_CHILDREN:
				
					tmaxProperty.Value = m_lChildCount;
					break;
					
				case DXTERTIARY_PROP_BARCODE_ID:
				
					tmaxProperty.Value = m_lBarcodeId;
					break;
					
				case DXTERTIARY_PROP_MAPPED:
				
					tmaxProperty.Value = Mapped;
					break;
					
				case DXTERTIARY_PROP_HAS_SHORTCUTS:
				
					tmaxProperty.Value = HasShortcuts;
					break;
					
				case DXTERTIARY_PROP_FOREIGN_BARCODE:
				
					tmaxProperty.Value = GetForeignBarcode();
					break;
					
				case DXTERTIARY_PROP_DURATION:
				
					tmaxProperty.Value = CTmaxToolbox.SecondsToString(GetDuration());
					break;

				case DXTERTIARY_PROP_SIBLING_ID:

					tmaxProperty.Value = this.SiblingId;
					break;

				case DXTERTIARY_PROP_SPLIT_SCREEN_ID:

					tmaxProperty.Value = this.SplitScreen;
					break;

				case DXTERTIARY_PROP_SPLIT_HORZ_ID:

					tmaxProperty.Value = this.SplitHorizontal;
					break;

				case DXTERTIARY_PROP_SPLIT_RIGHT_ID:

					tmaxProperty.Value = this.SplitRight;
					break;

				//	These properties are read-only
				case DXTERTIARY_PROP_MEDIA_TYPE:
				
					break;
					
				default:
				
					//	Could be an extents property
					if(GetExtent() != null)
						GetExtent().RefreshProperty(tmaxProperty);
						
					base.RefreshProperty(tmaxProperty);
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		/// <summary>This function is called to get the name of the file associated with the record</summary>
		/// <returns>The name of the associated file</returns>
		public override string GetFileName()
		{
			if(m_strFilename.Length > 0)
				return m_strFilename;
			else
				return base.GetFileName();
		}
		
		/// <summary>This method retrieves the extent exchange object associated with this record</summary>
		/// <returns>The extent's record exchange object</returns>
		public CDxExtent GetExtent()
		{
			//	Do we need to create the interface to the transcripts table?
			if(m_dxExtents == null)
			{
				if(CreateExtents() == false)
					return null;
				else
					m_dxExtents.Fill();
			}
				
			if(m_dxExtents.Count > 0)
				return (m_dxExtents[0] as CDxExtent);
			else
				return null; // Must not be a record for the extents

		}// public CDxExtent GetExtent()
		
		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public override bool GetCanClean(bool bFill)
		{
			//	Must be a treatment
			switch(this.MediaType)
			{
				case TmaxMediaTypes.Treatment:
				
					//	Use the parent secondary
					if(this.Secondary != null)
						return (this.Secondary.GetCanClean(bFill));
					break;
			}
			
			return false;
			
		}// public override bool GetCanClean(bool bFill)
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public override string GetText()
		{
			if((this.Database != null) && (this.Database.AppOptions != null))
				return GetText(this.Database.AppOptions.TertiaryTextMode);
			else
				return GetDefaultName();
		}
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		/// <param name="eMode">The desired TrialMax text mode</param>
		/// <returns>The text that represents this record</returns>
		public override string GetText(TmaxTextModes eMode)
		{
			string strText = "";

			if((eMode & TmaxTextModes.DisplayOrder) == TmaxTextModes.DisplayOrder)
			{
				switch(m_eMediaType)
				{
					case TmaxMediaTypes.Treatment:
					case TmaxMediaTypes.Link:
					
						strText = m_lDisplayOrder.ToString();
						break;
						
					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					default:
					
						break;
				}
				
			}
			
			if((eMode & TmaxTextModes.Barcode) == TmaxTextModes.Barcode)
			{
				switch(m_eMediaType)
				{
					case TmaxMediaTypes.Treatment:
					
						if(strText.Length > 0)
							strText += " - ";
						
						strText += this.Database.GetBarcode(this, false);
						
						//	Is this a split screen treatment?
						if((this.SplitScreen == true) && (this.SiblingId.Length > 0))
						{
							//	Add the barcode of the sibling
							if(this.Sibling != null)
								strText += String.Format(" <{0}>", this.Database.GetBarcode(this.Sibling, false));
							else
								strText += String.Format(" <!{0}!>", this.SiblingId);

						}// if((this.SplitScreen == true) && (this.SiblingId.Length > 0))
						
						break;
						
					case TmaxMediaTypes.Link:
					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					default:
					
						break;
				}
			}
			
			if((eMode & TmaxTextModes.Name) == TmaxTextModes.Name)
			{
				if(m_strName.Length > 0)
				{
					if(strText.Length > 0)
						strText += " - ";
						
					strText += m_strName;
				}
				else
				{
					switch(m_eMediaType)
					{
						case TmaxMediaTypes.Link:
						case TmaxMediaTypes.Designation:
						case TmaxMediaTypes.Clip:
					
							if(strText.Length > 0)
								strText += " - ";
						
							strText += GetDefaultName();
							break;
						
						case TmaxMediaTypes.Treatment:
						default:
					
							break;
					}
				}
				
			}
			
			if((eMode & TmaxTextModes.Filename) == TmaxTextModes.Filename)
			{
				switch(m_eMediaType)
				{
					case TmaxMediaTypes.Treatment:
					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					
						if(m_strFilename.Length > 0)
						{
							if(strText.Length > 0)
								strText += " - ";
						
							strText += m_strFilename;
						}
						break;
						
					case TmaxMediaTypes.Link:
					
						try
						{
							if((m_dxSecondary != null) && (m_dxSecondary.GetSource() != null))
							{
								//	The source record MUST be a clip or designation
								if(((CDxTertiary)m_dxSecondary.GetSource()).Filename.Length > 0)
								{
									if(strText.Length > 0)
										strText += " - ";
						
									strText += ((CDxTertiary)m_dxSecondary.GetSource()).Filename;
								}
								
							}
						
						}
						catch
						{
						}
						break;
						
					default:
					
						break;
				}

			}
			
			//	Do we need to set default values?
			if(strText.Length == 0)
			{
				if(m_eMediaType == TmaxMediaTypes.Treatment)
				{
					strText = this.Database.GetBarcode(this, false);

					//	Is this a split screen treatment?
					if((this.SplitScreen == true) && (this.SiblingId.Length > 0))
					{
						//	Add the barcode of the sibling
						if(this.Sibling != null)
							strText += String.Format(" <{0}>", this.Database.GetBarcode(this.Sibling, false));
						else
							strText += String.Format(" <!{0}!>", this.SiblingId);

					}// if((this.SplitScreen == true) && (this.SiblingId.Length > 0))

				}
				else
				{
					strText = GetDefaultName();
				}
			}
				
			return strText;
		}
					
		/// <summary>This function is called to get the name of the record</summary>
		/// <returns>The default name that represents this record</returns>
		public override string GetDefaultName()
		{
			string strName = "";
			
			switch(m_eMediaType)
			{
				case TmaxMediaTypes.Treatment:
				
					if(this.Database != null)
					{
						strName = this.Database.GetBarcode(this, false);
					}
					break;
					
				case TmaxMediaTypes.Designation:
				
					if(GetExtent() != null)
					{
						strName = CTmaxToolbox.PLToString(GetExtent().StartPL);
						strName += " - ";
						strName += CTmaxToolbox.PLToString(GetExtent().StopPL);
						
						if((m_dxSecondary != null) && (m_dxSecondary.Primary != null))
						{
							if(strName.Length > 0)
								strName += " ";
							strName += m_dxSecondary.Primary.Name;
						}

					}
					break;
					
				case TmaxMediaTypes.Clip:
				
					if(GetExtent() != null)
					{
						strName = CTmaxToolbox.SecondsToString(GetExtent().Start);
						strName += " - ";
						strName += CTmaxToolbox.SecondsToString(GetExtent().Stop);
						
						if((m_dxSecondary != null) && (m_dxSecondary.Filename != null))
						{
							if(strName.Length > 0)
								strName += " ";
							strName += m_dxSecondary.Filename;
						}

					}
					break;
					
				default:
				
					break;
			}
				
			if(strName.Length == 0)
				strName = m_lAutoId.ToString();
				
			return strName;
		
		}// protected override string GetDefaultName()
					
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

		/// <summary>This method is called to get the record exchange object for the sibling treatment</summary>
		public CDxTertiary GetSibling(bool bRefresh)
		{
			//	Have we already located the sibling record?
			if((bRefresh == false) && (m_dxSibling != null)) 
				return m_dxSibling;

			//	Do we have a valid id
			if((SiblingId != null) && (SiblingId.Length > 0))
			{
				//	Do we have access to the database?
				if(this.Database != null)
				{
					try
					{
						m_dxSibling = (CDxTertiary)(this.Database.GetRecordFromId(SiblingId, true));
					}
					catch
					{
						m_dxSibling = null;
					}

				}// if(this.Database != null)

			}// if((SiblingId != null) && (SiblingId.Length > 0))

			return m_dxSibling;

		}// public CDxTertiary GetSibling()

		/// <summary>This function is called to get the record's Admitted state</summary>
		/// <returns>True if the record has been Admitted</returns>
		public override bool GetAdmitted()
		{
			//	Assume the record is admitted if the parent is admitted
			if(m_dxSecondary != null)
				return m_dxSecondary.GetAdmitted();
			else
				return false;

		}// public override bool GetAdmitted()
		
		/// <summary>This method sets the XML attributes using the current property values</summary>
		/// <param name="xmlDesignation">The target XML designation</param>
		/// <returns>true if successful</returns>
		public bool SetAttributes(CXmlDesignation xmlDesignation)
		{
			xmlDesignation.CreatedBy	= this.Database.GetUserName(CreatedBy);
			xmlDesignation.CreatedOn	= CreatedOn.ToString();
			xmlDesignation.ModifiedBy	= this.Database.GetUserName(ModifiedBy);
			xmlDesignation.ModifiedOn	= ModifiedOn.ToString();
			xmlDesignation.Start		= Start;
			xmlDesignation.Stop			= Stop;
			xmlDesignation.StartTuned	= StartTuned;
			xmlDesignation.StopTuned	= StopTuned;
			xmlDesignation.FirstPL		= StartPL;
			xmlDesignation.LastPL		= StopPL;
			xmlDesignation.Highlighter	= (int)GetExtent().HighlighterId;
			xmlDesignation.Name			= Name;
			xmlDesignation.ScrollText   = ScrollText;
			
			if(this.MediaType == TmaxMediaTypes.Designation)
				xmlDesignation.Segment = GetExtent().XmlSegmentId.ToString();
			else
				xmlDesignation.Segment = this.Secondary.BarcodeId.ToString();
				
			if((this.Secondary != null) && (this.Secondary.Primary != null))
				xmlDesignation.PrimaryId = this.Secondary.Primary.MediaId;
			
			return true;
				
		}// public bool SetAttributes(CXmlDesignation xmlDesignation)
		
		/// <summary>This method tertiary properties using the XML attributes</summary>
		/// <param name="xmlDesignation">The source XML designation</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(CXmlDesignation xmlDesignation)
		{
			Start = xmlDesignation.Start;
			Stop = xmlDesignation.Stop;
			StartTuned = xmlDesignation.StartTuned;
			StopTuned = xmlDesignation.StopTuned;
			Name = xmlDesignation.Name;
			ScrollText = xmlDesignation.ScrollText;
			
			if(MediaType == TmaxMediaTypes.Designation)
			{
				StartPL = xmlDesignation.FirstPL;
				StopPL  = xmlDesignation.LastPL;
				MaxLineTime = xmlDesignation.GetMaxLineTime();
			}
			else
			{
				StartPL = 0;
				StopPL  = 0;
				MaxLineTime = -1.0;
			}
			
			return true;
				
		}// public bool SetProperties(CXmlDesignation xmlDesignation)
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="iId">The id of the property being set</param>
		/// <param name="strValue">The new property value</param>
		/// <param name="bConfirmed">True if new value has been confirmed</param>
		/// <param name="strMessage">Buffer in which to store a return message</param>
		/// <returns>true if successful</returns>
		public override bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		{
			switch(iId)
			{
				case DXTERTIARY_PROP_FOREIGN_BARCODE:
				
					if(strValue != null)
						SetForeignBarcode(strValue, true, true);
					else
						SetForeignBarcode("", true, false);
					
					return true;
					
				default:
				
					return base.SetProperty(iId, strValue, bConfirmed, strMessage);
					
			}// switch(iId)
			
		}// public override bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		
		/// <summary>This method is called to get the time required to play the record</summary>
		/// <returns>The total time in decimal seconds</returns>
		public override double GetDuration()
		{
			double dDuration = -1.0;
			
			//	What type of tertiary record?
			switch(m_eMediaType)
			{
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Designation:
				
					if(GetExtent() != null)
						dDuration = (GetExtent().Stop - GetExtent().Start);
					break;
					
				case TmaxMediaTypes.Treatment:
				default:
				
					break;
					
			}// switch(m_eMediaType)
			
			return dDuration;
		}
			
		/// <summary>Locates the link corresponding to the desired PL start position</summary>
		/// <param name="PL">The PL value to locate</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The link at the specified page/line</returns>
		public CDxQuaternary GetLink(long lPL, bool bExact)
		{
			CDxQuaternary	dxGreater = null;
			CDxQuaternary	dxLesser = null;
			
			try
			{
				//	Do we need to populate the child collection?
				if((m_dxQuaternaries.Count == 0) && (m_lChildCount > 0))
					Fill();
					
				//	Do we have any child links?
				if(m_dxQuaternaries.Count == 0)
					return null;
				
				foreach(CDxQuaternary O in m_dxQuaternaries)
				{
					//	Is this an exact match?
					if(O.StartPL == lPL)
					{
						return O;
					}
					else
					{
						//	Have we gone too far?
						if(O.StartPL > lPL)
						{
							dxGreater = O;
							break;
						}
						else
						{
							dxLesser = O;
						}
					
					}
					
				}// foreach(CDxQuaternary O in m_dxQuaternaries)

				//	Is the caller looking for an exact match?
				if(bExact == true) return null;
					
				//	Did we break out of the loop without finding a link 
				//	that appears after the specified position?
				if(dxGreater == null)
				{
					return dxLesser;
				}
				else
				{
					//	Are all links beyond the specified position?
					if(dxLesser == null)
					{
						return dxGreater;
					}
					else
					{
						//	Return the closer of the two links
						if((lPL - dxLesser.StartPL) <= (dxGreater.StartPL - lPL))
						{
							return dxLesser;
						}
						else
						{
							return dxGreater;
						}
						
					}// if(dxLesser == null)
				
				}// if(dxGreater == null)
				
			}
			catch
			{
				return null;
			}
			
		}// public CDxQuaternary GetLink(long lPL, bool bExact)

		/// <summary>Locates the link corresponding to the desired PL start position</summary>
		/// <param name="PL">The PL value to locate</param>
		/// <returns>The link at the specified page/line</returns>
		public CDxQuaternary GetLink(long lPL)
		{
			return GetLink(lPL, true);
		}
			
		/// <summary>This function is called to get the XML designation bound to this record</summary>
		/// <param name="bCreate">true to create if it does not exist</param>
		/// <param name="bLinks">true to load the collection of links</param>
		/// <param name="bTranscripts">true to load the collection of transcript lines</param>
		/// <returns>The XML designation</returns>
		public CXmlDesignation GetXmlDesignation(bool bCreate, bool bLinks, bool bTranscripts)
		{
			CXmlDesignation xmlDesignation = null;
			string			strFileSpec = "";
			
			//	Must be a clip or designation
			if((this.MediaType == TmaxMediaTypes.Designation) || (this.MediaType == TmaxMediaTypes.Clip))
			{
				//	Get the file path from the database
				strFileSpec = this.GetFileSpec();
				if((strFileSpec == null) || (strFileSpec.Length == 0)) return null;
				
				//	Does the file exist?
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					if((bCreate == false) || (this.Database.CreateXmlDesignation(this, strFileSpec) == false))
						return null;
				}
				
				try
				{
					xmlDesignation = new CXmlDesignation();
					xmlDesignation.FastFill(strFileSpec, bLinks, bTranscripts);
				}
				catch
				{
				}
			
			}// if((this.MediaType == TmaxMediaTypes.Designation) || (this.MediaType == TmaxMediaTypes.Clip))
			
			return xmlDesignation;
		
		}// public CXmlDesignation GetXmlDesignation(bool bCreate)
		
		/// <summary>This function is called to get the XML designation bound to this record</summary>
		/// <param name="bCreate">true to create if it does not exist</param>
		/// <returns>The XML designation</returns>
		public CXmlDesignation GetXmlDesignation(bool bCreate)
		{
			return GetXmlDesignation(bCreate, true, true);
		}
		
		/// <summary>This function is called to get the XML designation bound to this record</summary>
		/// <returns>The XML designation</returns>
		public CXmlDesignation GetXmlDesignation()
		{
			return GetXmlDesignation(true, true, true);
		}
		
		/// <summary>This method is called to determine if this record is a video designation</summary>
		/// <returns>True if this record is a video designation</returns>
		public bool GetIsVideoDesignation()
		{
			//	Is this a designation?
			if((this.MediaType == TmaxMediaTypes.Designation) && (this.Secondary != null))
			{
				//	Is the primary record a deposition?
				if(this.Secondary.Primary != null)
					return this.Secondary.Primary.MediaType == TmaxMediaTypes.Deposition;
			}
			return false;

		}// public bool GetIsVideoDesignation()

		/// <summary>This method retrieves the record's split screen attribute</summary>
		///	<returns>True if split screen</returns>
		public override bool GetSplitScreen()
		{
			return this.SplitScreen;
		}

		#endregion Public Methods

		#region Protected Methods
		
		/// <summary>This method creates the Extents table exchange object</summary>
		/// <returns>true if successful</returns>
		protected virtual bool CreateExtents()
		{
			//	Have we already created the collection?
			if(m_dxExtents != null) return true;
			
			try
			{
				//	Allocate the interface to the transcripts table
				m_dxExtents = new CDxExtents();
				m_dxExtents.Secondary = this.Secondary;
				m_dxExtents.Tertiary = this;
				
				if(m_dxCollection != null)
				{
                    m_dxExtents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				}
			
				return m_dxExtents.Open();
			}
			catch
			{
				m_dxExtents = null;
				return false;
			}

		}
				
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Performs one-time initialization of the object</summary>
		/// <param name="dxSecondary">The parent secondary record</param>
		private void Initialize(CDxSecondary dxSecondary)
		{
			m_dxQuaternaries.Tertiary = this;
			m_dxSecondary = dxSecondary;

			this.ScrollText = true; // Set default to match XML default
			
			//	Create the codes collection
			//m_dxCodes = new CDxCodes(this.Database);
			//m_dxCodes.Owner = this;
		
		}// private void Initialize(CDxPrimary dxPrimary)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The collection of tertiary media objects</summary>
		public CDxQuaternaries Quaternaries
		{
			get { return m_dxQuaternaries; }
		}
		
		/// <summary>The id of the secondary media parent</summary>
		public long SecondaryMediaId
		{
			get { return m_lSecondaryMediaId; }
			set { m_lSecondaryMediaId = value; }
		}
		
		/// <summary>The primary object that owns this record</summary>
		public CDxSecondary Secondary
		{
			get { return m_dxSecondary; }
			set { m_dxSecondary = value; }
		}
		
		
		/// <summary>The name of the file associated with this object</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
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

		/// <summary>The record exchange object for the sibling treatment</summary>
		public CDxTertiary Sibling
		{
			get { return GetSibling(false); }
			set { m_dxSibling = value; }
		}

		/// <summary>The PST identifier for the sibling treatment</summary>
		public string SiblingId
		{
			get { return m_strSiblingId; }
			set { m_strSiblingId = value; }
		}

		/// <summary>True if this record is the source for more than one script scene</summary>
		public bool HasShortcuts
		{
			get 
			{ 
				//	NOTE:	We reverse the bit logic for this attribute because earlier
				//			versions of the database (prior to this attribute) assumed that the
				//			designation was a shortcut - therefore we want zero to mean shortcut == true
				return ((m_lAttributes & (long)TmaxTertiaryAttributes.HasShortcuts) == 0); 
			}
			set 
			{ 
				//	See NOTE above for explanation of inverted logic	
				if(value == false)
					m_lAttributes |= (long)TmaxTertiaryAttributes.HasShortcuts;
				else
					m_lAttributes &= ~((long)TmaxTertiaryAttributes.HasShortcuts);
			}
		
		}// public bool HasShortcuts
		
		/// <summary>Flag to indicate that designation text should be scrolled during playback</summary>
		public bool ScrollText
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxTertiaryAttributes.ScrollText) != 0); 
			}
			set 
			{ 
				if(value == true)
					m_lAttributes |= (long)TmaxTertiaryAttributes.ScrollText;
				else
					m_lAttributes &= ~((long)TmaxTertiaryAttributes.ScrollText);
			}
		
		}// public bool ScrollText

		/// <summary>Flag to indicate that record identifies a split screen treatment</summary>
		public bool SplitScreen
		{
			get
			{
				return ((m_lAttributes & (long)TmaxTertiaryAttributes.SplitScreen) != 0);
			}
			set
			{
				if(value == true)
					m_lAttributes |= (long)TmaxTertiaryAttributes.SplitScreen;
				else
					m_lAttributes &= ~((long)TmaxTertiaryAttributes.SplitScreen);
			}

		}// public bool SplitScreen

		/// <summary>Flag to indicate that record identifies that split screen treatment is horizontal instead of vertical</summary>
		public bool SplitHorizontal
		{
			get
			{
				if(this.SplitScreen == true)
					return ((m_lAttributes & (long)TmaxTertiaryAttributes.SplitHorizontal) != 0);
				else
					return false;
			}
			set
			{
				if(value == true)
					m_lAttributes |= (long)TmaxTertiaryAttributes.SplitHorizontal;
				else
					m_lAttributes &= ~((long)TmaxTertiaryAttributes.SplitHorizontal);
			}

		}// public bool SplitHorizontal

		/// <summary>Flag to indicate that split screen treatment should be loaded in the right/bottom pane</summary>
		public bool SplitRight
		{
			get
			{
				if(this.SplitScreen == true)
					return ((m_lAttributes & (long)TmaxTertiaryAttributes.SplitRight) != 0);
				else
					return false;
			}
			set
			{
				if(value == true)
					m_lAttributes |= (long)TmaxTertiaryAttributes.SplitRight;
				else
					m_lAttributes &= ~((long)TmaxTertiaryAttributes.SplitRight);
			}

		}// public bool SplitRight

		/// <summary>The interface to the Extents table</summary>
		public CDxExtents Extents
		{
			get { return m_dxExtents; }
		}
		
		/// <summary>The transcript record associated with this primary if available</summary>
		public CDxExtent Extent
		{
			get { return GetExtent(); }
		}
		
		/// <summary>The start time</summary>
		public double Start
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().Start;
				else
					return -1;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().Start = value;	
			}
			
		}// public double Start
		
		/// <summary>The start PL value</summary>
		public long StartPL
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().StartPL;
				else
					return -1;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().StartPL = value;	
			}
			
		}// public long StartPL
		
		/// <summary>The start tuned flag</summary>
		public bool StartTuned
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().StartTuned;
				else
					return false;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().StartTuned = value;	
			}
			
		}// public bool StartTuned
		
		/// <summary>The stop time</summary>
		public double Stop
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().Stop;
				else
					return -1;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().Stop = value;	
			}
			
		}// public double Stop
		
		/// <summary>The stop PL value</summary>
		public long StopPL
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().StopPL;
				else
					return -1;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().StopPL = value;	
			}
			
		}// public long StopPL
		
		/// <summary>The stop tuned flag</summary>
		public bool StopTuned
		{
			get
			{
				if(GetExtent() != null)
					return GetExtent().StopTuned;
				else
					return false;
			}
			set 
			{ 
				if(GetExtent() != null)
					GetExtent().StopTuned = value;	
			}
			
		}// public bool StopTuned
		
		/// <summary>The time required to play the longest line</summary>
		public double MaxLineTime
		{
			get
			{
				if(GetExtent() != null)
				{
					if((this.Database != null) && (this.Database.MaxLineTimeIndex >= 0))
						return GetExtent().MaxLineTime;
					else
						return -1.0;
				}
				else
				{
					return -1.0;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					if((this.Database != null) && (this.Database.MaxLineTimeIndex >= 0))
						GetExtent().MaxLineTime = value;
				}
				
			}
			
		}// public double MaxLineTime
		
		/// <summary>true if this record is a deposition video designation</summary>
		public bool IsVideoDesignation
		{
			get { return GetIsVideoDesignation(); }
		}
		
		#endregion Properties
	
	}// class CDxTertiary

	/// <summary>
	/// This class is used to manage a ArrayList of CDxTertiary objects
	/// </summary>
	public class CDxTertiaries : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			SecondaryMediaId,
			BarcodeId,
			Children,
			Attributes,
			MediaType,
			Filename,
			SourceId,
			SourceType,
			Description,
			Name,
			DisplayOrder,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
			SiblingId,	//	Added in version 6.3.4
		}

		public const string TABLE_NAME = "TertiaryMedia";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Secondary property</summary>
		private CDxSecondary m_dxSecondary = null;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxTertiaries() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxTertiaries(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This function is called to set the database interface</summary>
		public override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			//	Do the base class processing first
			base.SetDatabase(tmaxDatabase);

			if(this.Database != null)
			{
				if((m_dxFields != null) && ((int)eFields.SiblingId < m_dxFields.Count))
					m_dxFields[(int)(eFields.SiblingId)].Index = this.Database.SiblingIdIndex;
			}

		}// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxTertiary">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxTertiary Add(CDxTertiary dxTertiary)
		{
			return (CDxTertiary)base.Add(dxTertiary);
			
		}// Add(CDxTertiary dxTertiary)

		/// <summary>This method will delete the specified record</summary>
		/// <param name="dxRecord">Object to be deleted</param>
		/// <returns>true if successful</returns>
		public override bool Delete(CBaseRecord dxRecord)
		{
			CDxExtent dxExtent = null;
			
			if(base.Delete(dxRecord) == false)
				return false;
				
			//	Is this a clip or designation?
			switch(((CDxTertiary)dxRecord).MediaType)
			{
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Designation:
				
					dxExtent = ((CDxTertiary)dxRecord).GetExtent();
				
					/// Delete the extent record if it exists
					if((dxExtent != null) && (dxExtent.Collection != null))
					{
						dxExtent.Collection.Delete(dxExtent);
					}
					break;
			
			}// switch(((CDxTertiary)dxRecord).MediaType)
			
			return true;
			
		}// public override bool Delete(CBaseRecord dxRecord)
		
		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			CDxExtents dxExtents = null;
			
			if(base.Update(dxRecord) == false)
				return false;
				
			switch(((CDxTertiary)dxRecord).MediaType)
			{
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Designation:
				
					dxExtents = ((CDxTertiary)dxRecord).Extents;
				
					/// Update the extent if it exists
					if((dxExtents != null) && (dxExtents.Count > 0))
					{
						dxExtents.Update(dxExtents[0]);
					}
					break;
					
				default:
				
					break;
					
			}// switch(((CDxTertiary)dxRecord).MediaType)
			
			return true;
			
		}// public override bool Update(CBaseRecord dxRecord)
		
		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxTertiaries Dispose()
		{
			return (CDxTertiaries)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxTertiary Find(long lAutoId, bool bBarcode)
		{
			return (CDxTertiary)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxTertiary Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxTertiary this[int iIndex]
		{
			get
			{
				return (CDxTertiary)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxTertiary GetAt(int iIndex)
		{
			return (CDxTertiary)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxTertiary	dxTertiary = (CDxTertiary)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.SecondaryMediaId.ToString() + ",");
			strSQL += (eFields.BarcodeId.ToString() + ",");
			strSQL += (eFields.Children.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.MediaType.ToString() + ",");
			strSQL += (eFields.Filename.ToString() + ",");
			strSQL += (eFields.SourceId.ToString() + ",");
			strSQL += (eFields.SourceType.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.DisplayOrder.ToString() + ",");

			if(this.GetSiblingIdEnabled() == true)
				strSQL += (eFields.SiblingId.ToString() + ",");

			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + dxTertiary.SecondaryMediaId.ToString() + "',");
			strSQL += ("'" + dxTertiary.BarcodeId.ToString() + "',");
			strSQL += ("'" + dxTertiary.ChildCount.ToString() + "',");
			strSQL += ("'" + dxTertiary.Attributes.ToString() + "',");
			strSQL += ("'" + ((int)dxTertiary.MediaType).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxTertiary.Filename) + "',");
			strSQL += ("'" + SQLEncode(dxTertiary.SourceId) + "',");
			strSQL += ("'" + ((int)dxTertiary.SourceType).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxTertiary.Description) + "',");
			strSQL += ("'" + SQLEncode(dxTertiary.Name) + "',");
			strSQL += ("'" + dxTertiary.DisplayOrder.ToString() + "',");

			if(this.GetSiblingIdEnabled() == true)
				strSQL += ("'" + SQLEncode(dxTertiary.SiblingId) + "',");

			strSQL += ("'" + dxTertiary.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxTertiary.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxTertiary.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxTertiary.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxTertiary	dxTertiary = (CDxTertiary)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.SecondaryMediaId.ToString() + " = '" + dxTertiary.SecondaryMediaId.ToString() + "',");
			strSQL += (eFields.BarcodeId.ToString() + " = '" + dxTertiary.BarcodeId.ToString() + "',");
			strSQL += (eFields.Children.ToString() + " = '" + dxTertiary.ChildCount.ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxTertiary.Attributes.ToString() + "',");
			strSQL += (eFields.MediaType.ToString() + " = '" + ((int)dxTertiary.MediaType).ToString() + "',");
			strSQL += (eFields.Filename.ToString() + " = '" + SQLEncode(dxTertiary.Filename) + "',");
			strSQL += (eFields.SourceId.ToString() + " = '" + SQLEncode(dxTertiary.SourceId) + "',");
			strSQL += (eFields.SourceType.ToString() + " = '" + ((int)dxTertiary.SourceType).ToString() + "',");
			strSQL += (eFields.Description.ToString() + " = '" + SQLEncode(dxTertiary.Description) + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxTertiary.Name) + "',");
			strSQL += (eFields.DisplayOrder.ToString() + " = '" + dxTertiary.DisplayOrder.ToString() + "',");

			if(this.GetSiblingIdEnabled() == true)
				strSQL += (eFields.SiblingId.ToString() + " = '" + SQLEncode(dxTertiary.SiblingId) + "',");

			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxTertiary.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxTertiary.CreatedOn.ToString() + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxTertiary.ModifiedBy.ToString() + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxTertiary.ModifiedOn.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxTertiary.AutoId.ToString();
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
			strSQL += " WHERE SecondaryMediaId = ";
			strSQL += m_dxSecondary.AutoId.ToString();
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
			
			if(m_dxSecondary != null)
			{
				strSQL += " WHERE SecondaryMediaId = ";
				strSQL += m_dxSecondary.AutoId.ToString();
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

		/// <summary>Called to determine if the SiblingId column is available</summary>
		/// <returns>True if available</returns>
		public bool GetSiblingIdEnabled()
		{
			if((m_dxFields != null) && ((int)(eFields.SiblingId) < m_dxFields.Count))
				return (m_dxFields[(int)eFields.SiblingId].Index >= 0);
			else
				return false;
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
			CDxTertiary dxTertiary = new CDxTertiary();
			
			if(dxTertiary != null)
			{
				dxTertiary.Collection = this;
				dxTertiary.Secondary = m_dxSecondary;
				
				if(m_dxSecondary != null)
					dxTertiary.SecondaryMediaId = m_dxSecondary.AutoId;
					
			}
			return ((CBaseRecord)dxTertiary);
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxTertiary dxTertiary = (CDxTertiary)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxTertiary.AutoId;
					m_dxFields[(int)eFields.SecondaryMediaId].Value  = dxTertiary.SecondaryMediaId;
					m_dxFields[(int)eFields.BarcodeId].Value  = dxTertiary.BarcodeId;
					m_dxFields[(int)eFields.Children].Value = dxTertiary.ChildCount;
					m_dxFields[(int)eFields.Attributes].Value = dxTertiary.Attributes;
					m_dxFields[(int)eFields.MediaType].Value = dxTertiary.MediaType;
					m_dxFields[(int)eFields.Filename].Value = dxTertiary.Filename;
					m_dxFields[(int)eFields.SourceId].Value = dxTertiary.SourceId;
					m_dxFields[(int)eFields.SourceType].Value = dxTertiary.SourceType;
					m_dxFields[(int)eFields.Description].Value = dxTertiary.Description;
					m_dxFields[(int)eFields.Name].Value = dxTertiary.Name;
					m_dxFields[(int)eFields.DisplayOrder].Value = dxTertiary.DisplayOrder;
					m_dxFields[(int)eFields.CreatedBy].Value = dxTertiary.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxTertiary.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxTertiary.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxTertiary.ModifiedOn;

					if(this.GetSiblingIdEnabled() == true)
						m_dxFields[(int)eFields.SiblingId].Value = dxTertiary.SiblingId;
				}
				else
				{
					dxTertiary.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxTertiary.SecondaryMediaId = (int)(m_dxFields[(int)eFields.SecondaryMediaId].Value);
					dxTertiary.BarcodeId = (int)(m_dxFields[(int)eFields.BarcodeId].Value);
					dxTertiary.ChildCount = (int)(m_dxFields[(int)eFields.Children].Value);
					dxTertiary.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxTertiary.MediaType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)(m_dxFields[(int)eFields.MediaType].Value));
					dxTertiary.Filename = (string)(m_dxFields[(int)eFields.Filename].Value);
					dxTertiary.SourceId = (string)(m_dxFields[(int)eFields.SourceId].Value);
					dxTertiary.SourceType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)m_dxFields[(int)eFields.SourceType].Value);
					dxTertiary.Description = (string)(m_dxFields[(int)eFields.Description].Value);
					dxTertiary.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxTertiary.DisplayOrder = (int)(m_dxFields[(int)eFields.DisplayOrder].Value);
					dxTertiary.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxTertiary.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxTertiary.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxTertiary.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);

					if(this.GetSiblingIdEnabled() == true)
					{
						//	This allows for NULL fields that may occur if the column was added with existing records
						try { dxTertiary.SiblingId = (string)(m_dxFields[(int)eFields.SiblingId].Value); }
						catch { dxTertiary.SiblingId = ""; }
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
			
		}// Exchange()


		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();

			//	The MaxLineTime column may or may not exist in the database
			if(this.Database != null)
			{
				if((m_dxFields != null) && ((int)eFields.SiblingId < m_dxFields.Count))
					m_dxFields[(int)(eFields.SiblingId)].Index = this.Database.SiblingIdIndex;
			}

		}// protected override void SetFields()

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
				case eFields.SecondaryMediaId:
				case eFields.BarcodeId:
				case eFields.Children:
				case eFields.Attributes:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				case eFields.DisplayOrder:
				
					dxField.Value = 0;
					break;
					
				case eFields.MediaType:
				case eFields.SourceType:
				
					dxField.Value = TmaxMediaTypes.Unknown;
					break;
					
				case eFields.Name:
				case eFields.SourceId:
				case eFields.Description:
				case eFields.Filename:
				case eFields.SiblingId:
				
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
		
		/// <summary>This is the primary media object that owns the secondary collection</summary>
		public CDxSecondary Secondary
		{
			get { return m_dxSecondary; }
			set { m_dxSecondary = value; }
		}

		#endregion Properties
		
		
	}//	CDxTertiaries
		
}// namespace FTI.Trialmax.Database
