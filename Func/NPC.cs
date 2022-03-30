using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcType
{
    None,
    Doll,
    Ant,
    Cat,
    Fairy
}

public class NPC : MonoBehaviour
{
    [HideInInspector]
    public List<string> _nextStoryNum;      // 다음으로 실행시킬 스토리보드의 넘버

    public NpcType npcType;

    [SerializeField] Transform[] m_tfWayPoints = null;      // Nav기능 이용을 위한 웨이포인트(포지션값)들의 변수 저장용
    [SerializeField] GameObject[] workTarget = null;        // 옷장씬의 거미줄 해체작업을 위한 거미줄벽 오브젝트들 저장용

    NavMeshAgent workingNpc = null;     // NavMeshAgent기능 활용을 위한 개체 지정용

    public Action<float> endAction;

    public Animator animator;           // 각 NPC의 애니메이션 실행용 애니메이터 지정용 변수
    public Collider col_Self;           // 이벤트 트리거가 되는 콜라이더 참조용 변수
    public Transform targetPoint;       // 각종 이동을 위한 목표지점(포지션값) 저장용 변수
    public int moveSpeed;               // NPC이동 기능 사용 시 이동속도

    int m_Count = 0;        // 무브카운트. Nav기능 사용 시 다음 웨이포인트를 지정하기 위한 카운트
    int w_Count = 0;        // 워크카운트. 작업을 완료한 횟수. 지정된 웨이포인트를 왕복할 때마다 작업단계를 카운트
    int e_Count = 3;        // 엔드카운트. 작업 종료지점 지정용도

    bool workStart = false;         // WorkToNextPoint 메서드 활성화 용도
    bool workEnd = false;           // WorkEnd 메서드 활성화 용도

    private void Start()
    {
        col_Self = GetComponent<Collider>();
        workingNpc = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        // WorkStart를 실행하면 진행되는 루틴
        if (workStart == true) 
            WorkToNextPoint();
        if (workEnd == true) 
            WorkEnd();
    }

    void OnTriggerEnter(Collider col)
    {
        if (_nextStoryNum == null) return;

        switch (npcType)
        {
            case NpcType.None:
                if (col.tag == "Player")
                {
                    foreach (string s in _nextStoryNum)
                    {
                        StoryManager.Instance.RunStory(s);
                    }
                }
                break;
            case NpcType.Doll:
                if (col.tag == "Spring")
                {
                    foreach (string s in _nextStoryNum)
                    {
                        StoryManager.Instance.RunStory(s);
                    }
                }
                break;
            case NpcType.Ant:
                if (col.tag == "Candy")
                {
                    if (this.gameObject.name == "Quest_ant")
                        animator.SetTrigger("eat");
                    foreach (string s in _nextStoryNum)
                    {
                        StoryManager.Instance.RunStory(s);
                    }
                }
                break;
            case NpcType.Cat:
                if (col.tag == "Can")
                {
                    foreach (string s in _nextStoryNum)
                    {
                        StoryManager.Instance.RunStory(s);
                    }
                }
                break;
        }
    }

    // NavMeshAgent에 의한 이동을 구현할 때는 SetDestination을 사용
    // 장애물을 무시하는 다이렉트한 이동을 구현할 때는 moveStaight를 true로 작동
    public void Move(bool moveStraight)
    {
        if (moveStraight == true)
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        else 
            workingNpc.SetDestination(targetPoint.position);

        endAction(0f);
    }

    // NPC의 위치를 순간이동
    public void TeleportNpc(Transform destination)
    {
        StartCoroutine(ActiveTeleport(destination.position, destination.rotation));
    }

    IEnumerator ActiveTeleport(Vector3 npcDestination, Quaternion npcRotation)
    {
        transform.position = npcDestination;
        transform.rotation = npcRotation;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        yield return new WaitForEndOfFrame();
        endAction(0f);
    }

    // NPC의 애니메이션을 스테이트 네임(string)을 통해 실행
    public void AnimPlay(string anim)
    {
        animator.Play(anim);
        StartCoroutine(OnFrameAnim(anim));
    }

    // NPC의 애니메이션을 파라미터 네임(string)을 통해 실행
    public void AnimTrigger(string anim)
    {
        animator.SetTrigger(anim);
        StartCoroutine(OnFrameAnim(anim));
    }
    
    IEnumerator OnFrameAnim(string anim)
    {
        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
        {
            yield return new WaitForEndOfFrame();
        }
        endAction(0f);
    }

    // NPC이벤트 작동을 위한 콜라이더의 활성화
    public void EventTriggerOn()
    {
        col_Self.enabled = true;
        endAction(0f);
    }

    // NPC이벤트 중복실행을 막기 위해 실행된 이벤트 콜라이더의 비활성화
    public void EventTriggerOff()
    {
        col_Self.enabled = false;
        endAction(0f);
    }
    
    // NPC 지정작업 실행 메서드
    public void WorkStart()
    {
        workStart = true;
    }

    // NPC 지정작업
    public void WorkToNextPoint()
    {
        if (workTarget == null)
        {
            return;
        }
        animator.SetTrigger("walk");
        if (workingNpc.velocity == Vector3.zero && m_tfWayPoints.Length != 0)
        {
            workingNpc.SetDestination(m_tfWayPoints[m_Count].position);
            if ((workingNpc.transform.position - m_tfWayPoints[m_Count].position).magnitude <= 2f)
            {
                m_Count++;
            }
            if (m_Count >= m_tfWayPoints.Length)
            {
                m_Count = 0;
                workTarget[w_Count].SetActive(false);
                w_Count++;
            }
            if (w_Count == e_Count)
            {
                workEnd = true;
            }
        }
    }

    // NPC 지정작업 종료 메서드
    public void WorkEnd()
    {
        if (w_Count < e_Count)
        {
            return;
        }
        workStart = false;
        workingNpc.enabled = false;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetPoint.rotation, Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        if ((transform.position - targetPoint.transform.position).magnitude <= 1f)
        {
            transform.rotation = targetPoint.rotation;
            transform.position = targetPoint.position;
            animator.SetTrigger("idle");
            endAction(0f);
            workEnd = false;
        }
    }
}
