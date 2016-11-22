using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readData
{
    class MyLog
    {
        //错误日志
        public static void errorLog(Exception ex, String erroInfo)
        {

            String LogAddress = @"E:\visualStudio2013\Projects\readData\" +
                     DateTime.Now.Year + '-' +
                     DateTime.Now.Month + '-' +
                     DateTime.Now.Day +'_'+ DateTime.Now.Hour+'_'+DateTime.Now.Minute+"_insertErrorInfo.log";
            FileStream fs;

            if (!File.Exists(LogAddress))
            {
                fs = new FileStream(LogAddress, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("错误信息{0}：", ex);
                sw.WriteLine("插入数据信息：{0}：", erroInfo);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            else
            {
                fs = new FileStream(LogAddress, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("错误信息{0}：", ex);
                sw.WriteLine("插入数据信息：{0}", erroInfo);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        //插入日志，提交信息
        public static void insertLog(String insertInfo)
        {
            String LogAddress = @"E:\visualStudio2013\Projects\readData\" +
                     DateTime.Now.Year + '-' +
                     DateTime.Now.Month + '-' +
                     DateTime.Now.Day + "_insertInfo.log";
            if (!File.Exists(LogAddress))
            {
                FileStream fs = new FileStream(LogAddress, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("插入数据信息：{0}：", insertInfo);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(LogAddress, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("插入数据信息：{0}", insertInfo);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        //插入错误的指令存储函数
        public static void errorInsertCommond(string path,List<string> insertErrorCommond) {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            foreach (var d in insertErrorCommond)
            {
                sw.WriteLine(d);
            }
            sw.Flush();
            sw.Close();
            fs.Close();

        }
        public static void writeSqlCommond(string path, List<string> SqlCommondList)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            foreach (var d in SqlCommondList)
            {
                sw.WriteLine(d);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
