using System;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class defines the options for validating a TrialMax database</summary>
	public class CTmaxValidateOptions
	{
		#region Private Members
		
		/// <summary>Local member bound to FileValidations property</summary>
		private CTmaxOptions m_aValidations = new CTmaxOptions();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxValidateOptions()
		{
			m_aValidations.Multiple = true;
			m_aValidations.Add(TmaxDatabaseValidations.Document, "Documents", true);
			m_aValidations.Add(TmaxDatabaseValidations.PowerPoint, "PowerPoint Presentations", true);
			m_aValidations.Add(TmaxDatabaseValidations.Recording, "Recordings", true);
			m_aValidations.Add(TmaxDatabaseValidations.Transcripts, "Deposition Transcripts", true);
			m_aValidations.Add(TmaxDatabaseValidations.Video, "Deposition Video", true);
			m_aValidations.Add(TmaxDatabaseValidations.Scripts, "Scripts", true);
			m_aValidations.Add(TmaxDatabaseValidations.Binders, "Binders", true);
			m_aValidations.Add(TmaxDatabaseValidations.BarcodeMap, "Barcode Map", true);
			m_aValidations.Add(TmaxDatabaseValidations.TransferCodes, "Transfer Codes", false);
			m_aValidations.Add(TmaxDatabaseValidations.CreateDesignations, "Create Missing Designations", false);
		}
		
		/// <summary>This method is called to retrieve the option object associatied with the specified FileValidation enumeration</summary>
		/// <param name="eValidation">The desired validation option</param>
		/// <returns>The associated CTmaxOption object</returns>
		public CTmaxOption GetTmaxOption(TmaxDatabaseValidations eValidation)
		{
			if(m_aValidations != null)
			{
				foreach(CTmaxOption O in m_aValidations)
				{
					if(((TmaxDatabaseValidations)O.Value) == eValidation)
						return O;
				}
			}

			return null;
		
		}// public CTmaxOption GetTmaxOption(TmaxDatabaseValidations eValidation)
		
		/// <summary>This method is called to determine if the specified file validation is checked</summary>
		/// <param name="eValidation">The validation option enumeration</param>
		/// <returns>true if checked (selected)</returns>
		public bool GetChecked(TmaxDatabaseValidations eValidation)
		{
			CTmaxOption O = null;
			
			try
			{
				if((O = GetTmaxOption(eValidation)) != null)
					return O.Selected;
			}
			catch
			{
			}

			return false;
		
		}// public bool GetChecked(TmaxDatabaseValidations eValidation)
		
		/// <summary>This method is called to set the checked state of the specified validation option</summary>
		/// <param name="eValidation">The validation option enumeration</param>
		/// <param name="bChecked">true if checked</param>
		public void SetChecked(TmaxDatabaseValidations eValidation, bool bChecked)
		{
			CTmaxOption O = null;
			
			try
			{
				if((O = GetTmaxOption(eValidation)) != null)
					O.Selected = bChecked;
			}
			catch
			{
			}
		
		}// public void SetChecked(TmaxDatabaseValidations eValidation, bool bChecked)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>File Validation Options</summary>
		public CTmaxOptions FileValidations
		{
			get { return m_aValidations; }
		}
		
		/// <summary>True to validate document files</summary>
		public bool Documents
		{
			get { return GetChecked(TmaxDatabaseValidations.Document); }
			set { SetChecked(TmaxDatabaseValidations.Document, value); }
		}
		
		/// <summary>True to validate PowerPoint files</summary>
		public bool PowerPoints
		{
			get { return GetChecked(TmaxDatabaseValidations.PowerPoint); }
			set { SetChecked(TmaxDatabaseValidations.PowerPoint, value); }
		}
		
		/// <summary>True to validate Recording files</summary>
		public bool Recordings
		{
			get { return GetChecked(TmaxDatabaseValidations.Recording); }
			set { SetChecked(TmaxDatabaseValidations.Recording, value); }
		}
		
		/// <summary>True to validate Transcript files</summary>
		public bool Transcripts
		{
			get { return GetChecked(TmaxDatabaseValidations.Transcripts); }
			set { SetChecked(TmaxDatabaseValidations.Transcripts, value); }
		}
		
		/// <summary>True to validate Deposition Video files</summary>
		public bool VideoFiles
		{
			get { return GetChecked(TmaxDatabaseValidations.Video); }
			set { SetChecked(TmaxDatabaseValidations.Video, value); }
		}
		
		/// <summary>True to validate Script files</summary>
		public bool Scripts
		{
			get { return GetChecked(TmaxDatabaseValidations.Scripts); }
			set { SetChecked(TmaxDatabaseValidations.Scripts, value); }
		}
		
		/// <summary>True to validate Binder source references</summary>
		public bool Binders
		{
			get { return GetChecked(TmaxDatabaseValidations.Binders); }
			set { SetChecked(TmaxDatabaseValidations.Binders, value); }
		}
		
		/// <summary>True to validate Barcode Map references</summary>
		public bool BarcodeMap
		{
			get { return GetChecked(TmaxDatabaseValidations.BarcodeMap); }
			set { SetChecked(TmaxDatabaseValidations.BarcodeMap, value); }
		}
		
		/// <summary>True to transfer coded properties from 6.1.5 and before</summary>
		public bool TransferCodes
		{
			get { return GetChecked(TmaxDatabaseValidations.TransferCodes); }
			set { SetChecked(TmaxDatabaseValidations.TransferCodes, value); }
		}
		
		/// <summary>True to create missing designations</summary>
		public bool CreateDesignations
		{
			get { return GetChecked(TmaxDatabaseValidations.CreateDesignations); }
			set { SetChecked(TmaxDatabaseValidations.CreateDesignations, value); }
		}
		
		#endregion Properties
	
	}// public class CTmaxValidateOptions

}// namespace FTI.Shared.Trialmax
