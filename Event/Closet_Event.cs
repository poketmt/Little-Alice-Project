using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet_Event : StoryBoard
{
    public Transform webMainObj;
        
    private const int eventEndCount = 4;    // �̺�Ʈ�� ����Ǵ� ī��Ʈ ��
    private int grabCount = 0;              // �Ź����� ��ƶ��� Ƚ�� ī��Ʈ
    private bool IsClosetEvent = false;     // �̺�Ʈ ���� ����

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
