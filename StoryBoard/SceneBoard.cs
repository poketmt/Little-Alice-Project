using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;

public class SceneBoard : StoryBoard
{
    public InputBridge player;  // 플레이어 객체 호출용 변수
    public string sceneName;    // 전환될 씬 이름 입력용 변수(+StartPoint클래스에서 플레이어 위치를 지정하기 위한 용도. 해당 클래스랑 같이 보면 이해할거) 
    

    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player_Min").GetComponent<InputBridge>();
    }

    // 플레이어의 curMapName 값을 sceneName으로 넣어준 후 씬 전환 실행
    public override void RunStory()
    {
        player.curMapName = sceneName;
        SceneManager.LoadScene(sceneName);
    }
}
