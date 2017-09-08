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

        public class WeChatXml
        {
            public System.Xml.XmlDocument wechatxml;
            System.Xml.XmlElement rrootElement;

            public string this[string snode]
            {
                get 
                {
                    System.Xml.XmlNode xmlnode = rrootElement.SelectSingleNode(snode);

                    if (xmlnode == null)
                    {
                        return "";
                    }

                    return xmlnode.InnerText;
                }
                set 
                {
                    try
                    {
                        System.Xml.XmlNode xmlnode = rrootElement.SelectSingleNode(snode);

                        if (xmlnode == null)
                        {
                            wechatxml.CreateElement(snode);

                            xmlnode = rrootElement.SelectSingleNode(snode);
                        }

                        xmlnode.InnerText = value;

                    }
                    catch (Exception ex)
                    {
                        BLL.GlfFun.AddLog(ex);
                    }
                }
            }

            
            public WeChatXml(System.Xml.XmlDocument xml)
            {
                wechatxml = xml;
                rrootElement = wechatxml.DocumentElement;

                string str = this["FromUserName"];
                this["FromUserName"] = this["ToUserName"];
                this["ToUserName"] = str;

                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                this["CreateTime"] = Convert.ToInt64(ts.TotalSeconds).ToString();
            }

            
        }

        public class WeChatActoken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}