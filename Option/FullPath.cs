#define FULLFRAMEWORK

using System;
using System.Reflection;
using System.IO;

namespace OptionMM
{
	/// <summary>
	/// TCPath ��ժҪ˵����
	/// </summary>
	class FullPath
	{
		public static string GetFullPathFileName(string strFile)
		{
			if(strFile==null)
			{
				return null;
			}
			string strRet = null;
			if( strFile.IndexOf(@"\") == -1 )
			{
				//�õ���ǰ·��
				strRet = GetRunPath();
				strRet = strRet + @"\" + strFile;
			}
			else
			{
				strRet = strFile;
			}
			return strRet;
		}

#if FULLFRAMEWORK
		public static string GetRunPath()
		{
			string sRet = null;
			sRet = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			sRet = sRet.Replace(@"file:\","");
			return sRet;
		}
#else
		/// <summary>
		/// ��õ�ǰ���������·��
		/// </summary>
		/// <returns></returns>
		public static string GetRunPath()
		{
			string sRet = null;
			sRet = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			return sRet;
		}
#endif
	}
}
