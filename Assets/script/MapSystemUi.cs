using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Tilemaps;

public class MapSystemUi : MonoBehaviour
{
    public Transform Camera;
    public InputField NameFlied;
    public Text LevelText;

    public GameObject OrigText;
    public GameObject OrigTileMap;
    public GameObject OrigLine;
    public GameObject OrigButton;
    public GameObject OrigPoint;

    public GridLayout WorldGrid;//мировая сетка

    public List<LineRenderer> Lines;

    public Transform ButtonStorage;
    public Transform TextStorage;
    public Transform LineStorage;
    public Transform PointStorage;

    public Transform RoomList;
    public Transform RedactorWindow;

    public Button CreateRoomButton;
    public Button SaveButton;

    public GameObject RoomListWindow;
}