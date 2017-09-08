using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class WeChat
    {


        public class Cfg
        {
            public class url
            {
                public const string LOCALURL = "https://tosion.apphb.com";
                public const string CUSTOMMESSAGE = "https://api.weixin.qq.com/cgi-bin/message/custom/send";
                public const string ALLMESSAGE = "https://api.weixin.qq.com/cgi-bin/message/mass/send";
                public const string SETBUTTON = " https://api.weixin.qq.com/cgi-bin/menu/create";
                public const string ACTOKEN = "https://api.weixin.qq.com/cgi-bin/token";
                public const string WEBCODE = "https://open.weixin.qq.com/connect/oauth2/authorize";
                public const string CODEACTOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";
            }
      
        }

        public class WeChatActoken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}