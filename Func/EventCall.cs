using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCall : MonoBehaviour
{
    [HideInInspector]
    public List<string> _nextStoryNum;      // �������� �����ų ���丮������ �ѹ�

    public bool countRun = false;           // ī��Ʈ�� ���� �̺�Ʈ�� ����ǰ� ���� ���� üũ
    private int eventEndCount = 5;          // Event�� ����Ǳ� ���� �����ؾ� �ϴ� ��ǥ ī��Ʈ
    private int eventCount = 0;             // ���� Eventī��Ʈ Ƚ�� 

    public void OnTriggerEnter(Collider col)
    {
        if (_nextStoryNum == null) return;

        if (col.gameObject.name == "BodyTrigger")
        {
            if (countRun == false)
            {
                foreach (string s in _nextStoryNum)
                {
                    StoryManager.Instance.RunStory(s);
                }
                gameObject.SetActive(false);
            }

            else if (countRun == true)
            {
                EventCount();
            }
        }
    }

    // ���� ���丮���� ���� �� ������Ʈ ��Ȱ��ȭ
    public void NextStory()
    {
        foreach (string s in _nextStoryNum)
        {
            StoryManager.Instance.RunStory(s);
            gameObject.SetActive(false);
        }
    }

    // eventCount������ ���������� ���� NextStory����
    public void EventCount()
    {
        if (!countRun) return;

        eventCount++;
        if (eventCount >= eventEndCount)
        {
            NextStory();
        }
    }
}
