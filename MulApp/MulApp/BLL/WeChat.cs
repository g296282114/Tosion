using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MulApp.BLL
{
    public class WeChat
    {
        const string C_WeChat_Token = "weixin";
        const string C_WeChat_AppId = "wxb9c9c1936b42acb0";
        const string C_WeChat_AppSecret = "41031124d519f5b6ed237e7764e11e4d";


        static string V_AcToken = "";
        static DateTime V_AcTime = BLL.GlfFun.Now();
        static int V_AcExpires = 0;

        public static string GetAcToken()
        {
            try
            {
                if (BLL.GlfFun.Now() >= V_AcTime.AddSeconds(V_AcExpires))
                    V_AcToken = "";

                if (V_AcToken != "")
                    return V_AcToken;

                string surl = Models.WeChat.Cfg.url.ACTOKEN + "?grant_type=client_credential&appid=" + C_WeChat_AppId + "&secret=" + C_WeChat_AppSecret;//Set Actoken
                string sret = BLL.GlfFun.SendPostStr(surl, "");
                Models.WeChat.WeChatActoken wci = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.WeChat.WeChatActoken>(sret);
                BLL.GlfFun.AddLog("New actoken:" + wci.access_token);

                V_AcToken = wci.access_token;
                V_AcTime = BLL.GlfFun.Now();
                V_AcExpires = wci.expires_in;

                return V_AcToken;

            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
                return "";
            }

        }

        public static string RefreshWechatButton()
        {
            string wechatbuttonstr = BLL.GlfFun.ReadStrFromFile("Content\\Json\\WechatButton.js");
            return SendWechat(Models.WeChat.Cfg.url.SETBUTTON, wechatbuttonstr);
        }

        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { C_WeChat_Token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SendWechat(string url, string parm)
        {
            if (url == "")
                return "";

            string actoken = GetAcToken();

            if (actoken == "")
                return "";

            return BLL.GlfFun.SendPostStr(url + "?access_token=" + actoken, parm);

        }

        public static Boolean CmdMess(ref System.Xml.XmlDocument msgxml)
        {

            try
            {
                string str1 = "";
                System.Xml.XmlElement rrootElement = msgxml.DocumentElement;

                System.Xml.XmlNode xml_msgtype = rrootElement.SelectSingleNode("MsgType"); 
                System.Xml.XmlNode xml_content = rrootElement.SelectSingleNode("Content");
                System.Xml.XmlNode xml_event = rrootElement.SelectSingleNode("Event");
                System.Xml.XmlNode xml_eventmsg = rrootElement.SelectSingleNode("EventKey");


                switch (xml_msgtype.InnerText)
                {
                    case "text":
                        xml_content.InnerText = "rec:" + xml_content.InnerText;
                        break;

                    case "event":

                        switch (xml_event.InnerText)
                        {
                            case "CLICK":
                                switch (xml_eventmsg.InnerText)
                                {
                                    case "TOSION_BUTTON_1_0":
                                        xml_msgtype.InnerText = "text";
                                        xml_content.InnerText = "buttonclick";
                                        break;
                                }
                                break;
                            default:
                                return false;
                        }
                        break;

                    default:
                        return false;

                }


                str1 = rrootElement.SelectSingleNode("FromUserName").InnerText;
                rrootElement.SelectSingleNode("FromUserName").InnerText = rrootElement.SelectSingleNode("ToUserName").InnerText;
                rrootElement.SelectSingleNode("ToUserName").InnerText = str1;

                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                rrootElement.SelectSingleNode("CreateTime").InnerText = Convert.ToInt64(ts.TotalSeconds).ToString();

                return true;
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
                return false;
            }

        }
    }
}