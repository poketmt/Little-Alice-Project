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

        // RunStory("1.0.0");          // 씬이 시작하자마자 바로 이벤트를 진행할 경우
    }

    public void RunStory (string _storyNum)
    {
        // storyBoards 리스트에 있는 모든 항목을 조회
        // 만약 x 객체의 storyNum 필드의 값이 _storyNum과 같다면
        // 단일 실행 StoryBoard b = storyBoards.FindAll(x => x.storyNum == _storyNum);
        // 동시 실행화
        List<StoryBoard> b = storyBoards.FindAll(x => x.storyNum == _storyNum);
        // print("Next Running StoryBoard Num=" + _storyNum);
        foreach (StoryBoard bs in b)
        if (bs != null) bs.RunStory();
    }

    
}
