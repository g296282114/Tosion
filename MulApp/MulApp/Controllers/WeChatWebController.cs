using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MulApp.Controllers
{
    public class WeChatWebController : Controller
    {
        //
        // GET: /WeChatWeb/

        public ActionResult Index()
        {
            return View();
        }

        public string RefreshWechatButton()
        {
            return BLL.WeChat.RefreshWechatButton();
        }

    }
}
