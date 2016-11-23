using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace readData
{
    class Progrom
    {
        static void Main(string[] args)
        {
            //整个文本
            string fileName = @"E:\visualStudio2013\Projects\readData\result5.txt";
            //关键字文本
            string keywordPath = @"E:\visualStudio2013\Projects\readData\keyword.txt";
            //关键字插入指令文本
            string keywordInsertCommondPath = @"E:\visualStudio2013\Projects\readData\keywordInsertCommond.txt";
            //所有插入指令文本
            string sqlCommondPath = @"E:\visualStudio2013\Projects\readData\SqlAllCommond2.txt";
            //插入错误语句文本
            string insertErroCommondPath = @"E:\visualStudio2013\Projects\readData\insertErroCommond.txt";
            string insertErroCommondPath2 = @"E:\visualStudio2013\Projects\readData\insertErroCommond2.txt";
            //错误语句修改后文本
            string errorComAltPath = @"E:\visualStudio2013\Projects\readData\insertErroCommond_alter.txt";
            //插入关系语句文本
            string insertRelationPath = @"E:\visualStudio2013\Projects\readData\RelationCommond_alter.txt";
            TxtFilter txtFilter = new TxtFilter();
            //解析关键字到字典中并且写到相应的文本中
           //Dictionary<string,int> keywordDic=txtFilter.filterKeyword(fileName);

            // 将插入关键字的语句返回到list中
            //List<string> insertKeyword = txtFilter.filterKeywordToSqlCommond(keywordPath);

            // 将插入语句写到keywordInsertCommond.txt" 保存关键字插入语句
            //MyLog.writeSqlCommond(keywordInsertCommondPath, insertKeyword);

            //将关键字与键值写到文本中
            //txtFilter.writeDic(keywordPath, keywordDic);

            //将所有插入语句返回到list中
            List<string> SQLCommondList =txtFilter.filterToSqlCommond2(fileName);

            //执行所有的插入语句
            DataBase.ExecuteSqlTran(insertErroCommondPath, SQLCommondList);

            //将所有插入语句写到SqlAllCommond2.txt中
           // MyLog.writeSqlCommond(sqlCommondPath, SQLCommondList);

            //解析错误文本信息到list中
            // List<string> SQLCommondList = txtFilter.filterErrCommondLog(insertErroCommondPath);

            //将修改后的错误语句放到alt文本中，解析返回
           //List<string> SQLCommondList = txtFilter.filterErrCommondLog(errorComAltPath);

            //List<string> SQLCommondList = txtFilter.filterCommondLog(insertErroCommondPath2);

            //错误信息保存日志与要执行的语句
            //DataBase.ExecuteSqlTran(insertErroCommondPath, SQLCommondList);

            //将插入命令写到error common的 alter path的文本中
            //MyLog.writeSqlCommond(errorComAltPath, SQLCommondList);

            //将所有关系插入指令解析出，并且存入相应的文本
           // List<string> relationCommondList = txtFilter.filterToSqlCommond3(fileName);
            //MyLog.writeSqlCommond(insertRelationPath, relationCommondList);
            //解析关系文本，并插入到表中
            //List<string> insertReComList = txtFilter.filterCommondLog(insertRelationPath);
            //DataBase.ExecuteSqlTran(insertErroCommondPath, insertReComList);

            //解析关键字列表，将关键字插入到表中
            //List<string> insertKeywordList = txtFilter.filterCommondLog(keywordInsertCommondPath);
            //DataBase.ExecuteSqlTran(insertErroCommondPath, insertKeywordList);

        }
    }
}
