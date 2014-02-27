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
	public class CDxSecondary : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXSECONDARY_PROP_FIRST_ID			= DX_MEDIA_RECORD_PROP_LAST + 2000;

		protected const int DXSECONDARY_PROP_BARCODE			= DXSECONDARY_PROP_FIRST_ID + 1;
		protected const int DXSECONDARY_PROP_MEDIA_TYPE			= DXSECONDARY_PROP_FIRST_ID + 2;
		protected const int DXSECONDARY_PROP_MEDIA_PATH			= DXSECONDARY_PROP_FIRST_ID + 3;
		protected const int DXSECONDARY_PROP_FILENAME			= DXSECONDARY_PROP_FIRST_ID + 4;
		protected const int DXSECONDARY_PROP_DISPLAY_ORDER		= DXSECONDARY_PROP_FIRST_ID + 5;
		protected const int DXSECONDARY_PROP_CHILDREN			= DXSECONDARY_PROP_FIRST_ID + 6;
		protected const int DXSECONDARY_PROP_BARCODE_ID			= DXSECONDARY_PROP_FIRST_ID + 7;
		protected const int DXSECONDARY_PROP_ROOT_PATH			= DXSECONDARY_PROP_FIRST_ID + 8;
		protected const int DXSECONDARY_PROP_REGISTER_PATH		= DXSECONDARY_PROP_FIRST_ID + 9;
		protected const int DXSECONDARY_PROP_MULTIPAGE_ID		= DXSECONDARY_PROP_FIRST_ID + 10;
		protected const int DXSECONDARY_PROP_HIGH_RESOLUTION	= DXSECONDARY_PROP_FIRST_ID + 11;
		protected const int DXSECONDARY_PROP_HIDDEN				= DXSECONDARY_PROP_FIRST_ID + 12;
		protected const int DXSECONDARY_PROP_AUTO_TRANSITION	= DXSECONDARY_PROP_FIRST_ID + 13;
		protected const int DXSECONDARY_PROP_TRANSITION_TIME	= DXSECONDARY_PROP_FIRST_ID + 14;
		protected const int DXSECONDARY_PROP_RELATIVE_PATH		= DXSECONDARY_PROP_FIRST_ID + 15;
		protected const int DXSECONDARY_PROP_ALIAS_ID			= DXSECONDARY_PROP_FIRST_ID + 16;
		protected const int DXSECONDARY_PROP_SOURCE_ID			= DXSECONDARY_PROP_FIRST_ID + 17;
		protected const int DXSECONDARY_PROP_SOURCE_TYPE		= DXSECONDARY_PROP_FIRST_ID + 18;
		protected const int DXSECONDARY_PROP_SOURCE_BARCODE		= DXSECONDARY_PROP_FIRST_ID + 19;
		protected const int DXSECONDARY_PROP_SOURCE_FILENAME	= DXSECONDARY_PROP_FIRST_ID + 20;
		protected const int DXSECONDARY_PROP_SOURCE_PATH		= DXSECONDARY_PROP_FIRST_ID + 21;
		protected const int DXSECONDARY_PROP_EXPORT_IMAGE		= DXSECONDARY_PROP_FIRST_ID + 22;
		protected const int DXSECONDARY_PROP_DURATION			= DXSECONDARY_PROP_FIRST_ID + 23;
		protected const int DXSECONDARY_PROP_SOURCE_DURATION	= DXSECONDARY_PROP_FIRST_ID + 24;
		protected const int DXSECONDARY_PROP_FOREIGN_BARCODE	= DXSECONDARY_PROP_FIRST_ID + 25;
		protected const int DXSECONDARY_PROP_MAPPED				= DXSECONDARY_PROP_FIRST_ID + 26;
		protected const int DXSECONDARY_PROP_SOURCE_SHORTCUTS   = DXSECONDARY_PROP_FIRST_ID + 27;
		
		#endregion Constants
	
		#region Private Members
		
		/// <summary>Local member bound to Tertiaries property</summary>
		private CDxTertiaries m_dxTertiaries = new CDxTertiaries();
		
		/// <summary>Local member bound to Primary property</summary>
		protected CDxPrimary m_dxPrimary = null;
		
		/// <summary>Local member bound to Extents property</summary>
		private CDxExtents m_dxExtents = null;
		
		/// <summary>Local member bound to Scene property</summary>
		protected CDxMediaRecord m_dxSource = null;
		
		/// <summary>Local member bound to SourceType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_eSourceType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to PrimaryMediaId property</summary>
		protected long m_lPrimaryMediaId = 0;
		
		/// <summary>Local member bound to MultipageId property</summary>
		protected long m_lMultipageId = -1;
		
		/// <summary>Local member bound to AliasId property</summary>
		private long m_lAliasId = 0;
		
		/// <summary>Local member bound to LinkedPath property</summary>
		protected string m_strRelativePath = "";
		
		/// <summary>Local member bound to Filename property</summary>
		protected string m_strFilename = "";
		
		/// <summary>Local member bound to Barcode property</summary>
		protected string m_strSourceId = "";
		
		/// <summary>Local member bound to TransitionTime property</summary>
		protected short m_sTransitionTime = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxSecondary() : base()
		{
			Initialize(null);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="dxPrimary">The parent Primary object</param>
		public CDxSecondary(CDxPrimary dxPrimary) : base()
		{
			Initialize(dxPrimary);
		}
		
		/// <summary>This method is called to fill the secondaries collection</summary>
		/// <returns>true if successful</returns>
		override public bool Fill()
		{
			bool bSuccessful = false;
			
			if(m_dxTertiaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(Collection != null)
					m_dxTertiaries.Database = Collection.Database;
					
				//	Clear the existing objects
				m_dxTertiaries.Clear();
				
				if((bSuccessful = m_dxTertiaries.Fill()) == true)
				{
					//	We do this check just to be safe and make sure
					//	the child count has not somehow gotten out of
					//	sync
					if(this.ChildCount != m_dxTertiaries.Count)
					{
						//Debug.Assert(this.ChildCount == m_dxTertiaries.Count, "Secondary AutoId: " + this.AutoId.ToString() + "-> ChildCount: " + this.ChildCount.ToString() + " != Tertiaries: " + m_dxTertiaries.Count.ToString() );
						
						if(Collection != null)
						{
							this.ChildCount = m_dxTertiaries.Count;
							Collection.Update(this);
						}
					
					}// if(this.ChildCount != m_dxTertiaries.Count)
					
				}// if((bSuccessful = m_dxTertiaries.Fill()) == true)
			
			}// if((bSuccessful = m_dxTertiaries.Fill()) == true)
			
			return bSuccessful;
			
		}// public bool Fill()
		
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

		/// <summary>This method is called to add a tertiary media object to the local collection</summary>
		/// <param name="dxTertiary">Tertiary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxTertiary dxTertiary)
		{
			if(m_dxTertiaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxTertiaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct secondary id
				dxTertiary.Secondary = this;
				dxTertiary.SecondaryMediaId = AutoId;
				
				return (m_dxTertiaries.Add(dxTertiary) != null);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to get the record exchange object for the scene's source media</summary>
		///	<returns>The interface to the source record</returns>
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
			
		}// public CDxMediaRecord GetScene()
		
		/// <summary>This method is called to update a tertiary media object</summary>
		/// <param name="dxTertiary">Tertiary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Update(CDxTertiary dxTertiary)
		{
			if(m_dxTertiaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxTertiaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct primary id
				dxTertiary.SecondaryMediaId = AutoId;
				
				return m_dxTertiaries.Update(dxTertiary);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to delete a tertiary media object</summary>
		/// <param name="dxTertiary">Tertiary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Delete(CDxTertiary dxTertiary)
		{
			int iIndex = -1;
			
			Debug.Assert(m_dxTertiaries != null);
			if(m_dxTertiaries == null) return false;
			
			//	Make sure this secondary exists in the collection
			if((iIndex = m_dxTertiaries.IndexOf(dxTertiary)) < 0) return false;
			
			//	Make sure the tertiary collection is using the correct database
			if(Collection != null)
			{
				m_dxTertiaries.Database = Collection.Database;
			}
			
			if(m_dxTertiaries.Delete(dxTertiary) == true)
			{
				//	Update the display order of remaining records
				for(int i = iIndex; i < m_dxTertiaries.Count; i++)
				{
					m_dxTertiaries[i].DisplayOrder = (i + 1);
					m_dxTertiaries.Update(m_dxTertiaries[i]);
				}
					
				//	Update this record
				if(Collection != null)
				{
					this.ChildCount = m_dxTertiaries.Count;
					Collection.Update(this);
				}
					
				return true;
				
			}
			
			//	Must have been an error
			return false;
		
		}// Delete(CDxTertiary dxTertiary)
		
		/// <summary>This function is called to delete the specified child record</summary>
		public override bool Delete(CDxMediaRecord dxChild)
		{
			return Delete((CDxTertiary)dxChild);
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
			return FTI.Shared.Trialmax.TmaxMediaLevels.Secondary;
		}
		
		/// <summary>This function is called to get the collection of child records</summary>
		/// <returns>The child collection if it exists</returns>
		public override CDxMediaRecords GetChildCollection()
		{
			//	Is this a script scene
			if((MediaType == TmaxMediaTypes.Scene) && (GetSource() != null))
			{
				//	Is the source a designation or clip?
				if((GetSource().MediaType == TmaxMediaTypes.Designation) || 
					(GetSource().MediaType == TmaxMediaTypes.Clip))
				{
					//	Return the source collection
					return GetSource().GetChildCollection();
				}
				
			}
			
			//	Return the local collection
			return m_dxTertiaries;
		}
		
		/// <summary>This function is called to get the total number of children</summary>
		/// <returns>The total number of children</returns>
		public override long GetChildCount()
		{
			//	Is this a script scene
			if((MediaType == TmaxMediaTypes.Scene) && (GetSource() != null))
			{
				//	Is the source a designation or clip?
				if((GetSource().MediaType == TmaxMediaTypes.Designation) || 
				   (GetSource().MediaType == TmaxMediaTypes.Clip))
				{
					//	Return the source count
					return GetSource().GetChildCount();
				}
				
			}
			
			//	Let the base class handle it
			return base.GetChildCount();
		}
		
		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public override CDxMediaRecord GetParent()
		{
			return m_dxPrimary;
		}
		
		/// <summary>This function is called to get the alias identifier assigned to the record</summary>
		/// <returns>The alias identifier</returns>
		public override long GetAliasId()
		{
			return AliasId;
		}
		
		/// <summary>This function is called to get the record's Admitted state</summary>
		/// <returns>True if the record has been Admitted</returns>
		public override bool GetAdmitted()
		{
			//	Assume the secondary is admitted if the parent is admitted
			if(m_dxPrimary != null)
				return m_dxPrimary.GetAdmitted();
			else
				return false;

		}// public override bool GetAdmitted()
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		/// <param name="tmaxProperties">The collection in which to place the property descriptors</param>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	Add the base class properties first
			base.GetProperties(tmaxProperties);
			
			//	What type of secondary record is this
			switch(m_eMediaType)
			{
				case TmaxMediaTypes.Page:
				
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_PATH, "Media Path", GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_FILENAME, "Filename", GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropGridEditors.Memo);
					tmaxProperties.Add(DXSECONDARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

					tmaxProperties.Add(DXSECONDARY_PROP_HIGH_RESOLUTION, "High Res", HighResolution, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
					tmaxProperties.Add(DXSECONDARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE_ID, "Barcode Id", m_lBarcodeId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					//tmaxProperties.Add(DXSECONDARY_PROP_ROOT_PATH, "Case Path", this.Database.GetFolderSpec(Primary, true), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_REGISTER_PATH, "Registered From", Primary.RegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					
					break;
				
				case TmaxMediaTypes.Slide:
				
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_PATH, "Media Path", Primary.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_FILENAME, "Filename", Primary.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropertyEditors.Memo);
					tmaxProperties.Add(DXSECONDARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

					tmaxProperties.Add(DXSECONDARY_PROP_MULTIPAGE_ID, "Slide Id", m_lMultipageId, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE_ID, "Barcode Id", m_lBarcodeId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_EXPORT_IMAGE, "Export Image", GetFileName(), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					//tmaxProperties.Add(DXSECONDARY_PROP_ROOT_PATH, "Case Path", this.Database.GetFolderSpec(Primary, true), TmaxPropertyCategories.Database, TmaxPropertyEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_REGISTER_PATH, "Registered From", Primary.RegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					
					break;
				
				case TmaxMediaTypes.Segment:
				
					//	Are we dealing with a recording segment?
					if(Primary.MediaType == TmaxMediaTypes.Recording)
					{
						tmaxProperties.Add(DXSECONDARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
						tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
						tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_PATH, "Media Path", GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_FILENAME, "Filename", GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropertyEditors.Memo);
						tmaxProperties.Add(DXSECONDARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

						tmaxProperties.Add(DXSECONDARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_BARCODE_ID, "Barcode Id", m_lBarcodeId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
						//tmaxProperties.Add(DXSECONDARY_PROP_ROOT_PATH, "Case Path", this.Database.GetFolderSpec(Primary, true), TmaxPropertyCategories.Database, TmaxPropertyEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_REGISTER_PATH, "Registered From", Primary.RegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

						//	Add the video extents properties
						if(GetExtent() != null)
							GetExtent().GetProperties(tmaxProperties, true, false, true, false);
			
						tmaxProperties.Add(DXSECONDARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					}
					else
					{
						tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_PATH, "Media Path", GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
						tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
						//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropertyEditors.Memo);

//						if(this.m_lAliasId > 0)
//							tmaxProperties.Add(DXSECONDARY_PROP_RELATIVE_PATH, "Relative Path", this.Database.GetAliasedPath(this), TmaxPropertyCategories.Media, TmaxPropertyEditors.Linked);
//						else
//							tmaxProperties.Add(DXSECONDARY_PROP_RELATIVE_PATH, "Relative Path", RelativePath, TmaxPropertyCategories.Media, TmaxPropertyEditors.Linked);

						tmaxProperties.Add(DXSECONDARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
//						tmaxProperties.Add(DXSECONDARY_PROP_ALIAS_ID, "Alias Id", m_lAliasId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
//						tmaxProperties.Add(DXSECONDARY_PROP_ROOT_PATH, "Case Path", this.Database.GetFolderSpec(Primary, true), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
						tmaxProperties.Add(DXSECONDARY_PROP_REGISTER_PATH, "Registered From", Primary.RegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

						//	Add the video extents properties
						if(GetExtent() != null)
							GetExtent().GetProperties(tmaxProperties, true, true, true, true);
			
						tmaxProperties.Add(DXSECONDARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					}
					
					break;
					
				case TmaxMediaTypes.Scene:
				
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DXSECONDARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_HIDDEN, "Hidden", Hidden, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_AUTO_TRANSITION, "Auto Transition", AutoTransition, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_TRANSITION_TIME, "Transition Time", TransitionTime, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
					tmaxProperties.Add(DXSECONDARY_PROP_BARCODE_ID, "Barcode Id", m_lBarcodeId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

					//	Add properties for the source record
					GetSourceProperties(tmaxProperties);
					
					break;
					
				default:
				
					break;
				
			}// switch(m_eMediaType)
			
		}// public override void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This function is called to add properties for the source record to the specified collection</summary>
		/// <param name="tmaxProperties">The collection in which to place the property descriptors</param>
		public void GetSourceProperties(CTmaxProperties tmaxProperties)
		{
			CDxMediaRecord dxSource = null;
			
			tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_ID, "Source PSTQ", SourceId, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_TYPE, "Source Type", SourceType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

			//	Get the source record
			if((dxSource = GetSource()) == null) return;
			
			//	What type of media is the source?
			switch(dxSource.MediaType)
			{
				case TmaxMediaTypes.Page:
				case TmaxMediaTypes.Slide:
				case TmaxMediaTypes.Treatment:
							
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_BARCODE, "Source Barcode", dxSource.GetBarcode(false), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_FILENAME, "Source Filename", dxSource.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_PATH, "Source Path", dxSource.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					break;
					
				case TmaxMediaTypes.Segment:
							
					//	NOTE:	Scenes can only reference recording segments
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_BARCODE, "Source Barcode", dxSource.GetBarcode(false), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_FILENAME, "Source Filename", dxSource.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_PATH, "Source Path", dxSource.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_DURATION, "Source Duration", CTmaxToolbox.SecondsToString(dxSource.GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
								
					//	Get the playback extents
					if(((CDxSecondary)dxSource).GetExtent() != null)
					{
						((CDxSecondary)dxSource).GetExtent().GetProperties(tmaxProperties, true, false, true, false);
					}
					break;
								
				case TmaxMediaTypes.Designation:
				case TmaxMediaTypes.Clip:
							
					//	NO barcodes for designations and clips
								
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_FILENAME, "Source Filename", dxSource.GetFileName(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_PATH, "Source Path", dxSource.GetFileSpec(), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_DURATION, "Source Duration", CTmaxToolbox.SecondsToString(dxSource.GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
								
					tmaxProperties.Add(DXSECONDARY_PROP_SOURCE_SHORTCUTS, "Source HasShortcuts", ((CDxTertiary)dxSource).HasShortcuts, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

					//	Get the playback extents
					if(((CDxTertiary)dxSource).GetExtent() != null)
					{
						if(dxSource.MediaType == TmaxMediaTypes.Clip)
							((CDxTertiary)dxSource).GetExtent().GetProperties(tmaxProperties, true, false, true, false);
						else
							((CDxTertiary)dxSource).GetExtent().GetProperties(tmaxProperties, true, true, true, true);
					}
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
				case DXSECONDARY_PROP_BARCODE:
				
					tmaxProperty.Value = this.Database.GetBarcode(this, true);
					break;
					
				case DXSECONDARY_PROP_MEDIA_PATH:
				
					tmaxProperty.Value = GetFileSpec();
					break;
					
				case DXSECONDARY_PROP_FILENAME:
				
					tmaxProperty.Value = GetFileName();
					break;
					
				case DXSECONDARY_PROP_DISPLAY_ORDER:
				
					tmaxProperty.Value = m_lDisplayOrder;
					break;
					
				case DXSECONDARY_PROP_MULTIPAGE_ID:
				
					tmaxProperty.Value = m_lMultipageId;
					break;
					
				case DXSECONDARY_PROP_ROOT_PATH:
				
					tmaxProperty.Value = this.Database.GetFolderSpec(Primary, true);
					break;
					
				case DXSECONDARY_PROP_REGISTER_PATH:
				
					tmaxProperty.Value = Primary.RegisterPath;
					break;
					
				case DXSECONDARY_PROP_CHILDREN:
				
					tmaxProperty.Value = m_lChildCount;
					break;
					
				case DXSECONDARY_PROP_BARCODE_ID:
				
					tmaxProperty.Value = m_lBarcodeId;
					break;
					
				case DXSECONDARY_PROP_MAPPED:
				
					tmaxProperty.Value = Mapped;
					break;
					
				case DXSECONDARY_PROP_HIGH_RESOLUTION:
				
					tmaxProperty.Value = HighResolution;
					break;
					
				case DXSECONDARY_PROP_HIDDEN:
				
					tmaxProperty.Value = Hidden;
					break;
					
				case DXSECONDARY_PROP_AUTO_TRANSITION:
				
					tmaxProperty.Value = AutoTransition;
					break;
					
				case DXSECONDARY_PROP_TRANSITION_TIME:
				
					tmaxProperty.Value = TransitionTime;
					break;
					
				case DXSECONDARY_PROP_ALIAS_ID:
				
					tmaxProperty.Value = m_lAliasId;
					break;
					
				case DXSECONDARY_PROP_RELATIVE_PATH:
				
					if(m_lAliasId > 0)
						tmaxProperty.Value = this.Database.GetAliasedPath(this);
					else
						tmaxProperty.Value = RelativePath;
					break;
					
				case DXSECONDARY_PROP_DURATION:
				
					tmaxProperty.Value = CTmaxToolbox.SecondsToString(GetDuration());
					break;
					
				case DXSECONDARY_PROP_SOURCE_ID:
				
					tmaxProperty.Value = SourceId;
					break;
					
				case DXSECONDARY_PROP_SOURCE_TYPE:
				
					tmaxProperty.Value = SourceType;
					break;
					
				case DXSECONDARY_PROP_SOURCE_BARCODE:
				
					if(GetSource() != null)
						tmaxProperty.Value = this.Database.GetBarcode(GetSource(), false);
					break;
					
				case DXSECONDARY_PROP_SOURCE_SHORTCUTS:
				
					if((GetSource() != null) && (GetSource().GetMediaLevel() == TmaxMediaLevels.Tertiary))
						tmaxProperty.Value = ((CDxTertiary)GetSource()).HasShortcuts;
					break;
					
				case DXSECONDARY_PROP_FOREIGN_BARCODE:
				
					tmaxProperty.Value = GetForeignBarcode();
					break;
					
				case DXSECONDARY_PROP_SOURCE_FILENAME:
				
					if(GetSource() != null)
						tmaxProperty.Value = GetSource().GetFileName();
					break;
					
				case DXSECONDARY_PROP_SOURCE_PATH:
				
					if(GetSource() != null)
						tmaxProperty.Value = GetSource().GetFileSpec();
					break;
					
				case DXSECONDARY_PROP_SOURCE_DURATION:
				
					if(GetSource() != null)
						tmaxProperty.Value = CTmaxToolbox.SecondsToString((GetSource().GetDuration()));
					break;
					
				case DXSECONDARY_PROP_EXPORT_IMAGE:
				
					tmaxProperty.Value = GetFileName();
					break;
					
				//	These properties are read-only
				case DXSECONDARY_PROP_MEDIA_TYPE:
				
					break;
					
				default:
				
					//	Could be an extents property
					if((m_eMediaType != TmaxMediaTypes.Page) &&
					   (m_eMediaType != TmaxMediaTypes.Slide))
					{
						if(GetExtent() != null)
							GetExtent().RefreshProperty(tmaxProperty);
					}
						
					base.RefreshProperty(tmaxProperty);
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
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
				case DXSECONDARY_PROP_RELATIVE_PATH:
				
					if(this.Database != null)
					{
						//	Verify the new path first
						if(this.Database.VerifyUserPath(this, strValue, !bConfirmed) == true)
						{
							return this.Database.SetUserPath(this, strValue);
						}
						else
						{
							return false;
						}
						
					}
					else
					{
						return false;
					}
					
				case DXSECONDARY_PROP_FOREIGN_BARCODE:
			
					if(strValue != null)
						SetForeignBarcode(strValue, true, true);
					else
						SetForeignBarcode("", true, false);
					
					return true;
					
				default:
				
					return base.SetProperty(iId, strValue, bConfirmed, strMessage);
					
			}// switch(iId)
			
		}// public override bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		
		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public override bool GetCanClean(bool bFill)
		{
			//	Must be a page or a scene
			switch(this.MediaType)
			{
				case TmaxMediaTypes.Page:
				
					//	Cleaning now includes rotating (ver 6.2.3)
					//return (HighResolution == true);
					return true;
					
				case TmaxMediaTypes.Scene:
				
					//	Use the source record to make the determination
					if(this.GetSource() != null)
						return (this.GetSource().GetCanClean(bFill));
					break;
			}
			
			return false;
			
		}// public override bool GetCanClean(bool bFill)
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public override string GetText()
		{
			if((this.Database != null) && (this.Database.AppOptions != null))
				return GetText(this.Database.AppOptions.SecondaryTextMode);
			else
				return base.GetText();
		}
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		/// <param name="eMode">The desired TrialMax text mode</param>
		/// <returns>The text that represents this record</returns>
		public override string GetText(TmaxTextModes eMode)
		{
			string			strText  = "";
			string			strSource = "";
			TmaxTextModes	eSource = 0;
			
			//	Is this secondary object a scene?
			if(MediaType == TmaxMediaTypes.Scene)
			{
				if((eMode & TmaxTextModes.DisplayOrder) == TmaxTextModes.DisplayOrder)
				{
					strText = m_lDisplayOrder.ToString();
				}
				
				if((eMode & TmaxTextModes.Barcode) == TmaxTextModes.Barcode)
				{
					//	Get the barcode for the scene
					if(strText.Length > 0)
						strText += " - ";
					strText += this.Database.GetBarcode(this, false);

				}
				
				//	Get the source record
				if(Source == null)
					Source = this.Database.GetRecordFromId(SourceId, true);
					
				//	Build the source text mode
				if((eMode & TmaxTextModes.Barcode) == TmaxTextModes.Barcode)
					eSource |= TmaxTextModes.Barcode;
				if((eMode & TmaxTextModes.Name) == TmaxTextModes.Name)
					eSource |= TmaxTextModes.Name;
				if((eMode & TmaxTextModes.Filename) == TmaxTextModes.Filename)
					eSource |= TmaxTextModes.Filename;
				
				if(Source != null)
					strSource = Source.GetText(eSource);
				else
					strSource = m_strSourceId; // Use PSTQ value
				
				if(strText.Length == 0)
					strText = this.Database.GetBarcode(this, false);
					
				if(strSource.Length > 0)
				{
					strText += ("  (" + strSource + ")");
				}
				
			}// if(MediaType == TmaxMediaTypes.Scene)
			else
			{
				if((eMode & TmaxTextModes.DisplayOrder) == TmaxTextModes.DisplayOrder)
				{
					strText = m_lDisplayOrder.ToString();
				}
				
				if((eMode & TmaxTextModes.Barcode) == TmaxTextModes.Barcode)
				{
					//	Get the barcode for the scene
					if(strText.Length > 0)
						strText += " - ";
					strText += this.Database.GetBarcode(this, false);

				}
				
				if((eMode & TmaxTextModes.Name) == TmaxTextModes.Name)
				{
					//	Get the barcode for the scene
					if(strText.Length > 0 && m_strName.Length > 0)
						strText += " - ";
					strText += m_strName;

				}
				
				if((eMode & TmaxTextModes.Filename) == TmaxTextModes.Filename)
				{
					//	Get the barcode for the scene
					if(strText.Length > 0 && m_strFilename.Length > 0)
						strText += " - ";
					strText += m_strFilename;

				}
				
				if(strText.Length == 0)
					strText = this.Database.GetBarcode(this, false);
				
			}// if(MediaType == TmaxMediaTypes.Scene)
				
			return strText;
		}
					
		/// <summary>This function is called to get the name of the file associated with the record</summary>
		/// <returns>The name of the associated file</returns>
		public override string GetFileName()
		{
			if(m_strFilename.Length > 0)
				return m_strFilename;
			else
				return base.GetFileName();
		}
		
		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		public override string GetMediaId()
		{
			if(Primary != null)
				return Primary.GetMediaId();
			else
				return "";
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
		
		/// <summary>This method is called to get the time required to play the record</summary>
		/// <param name="bIncludeDelay">true to include the transition delay for scenes</param>
		/// <returns>The total time in decimal seconds</returns>
		public override double GetDuration()
		{
			double dDuration = -1.0;
			
			//	What type of record is this?
			switch(MediaType)
			{
				case TmaxMediaTypes.Scene:
				
					//	Use the duration of the source record
					if(GetSource() != null)
						dDuration = GetSource().GetDuration();

					break;
					
				case TmaxMediaTypes.Segment:
				
					if(GetExtent() != null)
					{
						dDuration = (GetExtent().Stop - GetExtent().Start);
					}
					break;
					
				case TmaxMediaTypes.Page:
				case TmaxMediaTypes.Slide:
				default:
				
					break;
					
			}
			
			return dDuration;
		}

		/// <summary>This function is called to use the XML scene to set the record properties</summary>
		/// <param name="xmlScene">The scene used to initialize the object</param>
		public void SetProperties(CXmlScene xmlScene)
		{
			this.Hidden = xmlScene.Hidden;
			this.AutoTransition = xmlScene.AutoTransition;
			this.TransitionTime = (short)(xmlScene.TransitionTime);
			this.BarcodeId = xmlScene.BarcodeId;
		
		}// public void SetProperties(CXmlScene xmlScene)
		
		/// <summary>Called to determine if this is a script scene bound to a video designation</summary>
		/// <returns>true if bound to a video designation</returns>
		public bool GetIsVideoDesignation()
		{
			if(this.MediaType != TmaxMediaTypes.Scene) return false;
			if(this.GetSource() == null) return false;
			if(this.GetSource().MediaType != TmaxMediaTypes.Designation) return false;
			return ((CDxTertiary)(this.GetSource())).IsVideoDesignation;
		
		}// public bool GetIsVideoDesignation()
		
		/// <summary>Called to determine if there is scrollable text available for this record</summary>
		/// <returns>true if scrollable text is available</returns>
		public bool GetHasScrollableText()
		{
			if(this.MediaType != TmaxMediaTypes.Scene) return false;
			if(this.GetSource() == null) return false;
			if(this.GetSource().MediaType != TmaxMediaTypes.Designation) return false;
			if(((CDxPrimary)(this.GetSource())).MediaType != TmaxMediaTypes.Deposition) return false;
			return true;
		
		}// public bool GetHasScrollableText()
		
		/// <summary>Called to determine if the ScrollText option has been set for the source designation</summary>
		/// <returns>true if ScrollText is set</returns>
		public bool GetScrollText()
		{
			if(this.GetIsVideoDesignation() == false) return false;
			if(this.GetSource() == null) return false;
			return ((CDxTertiary)(GetSource())).IsVideoDesignation;
		
		}// public bool GetScrollText()
		
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
				m_dxExtents.Secondary = this;
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
		/// <param name="dxPrimary">The parent primary record</param>
		private void Initialize(CDxPrimary dxPrimary)
		{
			m_dxTertiaries.Secondary = this;	
			m_dxPrimary = dxPrimary;
			
			//	Create the codes collection
			//m_dxCodes = new CDxCodes(this.Database);
			//m_dxCodes.Owner = this;
		
		}// private void Initialize(CDxPrimary dxPrimary)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The id of the primary media parent</summary>
		public long PrimaryMediaId
		{
			get { return m_lPrimaryMediaId; }
			set { m_lPrimaryMediaId = value; }
		}
		
		/// <summary>The id of the page when it's part of a multipage primary format</summary>
		public long MultipageId
		{
			get { return m_lMultipageId; }
			set { m_lMultipageId = value; }
		}
		
		/// <summary>The time in seconds to transition to the next scene</summary>
		public short TransitionTime
		{
			get { return m_sTransitionTime; }
			set { m_sTransitionTime = value; }
		}
		
		/// <summary>The scene type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes SourceType
		{
			get { return GetSource() != null ? GetSource().MediaType : m_eSourceType; }
			set { m_eSourceType = value; }
		}
		
		/// <summary>The source record exchange object</summary>
		public CDxMediaRecord Source
		{
			get { return GetSource(); }
			set { m_dxSource = value; }
		}
		
		/// <summary>The primary object that owns this record</summary>
		public CDxPrimary Primary
		{
			get { return m_dxPrimary; }
			set { m_dxPrimary = value; }
		}
		
		/// <summary>The id of the driver/server alias when linked</summary>
		public long AliasId
		{
			get	{ return m_lAliasId;  }
			set { m_lAliasId = value; }
		}
		
		/// <summary>Path relative to the root folder or the aliased drive</summary>
		public string RelativePath
		{
			get { return m_strRelativePath; }
			set { m_strRelativePath = value;  }
		}
		
		/// <summary>True if source media is linked</summary>
		public bool Linked
		{
			get	{ return GetLinked(); }
		}
		
		/// <summary>True if source media has scrollable text</summary>
		public bool HasScrollableText
		{
			get	{ return GetHasScrollableText(); }
		}
		
		/// <summary>The name of the file associated with the secondary media</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value;  }
		}
		
		/// <summary>The PSTQ id of the secondary scene object</summary>
		public string SourceId
		{
			get { return m_strSourceId; }
			set { m_strSourceId = value;  }
		}
		
		/// <summary>The collection of tertiary media objects</summary>
		public CDxTertiaries Tertiaries
		{
			get	{ return m_dxTertiaries; }
		}
		
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
				{
					return GetExtent().Start;
				}
				else
				{
					return -1;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().Start = value;	
				}
				
			}
			
		}
		
		/// <summary>The start PL value</summary>
		public long StartPL
		{
			get
			{
				if(GetExtent() != null)
				{
					return GetExtent().StartPL;
				}
				else
				{
					return -1;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().StartPL = value;	
				}
				
			}
			
		}
		
		/// <summary>The start tuned flag</summary>
		public bool StartTuned
		{
			get
			{
				if(GetExtent() != null)
				{
					return GetExtent().StartTuned;
				}
				else
				{
					return false;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().StartTuned = value;	
				}
				
			}
			
		}
		
		/// <summary>The stop time</summary>
		public double Stop
		{
			get
			{
				if(GetExtent() != null)
				{
					return GetExtent().Stop;
				}
				else
				{
					return -1;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().Stop = value;	
				}
				
			}
			
		}
		
		/// <summary>The stop PL value</summary>
		public long StopPL
		{
			get
			{
				if(GetExtent() != null)
				{
					return GetExtent().StopPL;
				}
				else
				{
					return -1;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().StopPL = value;	
				}
				
			}
			
		}
		
		/// <summary>The stop tuned flag</summary>
		public bool StopTuned
		{
			get
			{
				if(GetExtent() != null)
				{
					return GetExtent().StopTuned;
				}
				else
				{
					return false;
				}
			
			}
			set 
			{ 
				if(GetExtent() != null)
				{
					GetExtent().StopTuned = value;	
				}
				
			}
			
		}
		
		/// <summary>Flag to indicate that the object should be hidden</summary>
		public bool Hidden
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxSecondaryAttributes.Hidden) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxSecondaryAttributes.Hidden;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxSecondaryAttributes.Hidden);
				}
			
			}
		
		}
		
		/// <summary>Flag to indicate that the object should be automatically transitioned during playback</summary>
		public bool AutoTransition
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxSecondaryAttributes.AutoTransition) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxSecondaryAttributes.AutoTransition;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxSecondaryAttributes.AutoTransition);
				}
			
			}
		
		}
		
		/// <summary>Flag to indicate that the object should be displayed in high resolution</summary>
		public bool HighResolution
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxSecondaryAttributes.HighResPage) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxSecondaryAttributes.HighResPage;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxSecondaryAttributes.HighResPage);
				}
			
			}
		
		}
		
		#endregion Properties
	
	}// class CDxSecondary

	/// <summary>
	/// This class is used to manage a ArrayList of CDxSecondary objects
	/// </summary>
	public class CDxSecondaries : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			PrimaryMediaId,
			BarcodeId,
			Children,
			Attributes,
			MediaType,
			SourceType,
			TransitionTime,
			SourceId,
			AliasId,
			RelativePath,
			Filename,
			MultipageId,
			Description,
			Name,
			DisplayOrder,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
		}

		protected const string TABLE_NAME = "SecondaryMedia";
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to Primary property</summary>
		protected CDxPrimary m_dxPrimary = null;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxSecondaries() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxSecondaries(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxSecondary">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxSecondary Add(CDxSecondary dxSecondary)
		{
			return (CDxSecondary)base.Add(dxSecondary);
			
		}// Add(CDxSecondary dxSecondary)

		/// <summary>This method will delete the specified record</summary>
		/// <param name="dxRecord">Object to be deleted</param>
		/// <returns>true if successful</returns>
		public override bool Delete(CBaseRecord dxRecord)
		{
			CDxExtent dxExtent = null;
			
			if(base.Delete(dxRecord) == false)
				return false;
				
			//	Is this a segment?
			if(((CDxSecondary)dxRecord).MediaType == TmaxMediaTypes.Segment)
			{
				dxExtent = ((CDxSecondary)dxRecord).GetExtent();
				
				/// Delete the transcript if it exists
				if((dxExtent != null) && (dxExtent.Collection != null))
				{
					dxExtent.Collection.Delete(dxExtent);
				}
			}
			
			return true;
			
		}
		
		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			CDxExtents dxExtents = null;
			
			if(base.Update(dxRecord) == false)
				return false;
				
			//	Is this a segment?
			if(((CDxSecondary)dxRecord).MediaType == TmaxMediaTypes.Segment)
			{
				dxExtents = ((CDxSecondary)dxRecord).Extents;
				
				/// Update the extent if it exists
				if((dxExtents != null) && (dxExtents.Count > 0))
				{
					dxExtents.Update(dxExtents[0]);
				}
			}
			
			return true;
			
		}
		
		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxSecondaries Dispose()
		{
			return (CDxSecondaries)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxSecondary Find(long lAutoId, bool bBarcode)
		{
			return (CDxSecondary)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxSecondary Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>This method will locate the segment with the specified XML identifier</summary>
		/// <param name="strXmlId">The xml key identifier</param>
		/// <returns>the segment with the specified XML identifier</returns>
		public CDxSecondary Find(string strXmlId)
		{
			long lXmlId = 0;
			
			try   { lXmlId = System.Convert.ToInt64(strXmlId); }
			catch { return null; }
			
			//	Locate the appropriate segment
			foreach(CDxSecondary O in this)
			{
				if((O.GetExtent() != null) && (O.GetExtent().XmlSegmentId == lXmlId))
				{
					return O;
				}
							
			}// foreach(CDxSecondary O in this)
			
			return null;
			
		}// public CDxSecondary Find(string strXmlId)
		
		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxSecondary this[int iIndex]
		{
			get
			{
				return (CDxSecondary)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxSecondary GetAt(int iIndex)
		{
			return (CDxSecondary)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to flush all records belonging to the collection
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = "DELETE FROM ";
			
			strSQL += TableName;
			strSQL += " WHERE PrimaryMediaId = ";
			strSQL += m_dxPrimary.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}
		
		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxSecondary	dxSecondary = (CDxSecondary)dxRecord;
			string			strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.PrimaryMediaId.ToString() + ",");
			strSQL += (eFields.BarcodeId.ToString() + ",");
			strSQL += (eFields.Children.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.MediaType.ToString() + ",");
			strSQL += (eFields.SourceType.ToString() + ",");
			strSQL += (eFields.TransitionTime.ToString() + ",");
			strSQL += (eFields.SourceId.ToString() + ",");
			strSQL += (eFields.AliasId.ToString() + ",");
			strSQL += (eFields.RelativePath.ToString() + ",");
			strSQL += (eFields.Filename.ToString() + ",");
			strSQL += (eFields.MultipageId.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.DisplayOrder.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + dxSecondary.PrimaryMediaId.ToString() + "',");
			strSQL += ("'" + dxSecondary.BarcodeId.ToString() + "',");
			strSQL += ("'" + dxSecondary.ChildCount.ToString() + "',");
			strSQL += ("'" + dxSecondary.Attributes.ToString() + "',");
			strSQL += ("'" + ((int)dxSecondary.MediaType).ToString() + "',");
			strSQL += ("'" + ((int)dxSecondary.SourceType).ToString() + "',");
			strSQL += ("'" + (dxSecondary.TransitionTime).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxSecondary.SourceId) + "',");
			strSQL += ("'" + dxSecondary.AliasId.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxSecondary.RelativePath) + "',");
			strSQL += ("'" + SQLEncode(dxSecondary.Filename) + "',");
			strSQL += ("'" + dxSecondary.MultipageId.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxSecondary.Description) + "',");
			strSQL += ("'" + SQLEncode(dxSecondary.Name) + "',");
			strSQL += ("'" + dxSecondary.DisplayOrder.ToString() + "',");
			strSQL += ("'" + dxSecondary.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxSecondary.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxSecondary.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxSecondary.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxSecondary	dxSecondary = (CDxSecondary)dxRecord;
			string			strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.PrimaryMediaId.ToString() + " = '" + dxSecondary.PrimaryMediaId.ToString() + "',");
			strSQL += (eFields.BarcodeId.ToString() + " = '" + dxSecondary.BarcodeId.ToString() + "',");
			strSQL += (eFields.Children.ToString() + " = '" + dxSecondary.ChildCount.ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxSecondary.Attributes.ToString() + "',");
			strSQL += (eFields.MediaType.ToString() + " = '" + ((int)dxSecondary.MediaType).ToString() + "',");
			strSQL += (eFields.SourceType.ToString() + " = '" + ((int)dxSecondary.SourceType).ToString() + "',");
			strSQL += (eFields.TransitionTime.ToString() + " = '" + ((int)dxSecondary.TransitionTime).ToString() + "',");
			strSQL += (eFields.SourceId.ToString() + " = '" + SQLEncode(dxSecondary.SourceId) + "',");
			strSQL += (eFields.AliasId.ToString() + " = '" + dxSecondary.AliasId.ToString() + "',");
			strSQL += (eFields.RelativePath.ToString() + " = '" + SQLEncode(dxSecondary.RelativePath) + "',");
			strSQL += (eFields.Filename.ToString() + " = '" + SQLEncode(dxSecondary.Filename) + "',");
			strSQL += (eFields.MultipageId.ToString() + " = '" + dxSecondary.MultipageId.ToString() + "',");
			strSQL += (eFields.Description.ToString() + " = '" + SQLEncode(dxSecondary.Description) + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxSecondary.Name) + "',");
			strSQL += (eFields.DisplayOrder.ToString() + " = '" + dxSecondary.DisplayOrder.ToString() + "',");
			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxSecondary.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxSecondary.CreatedOn.ToString() + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxSecondary.ModifiedBy.ToString() + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxSecondary.ModifiedOn.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxSecondary.AutoId.ToString();
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
			
			if(m_dxPrimary != null)
			{
				strSQL += " WHERE PrimaryMediaId = ";
				strSQL += m_dxPrimary.AutoId.ToString();
				strSQL += " ORDER BY ";
				strSQL += eFields.DisplayOrder.ToString();
				strSQL += ";";
			}
			else
			{
				strSQL += " ORDER BY ";
				strSQL += eFields.DisplayOrder.ToString();
				strSQL += ";";
			}
			
			return strSQL;
		
		}// public override string GetSQLSelect()

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
			CDxSecondary dxSecondary = new CDxSecondary();
			
			if(dxSecondary != null)
			{
				dxSecondary.Collection = this;
				dxSecondary.Primary = m_dxPrimary;
				
				if(m_dxPrimary != null)
					dxSecondary.PrimaryMediaId = m_dxPrimary.AutoId;
			}
			
			return ((CBaseRecord)dxSecondary);
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxSecondary dxSecondary = (CDxSecondary)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxSecondary.AutoId;
					m_dxFields[(int)eFields.PrimaryMediaId].Value  = dxSecondary.PrimaryMediaId;
					m_dxFields[(int)eFields.BarcodeId].Value  = dxSecondary.BarcodeId;
					m_dxFields[(int)eFields.Children].Value = dxSecondary.ChildCount;
					m_dxFields[(int)eFields.Attributes].Value = dxSecondary.Attributes;
					m_dxFields[(int)eFields.MediaType].Value = dxSecondary.MediaType;
					m_dxFields[(int)eFields.SourceType].Value = dxSecondary.SourceType;
					m_dxFields[(int)eFields.TransitionTime].Value = dxSecondary.TransitionTime;
					m_dxFields[(int)eFields.SourceId].Value = dxSecondary.SourceId;
					m_dxFields[(int)eFields.AliasId].Value = dxSecondary.AliasId;
					m_dxFields[(int)eFields.RelativePath].Value = dxSecondary.RelativePath;
					m_dxFields[(int)eFields.Filename].Value = dxSecondary.Filename;
					m_dxFields[(int)eFields.MultipageId].Value = dxSecondary.MultipageId;
					m_dxFields[(int)eFields.Description].Value = dxSecondary.Description;
					m_dxFields[(int)eFields.Name].Value = dxSecondary.Name;
					m_dxFields[(int)eFields.DisplayOrder].Value = dxSecondary.DisplayOrder;
					m_dxFields[(int)eFields.CreatedBy].Value = dxSecondary.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxSecondary.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxSecondary.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxSecondary.ModifiedOn;
				}
				else
				{
					dxSecondary.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxSecondary.PrimaryMediaId = (int)(m_dxFields[(int)eFields.PrimaryMediaId].Value);
					dxSecondary.BarcodeId = (int)(m_dxFields[(int)eFields.BarcodeId].Value);
					dxSecondary.ChildCount = (int)(m_dxFields[(int)eFields.Children].Value);
					dxSecondary.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxSecondary.MediaType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)(m_dxFields[(int)eFields.MediaType].Value));
					dxSecondary.SourceType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)(m_dxFields[(int)eFields.SourceType].Value));
					dxSecondary.TransitionTime = (short)(m_dxFields[(int)eFields.TransitionTime].Value);
					dxSecondary.SourceId = (string)(m_dxFields[(int)eFields.SourceId].Value);
					dxSecondary.AliasId = (int)(m_dxFields[(int)eFields.AliasId].Value);
					dxSecondary.RelativePath = (string)(m_dxFields[(int)eFields.RelativePath].Value);
					dxSecondary.Filename = (string)(m_dxFields[(int)eFields.Filename].Value);
					dxSecondary.MultipageId = (int)(m_dxFields[(int)eFields.MultipageId].Value);
					dxSecondary.Description = (string)(m_dxFields[(int)eFields.Description].Value);
					dxSecondary.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxSecondary.DisplayOrder = (int)(m_dxFields[(int)eFields.DisplayOrder].Value);
					dxSecondary.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxSecondary.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxSecondary.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxSecondary.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
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
				case eFields.PrimaryMediaId:
				case eFields.BarcodeId:
				case eFields.Children:
				case eFields.Attributes:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				case eFields.AliasId:
				case eFields.MultipageId:
				case eFields.DisplayOrder:
				case eFields.TransitionTime:
				
					dxField.Value = 0;
					break;
					
				case eFields.MediaType:
				case eFields.SourceType:
				
					dxField.Value = TmaxMediaTypes.Unknown;
					break;
					
				case eFields.Name:
				case eFields.SourceId:
				case eFields.Description:
				case eFields.RelativePath:
				case eFields.Filename:
				
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
		public CDxPrimary Primary
		{
			get { return m_dxPrimary; }
			set { m_dxPrimary = value; }
		}
		
		#endregion Properties
		
		
	}//	CDxSecondaries
		
}// namespace FTI.Trialmax.Database
