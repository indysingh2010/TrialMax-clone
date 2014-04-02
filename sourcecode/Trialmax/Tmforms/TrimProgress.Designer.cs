namespace FTI.Trialmax.Forms
{
    partial class CFTrimProgress
    {
        /// <summary>Component container required by form designer</summary>
        private System.ComponentModel.IContainer components;

        /// <summary>The form's cancel button</summary>
        private System.Windows.Forms.Button m_ctrlCancel;

        /// <summary>Status text</summary>
        private System.Windows.Forms.Label m_ctrlStatus;

        /// <summary>Errors messages list view</summary>
        private System.Windows.Forms.ListView m_ctrlErrors;

        /// <summary>Error messages image column</summary>
        private System.Windows.Forms.ColumnHeader I;

        /// <summary>Error messages barcode column</summary>
        private System.Windows.Forms.ColumnHeader BARCODE;

        /// <summary>Error messages description column</summary>
        private System.Windows.Forms.ColumnHeader DESCRIPTION;

        /// <summary>Error messages list label</summary>
        private System.Windows.Forms.Label m_ctrlErrorsLabel;

        /// <summary>Save As pushbutton</summary>
        private System.Windows.Forms.Button m_ctrlSaveAs;

        /// <summary>Image list for errors list view</summary>
        private System.Windows.Forms.ImageList m_ctrlImages;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFTrimProgress));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlSaveAs = new System.Windows.Forms.Button();
			this.m_ctrlStatus = new System.Windows.Forms.Label();
			this.m_ctrlErrors = new System.Windows.Forms.ListView();
			this.I = new System.Windows.Forms.ColumnHeader();
			this.BARCODE = new System.Windows.Forms.ColumnHeader();
			this.DESCRIPTION = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlErrorsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(292, 204);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 10;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlSaveAs
			// 
			this.m_ctrlSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveAs.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlSaveAs.Enabled = false;
			this.m_ctrlSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlSaveAs.Location = new System.Drawing.Point(204, 204);
			this.m_ctrlSaveAs.Name = "m_ctrlSaveAs";
			this.m_ctrlSaveAs.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlSaveAs.TabIndex = 9;
			this.m_ctrlSaveAs.Text = "&Save As";
			this.m_ctrlSaveAs.Click += new System.EventHandler(this.OnClickSaveAs);
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatus.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(368, 23);
			this.m_ctrlStatus.TabIndex = 11;
			this.m_ctrlStatus.Text = "status";
			// 
			// m_ctrlErrors
			// 
			this.m_ctrlErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.I,
            this.BARCODE,
            this.DESCRIPTION});
			this.m_ctrlErrors.FullRowSelect = true;
			this.m_ctrlErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.m_ctrlErrors.HideSelection = false;
			this.m_ctrlErrors.LabelWrap = false;
			this.m_ctrlErrors.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlErrors.MultiSelect = false;
			this.m_ctrlErrors.Name = "m_ctrlErrors";
			this.m_ctrlErrors.Size = new System.Drawing.Size(368, 136);
			this.m_ctrlErrors.SmallImageList = this.m_ctrlImages;
			this.m_ctrlErrors.TabIndex = 12;
			this.m_ctrlErrors.UseCompatibleStateImageBehavior = false;
			this.m_ctrlErrors.View = System.Windows.Forms.View.Details;
			// 
			// I
			// 
			this.I.Text = "";
			this.I.Width = 16;
			// 
			// BARCODE
			// 
			this.BARCODE.Text = "Barcode";
			this.BARCODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// DESCRIPTION
			// 
			this.DESCRIPTION.Text = "Description";
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "fatal_error.bmp");
			this.m_ctrlImages.Images.SetKeyName(1, "warning.bmp");
			// 
			// m_ctrlErrorsLabel
			// 
			this.m_ctrlErrorsLabel.Location = new System.Drawing.Point(8, 44);
			this.m_ctrlErrorsLabel.Name = "m_ctrlErrorsLabel";
			this.m_ctrlErrorsLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlErrorsLabel.TabIndex = 13;
			this.m_ctrlErrorsLabel.Text = "Errors:";
			// 
			// CFTrimProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 233);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlErrorsLabel);
			this.Controls.Add(this.m_ctrlErrors);
			this.Controls.Add(this.m_ctrlStatus);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlSaveAs);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTrimProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Trimming ...";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

        }


        #endregion
    }
}