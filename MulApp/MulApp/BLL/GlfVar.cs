using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MulApp.BLL
{
    public class GlfVar
    {

        static GlfVar()
        {
            try
            {
                MenuJson = JsonConvert.DeserializeObject<List<Models.Home.MenuJson>>(BLL.GlfFun.ReadStrFromFile("Content\\Json\\Menu.js"));
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }
        }

        public static List<System.String> lstErr = new List<System.String>();
        public static List<System.String> lstLog = new List<System.String>();
        public static List<Models.Home.MenuJson> MenuJson = null;
        public static Models.Glf.AppConfig AppConfig = new Models.Glf.AppConfig();

    }
}