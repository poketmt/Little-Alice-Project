using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private static StoryManager _instance;

    public static StoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StoryManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<StoryManager>();
        }
        if (_instance != this)
        {
            Destroy(this);
        }
    }

    // 1.0.0 -> 1.0.1 -> 1.1.0
    // public string storyNumber;

    public List<StoryBoard> storyBoards;

    public void Start()
    {
        storyBoards.AddRange(FindObjectsOfType<StoryBoard>());

        // RunStory("1.0.0");          // ���� �������ڸ��� �ٷ� �̺�Ʈ�� ������ ���
    }

    public void RunStory (string _storyNum)
    {
        // storyBoards ����Ʈ�� �ִ� ��� �׸��� ��ȸ
        // ���� x ��ü�� storyNum �ʵ��� ���� _storyNum�� ���ٸ�
        // ���� ���� StoryBoard b = storyBoards.FindAll(x => x.storyNum == _storyNum);
        // ���� ����ȭ
        List<StoryBoard> b = storyBoards.FindAll(x => x.storyNum == _storyNum);
        // print("Next Running StoryBoard Num=" + _storyNum);
        foreach (StoryBoard bs in b)
        if (bs != null) bs.RunStory();
    }

    
}
