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
            System.Xml.XmlDocument wechatxml = null;

            public string XmlStr()
            {
                if (wechatxml == null)
                    return "";

                return wechatxml.InnerXml;
            }

            public string this[string snode]
            {
                get 
                {
                    if (wechatxml == null)
                        return "";

                    System.Xml.XmlNode xmlnode = wechatxml.DocumentElement.SelectSingleNode(snode);

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
                        if (wechatxml == null)
                            return;

                        System.Xml.XmlNode xmlnode = wechatxml.DocumentElement.SelectSingleNode(snode);

                        if (xmlnode == null)
                        {
                            xmlnode = wechatxml.CreateElement(snode);
                            wechatxml.DocumentElement.AppendChild(xmlnode);
                        }

                        xmlnode.InnerText = value;

                    }
                    catch (Exception ex)
                    {
                        BLL.GlfFun.AddLog(ex);
                    }
                }
            }

            
            public WeChatXml(string xmlstr)
            {
                try
                {
                    if (xmlstr == "")
                        return;

                    wechatxml = new System.Xml.XmlDocument();
                    wechatxml.LoadXml(xmlstr);

                    if (wechatxml.DocumentElement == null)
                        wechatxml.CreateElement("xml");

                    string str = this["FromUserName"];
                    this["FromUserName"] = this["ToUserName"];
                    this["ToUserName"] = str;

                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    this["CreateTime"] = Convert.ToInt64(ts.TotalSeconds).ToString();

                    
                }
                catch (Exception ex)
                {
                    wechatxml = null;
                    BLL.GlfFun.AddLog(ex);
                }
                
            }

            
        }

        public class WeChatActoken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}