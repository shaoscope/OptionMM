using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Data;

namespace OptionMM
{
    class TSBase
    {
        public string TSName = "";
        public bool bRun = false;
        public virtual void Run()
        {

        }
        public virtual void Stop()
        {

        }

        public TSBase(string Name)
        {
            TSName = Name;
            TSInit();
        }

        protected virtual void TSInit()
        {
        }

        protected virtual void InitFromXML(string strFile)
        {
        }
    }
}
