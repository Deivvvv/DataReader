using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolerGame : MonoBehaviour
{
    class ScrolerUi
    {
        public GameObject[] PlayerBody;
        public Transform ActiveItem;
        public Transform OffItem;
        public GameObject OrigItem;
    }

    class GameItem
    {
        SideScrolerGame game;
        int ItemId;

    }
    // Start is called before the first frame update

    /*
     �������� ��������� �������� ��������
    ��������� �������, � � �����, � ���������
    ������� �������� ����, ����� � ���� ��������������� ����, � ������� ������������ ���-�� ������� � ����� ����� ���������
    ������
    ��������� �������� �� �������, ���� ������ �������� ������ ���. � ���� ������ ��������� ���������� �������, ���� �� ����� ����� �� ����� � ������ ��������� ��������, �� ������� ��������� � �������

    ������� �������� ��� ����� �����, � ������ ����� ���������� � ���������, ��� ����������� � ���������� ����� ������. ����� ����� ���������� ������ � ������������� �������, ��� �������� ��������, ����� . ��� ��������� �������� ������ �������, ����� ������ 
     */
    class Stage
    {
        public List<Vector3Int> Path = new List<Vector3Int>();

       public  void Sort()
        {
            List<Vector3Int> newPath = new List<Vector3Int>();

            while(Path.Count > 0)
            {
                int r = Random.Range(0, Path.Count);
                newPath.Add(Path[r]);
                Path.RemoveAt(r);
            }
            Path = newPath;
        }
    }
    class Biom
    {
        public string Name;
        public List<string> Item;//������ ���������
        public List<int> ItemId;//������ ���������
        public List<int> Size;//����������� ���������� �������� �� �������
        public List<int> SizeClount;//������ �����
        public List<int> ItemTime;//����� �� ������� ��������
    }

    private ScrolerUi ui;
    private GameObject[] PlayerBody;
    List<int> PlayerItem;
    List<int> PlayerItemSize;

    List<GameObject> activeItem;
    List<GameObject> activeItemTime;

    private List<Biom> biomsList;
    private List<Stage> stagesList;

    float gameTime;
    Vector2Int nextTime;
    int gameSatage;
    float gameMultipler;
    bool endStage =false;
    //�������� (����:����-������������-���������) + ��������� ����� �� ��������� ����

    Stage CreateStage(int id ,int size, int hard )
    {

        Vector3Int addItem(int id, int item)
        {
            return new Vector3Int(biomsList[id].ItemId[item], biomsList[id].ItemTime[item], biomsList[id].SizeClount[item]);
        }

        Stage stage = new Stage();
        List<Vector3Int> path = new List<Vector3Int>();

        int r = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < biomsList[id].Item.Count; j++)
            {
                r = Random.Range(0, 100);
                if (r < biomsList[id].Size[j])
                    path.Add(addItem(id, j));

            }
        }
        stage.Path = path;
        stage.Sort();

        return stage;
    }
    void CreatePathScene()
    { 
        stagesList = new List<Stage>();
        stagesList.Add(CreateStage(0, 1,1));
        stagesList.Add(CreateStage(0, 1, 1));
        stagesList.Add(CreateStage(0, 3, 2));
        stagesList.Add(CreateStage(0, 1, 4));


        gameMultipler = 1.5f;// \|/
       // float gameMultipler = path.Count + 0.1f * (path.Count - 1);
    }

    void PlayPath() //CreatePath(List<Vector3Int> path, float multi)
    {
        gameSatage = 0;
        nextTime = new Vector2Int(0, 0);
    }
    void NextPath()
    {
        if(nextTime[1]+1 == stagesList[nextTime[0]].Path.Count)
        {
            int id = nextTime[0] + 1;
            if(id > stagesList.Count)
            {
                Exit();
                return;
            }
            Debug.Log("���������, ���� ���-�� ������� ��������� ����� ����� 0");

            nextTime = new Vector2Int(id, 0);
            MovePath();
        }



    }
    void MovePath()
    {
        Debug.Log("��������� ������ � ���������� ����� �����");
    }

    void ConnectPlayer()
    {
        PlayerBody[0] =ui.PlayerBody[0];
        PlayerBody[1] = ui.PlayerBody[1];


        PlayerItem = new List<int>();
        PlayerItemSize = new List<int> ();
    }
    void PlayerControl()
    {
        
    }
    void Exit()
    {
        Debug.Log("!connectBd");
    }
    void CreateItem()
    {

    }
    void GrabItem(int idItem, GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(ui.OffItem);

        int id = PlayerItem.FindIndex(x => x ==idItem);
        if (id == -1)
        {
            id = PlayerItem.Count;
            PlayerItem.Add(idItem);
        }

        if (endStage)
        {
            if(ui.ActiveItem.childCount == 0)
            {

            }
        }
    }
    void Start()
    {
        Debug.Log("!connectBd");
        CreatePathScene();
        ConnectPlayer();
        PlayPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        PlayerControl();
    }

    IEnumerator ExecuteAfterTime(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        //������� ������

        CreateItem(); 
        NextPath();

    }
}
