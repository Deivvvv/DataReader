using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataSpace
{
    class UnitData
    {
        public string NameSys;
        public string Name;

    }
    class TeacherData
    {
        public string NameSys;
        public string Name;

        public List<TableData> Units;
    }
    class GroupData
    {
        public string NameSys;
        public string Name;

        public List<TeacherData> Units;

    }
    class SubjectData
    {
        public string NameSys;//название дисципилины
        public string Name;
        public List<GroupData> Data;
    }

    public static class Storage
    {
        public static intM zeroIntM = new intM(0);

        static List<GroupData> groupDatas;
        static List<SubjectData> subjects;

        public static string GetData(string basa, int id)
        {

            return " ";
        }
        public static int FindData(string basa, string name)
        {
            switch (basa) 
            {
                case ("Unit"):
                    //для поиска техники но тут нужен алгоритм покруче
                    break;

                case ("Subject"):
                    break;
                case ("Teacher"):
                    break;
                case ("ClassRoom"):
                    break;
            }


            return -1;
        }
    }

    class TableData
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
               // string[] str1 = 
               // if()
            }
            com = com[1].Split('.');
            //int[] com = System.DateTime.Now.ToString().Split('.').Select(int.Parse).ToArray();
            StartTime[0] = int.Parse(com[0]);
            StartTime[1] = int.Parse(com[1]);
        }
        public void SetEndTime()
        {
            if (EndTime.Length > 0)
                return;
            EndTime = System.DateTime.Now.ToString().Split('.').Select(int.Parse).ToArray();
        }

        public void SetTeacher(string str)
        {
            SetTeacher(Storage.FindData("Teacher",str));
        }
        public void SetTeacher(int i)
        {
            Teacher = i;
        }

        public void SetSubject(string str)
        {
            SetSubject(Storage.FindData("Subject",str));
        }
        public void SetSubject(int i)
        {
            Subject = i;
        }

        public void SetClassRoom(string str)
        {
            SetClassRoom(Storage.FindData("Class",str));
        }
        public void SetClassRoom(int i)
        {
            ClassRoom = i;
        }
    }
}
