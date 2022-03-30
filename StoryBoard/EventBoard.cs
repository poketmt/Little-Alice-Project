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
    public UnityEvent eventRun;             // UnityEvent ��� ȣ���� ���� ��ü ����
    
    public EventCall eventCall;             // EventCall ��� ȣ���� ���� ��ü ����
    
    public float delayTime;                 // ���� ���丮���� ������ �����ð��� �����ϱ� ���� ����

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

    // RunStory�� ����Ǿ��� �� EventBoard�ڽ��� EndStory�� ������ ������ ������Ų EventCall�� ���� ������ ������ ����
    // (�̺�Ʈ ������ü �׸��� ���������� ��� ���࿡ �� ������ collider���˿� ���� Trigger�ߵ��� �� ������ ����)
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