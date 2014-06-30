using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;

namespace OptionMM
{
	/// <summary>
	/// LOG�ļ���¼������Ŀ¼��LOG��Ŀ¼�£���������ΪLOG����Ŀ¼��LOG�ļ�����������Ŀ¼�¡�
	/// �����ڸ��£���Ҫ�����µ�������Ŀ¼ʱ������Ƿ���Ҫɾ���ϵļ�¼�ļ���
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
			//��õ���ǰ����·�������LOG�ļ��У����ļ�����ӣ��γ�ȫ·������
			string sRet = null;
            sRet = FullPath.GetRunPath();
			string sDate = DateTime.Today.ToString("yyyyMMdd");
			sRet = sRet + @"\LOG\"+sDate;
            object commandLock = new object();
            lock (commandLock)//������߳�ͬʱ����
            {
                if (!Directory.Exists(sRet))
                {
                    //bCreateDirectory = true;
                    Directory.CreateDirectory(sRet);
                    //�����ټ���Ƿ���Ҫɾ���ļ���
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
            
            lock (commandLock)//������߳�ͬʱ����
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
