using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TmaxSetupInsatller
{
    public partial class InstallerForm : Form
    {
        private Process _CRProcess;

        public InstallerForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                btnContinue.Enabled = false;
                btnClose.Enabled = false;
                InstallCrystalReport();

            }
            catch (Exception)
            {
                // do nothing
            }

        }

       

        private void InstallCrystalReport()
        {
            try
            {
                string fileName = Application.StartupPath + "\\" + Config.CRRedist;
                 
                if (!File.Exists(fileName)) return;
                _CRProcess = new Process();
                _CRProcess.StartInfo.FileName = fileName;
                _CRProcess.Start();
                _CRProcess.Exited += new EventHandler(CRprocess_Exited);
                _CRProcess.EnableRaisingEvents = true;

               

                

            }
            catch (Exception)
            {
                
                // do nothing
            }
            
        }

        private void InstallWMEncoder()
        {
            try
            {
                string fileName = Application.StartupPath + "\\" + Config.WMEncoder;

                
                if (!File.Exists(fileName)) return;
                _CRProcess = new Process();
                _CRProcess.StartInfo.FileName = fileName;
                _CRProcess.Start();
                _CRProcess.Exited += new EventHandler(WMEncoderProcess_Exited);
                _CRProcess.EnableRaisingEvents = true;

            }
            catch (Exception)
            {

                // do nothing
            }

        }

        private void InstallKLiteCodec()
        {
            try
            {
                string fileName = Application.StartupPath + "\\" + Config.KLiteCodec;

                if (!File.Exists(fileName)) return;
                _CRProcess = new Process();
                _CRProcess.StartInfo.FileName = fileName;
                _CRProcess.Start();
                _CRProcess.Exited += new EventHandler(_KLiteCodecProcess_Exited);
                _CRProcess.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            { 
                // do nothing
            }
        }

        void _KLiteCodecProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                if (_CRProcess != null)
                {
                    _CRProcess.Close();
                }
                SetControlPropertyValue(pbKLiteCodec, "Visible", true);                
                InstallationCompleted();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // Do nothing
            }
        }

        void CRprocess_Exited(object sender, EventArgs e)
        {
            try
            {
                if (_CRProcess != null)
                {
                    _CRProcess.Close();
                }
                SetControlPropertyValue(pbCRE, "Visible", true);
                InstallWMEncoder();
                
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
                // Do nothing
            }
            
        }
        void WMEncoderProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                if (_CRProcess != null)
                {
                    _CRProcess.Close();
                }
                SetControlPropertyValue(pbWMEncoder, "Visible", true);
                InstallKLiteCodec();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // Do nothing
            }

        }

       

        private void InstallationCompleted()
        {
            
            SetControlPropertyValue(pbWMEncoder, "Visible", true);
            SetControlPropertyValue(btnClose, "Enabled", true);
            SetControlPropertyValue(btnClose, "Text", "Close");
            SetControlPropertyValue(lblMessage, "Text", "Following components have been installed successfully");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

    }
}
