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
            string fileName = @"E:\visualStudio2013\Projects\readData\result5.txt";
            string keywordPath = @"E:\visualStudio2013\Projects\readData\keyword.txt";
            string keywordInsertCommondPath = @"E:\visualStudio2013\Projects\readData\keywordInsertCommond.txt";
            string sqlCommondPath = @"E:\visualStudio2013\Projects\readData\SqlAllCommond2.txt";
            string insertErroCommondPath = @"E:\visualStudio2013\Projects\readData\insertErroCommond.txt";
            string insertErroCommondPath2 = @"E:\visualStudio2013\Projects\readData\insertErroCommond2.txt";
            string errorComAltPath = @"E:\visualStudio2013\Projects\readData\insertErroCommond_alter.txt";
            TxtFilter txtFilter = new TxtFilter();
            //Dictionary<string,int> keywordDic=txtFilter.filterKeyword(fileName);
            List<string> insertKeyword = txtFilter.filterKeywordToSqlCommond(keywordPath);
            MyLog.writeSqlCommond(keywordInsertCommondPath, insertKeyword);
            //txtFilter.writeDic(keywordPath, keywordDic);
            //List<string> SQLCommondList =txtFilter.filterToSqlCommond2(fileName);
           // DataBase.ExecuteSqlTran(insertErroCommondPath, SQLCommondList);
            //MyLog.writeSqlCommond(sqlCommondPath, SQLCommondList);
             //List<string> SQLCommondList = txtFilter.filterErrCommondLog(insertErroCommondPath);
           //List<string> SQLCommondList = txtFilter.filterErrCommondLog(errorComAltPath);
            //List<string> SQLCommondList = txtFilter.filterCommondLog(insertErroCommondPath2);
            //DataBase.ExecuteSqlTran(insertErroCommondPath, SQLCommondList);
            //MyLog.writeSqlCommond(errorComAltPath, SQLCommondList);

        }
    }
}
