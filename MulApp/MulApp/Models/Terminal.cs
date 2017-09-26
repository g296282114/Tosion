using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class Terminal
    {
        public abstract class CmdTerminal
        {
            public abstract void CmdData(ref TSocketData socketSend, TSocketData socketRec);

        }

        public class TFacePassData
        {
            public int state { get; set; }
            public string msg { get; set; }
            public string data { get; set; }
            
        }

        public class TSocketData
        {
            public TSocketData()
            {
                fmodule = "";
                tmodule = "";
                fip = "";
                tip = "";
                cmd = "";
                data = "";
            }

            public string fmodule { get; set; }
            public string tmodule { get; set; }
            public string fip { get; set; }
            public string tip { get; set; }
            public string cmd { get; set; }
            public string data { get; set; }
        }
    }
}