using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.BLL
{

    public class Cmd : Models.Terminal.CmdTerminal
    {
        static Cmd g_self = null;

        public static void Test()
        {
            return;
        }

        public static Cmd GetInstance()
        {
            if (g_self == null)
            {
                g_self = new Cmd();
            }
            return g_self;
        }


        public override void CmdData(ref Models.Terminal.TSocketData socketSend, Models.Terminal.TSocketData socketRec)
        {
            switch (socketRec.cmd)
            {
                case "get_name":
                    socketSend.cmd = "return_name";
                    socketSend.data = BLL. GlfVar.AppConfig.gDevName;
                    BLL.Terminal.SendSocket(socketSend);
                    break;
                case "set_name":
                    GlfVar.AppConfig.gDevName = socketRec.data;

                    socketSend.cmd = "return_name";
                    socketSend.data = GlfVar.AppConfig.gDevName;
                    BLL.Terminal.SendSocket(socketSend);
                    break;



            }
        }
    }
}