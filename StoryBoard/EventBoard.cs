public enum RunTypeState
{
    Other,
    Self
}

public class EventBoard : StoryBoard
{
    [Header("StoryRunType")]
    public RunTypeState runTypeState;

    [Header("Event Board")]
    public UnityEvent eventRun;             // UnityEvent 기능 호출을 위한 객체 생성
    
    public EventCall eventCall;             // EventCall 기능 호출을 위한 객체 생성
    
    public float delayTime;                 // 다음 스토리보드 실행의 지연시간을 지정하기 위한 변수

    public override void Start()
    {
        base.Start();
    }

    public void OnEnable()
    {
        ExecutantCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        eventCall.OnTriggerEnter(other);
    }

    public override void RunStory()
    {
        base.RunStory();
        DecisionRunEvent();
    }

    public override void EndStory(float delayTime)
    {
        base.EndStory(delayTime);
    }

    // RunStory가 실행되었을 때 EventBoard자신의 EndStory를 실행할 것인지 참조시킨 EventCall을 통해 실행할 것인지 결정
    // (이벤트 실행주체 그리고 실행조건을 즉시 실행에 둘 것인지 collider접촉에 따른 Trigger발동에 둘 것인지 차이)
    void ExecutantCheck()
    {
        switch (eventRunType)
        {
            case EventRunType.Other:
                if (eventCall != null)
                {
                    eventCall._nextStoryNum = nextStoryNum;
                }
                break;
            case EventRunType.Self:
                break;
        }
    }

    void RunEvent()
    {
        eventRun.Invoke();
    }

    void RunStorySelf()
    {
        switch (eventRunType)
        {
            case EventRunType.Other:
                break;
            case EventRunType.Self:
                EndStory(delayTime);
                break;
        }
    }

    void DecisionRunEvent()
    {
        ExecutantCheck();
        RunEvent();
        RunStorySelf();
    }
}