using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Timers;

namespace MulApp.Controllers
{
    public class TerminalController : AsyncController
    {
        public static Boolean Terminal_Con = false;
        [AsyncTimeout(10000)]
        public void RecAsync()
        {
            Terminal_Con = true;
            AsyncManager.OutstandingOperations.Increment();
            BLL.Terminal.Notify += Terminal_Notify;
            BLL.Terminal.StartTimer();
        }

        public string RecCompleted(string sdata)
        {
            Terminal_Con = false;
            return sdata;
        }

        private void Terminal_Notify(object sender, BLL.Terminal.TerminalEventArgs arg)
        {
            BLL.Terminal.Notify -= Terminal_Notify;
            AsyncManager.Parameters["sdata"] = arg.content;
            AsyncManager.OutstandingOperations.Decrement();
        }

        public ViewResult TmlTest(string sip)
        {
            ViewData["sip"] = sip;
            return View();
        }

        public ViewResult TmlServer(string sip)
        {
            ViewData["sip"] = sip;
            return View();
        }

        public ViewResult TmlServerVideo(string sip)
        {
            ViewData["sip"] = sip;
            return View();
        }

        public ViewResult TmlServerCmd(string sip)
        {
            ViewData["sip"] = sip;
            return View();
        }


        public Boolean SendData(string sdata)
        {
            return BLL.Terminal.SendData(sdata);
        }

        [HttpPost]
        public Boolean SendData()
        {
            string sdata = BLL.GlfFun.GetPostStr(Request);
            return BLL.Terminal.SendData(sdata);
        }

        public ViewResult Index()
        {
            if (Terminal_Con)
            {
                ViewData["Terminal_Con"] = "";
            }
            else
            {
                ViewData["Terminal_Con"] = "DisConnect";
            }
            
            return View();
        }

        [HttpPost]
        public string CmdData()
        {
            try
            {
                string str = BLL.GlfFun.GetPostStr(Request);
                BLL.GlfFun.AddLog(str);
                Models.Terminal.TSocketData dcd = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Terminal.TSocketData>(str);
                BLL.Terminal.CmdData(dcd);
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }
            
            return "";
        }

        public void return_videoimg()
        {

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=videoimg.jpg");
            Response.BinaryWrite(BLL.Terminal.gvar_return_videoimg);
            Response.Flush();
            Response.End();
        }

    }
}
