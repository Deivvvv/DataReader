using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiLooker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MapSystem mapSystem;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GO = GameObject.Find("System");
        mapSystem = GO.GetComponent<MapSystem>();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (mapSystem != null)
            mapSystem.LoadKey(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (mapSystem != null)
            mapSystem.LoadKey(false);
    }
}
