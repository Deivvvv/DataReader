using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FileReader;

namespace DataSpace
{
    
    class ClassData : DataForm
    {
        public List<UnitData> unitList;
        public string WorldMark;//ссылка на маршрут к кабинету(можно напсиать программу навигации)
        public ClassData(string name)
        {
            Name = name;
        }
    }
    public class UnitData : DataForm
    {
        public struct IdCase
        {
            public string BNum;//бух номер
            public string INum;//инв. номер
            public int Tayp;//тип инвентарника

            public int Position;
            public int User;
            public int Status;
            //public List<string> note;
            public IdCase(string str1, string str2, int i)
            {
                BNum = str1;
                INum = str2;
                Tayp = i;

                Position = -1;
                User = -1;
                Status = -1;
            }
        }
        public struct MainIdCase{
            public string Name;
            public List<IdCase> Units;
            public MainIdCase(string str1)
            {
                Name = str1;
                Units = new List<IdCase>();
                //IdCase id = new IdCase()
            }
        }

        //фактическое оборудование
        //ClassData
        public List<MainIdCase> Ids;//оборудование такого типа
        public UnitData(string name)
        {
            Name = name;
            Ids = new List<MainIdCase>();
        }

    }
    class TeacherData : DataForm
    {

        public List<SubjectData> subjects;
        public List<UnitData> Units;
        public List<GroupData> Groups;

        public TeacherData(string name)
        {
            Name = name;
        }
    }
    public class GroupData : DataForm
    {

        public List<TeacherList> Teachers;
        public struct TeacherList
        {
            public int Name;
            public List<int> Subjects;
            public TeacherList(int name, int[] ints)
            {
                Name = name;
                Subjects = new List<int>(ints);
            }
            public void AddSub(int id)
            {
                int i = Subjects.FindIndex(x => x == id);
                if (i == -1)
                    Subjects.Add(i);
            }

        }
        public int MainGroup;
        public List<TableData> Subjects;
        public GroupData (int i,string name)
        {
            Name = name;
            MainGroup = i;
        }
        public void SetTable(List<TableData> subjects)
        {
            Subjects = subjects;
            List<TeacherList> teach = new List<TeacherList>();
            for(int i = 0; i < subjects.Count; i++)
            {
                int b = Teachers.FindIndex(x => x.Name == subjects[i].Teacher);
                if( b== -1)
                {
                    b = Teachers.Count;
                    Teachers.Add(new TeacherList(subjects[i].Teacher, new int[0]));
                }
                int a = Teachers[b].Subjects.FindIndex(x => x == Subjects[i].Subject);
                if(a == -1)
                {
                    Teachers[b].Subjects.Add(Subjects[i].Subject);
                }
            }
        }
    }
    class SubjectData: DataForm
    {
        public List<TeacherData> Data;
        public SubjectData(string name)
        {
            Name = name;
        }
    }
    public class DataForm
    {
        public string Name;
      
    }
    public static class Storage
    {
        public static intM zeroIntM = new intM(0);

        public struct MainGroup
        {
            public string Name;
            public List<GroupData> Groups;

            public MainGroup(string name)
            {
                Name = name;
                Groups = new List<GroupData>();
            }
            public void AddGroup(GroupData data)
            {
                Groups.Add(data);
            }
        }

        //static List<string> mainGroup;
        static List<TeacherData> teachers;
        static List<UnitData> units;
        static List<ClassData> classs;
        static List<MainGroup> groups;
        static List<SubjectData> subjects;
        private static int groupInt;
        static List<TableData> sendData;
        //private static TableData cData;
        public static void StartSystem()
        {
            units = Reader.LoadUnit();
            List<UnitData> unitsDPO = Reader.LoadUnits();
            for (int i = 0; i < unitsDPO.Count; i++)
                units.Add(unitsDPO[i]);
            classs = Reader.LoadClass();
            subjects = Reader.LoadSubject();
            teachers = Reader.LoadTeacher();

            Reader.LoadGroup();

        }
        public static string[] GetGroupList()
        {
            string[] com = new string[groups.Count];
            for(int i = 0; i < groups.Count; i++)
            {
                com[i] = groups[i].Name;
            }
            return com;
        }
        public static string[] SwicthGroup(int xi)
        {
            groupInt = xi;
            string[] com = new string[groups[xi].Groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                com[i] = groups[xi].Groups[i].Name;
            }
            return com;

        }

        public static List<TableData> GetAllTable()
        {
            UnityEngine.Debug.Log("!");
            sendData = new List<TableData>();
            for (int i = 0; i < groups.Count; i++)
                for (int j = 0; j < groups[i].Groups.Count; j++)
                    for (int k = 0; k < groups[i].Groups[j].Subjects.Count; k++)
                    {
                        TableData data = groups[i].Groups[j].Subjects[k];
                        // if(data.Time[0] ==dt[0] && data.Time[1] == dt[1])
                        sendData.Add(data);
                    }
            return sendData;
        }

        public static List<TableData> GetDay() 
        {
           // List<TableData> tables = new List<TableData>();


            return sendData;
        }
        public static void CreateDayTime()
        {
            string[] com = System.DateTime.Now.ToString().Split(' ');
          //  int[] dt = com[0].Split('.').Select(int.Parse).ToArray();
            sendData = new List<TableData>();
            for(int i=0;i<groups.Count;i++)
                for (int j = 0; j < groups[i].Groups.Count; j++)
                    for (int k = 0; k < groups[i].Groups[j].Subjects.Count; k++)
                    {
                        TableData data = groups[i].Groups[j].Subjects[k];
                       // if(data.Time[0] ==dt[0] && data.Time[1] == dt[1])
                            sendData.Add(data);
                    }

        }
        
        public static void GroupSetTable(int a, int b,List<TableData> data)
        {
            groups[a].Groups[b].SetTable(data);
        }
        public static void SetMainGroup(string[] com)
        {
            groups = new List<MainGroup>();
            foreach (string str in com)
                groups.Add(new MainGroup(str));
        }
        public static void GroupAdd(int id, GroupData data)
        {
            groups[id].AddGroup(data);
        }
       
        public static void SaveGroup(int id,int i)
        {
            Saver.SaveGroup(groups[id].Groups[i]);
        }

        public static string GetNameGroup(int id, int i)
        {
            return groups[id].Groups[i].Name;

        }
        public static int FindIdGroup(int id,string name)
        {
            UnityEngine.Debug.Log(id);
            UnityEngine.Debug.Log(name);
            int k = groups[id].Groups.FindIndex(x => x.Name == name);
            if (k == -1)
            {
                k = groups[id].Groups.Count;
                groups[id].AddGroup(new GroupData(id,name));
                groups[id].Groups[k].Teachers = new List<GroupData.TeacherList>();
            }

            return k;
        }
        public static string GetName(string tayp,int i)
        {
            string str = "";
            switch (tayp)
            {
                case ("Class"):
                    return classs[i].Name;
                    break;
                case ("Teacher"):
                    return teachers[i].Name;
                    break;
                case ("Unit"):
                    return units[i].Name;
                    break;
                case ("Group"):
                    return groups[i].Name;
                    break;
                case ("Subject"):
                    return subjects[i].Name;
                    break;
                case ("MainGroup"):
                    return groups[i].Name;
                    break;

            }

            return str;
        }
        public static int FindId(string tayp, string name, bool scan = true)
        {
            int i = 0;
            switch (tayp)
            {
                case ("Class"):
                    i = classs.FindIndex(x => x.Name == name);
                    break;
                case ("Teacher"):
                    i = teachers.FindIndex(x => x.Name == name);
                    break;
                case ("Unit"):
                    i = units.FindIndex(x => x.Name == name);
                    break;
                case ("Group"):
                    i = groups.FindIndex(x => x.Name == name);
                    break;
                case ("Subject"):
                    i = subjects.FindIndex(x => x.Name == name);
                    break;

            }

            if (scan) 
            {
                if (i == -1)
                    switch (tayp)
                    {
                        case ("Group"):
                            i = groups.Count;
                            groups.Add(new MainGroup(name));

                            break;

                        case ("Class"):
                            {
                                ClassData data = new ClassData(name);
                                i = classs.Count;

                                Saver.SaveClass(data);
                                classs.Add(data);
                            }
                            break;
                        case ("Teacher"):
                            {
                                TeacherData data = new TeacherData(name);
                                i = teachers.Count;
                                Saver.SaveTeacher(data);
                                teachers.Add(data);
                            }
                            break;
                        case ("Unit"):
                            //{
                            //    UnitData data = new UnitData(name);
                            //}
                            break;
                        //case ("Group"):
                        //    {
                        //        GroupData data = new GroupData(name);
                        //        i = groups.Count;
                        //        FileReader.Saver.SaveGroup(data);
                        //        groups.Add(data);
                        //    }
                        //    break;
                        case ("Subject"):
                            {
                                SubjectData data = new SubjectData(name);
                                i = subjects.Count;
                                Saver.SaveSubject(data);
                                subjects.Add(data);
                            }
                            break;
                        //case ("MainGroup"):
                        //    i = mainGroup.Count;
                        //    mainGroup.Add(name);
                        //    break;
                        default:
                            UnityEngine.Debug.Log(tayp);
                            break;
                    }

                //int j = 0;
                //switch (tayp)
                //{
                //    case ("Teacher"):
                //        //j = groups[cData.Group].Teachers.FindIndex(x => x.Name == i);
                //        //if(j == -1)
                //            groups[cData.Group].Teachers.Add(new GroupData.TeacherList(i, new int[0]));
                //        // cData.Teacher = i;
                //        break;
                //    case ("Class"):
                //       // groups[cData.Group].Teachers.Add(new GroupData.TeacherList(i, new int[0]));
                //       // cData.Teacher = i;
                //        break;
                //    case ("Subejct"):
                //        j = groups[cData.Group].Teachers.FindIndex(x => x.Name == cData.Teacher);
                //        groups[cData.Group].Teachers[j].AddSub(i);
                //        break;
                //}
            }


            return i;
        }
    
        //public static string GetData(string basa, int id)
        //{

        //    return " ";
        //}
        //public static int FindData(string basa, string name)
        //{
        //    switch (basa) 
        //    {
        //        case ("Unit"):
        //            //для поиска техники но тут нужен алгоритм покруче
        //            break;

        //        case ("Subject"):
        //            break;
        //        case ("Teacher"):
        //            break;
        //        case ("ClassRoom"):
        //            break;
        //    }


        //    return -1;
        //}
    }

    public class TableData
    {
        //public int Day;
        //public int Month;
        //public int Year;

        public int[] Time;
        public int[] StartTime;
        public int[] EndTime;
        //public intM EndTimeReal;

        public int Subject;//дисциплина

        public int Teacher;//преподаватель

        public int ClassRoom;//класс
        public TableData(string str)
        {
            int[] com = str.Split('.').Select(int.Parse).ToArray();
            Time = new int[7];//7 size
            for (int i = 0; i < com.Length; i++)
                Time[i] = com[i];

            StartTime = new int[0];//2 size
            EndTime = new int[0];// 5 size
            //Group[0] = Storage.FindId("MainGroup", group);
            //Group = Storage.FindId("Group",group);
        }
        public TableData(TableData data)
        {
            Time = data.Time;

            StartTime = data.StartTime;
            EndTime = data.EndTime;
            Subject = data.Subject;
            Teacher = data.Teacher;
            ClassRoom = data.ClassRoom;
        }

        public string GetTime()
        {
            return $"{Time[0]}.{Time[1]}.{Time[2]}";
        }
        public string GetRealTime()
        {
            string str = "";
            if (StartTime.Length > 0)
            {
                str = $"{StartTime[0]}:{StartTime[1]} - ";
                if(EndTime.Length>0)
                    str += $"{EndTime[0]}:{EndTime[1]} ";
            }
            else
            {
                str = $"{Time[3]}:{Time[4]} - {Time[5]}:{Time[6]}";
            }
            return str;
        }
        public void SetTime(string str)
        {
            int[] com = str.Split('-').Select(int.Parse).ToArray();
            for(int i=0;i<com.Length;i++)
                Time[3+i] = com[i];

        }

        public void SetRealTime(string str, bool start)
        {
            if (str == "-")
                return;
            int[] com = str.Split('.').Select(int.Parse).ToArray();
            if (start)
                StartTime = com;
            else
                EndTime = com;
        }
        public void SetRealTime(bool start)
        {
            if (start)
            {
                if (StartTime.Length > 0)
                    return;
            }
            else
                if (EndTime.Length > 0)
                return;
            
            string[] com = System.DateTime.Now.ToString().Split(' ');
            if (start)
            {
                StartTime = new int[2];
                com = com[1].Split('.');
                StartTime[0] = int.Parse(com[0]);
                StartTime[1] = int.Parse(com[1]);
            }
            else
            {
                int[] ints = com[0].Split('.').Select(int.Parse).ToArray();
                if(ints[0] == Time[0] && ints[1] == Time[1] && ints[2] == Time[2])
                {
                    EndTime = new int[2];
                    com = com[1].Split('.');
                    EndTime[0] = int.Parse(com[0]);
                    EndTime[1] = int.Parse(com[1]);
                }
                else
                {
                    EndTime = new int[5];
                    EndTime[0] = ints[0];
                    EndTime[1] = ints[1];
                    EndTime[2] = ints[2];

                    ints = com[1].Split('.').Select(int.Parse).ToArray();
                    EndTime[3] = ints[0];
                    EndTime[4] = ints[0];
                }
            }
        }

        //public void SetTeacher(string str)   { SetTeacher(Storage.FindData("Teacher",str)); }
        //public void SetTeacher(int i) { Teacher = i; }

        //public void SetSubject(string str)  {  SetSubject(Storage.FindData("Subject",str));  }
        //public void SetSubject(int i) { Subject = i; }

        //public void SetClassRoom(string str)  {   SetClassRoom(Storage.FindData("Class",str));  }
        //public void SetClassRoom(int i) { ClassRoom = i;  }
    }
}
