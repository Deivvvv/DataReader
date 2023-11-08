using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

using DataSpace;
using TableSystem;
namespace FileReader
{
    public struct stringData
    {
        public int Id;
        public string Text;
        public stringData(int id, string str)
        {
            Id = id;
            Text = str;
        }
    }
    static class Reader
    {
        static string mainPath = Application.dataPath + "/Dir/";

        public static void Start()
        {
            string[] com = { "AddTable", "Group", "DataTime" };
            foreach (string path in com)
                if (!Directory.Exists($"{mainPath}{path}/"))
                    Directory.CreateDirectory($"{mainPath}{path}/");

            if (AddTableScan())
                AddData();

            /*
             load
            class room
            subject
            teacher <-classroom <-subject
             
             */
        }
        static void AddData()
        {
            int i = 0;
            string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xml");//.xml
            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);//(mainPath + "Charter.xml");
                string[] com1 = path.Split('_');//com[0] - путь до исходного файла // com[1+] |table|gmu|05-2023
                string newPath = $"{mainPath}{com1[1]}/{com1[2]}/";
                switch (com1[1])
                {
                    //case ("Class"):
                    //    File.Move(path, $"{newPath}{com1[3]}");
                    //    break;
                    case ("GroupTable"):
                        i = Storage.FindId("Group", com1[2]);
                        {
                            com1 = System.DateTime.Today.ToString().Split(' ');
                            int[] timeData = com1[0].Split('.').Select(int.Parse).ToArray();
                            //int[] timeData = com1.Select(int.Parse).ToArray();
                            List<TableData> tableDatas = new List<TableData>();
                            nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                            foreach (XmlNode x in nodes)
                            {
                                node = XElement.Load(new XmlNodeReader(x));
                                TableData data = new TableData(node.Element("Time").Value, com[0]);
                                if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                                    continue;

                                data.SetRealTime(node.Element("StartTime").Value, true);
                                data.SetRealTime(node.Element("EndTime").Value, false);
                                Storage.ConnectData(data);
                                data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                                data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                                data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                                tableDatas.Add(data);
                            }
                            Storage.GroupSetTable(tableDatas);
                        }

                        break;
                }
            }
            
        }
        static bool AddTableScan()
        {
            string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xml");//.xml

            foreach (string path in com)
            {
                Debug.Log(path);
                string[] com1 = path.Split('_');//com[0] - путь до исходного файла // com[1+] |table|gmu|05-2023
                string newPath = $"{mainPath}{com1[1]}/{com1[2]}/";
                //com1 = com1[2].Split('.');
                //newPath += com1[0];//$"{ com1[0]}/";
                if (!Directory.Exists($"{newPath}"))
                    Directory.CreateDirectory($"{newPath}");
                File.Move(path,$"{newPath}{com1[3]}");
            }
            Debug.Log(com.Length);
            return (com.Length > 0);
        }

        static List<string> paths;
        static void ScanFolders(string str)
        {

            string[] allfolders = Directory.GetDirectories(str);
            if (allfolders.Length > 0)
            {
                foreach (string folder in allfolders)
                    ScanFolders(folder);
                return;
            }
            else
            {
                allfolders = Directory.GetFiles(str, "*.xml");//.xml
                foreach (string folder in allfolders)
                    paths.Add(folder);
            }
        }
        static void RebuildTable()
        {
            paths = new List<string>();
            string[] com = System.DateTime.Today.ToString().Split(' ');
            com = com[0].Split('.');
            int[] timeData = com.Select(int.Parse).ToArray();
            //Directory.GetFiles($"{mainPath}AddTable/", "*.xml");//.xml

            ScanFolders($"{mainPath}Table/");


            //Info/2023/02/01/
            // string DataTime = System.DateTime.Today.ToString();
            //Debug.Log(DataTime);
            // string[] com = DataTime.Split('.');
        }
        static void LoadGroup()
        {
            string[] com1;
            string[] allfolders = Directory.GetDirectories($"{mainPath}Group/");
            string[] com = new string[allfolders.Length];
            for (int i = 0; i < com.Length; i++)
            {
                com1 = allfolders[i].Split('/');
                com[i] = com1[com1.Length - 2];
            }
            Storage.SetMainGroup(com);

            /*
             порядок загрузки
            классы
            дисциплины
            преподаватели
            группы
             */

            for (int i = 0; i < allfolders.Length; i++)
            {
                com = Directory.GetFiles(allfolders[i], "*.xml");
                for (int j = 0; j < com.Length; j++)
                {
                    com1 = com[1].Split('/');
                    com1 = com1[com1.Length - 1].Split('.');
                    Storage.GroupAdd(i, LoadGroup(com[j], com1[0]));
                    //com1[0]
                    UnityEngine.Debug.Log("! указатель на группу прикрепляется вообщем списке в формате int[2]");
                }
            }
        }


        static int AddsString(List<stringData> sSub, string tayp, string name)
        {

            int k = sSub.FindIndex(x => x.Text == name);
            if (k == -1)
            {
                k = Storage.FindId(tayp, name);
                sSub.Add(new stringData(k, name));
            }

            return k;
        }

        static GroupData LoadGroup(string path, string name)
        {

            List<stringData> sSubject = new List<stringData>();
            List<stringData> sTeacher = new List<stringData>();
            List<stringData> sClass = new List<stringData>();

            GroupData group = new GroupData(name);
            XmlDocument root = new XmlDocument();
            root.Load(path);
            XElement node;
            XmlNodeList nodes;

            /*
             
                XElement action = new XElement("Teacher");
                string str = Storage.GetName("Teacher", group.Teachers[i].Name);
                foreach (int j in group.Teachers[i].Subjects)
                   str += AddsString(sSubject, "Subject", j);

                action.Add(new XElement("Text", str));
                root.Add(action);
            }

            foreach(TableData data in group.Subjects)
            {
                XElement action = new XElement("Subject");
             
             */
            nodes = root.DocumentElement.SelectNodes("descendant::Teacher"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                int[] id = 
                TableData data = new TableData(node.Element("Time").Value);
                data.SetRealTime(node.Element("StartTime").Value, true);
                data.SetRealTime(node.Element("EndTime").Value, false);
                // Storage.ConnectData(data);
                data.Subject = AddsString(sSubject, "Subject", node.Element("Subject").Value);
                data.Teacher = AddsString(sTeacher, "Teacher", node.Element("Teacher").Value);
                data.ClassRoom = AddsString(sClass, "Class", node.Element("Class").Value);

                group.Subjects.Add(data);
            }

            nodes = root.DocumentElement.SelectNodes("descendant::Subject"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                TableData data = new TableData(node.Element("Time").Value);
                data.SetRealTime(node.Element("StartTime").Value, true);
                data.SetRealTime(node.Element("EndTime").Value, false);
               // Storage.ConnectData(data);
                data.Subject = AddsString(sSubject,"Subject", node.Element("Subject").Value);
                data.Teacher = AddsString(sTeacher,"Teacher", node.Element("Teacher").Value);
                data.ClassRoom = AddsString(sClass,"Class", node.Element("Class").Value);

                group.Subjects.Add(data);
            }
            return group;
        }

        static void ReadTable(string path)
        {

            TableData data = null;
            string[] com;
            int[] ints = new int[2];
            com = path.Split('/');
            ints[0] = Storage.FindId("MainGroup", com[com.Length - 2]);
            com = com[com.Length - 1].Split('.');
            ints[1] = Storage.FindId($"{ints[0]}_Group", com[0]);

            XmlDocument root = new XmlDocument();
            root.Load(path);//(mainPath + "Charter.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                data = new TableData(node.Element("Time").Value, com[0]);
                data.SetRealTime(node.Element("StartTime").Value, true);
                data.SetRealTime(node.Element("EndTime").Value, false);
                Storage.ConnectData(data);
                data.Subject = Storage.FindId("Subject",node.Element("Subject").Value);
                data.Teacher = Storage.FindId("Teacher",node.Element("Teacher").Value);
                data.ClassRoom = Storage.FindId("Class", node.Element("Class").Value);
            }

            if(data != null)
                Storage.SaveGroup(data.Group);
            //string[] origData = textassetData.text.Split
        }

        #region groupData
        static void ReadDataGroup()
        {
            string[] com = Directory.GetFiles($"{mainPath}Data/Group/", "*.xml");//.xml
        }
        static List<SubjectData> ReadDataSubject()
        {
            List<SubjectData> datas = new List<SubjectData>();
            string path = $"{mainPath}Data/Misk/Subject.xml";
            if (File.Exists(path))
            {

                SubjectData data;
                XmlDocument root = new XmlDocument();
                root.Load(path);//(mainPath + "Charter.xml");
                XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
                XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                foreach (XmlNode x in nodes)
                {
                    node = XElement.Load(new XmlNodeReader(x));
                    data = new SubjectData(node.Element("Name").Value);

                    datas.Add(data);
                }
            }
            return datas;
        }
        static List<TeacherData> ReadDataTeacher()
        {
            List<TeacherData> datas = new List<TeacherData>();

            string[] com = Directory.GetFiles($"{mainPath}Data/Teacher/", "*.xml");//.xml
            TeacherData data;
            foreach (string path in com)
            {
                XmlDocument root = new XmlDocument();
                //root.Load(path);//(mainPath + "Charter.xml");
                XElement node = XDocument.Parse(File.ReadAllText(path)).Element("root");
                //node = XDocument.Parse(File.ReadAllText(mainPath + "Roles.xml")).Element("root");
                data = new TeacherData(node.Element("Name").Value);

                datas.Add(data);

            }
            return datas;
        }
        static List<ClassData> ReadDataClassRoom()
        {
            List<ClassData> datas = new List<ClassData>();
            string path = $"{mainPath}Data/Misk/ClassRoom.xml";
            if (File.Exists(path))
            {
                ClassData data;
                XmlDocument root = new XmlDocument();
                root.Load(path);//(mainPath + "Charter.xml");
                XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
                XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                foreach (XmlNode x in nodes)
                {
                    node = XElement.Load(new XmlNodeReader(x));
                    data = new ClassData(node.Element("Name").Value);

                    datas.Add(data);
                }
            }
            return datas;
        }
        #endregion
        //static void AddTable(string name, List<string> data)
        //{
        //    //data ->превращем в xml
        //    string[] com = name.Split('_');
        //    string[] allfolders = Directory.GetDirectories($"{mainPath}Group/");
        //    foreach (string folder in allfolders)
        //    {
        //        string[] com1 = folder.Split('/');
        //        string str = com1[com1.Length];
        //        if(str == com[0])
        //        {
        //            bool use = false;
        //            com1 = Directory.GetFiles($"{mainPath}Group/{str}/", "*.xml");//.xml
        //            foreach (string folders in com1)
        //            {
        //                Debug.Log(folders);
        //                use = (folders == com[1]);
        //                if (use)
        //                {
        //                    File.Delete(folders);
        //                    break;
        //                }
        //            }

        //            break;
        //        }
        //        //Console.WriteLine(str);
        //        //if(Directory.Exists(com1[]))
        //    }


        //    //0-data  1-time 2-dischiplin 3- prepod 4- audit
        //    XElement root = new XElement("root");
        //    foreach(string str in data)
        //        root.Add(new XElement("Data",str));

        //    //root.Add(new XElement("Time", data[1]));
        //    //root.Add(new XElement("Dischiplin", data[2]));
        //    //root.Add(new XElement("Prepod", data[3]));
        //    //root.Add(new XElement("Audit", data[4]));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText($"{mainPath}Group/{com[0]}/{com[1]}.xml", saveDoc.ToString());
        //    //node = XElement.Load(new XmlNodeReader(x));
        //    //ParametrData data = new ParametrData(node.Element("Name").Value);
        //    //data.Set(node.Element("Value").Value);

        //    //words.Add(data);
        //    //string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xls");//.xml

        //}
        ////сбрасывать расписание , если за час(30 мин) не взяли  ноутбук

        //static void Read()
        //{

        //}
        //static void RebuildTable()
        //{
        //    string time = System.DateTime.Today.ToString();
        //    //System.DateTime
        //    Debug.Log(time);
        //}
        /// <summary>
        /// Создать таблицу
        /// </summary>
        //private void CreateExcel()
        //{
        //    Debug.Log(1);
        //    wk = new HSSFWorkbook();
        //    sheet = wk.CreateSheet("mySheet");
        //    Debug.Log(2);

        //    for (int i = 0; i <= 20; i++)
        //    {
        //        row = sheet.CreateRow(i);
        //        cell = row.CreateCell(0);
        //        cell.SetCellValue(i);
        //    }

        //    fs = File.Create(filePath);
        //    wk.Write(fs);
        //    fs.Close();
        //    fs.Dispose();
        //    Debug.Log("Форма успешно создана");
        //}

        //private void LoadExcel()
        //{
        //    fs = File.OpenRead(filePath);
        //    wk = new HSSFWorkbook(fs);
        //    sheet = wk.GetSheetAt(0);
        //    for (int j = 1; j <= sheet.LastRowNum; j++)
        //    {
        //        row = sheet.GetRow(j);
        //        if (row != null)
        //        {
        //            for (int k = 0; k < row.LastCellNum; k++)
        //            {
        //                Debug.Log(row.GetCell(k).ToString());
        //            }
        //        }
        //    }
        //}
    }

    static class Saver
    {
        static string AddsString(List<stringData> sSub, string tayp,int id,bool Short = false)
        {
            string str = (Short) ? "" : "_";
            
            int k = sSub.FindIndex(x => x.Id == id);
            if (k != -1)
                str += $"_{sSub[k].Text}";
            else
            {
                stringData data = new stringData(id, Storage.GetName(tayp, id));
                str += $"_{data.Text}";
                sSub.Add(data);
            }

            return str;
        }
        static string mainPath = Application.dataPath + "/Dir/";
        public static void SaveGroup(GroupData group)
        {
            List<stringData> sSubject = new List<stringData>();
            List<stringData> sTeacher = new List<stringData>();
            List<stringData> sClass = new List<stringData>();

            XElement root = new XElement("root");
            //root.Add(new XElement("Name", story.Name));
            for (int i = 0; i < group.Teachers.Count; i++)
            {
                XElement action = new XElement("Teacher");
                string str = Storage.GetName("Teacher", group.Teachers[i].Name);
                foreach (int j in group.Teachers[i].Subjects)
                   str += AddsString(sSubject, "Subject", j);

                action.Add(new XElement("Text", str));
                root.Add(action);
            }

            foreach(TableData data in group.Subjects)
            {
                XElement action = new XElement("Subject");
                string str = "" + data.Time[0];
                for (int i = 1; i < data.Time.Length; i++)
                    str += "." + data.Time[i];
                action.Add("Time",str);

                action.Add("Subject", AddsString(sSubject, "Subject",data.Subject, true));
                action.Add("Teacher", AddsString(sTeacher, "Teacher", data.Teacher, true));
                action.Add("Class", AddsString(sClass, "Class", data.ClassRoom, true));


                root.Add(action);
            }
           

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Group/{group.Name}.xml", saveDoc.ToString());
        }

        public static void SaveClass(ClassData data)
        {
            XElement root = new XElement("root");


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}ClassRoom/{data.Name}.xml", saveDoc.ToString());
        }
        public static void SaveTeacher(TeacherData data)
        {
            XElement root = new XElement("root");

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Teacher/{data.Name}.xml", saveDoc.ToString());
        }
        public static void SaveSubject(SubjectData data)
        {
            XElement root = new XElement("root");

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Subject/{data.Name}.xml", saveDoc.ToString());
        }
    }
}
