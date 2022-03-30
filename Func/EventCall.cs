using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCall : MonoBehaviour
{
    [HideInInspector]
    public List<string> _nextStoryNum;      // 다음으로 실행시킬 스토리보드의 넘버

    public bool countRun = false;           // 카운트를 통해 이벤트가 실행되게 할지 유무 체크
    private int eventEndCount = 5;          // Event가 실행되기 위해 도달해야 하는 목표 카운트
    private int eventCount = 0;             // 현재 Event카운트 횟수 

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

    // 다음 스토리보드 실행 후 오브젝트 비활성화
    public void NextStory()
    {
        foreach (string s in _nextStoryNum)
        {
            StoryManager.Instance.RunStory(s);
            gameObject.SetActive(false);
        }
    }

    // eventCount조건을 만족시켰을 때만 NextStory실행
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
