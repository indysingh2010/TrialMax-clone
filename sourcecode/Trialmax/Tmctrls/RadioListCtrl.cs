using System;
using System.Windows.Forms;

namespace FTI.Trialmax.Controls
{
	/// <summary>
	/// This class is a derived version of the CheckedListBox class. It
	/// makes the check boxes act as radio buttons instead of multiple selections
	/// </summary>
	public class CRadioListCtrl : System.Windows.Forms.CheckedListBox
	{
		#region Protected Members
			
		/// <summary>Local member bound to SingleSelection property</summary>
		protected bool m_bSingleSelection = true;
		
		#endregion Protected Members
			
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CRadioListCtrl()
		{
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// Overloaded version of this method to make check boxes behave like radio buttons
		/// </summary>
		/// <param name="e">System event argument object</param>
		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			if(m_bSingleSelection == true)
			{
				//	Are we checking a new item?
				if(e.NewValue == CheckState.Checked)
				{
					//	Uncheck all existing checked items
					foreach(int i in this.CheckedIndices)
					{
						if(i != e.Index)
							SetItemCheckState(i, CheckState.Unchecked);
					}
				
				}
			}
			base.OnItemCheck(e);
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>This property is used to assign a value to the item</summary>
		public bool SingleSelection
		{
			get
			{
				return m_bSingleSelection;
			}
			set
			{
				m_bSingleSelection = value;
			}
			
		}// SingleSelection property
			
		#endregion Properties
		
	
	}//	CRadioListCrl

	/// <summary>
	/// Objects of this class are used to populate an arraylist that
	/// can be assigned as the data source for the radio list control
	/// </summary>
	public class CRadioListItem
	{
		#region Protected Members
			
		/// <summary>Local member bound to Value property</summary>
		protected object m_oValue = null;
		
		/// <summary>Local member associated with the Text property</summary>
		protected string m_strText = "";
			
		/// <summary>Local member associated with the Selected property</summary>
		protected bool m_bSelected = false;
			
		#endregion Protected Members
			
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CRadioListItem()
		{
		}
		
		/// <summary>
		/// Overloaded constructor
		/// </summary>
		/// <param name="strText">The value assigned to the Text property</param>
		public CRadioListItem(string strText)
		{
			m_strText = strText;
		}
		
		/// <summary>
		/// Overloaded constructor
		/// </summary>
		/// <param name="oValue">The object assigned to the Value property</param>
		/// <param name="strText">The value assigned to the Text property</param>
		public CRadioListItem(object oValue, string strText)
		{
			m_oValue = oValue;
			m_strText = strText;
		}
		
		/// <summary>
		/// Overloaded member to get the object as a string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return m_strText;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// This method is called when the value of the Value property changes
		/// </summary>
		protected virtual void OnValueChanged()
		{
		}
		
		/// <summary>
		/// This method is called when the value of the Selected property changes
		/// </summary>
		protected virtual void OnSelectedChanged()
		{
		}
		
		/// <summary>
		/// This method is called when the value of the Text property changes
		/// </summary>
		protected virtual void OnTextChanged()
		{
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>This property is used to set the text displayed in the list box</summary>
		public string Text
		{
			get
			{
				return m_strText;
			}
			set
			{
				m_strText = value;
				
				//	Notify the derived class
				OnTextChanged();
			}
			
		}// Text property
			
		/// <summary>This property is used to assign a value to the item</summary>
		public object Value
		{
			get
			{
				return m_oValue;
			}
			set
			{
				m_oValue = value;
				
				//	Notify the derived class
				OnValueChanged();
			}
			
		}// Value property
			
		/// <summary>This property is used to assign a value to the item</summary>
		public bool Selected
		{
			get
			{
				return m_bSelected;
			}
			set
			{
				m_bSelected = value;
				
				//	Notify the derived class
				OnSelectedChanged();
			}
			
		}// Selected property
			
		#endregion Properties
		
	}//	CRadioListItem

}// namespace FTI.Trialmax.Controls
