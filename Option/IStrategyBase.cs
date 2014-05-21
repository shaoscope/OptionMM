using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    public interface IStrategyBase
    {
        /// <summary>
        /// 策略是否在运行标记
        /// </summary>
        public bool isRunning;
        
        public virtual void Run()
        {

        }
        public virtual void Stop()
        {

        }

    }
}
