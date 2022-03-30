using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StartType
{
    None,
    Start
}


public class StoryBoard : MonoBehaviour
{
    [Header("Story Board")]
    public string storyNum;                 // 스토리보드별 개체 식별용 스토리번호 
    public StartType startType;

    [Header("Next Board")]
    public List<string> nextStoryNum;       // 다음으로 실행시킬 스토리보드의 번호

    public virtual void Start()
    {
        if (startType == StartType.Start)
        {
            RunStory();
        }
    }

    public virtual void RunStory()
    {
        print("RunStory: " + storyNum);
    }

    void EndStoryAction()
    {
        // 단일 번호 실행 if (nextStoryNum != null) StoryManager.Instance.RunStrory(nextStoryNum);
        // nextStoryNum 배열(리스트 등)의 모든 항목 중 하나를 s라고 하자
        // 모든 항목마다 구현부를 실행함
        foreach (string s in nextStoryNum)
        if (s != null)
            StoryManager.Instance.RunStory(s);
    }

    public virtual void EndStory(float time)
    {
        if (nextStoryNum != null)
        {
            Invoke("EndStoryAction", time);
        }
    }
}
