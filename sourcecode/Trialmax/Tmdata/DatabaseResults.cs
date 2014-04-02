using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the results of an edit designation operation</summary>
	public class CTmaxDatabaseResults
	{
		#region Private Members
		
		/// <summary>Local member bound to Selection property</summary>
		private CTmaxItem m_tmaxSelection = new CTmaxItem();
		
		/// <summary>Local member bound to Edited property</summary>
		private CTmaxItem m_tmaxEdited = new CTmaxItem();
		
		/// <summary>Local member bound to Added property</summary>
		private CTmaxItems m_tmaxAdded = new CTmaxItems();
		
		/// <summary>Local member bound to Deleted property</summary>
		private CTmaxItems m_tmaxDeleted = new CTmaxItems();
		
		/// <summary>Local member bound to Updated property</summary>
		private CTmaxItems m_tmaxUpdated = new CTmaxItems();

		/// <summary>Local member bound to ObjectionsAdded property</summary>
		private CTmaxItems m_tmaxObjectionsAdded = new CTmaxItems();

		/// <summary>Local member bound to Deleted property</summary>
		private CTmaxItems m_tmaxObjectionsDeleted = new CTmaxItems();

		/// <summary>Local member bound to Updated property</summary>
		private CTmaxItems m_tmaxObjectionsUpdated = new CTmaxItems();

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxDatabaseResults()
		{
		}// public CTmaxDatabaseResults()
		
		/// <summary>This method is called to reset the class members</summary>
		/// <returns>true if successful</returns>
		public void Clear()
		{
			//	Flush all the collections
			if(m_tmaxAdded != null)
				m_tmaxAdded.Clear();
			if(m_tmaxDeleted != null)
				m_tmaxDeleted = null;
			if(m_tmaxUpdated != null)
				m_tmaxUpdated = null;
			if(m_tmaxObjectionsAdded != null)
				m_tmaxObjectionsAdded.Clear();
			if(m_tmaxObjectionsDeleted != null)
				m_tmaxObjectionsDeleted = null;
			if(m_tmaxObjectionsUpdated != null)
				m_tmaxObjectionsUpdated = null;
			
			//	Reset the bounded records
			if(m_tmaxSelection != null)
			{
				m_tmaxSelection.SetRecord(null);
				m_tmaxSelection.SubItems.Clear();
			}
			if(m_tmaxEdited != null)
			{
				m_tmaxEdited.SetRecord(null);
				m_tmaxEdited.SubItems.Clear();
			}
			
		}// public void Clear()
		
		/// <summary>Called when an objection has been added to the database</summary>
		/// <param name="tmaxObjection">The objection that has been added</param>
		public void OnAdded(CTmaxObjection tmaxObjection)
		{
			CTmaxItem tmaxDeposition = null;
			
			try
			{
				//	Get the parent deposition item
				if((tmaxDeposition = Find(m_tmaxObjectionsAdded, (CDxMediaRecord)(tmaxObjection.ICaseDeposition))) == null)
				{
					tmaxDeposition = new CTmaxItem((CDxMediaRecord)(tmaxObjection.ICaseDeposition));
					m_tmaxObjectionsAdded.Add(tmaxDeposition);
				}
				
				//	Add to the deposition's subitem collection
				tmaxDeposition.SubItems.Add(new CTmaxItem(tmaxObjection));
			}
			catch
			{
			}
			
		}// public void OnAdded(CTmaxObjection tmaxObjection)

		/// <summary>Called when an objection has been added to the database</summary>
		/// <param name="oxObjection">The objection record that has been added</param>
		public void OnAdded(COxObjection oxObjection)
		{
			if(oxObjection.TmaxObjection != null)
				OnAdded(oxObjection.TmaxObjection);
		}

		/// <summary>Called when an objection has been updated in the database</summary>
		/// <param name="tmaxObjection">The objection that has been updated</param>
		public void OnUpdated(CTmaxObjection tmaxObjection)
		{
			try
			{
				m_tmaxObjectionsUpdated.Add(new CTmaxItem(tmaxObjection));
			}
			catch
			{
			}

		}// public void OnUpdated(CTmaxObjection tmaxObjection)

		/// <summary>Called when an objection has been updated</summary>
		/// <param name="oxObjection">The objection record that has been updated</param>
		public void OnUpdated(COxObjection oxObjection)
		{
			if(oxObjection.TmaxObjection != null)
				OnUpdated(oxObjection.TmaxObjection);
		}

		/// <summary>Called when an objection has been deleted in the database</summary>
		/// <param name="tmaxObjection">The objection that has been deleted</param>
		public void OnDeleted(CTmaxObjection tmaxObjection)
		{
			try
			{
				m_tmaxObjectionsDeleted.Add(new CTmaxItem(tmaxObjection));
			}
			catch
			{
			}

		}// public void OnDeleted(CTmaxObjection tmaxObjection)

		/// <summary>Called when an objection has been deleted</summary>
		/// <param name="oxObjection">The objection record that has been deleted</param>
		public void OnDeleted(COxObjection oxObjection)
		{
			if(oxObjection.TmaxObjection != null)
				OnDeleted(oxObjection.TmaxObjection);
		}

		/// <summary>Called to search the specified collection for an item bound to the specified media record</summary>
		/// <param name="tmaxItems">The collection to be searched</param>
		/// <param name="dxRecord">The record to be located</param>
		/// <returns></returns>
		public CTmaxItem Find(CTmaxItems tmaxItems, CDxMediaRecord dxRecord)
		{
			if((tmaxItems != null) && (tmaxItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItems)
				{
					if(ReferenceEquals(O.GetMediaRecord() , dxRecord) == true)
						return O;
				}

			}// if((tmaxItems != null) && (tmaxItems.Count > 0))
			
			//	Must not be in the collection
			return null;

		}// public CTmaxItem Find(CTmaxItems tmaxItems, CDxMediaRecord dxRecord)

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Event item that identifies the scene that should be selected</summary>
		public CTmaxItem Selection
		{
			get { return m_tmaxSelection; }
		}
		
		/// <summary>Event item that identifies the designation that has been edited by the operation</summary>
		public CTmaxItem Edited
		{
			get { return m_tmaxEdited; }
		}
		
		/// <summary>Event items that identify scenes added to the script</summary>
		public CTmaxItems Added
		{
			get { return m_tmaxAdded; }
		}
		
		/// <summary>Event items that identify scenes that were deleted</summary>
		public CTmaxItems Deleted
		{
			get { return m_tmaxDeleted; }
		}
		
		/// <summary>Event items that identify scenes that were updated</summary>
		public CTmaxItems Updated
		{
			get { return m_tmaxUpdated; }
		}

		/// <summary>Event items that identify objections added to the database</summary>
		public CTmaxItems ObjectionsAdded
		{
			get { return m_tmaxObjectionsAdded; }
		}

		/// <summary>Event items that identify objections that were deleted</summary>
		public CTmaxItems ObjectionsDeleted
		{
			get { return m_tmaxObjectionsDeleted; }
		}

		/// <summary>Event items that identify objections that were updated</summary>
		public CTmaxItems ObjectionsUpdated
		{
			get { return m_tmaxObjectionsUpdated; }
		}

		#endregion Properties	
	
	}// class CTmaxDatabaseResults
	
}// namespace FTI.Trialmax.Database

