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
    public string storyNum;                 // ���丮���庰 ��ü �ĺ��� ���丮��ȣ 
    public StartType startType;

    [Header("Next Board")]
    public List<string> nextStoryNum;       // �������� �����ų ���丮������ ��ȣ

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
        // ���� ��ȣ ���� if (nextStoryNum != null) StoryManager.Instance.RunStrory(nextStoryNum);
        // nextStoryNum �迭(����Ʈ ��)�� ��� �׸� �� �ϳ��� s��� ����
        // ��� �׸񸶴� �����θ� ������
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
