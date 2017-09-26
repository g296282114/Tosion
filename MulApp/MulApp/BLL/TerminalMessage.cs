using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace MulApp.BLL
{
    public class TerminalMessage
    {
        public class TerminalEventArgs : EventArgs
        {
            public string content { get; set; }
        }

        public static event EventHandler<TerminalEventArgs> Notify;
        public static Timer recTimer = new Timer(5000);

        static TerminalMessage()
        {
            recTimer.Elapsed += new ElapsedEventHandler(TimerEventFunction);
            recTimer.AutoReset = false;
            recTimer.Enabled = true;
        }

        public static void TimerEventFunction(Object sender, ElapsedEventArgs e)
        {
            recTimer.Enabled = false;
            SendData("{}");
        }

        public static void StartTimer()
        {
            recTimer.Enabled = true;

        }

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