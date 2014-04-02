using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class contains information associated with a record in the Media table of the Exhibits report source</summary>
	public class CRSExhibitsMedia : ITmaxSortable
	{
		#region Constants
		
		public const int SORT_BY_BARCODE			= 0;
		public const int SORT_BY_NAME				= 1;
		public const int SORT_BY_FOREIGN_BARCODE	= 2;
		public const int SORT_BY_MEDIA_TYPE			= 3;
		public const int SORT_BY_LAST_MODIFIED		= 4;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Collection of child media</summary>
		private CRSExhibitsMedias m_aChildren = null;
		
		/// <summary>Local member bound to Source property</summary>
		private CDxMediaRecord m_dxSource = null;
		
		/// <summary>Local member bound to Barcode property</summary>
		private string m_strBarcode = "";
		
		/// <summary>Local member bound to MediaType property</summary>
		private short m_sMediaType = 0;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Path property</summary>
		private string m_strPath = "";
		
		/// <summary>Local member bound to Description property</summary>
		private string m_strDescription = "";
		
		/// <summary>Local member bound to ForeignBarcode property</summary>
		private string m_strForeignBarcode = "";
		
		/// <summary>Local member bound to LastModified property</summary>
		private System.DateTime m_dtLastModified = System.DateTime.Now;
		
		/// <summary>Local member bound to Admitted property</summary>
		private bool m_bAdmitted = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CRSExhibitsMedia()
		{
		}
	
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="rsMedia">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than rsMedia, 0 if equal, 1 if greater than</returns>
		public int Compare(CRSExhibitsMedia rsMedia, long lMode)
		{
			//	What sort mode is being used?
			switch(lMode)
			{
				case SORT_BY_FOREIGN_BARCODE:
				
					return CTmaxToolbox.Compare(ForeignBarcode, rsMedia.ForeignBarcode, true);
					
				case SORT_BY_NAME:
				
					return CTmaxToolbox.Compare(Name, rsMedia.Name, true);
					
				case SORT_BY_MEDIA_TYPE:
				
					if(MediaType == rsMedia.MediaType)
						return CTmaxToolbox.Compare(Barcode, rsMedia.Barcode, true);
					else
						return (MediaType < rsMedia.MediaType) ? -1 : 1;
					
				case SORT_BY_LAST_MODIFIED:
				
					if(LastModified == rsMedia.LastModified)
						return CTmaxToolbox.Compare(Barcode, rsMedia.Barcode, true);
					else
						return (LastModified < rsMedia.LastModified) ? 1 : -1;
					
				default:
				
					return CTmaxToolbox.Compare(Barcode, rsMedia.Barcode, true);
			
			}
					
		}// public int Compare(CRSExhibitsMedia rsMedia, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than rsMedia, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CRSExhibitsMedia)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Collection of child media objects</summary>
		public CRSExhibitsMedias Children
		{
			get
			{
				if(m_aChildren == null)
					m_aChildren = new CRSExhibitsMedias();
				
				return m_aChildren;
			}
			
		}
		
		/// <summary>The source record</summary>
		public CDxMediaRecord Source
		{
			get { return m_dxSource; }
			set { m_dxSource = value; }
		}
		
		/// <summary>The TrialMax media type</summary>
		public short MediaType
		{
			get { return m_sMediaType; }
			set { m_sMediaType = value; }
		}
		
		/// <summary>The barcode of the media object</summary>
		public string Barcode
		{
			get { return m_strBarcode; }
			set { m_strBarcode = value; }
		}
		
		/// <summary>The foreign (mapped) barcode</summary>
		public string ForeignBarcode
		{
			get { return m_strForeignBarcode; }
			set { m_strForeignBarcode = value; }
		}
		
		/// <summary>The record's description</summary>
		public string Description
		{
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}
		
		/// <summary>The record's name</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The folder containing the record's media files</summary>
		public string Path
		{
			get { return m_strPath; }
			set { m_strPath = value; }
		}
		
		/// <summary>The last modified date</summary>
		public System.DateTime LastModified
		{
			get { return m_dtLastModified; }
			set { m_dtLastModified = value; }
		}
		
		/// <summary>True if admitted</summary>
		public bool Admitted
		{
			get { return m_bAdmitted; }
			set { m_bAdmitted = value; }
		}
		
		#endregion Properties
		
	}// public class CRSExhibitsMedia

	/// <summary>This class manages a list of search results</summary>
	public class CRSExhibitsMedias : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CRSExhibitsMedias() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			SetSortOrder(CRSExhibitsMedia.SORT_BY_BARCODE);
			
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="rsMedia">CRSExhibitsMedia object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CRSExhibitsMedia Add(CRSExhibitsMedia rsMedia)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(rsMedia as object);

				return rsMedia;
			}
			catch
			{
				return null;
			}
			
		}// public CRSExhibitsMedia Add(CRSExhibitsMedia rsMedia)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="rsMedia">The filter object to be removed</param>
		public void Remove(CRSExhibitsMedia rsMedia)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(rsMedia as object);
			}
			catch
			{
			}
		
		}// public void Remove(CRSExhibitsMedia rsMedia)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="rsMedia">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CRSExhibitsMedia rsMedia)
		{
			// Use base class to process actual collection operation
			return base.Contains(rsMedia as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CRSExhibitsMedia this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CRSExhibitsMedia);
			}
		}

		/// <summary>This method is called to set the collection sort order</summary>
		/// <param name="iSortOrder">The sort order identifier</param>
		public void SetSortOrder(int iSortOrder)
		{
			if(Comparer != null)
			{
				((CTmaxSorter)Comparer).Mode = iSortOrder;
			}
			
		}
		
		#endregion Public Methods
		
	}//	public class CRSExhibitsMedias
		
}// namespace FTI.Trialmax.Reports
