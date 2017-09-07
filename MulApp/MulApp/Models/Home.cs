using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class Home
    {
        public class MenuJson
        {
            public int id { get; set; }
            public string menuname { get; set; }
            public int parid { get; set; }
            public string menuico { get; set; }
            public string menuurl { get; set; }
            public List<MenuJson> lmt = new List<MenuJson>();
        }
    }
}