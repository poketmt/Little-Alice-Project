using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet_Event : StoryBoard
{
    public Transform webMainObj;
        
    private const int eventEndCount = 4;    // 이벤트가 종료되는 카운트 값
    private int grabCount = 0;              // 거미줄을 잡아뜯은 횟수 카운트
    private bool IsClosetEvent = false;     // 이벤트 시작 유무

    public void StartWeb()
    {
        IsWebEvent = true;
    }

    public void EndWeb()
    {
        IsWebEvent = false;
        base.EndStory(2f);
    }

    public void GrabWeb ()
    {
        if (!IsWebEvent) return;
        grabCount++;
        if (grabCount >= eventEndCount)
        {
            EndWeb();
        }
    }
}
