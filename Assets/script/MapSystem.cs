using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FileReader;

public class MapSystem : MonoBehaviour
{
    public struct LevelRoom
    {
        public List<int> Id;
    }

    private MapSystemUi ui;
    private List<Room> roomList;
    private LevelRoom[] roomListLevels;
    private int idRoom = -1, localId;

    string[] levelName = {"Все комнаты", "1 Этаж", "2 Этаж", "3 Этаж", "Мансардный этаж" };
    int[] levels = new int[3];

    Vector3 vFix = new Vector3(0.5f, 0.5f, 0);
    Vector3 cellPosition, lookPosition, afterPosition;
    int lookPos = -1;
    float fixCof = 0.95f;
    public float speed = 0.95f;
    int[] zoom = new int[3];
    int[] zoomMap = {10,18, 24,30,36, 45, 54 };

    bool editor, blok;
    void Start()
    {
        ui = gameObject.GetComponent<MapSystemUi>();
        zoom[2] = zoomMap.Length-1;
        levels[2] = levelName.Length-1;


        Application.targetFrameRate = 30;
        ui.Camera.gameObject.GetComponent<Camera>().orthographicSize = zoomMap[zoom[1]];

        GameObject go = null;
       // Reader.Start();

        ui.CreateRoomButton.onClick.AddListener(() => CreateRoom());
        ui.SaveButton.onClick.AddListener(() => SaveRooms());
        //ui. deliteroom
        // roomList = FileReader.LoadRoomsList();
        roomList = Reader.LoadRooms(levelName.Length, gameObject.GetComponent<MapSystem>());
        if (roomList.Count == 0)
            CreateRoom();
        else
        {
            for (int i = 0; i < roomList.Count; i++)
            {

                go = Instantiate(ui.OrigLine);
                go.transform.SetParent(ui.LineStorage);
                ui.Lines.Add(go.GetComponent<LineRenderer>());


                go = Instantiate(ui.OrigText);
                go.transform.SetParent(ui.TextStorage);
                go.GetComponent<TextMesh>().text = roomList[i].Name;
                go.transform.position = roomList[i].TextPosition;
            }



            LoadButton("Map");
        }

        go = Instantiate(ui.OrigPoint);
        go.transform.SetParent(ui.PointStorage);

        SwitchLevel(1);
       // LoadRoomList();
    }
    public void GetLevelList(LevelRoom[] rooms)
    {
        roomListLevels = rooms;
    }
    void SwitchLevel(int id)
    {
        List<int> rooms = null;
        levels[1] = id;
        ui.LevelText.text = levelName[id];
        if (id == 0)
        {
            ui.CreateRoomButton.interactable = false;
            rooms = new List<int>(new int[roomList.Count]);
            for (int i = 0; i < roomList.Count; i++)
                rooms[i] = i;

        }
        else
        {
            ui.CreateRoomButton.interactable = true;
            rooms = roomListLevels[id].Id;
        }
        LoadRoomList(rooms);
    }
    void SaveRooms()
    {
        List<Room> list = new List<Room>();
        List<int> id = new List<int>();
        for (int i=0;i< roomList.Count;i++)
            if (roomList[i].Save)
            {
                list.Add(roomList[i]);
                id.Add(i);

                roomList[i].Save = false;
            }

        if(list.Count>0)
            Saver.SaveRooms(list,id.ToArray(), ui.WorldGrid);
    }
    //организация храниня map/level/room-dataroom
    public class Room
    {
        public string Name, OldName;
        public int Level;
        public List<Vector3> Lines;
        public Vector3 TextPosition;
        public int[] Border = new int[4];
        public bool Save;
    }


    float lerpF(int a, int b)
    {
        return a + 1f * (b - a) / 2;
    }
    public void ViewRoom(int id, bool trans = false)
    {

        if (trans)
        {
            //int[] i = roomList[id].Border;
            ui.Camera.position = new Vector3(roomList[id].TextPosition[0], roomList[id].TextPosition[1], -10);
            if(localId !=-1)
            {
                ui.Lines[localId].positionCount = 0;
            }
            
            localId = id;

        }
        ui.NameFlied.text = roomList[id].Name;

        ViewLine(ui.Lines[id], roomList[id].Lines, id);
    }

    void AddRoomBotton(int id, int i)
    {
        void SetButton(Button button, int i)
        {
            button.onClick.AddListener(() => OpenRoom(i));
        }
        GameObject go = ui.RoomList.GetChild(i).gameObject;//GetButton(ui.RoomList);
        go.transform.GetChild(0).gameObject.GetComponent<Text>().text = roomList[id].Name;

        SetButton(go.GetComponent<Button>(), id);

        if (go.GetComponent<ViewRoomSctipt>() == null)
            go.AddComponent<ViewRoomSctipt>();
        go.GetComponent<ViewRoomSctipt>().SetId(id);

    }
    void LoadRoomList(List<int> rooms)
    {
       

        for (int i =0;i< ui.RoomList.childCount; i++)
        {
            ui.RoomList.GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        for (int i = ui.RoomList.childCount; i < rooms.Count; i++)
        {
            GetButton(ui.RoomList);
        }

        int size = ui.RoomList.childCount;
        for (int i = rooms.Count; i < size; i++)
        {
            Transform trans = ui.RoomList.GetChild(0);
            Destroy(trans.gameObject.GetComponent<ViewRoomSctipt>());
            trans.SetParent(ui.ButtonStorage);
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            AddRoomBotton(rooms[i], i);
        }

        if (levels[1] == 0)
        {

            for (int i = 0; i < rooms.Count; i++)
                ui.Lines[i].positionCount = 0;


            for (int i = 0; i < ui.PointStorage.childCount; i++)
                ui.PointStorage.GetChild(i).gameObject.SetActive(false);

            //ClearRoom();
        }
        else
            for (int i = 0; i < rooms.Count; i++)
                ViewRoom(rooms[i]);
    }

    public  void LoadKey(bool use)   {  blok = use;   }
    GameObject GetButton(Transform trans)
    {
        GameObject go = null;
        if (ui.ButtonStorage.childCount > 0)
        {
            go = ui.ButtonStorage.GetChild(0).gameObject;

            ui.ButtonStorage.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        else
            go = Instantiate(ui.OrigButton);

        go.transform.SetParent(trans);
        return go;
    }
    void ClearList(Transform t1, string mood)
    {
        Transform t2 = null;
        switch (mood)
        {
            case ("Button"):
                t2 = ui.ButtonStorage;
                break;
        }

        int id = t1.childCount;
        for (int i = 0; i < id; i++)
            t1.GetChild(0).SetParent(t2);
    }

    void LoadButton(string mood)
    {
        ClearList(ui.RedactorWindow, "Button");

        void AddButton(string mood)
        {
            GameObject go = GetButton(ui.RedactorWindow);
            Button button = go.GetComponent<Button>();
            string text = "";

            void WallEdit(bool use)
            {
                editor = use;
                ui.NameFlied.interactable = !editor;
                if (editor)
                {
                    LoadButton("RoomEdit");
                }
                else
                {
                    LoadButton("Room");
                    ViewLine(ui.Lines[idRoom], roomList[idRoom].Lines, idRoom);
                }
            }
            switch (mood)
            {
                

                case ("StartWallEditor"):
                    button.onClick.AddListener(() => WallEdit(true));
                    text = "Изменить стены";
                    break;
                case ("StopWallEditor"):
                    button.onClick.AddListener(() => WallEdit(false));
                    text = "Прекратить изменять стены";
                    break;
                case ("RoomClosed"):
                    button.onClick.AddListener(() => ClosedRoom());
                    text = "Закрыть комнату";
                    break;
                case ("RoomSave"):
                    //button.onClick.AddListener(() => SaveRoom());
                    text = "Сохранить комнату";
                    break;
                default:
                    Debug.Log(mood);
                    break;
            }

            go.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;


        }

        switch (mood)
        {
            case ("Map"):
                //ui.R
                //createRoom;
                break;
            case ("Room"):
                AddButton("RoomClosed");
                AddButton("StartWallEditor");
                //EditorRoom, SaveRoom, ClosedRoom,;
                break;
            case ("RoomEdit"):
                AddButton("StopWallEditor");
                //EditorRoom, SaveRoom, ClosedRoom,;
                break;
        }
        Debug.Log(mood);

    }


    void AddLinePoint(Vector3 v)
    {
        bool ScanLine(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if(v1[0] >v2[0])

            if (v3[0]> v1[0] &&
             v3[0] > v1[1] &&
            v3[0] > v2[0] &&
             v3[0] > v2[1]
             )
            {

            }
                //float cof = 0.2f;
                //return ((v1[0]-cof) < v3[0]||
                //    (v1[0] - cof) > v3[0]
                //    )


                return true;
        }

        List<Vector3> lines = roomList[idRoom].Lines;
        int id = lines.FindIndex(x => x == v);
        if (id != -1)
        {
            lookPos = id;
            return;
        }
        for(int i=0;i< lines.Count-1; i++)
        {
        //    if (i + 1 == lines.Count)
        //    {
        //        ScanLine(lines[i], lines[0]);
        //    }
        //    else
        //    {
                ScanLine(lines[i], lines[i + 1], cellPosition);
          //  }
        }

        lines.Add(v);

        ViewLine(ui.Lines[idRoom], lines, idRoom);

    }

    void RemoveLinePoint(Vector3 v)
    {
        List<Vector3> lines = roomList[idRoom].Lines;
        if (lines.Count == 0)
            return;
      //  ui.PointStorage.GetChild(lines.Count - 1).gameObject.SetActive(false);

        int id = lines.FindIndex(x => x == v);
        if (id != -1)
            lines.RemoveAt(id);
        else
            lines.RemoveAt(lines.Count - 1);

        ViewLine(ui.Lines[idRoom], lines, idRoom);
    }
    Vector3 Lerp(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }
    void ViewLine(LineRenderer line, List<Vector3> v, int id)
    {
        if (v.Count == 0)
            return;


        int[] cof = new int[4];
        cof[0] = cof[2] = Mathf.CeilToInt(v[0][0]);
        cof[1] = cof[3] = Mathf.CeilToInt(v[0][1]);

        if (v.Count > 1)
        {
            Vector3[] v1 = new Vector3[v.Count + 1];
            for (int i = 1; i < v.Count; i++)//(fix)
            {
                Vector3 v2 = v[i];
                int x = Mathf.CeilToInt(v2[0]);
                int y = Mathf.CeilToInt(v2[1]);
                if (cof[0] > x)
                    cof[0] = x;
                if (cof[2] < x)
                    cof[2] = x;

                if (cof[1] > y)
                    cof[1] = y;
                if (cof[3] < y)
                    cof[3] = y;
            }
            //Vector3 vc = new Vector3(lerpF(cof[0],cof[2]), lerpF(cof[1], cof[3]), 0);// Lerp(new Vector3(cof[0], cof[1], 0), new Vector3(cof[2], cof[3], 0), 0.5f);
            ui.TextStorage.GetChild(id).position =  roomList[id].TextPosition ;
            for (int i = 0; i < v.Count; i++)
                v1[i] = v[i];// Lerp(v[i], vc, fixCof);
            v1[v.Count] = v[0];//замыкаем линию

            line.positionCount = v1.Length;
            line.SetPositions(v1);
        }
        else
        {
            line.positionCount = 0;
        }

        {
            for (int i = ui.PointStorage.childCount; i < v.Count; i++)
            {
                GameObject go = Instantiate(ui.OrigPoint);
                go.transform.SetParent(ui.PointStorage);
            }
            for (int i = 0; i < v.Count; i++)
                ui.PointStorage.GetChild(i).gameObject.SetActive(true);

            for (int i = v.Count; i < ui.PointStorage.childCount; i++)
                ui.PointStorage.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < v.Count; i++)
            ui.PointStorage.GetChild(i).position = v[i];

        roomList[id].Border = cof;
    }

    void CreateRoom()
    {
        Room room = new Room();
        idRoom = roomList.Count;
        room.Lines = new List<Vector3>();
        room.OldName = room.Name = "Room" +roomList.Count;
        ui.NameFlied.text = room.Name;
        room.Level = levels[1];
        room.Save =true;

        GameObject go = Instantiate(ui.OrigLine);
        go.transform.SetParent(ui.LineStorage);
        ui.Lines.Add(go.GetComponent<LineRenderer>());


        go = Instantiate(ui.OrigText);
        go.transform.SetParent(ui.TextStorage);
        go.GetComponent<TextMesh>().text = room.Name;


        roomList.Add(room);
        roomListLevels[room.Level].Id.Add(idRoom);
        OpenRoom(idRoom);

        editor = true;
        LoadButton("RoomEdit");
        blok = false;
    }
    int SelectRoom()
    {
        int id =-1;
        int x = Mathf.CeilToInt(cellPosition[0]);
        int y = Mathf.CeilToInt(cellPosition[1]);
        for (int i = 0; i < roomList.Count; i++)
        {
            int[] coord = roomList[i].Border;
            if (coord[0] > x ||
                coord[1] > y ||
                coord[2] < x ||
                coord[3] < y
                )
                continue;
            else
            {
                id = i;
                break;
            }
        }

        return id;
    }
    void OpenRoom(int id)
    {
        //Debug.Log(id);
        ui.RoomListWindow.SetActive(false);
        idRoom = id;

        SwitchLevel(roomList[id].Level);
        ui.NameFlied.interactable = true;
        ui.NameFlied.text = roomList[id].Name;
        ViewLine(ui.Lines[id], roomList[id].Lines, id);
        LoadButton("Room");
      //  ui.Camera.position = new Vector3();
       // roomList[id];
        //код открытия полного интерфеса 
        //выгружаем информаю комнта, выгружаем информацию рабочего места
    }
    void ClosedRoom()
    {
        if (roomList[idRoom].Name != roomList[idRoom].OldName)
        {
            Debug.Log($"{roomList[idRoom].Name} != {roomList[idRoom].OldName}");
            int id = roomList.FindIndex(x => x.Name == ui.NameFlied.text);
            if (id != -1 && id != idRoom)
            {
                Debug.Log("CopyName");
                return;
            }
        }

        roomList[idRoom].Name = ui.NameFlied.text;
        ui.TextStorage.GetChild(idRoom).gameObject.GetComponent<TextMesh>().text = roomList[idRoom].Name;
        ui.RoomList.GetChild(idRoom).GetChild(0).gameObject.GetComponent<Text>().text = roomList[idRoom].Name;
        roomList[idRoom].Save = true;

        ui.NameFlied.interactable = false;
        ui.RoomListWindow.SetActive(true);
        LoadButton("Map");
        idRoom = -1;

    }

    // Start is called before the first frame update

    //void AddPointConvert(Vector2Int x1, Vector2Int x2 )
    //{
    //    intM x, y;
    //    if (x1[0] > x2[0])
    //        x = new intM(x2[0], x1[0]-x2[0]);
    //    else
    //        x = new intM(x1[0], x2[0] - x1[0]);
    //    if (x1[1] > x2[1])
    //        y = new intM(x2[1], x1[1] - x2[1]);
    //    else
    //        y = new intM(x1[1], x2[1] - x1[1]);

    //    PointModul modul = new PointModul();
    //    modul.StartPoint = x;
    //    modul.EndPoint = y;
    //    positions.Add(modul);
    //}
    //void RemovePoint(Vector2Int z)
    //{
    //    for(int i = 0; i < positions.Count; i++)
    //    {
    //        intM x = positions[i].StartPoint;
    //        if (z[0] >= x.Min && z[0] >= x.Min + x.Max)
    //        {

    //            intM y = positions[i].EndPoint;
    //            if (z[1] >= y.Min && z[1] >= y.Min + y.Max)
    //            {
    //                positions.RemoveAt(i);
    //                //ViewRoom();
    //                return;
    //            }
    //            else
    //                continue;

    //        }
    //        else
    //            continue;
    //    }
    //}
    //Vector2Int startPoint, curentPoint,actualPoint;

    //int MatrixSize(int a, int b)
    //{
    //    int c = 0;
    //    if(a <0 && b < 0)
    //        c = (a > b) ? a - b : b - a;
    //    else if (a < 0 || b < 0)
    //        c = (a > b) ? module(a) - module(b) : module(b) - module(a);
    //    else
    //        c = (a > b) ? a - b : b - a;

    //    if (c < 0)
    //        c = module(c);

    //    return c;//+1;
    //}

    //int[] MatrixConverter(Vector2Int v1, Vector2Int v2)
    //{
    //    int[] i = new int[4];
    //    //кордината и размерность
    //    i[0] = v1[0];
    //    i[1] = v1[1];
    //    i[2] = MatrixSize(v1[0], v2[0]);
    //    i[3] = MatrixSize(v1[1], v2[1]);
    //    if (v2[0] < i[0])
    //        i[0] = v2[0];
    //    if (v2[1] < i[1])
    //        i[1] = v2[1];

    //    return i;
    //}
    //bool FindInMatrix(int a,int b, int c )
    //{
    //    //if (a <= c || c <= b)
    //    //    return true;
    //    return (a <= c || c <= b);
    //}
    //void EditMatrix(bool add, int[] m1, int[] m2)
    //{ //x/y/xs/ys
    //    int[] m3 = new int[4];
    //    if (add)
    //    {

    //    }
    //    else
    //    {
    //        if (FindInMatrix(m1[0], m1[0]+ m1[2], m2[0]))
    //        {
    //            if (FindInMatrix(m1[1], m1[1] + m1[3], m2[1]))
    //            {



    //                return;
    //            }
    //        }
    //    }
    //}


    //     void EditMatrix(bool add, Vector2Int x2)
    // {
    //     int[] coord = new int[4];
    //     Vector2Int x1 = actualPoint;

    //     if (x1[0] > x2[0])
    //     {
    //         coord[0] = x2[0];
    //         coord[2] = x1[0];
    //     }
    //     else
    //     {
    //         coord[2] = x2[0];
    //         coord[0] = x1[0];
    //     }
    //     if (x1[1] > x2[1])
    //     {
    //         coord[1] = x2[1];
    //         coord[3] = x1[1];
    //     }
    //     else
    //     {
    //         coord[3] = x2[1];
    //         coord[1] = x1[1];
    //     }





    //     if (add)
    //     {

    //     }
    //     else if(MapMatrix.Length>0)
    //     {
    //         if (coord[2] < RoomPosition[0] ||
    //             coord[3] < RoomPosition[1] ||
    //             coord[0] > RoomPositionMax[0] ||
    //             coord[1] > RoomPositionMax[1]
    //             )
    //             return;

    //         if (coord[0] < RoomPosition[0])
    //             coord[0] = RoomPosition[0];
    //         if (coord[1] < RoomPosition[1])
    //             coord[1] = RoomPosition[1];
    //         if (coord[2] > RoomPositionMax[0])
    //             coord[2] = RoomPositionMax[0];
    //         if (coord[3] > RoomPositionMax[1])
    //             coord[3] = RoomPositionMax[1];
    //         if (coord[0] == RoomPosition[0] &&
    //             coord[1] == RoomPosition[1] &&
    //             coord[2] == RoomPositionMax[0] &&
    //             coord[3] == RoomPositionMax[1])
    //         {
    //             MapMatrix = new int[0];

    //             return;
    //         }



    //         int xMin = (coord[0] > RoomPosition[0]) ? coord[0] : RoomPosition[0]; 
    //         int xMax = (coord[2] < RoomPositionMax[0]) ? coord[2] : RoomPositionMax[0];
    //         int yMin = (coord[1] > RoomPosition[1]) ? coord[1] : RoomPosition[1];
    //         int yMax = (coord[3] < RoomPositionMax[1]) ? coord[3] : RoomPositionMax[1];
    //         int xsize = RoomPositionMax[0] - RoomPosition[0];
    //         for (int i = RoomPosition[0]-xMin; i< xMax;i++)
    //             for (int j = RoomPosition[1] -yMin; j < yMax; j++)
    //             {
    //                 int id = xsize * j + i;
    //                 MapMatrix[id] = 0;
    //             }

    //         int ysize = RoomPositionMax[1] - RoomPosition[1];
    //         for (int i =0; i< xsize;i++)
    //             for (int j = 0; j < ysize; j++)
    //                 if (MapMatrix[i] != -1) 
    //             { 
    //                     }
    //     }

    //  //MapMatrix   
    // }

    //int module(int a)
    // {
    //     if (a < 0)
    //         return -a;
    //     return a;
    // }




    // Update is called once per frame
    void Update()
    {
        if (idRoom == -1)
        {
            if (Input.GetMouseButtonDown(0) && levels[0] !=0)
            {
                if(localId != -1 && !blok)
                    OpenRoom(localId);
                // EditMatrix(true, curentPoint);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (levels[1] < levels[2])
                {
                    levels[1]++; 
                    SwitchLevel(levels[1]);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (levels[0] < levels[1])
                {
                    levels[1]--;
                    SwitchLevel(levels[1]);
                }
            }
        }
        else
        {
            if (editor && !blok)
            {
                if (lookPos != -1)
                {
                    List<Vector3> lines = roomList[idRoom].Lines;
                    if (Input.GetMouseButtonDown(0)) 
                        if (lines[lookPos] != cellPosition)
                        {
                            lines[lookPos] = cellPosition;
                            lookPos = -1;
                            // lookPosition = cellPosition;
                        }
                           


                    if (Input.GetMouseButtonDown(1))
                    {
                       // lines[lookPos] = lookPosition;
                        lookPos = -1;
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                        AddLinePoint(cellPosition);

                    if (Input.GetMouseButtonDown(1))
                        RemoveLinePoint(cellPosition);
                }
                //if (Input.GetMouseButtonDown(1))
                //{
                //    EditMatrix(false, curentPoint);
                //    //RemovePoint(curentPoint);
                //}
                //if (Input.GetMouseButtonDown(0))
                //{
                //   // EditMatrix(true, curentPoint);
                //}
            }
            else
            {
                if (Input.GetMouseButtonDown(2))
                {
                    roomList[idRoom].TextPosition = cellPosition;
                    ui.TextStorage.GetChild(idRoom).position = cellPosition;
                }
                if (Input.GetMouseButtonDown(1))
                    ClosedRoom();
            }
        }
    }

    private void FixedUpdate()
    {
        // Debug.Log($"{Input.mousePosition.x} {Input.mousePosition.y}");
        //if (Input.mousePosition.x > sB[0] &&
        //    Input.mousePosition.y > sB[1] &&
        //    Input.mousePosition.x < sB[2] &&
        //    Input.mousePosition.y < sB[3]
        //    )
        {

          //  Debug.Log($"{Input.mousePosition.x} {Input.mousePosition.y}");

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0) + vFix;
            //actualPosition = new Vector2Int(mousePos.x, mousePos.y);Mathf.CeilToInt(
            cellPosition = ui.WorldGrid.WorldToCell(mousePos);
            if (cellPosition != afterPosition && !blok)
            {
                afterPosition = cellPosition;

                if (idRoom != -1)
                {
                    if (editor )
                    {
                        List<Vector3> l = roomList[idRoom].Lines;
                        List<Vector3> lines = new List<Vector3>(new Vector3[l.Count]);
                        for (int i = 0; i < l.Count; i++)
                            lines[i] = l[i];

                        if (lookPos != -1)
                            lines[lookPos] = cellPosition;
                        else
                            lines.Add(cellPosition);

                        ViewLine(ui.Lines[idRoom], lines, idRoom);
                    }
                }
                else
                {
                    int id = SelectRoom();
                    if(id != localId)
                    {
                        localId = id;
                        if(id !=-1)
                            ViewRoom(localId);
                    }
                }
            }
        }


        if (Input.mouseScrollDelta.y < 0)
        {
            if (zoom[1] < zoom[2])
            {
                zoom[1]++;
                ui.Camera.gameObject.GetComponent<Camera>().orthographicSize = zoomMap[zoom[1]];
            }
        }
        else if (Input.mouseScrollDelta.y > 0)
            if (zoom[0] < zoom[1])
            {
                zoom[1]--;
                ui.Camera.gameObject.GetComponent<Camera>().orthographicSize = zoomMap[zoom[1]];
            }


        if (idRoom == -1 || editor)
        {
           // if(Input.GetMouseButton(0))
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // for moving camera based on input
            ui.Camera.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime; 
        }
        //gHex = new Hex(cellPosition[0], cellPosition[1]);
        //if (gHex != oldHex)
        //{
        //    oldHex = gHex;
        //    CorWorld.text = $"{cellPosition}";
        //    ViewMap.ClearAllTiles();
        //    ViewMap.SetTile(new Vector3Int(oldHex.q, oldHex.r, 0), ViewTile);
        //    // ViewMap.SetTile(Hex.Conv(oldHex), ViewTile);
        //    if (mood == "EditPoint")
        //        pathLine.SetPosition(1, WorldGrid.CellToWorld(cellPosition) + vFix);

        //}
        //if (sliderLook)
        //    UpdateSlidder();
        //if (pathMood)
        //{
        //    pathLine.SetPosition(1, mousePos);
        //}
    }
}
