# c# 项目代码
## txtdataToMysql  
项目描述：解析文本文件，该文件使用爬虫爬下的数十万条数据，主要是图片以及图片关键字，图片信息使用`','`分隔，关键字使用 `' '`（一个空格）,关键字不允许重复。    


- 分析需要创建的数据库  
  该数据库为多对多，所以需要三张表，一张表为`image_info`，一张为`image_keyword`，一张为关系表`img_and_key_relation`。在这里并没有添加外键，加入外键后插入数据难度比较大，`Mysql`还没有具体学习过，关于主键与外键的关系，以及外键的作用等以后补充。以下为创建数据库的代码   
```   
create database emotion;  
use emotion;   
create table image_Keyword(keyword_id int(4) not null primary key,keyword varchar(40) not null);
create table image_info(image_id int(4) not null primary key,id_other varchar(20) not null,id_self varchar(9) not null,image_url varchar(100) not null);
create table img_and_key_relation(relation_id int(4) not null primary key auto_increment,keyword_id int(4) not null,image_id int(4) not null);
```
这里并没有将`image_info`与`image_keyword`的主键设为自动增加，为了插入`img_and_key_relation`表时能够获得其余两张表的键值，具体获得方法将在代码中讲解。  
 解析文本
  代码使用c#写的所以可以使用按行读取文本，使用`Split()`方法将文本进行解析，对于文本解析专门设置了一个`class TxtFilter`，在该类中实现文本解析方法，在插入关键字时，使用`Dictionary<string, int> keywordDic`,将关键字存入字典中，字典的关键字为`解析的关键字`,使用字典因为要求键值唯一，这样在匹配关键字是可以方便，开始的时候并没有注意到Mysql数据库大小写不敏感，而c#的字典键值为大小写敏感的，所有在插入关键字是有些重复了，设置字典大小写不敏感需要如此创建字典
` Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);`这样可以保证插入的关键字唯一。对判断关键字的唯一性使用`hashset`也是可以，但是不知道它能否无视大小写，所以还需要补充，`hashset`的时间复杂度为`O(1)`,而`Dictionary`的时间复杂度为`O(n)`，当数据量比较大时查找速度比较快。`Dictionary`与`hashset`的区别再去字典使用`key`查找`value`,查找`value`的速度比较快，而`hashset`为一个集合，关于这两个的区别以后还会补充，目前只知道这些。  
- c#连接Mysql数据库
```
 private static string constr = @"Server=localhost;UserId=root;password=yourpassword;Database=databaseName";
        public MySqlConnection sqlConnection() {
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            string msg = conn.State.ToString();
            return conn;
        }
        public void sqlConnectionClose(MySqlConnection conn) {
            conn.Close();
        }
```
这里创建了一个数据库类，在该类中实现以上两种方法，目的保护数据库账户密码，在这里并不需要，只是个人的一点想法，不知道对错
- 如何大批量插入数据到Mysql中
  这里参考了[链接](http://blog.csdn.net/wwwww112233/article/details/8562630)采用了事物处理的方法，具体的还需要进一步学习。主要是将插入语句放入到一个`List<string>`中，所以需要解析文本，生成插入语句，将所有的插入语句添加到一个列表中，为了检查插入语句的正确性，将所有的插入语句写入到一个txt文本中，方便检查。
- txtFilter类分析
  解析的文本文件由一些特点，有些图片没有关键字，所以对于这些信息需要判断删除，并且解析关键字的时候有的关键字是空格，这些问题在开始的时候都没有考虑到。所以在原来的代码上添加了一些判定条件。为了检查方便将三张表的插入语句各自打印了出来，有些代码存在重复，可以优化一些，比如针对不同的操作设定操作码，使用`switch case`语句，选择不同的筛选方式。先写出关键的文本解析语句，将所有插入语句存入`List<string>`中，并且定义了将插入语句写入文本的函数，这样有利于查看插入语句是否存在问题。以下是主要函数代码：
```
        public List<string> filterToSqlCommond2(string fileName)
        {
            List<string> SQLStringList = new List<string>();
            Data fileData = new Data();
            Dictionary<string, int> keywordDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            string sqlCommond="";
            int numOfKeyword = 0;
            int numOfImage = 0;
            //如果使用HashSet判断关键字是否存在会更快一些，这里并没有用这种方法
            //HashSet<string> keywordSet = new HashSet<string>();
            StreamReader sr = new StreamReader(fileName);
            string strContent;
            while ((strContent = sr.ReadLine()) != null)
            {
                string[] strArr = strContent.Split(',');
                fileData.id_other = strArr[0];
                fileData.id_self = strArr[1];
                fileData.image_url = strArr[3];
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
```
为了将关键字是空格的图像解析出去，所以使用了两种方式，一般有空格的图像信息的键值长度为`1`，但是键值为`1`的又不一定是空格（有点啰嗦意思理解就好）所以将解析方式设为两种，键值长度为`1`与`>1`两种。然后判断键值是否为`空格`，不是空格的话将图像长度`+1`，数据库中图像表的键值与图片长度（相当于图片的`index`）相关联。
因为一个图像不一定有一个关键字，所以图像的插入语句与关键字与关系表的插入语句分开统计。获得每个图片关键字的长度，将每一个图片的关键字使用`Sqlit(' ')`解析到一个字符串数组中，遍历字符串判断关键字是否存在在字典中，不存在的话将该关键字插入到关键字表中，存在的话直接在关键字字典中查找该关键字的键值，将图像的键值与该关键字的键值插入到关系表中。最后将该图片的所有插入语句赋值给`sqlCommond`，加入到`指令列表SQLStringList`中，解析完后返回列表。
 



