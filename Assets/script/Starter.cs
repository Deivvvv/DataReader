using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FileReader;
public class Starter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;//Limit framerate to cinematic 24fps. Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Reader.Start();

        gameObject.GetComponent<GuiScript>().LoadInfoWindow();
        Debug.Log("������� ������� ���������� ��������� 10 �������� � �������� ������ ��������  �� ������ ��������������");
        Debug.Log("����������� � ������ ���� �������������� � ������ � ������� �������� ������");
        //target frame rate =15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
