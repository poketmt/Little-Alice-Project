using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcState
{
    None,
    Move,        // NPC 지점 이동
    Teleport,    // NPC 텔레포트
    AnimPlay,    // NPC 애니메이션 재생
    AnimTrigger, // NPC 애니메이션 트리거 재생
    Work,        // NPC 부여된 작업 시작
}

public class NPCBoard : StoryBoard
{
    public NpcState npcState;            // Enum타입 변수 저장

    public NPC npc;                      // 타겟 NPC 객체 생성
    public Transform targetDestination;  // NPC 텔레포트 시킬 위치

    public string callAnim;              // 애니메이션 파라미터 호출용 변수
    public bool moveStraight = false;    // NavMeshAgent의 기능과 MoveTowards를 사용한 두 가지 이동방식 중 상황에 맞는 것을 선택

    public override void Start()
    {
        base.Start();
        if (npc != null)
        npc._nextStoryNum = nextStoryNum;
    }

    public override void RunStory()
    {
        base.RunStory();
        npc.endAction = EndStory;
        switch (npcState)
        {
            case NpcState.Move:
                npc.Move();
                break;
            case NpcState.Teleport:
                npc.TeleportNpc(targetDestination);
                break;
            case NpcState.AnimPlay:
                npc.AnimPlay(callAnim);
                break;
            case NpcState.AnimTrigger:
                npc.AnimTrigger(callAnim);
                break;
            case NpcState.Work:
                npc.WorkStart();
                break;
        }
    }

    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
