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
    public List<string> _nextStoryNum;      // �������� �����ų ���丮������ �ѹ�

    public NpcType npcType;

    [SerializeField] Transform[] m_tfWayPoints = null;      // Nav��� �̿��� ���� ��������Ʈ(�����ǰ�)���� ���� �����
    [SerializeField] GameObject[] workTarget = null;        // ������� �Ź��� ��ü�۾��� ���� �Ź��ٺ� ������Ʈ�� �����

    NavMeshAgent workingNpc = null;     // NavMeshAgent��� Ȱ���� ���� ��ü ������

    public Action<float> endAction;

    public Animator animator;           // �� NPC�� �ִϸ��̼� ����� �ִϸ����� ������ ����
    public Collider col_Self;           // �̺�Ʈ Ʈ���Ű� �Ǵ� �ݶ��̴� ������ ����
    public Transform targetPoint;       // ���� �̵��� ���� ��ǥ����(�����ǰ�) ����� ����
    public int moveSpeed;               // NPC�̵� ��� ��� �� �̵��ӵ�

    int m_Count = 0;        // ����ī��Ʈ. Nav��� ��� �� ���� ��������Ʈ�� �����ϱ� ���� ī��Ʈ
    int w_Count = 0;        // ��ũī��Ʈ. �۾��� �Ϸ��� Ƚ��. ������ ��������Ʈ�� �պ��� ������ �۾��ܰ踦 ī��Ʈ
    int e_Count = 3;        // ����ī��Ʈ. �۾� �������� �����뵵

    bool workStart = false;         // WorkToNextPoint �޼��� Ȱ��ȭ �뵵
    bool workEnd = false;           // WorkEnd �޼��� Ȱ��ȭ �뵵

    private void Start()
    {
        col_Self = GetComponent<Collider>();
        workingNpc = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        // WorkStart�� �����ϸ� ����Ǵ� ��ƾ
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

    // NavMeshAgent�� ���� �̵��� ������ ���� SetDestination�� ���
    // ��ֹ��� �����ϴ� ���̷�Ʈ�� �̵��� ������ ���� moveStaight�� true�� �۵�
    public void Move(bool moveStraight)
    {
        if (moveStraight == true)
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        else 
            workingNpc.SetDestination(targetPoint.position);

        endAction(0f);
    }

    // NPC�� ��ġ�� �����̵�
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

    // NPC�� �ִϸ��̼��� ������Ʈ ����(string)�� ���� ����
    public void AnimPlay(string anim)
    {
        animator.Play(anim);
        StartCoroutine(OnFrameAnim(anim));
    }

    // NPC�� �ִϸ��̼��� �Ķ���� ����(string)�� ���� ����
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

    // NPC�̺�Ʈ �۵��� ���� �ݶ��̴��� Ȱ��ȭ
    public void EventTriggerOn()
    {
        col_Self.enabled = true;
        endAction(0f);
    }

    // NPC�̺�Ʈ �ߺ������� ���� ���� ����� �̺�Ʈ �ݶ��̴��� ��Ȱ��ȭ
    public void EventTriggerOff()
    {
        col_Self.enabled = false;
        endAction(0f);
    }
    
    // NPC �����۾� ���� �޼���
    public void WorkStart()
    {
        workStart = true;
    }

    // NPC �����۾�
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

    // NPC �����۾� ���� �޼���
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
