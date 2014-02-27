using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control allows the user to edit the system's drive aliases</summary>
	public class CTmaxAliasesCtrl : System.Windows.Forms.UserControl
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Form's image list</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>Form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlUndoAll;

		/// <summary>Label for the current value static text control</summary>
		private System.Windows.Forms.Label m_ctrlUserPathLabel;

		/// <summary>Static text control to display current value</summary>
		private System.Windows.Forms.TextBox m_ctrlUserPath;

		/// <summary>Label for the original value static text control</summary>
		private System.Windows.Forms.Label m_ctrlOriginalLabel;

		/// <summary>Static text control to display original value</summary>
		private System.Windows.Forms.Label m_ctrlOriginal;

		/// <summary>Label for the previous value static text control</summary>
		private System.Windows.Forms.Label m_ctrlPreviousLabel;

		/// <summary>Static text control to display previous value</summary>
		private System.Windows.Forms.Label m_ctrlPrevious;

		/// <summary>List box to display aliases</summary>
		private System.Windows.Forms.ListView m_ctrlAliases;

		/// <summary>Column to display the image to indicate if the folder exists</summary>
		private System.Windows.Forms.ColumnHeader I;

		/// <summary>Current value assigned to the alias</summary>
		private System.Windows.Forms.ColumnHeader ALIAS;

		/// <summary>Browse button</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;
		
		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;

		/// <summary>Local flag to indicate if the collection is read only</summary>
		private bool m_bReadOnly = false;

		/// <summary>Local member to inhibit event processing</summary>
		private bool m_bIgnoreEvents = false;
		
		/// <summary>Local member to prevent reentrancy while restoring the list view selection</summary>
		private bool m_bRestoringSelection = false;
		
		/// <summary>Local member to control automatic updating</summary>
		private bool m_bSetOnSelect = false;
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to CaseOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseOptions m_tmaxCaseOptions = null;

		private System.Windows.Forms.ColumnHeader A;
		private System.Windows.Forms.ColumnHeader NEW;
		private System.Windows.Forms.Button m_ctrlUndo;
		private System.Windows.Forms.Button m_ctrlSetAlias;

		/// <summary>Local member to keep track of alias collection</summary>
		private FTI.Shared.Trialmax.CTmaxAliases m_tmaxAliases = null;

		/// <summary>Local member to keep track of active alias</summary>
		private FTI.Shared.Trialmax.CTmaxAlias m_tmaxAlias = null;

		#endregion Private Members

		#region Public Methods
		
		public CTmaxAliasesCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		
		/// <summary>This method applies the user changes</summary>
		/// <returns>true if successful</returns>
		public bool Apply()
		{
			//	Make sure the existing alias is updated
			if(m_tmaxAlias != null)
				SetAliasPath(true);
				
			//	Update the values of any of those that have changed?
			foreach(CTmaxAlias O in m_tmaxAliases)
			{
				if(O.Modified == true)
				{
					O.Previous = O.Current;
					O.Current = O.Editor;
					O.Modified = false;
					
					//	Update the flag to indicate one or more have been edited
					m_bModified = true;
				} 
				
				O.Editor = "";
				
			}// foreach(CTmaxAlias O in m_tmaxAliases)
			
			//	Make sure we release the lock
			if((m_tmaxAliases != null) && (m_bReadOnly == false))
				m_tmaxCaseOptions.ReleaseAliases(m_bModified);
				
			return true;
						
		}// private bool Apply()

		/// <summary>This method is called by the parent form when the user cancels</summary>
		/// <returns>true if ok to close the form</returns>
		public bool Cancel()
		{
			//	Make sure we release the lock
			if((m_tmaxAliases != null) && (m_bReadOnly == false))
				m_tmaxCaseOptions.ReleaseAliases(false);
				
			return true;

		}// private bool Cancel()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
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
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxAliasesCtrl));
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlSetAlias = new System.Windows.Forms.Button();
			this.m_ctrlUndo = new System.Windows.Forms.Button();
			this.m_ctrlAliases = new System.Windows.Forms.ListView();
			this.I = new System.Windows.Forms.ColumnHeader();
			this.ALIAS = new System.Windows.Forms.ColumnHeader();
			this.A = new System.Windows.Forms.ColumnHeader();
			this.NEW = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlPreviousLabel = new System.Windows.Forms.Label();
			this.m_ctrlPrevious = new System.Windows.Forms.Label();
			this.m_ctrlUserPathLabel = new System.Windows.Forms.Label();
			this.m_ctrlUserPath = new System.Windows.Forms.TextBox();
			this.m_ctrlOriginalLabel = new System.Windows.Forms.Label();
			this.m_ctrlOriginal = new System.Windows.Forms.Label();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlUndoAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlSetAlias
			// 
			this.m_ctrlSetAlias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSetAlias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlSetAlias.Location = new System.Drawing.Point(256, 183);
			this.m_ctrlSetAlias.Name = "m_ctrlSetAlias";
			this.m_ctrlSetAlias.TabIndex = 43;
			this.m_ctrlSetAlias.Text = "&Set Map";
			this.m_ctrlSetAlias.Click += new System.EventHandler(this.OnClickSetAlias);
			// 
			// m_ctrlUndo
			// 
			this.m_ctrlUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlUndo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlUndo.Location = new System.Drawing.Point(8, 183);
			this.m_ctrlUndo.Name = "m_ctrlUndo";
			this.m_ctrlUndo.TabIndex = 41;
			this.m_ctrlUndo.Text = "&Undo";
			this.m_ctrlUndo.Click += new System.EventHandler(this.OnClickUndo);
			// 
			// m_ctrlAliases
			// 
			this.m_ctrlAliases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAliases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.I,
																							this.ALIAS,
																							this.A,
																							this.NEW});
			this.m_ctrlAliases.FullRowSelect = true;
			this.m_ctrlAliases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_ctrlAliases.HideSelection = false;
			this.m_ctrlAliases.LabelWrap = false;
			this.m_ctrlAliases.Location = new System.Drawing.Point(8, 7);
			this.m_ctrlAliases.MultiSelect = false;
			this.m_ctrlAliases.Name = "m_ctrlAliases";
			this.m_ctrlAliases.Size = new System.Drawing.Size(320, 84);
			this.m_ctrlAliases.SmallImageList = this.m_ctrlImages;
			this.m_ctrlAliases.TabIndex = 38;
			this.m_ctrlAliases.View = System.Windows.Forms.View.Details;
			this.m_ctrlAliases.SelectedIndexChanged += new System.EventHandler(this.OnSelChanged);
			// 
			// I
			// 
			this.I.Text = "I";
			this.I.Width = 16;
			// 
			// ALIAS
			// 
			this.ALIAS.Text = "ALIAS";
			// 
			// A
			// 
			this.A.Text = "ARROW";
			this.A.Width = 30;
			// 
			// NEW
			// 
			this.NEW.Text = "NEW";
			// 
			// m_ctrlPreviousLabel
			// 
			this.m_ctrlPreviousLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlPreviousLabel.Location = new System.Drawing.Point(8, 119);
			this.m_ctrlPreviousLabel.Name = "m_ctrlPreviousLabel";
			this.m_ctrlPreviousLabel.Size = new System.Drawing.Size(56, 16);
			this.m_ctrlPreviousLabel.TabIndex = 49;
			this.m_ctrlPreviousLabel.Text = "Previous:";
			this.m_ctrlPreviousLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPrevious
			// 
			this.m_ctrlPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPrevious.Location = new System.Drawing.Point(68, 119);
			this.m_ctrlPrevious.Name = "m_ctrlPrevious";
			this.m_ctrlPrevious.Size = new System.Drawing.Size(256, 16);
			this.m_ctrlPrevious.TabIndex = 48;
			this.m_ctrlPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlUserPathLabel
			// 
			this.m_ctrlUserPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlUserPathLabel.Location = new System.Drawing.Point(8, 147);
			this.m_ctrlUserPathLabel.Name = "m_ctrlUserPathLabel";
			this.m_ctrlUserPathLabel.Size = new System.Drawing.Size(56, 20);
			this.m_ctrlUserPathLabel.TabIndex = 47;
			this.m_ctrlUserPathLabel.Text = "Map To:";
			this.m_ctrlUserPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlUserPath
			// 
			this.m_ctrlUserPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUserPath.Location = new System.Drawing.Point(68, 147);
			this.m_ctrlUserPath.Name = "m_ctrlUserPath";
			this.m_ctrlUserPath.Size = new System.Drawing.Size(228, 20);
			this.m_ctrlUserPath.TabIndex = 39;
			this.m_ctrlUserPath.Text = "";
			// 
			// m_ctrlOriginalLabel
			// 
			this.m_ctrlOriginalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlOriginalLabel.Location = new System.Drawing.Point(8, 99);
			this.m_ctrlOriginalLabel.Name = "m_ctrlOriginalLabel";
			this.m_ctrlOriginalLabel.Size = new System.Drawing.Size(56, 16);
			this.m_ctrlOriginalLabel.TabIndex = 46;
			this.m_ctrlOriginalLabel.Text = "Original:";
			this.m_ctrlOriginalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlOriginal
			// 
			this.m_ctrlOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOriginal.Location = new System.Drawing.Point(68, 99);
			this.m_ctrlOriginal.Name = "m_ctrlOriginal";
			this.m_ctrlOriginal.Size = new System.Drawing.Size(256, 16);
			this.m_ctrlOriginal.TabIndex = 45;
			this.m_ctrlOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(304, 147);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 40;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlUndoAll
			// 
			this.m_ctrlUndoAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlUndoAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlUndoAll.Location = new System.Drawing.Point(88, 183);
			this.m_ctrlUndoAll.Name = "m_ctrlUndoAll";
			this.m_ctrlUndoAll.TabIndex = 42;
			this.m_ctrlUndoAll.Text = " Undo &All";
			// 
			// CTmaxAliasesCtrl
			// 
			this.Controls.Add(this.m_ctrlSetAlias);
			this.Controls.Add(this.m_ctrlUndo);
			this.Controls.Add(this.m_ctrlAliases);
			this.Controls.Add(this.m_ctrlPreviousLabel);
			this.Controls.Add(this.m_ctrlPrevious);
			this.Controls.Add(this.m_ctrlUserPathLabel);
			this.Controls.Add(this.m_ctrlUserPath);
			this.Controls.Add(this.m_ctrlOriginalLabel);
			this.Controls.Add(this.m_ctrlOriginal);
			this.Controls.Add(this.m_ctrlBrowse);
			this.Controls.Add(this.m_ctrlUndoAll);
			this.Name = "CTmaxAliasesCtrl";
			this.Size = new System.Drawing.Size(336, 212);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to populate the list box</summary>
		/// <returns>true if successful</returns>
		protected bool Fill()
		{
			bool			bSuccessful = false;
			ListViewItem	lvItem = null;
			
			Debug.Assert(m_ctrlAliases != null);
			Debug.Assert(m_ctrlAliases.IsDisposed == false);
			if(m_ctrlAliases == null) return false;
			if(m_ctrlAliases.IsDisposed == true) return false;
			
			try
			{
				//	Clear the existing aliases
				m_ctrlAliases.Items.Clear();
				
				//	Do we have an active list of aliases?
				if((m_tmaxAliases != null) && (m_tmaxAliases.Count > 0))
				{
					foreach(CTmaxAlias O in m_tmaxAliases)
					{
						//	Make sure the modified flag is reset
						O.Modified = false;
						
						lvItem = new ListViewItem();
						
						SetListItemProps(lvItem, O, true);
						
						m_ctrlAliases.Items.Add(lvItem);
					}
				
				}
				
				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_ctrlAliases.Columns[1].Width = -2;
				m_ctrlAliases.Columns[3].Width = -2;
				ResumeLayout();
			
				//	Select the first alias in the list
				if(m_ctrlAliases.Items.Count > 0)
					m_ctrlAliases.Items[0].Selected = true;
					
				//	Done
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", "An exception was raised while filling the alias list", Ex);
			}
			finally
			{
			}

			return bSuccessful;
		
		}// protected bool Fill()
		
		/// <summary>This method is called to set the properties for the list item</summary>
		///	<param name="lvItem">The list item being set</param>
		///	<param name="tmaxAlias">The link associated with the item</param>
		/// <param name="bInitialize">true if the item is being initialized</param>
		/// <returns>the updated item</returns>
		protected ListViewItem SetListItemProps(ListViewItem lvItem, CTmaxAlias tmaxAlias, bool bInitialize)
		{
			string	strText = "";
			bool	bExists = false;
			
			Debug.Assert(lvItem != null);
			Debug.Assert(tmaxAlias != null);
			if(lvItem == null) return null;
			if(tmaxAlias == null) return null;
			
			//	Set the item tag so we can reference back to the alias object
			lvItem.Tag = tmaxAlias;
			
			//	Does the folder for this alias exist?
			try
			{
				if(tmaxAlias.Modified == true)
					bExists = System.IO.Directory.Exists(tmaxAlias.Editor);
				else
					bExists = System.IO.Directory.Exists(tmaxAlias.Current);
			}
			catch
			{
				bExists = false;
			}
			
			lvItem.ImageIndex = bExists ? 1 : 2;
							
			//	Set the current path
			if(tmaxAlias.Current.EndsWith("\\"))
				strText = tmaxAlias.Current.Substring(0, tmaxAlias.Current.Length - 1);
			else
				strText = tmaxAlias.Current;

			if(bInitialize == true)
				lvItem.SubItems.Add(strText);
			else
				lvItem.SubItems[1].Text = strText;
								
			//	Has the alias been modified
			if(tmaxAlias.Modified == true)
			{
				//	Set the modified path
				if(tmaxAlias.Editor.EndsWith("\\"))
					strText = tmaxAlias.Editor.Substring(0, tmaxAlias.Editor.Length - 1);
				else
					strText = tmaxAlias.Editor;

				if(bInitialize == true)
				{
					lvItem.SubItems.Add("-->");
					lvItem.SubItems.Add(strText);
				}
				else
				{
					lvItem.SubItems[2].Text = "-->";
					lvItem.SubItems[3].Text = strText;
				}
			}
			else
			{
				if(bInitialize == true)
				{
					lvItem.SubItems.Add("");
					lvItem.SubItems.Add("");
				}
				else
				{
					lvItem.SubItems[2].Text = "";
					lvItem.SubItems[3].Text = "";
				}
			
			}
			
			return lvItem;
		
		}// protected ListViewItem SetListItemProps(ListViewItem lvItem, CTmaxAlias tmaxAlias)
		
		/// <summary>This method is called to set the selection in the list box</summary>
		/// <param name="tmaxAlias">The alias to be selected</param>
		/// <returns>true if successful</returns>
		public bool SetListSelection(CTmaxAlias tmaxAlias)
		{
			ListViewItem lvItem = null;
			bool		 bSuccessful = true;
			
			try
			{
				m_bIgnoreEvents = true;
				
				//	Clear the current selections
				if(m_ctrlAliases.SelectedItems != null)
					m_ctrlAliases.SelectedItems.Clear();
				
				if(tmaxAlias != null)
				{
					if((lvItem = GetListItem(tmaxAlias)) != null)
					{
						lvItem.Selected = true;
						m_ctrlAliases.EnsureVisible(lvItem.Index);
					}
				}

			}
			catch
			{
				bSuccessful = false;
			}
			finally
			{
				m_bIgnoreEvents = false;
			}
			
			return bSuccessful;
			
		}// public bool SetListSelection(CTmaxAlias tmaxAlias)
		
		/// <summary>This method is called to get the list view item associated with the specified alias</summary>
		///	<param name="tmaxAlias">The link associated with the item</param>
		/// <returns>the list item if found</returns>
		protected ListViewItem GetListItem(CTmaxAlias tmaxAlias)
		{
			Debug.Assert(tmaxAlias != null);
			
			foreach(ListViewItem O in m_ctrlAliases.Items)
			{
				if(ReferenceEquals(tmaxAlias, O.Tag) == true)
					return O;
			}
			
			return null;
		
		}// protected ListViewItem GetListItem(CTmaxAlias tmaxAlias)
		
		/// <summary>This method handle's the Browse button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			FTI.Shared.CBrowseForFolder browser = new CBrowseForFolder();
			string strPath = "";
			
			browser.Folder = m_ctrlUserPath.Text;
			browser.Prompt = "Select the new alias drive / server : ";
			browser.NoNewFolder = false;
			
			if(browser.ShowDialog(this.Handle) == DialogResult.OK)
			{
				if(CTmaxAlias.PathToAlias(browser.Folder, ref strPath) == true)
					m_ctrlUserPath.Text = strPath;
				else
					FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);

			}
			
		}

		/// <summary>This method handle's the Undo button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickUndo(object sender, System.EventArgs e)
		{
			if(m_tmaxAlias != null)
				Undo(m_tmaxAlias);
		}

		/// <summary>This method handle's the Undo All button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickUndoAll(object sender, System.EventArgs e)
		{
			if(m_tmaxAliases != null)
			{
				foreach(CTmaxAlias O in m_tmaxAliases)
				{
					try
					{
						Undo(O);
					}
					catch
					{
					}
				}
			}
		
		}// private void OnClickUndoAll(object sender, System.EventArgs e)

		/// <summary>This method handle's the SetAlias button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSetAlias(object sender, System.EventArgs e)
		{
			if(m_tmaxAlias != null)
			{
				SetAliasPath(true);
			}
		}

		/// <summary>This method is called to set the active alias</summary>
		/// <param name="tmaxAlias">The alias to be activated</param>
		/// <param name="bSynchronize">true to synchronize the list view selection</param>
		/// <returns>true if successful</returns>
		private bool SetAlias(CTmaxAlias tmaxAlias, bool bSynchronize)
		{
			//	Select this item in the list box
			if(bSynchronize == true)
				SetListSelection(tmaxAlias);
			
			//	Reset the local members and update the controls
			m_tmaxAlias = tmaxAlias;
			
			m_ctrlUndoAll.Enabled = ((tmaxAlias != null) && (m_tmaxAliases.IsModifed() == true));
			m_ctrlUndo.Enabled = ((tmaxAlias != null) && (tmaxAlias.Modified == true));
			m_ctrlBrowse.Enabled = ((tmaxAlias != null) && (m_bReadOnly == false));
			m_ctrlSetAlias.Enabled = ((tmaxAlias != null) && (m_bReadOnly == false));
			m_ctrlUserPath.Enabled = ((tmaxAlias != null) && (m_bReadOnly == false));
			m_ctrlPrevious.Enabled = (tmaxAlias != null);
			m_ctrlOriginal.Enabled = (tmaxAlias != null);
			m_ctrlUserPathLabel.Enabled = ((tmaxAlias != null) && (m_bReadOnly == false));
			m_ctrlPreviousLabel.Enabled = (tmaxAlias != null);
			m_ctrlOriginalLabel.Enabled = (tmaxAlias != null);
			
			if(m_tmaxAlias != null)
			{
				if(m_tmaxAlias.Modified == true)
				{
					if(m_tmaxAlias.Editor.EndsWith("\\") == true)
						m_ctrlUserPath.Text = m_tmaxAlias.Editor.Substring(0, m_tmaxAlias.Editor.Length - 1);
					else
						m_ctrlUserPath.Text = m_tmaxAlias.Editor;
				}
				else
				{
					if(m_tmaxAlias.Current.EndsWith("\\") == true)
						m_ctrlUserPath.Text = m_tmaxAlias.Current.Substring(0, m_tmaxAlias.Current.Length - 1);
					else
						m_ctrlUserPath.Text = m_tmaxAlias.Current;
				}

				if(m_tmaxAlias.Original.EndsWith("\\") == true)
					m_ctrlOriginal.Text = m_tmaxAlias.Original.Substring(0, m_tmaxAlias.Original.Length - 1);
				else
					m_ctrlOriginal.Text = m_tmaxAlias.Original;
			
				if(m_tmaxAlias.Previous.EndsWith("\\") == true)
					m_ctrlPrevious.Text = m_tmaxAlias.Previous.Substring(0, m_tmaxAlias.Previous.Length - 1);
				else
					m_ctrlPrevious.Text = m_tmaxAlias.Previous;
			
			}
			else
			{
				m_ctrlUserPath.Text = "";
				m_ctrlPrevious.Text = "";
				m_ctrlOriginal.Text = "";
			}
				
			return true;
			
		}// private bool SetAlias(CTmaxAlias tmaxAlias)
		
		/// <summary>This method is called to refresh the list item associated with the specified alias</summary>
		private void RefreshListItem(CTmaxAlias tmaxAlias)
		{
			ListViewItem lvItem = null;

			if((lvItem = GetListItem(tmaxAlias)) != null)
			{
				SetListItemProps(lvItem, tmaxAlias, false);

				SuspendLayout();
				m_ctrlAliases.Columns[1].Width = -2;
				m_ctrlAliases.Columns[3].Width = -2;
				ResumeLayout();
			}
			
			m_ctrlUndo.Enabled = ((tmaxAlias != null) && (tmaxAlias.Modified == true));
			m_ctrlUndoAll.Enabled = ((tmaxAlias != null) && (m_tmaxAliases.IsModifed() == true));
			
		}// private void RefreshListItem(CTmaxAlias tmaxAlias)
			
		/// <summary>This method is called to undo the existing modification</summary>
		private void Undo(CTmaxAlias tmaxAlias)
		{
			if(tmaxAlias.Modified == true)
			{
				tmaxAlias.Modified = false;
				tmaxAlias.Editor = "";
				RefreshListItem(tmaxAlias);
				
				//	Make sure the other controls get updated if this is 
				//	the active alias
				if(ReferenceEquals(tmaxAlias, m_tmaxAlias) == true)
					SetAlias(tmaxAlias, true);
			}
			
		}// private void Undo(CTmaxAlias tmaxAlias)
			
		/// <summary>This method is called to determine if it is OK to change the active alias</summary>
		/// <returns>true if OK to continue</returns>
		private bool SetAliasPath(bool bSilent)
		{
			string	strCurrent = "";
			string	strUser = "";
			string	strMsg = "";
			
			//	Do we have an active alias?
			if(m_tmaxAlias == null) return true;
			
			//	Is the user clearing the current value?
			if(m_ctrlUserPath.Text.Length == 0)
			{
				Undo(m_tmaxAlias);
				return true;
			}
			
			//	Get the value being set by the user
			if(CTmaxAlias.PathToAlias(m_ctrlUserPath.Text, ref strUser) == false)
			{
				if(bSilent == false)
				{
					strMsg = (strUser + " is not a valid drive or UNC path");
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				else
				{
					//	Silent means we just ignore the error
					return true;
				}
			}
				
			//	Get the current value assigned to this alias
			if(m_tmaxAlias.Modified == true)
				CTmaxAlias.PathToAlias(m_tmaxAlias.Editor, ref strCurrent);
			else
				CTmaxAlias.PathToAlias(m_tmaxAlias.Current, ref strCurrent);
				
			//	Has the value changed?
			if(String.Compare(strUser, strCurrent, true) == 0) return true;
			
			//	Should we confirm that the folder exists
			if(bSilent == false)
			{
				if(System.IO.Directory.Exists(strUser) == false)
				{
					strMsg = (strUser + " does not exist on this machine.\n\nDo you want to continue?");
					switch(MessageBox.Show(strMsg, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
						case DialogResult.Yes:
						
							break;
							
						case DialogResult.No:
						
							return false;
							
						case DialogResult.Cancel:
						
							return true;
					}
					
				}
				
			}
			
			//	Update the alias
			m_tmaxAlias.Editor = strUser;
			m_tmaxAlias.Modified = true;
			
			//	Update the list box
			RefreshListItem(m_tmaxAlias);

			return true;
			
		}// private bool SetAliasPath(bool bSilent)
			
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			if(m_tmaxCaseOptions != null)
			{
				//	Attempt to lock the aliases for editing
				m_tmaxAliases = m_tmaxCaseOptions.GetAliases(true, m_tmaxCaseOptions.AllowEditAliases);
				
				//	If the lock failed try getting it as read only
				if(m_tmaxAliases == null)
				{
					m_tmaxAliases = m_tmaxCaseOptions.Aliases;
					m_bReadOnly = true;
				}
				
			}
			
			if((m_tmaxAliases != null) && (m_tmaxAliases.Count > 0))
			{
				Cursor.Current = Cursors.WaitCursor;
				
				Fill();
				
				Cursor.Current = Cursors.Default;
			}
			else
			{
				SetAlias(null, false);
			}
		
		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>Traps the event fired when the user selects an alias in the list box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		private void OnSelChanged(object sender, System.EventArgs e)
		{
			if(m_bIgnoreEvents == false)
			{
				//	Make this the active alias
				if(m_ctrlAliases.SelectedItems.Count > 0)
				{
					//	Are we restoring the previous selection?
					if(m_bRestoringSelection == true)
					{
						//	MUST call this again to put the selection back to where we
						//	want it to be (don't understand this event chain)
						SetListSelection(m_tmaxAlias);
						
						m_bRestoringSelection = false;
						return;
					}
					
					//	Are we automatically applying changes when the
					//	selection changes?
					if(m_bSetOnSelect == true)
					{
						//	Is it ok to change the active link
						if(SetAliasPath(false) == true)
						{
							SetAlias((CTmaxAlias)(m_ctrlAliases.SelectedItems[0].Tag), true);
						}
						else
						{
							m_bRestoringSelection = true;
							SetListSelection(m_tmaxAlias);
						}
					}
					else
					{					
						SetAlias((CTmaxAlias)(m_ctrlAliases.SelectedItems[0].Tag), true);
					}
					
				}
				else if(m_ctrlAliases.Items.Count == 0)
				{
					SetAlias(null, false);
				}
				
			}
			
		}// protected void OnSelChanged(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}		
		
		/// <summary>Case options containing the case paths to be configured</summary>
		public FTI.Shared.Trialmax.CTmaxCaseOptions CaseOptions
		{
			get	{ return m_tmaxCaseOptions; }
			set	{ m_tmaxCaseOptions = value; }
		}		
		
		/// <summary>True if values have been modified</summary>
		public bool Modified
		{
			get	{ return m_bModified; }
		}
				
		#endregion Properties
	
	}// public class CTmaxAliasesCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
