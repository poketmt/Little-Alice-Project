using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager ui_instance;

    public static UIManager Instance
    {
        get
        {
            if (ui_instance == null)
            {
                ui_instance = FindObjectOfType<UIManager>();
            }
            return ui_instance;
        }
    }

    public Action<float> endAction;     // 타 클래스에서 UIManager의 특정 메서드에게 다음 행동을 지정해주기 위한 Action 변수

    [Header("Main Text")]
    public Text mainText; // 메인 Text 창(휘발성)
    [Header("MainText Box")]
    public GameObject mainTextBox;
    [Header("Mission Text")]
    public Text missonText; // 미션 Text 창
    //[Header("Conversation Text")]
    //public Text conversationText; // 대화 Text 창(휘발성)
    //public Text conversationText1; // 대화2 Text 창(휘발성)
    //public Text conversationText2; // 대화3 Text 창(휘발성)
    [Header("Slingshot Text")]
    public Text ammoText; // 새총 탄알 Text 창(새총 Active시 활성화)
    [Header("Charcter Text")]
    public Text[] character;
    [Header("CharacterText Box")]
    public GameObject[] characterTextBox;
    [HideInInspector]
    public float comparison_time; // 현재 실행되는 시간을 저장할 변수(한 가지의 대화 text 필드만 실행되고 있을 경우)
    [HideInInspector]
    public float save_time; // 추가로 실행되는 시간을 저장할 변수(실행 중에 대화 text 필드가 바뀌어 실해되어야 할 경우)

    private float delayTime;        // endAction에 넘겨받는 EndStory의 지연실행 매개변수를 넣어 주기 위한 변수

    private int character_index;    // 캐릭터 대화 시 지정된 캐릭터 Index를 식별하기 위한 변수

    private void Start()
    {
        if (mainText != null)
        {
            mainText.text = string.Empty;
        }

        if (missonText != null)
        {
            missonText.text = string.Empty;
        }
        
        if (character[character.Length-1] != null)
        {
            foreach(Text t in character)
                t.text = string.Empty;
        }
    }

    // 메인 텍스트 팝업 메소드(Stage 변경시 호출하여 사용)
    public void UpdateMainText(string text)
    {
        mainTextBox.SetActive(true);
        mainText.text = text;
        StartCoroutine(DistoryText(mainText, 3.0f));
    }
    // 미션 텍스트 팝업 메소드(Mission 시작시 호출 및 종료 후 재호출)
    public void UpdateMissonText(string text, bool isstate)
    {
        if (isstate == true)
        {
            missonText.text = text;
            UpdateMainText(text); // 메인 Text창에도 띄워주기
        }
        else
        {
            missonText.text = string.Empty;
        }
    }

    // 대화창 순차적 실행(바꿀 text내용, 실행시킬 시간)
    // * 텍스트 실행시 Alive_time이 일정하게 들어오지 않는 경우 수정 필요.(B 단계) ex) 10초 text 입력 -> 2초 뒤 3초 text 입력
    // ==> 아래에 있는 text가 위에 text보다 먼저 삭제되는 문제점 발생.
    /*public void UpdateConversationText(string text, float Alive_time)
    {
        // 첫번째 Text 창이 비어 있을 경우
        if (conversationText.text == null)
        {
            conversationText.text = text; // 받아온 Text 출력
            comparison_time = Time.time + Alive_time; // 첫 번째 Text 소멸시간 저장
            StartCoroutine(DistoryText(conversationText, Alive_time)); // 소멸시간 시작
        }
        else
        {
            // 첫 번째 Text창이 비어 있지 않고 두 번째 Text창이 비어 있을 경우
            if (conversationText1.text == null)
            {
                conversationText1.text = conversationText.text; // 첫 번째 Text를 두 번째 Text로 이동
                save_time = comparison_time - Time.time; // 두 번째로 이동한 Text가 더 재생되어야 할 시간 저장
                StartCoroutine(DistoryText(conversationText1, save_time)); // 남은 시간만큼 추가 재생하고 소멸
                conversationText.text = text; // 새로 받아온 Text 출력
                comparison_time = Time.time + Alive_time; // 새로 받아온 Text 소멸시간 저장
                StartCoroutine(DistoryText(conversationText, Alive_time)); // 소멸시간 시작
            }

            // 세번째 창만 비어 있을 경우
            else if (conversationText.text != null && conversationText1.text != null && conversationText2.text == null)
            {
                conversationText2.text = conversationText1.text; // 두 번째 Text를 세 번째 Text로 이동 
                save_time = save_time - Time.time; // 세 번째로 이동한 Text가 더 재생되어야 할 시간 저장
                StartCoroutine(DistoryText(conversationText2, save_time)); // 남은 시간만큼 추가 재생하고 소멸
                conversationText1.text = conversationText.text; // 첫 번째 Text를 두 번째 Text로 이동
                comparison_time = comparison_time - Time.time; // 두 번째로 이동한 Text가 더 재생되어야 할 시간 저장
                StartCoroutine(DistoryText(conversationText1, comparison_time)); // 남은 시간만큼 추가 재생하고 소멸
                conversationText.text = text; // 새로 받아온 Text 출력
                comparison_time = Time.time + Alive_time; // 새로 받아온 Text 소멸시간 저장
                StartCoroutine(DistoryText(conversationText, Alive_time)); // 소명시간 시작
            }
        }
    }*/


    /*
    public void UpdateConversationText(string text)
    {
        // 첫번째 Text 창이 비어 있을 경우
        if (conversationText.text == null)
        {
            conversationText.text = text; // 받아온 Text 출력
        }
        // 두번째 Text 창이 비어 있을 경우
        else if (conversationText1.text == null)
        {
            conversationText1.text = conversationText.text; // 첫 번째 Text를 두 번째 Text로 이동
            conversationText.text = text; // 새로 받아온 Text 출력
        }
        // 세번째 Text창이 비어 있을 경우 + 모든 창이 가득 차 있을 경우
        else
        {
            conversationText2.text = conversationText1.text; // 두 번째 Text를 세 번째 Text로 이동 
            conversationText1.text = conversationText.text; // 첫 번째 Text를 두 번째 Text로 이동
            conversationText.text = text; // 새로 받아온 Text 출력
        }
    }
    */

    public void UpdateCharacterText(string text, float live_time, int character_index, float delayTime) // update할 text, 살려둘 시간, 캐릭터 순번
    {
        this.delayTime = delayTime;
        this.character_index = character_index;
        if(string.IsNullOrEmpty(character[character_index].text)) // 해당 text 박스가 비어있을 경우 대화 Update
        {
            characterTextBox[character_index].SetActive(true);
            character[character_index].text = text;
            Invoke("EmptyCharacterText", live_time);
        }
        //else // 해당 프레임이 종료될 때 까지 기다렸다가 대화 Update 재요청
        //{
        //    new WaitForEndOfFrame();
        //    UpdateCharacterText(text, live_time, charcter_index);
        //}
    }

    // Invoke를 통해 해당 캐릭터의 대사창이 리셋될 시간 간격을 결정
    public void EmptyCharacterText()    
    {
        character[character_index].text = string.Empty;
        characterTextBox[character_index].SetActive(false);
        if (endAction != null) endAction(delayTime);
    }

    /*
    IEnumerator CharacterText(float live_time, int character_index) // 대화창이 살이 있을 시간
    {
        emptyText = true;
        yield return new WaitForSeconds(live_time); // 살리려한 시간만큼 놔두기
        if (emptyText == true)
        {
            character[character_index].text = string.Empty; // text창 초기화
            if (character[character_index].text == null)
                endAction(1f);
        }
    }
    */

    public void UpdateAmmoText(int ammo, int remain)
    {
        string ammo_print;
        ammo_print = ammo + " / " + remain;
        ammoText.text = ammo_print;
    }
    // 텍스트 초기화 메소드(변경 Text 및 시간 호출하여 사용)
    IEnumerator DistoryText(Text text, float time)
    {
        yield return new WaitForSeconds(time);
        text.text = string.Empty;
        mainTextBox.SetActive(false);
    }
    // ? 이전 재생중 WaitForSeconds를 멈추기 위해서는 어떻게 소스를 바꿀지...
}