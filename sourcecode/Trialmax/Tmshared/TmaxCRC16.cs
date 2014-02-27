using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class implements operations for CRC-16 calculations</summary>
	public class CTmaxCRC16
	{
		#region Constants

		/// <summary>Error identifiers</summary>
		private const int ERROR_GET_CRC16_EX = 0;

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Odd parity values used to calculate the CRC value</summary>
		short[] m_aOddParity = { 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0 };

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxCRC16()
		{
			SetErrorStrings();
			m_tmaxEventSource.Name = "TmaxCRC16";
		}

		/// <summary>Called to calculate the CRC for the specified string</summary>
		/// <param name="strString">The source string</param>
		/// <returns>The calculated CRC value</returns>
		public ushort GetCRC16(string strString)
		{
			ushort usCRC16 = 0;

			try
			{
				for(int i = 0; i < strString.Length; i++)
				{
					usCRC16 = Calculate((byte)strString[i], usCRC16);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCRC16", m_tmaxErrorBuilder.Message(ERROR_GET_CRC16_EX, strString), Ex);
				usCRC16 = 0;
			}
			
			return usCRC16;
		
		}// public ushort GetCRC16(string strString)

		#endregion Public Methods

		#region Private Methods

		/// <summary>Called to calculate the CRC for a byte in the source array</summary>
		/// <param name="byData">The source data btye</param>
		/// <param name="usAccumulated">The accumulated CRC value</param>
		/// <returns>The new CRC value</returns>
		private ushort Calculate(byte byData, ushort usAccumulated)
		{
			ushort usData = 0;

			usData = (ushort)byData;
			usData = (ushort)((ushort)(usData ^ (usAccumulated & 0x00ff)) & (ushort)0x00ff);

			usAccumulated >>= 8;

			if((m_aOddParity[usData & (ushort)0x000f] ^ m_aOddParity[usData >> 4]) != 0)
				usAccumulated ^= 0xc001;

			usData <<= 6;
			usAccumulated ^= usData;
			usData <<= 1;
			usAccumulated ^= usData;

			return usAccumulated;

		}// public ushort Calculate(byte byData, ushort usAccumulated)

		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;

			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;

			if(aStrings == null) return;

			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to calculate the CRC16 value for: <%1>");
		}

		#endregion Private Methods

		#region Properties

		/// <summary>The EventSource for this object</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		#endregion Properties

	}// class CTmaxCRC16

}// namespace FTI.Shared.Trialmax
