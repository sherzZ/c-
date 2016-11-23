using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readData
{

    class TxtFilter
    {
        private struct Data
        {
            public string id_other;
            public string id_self;
            public string image_url;
        }
        public Dictionary<string, int> filterKeyword(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
           // HashSet<string> keywordSet = new HashSet<string>();
            string strContent;
            int rowNum = 0;
            int keywordListLength = 0;
            while ((strContent = sr.ReadLine()) != null)
            {
                rowNum += 1;
                string[] strArr = strContent.Split(',');
                string[] keyword = strArr[4].Split(' ');
                int keyword_Length = keyword.Length;
                for (int i = 0; i < keyword_Length; i++)
                {
                    if (!string.IsNullOrEmpty(keyword[i]))
                    {
                        if (!keywordDic.Keys.Contains(keyword[i]))
                        {
                            keywordListLength += 1;
                            keywordDic.Add(keyword[i], keywordListLength);
                        }
                    }
                }
            }
            return keywordDic;
        }
        public void writeDic(string path, Dictionary<string, int> myDic)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            foreach (var d in myDic)
            {
                sw.WriteLine(d.Key + "," + d.Value);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public List<string> filterKeywordToSqlCommond(string fileName) {
            List<string> SQLStringList = new List<string>();
            StreamReader sr = new StreamReader(fileName);
            string strContent;
            while ((strContent = sr.ReadLine()) != null)
            {
                string[] strArr = strContent.Split(',');
                string sqlCommond = string.Format(@"insert into imagekeyword(keyword_id,keyword) value('{0}','{1}'); ", strArr[1],strArr[0]);
                SQLStringList.Add(sqlCommond);             
            }
            return SQLStringList;
        }

        public List<string> filterToSqlCommond(string fileName) {
            List<string> SQLStringList = new List<string>();
            Data fileData = new Data();
            Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            //Dictionary<string, int> imageDic = new Dictionary<string, int>();
            string sqlCommond;
            int numOfKeyword = 0;
            int numOfImage = 0;
            //HashSet<string> keywordSet = new HashSet<string>();
            StreamReader sr = new StreamReader(fileName);
            string strContent;
            while ((strContent = sr.ReadLine()) != null) {
                numOfImage += 1;
                string[] strArr = strContent.Split(',');
                fileData.id_other = strArr[0];
                fileData.id_self = strArr[1];
                fileData.image_url = strArr[3];
               // imageDic.Add(fileData.id_other, numOfImage);
                string insertImageCommond = string.Format(@"insert into image(image_id,id_other,id_self,image_url) values('{0}','{1}','{2}','{3}');", numOfImage,fileData.id_other, fileData.id_self, fileData.image_url);
                string insertKeyAndR = "";
                string[] keyword = strArr[4].Split(' ');
                //numKeyword表示读取每一行获得的关键字个数
                int numKeyword = keyword.Length;
                for (int i = 0; i < numKeyword; i++) {
                    if (i == 0)
                    {
                        if (!keywordDic.Keys.Contains(keyword[i]))
                        {
                            numOfKeyword += 1;
                            keywordDic.Add(keyword[i], numOfKeyword);
                            insertKeyAndR = string.Format(@" insert into imagekeyword(keyword_id,keyword) value('{0}','{1}');insert into img_and_key_relation(keyword_id,image_id) values('{2}','{3}');", keywordDic[keyword[i]], keyword[i], keywordDic[keyword[i]], numOfImage);
                            sqlCommond = insertImageCommond + insertKeyAndR;
                            SQLStringList.Add(sqlCommond);
                        }
                        else
                        {
                            int keyword_id = keywordDic[keyword[i]];
                            insertKeyAndR = string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{0}','{1}');", keyword_id, numOfImage);
                            sqlCommond = insertImageCommond + insertKeyAndR;
                            SQLStringList.Add(sqlCommond);
                        }
                    }
                    else {
                        if (!keywordDic.Keys.Contains(keyword[i]))
                        {
                            numOfKeyword += 1;
                            keywordDic.Add(keyword[i], numOfKeyword);
                            insertKeyAndR = string.Format(@" insert into imagekeyword(keyword_id,keyword) value('{0}','{1}');insert into img_and_key_relation(keyword_id,image_id) values('{2}','{3}');", keywordDic[keyword[i]], keyword[i], keywordDic[keyword[i]], numOfImage);
                            SQLStringList.Add(insertKeyAndR);
                        }
                        else
                        {
                            int keyword_id = keywordDic[keyword[i]];
                            insertKeyAndR = string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{0}','{1}');", keyword_id, numOfImage);
                            SQLStringList.Add(insertKeyAndR);
                        }

                    }
                }
            }
            return SQLStringList;
        }
        public List<string> filterToSqlCommond2(string fileName)
        {
            List<string> SQLStringList = new List<string>();
            Data fileData = new Data();
            Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            //Dictionary<string, int> imageDic = new Dictionary<string, int>();
            string sqlCommond="";
            int numOfKeyword = 0;
            int numOfImage = 0;
            //HashSet<string> keywordSet = new HashSet<string>();
            StreamReader sr = new StreamReader(fileName);
            string strContent;
            while ((strContent = sr.ReadLine()) != null)
            {
                string[] strArr = strContent.Split(',');
                fileData.id_other = strArr[0];
                fileData.id_self = strArr[1];
                fileData.image_url = strArr[3];
                // imageDic.Add(fileData.id_other, numOfImage);
                string insertImageCommond = "";
                string insertKeyAndR = "";
                string[] keyword = strArr[4].Split(' ');
                //numKeyword表示读取每一行获得的关键字个数
                int numKeyword = keyword.Length;
                switch(numKeyword){
                    case 1:
                        if (!string.IsNullOrEmpty(keyword[0]))
                        {
                            numOfImage += 1;
                            insertImageCommond = string.Format(@"insert into image(image_id,id_other,id_self,image_url) values('{0}','{1}','{2}','{3}');", numOfImage, fileData.id_other, fileData.id_self, fileData.image_url);
                            if (!keywordDic.Keys.Contains(keyword[0]))
                            {
                                numOfKeyword += 1;
                                keywordDic.Add(keyword[0], numOfKeyword);
                                string.Format(@" insert into imagekeyword(keyword_id,keyword) value('{0}','{1}');insert into img_and_key_relation(keyword_id,image_id) values('{2}','{3}');", keywordDic[keyword[0]], keyword[0], keywordDic[keyword[0]], numOfImage);
                            }
                            else
                            {
                                int keyword_id = keywordDic[keyword[0]];
                                insertKeyAndR = insertKeyAndR + string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{0}','{1}');", keyword_id, numOfImage);
                            }                           
                        }
                         sqlCommond = insertImageCommond + insertKeyAndR;
                         SQLStringList.Add(sqlCommond);
                        break;
                    default:
                        numOfImage += 1;
                        insertImageCommond = string.Format(@"insert into image(image_id,id_other,id_self,image_url) values('{0}','{1}','{2}','{3}');", numOfImage, fileData.id_other, fileData.id_self, fileData.image_url);
                        for (int i = 0; i < numKeyword; i++)
                        {
                            if (!string.IsNullOrEmpty(keyword[i]))
                            {
                                if (!keywordDic.Keys.Contains(keyword[i]))
                                {
                                    numOfKeyword += 1;
                                    keywordDic.Add(keyword[i], numOfKeyword);
                                    insertKeyAndR = insertKeyAndR + string.Format(@" insert into imagekeyword(keyword_id,keyword) value('{0}','{1}');insert into img_and_key_relation(keyword_id,image_id) values('{2}','{3}');", keywordDic[keyword[i]], keyword[i], keywordDic[keyword[i]], numOfImage);
                                }
                                else
                                {
                                    int keyword_id = keywordDic[keyword[i]];
                                    insertKeyAndR = insertKeyAndR + string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{0}','{1}');", keyword_id, numOfImage);
                                }                              
                            }
                        }
                        sqlCommond = insertImageCommond + insertKeyAndR;
                        SQLStringList.Add(sqlCommond);
                        break;
                }
            }
            return SQLStringList;
        }

        //将关系表解析出生成list
        public List<string> filterToSqlCommond3(string fileName)
        {
            List<string> SQLStringList = new List<string>();
            Data fileData = new Data();
            Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            //Dictionary<string, int> imageDic = new Dictionary<string, int>();
            string sqlCommond = "";
            int numOfKeyword = 0;
            int numOfImage = 0;
            //HashSet<string> keywordSet = new HashSet<string>();
            StreamReader sr = new StreamReader(fileName);
            string strContent;
            while ((strContent = sr.ReadLine()) != null)
            {
                numOfImage += 1;
                string[] strArr = strContent.Split(',');
                fileData.id_other = strArr[0];
                fileData.id_self = strArr[1];
                fileData.image_url = strArr[3];
                // imageDic.Add(fileData.id_other, numOfImage);
                //string insertImageCommond = string.Format(@"insert into image(image_id,id_other,id_self,image_url) values('{0}','{1}','{2}','{3}');", numOfImage, fileData.id_other, fileData.id_self, fileData.image_url);
                string insertKeyAndR = "";
                string[] keyword = strArr[4].Split(' ');
                //numKeyword表示读取每一行获得的关键字个数
                int numKeyword = keyword.Length;
                for (int i = 0; i < numKeyword; i++)
                {
                    if (!keywordDic.Keys.Contains(keyword[i]))
                    {
                        numOfKeyword += 1;
                        keywordDic.Add(keyword[i], numOfKeyword);
                        insertKeyAndR = insertKeyAndR + string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{2}','{3}');", keywordDic[keyword[i]], keyword[i], keywordDic[keyword[i]], numOfImage);
                    }
                    else
                    {
                        int keyword_id = keywordDic[keyword[i]];
                        insertKeyAndR = insertKeyAndR + string.Format(@"insert into img_and_key_relation(keyword_id,image_id) values('{0}','{1}');", keyword_id, numOfImage);
                    }
                }
                sqlCommond = insertKeyAndR;
                SQLStringList.Add(sqlCommond);
            }
            return SQLStringList;
        }
       
        //读取插入指令文本信息，将插入语句转为list<string>
        public List<string> filterCommondLog(string CommondLogPath) {
            List<string> errComList = new List<string>();
            StreamReader sr = new StreamReader(CommondLogPath);
            string strContent;
            while ((strContent = sr.ReadLine()) != null) {
                errComList.Add(strContent);
            }
            return errComList;
        }
        //读取错误插入语句信息，解析该文本，将文本第一条语句过滤掉,返回list<string>commond
        public List<string> filterErrCommondLog(string errorCommondLogPath) {
            List<string> sqlCommondList = new List<string>();
            StreamReader sr = new StreamReader(errorCommondLogPath);
            string strContent;
            //string sqlCommond="";
            while ((strContent = sr.ReadLine()) != null) {
                string[] strArr = strContent.Split(';');
                int length = strArr.Length;
                string sqlCommond = "";
                for (int i = 1; i < length;i++ )
                {
                     sqlCommond+= strArr[i]+";";
                }
                sqlCommondList.Add(sqlCommond);
            }
            return sqlCommondList;
        }
    }
}
