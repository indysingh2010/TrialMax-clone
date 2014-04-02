using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class serves as the base class for all data exchange objects used by
	/// the Trialmax application. Data exchange objects are used to transfer between
	/// the application and it's associated database.
	/// </summary>
	public class CDxMediaRecord : CBaseRecord, ITmaxMediaRecord, ITmaxListViewCtrl, ITmaxScriptBoxCtrl
	{
		#region Constants
		
		protected const int DX_MEDIA_RECORD_PROP_AUTOID			= 1;
		protected const int	DX_MEDIA_RECORD_PROP_PSTQ			= 2;
		protected const int DX_MEDIA_RECORD_PROP_CREATED_BY		= 3;
		protected const int DX_MEDIA_RECORD_PROP_CREATED_ON		= 4;
		protected const int DX_MEDIA_RECORD_PROP_MODIFIED_BY	= 5;
		protected const int DX_MEDIA_RECORD_PROP_MODIFIED_ON	= 6;
		protected const int DX_MEDIA_RECORD_PROP_NAME			= 7;
		protected const int DX_MEDIA_RECORD_PROP_DESCRIPTION	= 8;
		protected const int DX_MEDIA_RECORD_PROP_ATTRIBUTES		= 9;
		
		protected const int DX_MEDIA_RECORD_PROP_LAST			= 255;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to Codes property</summary>
		protected CDxCodes m_dxCodes = null;
		
		/// <summary>Local member bound to AutoId property</summary>
		protected long m_lAutoId = 0;
		
		/// <summary>Local member bound to BarcodeId property</summary>
		protected long m_lBarcodeId = 0;
		
		/// <summary>Local member bound to Attributes property</summary>
		protected long m_lAttributes = 0;
		
		/// <summary>Local member bound to MediaType property</summary>
		protected FTI.Shared.Trialmax.TmaxMediaTypes m_eMediaType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to CreatedBy property</summary>
		protected long m_lCreatedBy = 0;
		
		/// <summary>Local member bound to CreatedOn property</summary>
		protected DateTime m_tsCreatedOn = System.DateTime.Now;
		
		/// <summary>Local member bound to Modified property</summary>
		protected bool m_bModified = false;
		
		/// <summary>Local member bound to ModifiedBy property</summary>
		protected long m_lModifiedBy = 0;
		
		/// <summary>Local member bound to Children property</summary>
		protected long m_lChildCount = 0;
		
		/// <summary>Local member bound to DisplayOrder property</summary>
		protected long m_lDisplayOrder = 0;
		
		/// <summary>Local member bound to ModifiedOn property</summary>
		protected DateTime m_tsModifiedOn = System.DateTime.Now;
		
		/// <summary>Local member bound to Name property</summary>
		public string m_strName = "";
		
		/// <summary>Local member bound to ForeignCode property</summary>
		protected string m_strForeignBarcode = "";
		
		/// <summary>Local member bound to Description property</summary>
		protected string m_strDescription = "";
		
		/// <summary>Local member used to manage the Codes collection</summary>
		protected bool m_bInitializeCodes = true;

		/// <summary>Local member bound to Holding property</summary>
		protected bool m_bHolding = false;

		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxMediaRecord() : base()
		{
		}
	
		/// <summary>Constructor</summary>
		public CDxMediaRecord(CDxMediaRecords dxCollection) : base(dxCollection)
		{
		}
	
		/// <summary>This function is called to delete the specified child record</summary>
		public virtual bool Delete(CDxMediaRecord dxChild)
		{
			//	Default behavior is to let the child collection do the job
			if(GetChildCollection() != null)
				return GetChildCollection().Delete(dxChild);
			else
				return false;
		}

	    public virtual void ResetDisplayOrder()
	    {
	        
	    }

        public virtual void ResetSortProperty()
        {
        }

	    /// <summary>This function is called to delete all codes owned by this record</summary>
		public virtual bool DeleteCodes()
		{
			//	Don't bother if codes not enabled
			if(this.CodesEnabled == false) return false;
			
			//	Does this record have a valid collection
			if(m_dxCodes != null)
			{
				//	Make sure the codes collection is using the correct database
				if(this.Database != null)
					m_dxCodes.Database = this.Database;
					
				return m_dxCodes.Flush();
			}
			else
			{
				return false;
			}
		
		}// public virtual bool DeleteCodes()
		
		/// <summary>This function is called to reorder the existing children to match the specified collection</summary>
		public virtual bool Reorder(CDxMediaRecords dxReordered)
		{
			CDxMediaRecords	dxChildren = GetChildCollection();
			int			iIndex;
			
			Debug.Assert(dxReordered != null);
			Debug.Assert(dxChildren != null);
			Debug.Assert(dxReordered.Count == dxChildren.Count);
			if(dxChildren == null) return false;
			if(dxReordered == null) return false;
			if(dxChildren.Count != dxReordered.Count) return false;
			
			//	The child collection is assumed to be out of order
			//
			//	NOTE:	We MUST clear this flag or the collection will
			//			fail on calls to IndexOf() because it thinks the
			//			collection is sorted
			dxChildren.IsSorted = false;
			
			for(int i = 0; i < dxReordered.Count; i++)
			{
				if((iIndex = dxChildren.IndexOf(dxReordered[i])) >= 0)
				{
					if(dxChildren[iIndex].DisplayOrder != i + 1)
					{
						dxChildren[iIndex].DisplayOrder = i + 1;
						dxChildren.Update(dxChildren[iIndex]);
					}					
				}
				else
				{
					Debug.Assert(false);
				}
				
			}	
			
			//	Now make sure the local collection is in the correct order
			dxChildren.Clear();
			foreach(CDxMediaRecord dxRecord in dxReordered)
				dxChildren.AddList(dxRecord);
			
			//	Let the derived class know before we update this record
			OnReordered();
			
			//	Update this record so that the time stamp gets changed
			if(Collection != null)
				Collection.Update(this);
				
			return true;
			
		}// public virtual bool Reorder(CDxMediaRecords dxReordered)
		
		/// <summary>This function is called to get the unique identifier for this record</summary>
		/// <returns>The unique id assigned by the database</returns>
		public virtual string GetUniqueId()
		{
			if(this.Database != null)
				return this.Database.GetUniqueId(this);
			else
				return "";
		}
		
		/// <summary>This method is called to foreign barcode mapped to this record</summary>
		/// <returns>The foreign barcode if mapped</returns>
		public virtual string GetForeignBarcode()
		{
			string strForeign = "";
			
			//	Does this record have an entry in the foreign barcode table?
			if(this.Mapped == true)
			{
				//	Do we need to retrieve the barcode?
				if(m_strForeignBarcode.Length == 0)
				{
					if(Collection != null)
						m_strForeignBarcode = Collection.GetForeignBarcode(this);
					else
						return "";
				}
				
				//	Was there really an entry in the barcode map?
				if(m_strForeignBarcode.Length > 0)
				{
					return m_strForeignBarcode;
				}
				else
				{
					//	The mapped attribute must be out of sync
					if(this.Database != null)
					{
						this.Mapped = false;
						Collection.Update(this);
						Collection.Database.FireInternalUpdate(this);
					}
					
				}// if(m_strForeignBarcode.Length > 0)
					
			}// if(this.Mapped == true)
			
/*	INHERITED
			//	Since this record is not mapped maybe it's parent is
			if(this.GetParent() != null)
			{
				//	Get the foreign barcode for the parent record
				strForeign = this.GetParent().GetForeignBarcode();
				
				//	Add the barcode id to the parent's value
				if(strForeign.Length > 0)
					strForeign += ("." + this.BarcodeId.ToString());
			
			}
*/		
			return strForeign;
			
		}// public virtual string GetForeignBarcode()
			
		/// <summary>This method is called to set the foreign barcode mapped to this record</summary>
		/// <param name="strForeignBarcode">the barcode to be mapped to this record</param>
		/// <param name="bUpdate">true to update the record in the BarcodeMap table</param>
		/// <param name="bConfirm">true to prompt the user for confirmation if foreign barcode already exists</param>
		/// <returns>true if successful</returns>
		public virtual bool SetForeignBarcode(string strForeignBarcode, bool bUpdate, bool bConfirm)
		{
			bool bModified = true;
			
			//	First determine if the value has actually changed?
			if(m_strForeignBarcode != null)
			{
				if(strForeignBarcode != null)
					bModified = (String.Compare(m_strForeignBarcode, strForeignBarcode, false) != 0);
			}
			else
			{
				bModified = (strForeignBarcode != null);
			}
			
			//	Nothing to do if not modified 
			if(bModified == false) return true;
			
			//	Do we need to update the database?
			if((bUpdate == true) && (Collection != null))
			{
				if(Collection.SetForeignBarcode(this, strForeignBarcode, bConfirm) == false)
					return false;
			}
			
			m_strForeignBarcode = strForeignBarcode;
			return true;
		
		}// public virtual bool SetForeignBarcode(string strForeignBarcode)
			
		/// <summary>This function is called to reset the Codes associated with this record</summary>
		public virtual void ResetCodes()
		{
			//	Force a refresh the next time the codes collection is referenced
			m_bInitializeCodes = true;
		}
		
		/// <summary>This function is called to get the text used to display this record in a TrialMax tree</summary>
		public virtual string GetText(TmaxTextModes eMode)
		{
			return GetText();
		}
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public virtual string GetText()
		{
			return GetDefaultName();
		}
		
		/// <summary>This method is called to get the time required to play the record</summary>
		/// <returns>The total time in decimal seconds</returns>
		public virtual double GetDuration()
		{
			return -1.0;
		}
			
		/// <summary>This function is called to override the default implementation to return the default text descriptor</summary>
		public override string ToString()
		{
			return GetText();
		}
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		public virtual string GetDisplayString()
		{
			return GetText();
			
		}// virtual public string GetDisplayString()
		
		/// <summary>This function is called to get the default text used to display the name of the record</summary>
		/// <returns>The default name that represents this record</returns>
		public virtual string GetDefaultName()
		{
			return m_lAutoId.ToString();
		}
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="Args">The encapsulated arguments</param>
		/// <returns>true if successful</returns>
		public bool SetProperty(CTmaxSetPropertyArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Property != null);
			Debug.Assert(Args.NewValue != null);
			if(Args == null) return false;
			
			if((Args.Property == null) || (Args.NewValue == null))
			{
				Args.Message = "Invalid arguments";
				return false;
			}
			
			return SetProperty(Args.Property.Id, Args.NewValue, Args.Confirmed, Args.Message);
			
		}// public virtual bool SetProperty(CTmaxSetPropertyArgs Args)
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="iId">The id of the property being set</param>
		/// <param name="strValue">The new property value</param>
		/// <param name="bConfirmed">True if new value has been confirmed</param>
		/// <param name="strMessage">Buffer in which to store a return message</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		{
			switch(iId)
			{
				case DX_MEDIA_RECORD_PROP_NAME:
				
					m_strName = strValue;
					return true;
					
				case DX_MEDIA_RECORD_PROP_DESCRIPTION:
				
					this.Description = strValue;
					return true;
					
				default:
				
					if(strMessage != null)
						strMessage = "The property is read only";
					return false;
					
			}// switch(iId)
			
		}// public virtual bool SetProperty(int iId, string strValue, bool bConfirmed, string strMessage)
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="iId">The id of the property being set</param>
		/// <param name="strValue">The new property value</param>
		/// <param name="bConfirmed">True if new value has been confirmed</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperty(int iId, string strValue, bool bConfirmed)
		{
			string strMessage = "";
			return SetProperty(iId, strValue, bConfirmed, strMessage);
		}
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="iId">The id of the property being set</param>
		/// <param name="strValue">The new property value</param>
		/// <param name="strMessage">Buffer in which to store a return message</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperty(int iId, string strValue, string strMessage)
		{
			return SetProperty(iId, strValue, false, strMessage);
		}
		
		/// <summary>This function is called to set a property associated with the record</summary>
		/// <param name="iId">The id of the property being set</param>
		/// <param name="strValue">The new property value</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperty(int iId, string strValue)
		{
			string strMessage = "";
			return SetProperty(iId, strValue, strMessage);
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This function is called to get the media level of the derived object</summary>
		public virtual FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			//	Default is to not be associated with media
			return FTI.Shared.Trialmax.TmaxMediaLevels.None;
		}
		
		/// <summary>This function is called to get the date the media was added to the database</summary>
		public virtual System.DateTime GetCreatedOn()
		{
			return m_tsCreatedOn;
		}
		
		/// <summary>This function is called to get the id of the user that created the record</summary>
		public virtual long GetCreatedBy()
		{
			return m_lCreatedBy;
		}
		
		/// <summary>This function is called to get the date the media was last modified</summary>
		public virtual System.DateTime GetModifiedOn()
		{
			return m_tsModifiedOn;
		}
		
		/// <summary>This function is called to get the id of the user that modified the record</summary>
		public virtual long GetModifiedBy()
		{
			return m_lModifiedBy;
		}
		
		/// <summary>This function is called to get the record attributes</summary>
		/// <returns>The current attributes</returns>
		public virtual long GetAttributes()
		{
			return m_lAttributes;
		}
		
		/// <summary>This function is called to get the record's Admitted state</summary>
		/// <returns>True if the record has been Admitted</returns>
		public virtual bool GetAdmitted()
		{
			return GetAdmitted(false);
		}
		
		/// <summary>This function is called to get the record's Admitted state</summary>
		/// <param name="bIgnoreCode">true to read the attribute instead of the coded value</param>
		/// <returns>True if the record has been Admitted</returns>
		public virtual bool GetAdmitted(bool bIgnoreCode)
		{
			string	strValue = "";
			bool	bAdmitted = false;
			
			//	This is one of our coded properties
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((this.AutoId > 0) && (GetCodes(false) != null) && (bIgnoreCode == false))
			{
				strValue = GetCodedPropValue(TmaxCodedProperties.Admitted, false);
			
				if((strValue != null) && (strValue.Length > 0))
					bAdmitted = CTmaxCaseCode.ToBool(strValue);				
			}
			else
			{
				bAdmitted = ((Attributes & (long)(TmaxCommonAttributes.Admitted)) != 0);
			}
			
			return bAdmitted;
			
		}// public virtual bool GetAdmitted()
		
		/// <summary>This function is called to set the record's Admitted state</summary>
		/// <param name="bAdmitted">The new Admitted state</param>
		public virtual void SetAdmitted(bool bAdmitted)
		{
			SetAdmitted(bAdmitted, false);		
		}
		
		/// <summary>This function is called to set the record's Admitted state</summary>
		/// <param name="bAdmitted">The new Admitted state</param>
		/// <param name="bIgnoreCode">true to ignore the coded property</param>
		public virtual void SetAdmitted(bool bAdmitted, bool bIgnoreCode)
		{
			//	This is one of our coded properties
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((this.AutoId > 0) && (GetCodes(false) != null) && (bIgnoreCode == false))
			{
				if(bAdmitted == true)
					SetCodedPropValue(TmaxCodedProperties.Admitted, "true", false);
				else
					SetCodedPropValue(TmaxCodedProperties.Admitted, "", false);
			}
			else
			{
				//	Set the appropriate common bit in the record's attributes
				if(bAdmitted == true)
					m_lAttributes |= (long)TmaxCommonAttributes.Admitted;
				else
					m_lAttributes &= ~((long)TmaxCommonAttributes.Admitted);
			}
		
		}// public virtual void SetAdmitted(bool bAdmitted)
		
		/// <summary>This function is called to get the record's Mapped state</summary>
		/// <returns>True if the record has been Mapped</returns>
		public virtual bool GetMapped()
		{
			//	Default behavior is to use the Mapped attribute
			return ((Attributes & (long)(TmaxCommonAttributes.Mapped)) != 0);
		}

		/// <summary>This method retrieves the record's split screen attribute</summary>
		///	<returns>True if split screen</returns>
		public virtual bool GetSplitScreen()
		{
			return false;
		}
		
		/// <summary>This function is called to set the record's Mapped state</summary>
		/// <param name="bMapped">The new Mapped state</param>
		public virtual void SetMapped(bool bMapped)
		{
			//	Default behavior is to set the associated attribute
			if(bMapped == true)
			{
				m_lAttributes |= (long)TmaxCommonAttributes.Mapped;
			}
			else
			{
				m_lAttributes &= ~((long)TmaxCommonAttributes.Mapped);
			}
		
		}// public virtual void SetMapped(bool bMapped)
		
		/// <summary>This function is called to get the total number of children</summary>
		/// <returns>The total number of children</returns>
		public virtual long GetChildCount()
		{
			return m_lChildCount;
		}
		
		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public virtual CDxMediaRecord GetParent()
		{
			return null;
		}
		
		/// <summary>This function is called to get the AutoId of the parent record</summary>
		/// <returns>The autoid assigned to the parent if it exists</returns>
		public virtual long GetParentId()
		{
			long				lId = 0;
			ITmaxMediaRecord	IParent = null;
			
			if((IParent = GetParent()) != null)
				lId = IParent.GetAutoId();
				
			return lId;
			
		}// public virtual long GetParentId()
		
		/// <summary>This function is called to get the parent media type</summary>
		/// <returns>The parent media type if available</returns>
		public virtual TmaxMediaTypes GetParentMediaType()
		{
			if(GetParent() != null)
				return GetParent().MediaType;
			else
				return TmaxMediaTypes.Unknown;
		}
		
		/// <summary>This method is called to determine the relationship that exists between this record and the specified record</summary>
		/// <param name="dxRecord">The record exchange object being evaluated</param>
		/// <param name="bCheckGrandparent">true to check for grandparent relationship</param>
		/// <returns>The enumerated media relationship</returns>
		public virtual TmaxMediaRelationships GetRelationship(CDxMediaRecord dxRecord, bool bCheckGrandparent)
		{
			TmaxMediaRelationships eRelationship = TmaxMediaRelationships.None;
			
			//	Equal?
			if(ReferenceEquals(this, dxRecord) == true)
			{
				return TmaxMediaRelationships.Same;
			}
			
			//	Parent?
			if((this.GetParent() != null) &&
			   (ReferenceEquals(this.GetParent(), dxRecord) == true))
			{
				return TmaxMediaRelationships.Parent;
			}
			
			//	Child?
			if((this.GetChildCollection() != null) &&
			   (this.GetChildCollection().Contains(dxRecord) == true))
			{
				return TmaxMediaRelationships.Child;
			}
			
			//	Sibling?
			if((this.GetParent() != null) &&
			   (dxRecord.GetParent() != null) && 
			   (ReferenceEquals(this.GetParent(), dxRecord.GetParent()) == true))
			{
				return TmaxMediaRelationships.Sibling;
			}
			
			//	Grandchild?
			if((this.GetChildCollection() != null) && (this.GetChildCollection().Count > 0))
			{
				foreach(CDxMediaRecord O in this.GetChildCollection())
				{
					eRelationship = O.GetRelationship(dxRecord, false);
					if((eRelationship == TmaxMediaRelationships.Child) ||
					   (eRelationship == TmaxMediaRelationships.Grandchild))
						return TmaxMediaRelationships.Grandchild;
				}
				
			}
			
			//	Grandparent
			if(bCheckGrandparent == true)
			{
				//	For the caller's record to be a grandparent this record must be a grandchild
				if(dxRecord.GetRelationship(this, false) == TmaxMediaRelationships.Grandchild)
					return TmaxMediaRelationships.Grandparent;
			}
			
			//	No relationship
			return TmaxMediaRelationships.None;
		}
		
		/// <summary>This method is called to determine if the specified record is a descendant of this record</summary>
		/// <param name="dxRecord">The record exchange object being evaluated</param>
		/// <returns>True if dxRecord is descended from this record</returns>
		public virtual bool IsDescendant(CDxMediaRecord dxRecord)
		{
			CDxMediaRecord dxParent = null;
			
			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return false;
			
			//	Get the parent of the specified record
			dxParent = dxRecord.GetParent();
			
			while(dxParent != null)
			{
				//	Is this record the current parent?
				if(ReferenceEquals(dxParent, this) == true)
					return true;
				else
					dxParent = dxParent.GetParent();
			
			}//	while(dxParent != null)
			
			//	Must not be descendant
			return false;	
			
		}// public virtual bool IsDescendant(CDxMediaRecord dxRecord)
		
		/// <summary>This method is called to determine if the specified record is an ancestor of this record</summary>
		/// <param name="dxRecord">The record exchange object being evaluated</param>
		/// <returns>True if dxRecord is an ancestor</returns>
		public virtual bool IsAncestor(CDxMediaRecord dxRecord)
		{
			CDxMediaRecord dxParent = null;
			
			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return false;
			
			//	Get the parent of the this record
			dxParent = this.GetParent();
			
			while(dxParent != null)
			{
				//	Is this record the current parent?
				if(ReferenceEquals(dxParent, dxRecord) == true)
					return true;
				else
					dxParent = dxParent.GetParent();
			
			}//	while(dxParent != null)
			
			//	Must not be an ancestore
			return false;	
			
		}// public virtual bool IsAncestor(CDxMediaRecord dxRecord)
		
		/// <summary>This function is called to get the name of the file associated with the record</summary>
		/// <returns>The name of the associated file</returns>
		public virtual string GetFileName()
		{
			string strFileSpec = GetFileSpec();
			
			if(strFileSpec.Length > 0)
				return System.IO.Path.GetFileName(strFileSpec);
			else
				return "";
		}
		
		/// <summary>This function is called to get the path of the file associated with the record</summary>
		/// <returns>The path to the associated file</returns>
		public virtual string GetFileSpec()
		{
			if(this.Database != null)
				return this.Database.GetFileSpec(this);
			else
				return "";
		}
		
		/// <summary>This function is called to get the barcode that identifies this record</summary>
		/// <param name="bIgnoreMapped">The true to ignore the record's foreign barcode if mapped</param>
		/// <returns>The record's barcode</returns>
		public virtual string GetBarcode(bool bIgnoreMapped)
		{
			if(this.Database != null)
				return this.Database.GetBarcode(this, bIgnoreMapped);
			else
				return "";
		}
		
		/// <summary>This function is called to get the barcode identifier assigned to this record</summary>
		/// <returns>The record's barcode identifier</returns>
		public virtual long GetBarcodeId()
		{
			return m_lBarcodeId;
		}
		
		/// <summary>This function is called to get the name associated with this record</summary>
		/// <returns>The name associated with this record</returns>
		public virtual string GetName()
		{
			return m_strName;
		}
		
		/// <summary>This function is called to get the description associated with this record</summary>
		/// <returns>The description associated with this record</returns>
		public virtual string GetDescription()
		{
			return GetDescription(false);			
		}
		
		/// <summary>This function is called to get the description associated with this record</summary>
		/// <param name="bIgnoreCode">True to ignore the coded value</param>
		/// <returns>The description associated with this record</returns>
		public virtual string GetDescription(bool bIgnoreCode)
		{
			//	Does this record type use codes?
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((this.AutoId > 0) && (GetCodes(false) != null) && (bIgnoreCode == false))
			{
				return GetCodedPropValue(TmaxCodedProperties.Description, false);
			}
			else
			{
				return m_strDescription;
			}
			
		}// public virtual string GetDescription(bool bIgnoreCode)
		
		/// <summary>This function is called to set the description associated with this record</summary>
		/// <param name="strValue">The new value to be assigned to the description</param>
		public virtual void SetDescription(string strValue)
		{
			//	Does this record type use codes?
			//
			//	NOTE:	We have to check the AutoId because the record may not have
			//			been added to the database yet
			if((this.AutoId > 0) && (GetCodes(false) != null))
			{
				SetCodedPropValue(TmaxCodedProperties.Description, strValue, false);
			}
			else
			{
				//	Set the local class member
				m_strDescription = (strValue != null) ? strValue : "";
			}
			
		}// public virtual void SetDescription(string strValue)

		/// <summary>Called to get the collection of child records</summary>
		/// <returns>The child collection if it exists</returns>
		public virtual CDxMediaRecords GetChildCollection()
		{
			return null;
		}

		/// <summary>Called to fill the child collection</summary>
		/// <returns>true if successful</returns>
		public virtual bool Fill()
		{
			return false;
		}

		/// <summary>This method is called to get the media id of the primary owner</summary>
		/// <returns>The media id of the primary record that owns this record</returns>
		public virtual string GetMediaId()
		{
			return "";
		}

		/// <summary>This function is called to determine if image cleaning tools can be applied to the media associated with this record</summary>
		/// <param name="bFill">True if OK to fill the child collection to make the determination</param>
		/// <returns>True if tools can be applied</returns>
		public virtual bool GetCanClean(bool bFill)
		{
			return false;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public virtual void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	Add properties that are exposed for all (or nearly all) media types
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_AUTOID, "Database Id", m_lAutoId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_PSTQ, "PSTQ Id", Collection.Database.GetUniqueId(this), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_ATTRIBUTES, "Attributes", m_lAttributes, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_CREATED_BY, "Created By", Collection.Database.GetUserName(m_lCreatedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_CREATED_ON, "Created On", m_tsCreatedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_MODIFIED_BY, "Modified By", Collection.Database.GetUserName(m_lModifiedBy), TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(DX_MEDIA_RECORD_PROP_MODIFIED_ON, "Modified On", m_tsModifiedOn, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method is called to get the id of the property with the specified name</summary>
		/// <param name="strName">The property name</param>
		/// <returns>The associated identifier</returns>
		public virtual int GetPropertyId(string strName)
		{
			if(String.Compare(strName, "Name", true) == 0)
				return DX_MEDIA_RECORD_PROP_NAME;
			else if(String.Compare(strName, "Description", true) == 0)
				return DX_MEDIA_RECORD_PROP_DESCRIPTION;
			else
				return -1;
		
		}// public virtual int GetPropertyId(string strName)
		
		/// <summary>This method will refresh the value of all properties in the specified collection</summary>
		/// <param name="tmaxProperty">The properties to be refreshed</param>
		public virtual void RefreshProperties(CTmaxProperties tmaxProperties)
		{
			foreach(CTmaxProperty O in tmaxProperties)
			{
				try
				{
					RefreshProperty(O);
				}
				catch
				{
				}
				
			}// foreach(CTmaxProperty O in tmaxProperties)
				
		}// public virtual void RefreshProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DX_MEDIA_RECORD_PROP_ATTRIBUTES:
				
					tmaxProperty.Value = m_lAttributes;
					break;
					
				case DX_MEDIA_RECORD_PROP_MODIFIED_BY:
				
					tmaxProperty.Value = Collection.Database.GetUserName(m_lModifiedBy);
					break;
					
				case DX_MEDIA_RECORD_PROP_MODIFIED_ON:
				
					tmaxProperty.Value = m_tsModifiedOn;
					break;
					
				case DX_MEDIA_RECORD_PROP_NAME:
				
					tmaxProperty.Value = m_strName;
					break;
					
				case DX_MEDIA_RECORD_PROP_DESCRIPTION:
				
					tmaxProperty.Value = this.Description;
					break;
					
				//	These properties are read-only
				case DX_MEDIA_RECORD_PROP_AUTOID:
				case DX_MEDIA_RECORD_PROP_PSTQ:
				case DX_MEDIA_RECORD_PROP_CREATED_BY:
				case DX_MEDIA_RECORD_PROP_CREATED_ON:
					
				default:
				
					break;
					
			}
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		/// <summary>This function is called to get the record's display order index</summary>
		/// <returns>The display order index</returns>
		public virtual long GetDisplayOrder()
		{
			return m_lDisplayOrder;
		}
		
		/// <summary>This method is called by the base class after reordering the children but before updating the record</summary>
		public virtual void OnReordered()
		{
		}
	
		/// <summary>This method creates an xml node that represents this record</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name to be assigned to the node</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			CTmaxProperties	tmaxProperties = new CTmaxProperties();
				
			GetProperties(tmaxProperties);
			
			return tmaxProperties.ToXmlNode(xmlDocument, strName);
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return this.AutoId.ToString();
		}
		
		///	<summary>This method is called to determine if the record media is linked</summary>
		/// <returns>True if linked</returns>
		public virtual bool GetLinked()
		{
			return (GetAliasId() > 0);
		}
		
		///	<summary>This method is called to retrieve the id of the server/drive alias assigned to the record</summary>
		/// <returns>The record's alias identifier</returns>
		public virtual long GetAliasId()
		{
			return 0;
		}
		
		///	<summary>This method retrieves the aliased path for a linked media record</summary>
		/// <returns>The fully qualified aliased path if linked</returns>
		public virtual string GetAliasedPath()
		{
			if(this.Database != null)
				return this.Database.GetAliasedPath(this);
			else
				return "";
		}

        protected virtual CDxCode GetCode(CTmaxCaseCode caseCode)
        {
            CDxCodes code = new CDxCodes(this.Database);
            code.Fill(caseCode, this);
            return code[0];
        }

        public string GetCodePickItemValue(CTmaxCaseCode caseCode)
        {
            CTmaxPickItem pickItem;
            CDxCode code = GetCode(caseCode);
            if (code == null) return null;
            pickItem = code.GetPickItem(code.GetInteger(), code.GetPickList(false));
            return pickItem != null ? pickItem.Name : null;
            
        }

        public DateTime? GetCodeDateValue(CTmaxCaseCode caseCode)
        {
            CDxCode code = GetCode(caseCode);
            return code!=null?code.GetDate():(DateTime?)null;
        }

        public int? GetCodeIntValue(CTmaxCaseCode caseCode)
        {
            CDxCode code = GetCode(caseCode);
            return code!=null?code.GetInteger():(int?)null;
        }

        public string GetCodeText(CTmaxCaseCode caseCode)
        {
            CDxCode code = GetCode(caseCode);
            return code!=null?code.Value.Trim():null;
        }

        public bool? GetCodeBoolValue(CTmaxCaseCode caseCode)
        {
            CDxCode code = GetCode(caseCode);
            return code!=null?code.GetBool():(bool?)null;
        }

        public double? GetCodeDecimalValue(CTmaxCaseCode caseCode)
        {
            CDxCode code = GetCode(caseCode);
            return code!=null? code.GetDecimal():(double?)null;
        }

		/// <summary>This function is called to get the collection of codes associated with this record</summary>
		/// <param name="bRefresh">true to force refreshing of the collection</param>
		/// <returns>The record's codes collection</returns>
		public virtual CDxCodes GetCodes(bool bRefresh)
		{
			//	Are code operations disabled?
			if(this.Database == null) return null;
				
			//	Does this record support codes (fielded data)?
			if(m_dxCodes == null) return null;
			
			//	Are codes enabled for this database?
			if(this.Database.CodesEnabled == false) return null;
			
			//	Do we need to fill the collection?
			if((m_bInitializeCodes == true) || (bRefresh == true))
			{
				//	Make sure the collection has been assigned the active database
				m_dxCodes.Database = this.Database;
				
				//	Make sure the sorter has been assigned
				if(m_dxCodes.Comparer == null)
				{
					m_dxCodes.Comparer = new CTmaxRecordSorter();
					m_dxCodes.KeepSorted = false;
				}
				
				//	Clear the collection and read the table
				m_dxCodes.Clear();
				m_dxCodes.Fill();
				m_dxCodes.Sort();
				
				//	Collection has been initialized
				m_bInitializeCodes = false;
			}
			
			return m_dxCodes;
		
		}// public virtual CDxCodes GetCodes(bool bRefresh)
		
		/// <summary>This method adds the specified code to the collection owned by this record</summary>
		/// <param name="dxCode">The code to be added</param>
		/// <returns>The exchange interface to the code if successful</returns>
		public CDxCode Add(CDxCode dxCode)
		{
			CDxCodes dxCodes = null;
			
			if((dxCodes = GetCodes(false)) != null)
			{
				//	Make sure the collection has been assigned the correct database
				if(this.Database != null)
					dxCodes.Database = this.Database;
					
				return dxCodes.Add(dxCode);
			}
			else
			{
				return null;
			}
				
		}// public CDxCode Add(CDxCode dxCode)
		
		/// <summary>This function is called to delete the specified code</summary>
		public virtual bool Delete(CDxCode dxCode)
		{
			CDxCodes dxCodes = null;
			
			//	Don't bother if codes not enabled
			if(this.CodesEnabled == false) return false;
			
			if((dxCodes = GetCodes(false)) != null)
			{
				//	Make sure the codes collection is using the correct database
				if(this.Database != null)
					dxCodes.Database = this.Database;
					
				return dxCodes.Delete(dxCode);
			}
			else
			{
				return false;
			}
		
		}// public virtual bool Delete(CDxCode dxCode)
		
		/// <summary>This function is called to update the specified code</summary>
		public virtual bool Update(CDxCode dxCode)
		{
			CDxCodes dxCodes = null;
			
			if((dxCodes = GetCodes(false)) != null)
			{
				//	Make sure the codes collection is using the correct database
				if(this.Database != null)
					dxCodes.Database = this.Database;
					
				return dxCodes.Update(dxCode);
			}
			else
			{
				return false;
			}
		
		}// public virtual bool Update(CDxCode dxCode)
		
		/// <summary>This function is called to get the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		public virtual string[] GetListViewNames(int iDisplayMode)
		{
			return null;
		}
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		public virtual string[] GetListViewValues(int iDisplayMode)
		{
			return null;
		}
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		public virtual int GetListViewImage(int iDisplayMode)
		{
			//	No images used for this type
			return -1;
		}
		
		/// <summary>This method is called to get the value assigned to the specified coded property</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <returns>The assigned value</returns>
		public virtual string GetCodedPropValue(TmaxCodedProperties e, bool bRefresh)
		{
			string		strValue = "";
			CDxCodes	dxCodes = null;
			CDxCode		dxCode = null;
			
			try
			{
				//	Get the codes collection for this record
				if((dxCodes = GetCodes(bRefresh)) != null)
				{
					//	Locate the requested code
					if((dxCode = dxCodes.Find(e)) != null)
					{
						strValue = dxCode.GetValue(false);
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				if(this.Collection != null)
                    this.Collection.FireError("GetCodedPropValue",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_GET_CODED_PROP_VALUE_EX,this.GetBarcode(false),e.ToString()),Ex);
			}
			
			return strValue;
			
		}// public string GetCodedPropValue(TmaxCodedProperties e)
			
		/// <summary>This method is called to set the value assigned to the specified coded property</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <param name="strValue">The value to be assigned</param>
		/// <param name="bRefresh">true to refresh the collection first</param>
		/// <returns>True if successful</returns>
		public virtual bool SetCodedPropValue(TmaxCodedProperties e, string strValue, bool bRefresh)
		{
			bool		bSuccessful = false;
			CDxCodes	dxCodes = null;
			CDxCode		dxCode = null;
			
			try
			{
				//	Get the codes collection for this record
				if((dxCodes = GetCodes(bRefresh)) == null) 
					return false;
				
				//	Locate the requested code
				dxCode = dxCodes.Find(e);
				
				//	Is the caller attempting to delete this value?
				if((strValue == null) || (strValue.Length == 0))
				{
					//	Do we need to delete this code?
					if(dxCode != null)
						bSuccessful = dxCodes.Delete(dxCode);
					else
						bSuccessful = true; // Nothing to do
				}
				else
				{
					//	Are we changing the current value?
					if(dxCode != null)
					{
						if(dxCode.SetValue(strValue) == true)
							bSuccessful = dxCodes.Update(dxCode);
					}
					else
					{
						//	Create a new code object
						if((dxCode = dxCodes.Alloc(e)) != null)
						{
							if(dxCode.SetValue(strValue) == true)
								bSuccessful = (dxCodes.Add(dxCode) != null);
						}
						
					}	
				
				}// if((strValue == null) || (strValue.Length == 0))
			
			}
			catch(System.Exception Ex)
			{
				if(this.Collection != null)
                    this.Collection.FireError("SetCodedPropValue",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_SET_CODED_PROP_VALUE_EX,this.GetBarcode(false),e.ToString()),Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetCodedPropValue(TmaxCodedProperties e, string strValue, bool bRefresh)
			
		/// <summary>Called to exchange the coded property values with their associated class members</summary>
		/// <param name="bSetCodes">True to set the codes using the associated class members</param>
		/// <param name="bUnassigned">True to exchange unassigned codes/members</param>
		/// <param name="bUpdate">True to update the owner record time stamps</param>
		/// <param name="bRefresh">True to refresh the collection prior to performing the operation</param>
		/// <returns>True if successful</returns>
		public virtual bool ExchangeCodedProperties(bool bSetCodes, bool bUnassigned, bool bUpdate, bool bRefresh)
		{
			CDxCodes	dxCodes = null;
			bool		bSuccessful = true;
			bool		bAdmitted = false;
			bool		bModified = false;
			
			//	Must have a codes collection
			if((dxCodes = GetCodes(bRefresh)) == null) return false;
			
			try
			{
				//	Are we setting the coded property values?
				if(bSetCodes == true)
				{
					//	Use Description class member to set the code
					if((bUnassigned == true) || (m_strDescription.Length > 0))
					{
						if(this.Description != m_strDescription)
						{
							this.Description = m_strDescription;
							m_strDescription = "";
							bModified = true;
						}
					
					}
					
					//	Use the Admitted attribute to set the code
					bAdmitted = GetAdmitted(true);
					if((bUnassigned == true) || (bAdmitted == true))
					{
						if(bAdmitted != this.Admitted)
						{
							this.Admitted = bAdmitted;
							SetAdmitted(false, true);
							bModified = true;
						}
						
					}
					
				}
				else
				{
					//	Use the Description code value to set the class member
					if((bUnassigned == true) || (this.Description.Length > 0))
					{	
						if(m_strDescription != this.Description)
						{
							m_strDescription = this.Description;
							bModified = true;
						}
						
					}					
						
					//	Use the Admitted code value to set the attribute bit
					if((bUnassigned == true) || (this.Admitted == true))
					{
						if(this.Admitted != GetAdmitted(true))
						{
							SetAdmitted(this.Admitted, true);
							bModified = true;
						}
						
					}
				
				}// if(bSetCodes == true)
				
				//	Should we update the record?
				if((bUpdate == true) && (bModified == true) && (this.Collection != null))
				{
					this.Collection.Update(this);
				}
				
			}
			catch(System.Exception Ex)
			{
				if(this.Collection != null)
                    this.Collection.FireError("ExchangeCodedProperties",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_EXCHANGE_CODED_PROPS_EX,this.GetBarcode(false),bSetCodes.ToString()),Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
		
		}// public virtual bool ExchangeCodedProperties(bool bRefresh, bool bSetCodes, bool bIgnoreUnassigned)
		
		/// <summary>This method is called to get the value to be exported for the specified column</summary>
		/// <param name="tmaxColumn">The export column descriptor</param>
		/// <param name="bRefresh">True to refresh coded data</param>
		/// <param name="tmaxOptions">The export options defined for the operation</param>
		/// <returns>The value associated with the specified code</returns>
		public virtual bool GetExportValues(CTmaxExportColumn tmaxColumn, bool bRefresh, CTmaxExportOptions tmaxOptions)
		{
			bool		bSuccessful = true;
			CDxCodes	dxCodes = null;
			CDxCodes	dxValues = null;
			string		strConcatenated = "";
			
			//	Make sure the column has a valid collection
			if(tmaxColumn == null) return false;
			if(tmaxColumn.Values == null) return false;
			
			//	Is this column bound to a case code?
			if(tmaxColumn.CaseCode != null)
			{
				if((dxCodes = GetCodes(bRefresh)) != null)
				{
					if((dxValues = dxCodes.FindAll(tmaxColumn.CaseCode)) != null)
					{
						//	Add each to the columns collection
						foreach(CDxCode O in dxValues)
						{
							//	Are we concatenating the values?
							if(tmaxOptions.Concatenate == true)
							{
								if(strConcatenated.Length > 0)
									strConcatenated += tmaxOptions.GetConcatenator();
								strConcatenated += O.GetValue(true);
							}
							else
							{
								tmaxColumn.Values.Add(O.GetValue(true));
								
							}// if(tmaxOptions.Concatenate == true)
						
						}// foreach(CDxCode O in dxValues)
						
						if((tmaxOptions.Concatenate == true) && (strConcatenated.Length > 0))
							tmaxColumn.Values.Add(strConcatenated);
							
					}// if((dxValues = dxCodes.FindAll(tmaxColumn.CaseCode)) != null)
					
				}
				else
				{
					bSuccessful = false;
				
				}// if((dxCodes = GetCodes(bRefresh)) != null)

			}// if(tmaxColumn.CaseCode != null)
			
			//	Is it an enumerated column?
			else if(tmaxColumn.TmaxEnumId != TmaxExportColumns.Invalid)
			{
				switch(tmaxColumn.TmaxEnumId)
				{
					case TmaxExportColumns.Barcode:
					
						tmaxColumn.Values.Add(this.GetBarcode(false));
						break;
						
					case TmaxExportColumns.Name:
					
						tmaxColumn.Values.Add(this.GetName());
						break;
						
					default:
					
						Debug.Assert(false, tmaxColumn.TmaxEnumId.ToString() + " not handled");
						break;
						
				}//  switch(tmaxColumn.TmaxEnumId)
			}
			else
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public virtual bool GetExportValues(CTmaxExportColumn tmaxColumn, bool bRefresh)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The collection that owns this record</summary>
		new public CDxMediaRecords Collection
		{
			get { return ((CDxMediaRecords)m_dxCollection); }
			set
			{
				m_dxCollection = (CDxMediaRecords)value;
				
				//	Notify the derived class
				OnCollectionChanged();
			}
			
		}

        /// <summary>The active database</summary>
        new public CTmaxCaseDatabase Database
        {
            get { return (CTmaxCaseDatabase)(base.Database); }
        }
        
        /// <summary>AutoId value assigned by the database engine</summary>
		public long AutoId
		{
			get { return m_lAutoId; }
			set { m_lAutoId = value; }
		}
		
		/// <summary>The Id used to construct the barcode</summary>
		public long BarcodeId
		{
			get { return GetBarcodeId(); }
			set { m_lBarcodeId = value; }
		}
		
		/// <summary>The media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get { return m_eMediaType; }
			set { m_eMediaType = value; }
		}
		
		/// <summary>The packed flags that define the attributes</summary>
		public long Attributes
		{
			get { return m_lAttributes; }
			set { m_lAttributes = value; }
		}
		
		/// <summary>Are code (fielded data) operations enabled</summary>
		public bool CodesEnabled
		{
			get { return (this.Database != null) ? this.Database.CodesEnabled : false; }
		}
		
		/// <summary>The number of children belonging to the record</summary>
		public long ChildCount
		{
			get { return GetChildCount(); }
			set { m_lChildCount = value; }
		}
		
		/// <summary>The postion of the record in the display order</summary>
		public long DisplayOrder
		{
			get { return GetDisplayOrder(); }
			set { m_lDisplayOrder = value; }
		}
		
		/// <summary>The media description</summary>
		public string Description
		{
			get { return GetDescription(); }
			set { SetDescription(value); }
		}
		
		/// <summary>The descriptive Name</summary>
		public string Name
		{
			get { return GetName(); }
			set { m_strName = value; }
		}
		
		/// <summary>The identifier of the user that created the media record</summary>
		public long CreatedBy
		{
			get { return GetCreatedBy(); }
			set { m_lCreatedBy = value; }
		}
		
		/// <summary>The date and time the media entry was created</summary>
		public DateTime CreatedOn
		{
			get { return GetCreatedOn(); }
			set { m_tsCreatedOn = value; }
		}
		
		/// <summary>Flag to indicate that the exchange object has been modified</summary>
		public bool Modified
		{
			get { return m_bModified; }
			set { m_bModified = value; }
		}
		
		/// <summary>The identifier of the user that last modified the media entry</summary>
		public long ModifiedBy
		{
			get { return GetModifiedBy(); }
			set { m_lModifiedBy = value; }
		}
		
		/// <summary>The date and time the media was last modified</summary>
		public DateTime ModifiedOn
		{
			get { return GetModifiedOn(); }
			set { m_tsModifiedOn = value; }
		}

        public string SpareText
        {
            get; set;
        }

        public int? SpareNumber
        {
            get; set;
        }
		/// <summary>Property used to display the object in generic Windows controls</summary>
		public string DisplayString
		{
			get{ return GetDisplayString();	}
		}
		
		/// <summary>Foreign barcode mapped to this record</summary>
		public string ForeignBarcode
		{
			get { return GetForeignBarcode(); }
			set
			{
				if(m_strForeignBarcode != value)
					SetForeignBarcode(value, true, false);
			}
		
		}
		
		/// <summary>Flag to indicate that the object has an entry in the barcode map</summary>
		public bool Mapped
		{
			get { return GetMapped(); }
			set { SetMapped(value); }
		}
		
		/// <summary>Flag to indicate that the object has been admitted</summary>
		public bool Admitted
		{
			get { return GetAdmitted(); }
			set { SetAdmitted(value); }
		}
		
		/// <summary>The collection of codes associated with this record</summary>
		public CDxCodes Codes
		{
			get { return GetCodes(false); }
		}

		/// <summary>Flag to assist managing records stored in a temporary holding collection</summary>
		public bool Holding
		{
			get { return m_bHolding; }
			set { m_bHolding = value; }
		}

		#endregion Properties
		
		/// <summary>This method retrieves the identifier assigned by the database</summary>
		///	<returns>The record id</returns>
		long ITmaxMediaRecord.GetAutoId()
		{
			return AutoId;
		}
		
		/// <summary>This method retrieves the record's parent interface</summary>
		///	<returns>The parent interface</returns>
		ITmaxMediaRecord ITmaxMediaRecord.GetParent()
		{
			return (GetParent() as ITmaxMediaRecord);
		}
		
		/// <summary>This method retrieves the media type associated with the record</summary>
		///	<returns>The TrialMax media type</returns>
		FTI.Shared.Trialmax.TmaxMediaTypes ITmaxMediaRecord.GetMediaType()
		{
			return m_eMediaType;
		}

		/// <summary>This method retrieves the record's SplitScreen attribute</summary>
		///	<returns>True if split screen</returns>
		bool ITmaxMediaRecord.GetSplitScreen()
		{
			return GetSplitScreen();
		}

		/// <summary>This method retrieves an XML node that represents the record</summary>
		/// <param name="xmlDocument">XML document used to create the node</param>
		/// <param name="strName">Name to be assigned to the node</param>
		///	<returns>The XML representation of this record's properties</returns>
		XmlNode ITmaxMediaRecord.GetXmlNode(XmlDocument xmlDocument, string strName)
		{
			return ToXmlNode(xmlDocument, strName);
		}
		
		/// <summary>This method retrieves the parent/child media relationship that exists with the specified record</summary>
		///	<returns>The enumerated media relationship</returns>
		TmaxMediaRelationships ITmaxMediaRecord.GetRelationship(ITmaxMediaRecord IRecord)
		{
			return GetRelationship((CDxMediaRecord)IRecord, true);
		}
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			return GetListViewNames(iDisplayMode);
			
		}// string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			return GetListViewValues(iDisplayMode);
						
		}// string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			return GetListViewImage(iDisplayMode);
		}
		
		/// <summary>This function is called to get the Name bound to the object</summary>
		/// <returns>The object's name</returns>
		string ITmaxScriptBoxCtrl.GetName()
		{
			return GetName();
			
		}// string ITmaxScriptBoxCtrl.GetName()
		
		/// <summary>This function is called to get the MediaId bound to the object</summary>
		/// <returns>The object's name</returns>
		string ITmaxScriptBoxCtrl.GetMediaId()
		{
			return GetMediaId();
			
		}// string ITmaxScriptBoxCtrl.GetMediaId()
		
	}// public class CDxMediaRecord : CBaseRecord, ITmaxMediaRecord, ITmaxListViewCtrl, ITmaxScriptBoxCtrl

	/// <summary>
	/// This class is used to manage a dynamic array list of CDxMediaRecord objects.
	/// </summary>
	public class CDxMediaRecords : CBaseRecords
	{
		#region Protected Members
		
		/// <summary>Local member bound to DisplayText property</summary>
		protected string m_strDisplayText = "";
		
		#endregion Protected Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxMediaRecords() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxMediaRecords(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

        /// <summary>This function is called to set the database interface</summary>
        public virtual void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
        {
            base.SetDatabase(tmaxDatabase as CBaseDatabase);
            
        }// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)

        /// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxRecord">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public override CBaseRecord Add(CBaseRecord dxRecord)
		{
			string strSQL = "";

			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return null;
			if(this.Database == null) return null;
            if(this.Database.IsConnected == false) return null;
			
			try
			{
				try
				{
					//	Make sure the user information is set
                    ((CDxMediaRecord)dxRecord).CreatedBy = this.Database.GetUserId();
					((CDxMediaRecord)dxRecord).CreatedOn  = System.DateTime.Now;
					((CDxMediaRecord)dxRecord).ModifiedBy = ((CDxMediaRecord)dxRecord).CreatedBy;
					((CDxMediaRecord)dxRecord).ModifiedOn = ((CDxMediaRecord)dxRecord).CreatedOn;
				}
				catch(System.Exception Ex)
				{
                    FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_ADD_EX,TableName," "),Ex,GetErrorItems(dxRecord));
					return null;
				}
				
				//	Get the SQL statement					
				strSQL = GetSQLInsert(dxRecord);

				Debug.Assert(strSQL.Length > 0);

				//	Execute the statement
                if(this.Database.Execute(strSQL) == true)
				{	
					//	Get the id assigned by the database
                    ((CDxMediaRecord)dxRecord).AutoId = this.Database.GetAutoNumber();

					//	Add to the collection
					if(((CDxMediaRecord)dxRecord).AutoId != 0)
					{
						//	Set the ownership to be this collection
						dxRecord.Collection = this;
						
						//	Add it to the underlying array list
						base.Add(dxRecord as object);
						
						//	Add any coded properties that may belong to this record
						((CDxMediaRecord)dxRecord).ExchangeCodedProperties(true, false, false, false);
						
						return dxRecord;
					}
					else
					{
                        FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_INVALID_AUTO_ID,TableName,strSQL),GetErrorItems(dxRecord));
					}
				
				}//if(m_tmaxDatabase.Connection.Execute(strSQL)) == true)
				
			}
			catch(OleDbException oleEx)
			{
                FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_ADD_SQL_EX,TableName,strSQL),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
                FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_ADD_SQL_EX,TableName,strSQL),Ex,GetErrorItems(dxRecord));
			}
			
			return null;
			
		}// public override CBaseRecord Add(CBaseRecord dxRecord)

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			//	Make sure the user information is updated
            ((CDxMediaRecord)dxRecord).ModifiedBy = this.Database.GetUserId();
			((CDxMediaRecord)dxRecord).ModifiedOn = System.DateTime.Now;
				
			if(base.Update(dxRecord) == true)
			{
				((CDxMediaRecord)dxRecord).Modified = false;
				return true;
			}
			else
			{
				return false;
			}
			
		}// public override bool Update(CBaseRecord dxRecord)

		/// <summary>This function is called to synchronize the barcodes to the display order for all children belonging to this record</summary>
		/// <param name="bRecurse">true to recurse into child record collections</param>
		/// <returns>true if one or more records required synchronization</returns>
		public virtual bool Synchronize(bool bRecurse)
		{
			bool		bModified = false;
			long		lBarcode = 0;
			
			//	Do we need to update the barcode for this record?
			foreach(CDxMediaRecord O in this)
			{
				if(O.BarcodeId != O.DisplayOrder)
				{
					lBarcode = O.BarcodeId;
					O.BarcodeId = O.DisplayOrder;
					
					if(Update(O) == true)
					{
						bModified = true;
					}
					else
					{
						O.BarcodeId = lBarcode;
					}
				
				}// if(O.BarcodeId != O.DisplayOrder)
				
				//	Are we supposed to recurse into the child collections
				if((bRecurse == true) && (O.GetChildCollection() != null))
				{
					if(O.GetChildCollection().Synchronize(true) == true)
						bModified = true;
				}
				
			}// foreach(CDxMediaRecord O in this)
			
			return bModified;
		
		}// public virtual bool Synchronize(bool bRecurse)
		
		/// <summary>This function is called to reset the Codes associated all records in this collection</summary>
		public virtual void ResetCodes()
		{
			foreach(CDxMediaRecord O in this)
				O.ResetCodes();
		}
		
		/// <summary>This method is called to get the text representation of this record set</summary>
		/// <returns>The text descriptor</returns>
		public override string ToString()
		{
			if((m_strDisplayText != null) && (m_strDisplayText.Length > 0))
				return m_strDisplayText;
			else
				return base.ToString();
		}

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public virtual CDxMediaRecord Find(long lId)
		{
			return Find(lId, false);	
		}
			
		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public virtual CDxMediaRecord Find(long lId, bool bBarcode)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(CDxMediaRecord O in m_aList)
				{
					if(bBarcode == true)
					{
						if(O.BarcodeId == lId)
							return O;
					}
					else
					{
						if(O.AutoId == lId)
							return O;
					}
				}
			}

			return null;
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxMediaRecord this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return GetAt(index);
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxMediaRecord GetAt(int index)
		{
			// Use base class to process actual collection operation
			return (base.GetAt(index) as CDxMediaRecord);
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public virtual int IndexOf(CDxMediaRecord value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value as object);
		}
		
		/// <summary>This method is called to retrieve the foreign barcode for the specified record</summary>
		/// <returns>The foreign barcode associated with the specified record</returns>
		public virtual string GetForeignBarcode(CDxMediaRecord dxRecord)
		{
			CDxBarcode dxBarcode = null;

            if(this.Database == null) return "";
            if(this.Database.BarcodeMap == null) return "";
		
			try
			{
				//	Is this record in the map?
                if((dxBarcode = this.Database.BarcodeMap.FindSource(dxRecord)) != null)
					return dxBarcode.ForeignBarcode;
					
			}
			catch(OleDbException oleEx)
			{
                FireError("GetForeignBarcode",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_GET_FOREIGN_BARCODE_EX),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
                FireError("GetForeignBarcode",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_GET_FOREIGN_BARCODE_EX),Ex,GetErrorItems(dxRecord));
			}
			
			//	Not found or an exception occurred
			return "";
		
		}// public virtual string GetForeignBarcode()
			
		/// <summary>This method is called to set the foreign barcode for the specified record</summary>
		///	<param name="dxRecord">The record to be mapped</param>
		///	<param name="strForeignBarcode">The foreign barcode to be mapped to the record</param>
		///	<param name="bConfirm">True if the user should be prompted when duplicates exist</param>
		/// <returns>True if successful</returns>
		public virtual bool SetForeignBarcode(CDxMediaRecord dxRecord, string strForeignBarcode, bool bConfirm)
		{
			CDxBarcode	dxBarcode = null;
			CDxBarcodes	dxBarcodes = null;
			string		strMsg = "";

            if(this.Database == null) return false;
            if(this.Database.BarcodeMap == null) return false;
		
			try
			{
				//	Are we adding/updating the foreign barcode?
				if((strForeignBarcode != null) && (strForeignBarcode.Length > 0))
				{
					//	Get the collection of records already assigned this foreign barcode
                    if((dxBarcodes = this.Database.BarcodeMap.SearchForeign(strForeignBarcode)) != null)
					{
						//	Make sure the caller's record does not appear in the collection
						//
						//	NOTE:	It could be that the use is just changing the case and
						//			foreign barcodes are not case sensitive
						if((dxBarcode = dxBarcodes.FindSource(dxRecord)) != null)
							dxBarcodes.Remove(dxBarcode);
					}
					
					//	Do we have duplicates in the barcode map?
					//
					//	NOTE:	At the most there should only be one duplicate
					//			but we search for all just to play it safe
					if((dxBarcodes != null) && (dxBarcodes.Count > 0))
					{
						//	We can't delete an inherited foreign barcode
						if(dxBarcodes[0].Inherited == true)
						{
                            strMsg = this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_INHERITED_FOREIGN_BARCODE,dxRecord.GetBarcode(true),strForeignBarcode,dxBarcodes[0].GetSource().GetBarcode(true));
							
							if(bConfirm == true)
								MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							else
								FireError("SetForeignBarcode", strMsg);
								
							return false;
						}
						
						//	Should we prompt for confirmation before continuing
						if((bConfirm == true) && (dxBarcodes[0].GetSource() != null))
						{
							if(MessageBox.Show(strForeignBarcode + " is already mapped to " + dxBarcodes[0].GetSource().GetBarcode(true) + " Are you sure you want to replace this mapping?", "",
							                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
								return false;
						}
						
						//	Remove all duplicate records contained in the barcode map
                        this.Database.BarcodeMap.Delete(dxBarcodes);
						
						//	Update each of the source records
						foreach(CDxBarcode O in dxBarcodes)
						{
							if(O.GetSource() != null)
							{							
								try
								{
									O.GetSource().SetForeignBarcode("", false, false);
									O.GetSource().SetMapped(false);
									O.GetSource().Collection.Update(O.GetSource());
								
									//	Notify the application
									//
									//	NOTE:	This is the only means the application has to know that these
									//			records have been modified
                                    this.Database.FireInternalUpdate(O.GetSource());
								}
								catch
								{
								}
							
							}// if(O.GetSource() != null)
						
						}// foreach(CDxBarcode O in dxBarcodes)
						
						//	Clear the temporary collection
						dxBarcodes.Clear();
						dxBarcodes = null;
						
					}// if((dxBarcodes != null) && (dxBarcodes.Count > 0))

					//	Is this record already mapped?
                    if((dxBarcode = this.Database.BarcodeMap.FindSource(dxRecord)) != null)
					{
						dxBarcode.ForeignBarcode = strForeignBarcode;
                        this.Database.BarcodeMap.Update(dxBarcode);
					}
					else
					{
						//	Create a new map object
						dxBarcode = new CDxBarcode();

                        dxBarcode.PSTQ = this.Database.GetUniqueId(dxRecord);
						dxBarcode.ForeignBarcode = strForeignBarcode;

						//	Add to the map
                        this.Database.BarcodeMap.Add(dxBarcode);
			
						//	This record is now mapped
						dxRecord.SetMapped(true);
					}
				
				}
				else
				{
					//	Does the record have an entry in the barcode map?
					if(dxRecord.Mapped == true)
					{
                        if((dxBarcode = this.Database.BarcodeMap.FindSource(dxRecord)) != null)
                            this.Database.BarcodeMap.Delete(dxBarcode);
						
						//	This record is no longer mapped
						dxRecord.SetMapped(false);
					}
					else
					{
						//	Nothing to do
						return true;
					}
					
				}
					
				//	Update the record time stamps and attributes
				dxRecord.Collection.Update(dxRecord);
				
				return true;
			}
			catch(OleDbException oleEx)
			{
                FireError("SetForeignBarcode",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_SET_FOREIGN_BARCODE_EX,dxRecord.GetBarcode(true),strForeignBarcode),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
                FireError("SetForeignBarcode",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_SET_FOREIGN_BARCODE_EX,dxRecord.GetBarcode(true),strForeignBarcode),Ex,GetErrorItems(dxRecord));
			}
			
			return false;
		
		}// public virtual bool SetForeignBarcode(CDxMediaRecord dxRecord, string strForeignBarcode)
			
		/// <summary>This function is called to get the next valid barcode identifier</summary>
		/// <returns>The next available id</returns>
		public virtual long GetNextBarcodeId()
		{
			long lBarcodeId = 0;
			
			// Search for highest barcode in the collection
			if(m_aList != null)
			{
				foreach(CDxMediaRecord O in m_aList)
				{
					if(O.BarcodeId > lBarcodeId)
						lBarcodeId = O.BarcodeId;
				}
			}

			return (lBarcodeId + 1);
		
		}// public virtual long GetNextBarcodeId()
		
		/// <summary>This function is called to get the display order for the next object</summary>
		/// <returns>The display order index</returns>
		public virtual long GetNextDisplayOrder()
		{
			long lDisplayOrder = 1;
			
			foreach(CDxMediaRecord O in this)
			{
				if(lDisplayOrder <= O.DisplayOrder)
					lDisplayOrder = (O.DisplayOrder + 1);
			}
			
			return lDisplayOrder;
		
		}// public virtual long GetNextDisplayOrder()
		
		#endregion Public Methods
		
		#region Properties

        /// <summary>The active database</summary>
        new public CTmaxCaseDatabase Database
        {
            get { return (CTmaxCaseDatabase)(base.Database); }
            set { SetDatabase(value); }
        }

        /// <summary>Are code (fielded data) operations enabled</summary>
		public bool CodesEnabled
		{
			get { return (this.Database != null) ? this.Database.CodesEnabled : false; }
		}
		
		/// <summary>This property identifies the text descriptor for this record set</summary>
		public string DisplayText
		{
			get { return m_strDisplayText; }
			set { m_strDisplayText = value; }
		}
		
		#endregion Properties
		
	}//	public class CDxMediaRecord : CBaseRecord, ITmaxMediaRecord, ITmaxListViewCtrl, ITmaxScriptBoxCtrl
		
}// namespace FTI.Trialmax.Database
