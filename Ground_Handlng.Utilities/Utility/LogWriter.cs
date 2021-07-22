using Ground_Handlng.Abstractions.Utility;
using System;
using System.IO;

namespace Ground_Handlng.Utilities.Utility
{
    public class LogWriter //: ILogWriter
    {
        public void CreateLog(string strLogText, string strModuleName, string logName)
        {
            string strLogFileName = null;

            try
            {
                string strLOGDIR = @"C:\AdminLTETemplate\Log\";
                if (!Directory.Exists(strLOGDIR))
                {
                    Directory.CreateDirectory(strLOGDIR);
                }
                strLogFileName = strLOGDIR + logName + " " + DateTime.Now.ToString("dd-MMM-yyyy") + ".log";
                StreamWriter fileLogFileWriter = new StreamWriter(strLogFileName, true);
                strLogText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt") + " " + strLogText + strModuleName;
                fileLogFileWriter.WriteLine(strLogText);
                fileLogFileWriter.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
