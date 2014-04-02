using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Panes
{
	/// <summary>This is the script builder's background panel. It hosts the script scene controls</summary>
	public class CScriptPanel : System.Windows.Forms.Panel
	{
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CScriptPanel() : base()
		{
			m_tmaxEventSource.Name = "Script Panel Events";
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method handles events fired by the background panel when it needs to be painted</summary>
		/// <param name="e">The system paint arguments</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnPaint", "Panel Paint");
			}
			catch
			{
			}
			
		}// private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)

		/// <summary>This method handles events fired by the background panel when it needs to be painted</summary>
		/// <param name="e">The system paint arguments</param>
		protected override void OnInvalidated(System.Windows.Forms.InvalidateEventArgs e)
		{
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnInvalidate", "Panel Invalidate");
			}
			catch
			{
			}
			
		}// private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)

		protected override void DefWndProc(ref Message m)
		{
			string strText = "";
			
			if(m.Msg == FTI.Shared.Win32.User.WM_PAINT)
			{
				strText = "WM_PAINT";
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_NCPAINT)
			{
				strText = "WM_NCPAINT";
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_ERASEBKGND)
			{
				strText = "WM_ERASEBKGND";
			}
			else
			{
				strText = m.Msg.ToString();
			}

			//m_tmaxEventSource.FireDiagnostic(this, "DEF", "PANEL DWP: " + strText);
			base.DefWndProc(ref m);
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource;	}
			
		}// EventSource property
		
		
		#endregion Properties
		
	}// public class CScriptPanel : System.Windows.Forms.Panel

}// namespace FTI.Trialmax.Panes
