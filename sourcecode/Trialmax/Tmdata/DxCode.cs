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
	/// <summary>This class manages the information for code attached to a media record</summary>
	public class CDxCode : FTI.Trialmax.Database.CDxMediaRecord, ITmaxPropGridCtrl
	{
		#region Constants
		
		public const int TMAX_LISTVIEW_DISPLAY_MODE_NORMAL = 0;
		public const int TMAX_LISTVIEW_DISPLAY_MODE_CONFIRM = 1;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to CaseCode property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to PickList property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickList = null;
		
		/// <summary>Local member bound to Owner property</summary>
		private CDxMediaRecord m_dxOwner = null;
		
		/// <summary>Local member bound to PrimaryId property</summary>
		private long m_lPrimaryId = 0;
		
		/// <summary>Local member bound to SecondaryId property</summary>
		private long m_lSecondaryId = 0;
		
		/// <summary>Local member bound to TertiaryId property</summary>
		private long m_lTertiaryId = 0;
		
		/// <summary>Local member bound to CaseCodeId property</summary>
		private long m_lCaseCodeId = 0;
		
		/// <summary>Local member bound to Type property</summary>
		private FTI.Shared.Trialmax.TmaxCodeTypes m_eType = TmaxCodeTypes.Unknown;
		
		/// <summary>Local member use to store the current value of the code as a string</summary>
		private string m_strValue = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxCode() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		/// <param name="tmaxCaseCode">The case code used to create the code</param>
		/// <param name="dxOwner">The media record that owns this object</param>
		public CDxCode(CTmaxCaseCode tmaxCaseCode, CDxMediaRecord dxOwner) : base()
		{
			Initialize(tmaxCaseCode, dxOwner);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="tmaxCaseCode">The case code used to create the code</param>
		public CDxCode(CTmaxCaseCode tmaxCaseCode) : base()
		{
			Initialize(tmaxCaseCode, null);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="dxOwner">The media record that owns this code</param>
		public CDxCode(CDxMediaRecord dxOwner) : base()
		{
			Initialize(null, dxOwner);
		}
		
		/// <summary>This method initializes the object's properties</summary>
		/// <param name="tmaxCaseCode">The case code used to create the code</param>
		/// <param name="dxOwner">The media record that owns this code</param>
		public void Initialize(CTmaxCaseCode tmaxCaseCode, CDxMediaRecord dxOwner)
		{
			SetCaseCode(tmaxCaseCode);
			SetOwner(dxOwner);
			
		}// public void Initialize(CTmaxCaseCode tmaxCaseCode, CDxMediaRecord dxOwner)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="dxRecord">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier (specific to derived class)</param>
		/// <returns>-1 if this result less than, 0 if equal, 1 if greater than</returns>
		public override int Compare(CBaseRecord dxRecord, long lMode)
		{
			CDxCode dxCompare = (CDxCode)dxRecord;
			int		iReturn = -1;
			
			//	Are these the same objects?
			if(ReferenceEquals(this, dxCompare) == true)
			{
				iReturn = 0;
			}
			else
			{
				if((this.CaseCode != null) && (dxCompare.CaseCode != null))
					iReturn = this.CaseCode.Compare(dxCompare.CaseCode, lMode);
				
				//	Is this a duplicate code?
				if(iReturn == 0)
				{
					if(this.AutoId > 0)
					{
						if(dxCompare.AutoId > 0)
						{
							if(this.AutoId < dxCompare.AutoId)
								iReturn = -1;
							else if(this.AutoId > dxCompare.AutoId)
								iReturn = 1;
						}
						else
						{
							iReturn = -1; // Unassigned always last
						}
						
					}
					else
					{
						if(dxCompare.AutoId > 0)
						{
							iReturn = 1; // Unassigned always last
						}
						else
						{
							//	Nothing else to test for
						}
						
					
					}// if(this.AutoId > 0)
						
				}// if(iReturn == 0)
				
			}
			
			return iReturn;
					
		}// public int Compare(CBaseRecord dxRecord, long lMode)
		
		/// <summary>This function is called to get the name associated with this record</summary>
		/// <returns>The name associated with this record</returns>
		public override string GetName()
		{
			//	Do we still need to look up the name for this code?
			if((m_strName.Length == 0) && (GetCaseCode(false) != null))
			{
				m_strName = GetCaseCode(false).Name;
			}
			return m_strName;
			
		}// public override string GetName()
		
		/// <summary>This method sets the owner record</summary>
		/// <param name="dxOwner">The media record that owns this code</param>
		public void SetOwner(CDxMediaRecord dxOwner)
		{
			CDxTertiary		dxTertiary = null;
			CDxSecondary	dxSecondary = null;
			
			//	Initialize the class members
			m_dxOwner = dxOwner;
			m_lPrimaryId = 0;
			m_lSecondaryId = 0;
			m_lTertiaryId = 0;
			
			if(dxOwner != null)
			{
				//	What media level is the owner?
				switch(dxOwner.GetMediaLevel())
				{
					case TmaxMediaLevels.Primary:
					
						m_lPrimaryId = ((CDxPrimary)dxOwner).AutoId;
						break;
						
					case TmaxMediaLevels.Secondary:
					
						dxSecondary = (CDxSecondary)dxOwner;
						m_lSecondaryId = dxSecondary.AutoId;
						Debug.Assert(dxSecondary.Primary != null);
						if(dxSecondary.Primary != null)
							m_lPrimaryId = dxSecondary.Primary.AutoId;
						break;
						
					case TmaxMediaLevels.Tertiary:
					
						dxTertiary = (CDxTertiary)dxOwner;
						m_lTertiaryId = dxTertiary.AutoId;
						
						Debug.Assert(dxTertiary.Secondary != null);
						if(dxTertiary.Secondary != null)
						{
							m_lSecondaryId = dxTertiary.Secondary.AutoId;
						
							Debug.Assert(dxTertiary.Secondary.Primary != null);
							if(dxTertiary.Secondary.Primary != null)
								m_lPrimaryId = dxTertiary.Secondary.Primary.AutoId;
							
						}
						break;
						
					default:
					
						Debug.Assert(false, "Invalid code owner");
						break;
				
				}// switch(dxOwner.GetMediaLevel())
				
			}// if(dxOwner != null)
			
		}// public void SetOwner(CDxMediaRecord dxOwner)
		
		/// <summary>This method uses the specified update object to set this object's properties</summary>
		/// <param name="tmaxUpdate">The code update used to set the properties</param>
		///	<returns>true if successful</returns>
		public bool SetProperties(CTmaxCodeUpdate tmaxUpdate)
		{
			bool bSuccessful = false;
			
			try
			{
				if(tmaxUpdate.CaseCode == null) return false;
				
				//	Reset the properties
				this.CaseCode = null;
				this.PickList = null;
				this.Value = "";
				
				//	Set the case code
				SetCaseCode(tmaxUpdate.CaseCode);
				
				//	Is this a pick list code?
				if(this.CaseCode.Type == TmaxCodeTypes.PickList)
				{
					//	Is this a multi-level pick list?
					if(this.CaseCode.IsMultiLevel == true)
					{
						if(tmaxUpdate.MultiLevelSelection != null)
						{
							this.PickList = tmaxUpdate.MultiLevelSelection.Parent;
							bSuccessful = this.SetValue(tmaxUpdate.MultiLevelSelection.UniqueId.ToString());
						}

					}
					else
					{
						//	Use the value bound to the update
						if(tmaxUpdate.Value.Length > 0)
						{
							//	This allows us to use the name instead of ID to set the value
							bSuccessful = ((ITmaxPropGridCtrl)this).SetValue(tmaxUpdate.Value);
						}
						
					}// if(this.CaseCode.IsMultiLevel == true)
					
				}
				else
				{
					if(tmaxUpdate.Value.Length > 0)
						bSuccessful = SetValue(tmaxUpdate.Value);
				
				}// if(this.CaseCode.Type == TmaxCodeTypes.PickList)
				
			}
			catch
			{
			
			}
			
			return bSuccessful;
			
		}// public bool SetProperties(CTmaxCodeUpdate tmaxUpdate)
		
		/// <summary>This method sets the code's case code information</summary>
		/// <param name="tmaxCaseCode">The case code used to create the code</param>
		public void SetCaseCode(CTmaxCaseCode tmaxCaseCode)
		{
			if(tmaxCaseCode != null)
			{
				m_tmaxCaseCode = tmaxCaseCode;
				m_lCaseCodeId  = tmaxCaseCode.UniqueId;
				m_eType		   = tmaxCaseCode.Type;
			}
			else
			{
				m_tmaxCaseCode = null;
				m_lCaseCodeId = -1;
				m_eType = TmaxCodeTypes.Unknown;
			}
			
		}// public void SetCaseCode(CTmaxCaseCode tmaxCaseCode)
		
		/// <summary>This function is called to get the case code that was used to create the code</summary>
		/// <param name="bRefresh">True to force a refresh of the case code object</param>
		/// <returns>The case code associated with this record</returns>
		public FTI.Shared.Trialmax.CTmaxCaseCode GetCaseCode(bool bRefresh)
		{
			//	Do we need to look up the case code?
			if((m_tmaxCaseCode == null) || (bRefresh == true))
			{
				//	Do we still need to look up the name for this code?
				if((this.Database != null) && (this.Database.CaseCodes != null))
				{
					m_tmaxCaseCode = this.Database.CaseCodes.Find(this.CaseCodeId);
				
				}// if((this.Database != null) && (this.Database.CaseCodes != null))
			
			}// if(m_tmaxCaseCode == null)
			
			//	Make sure we have the pick list bound to the case code
			if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.Type == TmaxCodeTypes.PickList))
			{
				if((m_tmaxCaseCode.PickList == null) || (bRefresh == true))
				{
					if((m_tmaxCaseCode.PickListId > 0) && (this.Database != null))
					{
						this.Database.GetPickList(m_tmaxCaseCode);
					}
				
				}// if((m_tmaxCaseCode.PickList == null) || (bRefresh == true))
			
			}// if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.Type == TmaxCodeTypes.PickList))

			return m_tmaxCaseCode;
			
		}// public FTI.Shared.Trialmax.CTmaxCaseCode GetCaseCode()
		
		/// <summary>This function is called to get the id of the case code that was used to create the code</summary>
		/// <returns>The id of the case code associated with this record</returns>
		public long GetCaseCodeId()
		{
			if(GetCaseCode(false) != null)
				return GetCaseCode(false).UniqueId;
			else
				return 0;
			
		}// public long GetCaseCodeId()
		
		/// <summary>This function is called to get the pick list that owns the value assigned to this code</summary>
		/// <param name="bRefresh">True to force a refresh of the pick list object</param>
		/// <returns>The pick list associated with this record</returns>
		public FTI.Shared.Trialmax.CTmaxPickItem GetPickList(bool bRefresh)
		{
			CTmaxPickItem	tmaxValue = null;
			long			lValueId = 0;
			
			//	Don't bother if this code is not bound to a pick list
			if((GetCaseCode(bRefresh) == null) || (m_tmaxCaseCode.Type != TmaxCodeTypes.PickList))
				return null;
			
			//	Do we need to assign the pick list?
			if((m_tmaxPickList == null) || (bRefresh == true))
			{
				//	Is this a multi-level code?
				if(this.IsMultiLevel == true)
				{
					//	We have to use the value assigned to this code to get the parent list
					if((lValueId = this.GetInteger()) > 0)
					{
						if((tmaxValue = this.Database.PickLists.FindValue(lValueId)) != null)
						{
							if((m_tmaxPickList = tmaxValue.Parent) == null)
								m_tmaxPickList = this.Database.PickLists.FindList(tmaxValue.ParentId);
							
						}
					
					}// if((lValueId = this.GetInteger()) > 0)
					
				}
				else
				{
					//	Use the pick list bound to the case code
					m_tmaxPickList = m_tmaxCaseCode.PickList;
				}
			
			}// if((m_tmaxPickList == null) || (bRefresh == true))
			
			return m_tmaxPickList;
			
		}// public FTI.Shared.Trialmax.CTmaxPickItem GetPickList(bool bRefresh)
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			//	Do we have an owner?
			if(m_dxOwner != null)
			{
				return m_dxOwner.GetMediaLevel();
			}
			else
			{
				if(m_lTertiaryId > 0)
					return FTI.Shared.Trialmax.TmaxMediaLevels.Tertiary;
				else if(m_lSecondaryId > 0)
					return FTI.Shared.Trialmax.TmaxMediaLevels.Secondary;
				else
					return FTI.Shared.Trialmax.TmaxMediaLevels.Primary;
			}
			
		}// public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Code;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//	We don't use properties with codes
			
		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	We don't use properties with codes
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		/// <summary>The method converts the current value to a string</summary>
		/// <returns>The current value as a string</returns>
		public override string ToString()
		{
			return m_strValue;
		}

		/// <summary>The method reads the current value</summary>
		/// <param name="bTranslate">True to translate pick value identifiers to their equivalent string</param>
		/// <returns>The current value as a string</returns>
		public string GetValue(bool bTranslate)
		{
			if(m_strValue == null)
				m_strValue = "";
				
			if((this.Type == TmaxCodeTypes.PickList) && (bTranslate == true) && (m_strValue.Length > 0))
			{
				return GetPickName(GetInteger(), null);
			}
			else
			{
				return m_strValue;
			}
				
		}// public string GetValue(bool bTranslate)
		
		/// <summary>This method is called to set the current value</summary>
		/// <param name="strValue">The new value for this code</param>
		/// <returns>true if successful</returns>
		public bool SetValue(string strValue)
		{
			bool bValid = true;

			//	Is the user attempting to delete this code?
			if((strValue == null) || (strValue.Length == 0))
			{
				m_strValue = "";
			}
			else if(CTmaxCaseCode.IsValid(m_eType, strValue, this.PickList) == true)
			{
				m_strValue = strValue;
			}
			else
			{
				bValid = false;
			}
			
			return bValid;
		
		}// public bool SetValue(string strValue)			
				
		/// <summary>This method is called to determine if the specified value is valid</summary>
		/// <param name="strValue">The new value for this code</param>
		/// <returns>true if valid</returns>
		public bool IsValid(string strValue)
		{
			//	Allow the user to NULL the code
			if((strValue == null) || (strValue.Length == 0))
			{
				return true;
			}
			else
			{
				//	Is this value valid for the type?
				return CTmaxCaseCode.IsValid(m_eType, strValue);
			}
		
		}// public bool IsValid(string strValue)			
				
		/// <summary>This method is called to determine if this is an unassigned (null) tag</summary>
		/// <returns>true if null</returns>
		public bool IsNull()
		{
			return ((m_strValue == null) || (m_strValue.Length == 0));
		}	
				
		/// <summary>The method reads the current value as a boolean</summary>
		/// <returns>The current value as a boolean</returns>
		public bool GetBool()
		{
			return CTmaxCaseCode.ToBool(m_strValue);	
		}
		
		/// <summary>The method reads the current value as a integer</summary>
		/// <returns>The current value as an integer</returns>
		public int GetInteger()
		{
			return CTmaxCaseCode.ToInteger(m_strValue);	
		}
		
		/// <summary>The method reads the current value as a floating point number</summary>
		/// <returns>The current value as a decimal number</returns>
		public double GetDecimal()
		{
			return CTmaxCaseCode.ToDecimal(m_strValue);	
		}
		
		/// <summary>The method reads the current value as a system date/time value</summary>
		/// <returns>The current value as a date/time value</returns>
		public System.DateTime GetDate()
		{
			return CTmaxCaseCode.ToDate(m_strValue);	
		}
		
		/// <summary>The method reads the current value as an object of the appropriate data type</summary>
		/// <returns>The current value as an object of the type defined by the code's code descriptor</returns>
		public object GetObject()
		{
			return CTmaxCaseCode.ToObject(m_eType, m_strValue);	
		}
		
		/// <summary>The method retrieves the bounded pick list value with the specified name</summary>
		/// <param name="strName">The name of the item to be located</param>
		/// <param name="tmaxPickList">The optional parent pick list to be searched</param>
		/// <returns>The pick item of the same name</returns>
		public CTmaxPickItem GetPickItem(string strName, CTmaxPickItem tmaxPickList)
		{
			CTmaxPickItem	tmaxPickItem = null;
			
			//	Don't bother if not of type pick list
			if(this.Type != TmaxCodeTypes.PickList) return null;
			
			//	Use the pick list assigned to this object if not specified by the caller
			if(tmaxPickList == null)
				tmaxPickList = this.PickList;
				
			//	Make sure we have the pick list
			if(tmaxPickList != null)
			{
				if(tmaxPickList.Children != null)
					tmaxPickItem = tmaxPickList.Children.Find(strName, !(tmaxPickList.CaseSensitive));
			}
			
			return tmaxPickItem;
			
		}// public CTmaxPickItem GetPickItem(string strName, CTmaxPickItem tmaxPickList)
		
		/// <summary>The method retrieves the bounded pick list value with the identifier</summary>
		/// <param name="lUniqueId">The id of the item to be located</param>
		/// <param name="tmaxPickList">The optional parent pick list to be searched</param>
		/// <returns>The pick item of the desired Id</returns>
		public CTmaxPickItem GetPickItem(long lUniqueId, CTmaxPickItem tmaxPickList)
		{
			CTmaxPickItem tmaxPickItem = null;
			
			//	Don't bother if not of type pick list
			if(this.Type != TmaxCodeTypes.PickList) return null;
			
			//	Use the pick list assigned to this object if not specified by the caller
			if(tmaxPickList == null)
				tmaxPickList = this.PickList;
				
			//	Make sure we have the pick list
			if(tmaxPickList != null)
			{
				if(tmaxPickList.Children != null)
					tmaxPickItem = tmaxPickList.Children.Find(lUniqueId);
			}
			
			return tmaxPickItem;
			
		}// public CTmaxPickItem GetPickItem(long lUniqueId)
		
		/// <summary>The method retrieves the id of the bounded pick list value with the specified name</summary>
		/// <param name="strName">The name of the item to be located</param>
		/// <param name="tmaxPickList">The optional parent pick list to be searched</param>
		/// <returns>The unique id of the pick item of the same name</returns>
		public long GetPickId(string strName, CTmaxPickItem tmaxPickList)
		{
			CTmaxPickItem	tmaxPickItem = null;
			long			lId = 0;
			
			if((tmaxPickItem = GetPickItem(strName, tmaxPickList)) != null)
				lId = tmaxPickItem.UniqueId;
			
			return lId;
			
		}// public CTmaxPickItem GetPickId(string strName, CTmaxPickItem tmaxPickList)
		
		/// <summary>The method retrieves the name of the bounded pick list value with the specified id</summary>
		/// <param name="lUniqueId">The Id of the item to be located</param>
		/// <param name="tmaxPickList">The optional parent pick list to be searched</param>
		/// <returns>The unique id of the pick item of the same id</returns>
		public string GetPickName(long lUniqueId, CTmaxPickItem tmaxPickList)
		{
			CTmaxPickItem	tmaxPickItem = null;
			string			strName = "";
			
			if((tmaxPickItem = GetPickItem(lUniqueId, tmaxPickList)) != null)
				strName = tmaxPickItem.Name;
			
			return strName;
			
		}// public string GetPickName(long lUniqueId, CTmaxPickItem tmaxPickList)
		
		/// <summary>This function is called to determine if the code is bound to a multi-level pick list item</summary>
		/// <returns>True if bound to a multi-level pick list item</returns>
		public bool GetIsMultiLevel()
		{
			if(this.CaseCode != null)
				return this.CaseCode.IsMultiLevel;
			else
				return false;
			
		}// public bool GetIsMultiLevel()
		
		/// <summary>This function is called to get the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		public override string[] GetListViewNames(int iDisplayMode)
		{
			string[] aNames = null;
			
			if(iDisplayMode == TMAX_LISTVIEW_DISPLAY_MODE_CONFIRM)
			{
				aNames = new string[3];
				aNames[0] = "MediaId";
				aNames[1] = "Field";
				aNames[2] = "Value";
			}
			else
			{
				aNames = new string[2];
				aNames[0] = "Name";
				aNames[1] = "Value";
			}

			return aNames;

		}// public override string[] GetListViewNames(int iDisplayMode)
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		public override string[] GetListViewValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			if(iDisplayMode == TMAX_LISTVIEW_DISPLAY_MODE_CONFIRM)
			{
				aValues = new string[3];
				if(this.Owner == null)
				{
					if((this.Database != null) && (this.Database.Primaries != null))
						this.Owner = this.Database.Primaries.Find(this.PrimaryId);
				}
				if(this.Owner != null)
					aValues[0] = this.Owner.GetBarcode(false);
				else
					aValues[0] = this.PrimaryId.ToString();
					
				aValues[1] = this.Name;
				aValues[2] = ((ITmaxPropGridCtrl)this).GetValue();
			}
			else
			{
				aValues = new string[2];
				aValues[0] = this.Name;
				aValues[1] = this.Value;
			}
			
			return aValues;

		}// public override string[] GetListViewValues(int iDisplayMode)
		
		/// <summary>This function is called to get the name used to display this object in the property grid</summary>
		/// <returns>The tag name</returns>
		string ITmaxPropGridCtrl.GetName()
		{
			return this.Name;
		}
		
		/// <summary>This function is called to get the value of this object in the property grid</summary>
		/// <returns>The current value</returns>
		string ITmaxPropGridCtrl.GetValue()
		{
			return this.GetValue(true);
		
		}// string ITmaxPropGridCtrl.GetValue()
		
		/// <summary>This function is called to get the category this object belongs to in the property grid</summary>
		/// <returns>null - tags don't use categories</returns>
		object ITmaxPropGridCtrl.GetCategory()
		{
			return null;
		}
		
		/// <summary>This function is called to get the option tag the property grid uses to identify this object in events if fires</summary>
		/// <returns>the reference to this object</returns>
		object ITmaxPropGridCtrl.GetTag()
		{
			return this;
		}

		/// <summary>This function is called to get the flag to indicate if the object should be visible in the property grid control</summary>
		/// <returns>true if visible</returns>
		bool ITmaxPropGridCtrl.GetVisible()
		{
			return true;
		}
		/// <summary>This function is called to compare the specified object to this object for sorting</summary>
		/// <param name="ICompare">The object to be compared</param>
		/// <param name="lSortOn">The owner specified sort order identifier</param>
		/// <returns>0 if equal, -1 if this object is less, 1 if greater</returns>
		int ITmaxPropGridCtrl.Compare(ITmaxPropGridCtrl ICompare, long lSortOn)
		{
			try
			{
				return this.Compare((CBaseRecord)ICompare, lSortOn);
			}
			catch
			{
				return -1;
			}
		
		}// int ITmaxPropGridCtrl.Compare(ITmaxPropGridCtrl ICompare)
		
		/// <summary>This function is called to get the id assigned to the property</summary>
		/// <returns>the object id</returns>
		long ITmaxPropGridCtrl.GetId()
		{
			return this.AutoId;
		}
		
		/// <summary>This function is called to get the case code assigned to the property</summary>
		/// <returns>the case code bound to this record</returns>
		CTmaxCaseCode ITmaxPropGridCtrl.GetCaseCode()
		{
			return this.CaseCode;
		}
		
		/// <summary>This function is called to get the pick list item assigned to the property</summary>
		/// <returns>the pick list item bound to this record</returns>
		CTmaxPickItem ITmaxPropGridCtrl.GetPickItem()
		{
			return this.GetPickItem(GetInteger(), null);
		}

		/// <summary>This function is called to set the object's value</summary>
		/// <param name="newValue">The new value of the object</param>
		/// <returns>true if successful</returns>
		bool ITmaxPropGridCtrl.SetValue(string strValue)
		{	
			if((strValue != null) && (strValue.Length > 0))
			{
				if(this.Type == TmaxCodeTypes.PickList)
				{
					CTmaxPickItem tmaxPickItem = null;
				
					if((tmaxPickItem = GetPickItem(strValue, null)) != null)
					{
						return this.SetValue(tmaxPickItem.UniqueId.ToString());
					}
					else
					{
						return false;
					}

				}
				else
				{
					return this.SetValue(strValue);
				}
				
			}
			else
			{
				return this.SetValue("");
			}
			
		}// bool ITmaxPropGridCtrl.SetValue(string strValue)
		
		/// <summary>This method is called to set the value of the multi-level property</summary>
		/// <param name="tmaxParent">The new parent pick list</param>
		/// <param name="strValue">The new value to be assigned to the property</param>
		/// <returns>True if successful</returns>
		bool ITmaxPropGridCtrl.SetValue(CTmaxPickItem tmaxParent, string strValue)
		{
			bool bSuccessful = false;
	
			//	Is this a multilevel code?
			if((this.IsMultiLevel == true) && (tmaxParent != null))
			{
				//	Update the parent pick list
				m_tmaxPickList = tmaxParent;
			}
				
			//	Assign the new value
			if((bSuccessful = ((ITmaxPropGridCtrl)this).SetValue(strValue)) == false)
				m_tmaxPickList = null; // Force a refresh
				
			return bSuccessful;
		}

		/// <summary>This function is called to set the flag to indicate if the object should be visible in the property grid control</summary>
		/// <param name="bVisible">true if visible</param>
		void ITmaxPropGridCtrl.SetVisible(bool bVisible)
		{
		}
		
		/// <summary>This function is called to get the editor best suited for this property</summary>
		/// <returns>The enumerated editor identifier</returns>
		TmaxPropGridEditors ITmaxPropGridCtrl.GetEditor()
		{
			//	What type of code is this?
			switch(this.Type)
			{
				case TmaxCodeTypes.Boolean:		return TmaxPropGridEditors.Boolean;
				case TmaxCodeTypes.Date:		return TmaxPropGridEditors.Date;
				case TmaxCodeTypes.Decimal:		return TmaxPropGridEditors.Double;
				case TmaxCodeTypes.Integer:		return TmaxPropGridEditors.Integer;
				case TmaxCodeTypes.Memo:		return TmaxPropGridEditors.Memo;
				case TmaxCodeTypes.PickList:	return this.IsMultiLevel ? TmaxPropGridEditors.MultiLevel : TmaxPropGridEditors.DropList;
				case TmaxCodeTypes.Text:	
				default:						return TmaxPropGridEditors.Text;
			}
		
		}// TmaxPropGridEditors ITmaxPropGridCtrl.GetEditor()
		
		/// <summary>This method is called to get the collection of drop list values for the property</summary>
		/// <returns>True if successful</returns>
		System.Collections.ICollection ITmaxPropGridCtrl.GetDropListValues()
		{
			if(this.CaseCode != null)
			{
				if(this.Type == TmaxCodeTypes.PickList)
				{
					if(this.PickList != null)
					{
						this.PickList.Children.Sort(true);
						return this.PickList.Children;
					}
					
				}
				
			}// if(this.CaseCode != null)
			
			return null;
		
		}// System.Collections.ICollection ITmaxPropGridCtrl.GetDropListValues()

        public bool FillCode()
        {
            string strSQL = "";
            bool bSuccessful = false;

            if (this.Database == null) return false;
            if (this.Database.IsConnected == false) return false;

            strSQL = GetSQLSelect();

       /*     if (strSQL.Length > 0)
                bSuccessful = Fill(strSQL);*/
            return bSuccessful;
        }

	    private string m_strTableName = "codes";
        public string GetSQLSelect()
        {

            string strSQL = "SELECT * FROM Code" + m_strTableName;

            //	There must be an owner record assigned to this collection
            Debug.Assert(m_dxOwner != null, "No owner record assigned to the collection");
            if (m_dxOwner == null) return "";

            strSQL += " WHERE PrimaryId = ";
            strSQL += this.PrimaryId.ToString();
            strSQL += " AND CaseCodeId = ";
            strSQL += this.CaseCodeId.ToString();
            strSQL += ";";

            return strSQL;

        }// public override string GetSQLSelect()

		#endregion Public Methods

		#region Properties
		
		/// <summary>The case code used to create this code</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCode CaseCode
		{
			get { return GetCaseCode(false); }
			set { SetCaseCode(value);	}
		}
		
		/// <summary>The pick list that owns this code's value</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickList
		{
			get { return GetPickList(false); }
			set { m_tmaxPickList = value; }
		}
		
		/// <summary>The media record that owns this code</summary>
		public CDxMediaRecord Owner
		{
			get { return m_dxOwner; }
			set { m_dxOwner = value;	}
		}
		
		/// <summary>The AutoId of the primary record that owns this record</summary>
		public long PrimaryId
		{
			get { return m_lPrimaryId; }
			set { m_lPrimaryId = value;	}
		}
		
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
		
		/// <summary>The id of the case code used to create this code</summary>
		public long CaseCodeId
		{
			get { return m_lCaseCodeId; }
			set { m_lCaseCodeId = value;	}
		}
		
		/// <summary>The enumerated data type identifier</summary>
		public FTI.Shared.Trialmax.TmaxCodeTypes Type
		{
			get { return m_eType; }
			set { m_eType = value;	}
		}
		
		/// <summary>The value assigned to the code</summary>
		public string Value
		{
			get { return GetValue(false); }
			set { SetValue(value);	}
		}
		
		/// <summary>true if this code is bound to a multi-level pick list item</summary>
		public bool IsMultiLevel
		{
			get { return GetIsMultiLevel(); }
		}
		
		#endregion Properties
	
	}// class CDxCode

	/// <summary>This class manages a collection of CDxCode objects</summary>
	public class CDxCodes : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			PrimaryId,
			SecondaryId,
			TertiaryId,
			CaseCodeId,
			Type,
			valueInteger,
			valueDecimal,
			valueBoolean,
			valueDateTime,
			valueText,
			valueMemo,
			valuePickList,
			SpareNumber,
			SpareText,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "Codes";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Owner property</summary>
		private CDxMediaRecord m_dxOwner = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxCodes() : base()
		{
			//	Initialization code is in override Initialize()
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxCodes(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			//	Initialization code is in override Initialize()
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxCode">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxCode Add(CDxCode dxCode)
		{
			return (CDxCode)base.Add(dxCode);
			
		}// public CDxCode Add(CDxCode dxCode)

		/// <summary>This method is called to add the codes associated with a bulk update</summary>
		/// <param name="tmaxUpdates">The updates that define the bulk operation</param>
		/// <param name="dxPrimaries">The primary records being updated</param>
		///	<returns>true if successful</returns>
		public bool Add(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)
		{
			CDxCode dxCode = new CDxCode();
			bool	bSuccessful = false;
			
			if(tmaxUpdates == null) return false;
			if(dxPrimaries == null) return false;
			
			try
			{
				//	Search for all updates that define an Add action
				foreach(CTmaxCodeUpdate O in tmaxUpdates)
				{
					if(O.Action != TmaxCodeActions.Add) continue;
					if(O.CaseCode == null) continue;
					
					//	Set the code's property values 
					if(dxCode.SetProperties(O) == true)
					{
						//	Add this code to each of the specified primary records
						foreach(CDxPrimary dxPrimary in dxPrimaries)
						{
							//	Establish the ownership
							this.Owner = dxPrimary;
							dxCode.SetOwner(dxPrimary);
							
							//	Add the code
							this.Add(dxCode);
							
							//	This keeps the collection from getting big
							this.Clear();
						}
					
					}// if(dxCode.SetProperties(O) == true)				
					
				}// foreach(CTmaxCodeUpdate O in tmaxUpdates)
				
				bSuccessful = true;
			
			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// public bool Add(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)

		/// <summary>This method will perform cleanup of local resources</summary>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxCodes Dispose()
		{
			return (CDxCodes)base.Dispose();
			
		}// public new CDxCodes Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxCode Find(long lAutoId)
		{
			return (CDxCode)base.Find(lAutoId);
			
		}//	public new CDxCode Find(long lAutoId)

		/// <summary>Called to locate the all objects with the specified case code identifier</summary>
		/// <param name="lCodeId">The id of case codes to be located</param>
		/// <returns>All objects with the specified case code identifier</returns>
		public CDxCodes FindAll(long lCodeId)
		{
			CDxCodes dxCodes = new CDxCodes(this.Database);
			
			foreach(CDxCode O in this)
			{
				if(O.GetCaseCodeId() == lCodeId)
					dxCodes.AddList(O);
			}
			
			if((dxCodes != null) && (dxCodes.Count > 0))
				return dxCodes;
			else
				return null;
			
		}//	public CDxCodes FindAll(long lCodeId)

		/// <summary>Called to locate the first object with the specified case code type identifier</summary>
		/// <param name="eType">The type of case code to be located</param>
		/// <returns>The first object with the specified type</returns>
		public CDxCode Find(TmaxCodeTypes eType)
		{
			foreach(CDxCode O in this)
			{
				if(O.Type == eType)
					return O;
			}
			return null;
			
		}//	public new CDxCode Find(TmaxCodeTypes eType))

		/// <summary>Called to locate the all objects with the specified case code type identifier</summary>
		/// <param name="eType">The type of case code to be located</param>
		/// <returns>All objects with the specified type</returns>
		public CDxCodes FindAll(TmaxCodeTypes eType)
		{
			CDxCodes dxCodes = new CDxCodes(this.Database);
			
			foreach(CDxCode O in this)
			{
				if(O.Type == eType)
					dxCodes.AddList(O);
			}
			
			if((dxCodes != null) && (dxCodes.Count > 0))
				return dxCodes;
			else
				return null;
			
		}//	public CDxCodes FindAll(TmaxCodeTypes eType)

		/// <summary>Called to locate the first object created with the specified case code</summary>
		/// <param name="tmaxCaseCode">The case code used to create the object</param>
		/// <returns>The first object created with the specified code</returns>
		public CDxCode Find(CTmaxCaseCode tmaxCaseCode)
		{
			foreach(CDxCode O in this)
			{
				if(O.GetCaseCodeId() == tmaxCaseCode.UniqueId)
					return O;
			}
			return null;
		}

		/// <summary>Called to locate all objects created with the specified case code</summary>
		/// <param name="tmaxCaseCode">The case code used to create the objects</param>
		/// <returns>All objects created with the specified code</returns>
		public CDxCodes FindAll(CTmaxCaseCode tmaxCaseCode)
		{
			return FindAll(tmaxCaseCode.UniqueId);
		}

		/// <summary>Called to locate the first object mapped to the specified property</summary>
		/// <param name="eCodedProperty">The enumerated coded property identifier</param>
		/// <returns>The first code mapped to the specified property</returns>
		public CDxCode Find(TmaxCodedProperties eCodedProperty)
		{
			long lCodeId = 0;
			
			//	Get the code identifier for this coded property
			lCodeId = CTmaxCaseCodes.GetCodeId(eCodedProperty);
			
			foreach(CDxCode O in this)
			{
				if(O.GetCaseCodeId() == lCodeId)
					return O;
			}
			return null;
		
		}// public CDxCode Find(TmaxCodedProperties eCodedProperty)

		/// <summary>Called to locate all objects mapped to the specified property</summary>
		/// <param name="eCodedProperty">The enumerated coded property identifier</param>
		/// <returns>All objects mapped to the specified property</returns>
		public CDxCodes FindAll(TmaxCodedProperties eCodedProperty)
		{
			return FindAll(CTmaxCaseCodes.GetCodeId(eCodedProperty));
		}

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxCode this[int iIndex]
		{
			get
			{
				return (CDxCode)base[iIndex];
			}
		
		}// public new CDxCode this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxCode GetAt(int iIndex)
		{
			return (CDxCode)base.GetAt(iIndex);
		}

		/// <summary>This method is called to populate the collection with codes that reference the specified pick item or one of its children</summary>
		/// <param name="tmaxPickItem">The pick item referenced by the desired codes</param>
		/// <returns>true if successful</returns>
		public bool Fill(CTmaxPickItem tmaxPickItem)
		{
			string	strSQL = "";
			bool	bSuccessful = false;
			
			if(this.Database == null) return false;
            if(this.Database.IsConnected == false) return false;
		
			if(tmaxPickItem != null)
			{
				strSQL = GetSQLSearch(tmaxPickItem);

				if(strSQL.Length > 0)
					bSuccessful = Fill(strSQL);
			}
			
			return bSuccessful;
		
		}// public virtual bool Fill(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method is called to populate the collection with codes that reference the specified case code</summary>
		/// <param name="tmaxCaseCode">The case code referenced by the desired codes</param>
		/// <returns>true if successful</returns>
		public bool Fill(CTmaxCaseCode tmaxCaseCode)
		{
			string	strSQL = "";
			bool	bSuccessful = false;
			
			if(this.Database == null) return false;
			if(this.Database.IsConnected == false) return false;
		
			if(tmaxCaseCode != null)
			{
				strSQL = GetSQLSearch(tmaxCaseCode);

				if(strSQL.Length > 0)
					bSuccessful = Fill(strSQL);
			}
			
			return bSuccessful;
		
		}// public virtual bool Fill(CTmaxPickItem tmaxPickItem)

        public bool Fill(CTmaxCaseCode tmaxCaseCode, CDxMediaRecord dxOwner)
        {
            string strSQL = "";
            bool bSuccessful = false;

            if (this.Database == null) return false;
            if (this.Database.IsConnected == false) return false;

            if (tmaxCaseCode != null)
            {
                strSQL = GetSQLSearch(tmaxCaseCode,dxOwner);

                if (strSQL.Length > 0)
                    bSuccessful = Fill(strSQL);
            }

            return bSuccessful;

        }// public virtual bool Fill(CTmaxCaseCode tmaxCaseCode, CDxMediaRecord dxOwner)
		
		/// <summary>This method is called to get the SQL statement required to get codes that reference the specified pick list item or one of its children</summary>
		/// <param name="tmaxPickItem">The pick item being referenced</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLSearch(CTmaxPickItem tmaxPickItem)
		{
			string strSQL = "";
			CTmaxPickItems	tmaxValues = null;
			
			//	Get the collection of pick list values associated with the specified item
			if((tmaxValues = tmaxPickItem.GetValueItems()) != null)
			{
				if(tmaxValues.Count > 0)
				{
					strSQL = "SELECT * FROM " + m_strTableName;
					strSQL += (" WHERE (" + eFields.Type.ToString() + " = " + ((int)(TmaxCodeTypes.PickList)).ToString());
					strSQL += (" AND " + eFields.valuePickList.ToString() + " IN (");
					
					for(int i = 0; i < tmaxValues.Count; i++)
					{
						strSQL += tmaxValues[i].UniqueId.ToString();
						
						if(i < tmaxValues.Count - 1)
							strSQL += ",";
					}
					
					strSQL += "));";
				
				}// if(tmaxValues.Count > 0)
				
			}// if((tmaxValues = tmaxPickItem.GetValueItems()) != null)
			
			return strSQL;
		
		}// public string GetSQLSearch(CTmaxPickItem tmaxPickItem)

		/// <summary>This method is called to get the SQL statement required to get codes that reference the specified case code</summary>
		/// <param name="tmaxCaseCode">The case code being referenced</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLSearch(CTmaxCaseCode tmaxCaseCode)
		{
			string strSQL = "";
			
			strSQL = "SELECT * FROM " + m_strTableName;
			strSQL += (" WHERE " + eFields.CaseCodeId.ToString() + " = " + tmaxCaseCode.UniqueId.ToString() + ";");

			return strSQL;
		
		}// public string GetSQLSearch(CTmaxCaseCode tmaxCaseCode)

        public string GetSQLSearch(CTmaxCaseCode tmaxCaseCode,CDxMediaRecord dxMediaRecord)
        {
            string strSQL = "";

            strSQL = "SELECT * FROM " + m_strTableName;
            strSQL += " WHERE " + eFields.CaseCodeId.ToString() + " = " + tmaxCaseCode.UniqueId.ToString() ;
            strSQL += " and " + eFields.PrimaryId.ToString() + "=" + dxMediaRecord.AutoId.ToString() +";";

            return strSQL;

        }

		/// <summary>This method is called to delete the codes in the specified collection</summary>
		/// <param name="dxCodes">The codes to be deleted</param>
		///	<returns>true if successful</returns>
		public bool Delete(CDxCodes dxCodes)
		{
			string strSQL = GetSQLDelete(dxCodes);
			
			if((strSQL != null) && (strSQL.Length > 0))
				return Delete(strSQL);
			else
				return false;
			
		}// public virtual bool Delete(CDxCodes dxCodes)

		/// <summary>This method is called to delete the codes associated with a bulk update</summary>
		/// <param name="tmaxUpdates">The updates that define the bulk operation</param>
		/// <param name="dxPrimaries">The primary records being updated</param>
		///	<returns>true if successful</returns>
		public bool Delete(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)
		{
			string strSQL = GetSQLDelete(tmaxUpdates, dxPrimaries);
			
			if((strSQL != null) && (strSQL.Length > 0))
			{
				return Delete(strSQL);
			}
			else
			{
				return false;
			}
			
		}// public virtual bool Delete(CDxCodes dxCodes)

		/// <summary>This method is called to get the SQL statement required to delete the codes in the specified collection</summary>
		/// <param name="dxCodes">The codes to be deleted</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLDelete(CDxCodes dxCodes)
		{
			string strSQL = "";
			
			if(dxCodes == null) return "";
			if(dxCodes.Count == 0) return "";
			
			strSQL = "DELETE FROM " + m_strTableName;
			strSQL += (" WHERE " + eFields.AutoId.ToString() + " IN (");
					
			for(int i = 0; i < dxCodes.Count; i++)
			{
				strSQL += dxCodes[i].AutoId.ToString();
						
				if(i < dxCodes.Count - 1)
					strSQL += ",";
			}
					
			strSQL += ");";
			
			return strSQL;
		
		}// public string GetSQLDelete(CDxCodes dxCodes)

		/// <summary>This method is called to get the SQL statement required to delete the codes associated with a bulk update</summary>
		/// <param name="tmaxUpdates">The updates that define the bulk operation</param>
		/// <param name="dxPrimaries">The primary records being updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLDelete(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)
		{
			string strSQL = "";
			string strCodes = "";
			string strPrimaries = "";
			
			if(tmaxUpdates == null) return "";
			if(tmaxUpdates.Count == 0) return "";
			
			//	Get the collection of codes to be deleted
			foreach(CTmaxCodeUpdate O in tmaxUpdates)
			{
				if((O.Action == TmaxCodeActions.Add) || (O.Action == TmaxCodeActions.Delete))
				{
					if(O.CaseCode != null)
					{
						if(strCodes.Length > 0)
							strCodes += ",";
						strCodes += (O.CaseCode.UniqueId.ToString());
					}
				
				}// if((O.Action == TmaxCodeActions.Add) || (O.Action == TmaxCodeActions.Delete))
				
			}// foreach(CTmaxCodeUpdate O in tmaxUpdates)
			
			//	NO CODES = NO UPDATE
			if(strCodes.Length == 0) return "";
			
			//	Did the caller specify particular primary records?
			if((dxPrimaries != null) && (dxPrimaries.Count > 0))
			{
				//	Build the set of primary record identifiers
				foreach(CDxPrimary O in dxPrimaries)
				{
					if(strPrimaries.Length > 0)
						strPrimaries += ",";
					strPrimaries += O.AutoId.ToString();
				}
				
				strSQL = String.Format("DELETE FROM {0} WHERE (({1} IN ({2})) AND ({3} IN ({4})))",
										m_strTableName,
										eFields.CaseCodeId,
										strCodes,
										eFields.PrimaryId,
										strPrimaries);
			}
			else
			{
				//	Delete ALL codes of the specified type
				strSQL = String.Format("DELETE FROM {0} WHERE {1} IN ({2})",
									   m_strTableName,
									   eFields.CaseCodeId,
									   strCodes);
			
			}// if((dxPrimaries != null) && (dxPrimaries.Count > 0))
			
			return strSQL;
		
		}// public string GetSQLDelete(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)

		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string		strSQL = "SELECT * FROM " + m_strTableName;
			CDxCode	dxCode = new CDxCode(m_dxOwner);
			
			//	There must be an owner record assigned to this collection
			Debug.Assert(m_dxOwner != null, "No owner record assigned to the collection");
			if(m_dxOwner == null) return "";
			
			strSQL += " WHERE PrimaryId = ";
			strSQL += dxCode.PrimaryId.ToString();
			strSQL += " AND SecondaryId = ";
			strSQL += dxCode.SecondaryId.ToString();
			strSQL += " AND TertiaryId = ";
			strSQL += dxCode.TertiaryId.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLSelect()

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxCode	dxCode = (CDxCode)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			//	There must be an owner record assigned to the code
			Debug.Assert(dxCode.Owner != null, "No owner record assigned to the collection");
			if(dxCode.Owner == null) return "";
			
			strSQL += (eFields.PrimaryId.ToString() + ",");
			strSQL += (eFields.SecondaryId.ToString() + ",");
			strSQL += (eFields.TertiaryId.ToString() + ",");
			strSQL += (eFields.CaseCodeId.ToString() + ",");
			strSQL += (eFields.Type.ToString() + ",");
			strSQL += ((GetValueField(dxCode.Type)).ToString());
			
			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString());
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString());

			strSQL += ")";

			strSQL += " VALUES(";
			strSQL += ("'" + dxCode.PrimaryId.ToString() + "',");
			strSQL += ("'" + dxCode.SecondaryId.ToString() + "',");
			strSQL += ("'" + dxCode.TertiaryId.ToString() + "',");
			strSQL += ("'" + dxCode.CaseCodeId.ToString() + "',");
			strSQL += ("'" + ((int)(dxCode.Type)).ToString() + "',");
			strSQL += ("'" + GetValueString(dxCode) + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += (",'" + dxCode.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += (",'" + dxCode.ModifiedOn.ToString() + "'");

			strSQL += ")";

			return strSQL;
		
		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxCode	dxCode = (CDxCode)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.PrimaryId.ToString() + " = '" + dxCode.PrimaryId.ToString() + "',");
			strSQL += (eFields.SecondaryId.ToString() + " = '" + dxCode.SecondaryId.ToString() + "',");
			strSQL += (eFields.TertiaryId.ToString() + " = '" + dxCode.TertiaryId.ToString() + "',");
			strSQL += (eFields.CaseCodeId.ToString() + " = '" + dxCode.CaseCodeId.ToString() + "',");
			strSQL += (eFields.Type.ToString() + " = '" + ((int)(dxCode.Type)).ToString() + "',");
			strSQL += (GetValueField(dxCode.Type).ToString() + " = '" + GetValueString(dxCode) + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString() + " = '" + dxCode.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString() + " = '" + dxCode.ModifiedOn.ToString() + "'");

			strSQL += " WHERE AutoId = ";
			strSQL += dxCode.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to create the table</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreate()
		{
			string strSQL = "CREATE TABLE " + TableName;
			
			strSQL += "(";
			
			strSQL += "AutoId AUTOINCREMENT, ";
			strSQL += "PrimaryId LONG, ";
			strSQL += "SecondaryId LONG, ";
			strSQL += "TertiaryId LONG, ";
			strSQL += "CaseCodeId LONG, ";
			strSQL += "Type SHORT, ";
			
			strSQL += "valueInteger LONG, ";
			strSQL += "valueDecimal FLOAT, ";
			strSQL += "valueBoolean BIT, ";
			strSQL += "valueDateTime DATETIME, ";
			strSQL += "valueText TEXT(255), ";
			strSQL += "valueMemo MEMO, ";
			strSQL += "valuePickList LONG, ";
			
			strSQL += "SpareNumber LONG, ";
			strSQL += "SpareText TEXT(255), ";
			strSQL += "ModifiedBy LONG, ";
			strSQL += "ModifiedOn DATETIME";

			strSQL += ")";
			
			return strSQL;

		}// public override string GetSQLCreate()
		
		/// <summary>This method is called to get the SQL statement required to create the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreateColumn(int iColumn)
		{
			string strSQL = "";

			try
			{
				if(iColumn == ((int)(eFields.valuePickList)))
				{
					//	Format the SQL statement to add the column				
					strSQL = "ALTER TABLE ";
					strSQL += this.TableName;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.valuePickList.ToString() + " ");
					strSQL += "LONG ";
					strSQL += "DEFAULT 0";
				
					strSQL += " ;";

				}
				else if(iColumn == ((int)(eFields.ModifiedBy)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += this.TableName;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedBy.ToString() + " ");
					strSQL += "LONG ";
					strSQL += "DEFAULT 0";
				
					strSQL += " ;";

				}
				else if(iColumn == ((int)(eFields.ModifiedOn)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += this.TableName;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedOn.ToString() + " ");
					strSQL += "DATETIME;";

				}
				
			}
			catch
			{
			}
			
			return strSQL;
		
		}// public virtual string GetSQLCreateColumn(int iColumn)
		
		/// <summary>This method is called to get the SQL statement required to flush all records belonging to the collection</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string	strSQL = "";
			CDxCode	dxCode = null;
			
			//	Assume we are flushing the whole table if no owner assigned
			if(m_dxOwner == null)
			{
				strSQL = ("DELETE * FROM " + this.TableName);
			}
			else
			{
				dxCode = new CDxCode(m_dxOwner);

				strSQL = ("DELETE FROM " + this.TableName);
				
				switch(m_dxOwner.GetMediaLevel())
				{
					case TmaxMediaLevels.Primary:
					
						strSQL += " WHERE PrimaryId = ";
						strSQL += dxCode.PrimaryId.ToString();
						strSQL += ";";
						break;
						
					case TmaxMediaLevels.Secondary:
					
						strSQL += " WHERE SecondaryId = ";
						strSQL += dxCode.SecondaryId.ToString();
						strSQL += ";";
						break;
						
					case TmaxMediaLevels.Tertiary:
					
						strSQL += " WHERE TertiaryId = ";
						strSQL += dxCode.TertiaryId.ToString();
						strSQL += ";";
						break;
						
					default:
					
						Debug.Assert(false, "Invalid owner record on meta tag flush");
						strSQL = "";
						break;
				
				}// switch(m_dxOwner.GetMediaLevel())
			
			}// if(m_dxOwner == null)
			
			return strSQL;
		
		}// public override string GetSQLFlush()
		
		/// <summary>This function is called to get a code object</summary>
		/// <param name="tmaxCode">The case code used to initialize the object</param>
		/// <returns>A new code object</returns>
		public CDxCode Alloc(CTmaxCaseCode tmaxCode)
		{
			CDxCode dxCode = null;
			
			try
			{
				if((dxCode = new CDxCode(tmaxCode, this.Owner)) != null)
					dxCode.Collection = this;
			}
			catch
			{
			}
			
			return (dxCode);
		
		}// public CDxCode Alloc(CTmaxCaseCode tmaxCode)
		
		/// <summary>This function is called to get a code object</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <returns>A new code object</returns>
		public CDxCode Alloc(TmaxCodedProperties e)
		{
			CTmaxCaseCode tmaxCode = null;
			
			//	Get the case code associated with this property
			if((this.Database != null) && (this.Database.CaseCodes != null))
				tmaxCode = this.Database.CaseCodes.Find(e);
				
			if(tmaxCode != null)
				return Alloc(tmaxCode);
			else
				return null;
		
		}// public CDxCode Alloc(TmaxCodedProperties e)
		
		/// <summary>This function is called to refresh the case code reference for each object</summary>
		/// <returns>True if successful</returns>
		public bool RefreshCaseCodes()
		{
			bool bSuccessful = true;
			
			foreach(CDxCode O in this)
			{
				try 
				{ 
					if(O.GetCaseCode(true) == null)
						bSuccessful = false;
				}
				catch 
				{
					bSuccessful = false;
				}
				
			}// foreach(CDxCode O in this)
			
			return bSuccessful;
		
		}// public bool RefreshCaseCodes()

        /// <summary>This function is called to set the database interface</summary>
        public override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
        {
            //	Do the base class processing first
            base.SetDatabase(tmaxDatabase);

            //	 Set the indexes of the fields that may have been added by the database
            //
            //	NOTE:	These fields may appear in different locations depending upon
            //			whether or not they were added by the database
            SetFieldIndexes();

        }// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)

        #endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to initialize the object</summary>
		protected override void Initialize()
		{
			//	Do the base class processing first
			base.Initialize();
			
			//	Initialize for sorting
			this.KeepSorted = false;
			
		}// protected override void Initialize()
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			return ((CBaseRecord)Alloc(null));
		
		}// protected override CBaseRecord GetNewRecord()
		
		/// <summary>This method is called to exchange data between the code objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the code values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxCode dxCode = (CDxCode)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the code values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxCode.AutoId;
					m_dxFields[(int)eFields.PrimaryId].Value  = dxCode.PrimaryId;
					m_dxFields[(int)eFields.SecondaryId].Value  = dxCode.SecondaryId;
					m_dxFields[(int)eFields.TertiaryId].Value  = dxCode.TertiaryId;
					m_dxFields[(int)eFields.CaseCodeId].Value  = dxCode.CaseCodeId;
					m_dxFields[(int)eFields.Type].Value  = dxCode.Type;
					m_dxFields[(int)GetValueField(dxCode.Type)].Value  = dxCode.GetObject();

				}
				else
				{
					dxCode.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxCode.PrimaryId = (int)(m_dxFields[(int)eFields.PrimaryId].Value);
					dxCode.SecondaryId = (int)(m_dxFields[(int)eFields.SecondaryId].Value);
					dxCode.TertiaryId = (int)(m_dxFields[(int)eFields.TertiaryId].Value);
					dxCode.CaseCodeId = (int)(m_dxFields[(int)eFields.CaseCodeId].Value);
					dxCode.Type = (FTI.Shared.Trialmax.TmaxCodeTypes)((short)(m_dxFields[(int)eFields.Type].Value));
					dxCode.Value = (m_dxFields[(int)GetValueField(dxCode.Type)].Value).ToString();
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
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		#endregion Protected Methods
	
		#region Private Methods
		
		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();
			
			//	This prevents attempts to retrieve the spare values
			if((m_dxFields != null) && ((int)eFields.SpareNumber < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareNumber)].Index = -1;
			if((m_dxFields != null) && ((int)eFields.SpareText < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareText)].Index = -1;
			
			//	 Set the indexes of the fields that may have been added by the database
			//
			//	NOTE:	These fields may appear in different locations depending upon
			//			whether or not they were added by the database
			SetFieldIndexes();
			
		}// protected override void SetFields()
		
		/// <summary>This function is called to set the indexes of fields that may have been added by the database</summary>
		protected void SetFieldIndexes()
		{
			if(this.Database != null)
			{
				if((m_dxFields != null) && ((int)eFields.valuePickList < m_dxFields.Count))
					m_dxFields[(int)(eFields.valuePickList)].Index = this.Database.CodesIndexValuePickList;
				
				if((m_dxFields != null) && ((int)eFields.ModifiedBy < m_dxFields.Count))
					m_dxFields[(int)(eFields.ModifiedBy)].Index = this.Database.CodesIndexModifiedBy;
				
				if((m_dxFields != null) && ((int)eFields.ModifiedOn < m_dxFields.Count))
					m_dxFields[(int)(eFields.ModifiedOn)].Index = this.Database.CodesIndexModifiedOn;
			}
			
		}// protected void SetFieldIndexes()
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
		}
		
		/// <summary>This method will set the default value for the specified code</summary>
		/// <param name="eField">The enumerated code identifier</param>
		/// <param name="dxField">The code object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.AutoId:
				case eFields.PrimaryId:
				case eFields.SecondaryId:
				case eFields.TertiaryId:
				case eFields.CaseCodeId:
				case eFields.SpareNumber:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.Type:
				
					dxField.Value = FTI.Shared.Trialmax.TmaxCodeTypes.Unknown;
					break;
					
				case eFields.SpareText:
				
					dxField.Value = "";
					break;
					
				case eFields.ModifiedOn:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				case eFields.valueInteger:
				case eFields.valueDecimal:
				case eFields.valueDateTime:
				case eFields.valueBoolean:
				case eFields.valueText:
				case eFields.valueMemo:
				case eFields.valuePickList:
				
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown code identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		/// <summary>This method retrieves the appropriate value code for the specified type</summary>
		/// <param name="eType">The case code type identifier</param>
		/// <returns>The value code appropriate for the specified code type</returns>
		private eFields GetValueField(TmaxCodeTypes eType)
		{
			switch(eType)
			{
				case TmaxCodeTypes.Integer:
					return eFields.valueInteger;
				case TmaxCodeTypes.Decimal:
					return eFields.valueDecimal;
				case TmaxCodeTypes.Boolean:
					return eFields.valueBoolean;
				case TmaxCodeTypes.Date:
					return eFields.valueDateTime;
				case TmaxCodeTypes.Memo:
					return eFields.valueMemo;
				case TmaxCodeTypes.PickList:
					return eFields.valuePickList;
				case TmaxCodeTypes.Text:
				default:
					return eFields.valueText;
			
			}// switch(eType)
			
		}// private eFields GetValueField(CDxCode dxCode)
		
		/// <summary>This method retrieves the appropriate text representation of the code's current value</summary>
		/// <param name="dxCode">The desired meta tag</param>
		/// <returns>The text representation of the current value</returns>
		private string GetValueString(CDxCode dxCode)
		{
			switch(dxCode.Type)
			{
				case TmaxCodeTypes.Memo:
				case TmaxCodeTypes.Text:
				case TmaxCodeTypes.PickList:
				
					return SQLEncode(dxCode.GetValue(false));
				
				case TmaxCodeTypes.Boolean:
				
					return BoolToSQL(dxCode.GetBool());
					
				default:
				
					return dxCode.GetValue(false);
			}
			
		}// private string GetValueString(CDxCode dxCode)
		
		#endregion Private Methods
	
		#region Properties
		
		/// <summary>The media record that owns this collection of tags</summary>
		public CDxMediaRecord Owner
		{
			get { return m_dxOwner; }
			set { m_dxOwner = value; }
		}
		
		#endregion Properties
		
	}//	public class CDxCodes : CDxMediaRecords
		
}// namespace FTI.Trialmax.Database
