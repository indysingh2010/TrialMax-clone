using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>This class allows the user to choose a valid Tmview color</summary>
	public class CTmxViewColorPicker : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>List box for displaying available colors</summary>
		private System.Windows.Forms.ListView m_ctrlPicker;
		
		/// <summary>Image list containing available colors</summary>
		private System.Windows.Forms.ImageList m_ctrlColorImages;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Local member bound to Color property</summary>
		private TmxViewColors m_eColor = TmxViewColors.Yellow;
		
		/// <summary>Local member required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		#endregion Private Members

		#region Public Methods
		
		public CTmxViewColorPicker()
		{
			InitializeComponent();
			
			//	Set the current selection
			SetSelection();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>This method will set the selection in the list box to match the current Color property</summary>
		protected void SetSelection()
		{
			//	Make sure we have the required objects
			if((m_ctrlPicker == null) || (m_ctrlPicker.Items == null) || (m_ctrlPicker.Items.Count == 0)) return;
			
			foreach(ListViewItem lvItem in m_ctrlPicker.Items)
			{
				if(lvItem.Tag != null)
				{
					if(String.Compare(m_eColor.ToString(), (string)lvItem.Tag, true) == 0)
						lvItem.Selected = true;
					else
						lvItem.Selected = false;
				}
				else
				{
					lvItem.Selected = false;
				}
				
			}// foreach(ListViewItem lvItem in m_ctrlPicker.Items)
				
		}// protected void SetSelection()
		
		/// <summary>This method will get the selection in the list box</summary>
		protected void GetSelection()
		{
			//	Make sure we have the required objects
			if((m_ctrlPicker == null) || (m_ctrlPicker.Items == null) || (m_ctrlPicker.Items.Count == 0)) return;
			
			foreach(ListViewItem lvItem in m_ctrlPicker.Items)
			{
				if(lvItem.Selected == true && lvItem.Tag != null)
				{
					switch((string)lvItem.Tag)
					{
						case "Red":
						
							m_eColor = TmxViewColors.Red;
							break;
							
						case "Green":
						
							m_eColor = TmxViewColors.Green;
							break;
							
						case "Blue":
						
							m_eColor = TmxViewColors.Blue;
							break;
							
						case "Yellow":
						
							m_eColor = TmxViewColors.Yellow;
							break;
							
						case "DarkRed":
						
							m_eColor = TmxViewColors.DarkRed;
							break;
							
						case "DarkGreen":
						
							m_eColor = TmxViewColors.DarkGreen;
							break;
							
						case "DarkBlue":
						
							m_eColor = TmxViewColors.DarkBlue;
							break;
							
						case "LightRed":
						
							m_eColor = TmxViewColors.LightRed;
							break;
							
						case "LightGreen":
						
							m_eColor = TmxViewColors.LightGreen;
							break;
							
						case "LightBlue":
						
							m_eColor = TmxViewColors.LightBlue;
							break;
							
						case "Black":
						
							m_eColor = TmxViewColors.Black;
							break;
							
						case "White":
						
							m_eColor = TmxViewColors.White;
							break;
							
						default:
						
							Debug.Assert(false);
							m_eColor = TmxViewColors.Red;
							break;
							
					}
					
					break;
				}
				
			}// foreach(ListViewItem lvItem in m_ctrlPicker.Items)
				
		}// protected void SetSelection()
		
		#region Windows Form Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Red", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 0);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Green", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 1);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Blue", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 2);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Yellow", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 5);
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Black", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 7);
			System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "White", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 8);
			System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Dark Red", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 9);
			System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Dark Green", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 10);
			System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Dark Blue", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 11);
			System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								 new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Light Red", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 12);
			System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								 new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Light Green", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 13);
			System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								 new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Light Blue", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0))))}, 14);
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmxViewColorPicker));
			this.m_ctrlPicker = new System.Windows.Forms.ListView();
			this.m_ctrlColorImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlPicker
			// 
			this.m_ctrlPicker.FullRowSelect = true;
			listViewItem1.StateImageIndex = 0;
			listViewItem1.Tag = "Red";
			listViewItem2.StateImageIndex = 0;
			listViewItem2.Tag = "Green";
			listViewItem3.StateImageIndex = 0;
			listViewItem3.Tag = "Blue";
			listViewItem4.Tag = "Yellow";
			listViewItem5.StateImageIndex = 0;
			listViewItem5.Tag = "Black";
			listViewItem6.StateImageIndex = 0;
			listViewItem6.Tag = "White";
			listViewItem7.Tag = "DarkRed";
			listViewItem8.Tag = "DarkGreen";
			listViewItem9.Tag = "DarkBlue";
			listViewItem10.Tag = "LightRed";
			listViewItem11.Tag = "LightGreen";
			listViewItem12.Tag = "LightBlue";
			this.m_ctrlPicker.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						 listViewItem1,
																						 listViewItem2,
																						 listViewItem3,
																						 listViewItem4,
																						 listViewItem5,
																						 listViewItem6,
																						 listViewItem7,
																						 listViewItem8,
																						 listViewItem9,
																						 listViewItem10,
																						 listViewItem11,
																						 listViewItem12});
			this.m_ctrlPicker.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlPicker.MultiSelect = false;
			this.m_ctrlPicker.Name = "m_ctrlPicker";
			this.m_ctrlPicker.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlPicker.Size = new System.Drawing.Size(234, 124);
			this.m_ctrlPicker.SmallImageList = this.m_ctrlColorImages;
			this.m_ctrlPicker.TabIndex = 3;
			this.m_ctrlPicker.View = System.Windows.Forms.View.List;
			// 
			// m_ctrlColorImages
			// 
			this.m_ctrlColorImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.m_ctrlColorImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlColorImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlColorImages.ImageStream")));
			this.m_ctrlColorImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.Location = new System.Drawing.Point(42, 144);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 4;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(134, 144);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 5;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// CTmxViewColorPicker
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(250, 175);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk,
																		  this.m_ctrlPicker});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CTmxViewColorPicker";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color Picker";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// This method is called when the user clicks on OK
		/// </summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			//	Update the color property
			GetSelection();
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Th current color selection</summary>
		public TmxViewColors Color
		{
			get
			{
				return m_eColor;
			}
			set
			{
				m_eColor = value;
				
				if(m_ctrlPicker != null)
					SetSelection();
			}

		}// Color property
		
		
		#endregion Properties
		
	
	}// public class CTmxViewColorPicker : System.Windows.Forms.Form

}// namespace FTI.Trialmax.ActiveX
