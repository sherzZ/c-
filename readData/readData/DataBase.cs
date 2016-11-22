using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace readData
{
    class DataBase
    {
        private static string constr = @"Server=localhost;UserId=root;password=Z.yq1392010;Database=emotion";
        public MySqlConnection sqlConnection() {
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            string msg = conn.State.ToString();
           // Console.WriteLine(msg);
            //Console.ReadKey();
            return conn;
            //conn.Close();
        }
        public void sqlConnectionClose(MySqlConnection conn) {
            conn.Close();
        }
        public static void ExecuteSqlTran(string insertErrorCommondLogPath,List<string> SQLStringList) {

            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            MySqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            int numOfCommit = 0;
            List<string> insertErrorCommomd = new List<string>();
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            insertErrorCommomd.Add(SQLStringList[n]);
                            string errorInfo = string.Format("执行到第{0}条记录失败\r\n", n);
                            MyLog.errorLog(E, errorInfo);
                            continue;
                        }
                        catch (Exception e) {
                            insertErrorCommomd.Add(SQLStringList[n]);
                            string errorInfo = string.Format("执行到第{0}条记录失败\r\n", n);
                            MyLog.errorLog(e, errorInfo);
                            continue;
                        }
                       
                    }
                    if (n > 0 && (n % 500 == 0 || n == SQLStringList.Count - 1))
                    {
                        try
                        {
                            tx.Commit();
                            numOfCommit += 1;
                            string insertInfo=string.Format("已经提交第{0}次, 执行到第{1}条插入数据\r\n",numOfCommit,n);
                            MyLog.insertLog(insertInfo);
                            tx = conn.BeginTransaction();
                        }
                        catch (Exception e)
                        {
                            insertErrorCommomd.Add(SQLStringList[n]);
                            string errorInfo = string.Format("提交第{0}次记录失败\r\n", numOfCommit);
                            MyLog.errorLog(e, errorInfo);
                        }
                                   
                    }
                }
                MyLog.writeSqlCommond(insertErrorCommondLogPath, insertErrorCommomd);
        }
    }
}
