using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuiSctiptUi : MonoBehaviour
{
    public Transform GroupList;
    public Transform GroupNameList;
    public Transform ButtonList;

    public Transform ButtonStorage;
    public Transform TableStorage;

    public GameObject Button;
    public GameObject DataCase;
    public GameObject DataEventCase;


    public Transform YearWindow;
    public Transform MonthWindow;
    public Transform DayWindow;
    public Transform MainWindow;//groupList;

    public GameObject YearWindowMain;
    public GameObject MonthWindowMain;
    public GameObject DayWindowMain;
    public GameObject MainWindowMain;

    public Color[] GuiColor;
    public Color[] GuiColorSecond;

    public Button UpList;
    public Button DownList;
    public Text NameList;


    public Button UpYear;
    public Button DownYear;
    public Button UpYearLocal;
    public Button DownYearLocal;
    public Button UpMonth;
    public Button DownMonth;

    public GameObject UnitMainWindow;
    public Transform UnitWindow;
    public Transform UnitWindowTayp;
    public GameObject UnitButton;
    public Transform UnitButtonStorage;
}
