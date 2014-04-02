using System;
using System.Collections;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>
	/// This class contains methods that assist in the construction
	///	of Trialmax error argument objects
	/// </summary>
	public class CTmaxErrorBuilder
	{
		protected ArrayList m_aFormatStrings;
		
		/// <summary>Default constructor</summary>
		public CTmaxErrorBuilder()
		{
			m_aFormatStrings = new ArrayList();
		}
		
		/// <summary>This method will format the error message using the supplied parameters</summary>
		/// <param name="iError">Error index</param>
		/// <param name="objParam1">Object used to replace occurances of %1 in the format string</param>
		/// <param name="objParam2">Object used to replace occurances of %2 in the format string</param>
		/// <param name="objParam3">Object used to replace occurances of %3 in the format string</param>
		public virtual string Message(int iError, object objParam1, object objParam2, object objParam3)
		{
			string strMessage = GetFormatString(iError);

			//	Insert the parameters
			if(objParam1 != null)
				strMessage = strMessage.Replace("%1", objParam1.ToString());
				
			if(objParam2 != null)
				strMessage = strMessage.Replace("%2", objParam2.ToString());
			
			if(objParam3 != null)
				strMessage = strMessage.Replace("%3", objParam3.ToString());
				
			return strMessage;
		
		}// Message

		/// <summary>This method will format the error message using the supplied parameters</summary>
		/// <param name="iError">Error index</param>
		/// <param name="objParam1">Object used to replace occurances of %1 in the format string</param>
		/// <param name="objParam2">Object used to replace occurances of %2 in the format string</param>
		public virtual string Message(int iError, object objParam1, object objParam2)
		{
			return Message(iError, objParam1, objParam2, null);
		}

		/// <summary>This method will format the error message using the supplied parameters</summary>
		/// <param name="iError">Error index</param>
		/// <param name="objParam1">Object used to replace occurances of %1 in the format string</param>
		public virtual string Message(int iError, object objParam1)
		{
			return Message(iError, objParam1, null, null);
		}

		/// <summary>This method will format the error message using the supplied parameters</summary>
		/// <param name="iError">Error index</param>
		public virtual string Message(int iError)
		{
			return GetFormatString(iError);
		}

		/// <summary>
		/// This method will locate the appropriate error message format string
		/// </summary>
		/// <param name="iError">Error index</param>
		/// <returns>The format string associated with the identifier</returns>
		/// <remarks>Derived classes should override this method to perform class specific translations</remarks>
		public virtual string GetFormatString(int iError)
		{
			string strFormat;
			
			if((m_aFormatStrings != null) && (iError >= 0) && (iError < m_aFormatStrings.Count))
			{
				strFormat = (string)m_aFormatStrings[iError];
			}
			else
			{
				strFormat = "Error #" + iError.ToString();
			}
			
			return strFormat;
			
		}//GetFormatString(int iError)
	
		/// <summary>This method will allocate and populate an error items collection using the specified values</summary>
		/// <param name="strName1">Name assigned to item 1</param>
		/// <param name="objValue1">Object used to set Value property of item 1</param>
		/// <param name="strName2">Name assigned to item 2</param>
		/// <param name="objValue2">Object used to set Value property of item 2</param>
		/// <param name="strName3">Name assigned to item 3</param>
		/// <param name="objValue3">Object used to set Value property of item 3</param>
		public static CTmaxErrorArgs.CErrorItems Items(string strName1, object objValue1, string strName2, object objValue2, string strName3, object objValue3)
		{
			CTmaxErrorArgs.CErrorItems aItems = new CTmaxErrorArgs.CErrorItems();
			
			if(aItems != null)
			{
				if((strName1 != null) && (objValue1 != null))
					aItems.Add(new CTmaxErrorArgs.CErrorItem(strName1, objValue1.ToString()));
				
				if((strName2 != null) && (objValue2 != null))
					aItems.Add(new CTmaxErrorArgs.CErrorItem(strName2, objValue2.ToString()));
				
				if((strName3 != null) && (objValue3 != null))
					aItems.Add(new CTmaxErrorArgs.CErrorItem(strName3, objValue3.ToString()));
			}
			
			return aItems;
		
		}// Items

		/// <summary>This method will allocate and populate an error items collection using the specified values</summary>
		/// <param name="strName1">Name assigned to item 1</param>
		/// <param name="objValue1">Object used to set Value property of item 1</param>
		/// <param name="strName2">Name assigned to item 2</param>
		/// <param name="objValue2">Object used to set Value property of item 2</param>
		public static CTmaxErrorArgs.CErrorItems Items(string strName1, object objValue1, string strName2, object objValue2)
		{
			return Items(strName1, objValue1, strName2, objValue2, null, null);
		}

		/// <summary>This method will allocate and populate an error items collection using the specified values</summary>
		/// <param name="strName1">Name assigned to item 1</param>
		/// <param name="objValue1">Object used to set Value property of item 1</param>
		public static CTmaxErrorArgs.CErrorItems Items(string strName1, object objValue1)
		{
			return Items(strName1, objValue1, null, null, null, null);
		}

		/// <summary>Collection of format strings used to build error messages</summary>
		public ArrayList FormatStrings
		{
			get
			{
				return m_aFormatStrings;
			}

		}

	}// class CTmaxErrorBuilder
	
}// namespace FTI.Shared
