using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class StartPoint : MonoBehaviour
{
    public string mapName;          // 전환된 씬의 이름 체크를 위한 변수
    private InputBridge player;     // 캐릭터 객체를 가져오기 위한 변수

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
