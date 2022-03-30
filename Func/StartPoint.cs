using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

// �� ��ȯ �� �÷��̾��� ��ġ�� �����ϱ� ���� ���
public class StartPoint : MonoBehaviour
{
    public string mapName;          // ��ȯ�� ���� �̸� üũ�� ���� ����
    private InputBridge player;     // �÷��̾� ��ü�� �������� ���� ����

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
