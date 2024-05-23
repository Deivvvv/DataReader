using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Xls;
using System.IO;

using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// учет учеников
        /// Главная точка входа для приложения.
        /// </summary>
        /// 

        static string mood = "unit";//"post-pk"-обработка листов зявок приеной комисии;
        [STAThread]
        static void Main()
        {
           // mood = "unit";
            switch (mood)
            {
                case ("post-pk"):
                    Use();
                    break;
                case ("converter"):
                    Convert();
                    break;
                case ("BBRead"):
                    BBRead();
                    break;
                case ("unit"):
                    ConverterUnitList();
                    break;
            }
            return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }

        struct users
        {
            public users(string name, int size)
            {
                Name = name;
                Size = new string[size];
            }
            public string Name;
            public string[] Size;

        }
        static void BBRead()
        {
            int id=1;
            Workbook doc = new Workbook();
            Worksheet dsheet = doc.Worksheets[0];
            dsheet.Range[id, 1].Value = "наименование основное";

            dsheet.Range[id, 2].Value = "наименование дополнительное(код)(при наличии)";
            dsheet.Range[id, 3].Value = "инвентарный/номенклатурный";
            dsheet.Range[id, 4].Value = "Тип баланса";
            id++; 

            string[] coms = Directory.GetFiles($"doc/b/", "*.xls");
            Console.WriteLine(coms.Length);
            foreach (string path in coms)
            {
                Workbook workbook = new Workbook();
                Console.WriteLine(path);
                workbook.LoadFromFile(path);
                Worksheet sheet = workbook.Worksheets[0];
                string tayp = sheet.Range[1, 1].Value;
                //1-имя 3-код бухгалтерии 9- инвертарник
                int x = 0;
                while (  sheet.Range[24 + x, 1].Value.Length > 1)
                {
                    dsheet.Range[id, 1].Value = sheet.Range[24 + x, 1].Value;
                    
                    dsheet.Range[id, 2].Value = "_" + sheet.Range[24 + x, 3].Value;
                    dsheet.Range[id, 3].Value ="_"+ sheet.Range[24 + x, 9].Value;
                    dsheet.Range[id, 4].Value = tayp;
                    x++;
                    id++;
                }
                Console.WriteLine(sheet.Range[23 + x, 1].Value);
                Console.WriteLine(sheet.Range[24 + x, 1].Value);
                Console.WriteLine(sheet.Range[25 + x, 1].Value);


                id++;
            }


            dsheet.AllocatedRange.AutoFitColumns();

            //Применение стиля к первой строке
            CellStyle style = doc.Styles.Add("newStyle");
            style.Font.IsBold = true;
            dsheet.Range[1, 1, 1,  4].Style = style;

            //Сохранение в файл Excel
            doc.SaveToFile("C\\fnish.xlsx", ExcelVersion.Version2016);
        }

        struct MultiUnit
        {
            public string Name;
            public List<UnitData> units;

            public MultiUnit(string name)
            {
                Name = name;
                units = new List<UnitData>();
            }
            public void AddUnit(string str1, string str2, string str3, string str4)
            {
                int id = units.FindIndex(x => x.Name == str1);
                if(id == -1)
                {
                    UnitData data = new UnitData(str1);
                    id = units.Count;
                    units.Add(data);
                }

                units[id].AddUnit(str2, str3, str4);
            }
        }
        struct UnitData
        {
            public string Name;
            public List<string> BNum;
            public List<string> INum;
            public List<string> Tayp;
            public UnitData(string name)
            {
                Name = name;
                BNum = new List<string>();
                INum = new List<string>();
                Tayp = new List<string>();
            }
            public void AddUnit(string str2, string str3, string str4)
            {
                BNum.Add(str2);
                INum.Add(str3);
                Tayp.Add(str4);
            }
        }
        static void ConverterUnitList()
        {
            List<MultiUnit> multiUnits = new List<MultiUnit>();

            string[] coms = Directory.GetFiles($"UnitList/", "*.xlsx");
            foreach (string path in coms)
            {

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);
                for(int z =0;z< workbook.Worksheets.Count;z++)
                {
                
                    Worksheet sheet = workbook.Worksheets[z];

                    int x = 0;
                    while (sheet.Range[1 + x, 1].Value.Length > 1)
                    {
                        int id = multiUnits.FindIndex(j=>j.Name == sheet.Range[1 + x, 5].Value);
                        if(id == -1)
                        {
                            id = multiUnits.Count;
                            multiUnits.Add(new MultiUnit(sheet.Range[1 + x, 5].Value));
                        }
                        multiUnits[id].AddUnit(sheet.Range[1 + x, 1].Value, sheet.Range[1 + x, 2].Value, sheet.Range[1 + x, 3].Value, sheet.Range[1 + x, 4].Value);


                        x++;
                    }
                }

            }

            XElement root = new XElement("root");
            for(int i = 0; i < multiUnits.Count; i++)
            {
                XElement action = new XElement("TaypG");
                action.Add(new XElement("Name", multiUnits[i].Name));//преподаватель
                for(int j = 0; j < multiUnits[i].units.Count; j++)
                {

                    XElement uAction = new XElement("Unit");
                    uAction.Add(new XElement("Name", multiUnits[i].units[j].Name));
                    for(int k =0;k< multiUnits[i].units[j].BNum.Count; k++)
                    {

                        XElement uAction1 = new XElement("Units");
                        uAction1.Add(new XElement("BNum", multiUnits[i].units[j].BNum[k]));
                        uAction1.Add(new XElement("INum", multiUnits[i].units[j].INum[k]));
                        uAction1.Add(new XElement("Tayp", multiUnits[i].units[j].Tayp[k]));

                        uAction.Add(uAction1);
                    }
                    action.Add(uAction);
                }
                root.Add(action);
            }


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"output/_units_.xml", saveDoc.ToString());

        }
        static void Convert()
        {
            string[] coms = Directory.GetFiles($"input/", "*.xlsx");
            string[] sort = { "К.и.н.", "доц.", "Д.и.н.", "К.ф.н.", "К.э.н.", "К.ю.н.", "К.т.н." };
            //К.и.н. доц.
            foreach (string path in coms)
            {

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);

                    Worksheet sheet = workbook.Worksheets[0];
                    int x = 0;
                    XElement root = new XElement("root");

                    string str = "", data = "";
                    string[] com;
                    while (sheet.Range[1 + x, 1].Value.Length > 1 || sheet.Range[1 + x, 2].Value.Length > 1)
                    {
                        bool use = false;
                        XElement action = new XElement("Action");

                        com = sheet.Range[2 + x, 1].Value.Split(' ');
                        str = com[0];
                        if (str == " " || str == "")
                            str = data;
                        else
                            data = str;

                        com = sheet.Range[2 + x, 2].Value.Split('-');
                        foreach (string str1 in com)
                            str += "." + str1;

                        action.Add(new XElement("Time", str));
                        //action.Add(new XElement("StartTime", "-"));
                        //action.Add(new XElement("EndTime", "-"));

                        action.Add(new XElement("Subject", sheet.Range[2 + x, 3].Value));//занятие
                        com = sheet.Range[2 + x, 4].Value.Split(' ');
                        for (int i = 0; i < com.Length; i++)
                        {
                            if (!use)
                            {
                                foreach (string sorts in sort)
                                {
                                    use = (com[i] == sorts);
                                    if (use)
                                        break;
                                }
                                if (use)
                                {
                                    use = false;
                                    continue;
                                }

                                str = com[i];
                                use = true;
                                continue;
                            }
                            else
                                str += $" {com[i]}";
                        }

                        action.Add(new XElement("Teacher", str));//преподаватель
                        action.Add(new XElement("ClassRoom", sheet.Range[2 + x, 5].Value));//кабинет

                        x++;
                        root.Add(action);
                    }

                    str = sheet.Range[1, 1].Value.ToUpper();
                    str += "_" + sheet.Range[1, 2].Value.ToUpper();

                    str += "_";
                    str += (sheet.Range[1, 3].Value[0] == '.') ? sheet.Range[1, 3].Value.Substring(1) : sheet.Range[1, 3].Value;


                    XDocument saveDoc = new XDocument(root);
                    File.WriteAllText($"output/_{str}.xml", saveDoc.ToString());
                    //format//Group//Year
                 
            }
        }
        static void Use()
        {
            List<users> uList = new List<users>();
            //Создание объекта Workbook 

            string[] coms = Directory.GetFiles($"doc/", "*.xlsx");
            string[] names = new string[coms.Length];
            for (int i = 0; i < coms.Length; i++)
            {
                string[] ls = coms[i].Split('~');
                if(ls.Length > 1)
                {
                    string[] ls1 = ls[1].Split('$');
                    coms[i] = ls[0] + ls1[1];
                    names[i] = ls1[1];
                }
                else
                {
                    string[] ls1 = ls[0].Split('/');
                    names[i] = ls1[1];
                }

                ls = names[i].Split('.');
                names[i] = ls[0];

                Workbook workbook = new Workbook();
                Console.WriteLine(coms[i]);
                workbook.LoadFromFile(coms[i]);

                Worksheet sheet = workbook.Worksheets[0];
                int x = 0;
                while (sheet.Range[ 2 + x,1].Value.Length >1 )
                {
                    string str = sheet.Range[ 2 + x,1].Value + " " + sheet.Range[ 2 + x,2].Value;
                    string str1 = sheet.Range[ 2 + x,8].Value;
                    ls = str1.Split(' ');
                   // Console.WriteLine($"{coms[i] } {str}  {str1} {ls.Length}");
                    str1 = ls[2];
                    if (str1[str1.Length-1] != '2') 
                    {
                        int idx = uList.FindIndex(h => h.Name == str);
                        if (idx != -1)
                            uList[idx].Size[i] = sheet.Range[ 2 + x,11].Value;
                        else
                        {
                            users u = new users(str, coms.Length);
                            //Console.WriteLine($"{i} {com.Length} {j} {com1.Length} {com[0]}");
                            u.Size[i] = sheet.Range[ 2 + x,11].Value;
                            uList.Add(u);
                        } 
                    }
                    x++;
                }

                Console.WriteLine($"{x}");
               
            }

            {

                Workbook workbook = new Workbook();

                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Range[1, 1].Value = "Имя";
                for(int i=0;i< names.Length; i++)
                    worksheet.Range[1, 2+i].Value = names[i];


                for (int i = 0; i < uList.Count; i++)
                {
                    worksheet.Range[i + 2, 1].Value = uList[i].Name;
                    for (int j = 0; j < names.Length; j++)
                        worksheet.Range[i + 2, 2 + j].Value = uList[i].Size[j];

                }

                //Автоматическое подгонка ширины столбцов
                worksheet.AllocatedRange.AutoFitColumns();

                //Применение стиля к первой строке
                CellStyle style = workbook.Styles.Add("newStyle");
                style.Font.IsBold = true;
                worksheet.Range[1, 1, 1, names.Length + 1].Style = style;

                //Сохранение в файл Excel
                workbook.SaveToFile("C\\fnish.xlsx", ExcelVersion.Version2016);

            }


            //List<users> uList = new List<users>();


            //worksheet.Range[1, 1].Value = "Имя";
            //for (int i =0;i< listName.Count;i++)
            //{
            //    worksheet.Range[1, i + 2].Value = listName[i];
            //    string[] com = lists[i].Split('|');
            //    string[] com1 = listsNums[i].Split('|');
            //    for (int j = 0; j < com.Length; j++)
            //    {
            //        //Console.WriteLine($"{lists} {j}");
            //        int id = uList.FindIndex(x => x.Name == com[j]);
            //        if (id != -1)
            //            uList[id].Size[i] = com1[j];
            //        else
            //        {
            //            users u = new users(com[j], listName.Count);
            //            //Console.WriteLine($"{i} {com.Length} {j} {com1.Length} {com[0]}");
            //            u.Size[i] = com1[j];
            //            uList.Add(u);
            //        }

            //    }

            //    Console.WriteLine($"{i} {com.Length} {com1.Length} {listName[i]}");

            //}

            //for (int i = 0; i < uList.Count; i++)
            //{
            //    worksheet.Range[i+2, 1].Value = uList[i].Name;
            //    for(int j=0;j<listName.Count;j++)
            //        worksheet.Range[i + 2, 2+j].Value = uList[i].Size[j];

            //}



            //////Запись данных в определенные ячейки
            ////worksheet.Range[1, 1].Value = "Имя";
            ////worksheet.Range[1, 2].Value = "Возраст";
            ////worksheet.Range[1, 3].Value = "Департамент";
            ////worksheet.Range[1, 4].Value = "Дата найма";
            ////worksheet.Range[1, 2].Value = "Хейзел";
            ////worksheet.Range[2, 2].Value2 = 29;
            ////worksheet.Range[2, 3].Value = "Маркетинг";
            ////worksheet.Range[2, 4].Value = "2019-07-01";
            ////worksheet.Range[3, 1].Value = "Тина";
            ////worksheet.Range[3, 2].Value2 = 31;
            ////worksheet.Range[3, 3].Value = "Техническая поддержка";
            ////worksheet.Range[3, 4].Value = "2015-04-27";

            ////Автоматическое подгонка ширины столбцов
            //worksheet.AllocatedRange.AutoFitColumns();

            ////Применение стиля к первой строке
            //CellStyle style = workbook.Styles.Add("newStyle");
            //style.Font.IsBold = true;
            //worksheet.Range[1, 1, 1, listName.Count+1].Style = style;

            ////Сохранение в файл Excel
            //workbook.SaveToFile("C\\fnish.xlsx", ExcelVersion.Version2016);
            Console.WriteLine("щл");


        }
    }
}
