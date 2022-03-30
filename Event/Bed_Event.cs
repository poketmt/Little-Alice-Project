using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Bed_Event : MonoBehaviour
{
    public SmoothLocomotion locomotion;     // SmoothLocomotion 기능 호출용 객체 생성
    public GameObject[] searchPoints;       // 아이템 포인트들이 활성화될 위치 저장할 변수
    public GameObject[] items_prefab;       // 드랍될 아이템 종류 프리팹들 저장할 변수
    public GameObject[] blankets;           // 이불 오브젝트들 저장할 변수
    public MeshRenderer[] blankets_Fade;    // 각 단계에 따른 이불들의 페이드인/아웃 효과 적용을 위한 변수 
    public GameObject nyangPoint;           // 통조림 사용 지점(냥냥이 소환)
    public BoxCollider area;                // 아이템포인트가 리셋 될 스테이지 내 범위 지정
    
    private int blankets_num;               // 이불 오브젝트들을 순차적으로 페이드인/아웃 시키기 위한 인덱스용 변수

    [Header("Object Pooling")]
    public List<GameObject> itemObjs_1;     // 풀링 할 아이템오브젝트 리스트 1
    public List<GameObject> itemObjs_2;     // 풀링 할 아이템오브젝트 리스트 2

    private ItemActiveState itemActiveState;

    // 서치포인트 및 아이템의 리스폰을 제어하기 위한 상태 저장
    public enum ItemActiveState
    {
        None,
        Active1,
        Active2
    }

    void Start()
    {
        blankets_num = 0;
        Init(); // 오브젝트 풀링 준비
    }

    // 통조림(Can)아이템을 해당 콜라이더 위치에서 사용 시 고양이 소환하고
    // 3가지 메서드 실행(서치포인트 초기화, 아이템 드랍, 이불애니메이션 작동)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BodyTrigger")  // 추후 Cat으로 변경
        {
            locomotion.DoRigidBodyJump();            // 고양이 앞발 침대 타격 시 플레이어 바운스 효과
            ChangeItemActiveState();                 // 풀링될 오브젝트 리스트 2개의 상태를 계속 변경
            SearchPointRespawn();                    // 새로운 아이템탐색 포인트들(파티클)이 활성화 됨
            AnimBlanket();                           // Blankets 이벤트 메서드 
        }
    }

    // 서치포인트 위치값을 저장하는 드랍아이템들의 오브젝트 풀링을 위한 초기설정
    void Init()
    {
        itemObjs_1 = new List<GameObject>();
        itemObjs_2 = new List<GameObject>();

        for (int i = 0; i < items_prefab.Length; i++)
        {
            GameObject i1 = Instantiate(items_prefab[i], searchPoints[i].transform.position, Quaternion.identity);
            itemObjs_1.Add(i1);

            GameObject i2 = Instantiate(items_prefab[i], searchPoints[i].transform.position, Quaternion.identity);
            itemObjs_2.Add(i2);
        }

        itemActiveState = ItemActiveState.None;
        SearchPointRespawn();
    }

    // 매회 냥냥펀치마다 재생성될 아이템 서치포인트의 위치를 재설정하는 메서드
    private Vector3 ResetSearchPosition(int index)
    {
        Vector3 basePosition = area.transform.position;
        Vector3 range = area.size;
        print(range);

        float randonRange = 100f;

        float posX = basePosition.x + Random.Range(-range.x * randonRange, range.x * randonRange);
        float posY = basePosition.y;
        float posZ = basePosition.z + Random.Range(-range.z * randonRange, range.z * randonRange);

        Vector3 resetPosition = new Vector3(posX, posY, posZ);
        searchPoints[index].transform.position = resetPosition;

        return resetPosition;
    }

    // 풀링되는 아이템 리스트 2개의 작동 상태를 3개로 구분하고, 
    // Active1,2를 반복시킴으로서 각각 SetItemPosition과 SearchPointRespawn의 실행 내용을 결정
    void ChangeItemActiveState()
    {
        switch (itemActiveState)
        {
            case ItemActiveState.None:
                itemActiveState = ItemActiveState.Active1;
                break;
            case ItemActiveState.Active1:
                itemActiveState = ItemActiveState.Active2;
                break;
            case ItemActiveState.Active2:
                itemActiveState = ItemActiveState.Active1;
                break;
        }
    }

    // 아이템이 드랍되어야 할 포지션 값을 지정
    // -> 이전 서치포인트가 있던 지점을 기억하여 해당 위치에 아이템을 생성함과 동시에
    //    다음 서치포인트의 값을 저장하여 다음 생성될 아이템의 위치값을 미리 지정
    private void SetItemPosition ()
    {
        switch (itemActiveState)
        {
            case ItemActiveState.None:
                for (int i = 0; i < itemObjs_1.Count; i++)
                {
                    itemObjs_1[i].transform.position = searchPoints[i].transform.position;
                }
                break;
            case ItemActiveState.Active1:
                for (int i = 0; i < itemObjs_1.Count; i++)
                {
                    itemObjs_2[i].transform.position = searchPoints[i].transform.position;
                    itemObjs_1[i].SetActive(true);
                }
                break;
            case ItemActiveState.Active2:
                for (int i = 0; i < itemObjs_2.Count; i++)
                {
                    itemObjs_1[i].transform.position = searchPoints[i].transform.position;
                    itemObjs_2[i].SetActive(true);
                }
                break;
        }
    }

    // 아이템의 서치포인트를 재생성(하고, 기존 서치포인트에 아이템을 생성
    void SearchPointRespawn()
    {
        switch (itemActiveState)
        {
            case ItemActiveState.None:
                foreach (GameObject o in itemObjs_1) o.SetActive(false);
                foreach (GameObject o in itemObjs_2) o.SetActive(false);
                for (int i = 0; i < searchPoints.Length; i++)
                {
                    ResetSearchPosition(i);
                    SetItemPosition();
                }
                break;
            case ItemActiveState.Active1:
                foreach (GameObject o in itemObjs_2) o.SetActive(false);
                for (int i = 0; i < itemObjs_1.Count; i++)
                {
                    ResetSearchPosition(i);
                    SetItemPosition();
                }
                break;
            case ItemActiveState.Active2:
                foreach (GameObject o in itemObjs_1) o.SetActive(false);
                for (int i = 0; i < itemObjs_2.Count; i++)
                {
                    ResetSearchPosition(i);
                    SetItemPosition();
                }
                break;
        }
    }
    
    // 이불 애니메이션(페이드인/아웃)을 순차적으로 실행시킴
    private void AnimBlanket ()
    {
        StartCoroutine(FadeIn(blankets_num));
        blankets_num++;
        if (blankets_num >= blankets_Fade.Length) blankets_num = 0;
        StartCoroutine(FadeOut(blankets_num));
    }

    // blancket의 alpha가 1->0     기존 이불오브젝트가 서서히 사라짐
    private IEnumerator FadeIn(int blankets_num)
    {
        int index = blankets_num;
        Color c = blankets_Fade[index].material.color;

        yield return new WaitForEndOfFrame();

        while (c.a > 0f)
        {
            c.a -= 0.1f;
            blankets_Fade[index].material.color = c;

            yield return new WaitForSeconds(0.05f);
        }
        blankets[blankets_num].SetActive(false);
    }

    // blanket의 alpha가 0->1     다음 이불오브젝트가 서서히 나타남
    private IEnumerator FadeOut (int blankets_num)
    {
        int index = blankets_num;
        Color c = blankets_Fade[index].material.color;
        yield return new WaitForEndOfFrame();

        while (c.a < 1.0f)
        {
            blankets[blankets_num].SetActive(true);
            c.a += 0.1f;
            blankets_Fade[index].material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
