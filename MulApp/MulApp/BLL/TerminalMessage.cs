using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.BLL
{
    public class TerminalMessage
    {
        public class TerminalEventArgs : EventArgs
        {
            public string content { get; set; }
        }

        public static event EventHandler<TerminalEventArgs> Notify;

        public static Boolean SendData(string sdata)
        {
            Boolean bresult = false;
            try
            {
                if (Notify == null)
                    System.Threading.Thread.Sleep(1000);

                if (Notify != null)
                {
                    Notify(null, new TerminalEventArgs { content = sdata });
                    bresult = true;
                }

            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }


            return bresult;

        }
    }
}