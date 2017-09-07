using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class DBTable
    {
        public class TableBase
        {
            public string g_key = "g_id";

            public int g_id { get; set; }

            public virtual void InitTable()
            {
            }
        }

        public class Config: TableBase
        {
            public string gname { get; set; }
            public string gvalue { get; set; }

            public Config()
            {
                g_key = "gname";
            }

            public override void InitTable()
            {
                Config tb = new Config();
                tb.gname = "DevName";
                tb.gvalue = "Web";
                BLL.DBSql.Insert<Config>(tb);
            }
        }

        public class Test : TableBase
        {
            public string bb { get; set; }

        }
    }
}