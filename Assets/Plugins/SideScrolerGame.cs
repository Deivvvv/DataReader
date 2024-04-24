using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using DG.Tweening.Core;
//using DG.Tweening.Plugins.Core.PathCore;
//using DG.Tweening.Plugins.Options;
using DG.Tweening;

public class SideScrolerGame : MonoBehaviour
{
    static class BD 
    {
        static List<GameItem> GameItems;
        static List<Sprite> Sprites;
       public struct GameItem
        {
            public string Name;
            public string IdName;
            public int StartTime, EndTime;
            public Sprite FirstIcon;//базова€ иконка
            public Sprite DestroyIcon;//уничтоженан€ иконка

            public GameItem(string str, string id, int a, int b)
            {
                Name = str;
                IdName = id;
                StartTime = a;
                EndTime = b;
                FirstIcon = FindIcon(id);
                DestroyIcon = FindIcon(id +"-Des");

            }
        }
        


        static Sprite FindIcon(string str)
        {
            int id = Sprites.FindIndex(x => x.name == str);
            if (id != -1)
            {
                return Sprites[id];
            }
            return null;
        }

        public static void Start()
        {
            Debug.Log("!connectBd");
            GameItems = new List<GameItem>();
            GameItems.Add(new GameItem("ћука","Four", 7,3));
            GameItems.Add(new GameItem("ћолоко", "Milk", 7, 3));
            GameItems.Add(new GameItem("яица", "Egs", 7, 3));
        }


        public static GameItem GetGameItem(int id)
        {
            return GameItems[id];
        }

    }

    class ScrolerUi
    {
        public GameObject[] PlayerBody;
        public Transform ActiveItem, BufferItem, OffItem, StartPosition;
        public GameObject OrigItem, OrigGridPoint;
    }

    class GameItemCase : MonoBehaviour
    {
        SideScrolerGame game;
        int Id;
        void OnCollisionEnter(Collision collisionInfo)//Stay(Collision collisionInfo)
        {
            //  transform.DOMove(transform)
            //transform.DOMove();

            Debug.Log("Colision");
            {
                game.GrabItem(Id, gameObject);
            }
           // collisionInfo.gameObject.get
        }


        //Transform offItem;
        //GameObject Colider;
        //SideScrolerGame game;
        //int itemId;
        //public int Time, TimeEnd;


        //public GameItemCase(SideScrolerGame _game, int id, int time, int timeEnd, Transform _offItem)
        //{
        //    game = _game;
        //    itemId = id;
        //    Time = time;
        //    TimeEnd = timeEnd;
        //}
        //void useTime()
        //{
        //    Colider.SetActive(true);
        //}
        //void Check()//стоит ли игрок в зоне объекта
        //{
        //    game.GrabItem(itemId, gameObject);
        //}
        //IEnumerator EndTime()
        //{
        //    gameObject.transform.SetParent(offItem);
        //    //gameObject.GetComponent<SpriteRenderer>().sprite = BD.GetItemIcon( itemId, 1);//передаем номер и состо€ние(уничтоженно)
        //    yield return new WaitForSeconds(2);

        //    gameObject.SetActive(false);
        //}
    }
    // Start is called before the first frame update

    /*
     передаем настройки будущего маршрута
    указываем локацию, и еЄ длину, еЄ сложность
    система образует фазу, затем в фазе устанавливаютс€ шаги, в которых определаетс€ кол-во товаров и врем€ межуд посылками
    процеп
    персонажи приход€т на учатсок, Ѕлэр кидает продукты каждый шаг. в этот момент создаютс€ отложенные посылки, если гг будет сто€т на точке в момент окончани€ доставки, то посылка добавитс€ в корзину

    посылки выгл€д€т как черые точки, в начале точка прозрачан€ и маленька€, она расшир€етс€ и становитс€ более полной. когда точка становитс€ черной и максимального размера, она начинает сужатьс€, когда . она достигает половину своего размера, товар падает 
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
        public List<string> Item;//список продуктов
        public List<int> ItemId;//список продуктов
        public List<int> Size;//веро€тность добавлени€ продукта на очередь
        public List<int> SizeClount;//размер пачки
        public List<int> ItemTime;//врем€ на падени€ предмета
    }

    private ScrolerUi ui;
    private GameObject[] PlayerBody;
    private Rigidbody2D PlayerBodyRig;
    List<int> PlayerItem;
    List<int> PlayerItemSize;

    List<GameObject> activeItem;
    List<GameObject> activeItemTime;



    private List<Biom> biomsList;
    private List<Stage> stagesList;

    public Transform[] Grid;

    float gameTime;
    Vector2Int nextTime;
    int gameSatage;
    float gameMultipler;
    bool endStage =false;
    //сецнарий (этап:биом-длительность-сложность) + множитель пачек от сложности игры

    GameObject GetItemBody()
    {
        GameObject go = null;
        if(ui.OffItem.childCount > 0)
        {
            go = ui.OffItem.GetChild(0).gameObject;
        }
        else
        {
            go = Instantiate(ui.OrigItem);
        }
        go.transform.SetParent(ui.ActiveItem);

        return go;
    }
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
            if (!endStage) {
                endStage = true;
                return;
                    }
            endStage = false;
           // Debug.Log("ѕодождпть, пока кол-во актиных предметов будет равно 0");

            nextTime = new Vector2Int(id, 0);
            MovePath();
        }

        ExecuteAfterTime(stagesList[nextTime[0]].Path[nextTime[1]]);

    }
    void MovePath()
    {
        Debug.Log("јнимируем преход к следующему месту сбора");
    }

    void CreateGrid()
    {
        Vector2Int size = new Vector2Int(2, 2);
        Vector2Int eage = new Vector2Int(3, 3);
        Grid = new Transform[eage[0] * eage[1]];
      //  Grid[0] = ui.StartPosition;
        for (int i = 0; i < eage[0]; i++)
            for (int j = 0; j < eage[1]; j++)
            {
                Grid[i*eage[0]+j] = Instantiate(ui.StartPosition);
                Grid[i * eage[0] + j].position = ui.StartPosition.position + new Vector3(i * size[0] , j * size[1],0);
            }
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // for moving camera based on input
        PlayerBodyRig.AddForce(new Vector3(horizontal, vertical, 0));
        //ui.Camera.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
    }
    void Exit()
    {
        Debug.Log("!connectBd");
    }
   
    public void GrabItem(int idItem, GameObject go)
    {
        ClosedItem(idItem, go, false);

        int id = PlayerItem.FindIndex(x => x ==idItem);
        if (id == -1)
        {
            id = PlayerItem.Count;
            PlayerItem.Add(idItem);
            PlayerItemSize.Add(0);
        }
        PlayerItemSize[id]++;
      //  ClosedItem(idItem, go);

        if (endStage)
        {
            if(ui.ActiveItem.childCount == 0)
            {
                NextPath();
            }
        }
    }

    IEnumerator ClosedItem(int id, GameObject go, bool shorts)
    {
        // ui.ActiveItem
        if (!shorts)
        {
            go.transform.SetParent(ui.BufferItem);
            go.GetComponent<SpriteRenderer>().sprite = BD.GetGameItem(id).DestroyIcon;//передаем номер и состо€ние(уничтоженно)
            yield return new WaitForSeconds(2);
        }

        go.SetActive(false);
        go.transform.SetParent(ui.OffItem);
    }

    void Start()
    {
        BD.Start();
        CreateGrid();
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

    IEnumerator CreateItem(int id)
    {
        GameObject go = GetItemBody();
        BD.GameItem item = BD.GetGameItem(id);
        go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = item.FirsIcon;

        go.transform.position = PlayerBody[1].transform.position;
        Vector3 newV =    new Vector3(Random.Range(2,4), Random.Range(2, 4),0);//случайна€ позици€

        DOTween.KillAll(go);//отчистка аниматора
        go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        go.GetComponent<SpriteRenderer>().color = new Color(255,255,255,100);

        go.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 200), item.StartTime); 

        //SetAnimator(item)//настравиваем полный цикл анимаций
        yield return new WaitForSeconds(item.StartTime);
        go.transform.GetChild(0).gameObject.SetActive(true);
       // item.
        yield return new WaitForSeconds(item.EndTime);

    }
    IEnumerator ExecuteAfterTime(Vector3Int v)
    {
        yield return new WaitForSeconds(v[1]);
        for(int i =0;i<v[2];i++)
            CreateItem(v[0]);

        NextPath();

    }
}
