using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class implements a sorter interface that can be used to maintain sorted links</summary>
	public class CXmlBaseSorter : IComparer
	{
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to Mode property</summary>
		private int m_iMode = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlBaseSorter()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "XML Sorter";
		}
		
		/// <summary>Constructor</summary>
		public CXmlBaseSorter(int iMode)
		{
			m_iMode = iMode;
			
			//	Set the default event source name
			m_tmaxEventSource.Name = "XML Sorter";
		}
		
		/// <summary>This method is called to compare two links</summary>
		/// <param name="x">First object to be compared</param>
		/// <param name="y">Second object to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		int IComparer.Compare(object x, object y) 
		{
			return Compare((CXmlBase)x, (CXmlBase)y);
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to compare two links</summary>
		/// <param name="xmlBase1">First object to be compared</param>
		/// <param name="xmlBase2">Second object to be compared</param>
		/// <returns>-1 if xmlBase1 less than xmlBase2, 0 if equal, 1 if xmlBase1 greater than xmlBase2</returns>
		protected int Compare(CXmlBase xmlBase1, CXmlBase xmlBase2)
		{
			int	iReturn = -1;
			
			try
			{
				//	Check for equality first
				//
				//	NOTE:	.NET raises and exception if we don't return 0 for
				//			equal objects
				if(ReferenceEquals(xmlBase1, xmlBase2) == true)
				{
					iReturn = 0;
				}
				else
				{
					return xmlBase1.Compare(xmlBase2, m_iMode);
					
				}// if(ReferenceEquals(xmlBase1, xmlBase2) == true)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Compare", "Exception:", Ex);
				iReturn = -1;
			}
			
			return iReturn;
		
		}// protected virtual int Compare(CXmlBase xmlBase1, CXmlBase xmlBase2)
		
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
			
		}// EventSource property

		/// <summary>Sort mode defined by the derived class</summary>
		public int Mode
		{
			get { return m_iMode; }
			set { m_iMode = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlBaseSorter : IComparer

}// namespace FTI.Trialmax.Database
