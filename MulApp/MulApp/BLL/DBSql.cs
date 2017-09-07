using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Timers;

namespace MulApp.BLL
{
    public class DBSql
    {
        const string tb_head = "tb_";
        public static SqlConnection _con = null;
        static Timer _timer = new Timer(30000);

        public static SqlConnection g_con
        {
            get
            {
                try
                {
                    if (_con == null)
                    {
                        _con = new SqlConnection();
                        _con.ConnectionString = "Server=1219f07d-efa1-4e8b-9874-a61d0094d912.sqlserver.sequelizer.com;Database=db1219f07defa14e8b9874a61d0094d912;User ID=vjkddngmyeyyefly;Password=ammr6EAoe68jG3ZZUCKGUvKDvXyad8vBmSP7Mdas4kemGe5esfjPJ2mLHQ4ggzjf;";
                        _con.Open();
                    }

                    _timer.Enabled = false;
                    _timer.Enabled = true;

                }
                catch (Exception ex)
                {
                    BLL.GlfFun.AddLog(ex);
                }
                
                return _con;
            }
        }



        static DBSql()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] typeArr = assembly.GetTypes();// assembly.GetTypes();



                foreach (Type tp in typeArr)
                {

                    if (tp.BaseType.Name != "TableBase")
                        continue;

                    object obj = Activator.CreateInstance(tp, true);//根据类型创建实例
                    CreateTable(obj);

                }

                _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimeOut);//到达时间的时候执行事件；
                _timer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                _timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }

        }

        public static void TimeOut(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _con.Close();
                _con = null;
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }

        }



        public static int ExecSql(string sql)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = g_con;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            return com.ExecuteNonQuery();//执行SQL语句
        }

        public static SqlDataReader OpenSql(string sql)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = g_con;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            return com.ExecuteReader();//执行SQL语句
        }

        public static Boolean CreateTable<T>(T tb)
        {
            try
            {
                Type tbtype = tb.GetType();
                PropertyInfo[] tbprops = tbtype.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                string g_key = (tb as Models.DBTable.TableBase).g_key;


                string sql = "";

                string str = "";
                foreach (PropertyInfo pi in tbprops)
                {
                    str = "";
                    if (pi.PropertyType == typeof(string))
                    {
                        if (g_key == pi.Name)
                            str = pi.Name + " nvarchar(120)";
                        else
                            str = pi.Name + " nvarchar(max)";
                    }
                    else if (pi.PropertyType == typeof(int))
                    {
                        str = pi.Name + " int";

                        if (pi.Name == "g_id")
                            str += " identity(1,1)";
                    }
                    else
                    {
                        continue;
                    }

                    if (g_key == pi.Name)
                        str += " PRIMARY KEY";

                    if (sql != "")
                        sql += ",";

                    sql += str;

                }

                sql = " CREATE TABLE " + tb_head + tbtype.Name + "( " + sql + ")";

                ExecSql(sql);

                (tb as Models.DBTable.TableBase).InitTable();

                return true;
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }
            return false;

        }

        public static Boolean Insert<T>(T tb)
        {
            try
            {

                Type tbtype = typeof(T);
                PropertyInfo[] tbprops = tbtype.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                string sql = "";

                //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)

                string str1 = "";
                string str2 = "";

                foreach (PropertyInfo pi in tbprops)
                {
                    if (pi.Name == "g_id")
                        continue;

                    if (str1 != "")
                    {
                        str1 += " , ";
                        str2 += " , ";
                    }

                    if (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int))
                    {
                        str1 += pi.Name;
                        str2 += "'" + pi.GetValue(tb, null).ToString() + "'";
                    }
                    else
                    {
                        continue;
                    }


                }

                sql = " INSERT INTO " + tb_head + tbtype.Name + " ( " + str1 + " ) VALUES (" + str2 + " ) ";

                return ExecSql(sql) > 0;

            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }

            return false;
        }

        public static Boolean Update<T>(T tb)
        {
            try
            {


                Type tbtype = typeof(T);
                PropertyInfo[] tbprops = tbtype.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                string g_key = (tb as Models.DBTable.TableBase).g_key;

                string sql = "";

                string str1 = "";
                string str2 = "";

                foreach (PropertyInfo pi in tbprops)
                {
                    if (pi.Name == "g_id")
                        continue;

                    if (pi.Name == g_key)
                    {
                        str2 = pi.Name + " = '" + pi.GetValue(tb, null).ToString() + "'";
                        continue;
                    }


                    if (str1 != "")
                    {
                        str1 += " , ";
                    }


                    str1 += pi.Name + " = '" + pi.GetValue(tb, null) + "'";


                }

                sql = " UPDATE " + tb_head + tbtype.Name + " SET " + str1 + " WHERE " + str2;

                return ExecSql(sql) > 0;

            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }

            return false;
        }

        public static Boolean Select<T>(ref T tb)
        {
            try
            {

                string g_key = (tb as Models.DBTable.TableBase).g_key;
                Type tbtype = typeof(T);

                string sql = "SELECT * FROM " + tb_head + tbtype.Name + " WHERE " + g_key + " = '" + tbtype.GetProperty(g_key).GetValue(tb, null).ToString() + "'";

                SqlDataReader sdr = OpenSql(sql);

                PropertyInfo[] tbprops = tbtype.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                if (!sdr.Read())
                    return false;

                foreach (PropertyInfo pi in tbprops)
                {
                    pi.SetValue(tb, sdr[pi.Name], null);
                }

                return true;
            }
            catch (Exception ex)
            {
                BLL.GlfFun.AddLog(ex);
            }

            return false;

        }
    }
}