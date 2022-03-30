using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConversationType
{
    Main,
    MissionMain,
    Mission,
    Conversation
}

public class ConversationBoard : StoryBoard
{
    [Header("Conversation Board")]
    //public GameObject npc;
    public ConversationType conversationType;
    public string dialougue;
    public int characterIndex;
    public float delayTime;

    float minLifetime = 3f;
    float maxLifetime = 8f;
    float lifetimeSpeed = 0.25f;
    
    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        UIManager.Instance.endAction = EndStory;
        switch (conversationType)
        {
            case ConversationType.Main:
                UIManager.Instance.UpdateMainText(dialougue);
                break;
            case ConversationType.MissionMain:
                UIManager.Instance.UpdateMissonText(dialougue, true);
                break;
            case ConversationType.Mission:
                UIManager.Instance.UpdateMissonText(dialougue, false);
                break;
            case ConversationType.Conversation:
                UIManager.Instance.UpdateCharacterText(dialougue, CalculateTextLifetime(), characterIndex, delayTime);
                //StartCoroutine(DelayAction(delayTime));
                break;
        }

    }

    float CalculateTextLifetime ()
    {
        float time = dialougue.Length * lifetimeSpeed;

        if (time < minLifetime) time = minLifetime;
        else if (time > maxLifetime) time = maxLifetime;

        return time;
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    public override void EndStory(float delayTime)
    {
        base.EndStory(delayTime);
    }
}
