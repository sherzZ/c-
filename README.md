# c# 项目代码
##dataToMysql  
项目描述：解析文本文件，该文件使用爬虫爬下的数十万条数据，主要是图片以及图片关键字，图片信息使用`','`分隔，关键字使用 `' '`（一个空格）,关键字不允许重复。    


- 分析需要创建的数据库  
  该数据库为多对多，所以需要三张表，一张表为`image_info`，一张为`image_keyword`，一张为关系表`img_and_key_relation`。在这里并没有添加外键，加入外键后插入数据难度比较大，`Mysql`还没有具体学习过，关于主键与外键的关系，以及外键的作用等以后补充。以下为创建数据库的代码   
```   
create database emotion;  
use emotion;   
create table image_Keyword(keyword_id int(4) not null primary key,keyword varchar(40) not null);
create table image_info(image_id int(4) not null primary key,id_other varchar(20) not null,id_self varchar(9) not null,image_url varchar(100) not null);
create table img_and_key_relation(relation_id int(4) not null primary key auto_increment,keyword_id int(4) not null,image_id int(4) not null);```
这里并没有将`image_info`与`image_keyword`的主键设为自动增加，为了插入`img_and_key_relation`表时能够获得其余两张表的键值，具体获得方法将在代码中讲解。  
- 解析文本
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
  这里参考了[链接](http://blog.csdn.net/wwwww112233/article/details/8562630)采用了事物处理的方法，具体的还需要进一步学习。主要是将插入语句放入到一个`List<string>`中，
 
  



