using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>
	/// Objects of this class are used to pass parameters in TrialMax events
	/// </summary>
	public class CTmaxParameter
	{
		#region Private Members
		
		/// <summary>Local member accessed by Name property</summary>
		private string m_strName;
			
		/// <summary>Local member accessed by strValue property</summary>
		private string m_strValue;
			
		/// <summary>Local member accessed by dValue property</summary>
		private double m_dValue;
			
		/// <summary>Local member accessed by lValue property</summary>
		private long m_lValue;
		
		#endregion Private Members
		
		#region Public Methods
			
		/// <summary>Default constructor</summary>
		public CTmaxParameter()
		{
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		public CTmaxParameter(string strName)
		{
			m_strName = strName;
		}
		
		/// <summary>String based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="strValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, string strValue)
		{
			m_strName = strName;
			SetValue(strValue);
		}
		
		/// <summary>Double based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="dValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, double dValue)
		{
			m_strName = strName;
			SetValue(dValue);
		}
		
		/// <summary>Single based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="fValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, float fValue)
		{
			m_strName = strName;
			SetValue(fValue);
		}
		
		/// <summary>Integer based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="iValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, int iValue)
		{
			m_strName = strName;
			SetValue(iValue);
		}
		
		/// <summary>Long based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="lValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, long lValue)
		{
			m_strName = strName;
			SetValue(lValue);
		}
		
		/// <summary>Boolean based constructor</summary>
		/// <param name="strName">Name assigned to parameter</param>
		/// <param name="bValue">Value assigned to the parameter</param>
		public CTmaxParameter(string strName, bool bValue)
		{
			m_strName = strName;
			SetValue(bValue);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName)
		{
			m_strName = eName.ToString();
		}
		
		/// <summary>String based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="strValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, string strValue)
		{
			m_strName = eName.ToString();
			SetValue(strValue);
		}
		
		/// <summary>Double based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="dValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, double dValue)
		{
			m_strName = eName.ToString();
			SetValue(dValue);
		}
		
		/// <summary>Single based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="fValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, float fValue)
		{
			m_strName = eName.ToString();
			SetValue(fValue);
		}
		
		/// <summary>Integer based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="iValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, int iValue)
		{
			m_strName = eName.ToString();
			SetValue(iValue);
		}
		
		/// <summary>Long based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="lValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, long lValue)
		{
			m_strName = eName.ToString();
			SetValue(lValue);
		}
		
		/// <summary>Boolean based constructor</summary>
		/// <param name="eName">Enumerated TrialMax command parameter</param>
		/// <param name="bValue">Value assigned to the parameter</param>
		public CTmaxParameter(TmaxCommandParameters eName, bool bValue)
		{
			m_strName = eName.ToString();
			SetValue(bValue);
		}
		
		/// <summary>This method is called to read the parameter value as a string</summary>
		/// <returns>The current value as a string</returns>
		public string AsString()
		{
			return m_strValue;
		}
		
		/// <summary>This method is called to read the parameter value as an integer</summary>
		/// <returns>The current value as an integer</returns>
		public int AsInteger()
		{
			return System.Convert.ToInt32(m_lValue);
		}
		
		/// <summary>This method is called to read the parameter value as a long integer</summary>
		/// <returns>The current value as a long integer</returns>
		public long AsLong()
		{
			return m_lValue;
		}
		
		/// <summary>This method is called to read the parameter value as a boolean </summary>
		/// <returns>The current value as a boolean</returns>
		public bool AsBoolean()
		{
			return (m_lValue != 0) ? true : false;
		}
		
		/// <summary>This method is called to read the parameter value as a double precision floating point value</summary>
		/// <returns>The current value as a double precision floating point value</returns>
		public double AsDouble()
		{
			return m_dValue;
		}
		
		/// <summary>This method is called to read the parameter value as a single precision floating point value</summary>
		/// <returns>The current value as a single precision floating point value</returns>
		public float AsSingle()
		{
			return System.Convert.ToSingle(m_dValue);
		}
		
		/// <summary>This method is called to read the parameter as a key value pair (key=value) like in an ini file</summary>
		/// <returns>The parameter as a key value pair</returns>
		public string AsKeyValuePair()
		{
			string strKeyValue = Name + "=" + AsString();
			return strKeyValue;
		}
		
		/// <summary>This method is called to set the current value using the specified string</summary>
		/// <param name="strValue">The current value as a string</param>
		public void SetValue(string strValue)
		{
			m_strValue = strValue;
			
			m_dValue = 0;
			m_lValue = 0;
			
			try
			{
				//	check for boolean values first
				if(String.Compare(strValue, "true", true) == 0)
				{
					SetValue(true);
				}
				else if(String.Compare(strValue, "false", true) == 0)
				{
					SetValue(false);
				}
				else
				{
					m_dValue = System.Convert.ToDouble(strValue);
					m_lValue = System.Convert.ToInt64(strValue);
				}
			}
			catch
			{
			}
		
		}
		
		/// <summary>This method is called to set the current value using the specified long integer</summary>
		/// <param name="lValue">The current value as a long integer</param>
		public void SetValue(long lValue)
		{
			m_lValue = lValue;
			
			m_strValue = "";
			m_dValue = 0;
			
			try
			{
				m_dValue = System.Convert.ToDouble(lValue);
				m_strValue = System.Convert.ToString(lValue);
			}
			catch
			{
			}
		}
		
		/// <summary>This method is called to set the current value using the specified boolean</summary>
		/// <param name="bValue">The current value as a boolean</param>
		public void SetValue(bool bValue)
		{
			if(bValue == true)
			{
				m_dValue = 0.5;
				m_lValue = 1;
				m_strValue = "true";
			}
			else
			{
				m_lValue = 0;
				m_strValue = "false";
				m_dValue = 0;
			}
		}
		
		/// <summary>This method is called to set the current value using the specified double</summary>
		/// <param name="dValue">The current value as a double precision value</param>
		public void SetValue(double dValue)
		{
			m_dValue = dValue;
			
			m_strValue = "";
			m_lValue = 0;
			
			try
			{
				m_lValue = System.Convert.ToInt64(dValue);
				m_strValue = System.Convert.ToString(dValue);
			}
			catch
			{
			}
		}
		
		/// <summary>This method is called to set the current value using the specified single precision value</summary>
		/// <param name="fValue">The current value as a single precision value</param>
		public void SetValue(float fValue)
		{
			//	Promote to a double to set the value
			SetValue((double)fValue);
		}
		
		/// <summary>This method is called to set the current value using the specified integer value</summary>
		/// <param name="fValue">The current value as an integer value</param>
		public void SetValue(int iValue)
		{
			//	Promote to a long to set the value
			SetValue((long)iValue);
		}
		
		#endregion Public Methods
	
		#region Protected Methods
		
		
		#endregion Protected Methods
			
		#region Properties
		
		/// <summary>This property contains the parameter name</summary>
		public string Name
		{
			get
			{
				return m_strName;
			}
			set
			{
				m_strName = value;
			}
			
		} // SName property

		/// <summary>This reads and writes the parameter value as a string value</summary>
		public string strValue
		{
			get
			{
				return AsString();
			}
			set
			{
				SetValue(value);
			}
			
		} // strValue property

		/// <summary>This reads and writes the parameter value as a double precision floating point value</summary>
		public double dValue
		{
			get
			{
				return AsDouble();
			}
			set
			{
				SetValue(value);
			}
			
		} // dValue property
		
		/// <summary>This reads and writes the parameter value as a single precision floating point value</summary>
		public float fValue
		{
			get
			{
				return AsSingle();
			}
			set
			{
				SetValue(value);
			}
			
		} // fValue property
		
		/// <summary>This reads and writes the parameter value as a boolean value</summary>
		public bool bValue
		{
			get
			{
				return AsBoolean();
			}
			set
			{
				SetValue(value);
			}
			
		} // bValue property
		
		/// <summary>This reads and writes the parameter value as an integer value</summary>
		public int iValue
		{
			get
			{
				return AsInteger();
			}
			set
			{
				SetValue(value);
			}
			
		} // iValue property
		
		/// <summary>This reads and writes the parameter value as a long integer value</summary>
		public long lValue
		{
			get
			{
				return AsLong();
			}
			set
			{
				SetValue(value);
			}
			
		} // lValue property
		
		#endregion Properties
		
	}//	CTmaxParameter
		
	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxParameter objects
	/// </summary>
	public class CTmaxParameters : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxParameters()
		{
		}

		/// <summary>This method allows the caller to add a new parameter to the list</summary>
		/// <param name="Parameter">CTmaxParameter object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxParameter Add(CTmaxParameter tmaxParameter)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(tmaxParameter as object);

				return tmaxParameter;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxParameter tmaxParameter)

		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="lValue">The string value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, string strValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, strValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="lValue">The double precision floating point value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, double dValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, dValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="lValue">The single precision floating point value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, float fValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, fValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="lValue">The integer value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, int iValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, iValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="lValue">The long integer value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, long lValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, lValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="strName">The parameter name</param>
		///	<param name="bValue">The boolean value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(string strName, bool bValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(strName, bValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="lValue">The string value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, string strValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, strValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="lValue">The double precision floating point value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, double dValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, dValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="lValue">The single precision floating point value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, float fValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, fValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="lValue">The integer value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, int iValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, iValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="lValue">The long integer value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, long lValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, lValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>Adds a new parameter with the specified name and value</summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		///	<param name="bValue">The boolean value</param>
		/// <returns>The object added to the collection</returns>
		public CTmaxParameter Add(TmaxCommandParameters eName, bool bValue)
		{
			CTmaxParameter tmaxParameter = null;
			
			if((tmaxParameter = new CTmaxParameter(eName, bValue)) != null)
			{
				return Add(tmaxParameter);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>
		/// This method is called to remove the requested parameter from the collection
		/// </summary>
		/// <param name="tmaxParameter">The parameter object to be removed</param>
		public void Remove(CTmaxParameter tmaxParameter)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxParameter as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="tmaxParameter">The parameter object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxParameter tmaxParameter)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxParameter as object);
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxParameter this[string strName]
		{
			get 
			{
				return Find(strName);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified TrialMax command enumeration
		/// </summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		/// <returns>The object with the specified command enumeration</returns>
		public CTmaxParameter this[TmaxCommandParameters eName]
		{
			get
			{
				return Find(eName);
			}
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxParameter Find(string strName)
		{
			// Search for the object with the same name
			foreach(CTmaxParameter obj in base.List)
			{
				if(String.Compare(obj.Name, strName, true) == 0)
				{
					return obj;
				}
			}
			return null;

		}//	Find(string strName)

		/// <summary>
		/// Called to locate the object with the specified TrialMax command parameter
		/// </summary>
		/// <param name="eName">The enumerated TrialMax command parameter</param>
		/// <returns>The object with the specified enumeration if found</returns>
		public CTmaxParameter Find(TmaxCommandParameters eName)
		{
			return Find(eName.ToString());

		}//	Find(string eName)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxParameter this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxParameter);
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxParameter value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	CTmaxParameters
		
}
