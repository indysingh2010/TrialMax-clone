namespace TmaxSetupInsatller
{
    partial class InstallerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbWMEncoder = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbCRE = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWMEncoder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCRE)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(10, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(411, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "TrialMax 7 setup needs to install following other components. Press Install to co" +
                "ntinue.";
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(254, 172);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Install components for TrialMax 7";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(337, 172);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbWMEncoder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pbCRE);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 125);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Components";
            // 
            // pbWMEncoder
            // 
            this.pbWMEncoder.BackColor = System.Drawing.Color.Transparent;
            this.pbWMEncoder.Image = global::TmaxSetupInsatller.Properties.Resources.icon_green_check;
            this.pbWMEncoder.Location = new System.Drawing.Point(19, 61);
            this.pbWMEncoder.Name = "pbWMEncoder";
            this.pbWMEncoder.Size = new System.Drawing.Size(16, 16);
            this.pbWMEncoder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbWMEncoder.TabIndex = 6;
            this.pbWMEncoder.TabStop = false;
            this.pbWMEncoder.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Windows Media Encoder";
            // 
            // pbCRE
            // 
            this.pbCRE.BackColor = System.Drawing.Color.Transparent;
            this.pbCRE.Image = global::TmaxSetupInsatller.Properties.Resources.icon_green_check;
            this.pbCRE.Location = new System.Drawing.Point(19, 29);
            this.pbCRE.Name = "pbCRE";
            this.pbCRE.Size = new System.Drawing.Size(16, 16);
            this.pbCRE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbCRE.TabIndex = 4;
            this.pbCRE.TabStop = false;
            this.pbCRE.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Crystal Reports Engine";
            // 
            // InstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(426, 214);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallerForm";
            this.Text = "Install components for TrialMax 7";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWMEncoder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCRE)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbCRE;
        private System.Windows.Forms.PictureBox pbWMEncoder;
        private System.Windows.Forms.Label label2;
    }
}