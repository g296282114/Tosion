using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MulApp.Controllers
{
    public class WeChatController : Controller
    {
        //
        // GET: /WeChat/

        public string Index()
        {
            string echoStr = Request.QueryString["echoStr"];
            if (BLL.WeChat.CheckSignature(Request.QueryString["signature"], Request.QueryString["timestamp"], Request.QueryString["nonce"]))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    BLL.GlfFun.AddLog("New wechat:" + echoStr);
                    return echoStr;
                }
            }
            return null;
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostIndex()
        {
            try
            {
                string poststr = "";
                poststr = BLL.GlfFun.GetPostStr(Request);
                System.Xml.XmlDocument msgxml = new System.Xml.XmlDocument();
                msgxml.LoadXml(poststr);

                if (!BLL.WeChat.CmdMess(ref msgxml))
                    return Content("");

                return Content(msgxml.OuterXml);
            }
            catch (Exception e)
            {
                BLL.GlfFun.AddLog(e);
                return Content("");
            }

        }

    }
}
