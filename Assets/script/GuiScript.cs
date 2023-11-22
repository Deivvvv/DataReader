using System.Collections;
using System.Collections.Generic;
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

    public void LoadInfoWindow()
    {
        ui = gameObject.GetComponent<GuiSctiptUi>();
        //загружаем информацию о всех
        Storage.CreateDayTime();


        List<TableData> tables = Storage.GetDay();
        GameObject go = null;
        for (int i =0; i<tables.Count; i++)
        {
            go = GetDataTable();
            go.transform.SetParent(ui.MainWindow);
            GroupCaseUi gUi= go.GetComponent<GroupCaseUi>();
            gUi.Data.text = tables[i].GetTime();
            gUi.Time.text = tables[i].GetRealTime();
            gUi.Teacher.text = Storage.GetName("Teacher", tables[i].Teacher);
            gUi.Subject.text = Storage.GetName("Subject", tables[i].Subject);
            gUi.Calsss.text = Storage.GetName("Class", tables[i].ClassRoom);

        }
    }

    private void LoadGroup()
    { 
        void SetButton(Button button, int i)
        {
            button.onClick.AddListener(() => SwitchGroup(i));
        }

        string[] com = Storage.GetGroupList();
        GameObject go = null;
        for(int i = 0; i < com.Length; i++)
        {
            go = GetButton();
            go.transform.GetChild(0).GetComponent<Text>().text = com[i];
            SetButton(go.GetComponent<Button>(), i);

            go.transform.SetParent(ui.MainWindow);
        }
        //ui.
    }
    #region Time Manager
    void DataManager(int m)
    {
        string[] com = { "Year", "Mouth", "Day" };
        switch (com[m])
        {
            case ("Yaer"):
                com = Reader.GetYears();
                break;
        }
        int daysInJuly = System.DateTime.DaysInMonth(2001, July);
    }
    #endregion

    private void SwitchGroup(int xi)
    {
        string[] com = Storage.SwicthGroup(xi);
        for(int i = 0; i < ui.GroupNameList.childCount; i++)
        {

        }
    }
    private void SwitchGroupName(int xi)
    {

    }

   
}
