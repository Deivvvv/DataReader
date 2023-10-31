using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using TableSystem;
namespace FileReader
{ 
    static class Reader
    {
        //private static HSSFWorkbook wk;
        //private static FileStream fs;          // Файловый поток

        //private static ISheet sheet;           // Рабочий лист
        //private static IRow row;               //Строка
        //private static ICell cell;             // Столбец

        static string mainPath = Application.dataPath + "/Dir/";
        // Start is called before the first frame update
        public static void Starts()
        {
            string[] com = { "AddTable" ,"Group","DataTime"};
            foreach(string path in com)
                if (!Directory.Exists($"{mainPath}{path}/"))
                    Directory.CreateDirectory($"{mainPath}{path}/");
            ReadAddTable();
            //CreateExcel();
            RebuildTable();
        }
        static void ReadAddTable()
        {
            string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xlsx");//.xml
            foreach(string path in com)
            {
                //string[] origData = textassetData.text.Split
                fs = File.OpenRead(path);
                wk = new HSSFWorkbook(fs);
                sheet = wk.GetSheetAt(0);

                List<string> data = new List<string>();
                for (int j = 1; j <= sheet.LastRowNum; j++)
                {
                    //чтение содержимого файла
                    //0-data  1-time 2-dischiplin 3- prepod 4- audit

                    row = sheet.GetRow(j);
                    if (row != null)
                    {
                        string str = row.GetCell(0).ToString();
                        for (int k = 1; k < row.LastCellNum; k++)
                            str +="|"+ row.GetCell(k).ToString();
                        data.Add(str);
                    }
                }
                AddTable(path,data);//com- system anme, data - table info
            }

            //Sys.CreateNewRaspis();
        }


        static void AddTable(string name, List<string> data)
        {
            //data ->превращем в xml
            string[] com = name.Split('_');
            string[] allfolders = Directory.GetDirectories($"{mainPath}Group/");
            foreach (string folder in allfolders)
            {
                string[] com1 = folder.Split('/');
                string str = com1[com1.Length];
                if(str == com[0])
                {
                    bool use = false;
                    com1 = Directory.GetFiles($"{mainPath}Group/{str}/", "*.xml");//.xml
                    foreach (string folders in com1)
                    {
                        Debug.Log(folders);
                        use = (folders == com[1]);
                        if (use)
                        {
                            File.Delete(folders);
                            break;
                        }
                    }

                    break;
                }
                //Console.WriteLine(str);
                //if(Directory.Exists(com1[]))
            }


            //0-data  1-time 2-dischiplin 3- prepod 4- audit
            XElement root = new XElement("root");
            foreach(string str in data)
                root.Add(new XElement("Data",str));

            //root.Add(new XElement("Time", data[1]));
            //root.Add(new XElement("Dischiplin", data[2]));
            //root.Add(new XElement("Prepod", data[3]));
            //root.Add(new XElement("Audit", data[4]));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Group/{com[0]}/{com[1]}.xml", saveDoc.ToString());
            //node = XElement.Load(new XmlNodeReader(x));
            //ParametrData data = new ParametrData(node.Element("Name").Value);
            //data.Set(node.Element("Value").Value);

            //words.Add(data);
            //string[] com = Directory.GetFiles($"{mainPath}AddTable/", "*.xls");//.xml

        }
        //сбрасывать расписание , если за час(30 мин) не взяли  ноутбук

        static void Read()
        {

        }
        static void RebuildTable()
        {
            string time = System.DateTime.Today.ToString();
            //System.DateTime
            Debug.Log(time);
        }
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
    class TableData
    {

        //0-data  1-time 2-dischiplin 3- prepod 4- audit
    }
}
