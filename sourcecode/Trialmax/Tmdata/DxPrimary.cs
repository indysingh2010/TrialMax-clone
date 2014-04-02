using System;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class encapsulates the information used to define a Trialmax case
	/// </summary>
	public class CDxPrimary : FTI.Trialmax.Database.CDxMediaRecord, ITmaxDeposition
	{
		#region Constants
		
		protected const int DXPRIMARY_PROP_FIRST_ID			= DX_MEDIA_RECORD_PROP_LAST + 1000;

		protected const int DXPRIMARY_PROP_MEDIA_ID			= DXPRIMARY_PROP_FIRST_ID + 1;
		protected const int DXPRIMARY_PROP_EXHIBIT			= DXPRIMARY_PROP_FIRST_ID + 2;
		protected const int DXPRIMARY_PROP_ALT_BARCODE		= DXPRIMARY_PROP_FIRST_ID + 3;
		protected const int DXPRIMARY_PROP_ROOT_PATH		= DXPRIMARY_PROP_FIRST_ID + 4;
		protected const int DXPRIMARY_PROP_REGISTER_PATH	= DXPRIMARY_PROP_FIRST_ID + 5;
		protected const int DXPRIMARY_PROP_ALIAS_ID			= DXPRIMARY_PROP_FIRST_ID + 6;
		protected const int DXPRIMARY_PROP_BARCODE			= DXPRIMARY_PROP_FIRST_ID + 7;
		protected const int DXPRIMARY_PROP_MEDIA_TYPE		= DXPRIMARY_PROP_FIRST_ID + 8;
		protected const int DXPRIMARY_PROP_CHILDREN			= DXPRIMARY_PROP_FIRST_ID + 9;
		protected const int DXPRIMARY_PROP_ATTRIBUTES		= DXPRIMARY_PROP_FIRST_ID + 10;
		protected const int DXPRIMARY_PROP_MEDIA_PATH		= DXPRIMARY_PROP_FIRST_ID + 11;
		protected const int DXPRIMARY_PROP_RELATIVE_PATH	= DXPRIMARY_PROP_FIRST_ID + 12;
		protected const int DXPRIMARY_PROP_FIRST_FILE		= DXPRIMARY_PROP_FIRST_ID + 13;
		protected const int DXPRIMARY_PROP_PLAYLIST			= DXPRIMARY_PROP_FIRST_ID + 14;
		protected const int DXPRIMARY_PROP_VIDEO_PATH		= DXPRIMARY_PROP_FIRST_ID + 15;
		protected const int DXPRIMARY_PROP_DURATION			= DXPRIMARY_PROP_FIRST_ID + 16;
		protected const int DXPRIMARY_PROP_FOREIGN_BARCODE	= DXPRIMARY_PROP_FIRST_ID + 17;
		protected const int DXPRIMARY_PROP_MAPPED			= DXPRIMARY_PROP_FIRST_ID + 18;
		protected const int DXPRIMARY_PROP_ADMITTED			= DXPRIMARY_PROP_FIRST_ID + 19;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Secondaries property</summary>
		private CDxSecondaries m_dxSecondaries = new CDxSecondaries();
		
		/// <summary>Local member bound to Transcripts property</summary>
		private CDxTranscripts m_dxTranscripts = null;
		
		/// <summary>Local member bound to AliasId property</summary>
		private long m_lAliasId = 0;
		
		/// <summary>Local member bound to RelativePath property</summary>
		protected string m_strRelativePath = "";
		
		/// <summary>Local member bound to RegisterPath property</summary>
		protected string m_strRegisterPath = "";
		
		/// <summary>Local member bound to MediaId property</summary>
		protected string m_strMediaId = "";
		
		/// <summary>Local member bound to AltBarcode property</summary>
		protected string m_strAltBarcode = "";
		
		/// <summary>Local member bound to Exhibit property</summary>
		protected string m_strExhibit = "";
		
		/// <summary>Local member bound to Filename property</summary>
		protected string m_strFilename = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxPrimary() : base()
		{
			//	Establish ownership of the secondary collection
			m_dxSecondaries.Primary = this;
			
			//	Create the codes collection
			m_dxCodes = new CDxCodes(this.Database);
			m_dxCodes.Owner = this;
		}
		
		/// <summary>This method is called to fill the secondaries collection</summary>
		/// <returns>true if successful</returns>
		
		public override bool Fill()
		{
			bool bSuccessful = false;
			
			if(m_dxSecondaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
					m_dxSecondaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
						
				//	Is this an uninitialized PowerPoint presentation?
				if((MediaType == TmaxMediaTypes.Powerpoint) && (this.ChildCount == 0))
				{
					//	Add the slides contained in the presentation
					if(m_dxSecondaries.Database != null)
						bSuccessful = m_dxSecondaries.Database.ExportSlides(this);
				}
				else
				{
					//	Clear the existing objects
					m_dxSecondaries.Clear();
				
					//	Get the secondary records from the database			
					bSuccessful = m_dxSecondaries.Fill();
					
				}// if((MediaType == TmaxMediaTypes.Powerpoint) && (this.ChildCount == 0))
			
				//	Were we able to fill the child collection?
				if(bSuccessful == true)
				{
					//	Just in case something messed up..........
					if(this.ChildCount != m_dxSecondaries.Count)
					{
						//Debug.Assert(this.ChildCount == m_dxSecondaries.Count);
						
						if(Collection != null)
						{
							this.ChildCount = m_dxSecondaries.Count;
							Collection.Update(this);
						}
						
					}// if(this.ChildCount != m_dxSecondaries.Count)
					
				}// if(bSuccessful == true)
				
			}// if(m_dxSecondaries != null)
			
			return bSuccessful;
			
		}// public bool Fill()
		
		/// <summary>This method is called to add a secondary media object to the local collection</summary>
		/// <param name="dxSecondary">Secondary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxSecondary dxSecondary)
		{
			if(m_dxSecondaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxSecondaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct primary id
				dxSecondary.PrimaryMediaId = AutoId;
				dxSecondary.Primary = this;
				
				return (m_dxSecondaries.Add(dxSecondary) != null);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to add the specified transcript</summary>
		/// <param name="dxTranscript">Transcript media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxTranscript dxTranscript)
		{
			if(m_dxTranscripts == null)
				CreateTranscripts();
				
			if(m_dxTranscripts != null)
				return (m_dxTranscripts.Add(dxTranscript) != null);
			else
				return false;
				
		}

		/// <summary>This method is called to update a secondary media object</summary>
		/// <param name="dxSecondary">Secondary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Update(CDxSecondary dxSecondary)
		{
			if(m_dxSecondaries != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxSecondaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
					
				//	Make sure it has the correct primary id
				dxSecondary.PrimaryMediaId = AutoId;
				
				return m_dxSecondaries.Update(dxSecondary);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>This method is called to delete a secondary media object</summary>
		/// <param name="dxSecondary">Secondary media data exchange object</param>
		/// <returns>true if successful</returns>
		public bool Delete(CDxSecondary dxSecondary)
		{
			int	iIndex = -1;
			
			Debug.Assert(m_dxSecondaries != null);
			if(m_dxSecondaries == null) return false;
			
			//	Make sure this secondary exists in the collection
			if((iIndex = m_dxSecondaries.IndexOf(dxSecondary)) < 0) return false;
			
			//	Make sure the secondary collection is using the correct database
			if(m_dxCollection != null)
                m_dxSecondaries.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				
			if(m_dxSecondaries.Delete(dxSecondary) == true)
			{
				//	Update the display order of remaining records
				for(int i = iIndex; i < m_dxSecondaries.Count; i++)
				{
					m_dxSecondaries[i].DisplayOrder = (i + 1);
					m_dxSecondaries.Update(m_dxSecondaries[i]);
				}
						
				//	Update this record
				this.ChildCount = m_dxSecondaries.Count;
				if(iIndex == 0)
				{
					if(m_dxSecondaries.Count > 0)
						m_strFilename = m_dxSecondaries[0].Filename;
					else
						m_strFilename = "";
				}
				
				this.SetPlaylistFromChildren();
				
				if(Collection != null)
					Collection.Update(this);
						
				return true;
			}

			//	Must have been an error
			return false;
		
		}// Delete(CDxSecondary dxSecondary)
		
		/// <summary>This function is called to get the name of the file associated with the record</summary>
		/// <returns>The name of the associated file</returns>
		public override string GetFileName()
		{
			if(m_strFilename.Length > 0)
				return m_strFilename;
			else
				return base.GetFileName();
		}
		
		/// <summary>This method retrieves the transcript exchange object associated with this record</summary>
		/// <returns>The transcript's record exchange object</returns>
		public CDxTranscript GetTranscript()
		{
			//	Do we need to create the interface to the transcripts table?
			if(m_dxTranscripts == null)
			{
				if(CreateTranscripts() == false)
					return null;
				else
					m_dxTranscripts.Fill();
			}
				
			if(m_dxTranscripts.Count > 0)
				return (m_dxTranscripts[0] as CDxTranscript);
			else
				return null; // Must not be a record for the transcript

		}// public CDxTranscript GetTranscript()

		/// <summary>This method retrieves the FirstPL value of the bounded transcript</summary>
		/// <returns>The transcript's FirstPL value</returns>
		public long GetFirstPL()
		{
			if((this.MediaType == TmaxMediaTypes.Deposition) && (this.Transcript != null))
				return this.Transcript.FirstPL;
			else
				return 0;

		}// public long GetFirstPL()

		/// <summary>This method retrieves the LastPL value of the bounded transcript</summary>
		/// <returns>The transcript's LastPL value</returns>
		public long GetLastPL()
		{
			if((this.MediaType == TmaxMediaTypes.Deposition) && (this.Transcript != null))
				return this.Transcript.LastPL;
			else
				return 0;

		}// public long GetLastPL()

		/// <summary>This function is called to delete the specified secondary record</summary>
		public override bool Delete(CDxMediaRecord dxChild)
		{
			return Delete((CDxSecondary)dxChild);
		}
		
		/// <summary>This method is called by the base class after reordering the children but before updating the record</summary>
		public override void OnReordered()
		{
			//	Update the filename
			if((m_dxSecondaries != null) && (m_dxSecondaries.Count > 0))
				m_strFilename = m_dxSecondaries[0].Filename;
			else
				m_strFilename = "";
		}
	
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.Primary;
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This function is called to get the alias identifier assigned to the record</summary>
		/// <returns>The alias identifier</returns>
		public override long GetAliasId()
		{
			return AliasId;
		}
		
		/// <summary>This function is called to get the default text used to display the name of the record</summary>
		/// <returns>The default name that represents this record</returns>
		public override string GetDefaultName()
		{
			return m_strMediaId;
		}
		
		/// <summary>This function is called to get the collection of child records</summary>
		/// <returns>The child collection if it exists</returns>
		public override CDxMediaRecords GetChildCollection()
		{
			return m_dxSecondaries;
		}
		
		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		public override string GetMediaId()
		{
			return m_strMediaId;
		}

		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			double dDuration = 0;
			
			//	Add the base class properties first
			base.GetProperties(tmaxProperties);
			
			//	Add the type specific properties
			switch(m_eMediaType)
			{	
				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Powerpoint:
				case TmaxMediaTypes.Recording:

					tmaxProperties.Add(DXPRIMARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_ID, "Media Id", m_strMediaId, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropGridEditors.Memo);
					//tmaxProperties.Add(DXPRIMARY_PROP_ADMITTED, "Admitted", this.Admitted, TmaxPropertyCategories.Media, TmaxPropGridEditors.Boolean);
					
					if(((CDxPrimary)this).AliasId > 0)
						tmaxProperties.Add(DXPRIMARY_PROP_RELATIVE_PATH, "Relative Path", this.Database.GetAliasedPath(this), TmaxPropertyCategories.Media, TmaxPropGridEditors.Custom);
					else
						tmaxProperties.Add(DXPRIMARY_PROP_RELATIVE_PATH, "Relative Path", RelativePath, TmaxPropertyCategories.Media, TmaxPropGridEditors.Custom);

					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_PATH, "Media Path", this.Database.GetFolderSpec((CDxPrimary)this, false), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);

					if(MediaType == TmaxMediaTypes.Recording)
						tmaxProperties.Add(DXPRIMARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
					tmaxProperties.Add(DXPRIMARY_PROP_ROOT_PATH, "Case Media Path", this.Database.GetCasePath(this), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				
					tmaxProperties.Add(DXPRIMARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_ALIAS_ID, "Alias Id", m_lAliasId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_FIRST_FILE, "First File", Filename, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_REGISTER_PATH, "Registered From", m_strRegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
						
					break;
				
				case TmaxMediaTypes.Script:
				

					tmaxProperties.Add(DXPRIMARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_FOREIGN_BARCODE, "Foreign Barcode", GetForeignBarcode(), TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropGridEditors.Memo);
					//tmaxProperties.Add(DXPRIMARY_PROP_ADMITTED, "Admitted", this.Admitted, TmaxPropertyCategories.Media, TmaxPropGridEditors.Boolean);
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_ID, "Media Id", m_strMediaId, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					
					if((dDuration = GetDuration()) >= 0.0)
						tmaxProperties.Add(DXPRIMARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(dDuration), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					else
						tmaxProperties.Add(DXPRIMARY_PROP_DURATION, "Duration", "Manual", TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
					tmaxProperties.Add(DXPRIMARY_PROP_PLAYLIST, "Playlist", Playlist, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				
					tmaxProperties.Add(DXPRIMARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_MAPPED, "Mapped", Mapped, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

					break;
				
				case TmaxMediaTypes.Deposition:

					tmaxProperties.Add(DXPRIMARY_PROP_BARCODE, "Barcode", this.Database.GetBarcode(this, true), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					//tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropGridEditors.Memo);
					//tmaxProperties.Add(DXPRIMARY_PROP_ADMITTED, "Admitted", this.Admitted, TmaxPropertyCategories.Media, TmaxPropGridEditors.Boolean);
					if(((CDxPrimary)this).AliasId > 0)
					{
						tmaxProperties.Add(DXPRIMARY_PROP_RELATIVE_PATH, "Relative Video Path", this.Database.GetAliasedPath(this), TmaxPropertyCategories.Media, TmaxPropGridEditors.Custom);
					}
					else
					{
						tmaxProperties.Add(DXPRIMARY_PROP_RELATIVE_PATH, "Relative Video Path", RelativePath, TmaxPropertyCategories.Media, TmaxPropGridEditors.Custom);
					}
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_TYPE, "Media Type", m_eMediaType, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_MEDIA_ID, "Media Id", m_strMediaId, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
					tmaxProperties.Add(DXPRIMARY_PROP_DURATION, "Duration", CTmaxToolbox.SecondsToString(GetDuration()), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_ROOT_PATH, "Case Video Path", this.Database.GetCasePath(TmaxMediaTypes.Deposition), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
					
				
					tmaxProperties.Add(DXPRIMARY_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_ALIAS_ID, "Alias Id", m_lAliasId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_FIRST_FILE, "First File", Filename, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
					tmaxProperties.Add(DXPRIMARY_PROP_REGISTER_PATH, "Registered From", m_strRegisterPath, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				
					//	Should we add the transcript properties?
					if(GetTranscript() != null)
						GetTranscript().GetProperties(tmaxProperties);
					
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
				case DXPRIMARY_PROP_BARCODE:
				
					tmaxProperty.Value = this.Database.GetBarcode(this, true);
					break;
					
				case DXPRIMARY_PROP_MEDIA_PATH:
				
					tmaxProperty.Value = this.Database.GetFolderSpec(this, false);
					break;
					
				case DXPRIMARY_PROP_VIDEO_PATH:
				
					//	This property only gets added for depositions
					if(this.Linked == true)
						tmaxProperty.Value = this.Database.GetFolderSpec(this, false);
					break;
					
				case DXPRIMARY_PROP_MEDIA_ID:
				
					tmaxProperty.Value = MediaId;
					break;
					
				case DXPRIMARY_PROP_ADMITTED:
				
					tmaxProperty.Value = Admitted;
					break;
					
				case DXPRIMARY_PROP_EXHIBIT:
				
					tmaxProperty.Value = Exhibit;
					break;
					
				case DXPRIMARY_PROP_ALT_BARCODE:
				
					tmaxProperty.Value = AltBarcode;
					break;
					
				case DXPRIMARY_PROP_FOREIGN_BARCODE:
				
					tmaxProperty.Value = GetForeignBarcode();
					break;
					
				case DXPRIMARY_PROP_DURATION:
				
					tmaxProperty.Value = CTmaxToolbox.SecondsToString(GetDuration());
					break;
					
				case DXPRIMARY_PROP_ROOT_PATH:
				
					tmaxProperty.Value = this.Database.GetCasePath(this);
					break;
					
				case DXPRIMARY_PROP_REGISTER_PATH:
				
					tmaxProperty.Value = RegisterPath;
					break;
					
				case DXPRIMARY_PROP_CHILDREN:
				
					tmaxProperty.Value = m_lChildCount;
					break;
					
				case DXPRIMARY_PROP_ALIAS_ID:
				
					tmaxProperty.Value = m_lAliasId;
					break;
					
				case DXPRIMARY_PROP_FIRST_FILE:
				
					tmaxProperty.Value = GetFileName();
					break;
					
				case DXPRIMARY_PROP_PLAYLIST:
				
					tmaxProperty.Value = Playlist;
					break;
					
				case DXPRIMARY_PROP_MAPPED:
				
					tmaxProperty.Value = Mapped;
					break;
					
				case DXPRIMARY_PROP_RELATIVE_PATH:
				
					if(m_lAliasId > 0)
						tmaxProperty.Value = this.Database.GetAliasedPath(this);
					else
						tmaxProperty.Value = RelativePath;
					break;
					
				//	These properties are read-only
				case DXPRIMARY_PROP_MEDIA_TYPE:
				
					break;
					
				default:
				
					//	Could be a transcript property
					if(m_eMediaType == TmaxMediaTypes.Deposition)
					{
						if(GetTranscript() != null)
							GetTranscript().RefreshProperty(tmaxProperty);
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
				case DXPRIMARY_PROP_MEDIA_ID:
				
					if((strValue != null) && (strValue.Length > 0))
					{
						//	Make sure this MediaID does not already exist
						if(((CDxPrimaries)Collection).Find(strValue, this) != null)
						{
							if(strMessage != null)
								strMessage = (strValue + " is already an assigned MediaID. Duplicates are not permitted");
							return false;
						}
						else
						{
							//	Make sure it does not have any invalid characters
							if(MediaType != TmaxMediaTypes.Deposition)
							{
								if(strValue.IndexOfAny(this.Database.InvalidMediaIdChars.ToCharArray()) >= 0)
								{
									if(strMessage != null)
										strMessage = (strValue + " contains one or more invalid MediaID characters");
									return false;
								}
								else
								{
									strValue = this.Database.FormatMediaId(strValue, MediaType);
								}
								
							}
							
							m_strMediaId = strValue;
							return true;
						
						}
						
					}
					else
					{
						if(strMessage != null)
							strMessage = "NULL or empty MediaID is not permitted";
						return false;
					}
					
				case DXPRIMARY_PROP_ALT_BARCODE:
				
					if(strValue != null)
						m_strAltBarcode = strValue;
					else
						m_strAltBarcode = "";
					
					return true;
					
				case DXPRIMARY_PROP_FOREIGN_BARCODE:
				
					if(strValue != null)
						SetForeignBarcode(strValue, true, true);
					else
						SetForeignBarcode("", true, false);
					
					return true;
					
				case DXPRIMARY_PROP_ADMITTED:

					if((strValue == null) || (strValue.Length == 0))
						Admitted = false;
					else if(strValue == "0")
						Admitted = false;
					else if(String.Compare(strValue, "false", true) == 0)
						Admitted = false;
					else if(String.Compare(strValue, "no", true) == 0)
						Admitted = false;
					else
						Admitted = true;
					
					return true;
					
				case DXPRIMARY_PROP_EXHIBIT:

					if(strValue != null)
						this.Exhibit = strValue;
					else
						this.Exhibit = "";
					
					return true;
					
				case DXPRIMARY_PROP_RELATIVE_PATH:
				
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
					
				default:
				
					return base.SetProperty(iId, strValue, bConfirmed, strMessage);
					
			}// switch(iId)
			
		}// public override bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		
		/// <summary>This method is called to get the time required to play the record</summary>
		/// <returns>The total time in decimal seconds</returns>
		public override double GetDuration()
		{
			double dDuration = -1.0;
			
			//	What type of record is this?
			switch(MediaType)
			{
				case TmaxMediaTypes.Deposition:
				case TmaxMediaTypes.Recording:
				
					//	Fill the child collection if necessary
					if((this.Secondaries == null) || (this.Secondaries.Count == 0))
						this.Fill();
						
					//	Add the times for each secondary
					dDuration = 0;
					foreach(CDxSecondary O in this.Secondaries)
						dDuration += O.GetDuration();
						
					break;
					
				case TmaxMediaTypes.Script:
					
					//	Fill the child collection if necessary
					if((this.Secondaries == null) || (this.Secondaries.Count == 0))
						this.Fill();
						
					//	Add the times for each secondary
					dDuration = 0;
					for(int i = 0; i < this.Secondaries.Count; i++)
					{
						if(this.Secondaries[i].GetSource() != null)
						{
							//	What type of source media?
							switch(this.Secondaries[i].GetSource().MediaType)
							{
								case TmaxMediaTypes.Segment:
								case TmaxMediaTypes.Designation:
								case TmaxMediaTypes.Clip:
								
									//	Add the playback time for this scene
									dDuration += this.Secondaries[i].GetSource().GetDuration();
									break;
									
								case TmaxMediaTypes.Page:
								case TmaxMediaTypes.Slide:
								case TmaxMediaTypes.Treatment:
									break;
							
							}
							
							//	Ignore the transition time if this is a playlist
							if(Playlist == false)
							{
								//	Is this an automatic transition scene?
								if(this.Secondaries[i].AutoTransition == true)
								{
									dDuration += this.Secondaries[i].TransitionTime;
								}
								else
								{
									//	If this isn't the last scene there's no
									//	way for us to know the total time with
									//	a manual transition being in the script
									if(i < (this.Secondaries.Count - 1))
										return -1.0;
								}
							
							}// if(Playlist == true)
							
						}
						else
						{
							return -1.0;
						}
						
					}// foreach(CDxSecondary O in this.Secondaries)
						
					break;

				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Powerpoint:
				default:
				
					break;
			}
			
			return dDuration;
		}
		
		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public override bool GetCanClean(bool bFill)
		{
			//	Must be a script or document
			switch(this.MediaType)
			{
				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Script:
				
					//	Does this record have any children
					if(this.ChildCount > 0)
					{
						//	Should we fill the child collection?
						if(this.Secondaries.Count == 0)
						{
							//	Assume the cleaning tools can be applied if not allowed to fill
							if(bFill == false)	
								return true;
							else
								this.Fill();
						
						}// if(this.Secondaries.Count == 0)
						
						//	Check to see if any of the children can be cleaned
						foreach(CDxSecondary O in this.Secondaries)
							if(O.GetCanClean(bFill) == true)
								return true;					
					
					}// if(this.ChildCount > 0)
					
					break;
					
			}
			
			return false;
			
		}// public override bool GetCanClean(bool bFill)
		
		/// <summary>This method is called to get the id of the property with the specified name</summary>
		/// <param name="strName">The property name</param>
		/// <returns>The associated identifier</returns>
		public override int GetPropertyId(string strName)
		{
			int iId = -1;
			
			//	Check the base class first
			if((iId = base.GetPropertyId(strName)) > 0)
			{
				return iId;
			}
			else
			{
				if(String.Compare(strName, "Admitted", true) == 0)
					return DXPRIMARY_PROP_ADMITTED;
				else if(String.Compare(strName, "Exhibit", true) == 0)
					return DXPRIMARY_PROP_EXHIBIT;
				else
					return -1;
			}
		
		}// public virtual int GetPropertyId(string strName)
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public override string GetText()
		{
			switch(MediaType)
			{
				case TmaxMediaTypes.Deposition:
				
					return GetText(TmaxTextModes.Name);
					
				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Powerpoint:
				case TmaxMediaTypes.Recording:
				case TmaxMediaTypes.Script:
				default:
				
					return GetText(TmaxTextModes.MediaId);
			}

		}// public override string GetText()
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		/// <param name="eMode">The desired TrialMax text mode</param>
		/// <returns>The text that represents this record</returns>
		public override string GetText(TmaxTextModes eMode)
		{
			string strText = "";
			
			if((eMode & TmaxTextModes.MediaId) == TmaxTextModes.MediaId)
			{
				strText = m_strMediaId;
			}
			
			if((eMode & TmaxTextModes.Name) == TmaxTextModes.Name)
			{
				if(m_strName.Length > 0)
				{
					if(strText.Length > 0)
						strText += " - ";
						
					strText += m_strName;
				}
			}
			
			if((eMode & TmaxTextModes.Exhibit) == TmaxTextModes.Exhibit)
			{
				if(this.Exhibit.Length > 0)
				{
					if(strText.Length > 0)
						strText += " - ";
						
					strText += this.Exhibit;
				}
			}
			
			if(strText.Length == 0)
				strText = m_strMediaId;
				
			return strText;
		}

		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxDeposition.ShowAs()
		{
			return this.GetText();
		}

		/// <summary>This method is called to set the Playlist attribute using the available children</summary>
		public void SetPlaylistFromChildren()
		{
			//	Only Scripts can be playlists
			if(MediaType != TmaxMediaTypes.Script)
			{
				Playlist = false;
			}
			else
			{			
				if((m_dxSecondaries != null) && (m_dxSecondaries.Count > 0))
				{
					foreach(CDxSecondary O in m_dxSecondaries)
					{
						if(O.MediaType == TmaxMediaTypes.Scene)
						{
							if((O.GetSource() == null) || (O.GetSource().MediaType != TmaxMediaTypes.Designation))
							{
								Playlist = false;
								return;
							}
								
						}
						else
						{
							Debug.Assert(false);
							Playlist = false;
							return;
						}
						
					}
					
					//	Must be a playlist
					Playlist = true;
				}
				else
				{
					Playlist = false;
				}
				
			}
			
		}// public void SetPlaylistFromChildren()
		
		/// <summary>This function is called to get the description associated with this record</summary>
		/// <returns>The description associated with this record</returns>
		public string GetExhibit(bool bIgnoreCode)
		{
			//	Does this record type use codes?
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((bIgnoreCode == false) && (this.AutoId > 0) && (GetCodes(false) != null))
			{
				return GetCodedPropValue(TmaxCodedProperties.Exhibit, false);
			}
			else
			{
				return m_strExhibit;
			}
			
		}// public virtual string GetExhibit()
		
		/// <summary>This function is called to set the description associated with this record</summary>
		/// <param name="strValue">The new value to be assigned to the description</param>
		public void SetExhibit(string strValue)
		{
			//	Does this record type use codes?
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((this.AutoId > 0) && (GetCodes(false) != null))
			{
				SetCodedPropValue(TmaxCodedProperties.Exhibit, strValue, false);
			}
			else
			{
				//	Set the local class member
				m_strExhibit = (strValue != null) ? strValue : "";
			}
			
		}// public virtual void SetExhibit(string strValue)
		
		/// <summary>This method will locate the segment with the specified XML identifier</summary>
		/// <param name="strXmlId">The xml key identifier</param>
		/// <returns>the segment with the specified XML identifier</returns>
		public CDxSecondary GetSegment(string strXmlId)
		{
			//	Make sure the secondaries have been populated
			if(Secondaries.Count == 0)
				this.Fill();
				
			//	Get the requested segment
			return Secondaries.Find(strXmlId);
			
		}// public CDxSecondary GetSegment(string strXmlId)
		
		#endregion Public Methods

		#region Protected Methods
		
		/// <summary>Called to exchange the coded property values with their associated class members</summary>
		/// <param name="bSetCodes">True to set the codes using the associated class members</param>
		/// <param name="bUnassigned">True to exchange unassigned codes/members</param>
		/// <param name="bUpdate">True to update the owner record time stamps</param>
		/// <param name="bRefresh">True to refresh the collection prior to performing the operation</param>
		/// <returns>True if successful</returns>
		public override bool ExchangeCodedProperties(bool bSetCodes, bool bUnassigned, bool bUpdate, bool bRefresh)
		{
			bool		bSuccessful = true;
			bool		bModified = false;
			
			//	Perform the base class processing first
			if(base.ExchangeCodedProperties(bSetCodes, bUnassigned, bUpdate, bRefresh) == false)
				return false;
			
			//	Are we setting the coded property values?
			if(bSetCodes == true)
			{
				//	Use Exhibit class member to set the code
				if((bUnassigned == true) || (m_strExhibit.Length > 0))
				{
					if(this.Exhibit != m_strExhibit)
					{
						this.Exhibit = m_strExhibit;
						m_strExhibit = "";
						bModified = true;
					}
				
				}
				
			}
			else
			{
				//	Use the Exhibit code value to set the class member
				if((bUnassigned == true) || (this.Exhibit.Length > 0))
				{	
					if(m_strExhibit != this.Exhibit)
					{
						m_strExhibit = this.Exhibit;
						bModified = true;
					}
					
				}					
			
			}// if(bSetCodes == true)
			
			//	Should we update the record?
			if((bUpdate == true) && (bModified == true) && (this.Collection != null))
			{
				this.Collection.Update(this);
			}
			
			return bSuccessful;
		
		}// public override bool ExchangeCodedProperties(bool bRefresh, bool bSetCodes, bool bIgnoreUnassigned)
		
		/// <summary>This method creates the Transcripts table exchange object</summary>
		/// <returns>true if successful</returns>
		protected bool CreateTranscripts()
		{
			//	Have we already created the collection?
			if(m_dxTranscripts != null) return true;
			
			try
			{
				//	Allocate the interface to the transcripts table
				m_dxTranscripts = new CDxTranscripts();
				m_dxTranscripts.Primary = this;
				if(m_dxCollection != null)
				{
                    m_dxTranscripts.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				}
			
				return true;
			}
			catch
			{
				m_dxTranscripts = null;
				return false;
			}

		}// protected virtual bool CreateTranscripts()
		
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>The id of the driver/server alias when linked</summary>
		public long AliasId
		{
			get	{ return m_lAliasId;  }
			set { m_lAliasId = value; }
		}
		
		/// <summary>Path relative to root folder or aliased drive when linked</summary>
		public string RelativePath
		{
			get { return m_strRelativePath; }
			set { m_strRelativePath = value;  }
		}
		
		/// <summary>The path to the secondary files at time of registration</summary>
		public string RegisterPath
		{
			get { return m_strRegisterPath; }
			set { m_strRegisterPath = value;  }
		}
		
		/// <summary>The unique alpha-numeric media identifier</summary>
		public string MediaId
		{
			get { return m_strMediaId;  }
			set { m_strMediaId = value; }
		}
		
		/// <summary>The unique Exhibit identifier</summary>
		public string Exhibit
		{
			get { return GetExhibit(false); }
			set { SetExhibit(value); }
		}
		
		/// <summary>The name of the first secondary file</summary>
		public string Filename
		{
			get	{ return m_strFilename; }
			set { m_strFilename = value; }
		}
		
		/// <summary>The unique alternate barcode</summary>
		public string AltBarcode
		{
			get	{ return m_strAltBarcode; }
			set { m_strAltBarcode = value; }
		}
		
		/// <summary>True if source media is linked</summary>
		public bool Linked
		{
			get	{ return GetLinked(); }
		}
		
		/// <summary>The collection of secondary media objects</summary>
		public CDxSecondaries Secondaries
		{
			get	{ return m_dxSecondaries; }
		}
		
		/// <summary>The interface to the Transcripts table</summary>
		public CDxTranscripts Transcripts
		{
			get { return m_dxTranscripts; }
		}
		
		/// <summary>The transcript record associated with this primary if available</summary>
		public CDxTranscript Transcript
		{
			get { return GetTranscript(); }
		}
		
		/// <summary>Flag to indicate primary was merged at registration</summary>
		public bool Merged
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxPrimaryAttributes.Merged) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxPrimaryAttributes.Merged;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxPrimaryAttributes.Merged);
				}
			
			}
		
		}
		
		/// <summary>Flag to indicate the primary script contains only deposition video</summary>
		public bool Playlist
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxPrimaryAttributes.Playlist) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxPrimaryAttributes.Playlist;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxPrimaryAttributes.Playlist);
				}
			
			}
		
		}
		
		#endregion Properties
	
	}// class CDxPrimary

	/// <summary>
	/// This class is used to manage a ArrayList of CDxPrimary objects
	/// </summary>
	public class CDxPrimaries : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			Children,
			Attributes,
			MediaType,
			MediaId,
			Exhibit,
			RegisterPath,
			AliasId,
			RelativePath,
			Filename,
			Description,
			AltBarcode,
			Name,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "PrimaryMedia";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to FastFiltered property</summary>
		private bool m_bFastFiltered = false;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxPrimaries() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxPrimaries(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxPrimary">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxPrimary Add(CDxPrimary dxPrimary)
		{
			return ((CDxPrimary)(base.Add(dxPrimary)));
			
		}// Add(CDxPrimary dxPrimary)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxPrimaries Dispose()
		{
			return (CDxPrimaries)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxPrimary Find(long lAutoId, bool bBarcode)
		{
			return (CDxPrimary)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxPrimary Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Called to locate the object with the specified MediaID value
		/// </summary>
		/// <returns>The object with the specified MediaID</returns>
		public CDxPrimary Find(string strMediaID, CDxPrimary dxIgnore)
		{
			foreach(CDxPrimary O in this)
			{
				if(String.Compare(strMediaID, O.MediaId, true) == 0)
				{
					//	Are we ignoring an object?
					if((dxIgnore == null) || (ReferenceEquals(O, dxIgnore) == false)) 
						return O;
				}
			}
			
			return null;
			
		}//	Find(string strMediaID)

		/// <summary>
		/// Called to locate the object with the specified MediaID value
		/// </summary>
		/// <returns>The object with the specified MediaID</returns>
		public CDxPrimary Find(string strMediaID)
		{
			return Find(strMediaID, null);
			
		}//	Find(string strMediaID)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxPrimary this[int iIndex]
		{
			get
			{
				return (CDxPrimary)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxPrimary GetAt(int iIndex)
		{
			return (CDxPrimary)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxPrimary	dxPrimary = (CDxPrimary)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.Children.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.MediaType.ToString() + ",");
			strSQL += (eFields.MediaId.ToString() + ",");
			strSQL += (eFields.Exhibit.ToString() + ",");
			strSQL += (eFields.RegisterPath.ToString() + ",");
			strSQL += (eFields.AliasId.ToString() + ",");
			strSQL += (eFields.RelativePath.ToString() + ",");
			strSQL += (eFields.Filename.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.AltBarcode.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + dxPrimary.ChildCount.ToString() + "',");
			strSQL += ("'" + dxPrimary.Attributes.ToString() + "',");
			strSQL += ("'" + ((int)(dxPrimary.MediaType)).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.MediaId) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.GetExhibit(true)) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.RegisterPath) + "',");
			strSQL += ("'" + dxPrimary.AliasId.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.RelativePath) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.Filename) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.GetDescription(true)) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.Name) + "',");
			strSQL += ("'" + SQLEncode(dxPrimary.AltBarcode) + "',");
			strSQL += ("'" + dxPrimary.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxPrimary.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxPrimary.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxPrimary.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxPrimary	dxPrimary = (CDxPrimary)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.Children.ToString() + " = '" + dxPrimary.ChildCount.ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxPrimary.Attributes.ToString() + "',");
			strSQL += (eFields.MediaType.ToString() + " = '" + ((int)dxPrimary.MediaType).ToString() + "',");
			strSQL += (eFields.MediaId.ToString() + " = '" + SQLEncode(dxPrimary.MediaId) + "',");
			strSQL += (eFields.Exhibit.ToString() + " = '" + SQLEncode(dxPrimary.GetExhibit(true)) + "',");
			strSQL += (eFields.RegisterPath.ToString() + " = '" + SQLEncode(dxPrimary.RegisterPath) + "',");
			strSQL += (eFields.AliasId.ToString() + " = '" + dxPrimary.AliasId.ToString() + "',");
			strSQL += (eFields.RelativePath.ToString() + " = '" + SQLEncode(dxPrimary.RelativePath) + "',");
			strSQL += (eFields.Filename.ToString() + " = '" + SQLEncode(dxPrimary.Filename) + "',");
			strSQL += (eFields.Description.ToString() + " = '" + SQLEncode(dxPrimary.GetDescription(true)) + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxPrimary.Name) + "',");
			strSQL += (eFields.AltBarcode.ToString() + " = '" + SQLEncode(dxPrimary.AltBarcode) + "',");
			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxPrimary.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxPrimary.CreatedOn.ToString() + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxPrimary.ModifiedBy.ToString() + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxPrimary.ModifiedOn.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxPrimary.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			CDxTranscripts dxTranscripts = null;
			
			if(base.Update(dxRecord) == false)
				return false;
				
			//	Is this a deposition?
			if(((CDxPrimary)dxRecord).MediaType == TmaxMediaTypes.Deposition)
			{
				dxTranscripts = ((CDxPrimary)dxRecord).Transcripts;
				
				/// Update the transcript if it exists
				if((dxTranscripts != null) && (dxTranscripts.Count > 0))
				{
					dxTranscripts.Update(dxTranscripts[0]);
				}
			}
			
			return true;
			
		}// public override bool Update(CBaseRecord dxRecord)
		
		/// <summary>This method will delete the specified record</summary>
		/// <param name="dxRecord">Object to be deleted</param>
		/// <returns>true if successful</returns>
		public override bool Delete(CBaseRecord dxRecord)
		{
			CDxTranscript dxTranscript = null;
			
			if(base.Delete(dxRecord) == false)
				return false;
				
			//	Is this a deposition?
			if(((CDxPrimary)dxRecord).MediaType == TmaxMediaTypes.Deposition)
			{
				dxTranscript = ((CDxPrimary)dxRecord).GetTranscript();
				
				/// Delete the transcript if it exists
				if((dxTranscript != null) && (dxTranscript.Collection != null))
				{
					dxTranscript.Collection.Delete(dxTranscript);
				}
			}
			
			return true;
			
		}
		
		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>-1 if error, 0 if successful, 1 if duplicate</returns>
		public int UpdateMediaId(CDxPrimary dxPrimary)
		{
			string strSQL = "";
			
			//	Make sure we have a valid connection
            if(this.Database.IsConnected == false) return -1;

			try
			{
				strSQL = "UPDATE " + TableName + " SET ";
				strSQL += (eFields.MediaId.ToString() + " = '" + SQLEncode(dxPrimary.MediaId) + "'");
				strSQL += " WHERE AutoId = ";
				strSQL += dxPrimary.AutoId.ToString();
				strSQL += ";";
			
				//	Execute the statement
                this.Database.Execute(strSQL);
				
				//	Everything went OK
				return 0;
			}
			catch(OleDbException)
			{
				//	Assume this means a duplicate id
				//
				//	NOTE: We can't use the exception code because the same code
				//		  gets returned for different errors. Only real way to
				//		  do this with 100% reliability is to look ahead and check
				//		  to see if there is a duplicate id before we add the record
				return 1;				
			}
			catch(System.Exception Ex)
			{
                FireError("UpdateMediaId",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_PRIMARIES_UPDATE_MEDIA_ID,dxPrimary.AutoId),Ex,GetErrorItems(dxPrimary));
			}
			
			return -1;	//	Error
			
		}// UpdateMediaId(CDxPrimary dxPrimary)

		/// <summary>Called to populate the collection using the specified filter</summary>
		/// <param name="tmaxFilter">The filter used to populate the collection</param>
		/// <returns>true if successful</returns>
		public bool Fill(CTmaxFilter tmaxFilter)
		{
			string		strSQL = "";
			object		O = null;
			long		lId = 0;
			CDxPrimary	dxPrimary = null;
			
			try
			{
				//	Clear the existing members
				this.Clear();
				
				//	Get the SQL statement
				if(tmaxFilter != null) 
				{
					//	Make sure the filter is using the latest case codes
					tmaxFilter.CaseCodes = this.Database.CaseCodes;
					tmaxFilter.PickLists = this.Database.PickLists;
					
					strSQL = tmaxFilter.GetSQL();
					
					this.FastFiltered = (tmaxFilter.Advanced == false);
				}
				else
				{
					this.FastFiltered = false;
				}
					
				//	Assume all primaries to be included if no filter
				if(strSQL.Length == 0)
				{
                    foreach(CDxPrimary dxp in this.Database.Primaries)
						this.AddList(dxp);
					return true;
				}
				
				//	Create a data reader
                if(this.Database.IsConnected == false) return false;
                if(this.Database.GetDataReader(strSQL) == false)
					return false;

				//	Read each of the return values
                while(this.Database.AdvanceReader() == true)
				{
					//	The query results return the PrimaryId of matching records
                    if((O = this.Database.GetValue(0)) != null)
					{
						//	This is the AutoId of the record
						lId = (int)O;
						
						//	Get the actual value that's already been loaded by the database
                        if((dxPrimary = this.Database.Primaries.Find(lId,false)) != null)
						{
							this.AddList(dxPrimary);
						}
						
					}

                    if(this.Database.IsConnected == false) break;
						
				}// while(m_tmaxDatabase.AdvanceReader() == true)
					
				//	Release the reader
                this.Database.FreeDataReader();

				return true;
			
			}
			catch(OleDbException oleEx)
			{
                FireError("Fill",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_PRIMARIES_FILL_FILTERED_EX,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Fill",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_PRIMARIES_FILL_FILTERED_EX,strSQL),Ex);
			}
			
			return false;	//	Error
			
		}// public bool SetFilter(CTmaxFilter tmaxFilter)
		
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
			return ((CBaseRecord)(new CDxPrimary()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxPrimary dxPrimary = (CDxPrimary)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxPrimary.AutoId;
					m_dxFields[(int)eFields.Children].Value = dxPrimary.ChildCount;
					m_dxFields[(int)eFields.Attributes].Value = dxPrimary.Attributes;
					m_dxFields[(int)eFields.MediaType].Value = dxPrimary.MediaType;
					m_dxFields[(int)eFields.MediaId].Value = dxPrimary.MediaId;
					m_dxFields[(int)eFields.Exhibit].Value = dxPrimary.Exhibit;
					m_dxFields[(int)eFields.RegisterPath].Value = dxPrimary.RegisterPath;
					m_dxFields[(int)eFields.AliasId].Value = dxPrimary.AliasId;
					m_dxFields[(int)eFields.RelativePath].Value = dxPrimary.RelativePath;
					m_dxFields[(int)eFields.Filename].Value = dxPrimary.Filename;
					m_dxFields[(int)eFields.Description].Value = dxPrimary.Description;
					m_dxFields[(int)eFields.AltBarcode].Value = dxPrimary.AltBarcode;
					m_dxFields[(int)eFields.Name].Value = dxPrimary.Name;
					m_dxFields[(int)eFields.CreatedBy].Value = dxPrimary.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxPrimary.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxPrimary.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxPrimary.ModifiedOn;
				}
				else
				{
					dxPrimary.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxPrimary.ChildCount = (int)(m_dxFields[(int)eFields.Children].Value);
					dxPrimary.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxPrimary.MediaType = (FTI.Shared.Trialmax.TmaxMediaTypes)((short)(m_dxFields[(int)eFields.MediaType].Value));
					dxPrimary.MediaId = (string)(m_dxFields[(int)eFields.MediaId].Value);
					dxPrimary.Exhibit = (string)(m_dxFields[(int)eFields.Exhibit].Value);
					dxPrimary.RegisterPath = (string)(m_dxFields[(int)eFields.RegisterPath].Value);
					dxPrimary.AliasId = (int)(m_dxFields[(int)eFields.AliasId].Value);
					dxPrimary.RelativePath = (string)(m_dxFields[(int)eFields.RelativePath].Value);
					dxPrimary.Filename = (string)(m_dxFields[(int)eFields.Filename].Value);
					dxPrimary.Description = (string)(m_dxFields[(int)eFields.Description].Value);
					dxPrimary.AltBarcode = (string)(m_dxFields[(int)eFields.AltBarcode].Value);
					dxPrimary.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxPrimary.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxPrimary.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxPrimary.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxPrimary.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
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
				case eFields.AliasId:
				case eFields.Children:
				case eFields.Attributes:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.MediaType:
				
					dxField.Value = TmaxMediaTypes.Unknown;
					break;
					
				case eFields.Name:
				case eFields.AltBarcode:
				case eFields.MediaId:
				case eFields.Exhibit:
				case eFields.RegisterPath:
				case eFields.RelativePath:
				case eFields.Filename:
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
		
		/// <summary>True if fast filter was used to populate the collection</summary>
		public bool FastFiltered
		{
			get { return m_bFastFiltered; }
			set { m_bFastFiltered = value; }
		}
		
		#endregion Properties
		
	}//	CDxPrimaries
		
}// namespace FTI.Trialmax.Database
