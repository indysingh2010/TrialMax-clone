using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CXmlLink : CXmlBase
	{
		#region Constants
		
		public const string XML_LINK_ELEMENT_NAME = "link";

		public const string XML_LINK_ATTRIBUTE_SOURCE_DB_ID = "srcDBID";
		public const string XML_LINK_ATTRIBUTE_SOURCE_MEDIA_ID = "srcMediaID";
		public const string XML_LINK_ATTRIBUTE_HIDE = "hide";
		public const string XML_LINK_ATTRIBUTE_START = "start";
		public const string XML_LINK_ATTRIBUTE_START_TUNED = "startTuned";
		public const string XML_LINK_ATTRIBUTE_PAGE = "page";
		public const string XML_LINK_ATTRIBUTE_LINE = "line";
		public const string XML_LINK_ATTRIBUTE_PL = "pl";
		public const string XML_LINK_ATTRIBUTE_SPLIT = "split";
		public const string XML_LINK_ATTRIBUTE_HIDE_TEXT = "hideText";
		public const string XML_LINK_ATTRIBUTE_HIDE_VIDEO = "hideVideo";
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>This member is bounded to the DatabaseId property</summary>
		protected string m_strDatabaseId = "";	
		
		/// <summary>This member is bounded to the SourceDbId property</summary>
		protected string m_strSourceDbId = "";		
		
		/// <summary>This member is bounded to the SourceMediaId property</summary>
		protected string m_strSourceMediaId = "";		
		
		/// <summary>This member is bounded to the PL property</summary>
		protected long m_lPL = 0;		
		
		/// <summary>This member is bounded to the Page property</summary>
		protected long m_lPage = 0;		
		
		/// <summary>This member is bounded to the Line property</summary>
		protected int m_iLine = 0;
		
		/// <summary>This member is bounded to the Start property</summary>
		protected double m_dStart = 0;
		
		/// <summary>This member is bounded to the StartTuned property</summary>
		protected bool m_bStartTuned = false;
		
		/// <summary>This member is bounded to the Hide property</summary>
		protected bool m_bHide = false;
		
		/// <summary>This member is bounded to the Split property</summary>
		protected bool m_bSplit = false;

		/// <summary>This member is bounded to the HideText property</summary>
		protected bool m_bHideText = false;

		/// <summary>This member is bounded to the HideVideo property</summary>
		protected bool m_bHideVideo = false;

		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlLink() : base()
		{
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source link to be copied</param>
		public CXmlLink(CXmlLink xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
			
			if(xmlSource != null)
				Copy(xmlSource);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				if(Start == ((CXmlLink)xmlBase).Start)
					return 0;
				else if(Start > ((CXmlLink)xmlBase).Start)
					return 1;
				else
					return -1;
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		public void Copy(CXmlLink xmlSource)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
			
			DatabaseId = xmlSource.DatabaseId;
			Hide = xmlSource.Hide;
			Line = xmlSource.Line;
			Page = xmlSource.Page;
			PL = xmlSource.PL;
			SourceDbId = xmlSource.SourceDbId;
			SourceMediaId = xmlSource.SourceMediaId;
			Split = xmlSource.Split;
			Start = xmlSource.Start;
			StartTuned = xmlSource.StartTuned;
			HideText = xmlSource.HideText;
			HideVideo = xmlSource.HideVideo;
			
		}// public void Copy(CXmlLink xmlSource)
		
		/// <summary>Called to set the composite page/line value assigned to the link</summary>
		/// <param name="lPL">The new value</param>
		public void SetPL(long lPL)
		{
			if(lPL > 0)
			{
				m_lPL = lPL;
				m_lPage = CTmaxToolbox.PLToPage(lPL);
				m_iLine = CTmaxToolbox.PLToLine(lPL);
			}
			else
			{
				m_lPL = 0;
				m_lPage = 0;
				m_iLine = 0;
			}

		}// public void SetPL(long lPL)

		/// <summary>Called to get the composite page/line value assigned to the link</summary>
		/// <returns>The composite PL value</returns>
		public long GetPL()
		{
			if(m_lPL <= 0)
				m_lPL = CTmaxToolbox.GetPL(this.Page, this.Line);
				
			return m_lPL;

		}// public long GetPL()

		/// <summary>This method will set the link properties using the specified node</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlNode xmlNode)
		{
			XPathNavigator xpNavigator = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
					return SetProperties(xpNavigator);
				else
					return false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML link properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the link properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_SOURCE_DB_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSourceDbId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_SOURCE_MEDIA_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSourceMediaId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_START,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStart = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_START_TUNED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bStartTuned = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_PAGE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lPage = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_LINE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_iLine = System.Convert.ToInt32(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_HIDE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bHide = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_SPLIT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bSplit = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_HIDE_TEXT, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bHideText = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_LINK_ATTRIBUTE_HIDE_VIDEO, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bHideVideo = XmlToBool(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML link properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_LINK_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_SOURCE_DB_ID, SourceDbId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_SOURCE_MEDIA_ID, SourceMediaId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_START, DoubleToXml(Start)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_START_TUNED, BoolToXml(StartTuned)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_PL, PL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_PAGE, Page) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_LINE, Line) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_HIDE, BoolToXml(Hide)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_SPLIT, BoolToXml(Split)) == false)
						break;

					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_HIDE_TEXT, BoolToXml(HideText)) == false)
						break;

					if(AddAttribute(xmlElement, XML_LINK_ATTRIBUTE_HIDE_VIDEO, BoolToXml(HideVideo)) == false)
						break;

					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		override public string ToString()
		{
			if(PL < 0)
			{
				return (CTmaxToolbox.SecondsToString(Start) + " - " + SourceMediaId);
			}
			else
			{
				return (CTmaxToolbox.PLToString(PL) + " - " + SourceMediaId);
			}
			
		}// virtual public string GetDisplayString()
		
		/// <summary>Called to determine if the specifed position matches the start position of this link</summary>
		/// <param name="dPosition">The desired position</param>
		/// <param name="dTolerance">The tolerance on either side of the start position</param>
		/// <returns>0 if within bounds, negative if before, positive if after</returns>
		public double CheckPosition(double dPosition, double dTolerance)
		{
			//	Is the specified position within the bounds of this element?
			if((dPosition >= (m_dStart - dTolerance)) && (dPosition <= (m_dStart + dTolerance)))
			{
				return 0;	//	Within the bounds of this element
			}
			else
			{
				return (dPosition - m_dStart);
			}
			
		}
		
		/// <summary>Called to determine if the specifed position is within this element's start/stop boundries</summary>
		/// <param name="dPosition">The desired position</param>
		/// <returns>0 if within bounds, negative if before, positive if after</returns>
		public double CheckPosition(double dPosition)
		{
			return CheckPosition(dPosition, 0.03333);
		}
		
		/// <summary>Called to determine if the link's position falls within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <param name="dTolerance">The tolerance on either side of the stated positions</param>
		/// <returns>true if within range</returns>
		public bool CheckRange(double dStart, double dStop, double dTolerance)
		{
			//	Is the specified position within the bounds of this element?
			if((m_dStart >= (dStart - dTolerance)) && (m_dStart <= (dStop + dTolerance)))
			{
				return true;	//	Within the stated range
			}
			else
			{
				return false;
			}
			
		}// public bool CheckRange(double dStart, double dStop, double dTolerance)
		
		/// <summary>Called to determine if the link's position falls within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <returns>true if within range</returns>
		public bool CheckRange(double dStart, double dStop)
		{
			return CheckRange(dStart, dStop, 0.0);
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Unique ID assigned by the database</summary>
		/// <remarks>This is not a persistant attribute. It's used by the Manager's tune pane</remarks>
		public string DatabaseId
		{
			get{ return m_strDatabaseId; }
			set{ m_strDatabaseId = value; }
		}
		
		/// <summary>PST Identifier of source record in the database</summary>
		public string SourceDbId
		{
			get{ return m_strSourceDbId; }
			set{ m_strSourceDbId = value; }
		}
		
		/// <summary>Media Identifier of source record in the database</summary>
		public string SourceMediaId
		{
			get{ return m_strSourceMediaId; }
			set{ m_strSourceMediaId = value; }
		}
		
		/// <summary>Page/Line of the link</summary>
		public long PL
		{
			get{ return GetPL(); }
			set{ SetPL(value); }
		}
		
		/// <summary>Page where the link occurs</summary>
		public long Page
		{
			get{ return m_lPage; }
			set{ m_lPage = value; }
		}
		
		/// <summary>Line where the link occurs</summary>
		public int Line
		{
			get{ return m_iLine; }
			set{ m_iLine = value; }
		}
		
		/// <summary>Time into playback when link occurs</summary>
		public double Start
		{
			get{ return m_dStart; }
			set{ m_dStart = value; }
		}
		
		/// <summary>Start position has been tuned?</summary>
		public bool StartTuned
		{
			get{ return m_bStartTuned; }
			set{ m_bStartTuned = value; }
		}
		
		/// <summary>Hide the current link?</summary>
		public bool Hide
		{
			get{ return m_bHide; }
			set{ m_bHide = value; }
		}
		
		/// <summary>Split Screen ?</summary>
		public bool Split
		{
			get{ return m_bSplit; }
			set{ m_bSplit = value; }
		}

		/// <summary>Hide the scrolling text?</summary>
		public bool HideText
		{
			get { return m_bHideText; }
			set { m_bHideText = value; }
		}

		/// <summary>Hide the video playback?</summary>
		public bool HideVideo
		{
			get { return m_bHideVideo; }
			set { m_bHideVideo = value; }
		}

		#endregion Properties
		
	}// public class CXmlLink

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlLink objects
	/// </summary>
	public class CXmlLinks : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlLinks() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="xmlLink">CXmlLink object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlLink Add(CXmlLink xmlLink)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlLink as object);

				return xmlLink;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlLink xmlLink)

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="xmlLink">The object to be removed</param>
		public void Remove(CXmlLink xmlLink)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlLink as object);
			}
			catch
			{
			}
		}

		/// <summary>Called to locate the object with the specified database id</summary>
		/// <returns>The object with the specified id</returns>
		public CXmlLink Find(string strDatabaseId)
		{
			// Search for the object with the same name
			foreach(CXmlLink O in this)
			{
				if(String.Compare(O.DatabaseId, strDatabaseId, true) == 0)
				{
					return O;
				}
			}
			return null;

		}//	Find(string strSourceId)

		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlLink">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlLink xmlLink)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlLink as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlLink this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlLink);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlLink value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>Called to determine if all links fall within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <param name="dTolerance">The tolerance on either side of the stated positions</param>
		/// <param name="xmlOutOfRange">Collection to store those links that are out of range</param>
		/// <returns>true if all within range</returns>
		public bool CheckRange(double dStart, double dStop, double dTolerance, CXmlLinks xmlOutOfRange)
		{
			bool bInRange = true;
			
			// Search for each link that is outside of the range
			foreach(CXmlLink O in this)
			{
				if(O.CheckRange(dStart, dStop, dTolerance) == false)
				{
					bInRange = false;
					
					//	Stop here if we not interested in all occurances
					if(xmlOutOfRange != null)
						xmlOutOfRange.Add(O);
					else
						break;
				
				}// if(O.CheckRange(dStart, dStop, dTolerance) == false)
				
			}// foreach(CXmlLink O in this)
			
			return bInRange;

		}//	public bool CheckRange(double dStart, double dStop, double dTolerance, CXmlLinks xmlOutOfRange)

		/// <summary>Called to determine if all links fall within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <param name="dTolerance">The tolerance on either side of the stated positions</param>
		/// <returns>true if all within range</returns>
		public bool CheckRange(double dStart, double dStop, double dTolerance)
		{
			return CheckRange(dStart, dStop, dTolerance, null);
		}

		/// <summary>Called to determine if all links fall within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <param name="xmlOutOfRange">Collection to store those links that are out of range</param>
		/// <returns>true if all within range</returns>
		public bool CheckRange(double dStart, double dStop, CXmlLinks xmlOutOfRange)
		{
			return CheckRange(dStart, dStop, 0.0, xmlOutOfRange);
		}

		/// <summary>Called to determine if all links fall within the specified range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <returns>true if all within range</returns>
		public bool CheckRange(double dStart, double dStop)
		{
			return CheckRange(dStart, dStop, 0.0, null);
		}

		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="dTolerance">Tolerance applied to either side of the specified position</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(int iStart, double dPosition, double dTolerance, bool bExact)
		{
			int		iPrevious = iStart;
			int		i;
			double	dOffset = 0;
			double	dPrevious = 0;
			
			try
			{
				//	Make sure we have a non-empty collection
				if((m_aList == null) || (m_aList.Count == 0)) return -1;
				
				//	Set the start index
				if((iStart < 0) || (iStart >= m_aList.Count))
					iStart = 0;
					
				//	Is the start element at the desired position?
				if((dOffset = this[iStart].CheckPosition(dPosition, dTolerance)) == 0)
					return iStart;

				dPrevious = dOffset;
				iPrevious = iStart;
					
				//	Do we need to iterate forward?
				if(dOffset > 0)
				{
					for(i = iStart + 1; i < Count; i++)
					{
						if((dOffset = this[i].CheckPosition(dPosition, dTolerance)) == 0)
							return i;

						//	Have we gone to far?
						if(dOffset < 0)
							break;

						dPrevious = dOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				}
				else 
				{
					//	Go backwards in search of the closest element
					for(i = iStart - 1; i >= 0; i--)
					{
						if((dOffset = this[i].CheckPosition(dPosition, dTolerance)) == 0)
							return i;

						//	Have we gone to far?
						if(dOffset > 0)
							break;

						dPrevious = dOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				
				}
				
				//	Is the caller looking for an exact match?
				if(bExact == true)
					return -1; // Not found
					
				//	Did we break out of the loop without finding a match?
				if(dPrevious == dOffset)
				{
					Debug.Assert(iPrevious >= 0);
					Debug.Assert(iPrevious < Count);
					return iPrevious;
				}
				else
				{
					//	Which is closer
					if(System.Math.Abs(dPrevious) <= System.Math.Abs(dOffset))
					{
						Debug.Assert(iPrevious >= 0);
						Debug.Assert(iPrevious < Count);
						return iPrevious;
					}
					else
					{
						Debug.Assert(i >= 0);
						Debug.Assert(i < Count);
						return i;
					}
				
				}
				
			}
			catch
			{
				Debug.Assert(false);
				return -1;
			}
			
		}// public int Locate(int iStart, double dPosition, double dTolerance, bool bExact)

		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="dTolerance">Tolerance applied to either side of the specified position</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(int iStart, double dPosition, double dTolerance)
		{
			return Locate(iStart, dPosition, dTolerance, true);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(int iStart, double dPosition, bool bExact)
		{
			return Locate(iStart, dPosition, 0.03333, bExact);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(int iStart, double dPosition)
		{
			return Locate(iStart, dPosition, 0.03333, true);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="dTolerance">Tolerance applied to either side of the specified position</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(double dPosition, double dTolerance, bool bExact)
		{
			return Locate(0, dPosition, dTolerance, bExact);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="dTolerance">Tolerance applied to either side of the specified position</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(double dPosition, double dTolerance)
		{
			return Locate(0, dPosition, dTolerance, true);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(double dPosition, bool bExact)
		{
			return Locate(0, dPosition, 0.03333, bExact);
		}
		
		/// <summary>Locates the element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(double dPosition)
		{
			return Locate(0, dPosition, 0.03333, true);
		}
		
		/// <summary>Locates the element corresponding to the desired PL start position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="PL">The PL value to locate</param>
		/// <param name="lTolerance">Tolerance applied to either side of the specified position</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at the specified page/line</returns>
		public int Locate(int iStart, long lPL, long lTolerance, bool bExact)
		{
			int		iPrevious = iStart;
			int		i;
			long	lOffset = 0;
			long	lPrevious = 0;
			
			try
			{
				//	Make sure we have a non-empty collection
				if((m_aList == null) || (m_aList.Count == 0)) return -1;
				
				//	Set the start index
				if((iStart < 0) || (iStart >= m_aList.Count))
					iStart = 0;
					
				//	Is the start element at the desired position?
				if((lOffset = (lPL - this[iStart].PL)) == 0)
					return iStart;

				lPrevious = lOffset;
				iPrevious = iStart;
						
				//	Do we need to iterate forward?
				if(lOffset > 0)
				{
					for(i = iStart + 1; i < Count; i++)
					{
						if((lOffset = (lPL - this[i].PL)) == 0)
							return i;

						//	Have we gone to far?
						if(lOffset < 0)
							break;

						lPrevious = lOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				}
				else 
				{
					//	Go backwards in search of the closest element
					for(i = iStart - 1; i >= 0; i--)
					{
						if((lOffset = (lPL - this[i].PL)) == 0)
							return i;

						//	Have we gone to far?
						if(lOffset > 0)
							break;

						lPrevious = lOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				
				}
				
				//	Is the caller looking for an exact match?
				if(bExact == true)
					return -1; // Not found
					
				//	Did we break out of the loop without finding a match?
				if(lPrevious == lOffset)
				{
					Debug.Assert(iPrevious >= 0);
					Debug.Assert(iPrevious < Count);
					return iPrevious;
				}
				else
				{
					//	Which is closer
					if(System.Math.Abs(lPrevious) <= System.Math.Abs(lOffset))
					{
						Debug.Assert(iPrevious >= 0);
						Debug.Assert(iPrevious < Count);
						return iPrevious;
					}
					else
					{
						Debug.Assert(i >= 0);
						Debug.Assert(i < Count);
						return i;
					}
				
				}
				
			}
			catch
			{
				Debug.Assert(false);
				return -1;
			}
			
		}// public int Locate(int iStart, long lPL, long lTolerance, bool bExact)

		/// <summary>Locates the element corresponding to the desired PL start position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="PL">The PL value to locate</param>
		/// <returns>The index of the object at the specified page/line</returns>
		public int Locate(int iStart, long lPL)
		{
			return Locate(iStart, lPL, 0, true);
		}
			
		/// <summary>Locates the element corresponding to the desired PL start position</summary>
		/// <param name="PL">The PL value to locate</param>
		/// <returns>The index of the object at the specified page/line</returns>
		public int Locate(long lPL)
		{
			return Locate(0, lPL, 0, true);
		}
			
		#endregion Public Methods
		
	}//	public class CXmlLinks
		
}// namespace FTI.Shared.Xml
