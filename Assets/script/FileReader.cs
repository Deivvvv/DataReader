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
            string[] com = { "AddTable", "Data/Group", "Data/Unit", "Data/Class", "Data/Teacher", "Data/Year" };
            foreach (string path in com)
                if (!Directory.Exists($"{mainPath}{path}/"))
                    Directory.CreateDirectory($"{mainPath}{path}/");

            Storage.StartSystem();

            //if (AddTableScan())
            //    AddData();

            /*
             load
            class room
            subject
            teacher <-classroom <-subject
             
             */
        }
        public static List<UnitData> LoadUnit()
        {
            List<UnitData> datas = new List<UnitData>();
            string[] com = Directory.GetFiles($"{mainPath}Data/Unit/", "*.xml");//.xml

            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);//(mainPath + "Charter.xml");
                string[] com1 = com[1].Split('/');
                com1 = com1[com1.Length - 1].Split('.');
                UnitData data = new UnitData(com1[0]);

                //nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                //foreach (XmlNode x in nodes)
                //{
                //    node = XElement.Load(new XmlNodeReader(x));
                //    TableData data = new TableData(node.Element("Time").Value);
                //    if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                //        continue;

                //    //data.SetRealTime(node.Element("StartTime").Value, true);
                //    //data.SetRealTime(node.Element("EndTime").Value, false);
                //    Storage.ConnectData(data);
                //    data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                //    data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                //    data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                //    tableDatas.Add(data);
                //}
                datas.Add(data);
            }

            return datas;
        }
        public static List<ClassData> LoadClass()
        {
            List<ClassData> datas = new List<ClassData>();
            string[] com = Directory.GetFiles($"{mainPath}Data/Class/", "*.xml");//.xml

            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);//(mainPath + "Charter.xml");
                string[] com1 = com[1].Split('/');
                com1 = com1[com1.Length - 1].Split('.');
                ClassData data = new ClassData(com1[0]);
                //nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                //foreach (XmlNode x in nodes)
                //{
                //    node = XElement.Load(new XmlNodeReader(x));
                //    TableData data = new TableData(node.Element("Time").Value);
                //    if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                //        continue;

                //    //data.SetRealTime(node.Element("StartTime").Value, true);
                //    //data.SetRealTime(node.Element("EndTime").Value, false);
                //    Storage.ConnectData(data);
                //    data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                //    data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                //    data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                //    tableDatas.Add(data);
                //}
                datas.Add(data);
            }

            return datas;
        }
        //LoadSubject
        public static List<SubjectData> LoadSubject()
        {
            List<SubjectData> datas = new List<SubjectData>();
            string[] com = Directory.GetFiles($"{mainPath}Data/Class/", "*.xml");//.xml

            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);//(mainPath + "Charter.xml");
                string[] com1 = com[1].Split('/');
                com1 = com1[com1.Length - 1].Split('.');
                SubjectData data = new SubjectData(com1[0]);
                //nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                //foreach (XmlNode x in nodes)
                //{
                //    node = XElement.Load(new XmlNodeReader(x));
                //    TableData data = new TableData(node.Element("Time").Value);
                //    if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                //        continue;

                //    //data.SetRealTime(node.Element("StartTime").Value, true);
                //    //data.SetRealTime(node.Element("EndTime").Value, false);
                //    Storage.ConnectData(data);
                //    data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                //    data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                //    data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                //    tableDatas.Add(data);
                //}
                datas.Add(data);
            }

            return datas;
        }
        public static List<TeacherData> LoadTeacher()
        {
            List<TeacherData> datas = new List<TeacherData>();
            string[] com = Directory.GetFiles($"{mainPath}Data/Teacher/", "*.xml");//.xml

            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);//(mainPath + "Charter.xml");
                string[] com1 = path.Split('/');
                com1 = com1[com1.Length - 1].Split('.');
                TeacherData data = new TeacherData(com1[0]);
                //nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                //foreach (XmlNode x in nodes)
                //{
                //    node = XElement.Load(new XmlNodeReader(x));
                //    TableData data = new TableData(node.Element("Time").Value);
                //    if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                //        continue;

                //    //data.SetRealTime(node.Element("StartTime").Value, true);
                //    //data.SetRealTime(node.Element("EndTime").Value, false);
                //    Storage.ConnectData(data);
                //    data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                //    data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                //    data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                //    tableDatas.Add(data);
                //}
                datas.Add(data);
            }

            return datas;
        }

        public static List<int> GetYears()
        {
            string[] allfolders = Directory.GetDirectories($"{mainPath}Data/Year/");
            List<int> com = new List<int>();
            for(int i=0;i < allfolders.Length; i++)
            {
                string[] com1 = allfolders[i].Split('/');
                com.Add(int.Parse(com1[com1.Length - 1]));
            }

            return com;
        }
        public static List<int> GetMouths(int num)
        {
            List<int> com = new List<int>();
            string path = $"{mainPath}Data/Year/{num}/";
            if (!Directory.Exists(path))
                return com;

            string[] allfolders = Directory.GetDirectories(path);
            for (int i = 0; i < allfolders.Length; i++)
            {
                string[] com1 = allfolders[i].Split('/');

                com.Add(int.Parse(com1[com1.Length - 1]));
            }

            return com;
        }
        public static List<int> GetDays(int num1, int num2)
        {
            List<int> com = new List<int>();
            string path = $"{mainPath}Data/Year/{num1}/";
            if (!Directory.Exists(path))
                return com;

            path += $"{num2}/";
            if (!Directory.Exists(path))
                return com;

            string[] allfolders = Directory.GetFiles(path, "*.xml");//.xml
            for (int i = 0; i < allfolders.Length; i++)
            {
                string[] com1 = allfolders[i].Split('/');

                com.Add(int.Parse(com1[com1.Length - 1]));
            }

            return com;
        }
        public static void LoadTableGroup()
        {
            string[] com = Directory.GetFiles($"{mainPath}AddTable/Group/", "*.xml");//.xml
            XmlDocument root = new XmlDocument();
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes; // You can also use XPath here
            XmlNodeList nodes1; // You can also use XPath here
            XmlNodeList nodes2; // You can also use XPath here
            foreach (string path in com)
            {
                root.Load(path);
                string[] com1 = path.Split('_');
                com1 = com1[1].Split('.');
                int group = Storage.FindId("Group", com1[0]);
                int groupName = Storage.FindIdGroup(group, com1[1]);
                List<TableData> datas = new List<TableData>();
                string dataTime = "";

                Debug.Log(dataTime);
                nodes = root.DocumentElement.SelectNodes("descendant::DocumentProperties"); // You can also use XPath here
                Debug.Log(nodes.Count);
                foreach (XmlNode x in nodes)
                {

                    // node = XElement.Load(new XmlNodeReader(x));

                    Debug.Log(dataTime);
                    nodes1 = x.SelectNodes("descendant::Row"); // You can also use XPath here
                    foreach (XmlNode z in nodes)
                    {
                        int i = 0;
                        TableData data = null;
                        nodes2 = z.SelectNodes("descendant::Cell"); // You can also use XPath here
                        if (nodes.Count == 5) {
                            dataTime = XElement.Load(new XmlNodeReader(nodes2[0])).Element("Data").Value;
                            i++;
                        }
                        Debug.Log(dataTime);
                        data = new TableData(dataTime);
                        //for (; i < nodes2.Count; i++)
                        //{
                        //    node = XElement.Load(new XmlNodeReader(nodes2[i]));
                        //    node.Element("Data").Value
                        //}
                        datas.Add(data);
                    }


                    //    data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                    //data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                    //data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                    //datas.Add(data);
                }


               // Storage.GroupSetTable(group, groupName, datas);

            }

                //Worksheet
                //Row
                //Cell

                //string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xml");//.xml

                //foreach (string path in com)
                //{
                //    Debug.Log(path);
                //    string[] com1 = path.Split('_');//com[0] - путь до исходного файла // com[1+] |table|gmu|05-2023
                //    string newPath = $"{mainPath}{com1[1]}/{com1[2]}/";
                //    //com1 = com1[2].Split('.');
                //    //newPath += com1[0];//$"{ com1[0]}/";
                //    if (!Directory.Exists($"{newPath}"))
                //        Directory.CreateDirectory($"{newPath}");
                //    //File.Move(path,$"{newPath}{com1[3]}");
                //}
                //Debug.Log(com.Length);
                //return (com.Length > 0);
        }
        static void LoadUnits()
        {

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
                Debug.Log(com1[1]);
                switch (com1[1])
                {
                    //case ("Class"):
                    //    File.Move(path, $"{newPath}{com1[3]}");
                    //    break;
                    case ("TABLE")://GroupTable
                        i = Storage.FindId("Group", com1[2]);
                        int a = Storage.FindIdGroup(i, com1[3]);
                        {
                            
                            com1 = System.DateTime.Today.ToString().Split(' ');
                            int[] timeData = com1[0].Split('.').Select(int.Parse).ToArray();
                            //int[] timeData = com1.Select(int.Parse).ToArray();
                            List<TableData> tableDatas = new List<TableData>();
                            nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
                            foreach (XmlNode x in nodes)
                            {
                                node = XElement.Load(new XmlNodeReader(x));
                                TableData data = new TableData(node.Element("Time").Value);
                                //if (timeData[0] > data.Time[0] && timeData[1] > data.Time[1])
                                //    continue;

                                //data.SetRealTime(node.Element("StartTime").Value, true);
                                //data.SetRealTime(node.Element("EndTime").Value, false);
                                // Storage.ConnectData(data);
                                data.Subject = Storage.FindId("Subject", node.Element("Subject").Value);
                                data.Teacher = Storage.FindId("Teacher", node.Element("Teacher").Value);
                                data.ClassRoom = Storage.FindId("Class", node.Element("ClassRoom").Value);

                                tableDatas.Add(data);
                            }
                            Storage.GroupSetTable(i,a,tableDatas);
                        }
                        Debug.Log($"{i} {a}");

                        Storage.SaveGroup(i, a);
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
                //File.Move(path,$"{newPath}{com1[3]}");
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
        public static void LoadGroup()
        {
            string[] com1;
            string[] allfolders = Directory.GetDirectories($"{mainPath}Data/Group/");
            string[] com = new string[allfolders.Length];
            for (int i = 0; i < com.Length; i++)
            {
                com1 = allfolders[i].Split('/');
                com[i] = com1[com1.Length - 1];
                Debug.Log(com[i]);
            }
            Storage.SetMainGroup(com);

            /*
             порядок загрузки
            классы
            дисциплины
            преподаватели

            группы - можно использовать многопоточность
             */

            for (int i = 0; i < allfolders.Length; i++)
            {
                com = Directory.GetFiles(allfolders[i], "*.xml");
                for (int j = 0; j < com.Length; j++)
                {
                    com1 = com[j].Split('/');
                    com1 = com1[com1.Length - 1].Split('.');
                    Storage.GroupAdd(i, LoadGroup(i,com[j], com1[0]));
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

        static GroupData LoadGroup(int zi,string path, string name)
        {

            List<stringData> sSubject = new List<stringData>();
            List<stringData> sTeacher = new List<stringData>();
            List<stringData> sClass = new List<stringData>();

            GroupData group = new GroupData(zi,name);
            XmlDocument root = new XmlDocument();
            root.Load(path);
            XElement node;
            XmlNodeList nodes;

            group.Teachers = new List<GroupData.TeacherList>();
            nodes = root.DocumentElement.SelectNodes("descendant::TTeacher");
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                string[] com = node.Element("Text").Value.Split('_');
                int[] id = new int[com.Length - 1];
                for (int i = 1; i < com.Length; i++)
                    id[i - 1] = AddsString(sSubject, "Subject", com[i]);

                GroupData.TeacherList data = new GroupData.TeacherList(AddsString(sTeacher, "Teacher", com[0]), id);

                group.Teachers.Add(data);
            }

            List<TableData> tData = new List<TableData>();
            nodes = root.DocumentElement.SelectNodes("descendant::SSubject"); 
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                TableData data = new TableData(node.Element("Time").Value);
                //data.SetRealTime(node.Element("StartTime").Value, true);
                //data.SetRealTime(node.Element("EndTime").Value, false);
               // Storage.ConnectData(data);
                data.Subject = AddsString(sSubject,"Subject", node.Element("Subject").Value);
                data.Teacher = AddsString(sTeacher,"Teacher", node.Element("Teacher").Value);
                data.ClassRoom = AddsString(sClass,"Class", node.Element("Class").Value);

                tData.Add(data);
            }
            group.SetTable(tData);

            return group;
        }

        //static void ReadTable(string path)
        //{

        //    TableData data = null;
        //    string[] com;
        //    int[] ints = new int[2];
        //    com = path.Split('/');
        //    ints[0] = Storage.FindId("MainGroup", com[com.Length - 2]);
        //    com = com[com.Length - 1].Split('.');
        //    ints[1] = Storage.FindId($"{ints[0]}_Group", com[0]);

        //    XmlDocument root = new XmlDocument();
        //    root.Load(path);//(mainPath + "Charter.xml");
        //    XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
        //    XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
        //    foreach (XmlNode x in nodes)
        //    {
        //        node = XElement.Load(new XmlNodeReader(x));
        //        data = new TableData(node.Element("Time").Value, com[0]);
        //        data.SetRealTime(node.Element("StartTime").Value, true);
        //        data.SetRealTime(node.Element("EndTime").Value, false);
        //        Storage.ConnectData(data);
        //        data.Subject = Storage.FindId("Subject",node.Element("Subject").Value);
        //        data.Teacher = Storage.FindId("Teacher",node.Element("Teacher").Value);
        //        data.ClassRoom = Storage.FindId("Class", node.Element("Class").Value);
        //    }

        //    if(data != null)
        //        Storage.SaveGroup(data.Group);
        //    //string[] origData = textassetData.text.Split
        //}

        
    }

    static class Saver
    {
        static string AddsString(List<stringData> sSub, string tayp,int id,bool Short = false)
        {
            string str = (Short) ? "" : "_";
            
            int k = sSub.FindIndex(x => x.Id == id);
            if (k != -1)
                str += $"{sSub[k].Text}";
            else
            {
                stringData data = new stringData(id, Storage.GetName(tayp, id));
                str += $"{data.Text}";
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
                XElement action = new XElement("TTeacher");
                string str = Storage.GetName("Teacher", group.Teachers[i].Name);
                foreach (int j in group.Teachers[i].Subjects)
                   str += AddsString(sSubject, "Subject", j);

                action.Add(new XElement("Text", str));
                root.Add(action);
            }

            foreach(TableData data in group.Subjects)
            {
                XElement action = new XElement("SSubject");
                string str = "" + data.Time[0];
                for (int i = 1; i < data.Time.Length; i++)
                    str += "." + data.Time[i];
                action.Add(new XElement("Time",str));

                action.Add(new XElement("Subject", AddsString(sSubject, "Subject",data.Subject, true)));
                action.Add(new XElement("Teacher", AddsString(sTeacher, "Teacher", data.Teacher, true)));
                action.Add(new XElement("Class", AddsString(sClass, "Class", data.ClassRoom, true)));


                root.Add(action);
            }

            string str1 = Storage.GetName("Group", group.MainGroup);
            str1 = $"{ mainPath}Data/Group/{str1}/";
            if (!Directory.Exists(str1))
                Directory.CreateDirectory(str1);

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{str1}/{group.Name}.xml", saveDoc.ToString());
        }

        public static void SaveClass(ClassData data)
        {
            XElement root = new XElement("root");


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Data/Class/{data.Name}.xml", saveDoc.ToString());
        }
        public static void SaveTeacher(TeacherData data)
        {
            XElement root = new XElement("root");

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Data/Teacher/{data.Name}.xml", saveDoc.ToString());
        }
        public static void SaveSubject(SubjectData data)
        {
            XElement root = new XElement("root");

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Data/Subject/{data.Name}.xml", saveDoc.ToString());
        }
    }
}
