namespace FTI.Trialmax.Forms
{
    partial class CFDesignationsOverlap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.m_ctrlOverlap = new System.Windows.Forms.ListView();
            this.I = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BARCODE1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BARCODE2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SEGMENT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PAGE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LINE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.START = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.STOP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TEXT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.m_ctrlClose = new System.Windows.Forms.Button();
            this.m_ctrlSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_ctrlOverlap
            // 
            this.m_ctrlOverlap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlOverlap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.I,
            this.BARCODE1,
            this.BARCODE2,
            this.SEGMENT,
            this.PL,
            this.PAGE,
            this.LINE,
            this.QA,
            this.START,
            this.STOP,
            this.TEXT});
            this.m_ctrlOverlap.FullRowSelect = true;
            this.m_ctrlOverlap.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_ctrlOverlap.HideSelection = false;
            this.m_ctrlOverlap.LabelWrap = false;
            this.m_ctrlOverlap.Location = new System.Drawing.Point(12, 41);
            this.m_ctrlOverlap.MultiSelect = false;
            this.m_ctrlOverlap.Name = "m_ctrlOverlap";
            this.m_ctrlOverlap.Size = new System.Drawing.Size(734, 331);
            this.m_ctrlOverlap.TabIndex = 13;
            this.m_ctrlOverlap.UseCompatibleStateImageBehavior = false;
            this.m_ctrlOverlap.View = System.Windows.Forms.View.Details;
            // 
            // I
            // 
            this.I.Text = "";
            this.I.Width = 25;
            // 
            // BARCODE1
            // 
            this.BARCODE1.Text = "Barcode";
            // 
            // BARCODE2
            // 
            this.BARCODE2.Text = "Barcode";
            // 
            // SEGMENT
            // 
            this.SEGMENT.Text = "Segment";
            // 
            // PL
            // 
            this.PL.Text = "PL";
            this.PL.Width = 50;
            // 
            // PAGE
            // 
            this.PAGE.Text = "Page";
            this.PAGE.Width = 40;
            // 
            // LINE
            // 
            this.LINE.Text = "Line";
            this.LINE.Width = 40;
            // 
            // QA
            // 
            this.QA.Text = "QA";
            this.QA.Width = 30;
            // 
            // START
            // 
            this.START.Text = "Start";
            this.START.Width = 50;
            // 
            // STOP
            // 
            this.STOP.Text = "Stop";
            this.STOP.Width = 50;
            // 
            // TEXT
            // 
            this.TEXT.Text = "Text";
            this.TEXT.Width = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Overlapping Designations Summary";
            // 
            // m_ctrlClose
            // 
            this.m_ctrlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlClose.Location = new System.Drawing.Point(671, 378);
            this.m_ctrlClose.Name = "m_ctrlClose";
            this.m_ctrlClose.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlClose.TabIndex = 15;
            this.m_ctrlClose.Text = "&Close";
            this.m_ctrlClose.UseVisualStyleBackColor = true;
            this.m_ctrlClose.Click += new System.EventHandler(this.m_ctrlClose_Click);
            // 
            // m_ctrlSave
            // 
            this.m_ctrlSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSave.Location = new System.Drawing.Point(590, 378);
            this.m_ctrlSave.Name = "m_ctrlSave";
            this.m_ctrlSave.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlSave.TabIndex = 16;
            this.m_ctrlSave.Text = "&Save";
            this.m_ctrlSave.UseVisualStyleBackColor = true;
            this.m_ctrlSave.Click += new System.EventHandler(this.m_ctrlSave_Click);
            // 
            // CFDesignationsOverlap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 413);
            this.Controls.Add(this.m_ctrlSave);
            this.Controls.Add(this.m_ctrlClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_ctrlOverlap);
            this.Name = "CFDesignationsOverlap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Designations Overlap";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView m_ctrlOverlap;
        private System.Windows.Forms.ColumnHeader I;
        private System.Windows.Forms.ColumnHeader BARCODE1;
        private System.Windows.Forms.ColumnHeader SEGMENT;
        private System.Windows.Forms.ColumnHeader BARCODE2;
        private System.Windows.Forms.ColumnHeader PL;
        private System.Windows.Forms.ColumnHeader PAGE;
        private System.Windows.Forms.ColumnHeader LINE;
        private System.Windows.Forms.ColumnHeader QA;
        private System.Windows.Forms.ColumnHeader START;
        private System.Windows.Forms.ColumnHeader STOP;
        private System.Windows.Forms.ColumnHeader TEXT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_ctrlClose;
        private System.Windows.Forms.Button m_ctrlSave;
    }
}