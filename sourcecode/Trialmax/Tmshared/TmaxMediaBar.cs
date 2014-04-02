using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to manage a Trialmax media toolbar image strip</summary>
	public class CTmaxMediaBar
	{
		#region Private Members
		
		/// <summary>Local member bound to Images property</summary>
		private System.Windows.Forms.ImageList m_ctrlImages = null;

		/// <summary>Local member bound to ButtonWidth property</summary>
		private int m_iButtonWidth = 24;

		/// <summary>Local member bound to ButtonHeight property</summary>
		private int m_iButtonHeight = 18;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxMediaBar()
		{
		}
		
		/// <summary>This function will initialize the media bar image list</summary>
		/// <param name="Container">Component container used to initialize the image list</param>
		///	<returns>true if successful</returns>
		public bool Initialize(System.ComponentModel.IContainer Container)
		{
			try
			{
				//	Allocate and initialize the image list
				m_ctrlImages = new System.Windows.Forms.ImageList(Container);
				m_ctrlImages.ImageSize = new System.Drawing.Size(m_iButtonWidth, m_iButtonHeight);
				m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
				
				//	Get the embedded resource from the assembly
				System.IO.Stream MediaBarStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FTI.Shared.Mediabar.bmp");
				
				//	Convert the resource into a bitmap
				System.Drawing.Bitmap MediaBarBitmap = System.Drawing.Bitmap.FromStream(MediaBarStream) as System.Drawing.Bitmap;
				
				//	Populate the image list collection
				m_ctrlImages.Images.AddStrip(MediaBarBitmap);
				
				// Clean up
				MediaBarBitmap = null;
				MediaBarStream.Close();
				MediaBarStream = null;
				
				return (m_ctrlImages.Images.Count > 0);
				
			}
			catch
			{
				return false;
			}
		
		}// public bool Initialize(System.ComponentModel.IContainer Container)
		
		/// <summary>
		/// This method is called internally to convert the specified string to the associated media bar command</summary>
		/// <param name="strCommand">The text representation of the command</param>
		/// <returns>The associated media bar command</returns>
		public FTI.Shared.Trialmax.TmaxMediaBarCommands GetCommand(string strCommand)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxMediaBarCommands));
				
				foreach(TmaxMediaBarCommands eCommand in aCommands)
				{
					if(String.Compare(eCommand.ToString(), strCommand, true) == 0)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxMediaBarCommands.Invalid;
		}
		
		/// <summary>
		/// This method is called to get the image of the index associated with the media bar command
		/// </summary>
		/// <param name="eCommand">The desired media bar command</param>
		/// <returns>The index of the associated image if found</returns>
		public int GetImageIndex(TmaxMediaBarCommands eCommand)
		{
			switch(eCommand)
			{
				case TmaxMediaBarCommands.RotateCW:			return 4;
				case TmaxMediaBarCommands.RotateCCW:		return 5;
				case TmaxMediaBarCommands.Normal:			return 6;
				case TmaxMediaBarCommands.Zoom:				return 7;
				case TmaxMediaBarCommands.ZoomWidth:		return 8;
				case TmaxMediaBarCommands.Pan:				return 9;
				case TmaxMediaBarCommands.Callout:			return 10;
				case TmaxMediaBarCommands.Highlight:		return 13;
				case TmaxMediaBarCommands.Redact:			return 14;
				case TmaxMediaBarCommands.Draw:				return 11;
				case TmaxMediaBarCommands.Erase:			return 15;
				case TmaxMediaBarCommands.EraseLast:		return 16;
				case TmaxMediaBarCommands.Arrow:			return 72;
				case TmaxMediaBarCommands.Ellipse:			return 73;
				case TmaxMediaBarCommands.Freehand:			return 70;
				case TmaxMediaBarCommands.Line:				return 71;
				case TmaxMediaBarCommands.Polygon:			return 75;
				case TmaxMediaBarCommands.Rectangle:		return 74;
				case TmaxMediaBarCommands.Text:				return 69;
				case TmaxMediaBarCommands.FilledEllipse:	return 76;
				case TmaxMediaBarCommands.FilledPolygon:	return 78;
				case TmaxMediaBarCommands.ShadedCallouts:	return 55;
				case TmaxMediaBarCommands.Select:			return 17;
				case TmaxMediaBarCommands.SaveZap:			return 22;
				case TmaxMediaBarCommands.UpdateZap:		return 23;
				case TmaxMediaBarCommands.Red:				return 58;
				case TmaxMediaBarCommands.Green:			return 61;
				case TmaxMediaBarCommands.Blue:				return 64;
				case TmaxMediaBarCommands.DarkRed:			return 57;
				case TmaxMediaBarCommands.DarkGreen:		return 60;
				case TmaxMediaBarCommands.DarkBlue:			return 63;
				case TmaxMediaBarCommands.LightRed:			return 59;
				case TmaxMediaBarCommands.LightGreen:		return 62;
				case TmaxMediaBarCommands.LightBlue:		return 65;
				case TmaxMediaBarCommands.Yellow:			return 66;
				case TmaxMediaBarCommands.PureBlack:		return 67;
				case TmaxMediaBarCommands.PureWhite:		return 68;
				case TmaxMediaBarCommands.Play:				return 32;
				case TmaxMediaBarCommands.Pause:			return 31;
				case TmaxMediaBarCommands.PlayStart:		return 29;
				case TmaxMediaBarCommands.PlayEnd:			return 34;
				case TmaxMediaBarCommands.PlayBack:			return 30;
				case TmaxMediaBarCommands.PlayFwd:			return 33;
				case TmaxMediaBarCommands.Previous:			return 19;
				case TmaxMediaBarCommands.Next:				return 20;
                case TmaxMediaBarCommands.BlankPresentation:return 3;
				default:									return -1;
			}
			
		}// public static int GetImageIndex(TmaxMediaBarCommands eCommand)
		
		/// <summary>This method will translate a keystroke to a media bar command</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>the associated media bar command</returns>
		static public TmaxMediaBarCommands GetCommand(Keys eKey, Keys eModifiers)
		{
			TmaxMediaBarCommands eCommand = TmaxMediaBarCommands.Invalid;
			
			if(eModifiers == Keys.None)
			{
				switch(eKey)
				{
					case Keys.Z:				
						eCommand = TmaxMediaBarCommands.Zoom;
						break;
					case Keys.W:				
						eCommand = TmaxMediaBarCommands.ZoomWidth;
						break;
					case Keys.F:				
						eCommand = TmaxMediaBarCommands.Normal;
						break;
					case Keys.M:				
						eCommand = TmaxMediaBarCommands.SaveZap;
						break;
					case Keys.U:				
						eCommand = TmaxMediaBarCommands.UpdateZap;
						break;
					case Keys.C:				
						eCommand = TmaxMediaBarCommands.Callout;
						break;
					case Keys.H:				
						eCommand = TmaxMediaBarCommands.Highlight;
						break;
					case Keys.R:				
						eCommand = TmaxMediaBarCommands.Redact;
						break;
					case Keys.S:				
						eCommand = TmaxMediaBarCommands.Pan;
						break;
					case Keys.D:				
						eCommand = TmaxMediaBarCommands.Draw;
						break;
					case Keys.A:				
						eCommand = TmaxMediaBarCommands.Select;
						break;
					case Keys.E:				
						eCommand = TmaxMediaBarCommands.Erase;
						break;
					case Keys.Delete:			
						eCommand = TmaxMediaBarCommands.EraseLast;
						break;
					case Keys.OemOpenBrackets:	
						eCommand = TmaxMediaBarCommands.RotateCCW;
						break;
					case Keys.OemCloseBrackets:	
						eCommand = TmaxMediaBarCommands.RotateCW;
						break;
					case Keys.Oemcomma:	
						eCommand = TmaxMediaBarCommands.Previous;
						break;
					case Keys.OemPeriod:	
						eCommand = TmaxMediaBarCommands.Next;
						break;
				}
			
			}// if(eModifiers == Keys.None)
			
			else if(eModifiers == Keys.Shift)
			{
				switch(eKey)
				{
					case Keys.F:	
						eCommand = TmaxMediaBarCommands.Freehand;
						break;
					case Keys.A:	
						eCommand = TmaxMediaBarCommands.Arrow;
						break;
					case Keys.L:	
						eCommand = TmaxMediaBarCommands.Line;
						break;
					case Keys.S:	
						eCommand = TmaxMediaBarCommands.Rectangle;
						break;
					case Keys.C:	
						eCommand = TmaxMediaBarCommands.Ellipse;
						break;
					case Keys.H:	
						eCommand = TmaxMediaBarCommands.Polygon;
						break;
					case Keys.T:	
						eCommand = TmaxMediaBarCommands.Text;
						break;
				}
			
			}// else if(eModifiers == Keys.Shift)
			
			else if(eModifiers == (Keys.Control | Keys.Shift))
			{
				switch(eKey)
				{
					case Keys.C:	
						eCommand = TmaxMediaBarCommands.FilledEllipse;
						break;
					case Keys.H:	
						eCommand = TmaxMediaBarCommands.FilledPolygon;
						break;
				}
			
			}// else if(eModifiers == (Keys.Control | Keys.Shift))
			
			return eCommand;
			
		}// static public TmaxMediaBarCommands GetCommand(Keys eKey, Keys eModifiers)
		
		/// <summary>This method will retrieve the keystroke associated with the specified command</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>the associated media bar command</returns>
		static public void GetKeys(TmaxMediaBarCommands eCommand, ref Keys eKey, ref Keys eModifiers)
		{
			//	Initialize the keys
			eKey = Keys.None;
			eModifiers = Keys.None;
			
			//	Which command?
			switch(eCommand)
			{
				case TmaxMediaBarCommands.Zoom:
					eKey = Keys.Z;
					break;
				case TmaxMediaBarCommands.ZoomWidth:
					eKey = Keys.W;
					break;
				case TmaxMediaBarCommands.Normal:
					eKey = Keys.F;
					break;
				case TmaxMediaBarCommands.SaveZap:
					eKey = Keys.M;
					break;
				case TmaxMediaBarCommands.UpdateZap:
					eKey = Keys.U;
					break;
				case TmaxMediaBarCommands.Callout:
					eKey = Keys.C;
					break;
				case TmaxMediaBarCommands.Highlight:
					eKey = Keys.H;
					break;
				case TmaxMediaBarCommands.Redact:
					eKey = Keys.R;
					break;
				case TmaxMediaBarCommands.Pan:
					eKey = Keys.S;
					break;
				case TmaxMediaBarCommands.Draw:
					eKey = Keys.D;
					break;
				case TmaxMediaBarCommands.Select:
					eKey = Keys.A;
					break;
				case TmaxMediaBarCommands.Erase:
					eKey = Keys.E;
					break;
				case TmaxMediaBarCommands.EraseLast:
					eKey = Keys.Delete;
					break;
				case TmaxMediaBarCommands.RotateCCW:
					eKey = Keys.OemOpenBrackets;
					break;
				case TmaxMediaBarCommands.RotateCW:
					eKey = Keys.OemCloseBrackets;
					break;
				case TmaxMediaBarCommands.Next:
					eKey = Keys.OemPeriod;
					break;
				case TmaxMediaBarCommands.Previous:
					eKey = Keys.Oemcomma;
					break;
					
				case TmaxMediaBarCommands.Freehand:
					eKey = Keys.F;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Arrow:
					eKey = Keys.A;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Line:
					eKey = Keys.L;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Rectangle:
					eKey = Keys.S;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Ellipse:
					eKey = Keys.C;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Polygon:
					eKey = Keys.H;
					eModifiers = Keys.Shift;
					break;
				case TmaxMediaBarCommands.Text:
					eKey = Keys.T;
					eModifiers = Keys.Shift;
					break;
					
				case TmaxMediaBarCommands.FilledEllipse:
					eKey = Keys.C;
					eModifiers = (Keys.Control | Keys.Shift);
					break;
				case TmaxMediaBarCommands.FilledPolygon:
					eKey = Keys.H;
					eModifiers = (Keys.Control | Keys.Shift);
					break;
					
			}
			
		}// static public void GetKeys(TmaxMediaBarCommands eCommand, ref Keys eKey, ref Keys eModifiers)
		
		#endregion Public Methods
		
		#region Properties
	
		/// <summary>The image list for the media toolbar</summary>
		public System.Windows.Forms.ImageList Images
		{
			get
			{
				return m_ctrlImages;
			}
			
		} // Images property
		
		/// <summary>The width of each button</summary>
		public int ButtonWidth
		{
			get
			{
				return m_iButtonWidth;
			}
			
		} // ButtonWidth property
		
		/// <summary>The height of each button</summary>
		public int ButtonHeight
		{
			get
			{
				return m_iButtonHeight;
			}
			
		} // ButtonHeight property

		#endregion Properties

	}// public class CTmaxMediaBar

}// namespace FTI.Shared.Trialmax
