using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtQuestPoint : MonoBehaviour
{

    public GameObject[] lookAtTarget;   // �ٶ���� �� Ÿ���� �� ���ӿ�����Ʈ�� ����� ����
    public LayerMask QuestTarget;       // ����Ʈ �ȳ�(Indicator)�� ����Ǵ� ���� ���̾�Ÿ��
    public int questNum = 0;            // lookAtTarget�� �۵��� ������ ������ �ε��� ���ҿ� ����

    public string[] storyNumber;        // ���� StoryBoard ������ ���� ����

    void Update()
    {
        if (lookAtTarget != null)
        {
            if (questNum > lookAtTarget.Length-1)
            {
                return;
            }
            // lookAtTarget[]�� ����� 'questNum'��° ������Ʈ�� �ĺ��Ͽ� Ȱ��ȭ ���¶�� LookDestination�� ����
            if (lookAtTarget[questNum].activeSelf == true)
            {
                LookDestination();
            }
        }
    }

    // ������ ���̾��ũ�� �ٶ󺸾��� �� ���� ���丮���带 ����
    // questNum�� �������� ���� lookAtTarget�� �����·�
    public void LookDestination()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, Mathf.Infinity, QuestTarget))
        {
            lookAtTarget[questNum].SetActive(false);
            StoryManager.Instance.RunStory(storyNumber[questNum]);
            if(questNum < lookAtTarget.Length-1)
                questNum++;
        }
    }
}
