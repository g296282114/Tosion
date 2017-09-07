using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class Glf
    {
        public class AppConfig
        {
            string _DevName = null;

            public string gDevName{
                get{
                    if(_DevName == null)
                    {
                        Models.DBTable.Config dbc = new DBTable.Config();
                        dbc.gname = "DevName";

                        BLL.DBSql.Select(ref dbc);

                        _DevName = dbc.gvalue;
                    }
                    return _DevName;
                }
                set
                {
                    _DevName = value;

                    Models.DBTable.Config dbc = new DBTable.Config();
                    dbc.gname = "DevName";
                    dbc.gvalue = _DevName;
                    BLL.DBSql.Update(dbc);
                }
                }

        }
    }


}