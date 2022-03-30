using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;

public class SceneBoard : StoryBoard
{
    public InputBridge player;  // �÷��̾� ��ü ȣ��� ����
    public string sceneName;    // ��ȯ�� �� �̸� �Է¿� ����(+StartPointŬ�������� �÷��̾� ��ġ�� �����ϱ� ���� �뵵. �ش� Ŭ������ ���� ���� �����Ұ�) 
    

    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player_Min").GetComponent<InputBridge>();
    }

    // �÷��̾��� curMapName ���� sceneName���� �־��� �� �� ��ȯ ����
    public override void RunStory()
    {
        player.curMapName = sceneName;
        SceneManager.LoadScene(sceneName);
    }
}
