using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Newtonsoft.Json;
using System.Timers;
using System.IO;

namespace MulApp.BLL
{
    public static class Terminal
    {
        public class TerminalEventArgs : EventArgs
        {
            public string content { get; set; }
        }

        public static byte[] gvar_return_videoimg = null;
        public static event EventHandler<TerminalEventArgs> Notify;

        public static Timer recTimer = new Timer(5000);

        static Terminal()
        {
            recTimer.Elapsed += new ElapsedEventHandler(TimerEventFunction);
            recTimer.AutoReset = false;
            recTimer.Enabled = true;
        }

        public static void TimerEventFunction(Object sender, ElapsedEventArgs e)
        {
            recTimer.Enabled = false;
            SendData("");
        }

        public static void StartTimer()
        {
            recTimer.Enabled = true;
            
        }

        public static Boolean SendSocket(Models.Terminal.TSocketData socket)
        {

            return SendData(JsonConvert.SerializeObject(socket));
        }

        public static Boolean SendData(string sdata)
        {
            Boolean bresult = false;
            try
            {
                if (Notify == null)
                    System.Threading.Thread.Sleep(1000);

                if(Notify != null)
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

        public static void CmdData(Models.Terminal.TSocketData socketRec)
        {
            try
            {
                string smodule = socketRec.tmodule;

                Models.Terminal.TSocketData socketSend = new Models.Terminal.TSocketData();
                socketSend.fmodule = socketRec.tmodule;
                socketSend.tmodule = socketRec.fmodule;
                socketSend.tip = socketRec.fip;

                if (socketRec.tmodule == "")
                {
                    BLL.Cmd.GetInstance().CmdData(ref socketSend, socketRec);
                    return;
                }

                if (socketRec.cmd == "return_videoimg")
                {

                    Models.Terminal.TFacePassData fpd = JsonConvert.DeserializeObject<Models.Terminal.TFacePassData>(socketRec.data);
                    gvar_return_videoimg = Convert.FromBase64String(fpd.imgbase64);  
                    socketRec.data = "/terminal/return_videoimg";

                }

                BLL.TerminalMessage.SendData(JsonConvert.SerializeObject(socketRec));

                //Type moduleType = Type.GetType("MulApp.BLL." + smodule);
                //Models.Terminal.CmdTerminal ct = (Models.Terminal.CmdTerminal)moduleType.InvokeMember("GetInstance", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, new object[] { });
                //ct.CmdData(ref socketSend, socketRec);
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }
            
           

            


        }
 
        
    }
}