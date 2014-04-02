using System;
using System.Collections;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class implements a sorter interface that can be used to maintain sorted records</summary>
	public class CTmaxRecordSorter : IComparer
	{
		#region Constants
		
		public const int PRIMARY_SORT_ORDER_AUTO_ID = 0;
		public const int PRIMARY_SORT_ORDER_MEDIA_ID = 1;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to UseSource property</summary>
		private bool m_bUseSource = false;

		/// <summary>Local member bound to PrimarySortOrder property</summary>
		private int m_iPrimarySortOrder = PRIMARY_SORT_ORDER_AUTO_ID;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxRecordSorter()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Record Sorter";
		}
		
		/// <summary>This method is called to compare two records</summary>
		/// <param name="x">First record to be compared</param>
		/// <param name="y">Second record to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		int IComparer.Compare(object x, object y) 
		{
			return Compare((CDxMediaRecord)x, (CDxMediaRecord)y);
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to compare two records</summary>
		/// <param name="dxRecord1">First record to be compared</param>
		/// <param name="dxRecord2">Second record to be compared</param>
		/// <returns>-1 if dxRecord1 less than dxRecord2, 0 if equal, 1 if dxRecord1 greater than dxRecord2</returns>
		protected int Compare(CDxMediaRecord dxRecord1, CDxMediaRecord dxRecord2)
		{
			int	iReturn = -1;
			int	iLevel1 = 0;
			int iLevel2 = 0;
			
			try
			{
				//	Check for equality first
				//
				//	NOTE:	.NET raises and exception if we don't return 0 for
				//			equal objects
				if(ReferenceEquals(dxRecord1, dxRecord2) == true)
				{
					iReturn = 0;
				}
				
				//	Are these media records?
				else if((dxRecord1.GetDataType() == TmaxDataTypes.Media) && 
				        (dxRecord2.GetDataType() == TmaxDataTypes.Media))
				{
					//	At what media level is each record?
					iLevel1 = (int)dxRecord1.GetMediaLevel();
					iLevel2 = (int)dxRecord2.GetMediaLevel();
					
					//	Are the records at the same level?
					if(iLevel1 == iLevel2)
					{
						switch(dxRecord1.GetMediaLevel())
						{
							case TmaxMediaLevels.Primary:
								
								iReturn = Compare((CDxPrimary)dxRecord1, (CDxPrimary)dxRecord2);
								break;
								
							case TmaxMediaLevels.Secondary:
								
								iReturn = Compare((CDxSecondary)dxRecord1, (CDxSecondary)dxRecord2);
								break;
								
							case TmaxMediaLevels.Tertiary:
								
								iReturn = Compare((CDxTertiary)dxRecord1, (CDxTertiary)dxRecord2);
								break;
								
							case TmaxMediaLevels.Quaternary:
								
								iReturn = Compare((CDxQuaternary)dxRecord1, (CDxQuaternary)dxRecord2);
								break;
								
							default:
								
								if(dxRecord1.AutoId == dxRecord2.AutoId)
									iReturn = 0;
								else if(dxRecord1.AutoId > dxRecord2.AutoId)
									iReturn = 1;
								else
									iReturn = -1;
								
								break;
						}
					
					}
					else
					{
						iReturn = (iLevel1 > iLevel2) ? 1 : -1;
					}
					
				}// // else if((dxRecord1.GetDataType() == TmaxDataTypes.Media)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Compare", "Exception:", Ex);
				iReturn = -1;
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CDxMediaRecord dxRecord1, CDxMediaRecord dxRecord2)
		
		/// <summary>This function is called to compare two primary records</summary>
		/// <param name="dxPrimary1">First primary record to be compared</param>
		/// <param name="dxPrimary2">Second primary record to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		protected int Compare(CDxPrimary dxPrimary1, CDxPrimary dxPrimary2)
		{
			int	iReturn = -1;
			
			try
			{
				//	NOTE: Check for equality has already been performed

				//	Are we sorting on Media ID?
				if(this.PrimarySortOrder == CTmaxRecordSorter.PRIMARY_SORT_ORDER_MEDIA_ID)
				{
					return CTmaxToolbox.Compare(dxPrimary1.MediaId, dxPrimary2.MediaId, true);	
				}
				else
				{
					//	Primary records are sorted based on the order in which they
					//	appear in the database
					if(dxPrimary1.AutoId == dxPrimary2.AutoId)
						iReturn = 0;
					else if(dxPrimary1.AutoId > dxPrimary2.AutoId)
						iReturn = 1;
					else
						iReturn = -1;

				}// if(this.PrimarySortOrder == CTmaxRecordSorter.PRIMARY_SORT_ORDER_MEDIA_ID)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ComparePrimaries", "Exception:", Ex);
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CDxPrimary dxPrimary1, CDxPrimary dxPrimary2)
		
		/// <summary>This function is called to compare two secondary records</summary>
		/// <param name="dxSecondary1">First secondary record to be compared</param>
		/// <param name="dxSecondary2">Second secondary record to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		protected int Compare(CDxSecondary dxSecondary1, CDxSecondary dxSecondary2)
		{
			int			iReturn = -1;
			CDxMediaRecord	dxSource1 = null;
			CDxMediaRecord	dxSource2 = null;
			try
			{
				//	NOTE: Check for equality has already been performed

				//	Are we supposed to be using the source records?
				if(m_bUseSource == true)
				{
					dxSource1 = dxSecondary1.GetSource();
					dxSource2 = dxSecondary2.GetSource();
					
					if((dxSource1 != null) && (dxSource2 != null))
						return Compare(dxSource1, dxSource2);
				}
				
				//	Secondary records are sorted based on their display order
				if(dxSecondary1.DisplayOrder == dxSecondary2.DisplayOrder)
					iReturn = 0;
				else if(dxSecondary1.DisplayOrder > dxSecondary2.DisplayOrder)
					iReturn = 1;
				else
					iReturn = -1;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CompareSecondaries", "Exception:", Ex);
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CDxSecondary dxSecondary1, CDxSecondary dxSecondary2)
		
		/// <summary>This function is called to compare two tertiary records</summary>
		/// <param name="dxTertiary1">First tertiary record to be compared</param>
		/// <param name="dxTertiary2">Second tertiary record to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		protected int Compare(CDxTertiary dxTertiary1, CDxTertiary dxTertiary2)
		{
			int	iReturn = -1;
			
			try
			{
				//	NOTE: Check for equality has already been performed

				//	Are both of these records designations?
				if((dxTertiary1.MediaType == TmaxMediaTypes.Designation) &&
				   (dxTertiary2.MediaType == TmaxMediaTypes.Designation))
				{
					//	Sort records based on their start PL values first
					if(dxTertiary1.StartPL == dxTertiary2.StartPL)
					{
						//	Sort records based on their start times if PL is equal
						if(dxTertiary1.Start == dxTertiary2.Start)
							iReturn = 0;
						else if(dxTertiary1.Start > dxTertiary2.Start)
							iReturn = 1;
						else
							iReturn = -1;
					}
					else if(dxTertiary1.StartPL > dxTertiary2.StartPL)
						iReturn = 1;
					else
						iReturn = -1;
				}
				
				//	Are they both clips?
				else if((dxTertiary1.MediaType == TmaxMediaTypes.Clip) &&
						(dxTertiary2.MediaType == TmaxMediaTypes.Clip))
				{
					//	Sort records based on their start times
					if(dxTertiary1.Start == dxTertiary2.Start)
						iReturn = 0;
					else if(dxTertiary1.Start > dxTertiary2.Start)
						iReturn = 1;
					else
						iReturn = -1;
				}
				
				else
				{
					//	Sort records based on their display order
					if(dxTertiary1.DisplayOrder == dxTertiary2.DisplayOrder)
						iReturn = 0;
					else if(dxTertiary1.DisplayOrder > dxTertiary2.DisplayOrder)
						iReturn = 1;
					else
						iReturn = -1;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CompareTertiaries", "Exception:", Ex);
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CDxTertiary dxTertiary1, CDxTertiary dxTertiary2)
		
		/// <summary>This function is called to compare two quaternary records</summary>
		/// <param name="dxQuaternary1">First quaternary record to be compared</param>
		/// <param name="dxQuaternary2">Second quaternary record to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		protected int Compare(CDxQuaternary dxQuaternary1, CDxQuaternary dxQuaternary2)
		{
			int	iReturn = -1;
			
			try
			{
				//	Are we sorting two links?
				if((dxQuaternary1.MediaType == TmaxMediaTypes.Link) &&
				   (dxQuaternary2.MediaType == TmaxMediaTypes.Link))
				{
					//	Use the start times
					if(dxQuaternary1.Start == dxQuaternary2.Start)
						iReturn = 0;
					else if(dxQuaternary1.Start > dxQuaternary2.Start)
						iReturn = 1;
					else
						iReturn = -1;
				}
				else
				{
					//	Sort records based on their display order
					if(dxQuaternary1.DisplayOrder == dxQuaternary2.DisplayOrder)
						iReturn = 0;
					else if(dxQuaternary1.DisplayOrder > dxQuaternary2.DisplayOrder)
						iReturn = 1;
					else
						iReturn = -1;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CompareQuaternaries", "Exception:", Ex);
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CDxQuaternary dxQuaternary1, CDxQuaternary dxQuaternary2)
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
			
		}// EventSource property

		/// <summary>True to use the source record instead of actual record</summary>
		public bool UseSource
		{
			get { return m_bUseSource; }
			set { m_bUseSource = value; }
		}

		/// <summary>Sort order applied when comparing primary media records</summary>
		public int PrimarySortOrder
		{
			get { return m_iPrimarySortOrder; }
			set { m_iPrimarySortOrder = value; }
		}

		#endregion Properties
		
	}// public class CTmaxRecordSorter : IComparer

}// namespace FTI.Trialmax.Database
