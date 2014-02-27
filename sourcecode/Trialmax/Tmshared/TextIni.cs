using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FTI.Shared.Trialmax;
using System.Reflection;


namespace FTI.Shared.Text
{
    public class CTextIni
    {
        public string fileName = string.Empty;
        public void Write(string Line)
        {
            //	Open the file stream				
            FileStream fsSource = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            

            StreamWriter writer = new StreamWriter(fsSource, System.Text.Encoding.Default);
            writer.Write(Line);

            writer.Close();
            fsSource.Close();

        }


        public CTmaxCaseCodes GetCaseCodes()
        {
            CTmaxCaseCodes cTmaxCaseCodes = new CTmaxCaseCodes();
            
            //	Open the file stream				
            FileStream fsSource = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fsSource, System.Text.Encoding.Default);

            int intLineCounter = 0;
            string strLine;
            string strPropertHeader = reader.ReadLine();
            string[] arrProperty = GetPropertyArray(strPropertHeader);

            CTmaxCaseCode cTmaxCaseCode;

            while ((strLine = reader.ReadLine()) != null && (strLine != ""))
            {
                cTmaxCaseCode = GetCaseCode(strLine,arrProperty);
                cTmaxCaseCodes.Add(cTmaxCaseCode);
            }
            return cTmaxCaseCodes;

        }
        public CTmaxCaseCode GetCaseCode(string strLine,string[] arrProperty)
        {
            CTmaxCaseCode cTmaxCaseCode = new CTmaxCaseCode();

            string[] arrCaseCode = strLine.Split('\t');
            Type type;

            for(int i = 0 ; i<arrProperty.Length ;i++)
            foreach (PropertyInfo propertyInfo in cTmaxCaseCode.GetType().GetProperties())
            {
                if(propertyInfo.Name.ToLower() == arrProperty[i].ToLower())
                {
                    object objPropertyValue = new object();

                    if(arrProperty[i].ToLower() == "type")
                        objPropertyValue = (TmaxCodeTypes)(Convert.ToInt32(arrCaseCode[i]));
                    else if (arrProperty[i].ToLower() == "codedproperty")
                        objPropertyValue = (TmaxCodedProperties)(Convert.ToInt32(arrCaseCode[i]));
                    else
                        objPropertyValue = arrCaseCode[i] == "" ? null : arrCaseCode[i];

                    if (objPropertyValue!=null)
                    propertyInfo.SetValue(cTmaxCaseCode, Convert.ChangeType(objPropertyValue, propertyInfo.PropertyType), null);
                }
            }

            return cTmaxCaseCode;

        }
        public string[] GetPropertyArray(string strLine)
        {
            return strLine.Split('\t');
        }


    }


}
