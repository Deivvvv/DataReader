using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pController : MonoBehaviour
{
    public Transform world;
    public  GameObject EchoObejct;
    public bool fly = false;
    public int forse = 10;
    public int Jumpforse = 150;
    private Rigidbody PlayerBodyRig;
    public Transform Rotator;

    public  GameObject grabItem, Grabber;

    List<EffectUint> effectUints;
    struct EffectUint
    {
        public float LifeTime;
        public GameObject Go;

        public EffectUint(GameObject go, int time)
        {
            Go = go;
            LifeTime = time;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        effectUints = new List<EffectUint>();
        PlayerBodyRig = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if(!fly)
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (grabItem == null)
            {
                Grabber.SetActive(!Grabber.active);
            }
            else
                DownObject();
        }
    }
    public void GrabItem(GameObject go)
    {
        grabItem = go;
        Grabber.SetActive(false);
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.transform.SetParent(Rotator);
    }
    void DownObject()
    {
        if (grabItem == null)
            return;
        Debug.Log(grabItem);
        GameObject go = grabItem;
        go.transform.SetParent(world);
        go.GetComponent<Rigidbody>().isKinematic = false;
        grabItem = null;
    }

    void Jump()
    {
        PlayerBodyRig.AddForce(new Vector3(0,Jumpforse,0));
        fly = true;
    }
    void FixedUpdate()
    {
        PlayerControl();
        //if (fly)
        //{
        //    if ( gameObject.GetComponent<Rigidbody>().velocity[1] < 0.1f)
        //    {
        //        Debug.Log(fly);
        //        fly = false;
        //        Echo(2);
        //    }
        //}
        //Test();

        EffectManager();
       // Echo(5);
    }
    void EffectManager()
    {
        if (effectUints.Count > 0)
        {
            for(int i=0;i < effectUints.Count; i++)
            {
                EffectUint u = effectUints[i];
                u.LifeTime -= 1f * Time.deltaTime;
                effectUints[i] = u;
                //Debug.Log(u.LifeTime);
                if (u.LifeTime <= 0f)
                {
                    effectUints.RemoveAt(i);
                    i--;
                    Destroy(u.Go);
                }
            }
        }
    }
    void PlayerControl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float cof = 0.2f;

        if (horizontal  > cof||
            horizontal < -cof ||
          vertical > cof ||
          vertical < -cof )
        {
            Vector3 v = new Vector3(horizontal, 0, vertical);
            
                Rotator.rotation = Quaternion.Slerp(Rotator.rotation, Quaternion.LookRotation(v), 5 * Time.deltaTime);

            PlayerBodyRig.AddForce(v * forse);
        }

        // for moving camera based on input
        //ui.Camera.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;


    }

    void CreateEffect(string tayp, int time, int size)
    {
        GameObject go = Instantiate(EchoObejct, transform.position, transform.rotation);
        EffectUint u = new EffectUint(go, time);
        effectUints.Add(u);

        go.transform.DOScale(size, time);

    }
    //IEnumerator Echo()//(int time, int size)
    //{
    //    int time =5,  size = 10;
    //    GameObject go = Instantiate(EchoObejct, transform.position, transform.rotation);

    //    Debug.Log("Start");
    //    go.transform.DOScale(size, time);
    //    yield return new WaitForSeconds(time);
    //    //  Destroy(go);
    //}
    public void FlyObject(GameObject go)
    {
        if(go.tag == "Floor")
        {
            CreateEffect("Echo", 2, 10);
        }
        fly = false;
    }
    void Test()
    {
        // Bit shift the index of the layer (8) to get a bit mask
       // int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
      //  layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        bool curentFly = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2);
      //  Debug.Log(hit.distance);
        if (fly)
        {
            // Debug.Log(hit.distance > 1f);
            if (hit.distance < 0.9f)
            {
                fly = false;
                if(gameObject.GetComponent<Rigidbody>().velocity[1] <0)
                // Echo(2, 10);
                // int[] ints = { 2, 10 };;
                CreateEffect("Echo", 2, 10);
                //StartCoroutine("Echo");
               // Debug.Log("Start");
            }
        }
        //else
        //{
        //    if (hit.distance < 1.05f)
        //    {
        //        fly = false;
        //       // Echo(2);
        //    }
        //}
        //Debug.Log(hit.distance);
        //if(curentFly && fly)
        //{

        //    if (!fly)
        //    {
        //        Echo(2);
        //    }

        //    fly = curentFly;
        //}
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit :" + hit.distance);
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}
    }
}
