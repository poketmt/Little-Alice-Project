using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    AnimPlay,
    AnimTrigger,
    AnimBool
}

public class AnimatorBoard : StoryBoard
{
    public AnimState animState;

    public Animator targetAnimator;         // 타겟 애니메이터 지정용 변수
    public Animation targetAnimation;       // 타겟 애니메이션 지정용 변수
    public AnimationClip animClip;          // 재생할 애니메이션 클립 지정용 변수

    public string callAnim;                 // 재생할 애니메이션의 파라미터 입력용 변수

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        AnimationManager.instance.endAction = EndStory;

        switch (animState)
        {
            case AnimState.AnimPlay:
                AnimationManager.instance.AnimPlay(targetAnimation, animClip, callAnim);
                break;
            case AnimState.AnimTrigger:
                AnimationManager.instance.AnimTrigger(targetAnimator, callAnim);
                break;

        }
    }

    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
