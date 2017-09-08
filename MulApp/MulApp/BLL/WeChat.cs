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

        public static string CmdMess(string xmlstr)
        {

            try
            {
                Models.WeChat.WeChatXml wechatxml = new Models.WeChat.WeChatXml(xmlstr);

                switch (wechatxml["MsgType"])
                {
                    case "text":
                        wechatxml["Content"] = "rectest:" + wechatxml["Content"];
                        break;

                    case "event":

                        switch (wechatxml["Event"])
                        {
                            case "CLICK":
                                switch (wechatxml["EventKey"])
                                {
                                    case "TOSION_BUTTON_1_0":
                                        wechatxml["MsgType"] = "text";
                                        wechatxml["Content"] = "buttonclick";
                                        break;
                                }
                                break;
                            default:
                                return "";
                        }
                        break;

                    default:
                        return "";

                }


                return wechatxml.XmlStr();
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
                return "";
            }

        }
    }
}