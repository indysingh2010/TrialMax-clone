using System;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using System.Collections.Generic;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class is used to manage the exchange of data between the application
	/// and individual records in the BinderEntries table
	/// </summary>
	public class CDxBinderEntry : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXBINDER_PROP_FIRST_ID		= DX_MEDIA_RECORD_PROP_LAST + 12000;

		protected const int DXBINDER_PROP_AUTOID		= DXBINDER_PROP_FIRST_ID + 1;
		protected const int DXBINDER_PROP_PARENT_ID		= DXBINDER_PROP_FIRST_ID + 2;
		protected const int DXBINDER_PROP_ATTRIBUTES	= DXBINDER_PROP_FIRST_ID + 3;
		protected const int DXBINDER_PROP_DISPLAY_ORDER	= DXBINDER_PROP_FIRST_ID + 4;
		protected const int DXBINDER_PROP_CHILDREN		= DXBINDER_PROP_FIRST_ID + 5;
		protected const int DXBINDER_PROP_NAME			= DXBINDER_PROP_FIRST_ID + 6;
		protected const int DXBINDER_PROP_CREATED_BY	= DXBINDER_PROP_FIRST_ID + 7;
		protected const int DXBINDER_PROP_CREATED_ON	= DXBINDER_PROP_FIRST_ID + 8;
		protected const int DXBINDER_PROP_MODIFIED_BY	= DXBINDER_PROP_FIRST_ID + 9;
		protected const int DXBINDER_PROP_MODIFIED_ON	= DXBINDER_PROP_FIRST_ID + 10;
		protected const int DXBINDER_PROP_MEDIA_TYPE	= DXBINDER_PROP_FIRST_ID + 11;
		protected const int DXBINDER_PROP_PATH			= DXBINDER_PROP_FIRST_ID + 12;

		#endregion Constants

		#region Private Members
		
		/// <summary>Local member bound to Contents property</summary>
		private CDxBinderEntries m_dxContents = new CDxBinderEntries();
		
		/// <summary>Local member bound to Parent property</summary>
		private CDxBinderEntry m_dxParent = null;
		
		/// <summary>Local member bound to Source property</summary>
		private CDxMediaRecord m_dxSource = null;
		
		/// <summary>Local member bound to ParentId property</summary>
		private long m_lParentId = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxBinderEntry() : base()
		{
			m_dxContents.Parent = this;
		}
		
		/// <summary>Constructor</summary>
		public CDxBinderEntry(CDxBinderEntry dxParent) : base()
		{
			this.Parent = dxParent;
			m_dxContents.Parent = this;
		}
		
		/// <summary>This method is called to fill the child collection</summary>
		/// <returns>true if successful</returns>
		override public bool Fill()
		{
			bool bSuccessful = false;
			
			if(m_dxContents != null)
			{
				//	Make sure the collection is using the correct database
				if(m_dxCollection != null)
					m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				if(m_dxContents.Database == null)
					return false;
						
				//	Clear the existing objects
				m_dxContents.Clear();
			
				//	Get the child records from the database			
				if((bSuccessful = m_dxContents.Fill()) == true)
				{					
					//	We need to get the exchange object for each media child
					foreach(CDxBinderEntry O in m_dxContents)
					{
						O.Parent = this;
						O.GetSource(true);
					}
				}
			
				//	Were we able to fill the child collection?
				if(bSuccessful == true)
				{
					//	Just in case something messed up..........
					//
					//	NOTE:	We add the check for AutoId > 0 because this method may
					//			be getting called to populate the root binder maintained
					//			by the CTmaxCaseDatabase instance
					if((this.AutoId > 0) && (this.ChildCount != m_dxContents.Count))
					{
						//Debug.Assert(this.ChildCount == m_dxContents.Count, this.Name + " -> ChildCount: " + this.ChildCount.ToString() + " != Contents: " + this.Contents.Count.ToString());
						
						if(Collection != null)
						{
							this.ChildCount = m_dxContents.Count;
							Collection.Update(this);
						}
						
					}// if(this.ChildCount != m_dxContents.Count)
					
				}// if(bSuccessful == true)
				
			}

			return bSuccessful;
			
		}// public bool Fill()
		
		/// <summary>This method is called to get the record exchange object for the entry's source media</summary>
		/// <param name="bSilent">True to supress error message if source doesn't exist</param>
		///	<returns>The record exchange object that this entry references</returns>
		public CDxMediaRecord GetSource(bool bSilent)
		{
			//	Don't bother if not a media entry
			if(IsMedia() == false) return null;
			
			//	Have we already located the source record?
			if(m_dxSource != null) return m_dxSource;
			
			//	Do we have a valid id
			if((m_strName != null) && (m_strName.Length > 0))
			{
				//	Do we have access to the database?
				if(this.Database != null)
					m_dxSource = this.Database.GetRecordFromId(m_strName, bSilent);
			}
			
			return m_dxSource;
			
		}// public CDxMediaRecord GetSource()
		
		/// <summary>This method is called to determine if this object references a registered media object</summary>
		/// <returns>true if successful</returns>
		public bool IsMedia()
		{
			return ((m_lAttributes & (long)TmaxBinderAttributes.IsMedia) != 0);
		}
		
		/// <summary>This method is called to add a new binder to the contents collection</summary>
		/// <param name="dxEntry">The binder to be added</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxBinderEntry dxEntry)
		{
			if(m_dxContents != null)
			{
				//	Make sure the collection is using the correct database
				if(m_dxCollection != null)
					m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				if(m_dxContents.Database == null)
					return false;
					
				//	Make sure it has the correct parent information
				dxEntry.SetParent(this);
				
				return (m_dxContents.Add(dxEntry) != null);
			}
			else
			{
				return false;
			}
		
		}// public bool Add(CDxBinderEntry dxEntry)
		
		/// <summary>This method is called to update an entry in the child contents collection</summary>
		/// <param name="dxEntry">Record exchange object associated with the entry being updated</param>
		/// <returns>true if successful</returns>
		public bool Update(CDxBinderEntry dxEntry)
		{
			if(m_dxContents != null)
			{
				//	Make sure the secondary collection is using the correct database
				if(m_dxCollection != null)
                    m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
				if(m_dxContents.Database == null)
					return false;
					
				//	Make sure it has the correct parent information
				dxEntry.SetParent(this);
				
				return m_dxContents.Update(dxEntry);
			}
			else
			{
				return false;
			}
		
		}// public bool Update(CDxBinderEntry dxEntry)
		
		/// <summary>This method is called to delete an entry stored in the child contents collection</summary>
		/// <param name="dxEntry">Entry object to be deleted</param>
		/// <returns>true if successful</returns>
		public bool Delete(CDxBinderEntry dxEntry)
		{
			int	iIndex = -1;
			
			Debug.Assert(m_dxContents != null);
			if(m_dxContents == null) return false;
			
			//	Make sure this secondary exists in the collection
			if((iIndex = m_dxContents.IndexOf(dxEntry)) < 0) return false;
			
			//	Make sure the secondary collection is using the correct database
			if(m_dxCollection != null)
                m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
			if(m_dxContents.Database == null)
				return false;
				
			if(m_dxContents.Delete(dxEntry) == true)
			{
				//	Update the display order of remaining records
				for(int i = iIndex; i < m_dxContents.Count; i++)
				{
					m_dxContents[i].DisplayOrder = (i + 1);
					m_dxContents.Update(m_dxContents[i]);
				}
						
				//	Update this record
				this.ChildCount = m_dxContents.Count;
				if(m_dxCollection != null)
					m_dxCollection.Update(this);
						
				return true;
			}

			//	Must have been an error
			return false;
		
		}// public bool Delete(CDxBinderEntry dxEntry)
		
		/// <summary>This function is called to delete the specified child record</summary>
		public override bool Delete(CDxMediaRecord dxChild)
		{
			return Delete((CDxBinderEntry)dxChild);
		}

        /// <summary>This function is called to delete the specified child record</summary>
        public override void ResetDisplayOrder()
        {
            int iIndex = -1;

            Debug.Assert(m_dxContents != null);
            if (m_dxContents == null) return;

            //	Make sure the secondary collection is using the correct database
            if (m_dxCollection != null)
                m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
            if (m_dxContents.Database == null)
                return ;

                //	Update the display order of remaining records
                for (int i = 0; i < m_dxContents.Count; i++)
                {
                   // m_dxContents[i].DisplayOrder = (i + 1);
                    m_dxContents.Update(m_dxContents[i]);
                }

                //	Update this record
                this.ChildCount = m_dxContents.Count;
                if (m_dxCollection != null)
                    m_dxCollection.Update(this);

        }

        /// <summary>This function is called to delete the specified child record</summary>
        public override void ResetSortProperty()
        {
            int iIndex = -1;

            Debug.Assert(m_dxContents != null);
            if (m_dxContents == null) return;

            //	Make sure the secondary collection is using the correct database
            if (m_dxCollection != null)
                m_dxContents.Database = (CTmaxCaseDatabase)(m_dxCollection.Database);
            if (m_dxContents.Database == null)
                return;

            //	Update the display order of remaining records
            for (int i = 0; i < m_dxContents.Count; i++)
            {
                // m_dxContents[i].DisplayOrder = (i + 1);
                m_dxContents.Update(m_dxContents[i]);
            }

            //	Update this record
            if (m_dxCollection != null)
                m_dxCollection.Update(this);

        }
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Binder;
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.None;
		}
		
		/// <summary>This function is called to get the name associated with this record</summary>
		/// <returns>The name associated with this record</returns>
		public override string GetName()
		{
			return GetName(false);
		}
		
		/// <summary>This function is called to get the name associated with this record</summary>
		/// <param name="bIgnoreSource">True to ignore the source record</param>
		/// <returns>The name associated with this record</returns>
		public string GetName(bool bIgnoreSource)
		{
			if(this.IsMedia() && (bIgnoreSource == false))
			{
				if(GetSource(true) != null)
					return GetSource(true).GetName();
				else
					return m_strName;
			}
			else
			{
				return m_strName;
			}
			
		}// public string GetName(bool bIgnoreSource)

        public override string GetMediaId()
        {
            return GetMediaId(false);
        }

        /// <summary>This function is called to get the name associated with this record</summary>
        /// <param name="bIgnoreSource">True to ignore the source record</param>
        /// <returns>The name associated with this record</returns>
        public string GetMediaId(bool bIgnoreSource)
        {
            if (this.IsMedia() && (bIgnoreSource == false))
            {
                if (GetSource(true) != null)
                    return GetSource(true).GetMediaId();
                else
                    return m_strName;
            }
            else
            {
                return m_strName;
            }

        }// public string GetName(bool bIgnoreSource)

        protected override CDxCode GetCode(CTmaxCaseCode caseCode)
        {
            if (this.IsMedia())
            {
                CDxCodes code = new CDxCodes(this.Database);
                CDxMediaRecord cDxMediaRecord = this.GetSource(true);
                if (cDxMediaRecord==null) return null;
                if (cDxMediaRecord.GetType() == typeof (CDxSecondary))
                    code.Fill(caseCode, cDxMediaRecord.GetParent());
                else if (cDxMediaRecord.GetType() == typeof(CDxTertiary))
                {
                    CDxMediaRecord cDxParentMediaRecord = cDxMediaRecord.GetParent();
                    if (cDxParentMediaRecord != null)
                        code.Fill(caseCode, cDxParentMediaRecord.GetParent());
                }
                    
                else
                    code.Fill(caseCode, cDxMediaRecord);
                return code[0];
            }
            return null;
        }


	    /// <summary>This function is called to get the description associated with this record</summary>
		/// <returns>The description associated with this record</returns>
		public override string GetDescription(bool bIgnoreCode)
		{
			if((m_strDescription.Length > 0) || (bIgnoreCode == true))
			{
				return m_strDescription;
			}
			else
			{
				if(this.Database != null)
					return this.Database.ExpandBinderPath(GetParentPathId());
				else
					return GetParentPathId();

			}
		
		}// public override string GetDescription()
		
		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public override CDxMediaRecord GetParent()
		{
			return m_dxParent;
		}
		
		/// <summary>This function is called to get the AutoId of the parent record</summary>
		/// <returns>The autoid assigned to the parent if it exists</returns>
		public override long GetParentId()
		{
			return m_lParentId;
			
		}// public override long GetParentId()
		
		/// <summary>This function is called to get the collection of child records</summary>
		/// <returns>The child collection if it exists</returns>
		public override CDxMediaRecords GetChildCollection()
		{
			return m_dxContents;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	Is this a media entry?
			if(GetSource(false) != null)
			{
				GetSource(false).GetProperties(tmaxProperties);
				
				tmaxProperties.Add(DXBINDER_PROP_AUTOID, "Binder Id", m_lAutoId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_PARENT_ID, "Binder Parent Id", m_lParentId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_PATH, "Parent Binder Path", GetParentPathId(), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_DISPLAY_ORDER, "Binder Display Order", m_lDisplayOrder, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_NAME, "Binder Source Id", m_strName, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

				tmaxProperties.Add(DXBINDER_PROP_CREATED_BY, "Binder Created By", this.Database.GetUserName(m_lCreatedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_CREATED_ON, "Binder Created On", m_tsCreatedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_MODIFIED_BY, "Binder Modified By", this.Database.GetUserName(m_lModifiedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_MODIFIED_ON, "Binder Modified On", m_tsModifiedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			}
			else
			{
				tmaxProperties.Add(DXBINDER_PROP_AUTOID, "Database Id", m_lAutoId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_PARENT_ID, "Parent Id", m_lParentId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_PATH, "Parent Binder Path", GetParentPathId(), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_ATTRIBUTES, "Attributes", m_lAttributes, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_CREATED_BY, "Created By", this.Database.GetUserName(m_lCreatedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_CREATED_ON, "Created On", m_tsCreatedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_MODIFIED_BY, "Modified By", this.Database.GetUserName(m_lModifiedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_MODIFIED_ON, "Modified On", m_tsModifiedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_CHILDREN, "Children", m_lChildCount, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

				tmaxProperties.Add(DXBINDER_PROP_MEDIA_TYPE, "Media Type", "Binder", TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				tmaxProperties.Add(DXBINDER_PROP_DISPLAY_ORDER, "Display Order", m_lDisplayOrder, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
				tmaxProperties.Add(DX_MEDIA_RECORD_PROP_NAME, "Name", m_strName, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
				tmaxProperties.Add(DX_MEDIA_RECORD_PROP_DESCRIPTION, "Description", this.Description, TmaxPropertyCategories.Media, TmaxPropGridEditors.Memo);

			}
			
		}// public override void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXBINDER_PROP_ATTRIBUTES:
				
					tmaxProperty.Value = m_lAttributes;
					break;
					
				case DXBINDER_PROP_MODIFIED_BY:
				
					if(this.Database != null)
						tmaxProperty.Value = this.Database.GetUserName(m_lModifiedBy);
					break;
					
				case DXBINDER_PROP_MODIFIED_ON:
				
					tmaxProperty.Value = m_tsModifiedOn;
					break;
					
				case DXBINDER_PROP_PARENT_ID:
				
					tmaxProperty.Value = m_lParentId;
					break;
					
				case DXBINDER_PROP_PATH:
				
					tmaxProperty.Value = GetParentPathId();
					break;
					
				case DXBINDER_PROP_DISPLAY_ORDER:
				
					tmaxProperty.Value = m_lDisplayOrder;
					break;
					
				case DXBINDER_PROP_CHILDREN:
				
					tmaxProperty.Value = m_lChildCount;
					break;
					
				//	These properties are read-only
				case DXBINDER_PROP_AUTOID:
				case DXBINDER_PROP_CREATED_BY:
				case DXBINDER_PROP_CREATED_ON:
				case DXBINDER_PROP_MEDIA_TYPE:
				
					break;
					
				default:
				
					//	This could be a source record property
					if(GetSource(false) != null)
						GetSource(false).RefreshProperty(tmaxProperty);
					else
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
			//	Is this a media entry?
			if(IsMedia() == true)
			{
				//	Use the media record to set the property
				if((m_dxSource == null) && (this.Database != null))
					m_dxSource = this.Database.GetRecordFromId(m_strName, false);
					
				if(m_dxSource != null)
					return m_dxSource.SetProperty(iId, strValue, bConfirmed, strMessage);
				else
					return false;
			}
			else
			{
				//	Let the base class handle it
				return base.SetProperty(iId, strValue, bConfirmed, strMessage);
			}
						
		}// public override bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		
		/// <summary>This function is called to get the barcode that identifies this record</summary>
		/// <param name="bIgnoreMapped">The true to ignore the record's foreign barcode if mapped</param>
		/// <returns>The record's barcode</returns>
		public override string GetBarcode(bool bIgnoreMapped)
		{
			string			strBarcode = "";
			CDxMediaRecord	dxSource = null;
			
			if((this.Database != null) && (this.IsMedia() == true))
				dxSource = this.GetSource(true);
				
			if(dxSource != null)
				strBarcode = this.Database.GetBarcode(dxSource, bIgnoreMapped);
			
			return strBarcode;
		
		}// public override string GetBarcode(bool bIgnoreMapped)
		
		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public override bool GetCanClean(bool bFill)
		{
			//	If this is a media entry call it's method
			if(this.IsMedia() == true)
			{
				if(this.GetSource(true) != null)
					return this.GetSource(true).GetCanClean(bFill);
				else
					return false;
			}
			else
			{
				//	Nothing to clean if there are no children
				if(this.ChildCount == 0) return false;
			}
			
			//	Should we fill the child collection?
			if(this.Contents.Count == 0)
			{
				//	Assume the cleaning tools can be applied if not allowed to fill
				if(bFill == false)	
					return true;
				else
					this.Fill();
						
			}// if(this.Contents.Count == 0)
					
			//	Check to see if any of the contents can be cleaned
			foreach(CDxBinderEntry O in this.Contents)
			{
				//	We don't drill down into subinders for this test
				if((O.IsMedia() == true) && (O.GetSource(true) != null))
				{
					if(O.GetSource(true).GetCanClean(bFill) == true)
						return true;
				}
				
			}// foreach(CDxBinderEntry O in this.Contents)				

			return false;
			
		}// public override bool GetCanClean(bool bFill)
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		/// <param name="eMode">The desired TrialMax text mode</param>
		/// <returns>The text that represents this record</returns>
		public override string GetText(TmaxTextModes eMode)
		{
			//	Is this a media entry?
			if((this.IsMedia() == true) && (m_dxSource != null))
			{
				return m_dxSource.GetText(eMode);
			}
			else
			{
				return m_strName;

			}

		}// public override string GetText(TmaxTextModes eMode)
					
		/// <summary>This method is called to assign the parent binder</summary>
		/// <param name="dxParent">The exchange interface for the parent binder</param>
		/// <returns>True if successful</returns>
		public bool SetParent(CDxBinderEntry dxParent)
		{
			this.m_dxParent = dxParent;
			
			if(dxParent != null)
			{
				this.ParentId = dxParent.AutoId;
				this.Collection = dxParent.Contents;
			}
			else
			{
				this.ParentId = 0;
			}
			
			return true;
			
		}// public bool SetParent(CDxBinderEntry dxParent)
		
		/// <summary>This method is called to get the id path to this binder's parent</summary>
		/// <returns>The path to this entry</returns>
		public string GetParentPathId()
		{
			CDxMediaRecord	dxParent = GetParent();
			string			strPath  = "";
			
			if((dxParent != null) && (dxParent.AutoId > 0))
			{
				strPath = dxParent.AutoId.ToString();
				
				while((dxParent = dxParent.GetParent()) != null)
				{
					if(dxParent.AutoId > 0)
						strPath = (dxParent.AutoId.ToString() + "." + strPath);
					else
						break;
				}
			
			}// if((dxParent != null) && (dxParent.AutoId > 0))
			
			return strPath;
			
		}// public string GetParentPathId()
		
		/// <summary>This method is called to get the parent record from the database</summary>
		/// <returns>True if successful</returns>
		public bool QueryForParent()
		{
			//	Has the parent already been assigned?
			if(this.Parent != null) return true;
			
			//	Don't need a parent if at the root level
			if(this.ParentId <= 0) return true;

			try
			{
				//	Retrieve the parent record from the table
				CDxBinderEntries dxEntries = new CDxBinderEntries(this.Database);
				dxEntries.Fill(this.ParentId);
				if(dxEntries.Count != 0)
				{
					//	Set the parent for this entry
					SetParent(dxEntries[0]);
					
					//	Put this entry in the parent's collection
					this.Parent.Contents.Database = dxEntries.Database;
					this.Parent.Contents.AddList(this);
					
					dxEntries.Clear();
				}
				dxEntries = null;
			}
			catch
			{
			}
			
			return (this.Parent != null);		
			
		}// public bool QueryForParent()
		
		/// <summary>This method is called to get the complete parent chain from the database</summary>
		/// <returns>True if successful</returns>
		public bool QueryForParents()
		{
			//	Get the parent for this record
			if(QueryForParent() == false) return false;
			
			//	Should we keep going up the chain?
			if(this.Parent != null)
				return this.Parent.QueryForParents();
			else
				return true;
						
		}// public bool QueryForParents()
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The collection of sub-binders and media objects that are contained in the binder</summary>
		public CDxBinderEntries Contents
		{
			get { return m_dxContents; }
		
		}//	Contents property
		
		/// <summary>The record exchange object associated with the Parent binder</summary>
		public CDxBinderEntry Parent
		{
			get { return m_dxParent; }
			set { m_dxParent = value;  }
		
		}//	Parent property
		
		/// <summary>The record exchange object associated with this object if it represents registered media</summary>
		public CDxMediaRecord Source
		{
			get { return GetSource(true); }
			set { m_dxSource = value; }

		}//	Source property
		
		/// <summary>The AutoId value assigned to the parent binder</summary>
		public long ParentId
		{
			get { return m_lParentId; }
			set { m_lParentId = value; }
		}
		
		/// <summary>The id path from the root to this binder</summary>
		public string ParentPathId
		{
			get { return GetParentPathId(); }
		}
		
		#endregion Properties
	
	}// class CDxBinderEntry

    // Custom Comparer, sort by DisplayOrder
    public class DisplayOrderComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null || y == null || ReferenceEquals(x, y) ||
                !(x is CDxBinderEntry) || !(y is CDxBinderEntry))
                return 0;
            CDxBinderEntry xObj = (CDxBinderEntry)x;
            CDxBinderEntry yObj = (CDxBinderEntry)y;
            if (xObj.DisplayOrder > yObj.DisplayOrder)
                return 1;
            else if (xObj.DisplayOrder < yObj.DisplayOrder)
                return -1;
            return 0;
        }
    }
   
    public class MediaPrintJob 
    {
        public long ParentId;
        public CDxMediaRecords CBaseRecords;
        public int OrderIndex;
        public MediaPrintJob()
        {
            
        }
        public MediaPrintJob(long binderEntryId)
        {
            this.ParentId = binderEntryId;
        }
        public MediaPrintJob(long binderEntryId, CDxMediaRecords cBaseRecords)
        {
            this.ParentId = binderEntryId;
            this.CBaseRecords = cBaseRecords;
        }

        public void AddMediaRecord(CDxMediaRecord cDxMediaRecord)
        {
            if(CBaseRecords==null)
                CBaseRecords=new CDxMediaRecords();
            CBaseRecords.AddList(cDxMediaRecord);
        }
    }

    public class MediaPrintJobs:List<MediaPrintJob>
    {
        public void Add(MediaPrintJob mediaPrintJob)
        {
            if (this.Count == 0)
                mediaPrintJob.OrderIndex = 1;
            else
                mediaPrintJob.OrderIndex = this[this.Count - 1].OrderIndex + 1;
            base.Add(mediaPrintJob);
        }
    }

    public class CustomComparer : IComparer<MediaPrintJob>
    {
        public int Compare(MediaPrintJob x, MediaPrintJob y)
        {
            if (x.OrderIndex > y.OrderIndex)
                return 1;
            else if (x.OrderIndex < y.OrderIndex)
                return -1;
            return 0;
        }

    }

        /// <summary>
	/// This class is used to manage a ArrayList of CDxBinderEntry objects
	/// </summary>
	public class CDxBinderEntries : CDxMediaRecords
	{
		public enum eFields
		{
			AutoId = 0,
			ParentId,
			Path,
			Children,
			Attributes,
			Name,
			Description,
			DisplayOrder,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
            SpareText,
            SpareNumber,
		}

		public const string TABLE_NAME = "BinderEntries";
		
		#region Private Members
		
		/// <summary>Local member bound to Parent property</summary>
		private CDxBinderEntry m_dxParent = null;
		
		/// <summary>Local member bound to FillFromSource property</summary>
		private bool m_bFillFromSource = false;
		
		/// <summary>Local member bound to SourceId property</summary>
		private string m_strSourceId = "";
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxBinderEntries() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxBinderEntries(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxEntry">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxBinderEntry Add(CDxBinderEntry dxEntry)
		{
			return (CDxBinderEntry)base.Add(dxEntry);

		}// Add(CDxBinderEntry dxEntry)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxBinderEntries Dispose()
		{
			return (CDxBinderEntries)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxBinderEntry Find(long lAutoId, bool bBarcode)
		{
			return (CDxBinderEntry)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxBinderEntry Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxBinderEntry this[int iIndex]
		{
			get
			{
				return (CDxBinderEntry)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxBinderEntry GetAt(int iIndex)
		{
			return (CDxBinderEntry)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxBinderEntry	dxEntry = (CDxBinderEntry)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";

			strSQL += (eFields.ParentId.ToString() + ",");
			strSQL += (eFields.Path.ToString() + ",");
			strSQL += (eFields.Children.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.DisplayOrder.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + dxEntry.ParentId.ToString() + "',");
			
			//===========================================================
			//	NOTE: As of version 6.1.0 the Path field no longer used
			//strSQL += ("'" + SQLEncode(dxEntry.ParentPath) + "',");
			strSQL += ("'" + "" + "',");
			//===========================================================
			
			strSQL += ("'" + dxEntry.ChildCount.ToString() + "',");
			strSQL += ("'" + dxEntry.Attributes.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxEntry.GetName(true)) + "',");
			strSQL += ("'" + SQLEncode(dxEntry.GetDescription(true)) + "',");
			strSQL += ("'" + dxEntry.DisplayOrder.ToString() + "',");
			strSQL += ("'" + dxEntry.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxEntry.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxEntry.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxEntry.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to flush all records belonging to the collection
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = "DELETE FROM ";
			
			if(m_dxParent != null)
			{
				strSQL += TableName;
				strSQL += " WHERE ParentId = ";
				strSQL += m_dxParent.AutoId.ToString();
				strSQL += ";";
				
				return strSQL;
			}
			else
			{
				// Should not be called without parent defined
				Debug.Assert(m_dxParent != null);
				return "";
			}
		
		}
		
		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
        public override string GetSQLUpdate(CBaseRecord dxRecord)
        {
            CDxBinderEntry dxEntry = (CDxBinderEntry)dxRecord;
            string strSQL = "UPDATE " + TableName + " SET ";

            strSQL += (eFields.ParentId.ToString() + " = '" + dxEntry.ParentId.ToString() + "',");
            //strSQL += (eFields.Path.ToString() + " = '" + SQLEncode(dxEntry.ParentPath) + "',");
            strSQL += (eFields.Children.ToString() + " = '" + dxEntry.ChildCount.ToString() + "',");
            strSQL += (eFields.Attributes.ToString() + " = '" + dxEntry.Attributes.ToString() + "',");
            strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxEntry.GetName(true)) + "',");
            strSQL += (eFields.Description.ToString() + " = '" + SQLEncode(dxEntry.GetDescription(true)) + "',");
            strSQL += (eFields.DisplayOrder.ToString() + " = '" + dxEntry.DisplayOrder.ToString() + "',");
            strSQL += (eFields.CreatedBy.ToString() + " = '" + dxEntry.CreatedBy.ToString() + "',");
            strSQL += (eFields.CreatedOn.ToString() + " = '" + dxEntry.CreatedOn.ToString() + "',");
            strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxEntry.ModifiedBy.ToString() + "',");

            if (!string.IsNullOrEmpty(dxEntry.SpareText))
                strSQL += (eFields.SpareText.ToString() + " = '" + dxEntry.SpareText.ToString() + "',");
            if (dxEntry.SpareNumber != null && dxEntry.SpareNumber != 0)
                strSQL += (eFields.SpareNumber.ToString() + " = '" + dxEntry.SpareNumber.ToString() + "',");
            strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxEntry.ModifiedOn.ToString() + "'");
            strSQL += " WHERE AutoId = ";
            strSQL += dxEntry.AutoId.ToString();
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
			
			if((m_bFillFromSource == true) && (m_strSourceId.Length > 0))
			{
				strSQL += " WHERE ((" + eFields.Name.ToString() + " = '" + m_strSourceId + "'";
				strSQL += " OR ";
				strSQL += eFields.Name.ToString() + " LIKE '" + m_strSourceId + ".%')";
				
				strSQL += " AND (";
				strSQL += eFields.Attributes.ToString() + " > 0 ))";

				strSQL += " ORDER BY " + eFields.ParentId.ToString() + "," + eFields.DisplayOrder.ToString();
				strSQL += ";";
			}
			else if(m_dxParent != null)
			{
				strSQL += " WHERE ParentId = ";
				strSQL += m_dxParent.AutoId.ToString();
				strSQL += " ORDER BY ";
				strSQL += eFields.DisplayOrder.ToString();
				strSQL += ";";
			}
			else
			{
				strSQL += " WHERE ParentId = 0";
				strSQL += " ORDER BY ";
				strSQL += eFields.DisplayOrder.ToString();
				strSQL += ";";
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
			return ((CBaseRecord)(new CDxBinderEntry()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxBinderEntry	dxEntry = (CDxBinderEntry)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//=============================================================================
				//	NOTE:	As of version 6.1.0 we no longer retrieve path from the database
				//			This was done to make it more efficient when moving binders from
				//			one parent to another. If we retrieved the path from the database
				//			we would have to drill all children when it's parent was moved
				//=============================================================================

				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxEntry.AutoId;
					m_dxFields[(int)eFields.ParentId].Value = dxEntry.ParentId;
					//m_dxFields[(int)eFields.Path].Value = dxEntry.Path;
					m_dxFields[(int)eFields.Path].Value = "";
					m_dxFields[(int)eFields.Children].Value = dxEntry.ChildCount;
					m_dxFields[(int)eFields.Attributes].Value = dxEntry.Attributes;
					m_dxFields[(int)eFields.Name].Value = dxEntry.GetName(true);
					m_dxFields[(int)eFields.Description].Value = dxEntry.GetDescription(true);
					m_dxFields[(int)eFields.DisplayOrder].Value = dxEntry.DisplayOrder;
					m_dxFields[(int)eFields.CreatedBy].Value = dxEntry.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxEntry.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxEntry.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxEntry.ModifiedOn;
                    m_dxFields[(int)eFields.SpareText].Value = dxEntry.SpareText;
                    m_dxFields[(int)eFields.SpareNumber].Value = dxEntry.SpareNumber;
				}
				else
				{
					dxEntry.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxEntry.ParentId = (int)(m_dxFields[(int)eFields.ParentId].Value);
					//dxEntry.Path = (string)(m_dxFields[(int)eFields.Path].Value);
					dxEntry.ChildCount = (int)(m_dxFields[(int)eFields.Children].Value);
					dxEntry.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxEntry.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxEntry.Description = (string)(m_dxFields[(int)eFields.Description].Value);
					dxEntry.DisplayOrder = (int)(m_dxFields[(int)eFields.DisplayOrder].Value);
					dxEntry.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxEntry.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxEntry.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxEntry.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
                    dxEntry.SpareText = m_dxFields[(int)eFields.SpareText].Value!=DBNull.Value?
                        (string)m_dxFields[(int)eFields.SpareText].Value: null;
                    dxEntry.SpareNumber = m_dxFields[(int)eFields.SpareNumber].Value != DBNull.Value ?
                        (int?)(m_dxFields[(int)eFields.SpareNumber].Value):null;
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
				case eFields.ParentId:
				case eFields.Children:
				case eFields.Attributes:
				case eFields.DisplayOrder:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.Path:
				case eFields.Name:
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
		
		/// <summary>The record exchange object associated with the Parent binder</summary>
		public CDxBinderEntry Parent
		{
			get { return m_dxParent; }
			set { m_dxParent = value; }
		
		}//	Parent property
		
		/// <summary>True to populate the collection using the SourceId instead of ParentId</summary>
		public bool FillFromSource
		{
			get { return m_bFillFromSource; }
			set { m_bFillFromSource = value; }
		
		}//	FillFromSource property
		
		/// <summary>Source PSTQ identifier</summary>
		public string SourceId
		{
			get { return m_strSourceId; }
			set { m_strSourceId = value; }
		
		}//	SourceId property
		
		#endregion Properties
		
	}//	CDxBinderEntries
		
}// namespace FTI.Trialmax.Database
