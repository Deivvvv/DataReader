using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DataSpace;
using FileReader;

public class GuiScript : MonoBehaviour
{
    GuiSctiptUi ui;
    // Start is called before the first frame update

    GameObject GetButton()
    {
        GameObject go = null;
        if (ui.ButtonStorage.childCount > 0)
            go = ui.ButtonStorage.GetChild(0).gameObject;
        else 
            go = Instantiate(ui.Button);

        return go;
    }
    GameObject GetDataTable()
    {
        GameObject go = null;
        if (ui.TableStorage.childCount > 0)
            go = ui.TableStorage.GetChild(0).gameObject;
        else
            go = Instantiate(ui.DataCase);

        return go;
    }
    //void Start()
    //{
    //    ui = gameObject.GetComponent<GuiSctiptUi>();

    //}


    bool[] unitTaypB;
    int[] unitTaypS;
    string[] unitTayp;

    public void LoadUnitScreen()
    {
        //загрузить список типов оборудования, добавить функцию выбора одной, нескольких групп для загрузки, общая сумма оборудования (заргузка спиков по нажатии кнокпи подвеждения, наличение кнопки(выбруть, убрать всё))
        //загружать список оборудования в формате, название - кол-во - тип
        // как развернутый - ид номер бухгалтерии - ид личный номер - статус оборудования(исправен/неисправен) - кабинет местонахождения доп:за кем закреплен - фотографии еденицы оборудования

        void SetButtonTayp(Button button, int i)
        {
            button.onClick.AddListener(() => SwitchUnitTayp(i));
        }
        unitTayp = Storage.GetUnitTayps();

        unitTaypB = new bool[unitTayp.Length];
       // for (int i = 0; i < unitTaypB.Length; i++)
       //     unitTaypB[i] = true;
        unitTaypS = Storage.GetUnitTaypsSize();

        GameObject go;
        for(int i =0; i< unitTayp.Length; i++)
        {
            go = GetButton();
            go.transform.SetParent(ui.UnitWindowTayp);
            SetButtonTayp(go.GetComponent<Button>(), i);
            SwitchUnitTayp(i);
        }

        LoadUnit();
    }
    void OpenUnit(intM a)
    {
       UnitData.MainIdCase uCase = Storage.GetUnit(a);
    }
    void LoadUnit()
    {
        void SetButton(Button button, intM a)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OpenUnit(a));
        }


       // GameObject go;
        List<intM> com = Storage.GetUnits(unitTaypB);
        for (int i = com.Count; i < ui.UnitWindowTayp.childCount; i++)
            ui.UnitWindowTayp.GetChild(i).SetParent(ui.UnitButtonStorage);

        for (int i = ui.UnitWindow.childCount; i < com.Count; i++)
            GetButton().transform.SetParent(ui.UnitWindow);

        Transform tf;
        for (int i = 0; i < com.Count; i++)
        {
            Debug.Log($"{i} {com.Count}");
            tf = ui.UnitWindow.GetChild(i);

            tf.GetChild(0).gameObject.GetComponent<Text>().text = Storage.GetUnitName(com[i]);
            SetButton(tf.gameObject.GetComponent<Button>(),com[i]);
        }
    }

    void SwitchUnitTayp(int i)
    {
        void ViewUnitTayp(int i)
        {
            string str = "" +((unitTaypB[i]) ? "on" : "off") + $" {unitTayp[i]} ({unitTaypS[i]})";
            ui.UnitWindowTayp.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = str;
        }
        unitTaypB[i] = !unitTaypB[i];

        ViewUnitTayp(i);
    }

    public void LoadInfoWindow()
    {
        ui = gameObject.GetComponent<GuiSctiptUi>();
        Reader.LoadTableGroup();
        LoadUnitScreen();
        return;
        //загружаем информацию о всех
     //   Storage.CreateDayTime();

        string[] com = System.DateTime.Now.ToString().Split(' ');
        data = com[0].Split('.').Select(int.Parse).ToArray();
        actualYear = data[2] / 10;

        LoadTimeWindow();

        OpenWindow("Year");
        //OpenWindow("Group");
        SwitchGroup(0);
        //List<TableData> tables = Storage.GetDay();
        //GameObject go = null;
        //for (int i =0; i<tables.Count; i++)
        //{
        //    go = GetDataTable();
        //    go.transform.SetParent(ui.MainWindow);
        //    GroupCaseUi gUi= go.GetComponent<GroupCaseUi>();
        //    gUi.Data.text = tables[i].GetTime();
        //    gUi.Time.text = tables[i].GetRealTime();
        //    gUi.Teacher.text = Storage.GetName("Teacher", tables[i].Teacher);
        //    gUi.Subject.text = Storage.GetName("Subject", tables[i].Subject);
        //    gUi.Calsss.text = Storage.GetName("Class", tables[i].ClassRoom);

        //}
    }
    void LoadTable()
    {
        List<TableData> tables = Storage.GetAllTable();
        GameObject go = null;
        for (int i = 0; i < tables.Count; i++)
        {
            go = GetDataTable();
            go.transform.SetParent(ui.MainWindow);
            GroupCaseUi gUi = go.GetComponent<GroupCaseUi>();
            gUi.Data.text = tables[i].GetTime();
            gUi.Time.text = tables[i].GetRealTime();
            gUi.Teacher.text = Storage.GetName("Teacher", tables[i].Teacher);
            gUi.Subject.text = Storage.GetName("Subject", tables[i].Subject);
            gUi.Calsss.text = Storage.GetName("Class", tables[i].ClassRoom);

        }
    }

    void ViewYear()
    {
        GameObject go = null;
        //ui.GuiColor[0]//gray [1]white [2] white-blue
        int ay = (actualYear + useYear) * 10;
        for (int i = 0; i < ui.YearWindow.childCount; i++)
        {
            go = ui.YearWindow.GetChild(i).gameObject;
            int y = ay + i;
            int x = years.FindIndex(x => x == y);
            go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "" + y;
            if(x != -1)
            {
                go.GetComponent<Image>().color = (y == data[2]) ? ui.GuiColor[2] : ui.GuiColor[1];
                //можно добавить вывод количества дней когда выдавали оборудование?!

            }
            else
                go.GetComponent<Image>().color = ui.GuiColor[0];
        }
        int size = actualYear + useYear;
        ui.NameList.text = $"{size}0-{size}9:{data[1]}:{data[0]}";
    }
    void ViewMonth()
    {
        string[] name = { 
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"};

        GameObject go = null;
        for (int i = 0; i < ui.MonthWindow.childCount; i++)
        {
            go = ui.MonthWindow.GetChild(i).gameObject;
            int x = mouths.FindIndex(x => x == i);
            go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = name[i];
            if (x != -1)
            {
                go.GetComponent<Image>().color = (i == data[1]) ? ui.GuiColor[2] : ui.GuiColor[1];
                //можно добавить вывод количества дней когда выдавали оборудование?!

            }
            else
                go.GetComponent<Image>().color = ui.GuiColor[0];
        }

        ui.NameList.text = $"{data[2]}:{data[1]}:{data[1]}";
    }
    void ViewDay()
    {
        void Closed(GameObject go)
        {
            go.GetComponent<Image>().color = ui.GuiColor[0];
            go.transform.GetChild(0).gameObject.GetComponent<Image>().color = ui.GuiColor[0];
        }

        System.DateTime dt = new System.DateTime(data[2], data[1], 1);
        int dayOfWeek = (int)dt.DayOfWeek;
        int daysInMouth = System.DateTime.DaysInMonth(data[2], data[1]);
        GameObject go = null;

        int num = 0; 

        for (int i = 0; i < ui.DayWindow.childCount; i++)
        {
            go = ui.DayWindow.GetChild(i).gameObject;
            string str = "";
            if(num != 0)
            {
                if (num > daysInMouth)
                {
                    Closed(go);
                }
                else
                {
                    go.transform.GetChild(0).gameObject.GetComponent<Image>().color = ui.GuiColor[1];
                    //else
                    //    go.transform.GetChild(0).gameObject.GetComponent<Image>().color = ui.GuiColor[2];

                    Color c;
                    str = $"{num}";
                    if (num == data[0])
                    {
                        c = ui.GuiColor[2];
                        //go.transform.GetChild(0).gameObject.GetComponent<Image>().color = ui.GuiColor[2];
                    }
                    else
                    {
                        int x = days.FindIndex(x => x == num);
                        c = (x == -1) ? ui.GuiColor[1] : ui.GuiColor[3];
                    }
                    go.GetComponent<Image>().color = c;
                }
                num++;
            }
            else if(dayOfWeek == i + 1)
            {
                Closed(go);
                num++;
            }
            else
            {
                Closed(go);

            }
            go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =str;

        }
        ui.NameList.text = $"{data[2]}:{data[1]}:{data[1]}";
    }
    void OpenWindow(string str)
    {
        if (str == "Year")
        {
            ui.YearWindowMain.SetActive(true);
            ViewYear();
        }
        else
            ui.YearWindowMain.SetActive(false);

        if (str == "Mouth")
        {
            ui.MonthWindowMain.SetActive(true);
            ViewMonth();
        }
        else
            ui.MonthWindowMain.SetActive(false);

        if (str == "Day")
        {
            ui.DayWindowMain.SetActive(true);
            ViewDay();
        }
        else
            ui.DayWindowMain.SetActive(false);


        if (str == "Group")
        {
            ui.MainWindowMain.SetActive(true);
            LoadGroup();
            //ViewDay();
        }
        else
            ui.MainWindowMain.SetActive(false);

        //ui.NameList.text = str;
    }
    int systemMood = 0;
    void SwitchMood(bool up)
    {
        string[] com = { "Year", "Mouth" , "Day", "Group" };

        if (up) 
        {
            if(systemMood <com.Length-1)
                systemMood++; 
        }
        else
        {
            if(systemMood>0)
            systemMood--;
        }
       // Debug.Log(com[systemMood]);

      //  ui.NameList.text = com[systemMood];
        OpenWindow(com[systemMood]);
    }

    #region year
    int actualYear;
    int useYear;
    void SwitchTenYear(bool up)
    {
        if (up)
            useYear++;
        else
            useYear--;
        ViewYear();
    }
    void SwitchYear(bool up)
    {
        if (up)
            data[2]++;
        else
            data[2]--;
        ViewMonth();
    }
    void SwitchMonth(bool up)
    {
        if (up)
            data[1]++;
        else
            data[1]--;
        ViewDay();
    }
    void SwitchDay(bool up)
    {
        if (up)
            data[0]++;
        else
            data[0]--;
        LoadGroup();
    }
    #endregion
    void SetYear(int i)
    {
        data[2] = (actualYear + useYear) * 10 +i;
        OpenWindow("Mouth");
    }
    void SetMonth(int i)
    {
        data[1] = i;
        OpenWindow("Day");
    }
    void SetDay(int i)
    {
        data[0] = i;
        OpenWindow("Group");
    }
    void LoadTimeWindow()
    {
        void ConnectButton(Button button, int i, string mood)
        {
            switch (mood)
            {
                case ("Year"):
                    button.onClick.AddListener(() => SetYear(i));
                    break;
                case ("Month"):
                    button.onClick.AddListener(() => SetMonth(i));
                    break;
                case ("Day"):
                    button.onClick.AddListener(() => SetDay(i));
                    break;
                //case ("Group"):
                //    button.onClick.AddListener(() => SetDay(i));
                //    break;
            }
        }

        GameObject go = null;
        //year

        years = Reader.GetYears();
        for (int i = 0; i < 10; i++)
        {
            go = Instantiate(ui.DataCase);
            ConnectButton(go.GetComponent<Button>(), i, "Year");
            go.transform.SetParent(ui.YearWindow);
        }
        for (int i = 0; i < 12; i++)
        {
            go = Instantiate(ui.DataCase);
            ConnectButton(go.GetComponent<Button>(), i, "Month");
            go.transform.SetParent(ui.MonthWindow);
        }
        for (int i = 0; i < 42; i++)
        {
            go = Instantiate(ui.DataCase);
            ConnectButton(go.GetComponent<Button>(), i, "Day");
            go.transform.SetParent(ui.DayWindow);
        }

        ui.DownList.onClick.AddListener(() => SwitchMood(false));
        ui.UpList.onClick.AddListener(() => SwitchMood(true));

        ui.UpYear.onClick.AddListener(() => SwitchTenYear(true));
        ui.DownYear.onClick.AddListener(() => SwitchTenYear(false));
        ui.UpYearLocal.onClick.AddListener(() => SwitchYear(true));
        ui.DownYearLocal.onClick.AddListener(() => SwitchYear(false));
        ui.UpMonth.onClick.AddListener(() => SwitchMonth(true));
        ui.DownMonth.onClick.AddListener(() => SwitchMonth(false));

        LoadManager(1);
        LoadManager(2);
    }

    private void LoadGroup()
    { 
        void SetButton(Button button, int i)
        {
            //button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SwitchGroup(i));
        }

        GameObject go = null;
        string[] com = Storage.GetGroupList();
        for (int i = ui.GroupList.childCount; i < com.Length; i++)
        {
            go = GetButton();
            go.transform.GetChild(0).GetComponent<Text>().text = com[i];
            SetButton(go.GetComponent<Button>(), i);

            go.transform.SetParent(ui.GroupList);
        }

        //ui.
    }
    #region Time Manager
    private int[] data = new int[3];
    private List<int> years;
    private List<int> mouths;
    private List<int> days;

    private void LoadManager(int m)
    {
        string[] com = { "Year", "Mouth", "Day","Group" }; 
        switch (com[m])
        {
            case ("Yaer"):
                {
                    years = Reader.GetYears();
                }
                break;
            case ("Mouth"):
                {
                    if (years.Count > 1)
                        mouths = Reader.GetMouths(years[data[0]]);
                    else
                        mouths = new List<int>();
                }
                break;
            case ("Day"):
                {
                    if (mouths.Count > 1)
                        days = Reader.GetDays(years[data[0]], mouths[data[1]]);
                    else
                        days = new List<int>();
                }
                break;
            case ("Group"):
                break;
        }

    }
  
    #endregion

    private void SwitchGroup(int xi)
    {
        void SetButton(Button button, int i)
        {
          //  button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SwitchGroupName(i));
        }
        GameObject go = null;
        string[] com = Storage.SwicthGroup(xi);
        for (int i = com.Length; i < ui.GroupNameList.childCount; i++)
        {
            go = ui.GroupNameList.GetChild(i).gameObject;
            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.transform.SetParent(ui.ButtonStorage);
        }

        for (int i = ui.GroupNameList.childCount; i < com.Length; i++)
        {
            go = GetButton();
            SetButton(go.GetComponent<Button>(), i);
            go.transform.SetParent(ui.GroupNameList);
        }


        for(int i = 0; i < com.Length; i++)
            ui.GroupNameList.GetChild(i).GetChild(0).GetComponent<Text>().text = com[i];

        //Storage.gro// назначение выбранной группы в список
    }
    private void SwitchGroupName(int xi)
    {

    }

   
}
