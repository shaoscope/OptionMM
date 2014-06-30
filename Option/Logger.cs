using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;

namespace OptionMM
{
	/// <summary>
	/// LOG文件记录到运行目录的LOG子目录下，并以日期为LOG的子目录，LOG文件放在日期子目录下。
	/// 当日期更新，需要创建新的日期子目录时，检查是否需要删除老的记录文件。
	/// </summary>
	class Logger
	{
		private static int saveDays = 1;

		public static void SetSaveDays(int iDays)
		{
			saveDays = iDays;
		}

		private static void CheckIFDeleteOldLog()
		{
			DateTime dt = DateTime.Today;
			dt = dt.AddDays(0-saveDays);
			string sOldDate = dt.ToString("yyyyMMdd");
            string sPath = FullPath.GetRunPath();
			sPath = sPath + @"\LOG\" + sOldDate;
			if(Directory.Exists(sPath))
			{
				Directory.Delete(sPath,true);
			}
		}

		private static string GetFullPath(string strFileName)
		{
			//需得到当前运行路径，添加LOG文件夹，把文件名添加，形成全路径返回
			string sRet = null;
            sRet = FullPath.GetRunPath();
			string sDate = DateTime.Today.ToString("yyyyMMdd");
			sRet = sRet + @"\LOG\"+sDate;
            object commandLock = new object();
            lock (commandLock)//避免多线程同时连接
            {
                if (!Directory.Exists(sRet))
                {
                    //bCreateDirectory = true;
                    Directory.CreateDirectory(sRet);
                    //这里再检查是否需要删除文件夹
                    //CheckIFDeleteOldLog();
                }
            }
			sRet = sRet + @"\" + strFileName;
			return sRet;
		}
        static object commandLock = new object();
		public static void AddToLoggerFile(string sFileName, string strLogText)
		{
            if (strLogText == null)
            {
                return;
            }
			string strFilePath = GetFullPath(sFileName);
            
            lock (commandLock)//避免多线程同时连接
            {
                if (!File.Exists(strFilePath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(strFilePath))
                    {
                        sw.WriteLine(strLogText);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(strFilePath))
                    {
                        sw.WriteLine(strLogText);
                    }
                }
            }
		}
	}
}
