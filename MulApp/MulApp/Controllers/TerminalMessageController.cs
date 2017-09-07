using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MulApp.Controllers
{
    public class TerminalMessageController : AsyncController
    {
        public void RecAsync()
        {
            AsyncManager.OutstandingOperations.Increment();
            BLL.TerminalMessage.Notify += TerminalMessage_Notify;
        }

        public string RecCompleted(string sdata)
        {
            return sdata;
        }

        private void TerminalMessage_Notify(object sender, BLL.TerminalMessage.TerminalEventArgs arg)
        {
            BLL.TerminalMessage.Notify -= TerminalMessage_Notify;
            AsyncManager.Parameters["sdata"] = arg.content;
            AsyncManager.OutstandingOperations.Decrement();
        }

        public void Test()
        {
            BLL.TerminalMessage.SendData("{}");
        }

    }
}
