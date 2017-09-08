using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MulApp.Controllers
{
    public class GlfController : Controller
    {
        //
        // GET: /Glf/

        public string Test()
        {
            Models.WeChat.WeChatXml xml = new Models.WeChat.WeChatXml("<xml></xml>");
            xml["a"] = "sdfsdf";

            return xml.XmlStr();
  
        }

        public string Now()
        {
            return BLL.GlfFun.Now().ToString();
        }

        public string Log()
        {
            string rstr = "";

            foreach (string str in BLL.GlfVar.lstLog)
            {
                rstr += str + "<br/>";
            }

            return rstr;
        }

        public string Err()
        {
            string rstr = "";

            foreach (string str in BLL.GlfVar.lstErr)
            {
                rstr += str + "<br/>";
            }

            return rstr;
        }


    }
}
