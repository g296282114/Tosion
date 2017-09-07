using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Newtonsoft.Json;

namespace MulApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Main()
        {
            ViewData["username"] = "message";
            return View();
        }

        public string MenuJson()
        {
            return JsonConvert.SerializeObject(BLL.GlfVar.MenuJson);
        }

        public string Test()
        {

            Models.DBTable.Config c = new Models.DBTable.Config();
            c.gname = "AA";
            c.gvalue = "KK";
            BLL.DBSql.Update<Models.DBTable.Config>(c);

            return JsonConvert.SerializeObject(c);
        }

    }
}
