using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.Models
{
    public class WeChat
    {
        public const string LOCALURL = "https://tosion.apphb.com";
        public const string WECHAT_CUSTOMMESSAGE = "https://api.weixin.qq.com/cgi-bin/message/custom/send";
        public const string WECHAT_ALLMESSAGE = "https://api.weixin.qq.com/cgi-bin/message/mass/send";
        public const string WECHAT_SETBUTTON = " https://api.weixin.qq.com/cgi-bin/menu/create";
        public const string WECHAT_ACTOKEN = "https://api.weixin.qq.com/cgi-bin/token";
        public const string WECHAT_WEBCODE = "https://open.weixin.qq.com/connect/oauth2/authorize";
        public const string WECHAT_CODEACTOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";

        public class WeChatActoken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}