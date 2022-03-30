using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtQuestPoint : MonoBehaviour
{

    public GameObject[] lookAtTarget;   // 바라봐야 할 타겟이 될 게임오브젝트들 저장용 변수
    public LayerMask QuestTarget;       // 퀘스트 안내(Indicator)가 종료되는 조건 레이어타겟
    public int questNum = 0;            // lookAtTarget이 작동할 순서를 결정할 인덱서 역할용 변수

    public string[] storyNumber;        // 다음 StoryBoard 실행을 위한 변수

    void Update()
    {
        if (lookAtTarget != null)
        {
            if (questNum > lookAtTarget.Length-1)
            {
                return;
            }
            // lookAtTarget[]에 저장된 'questNum'번째 오브젝트를 식별하여 활성화 상태라면 LookDestination을 실행
            if (lookAtTarget[questNum].activeSelf == true)
            {
                LookDestination();
            }
        }
    }

    // 지정된 레이어마스크를 바라보았을 때 다음 스토리보드를 실행
    // questNum을 증가시켜 다음 lookAtTarget을 대기상태로
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
