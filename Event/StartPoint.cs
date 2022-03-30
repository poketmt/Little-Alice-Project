using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class StartPoint : MonoBehaviour
{
    public string mapName;          // ��ȯ�� ���� �̸� üũ�� ���� ����
    private InputBridge player;     // ĳ���� ��ü�� �������� ���� ����

    void Start()
    {
        player = FindObjectOfType<InputBridge>();
        
        if (mapName == player.curMapName)
        {
            player.transform.position = this.transform.position;
            player.transform.rotation = this.transform.rotation;
        }
    }
   
}
