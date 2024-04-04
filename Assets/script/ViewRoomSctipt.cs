using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewRoomSctipt : MonoBehaviour, IPointerEnterHandler
{
    public MapSystem mapSystem;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GO = GameObject.Find("System");
        mapSystem = GO.GetComponent<MapSystem>();
    }
    public void SetId(int i) { id = i; }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (mapSystem != null)
            mapSystem.ViewRoom(id, true);
    }
}
