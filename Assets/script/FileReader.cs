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
                RebuildTable();
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
        static void ReadTable(string path)
        {
            XmlDocument root = new XmlDocument();
            root.Load(path);//(mainPath + "Charter.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                TableData data = new TableData(node.Element("Time").Value);
                string[] com = node.Element("Time").Value.Split('.');
                //data.SetStartTime();
                //Debug.Log($"{data.StartTime[0]}-{data.StartTime[1]}");
              //  data.SetTime(com[0], false, false);
              //  data.SetTime(com[1], false, false);
              //  string str = "" + data.Day;

                //data.Set(node.Element("Data").Value);

                //   // words.Add(data);
            }
            //string[] origData = textassetData.text.Split
        }

        //private static HSSFWorkbook wk;
        //private static FileStream fs;          // Файловый поток

        //private static ISheet sheet;           // Рабочий лист
        //private static IRow row;               //Строка
        //private static ICell cell;             // Столбец

        // Start is called before the first frame update
        //public static void Starts()
        //{
        //    string[] com = { "AddTable" ,"Group","DataTime"};
        //    foreach(string path in com)
        //        if (!Directory.Exists($"{mainPath}{path}/"))
        //            Directory.CreateDirectory($"{mainPath}{path}/");
        //    ReadAddTable();
        //    //CreateExcel();
        //    RebuildTable();
        //}
        //static void ReadAddTable()
        //{
        //    string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xlsx");//.xml
        //    foreach(string path in com)
        //    {
        //        //string[] origData = textassetData.text.Split
        //        fs = File.OpenRead(path);
        //        wk = new HSSFWorkbook(fs);
        //        sheet = wk.GetSheetAt(0);

        //        List<string> data = new List<string>();
        //        for (int j = 1; j <= sheet.LastRowNum; j++)
        //        {
        //            //чтение содержимого файла
        //            //0-data  1-time 2-dischiplin 3- prepod 4- audit

        //            row = sheet.GetRow(j);
        //            if (row != null)
        //            {
        //                string str = row.GetCell(0).ToString();
        //                for (int k = 1; k < row.LastCellNum; k++)
        //                    str +="|"+ row.GetCell(k).ToString();
        //                data.Add(str);
        //            }
        //        }
        //        AddTable(path,data);//com- system anme, data - table info
        //    }

        //    //Sys.CreateNewRaspis();
        //}


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
}
