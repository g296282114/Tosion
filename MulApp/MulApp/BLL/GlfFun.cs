using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MulApp.BLL
{
    public class GlfFun
    {

        public static DateTime Now()
        {
            return DateTime.Now.AddSeconds(28800 - TimeZoneInfo.Local.BaseUtcOffset.TotalSeconds);
        }

        public static void AddLog(Exception ex)
        {
            
            //var st = new System.Diagnostics.StackTrace();
            //System.Reflection.MethodBase mb = st.GetFrame(1).GetMethod();
            //string str = Now().ToString() + "  " + ex.StackTrace + " " + mb.ReflectedType.FullName + "." + mb.Name + " (" + ex.StackTrace + " )" + "----->" + System.Web.HttpUtility.HtmlEncode(ex.Message);


            string str = Now().ToString() + "  " + ex.StackTrace + "  " + ex.Message;

            while (GlfVar.lstErr.Count > 2000)
            {
                GlfVar.lstErr.RemoveAt(0);
            }
            GlfVar.lstErr.Add(System.Web.HttpUtility.HtmlEncode(str)); 
        }

        public static void AddLog(string sdata)
        {
            var st = new System.Diagnostics.StackTrace();
            System.Reflection.MethodBase mb = st.GetFrame(1).GetMethod();
            string str = Now().ToString() + " " + mb.ReflectedType.FullName + "." + mb.Name + "----->" + System.Web.HttpUtility.HtmlEncode(sdata);

            if (GlfVar.lstLog.Count > 2000)
            {
                GlfVar.lstLog.RemoveAt(0);
            }
            GlfVar.lstLog.Add(str); 
        }

        public static string GetPostStr(HttpRequestBase hrb)
        {
            System.IO.Stream s = hrb.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            string rstr = System.Text.Encoding.UTF8.GetString(b);
            return rstr;
        }


        //public static void SaveStrToFile(string Path, string sdata)
        //{
        //    try
        //    {
        //        string sfilepath = HttpContext.Current.Server.MapPath("/") + Path;
        //        AddLog(sfilepath);
        //        StreamWriter sr = new StreamWriter(sfilepath, false, System.Text.Encoding.GetEncoding("utf-8"));

        //        sr.Write(sdata);
        //        sr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        AddLog(ex);
        //    }
        //}

        public static string ReadStrFromFile(string Path)
        {
            try
            {
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("/") + Path, System.Text.Encoding.GetEncoding("utf-8"));
                string content = sr.ReadToEnd().ToString();
                sr.Close();
                return content;
            }
            catch (Exception ex)
            {
                AddLog(ex);
                return "";
            }
        }

        public static string SendPostStr(string url, string postString)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postString);
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            //httpWebRequest.ContentType = "text/xml";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            // httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(data);
            //strJson为json字符串 
            System.IO.Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();
            var response = httpWebRequest.GetResponse();
            System.IO.Stream streamResponse = response.GetResponseStream();
            System.IO.StreamReader streamRead = new System.IO.StreamReader(streamResponse);
            String responseString = streamRead.ReadToEnd();

            AddLog("SendPostStr " + url + "  " + postString);
            
            AddLog(responseString);

            return responseString;
        }

    }
}