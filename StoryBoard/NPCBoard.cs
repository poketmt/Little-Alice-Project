using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcState
{
    None,
    Move,        // NPC ���� �̵�
    Teleport,    // NPC �ڷ���Ʈ
    AnimPlay,    // NPC �ִϸ��̼� ���
    AnimTrigger, // NPC �ִϸ��̼� Ʈ���� ���
    Work,        // NPC �ο��� �۾� ����
}

public class NPCBoard : StoryBoard
{
    public NpcState npcState;            // EnumŸ�� ���� ����

    public NPC npc;                      // Ÿ�� NPC ��ü ����
    public Transform targetDestination;  // NPC �ڷ���Ʈ ��ų ��ġ

    public string callAnim;              // �ִϸ��̼� �Ķ���� ȣ��� ����
    public bool moveStraight = false;    // NavMeshAgent�� ��ɰ� MoveTowards�� ����� �� ���� �̵���� �� ��Ȳ�� �´� ���� ����

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
