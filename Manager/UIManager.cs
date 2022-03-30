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

    public Action<float> endAction;     // Ÿ Ŭ�������� UIManager�� Ư�� �޼��忡�� ���� �ൿ�� �������ֱ� ���� Action ����

    [Header("Main Text")]
    public Text mainText; // ���� Text â(�ֹ߼�)
    [Header("MainText Box")]
    public GameObject mainTextBox;
    [Header("Mission Text")]
    public Text missonText; // �̼� Text â
    //[Header("Conversation Text")]
    //public Text conversationText; // ��ȭ Text â(�ֹ߼�)
    //public Text conversationText1; // ��ȭ2 Text â(�ֹ߼�)
    //public Text conversationText2; // ��ȭ3 Text â(�ֹ߼�)
    [Header("Slingshot Text")]
    public Text ammoText; // ���� ź�� Text â(���� Active�� Ȱ��ȭ)
    [Header("Charcter Text")]
    public Text[] character;
    [Header("CharacterText Box")]
    public GameObject[] characterTextBox;
    [HideInInspector]
    public float comparison_time; // ���� ����Ǵ� �ð��� ������ ����(�� ������ ��ȭ text �ʵ常 ����ǰ� ���� ���)
    [HideInInspector]
    public float save_time; // �߰��� ����Ǵ� �ð��� ������ ����(���� �߿� ��ȭ text �ʵ尡 �ٲ�� ���صǾ�� �� ���)

    private float delayTime;        // endAction�� �Ѱܹ޴� EndStory�� �������� �Ű������� �־� �ֱ� ���� ����

    private int character_index;    // ĳ���� ��ȭ �� ������ ĳ���� Index�� �ĺ��ϱ� ���� ����

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

    // ���� �ؽ�Ʈ �˾� �޼ҵ�(Stage ����� ȣ���Ͽ� ���)
    public void UpdateMainText(string text)
    {
        mainTextBox.SetActive(true);
        mainText.text = text;
        StartCoroutine(DistoryText(mainText, 3.0f));
    }
    // �̼� �ؽ�Ʈ �˾� �޼ҵ�(Mission ���۽� ȣ�� �� ���� �� ��ȣ��)
    public void UpdateMissonText(string text, bool isstate)
    {
        if (isstate == true)
        {
            missonText.text = text;
            UpdateMainText(text); // ���� Textâ���� ����ֱ�
        }
        else
        {
            missonText.text = string.Empty;
        }
    }

    // ��ȭâ ������ ����(�ٲ� text����, �����ų �ð�)
    // * �ؽ�Ʈ ����� Alive_time�� �����ϰ� ������ �ʴ� ��� ���� �ʿ�.(B �ܰ�) ex) 10�� text �Է� -> 2�� �� 3�� text �Է�
    // ==> �Ʒ��� �ִ� text�� ���� text���� ���� �����Ǵ� ������ �߻�.
    /*public void UpdateConversationText(string text, float Alive_time)
    {
        // ù��° Text â�� ��� ���� ���
        if (conversationText.text == null)
        {
            conversationText.text = text; // �޾ƿ� Text ���
            comparison_time = Time.time + Alive_time; // ù ��° Text �Ҹ�ð� ����
            StartCoroutine(DistoryText(conversationText, Alive_time)); // �Ҹ�ð� ����
        }
        else
        {
            // ù ��° Textâ�� ��� ���� �ʰ� �� ��° Textâ�� ��� ���� ���
            if (conversationText1.text == null)
            {
                conversationText1.text = conversationText.text; // ù ��° Text�� �� ��° Text�� �̵�
                save_time = comparison_time - Time.time; // �� ��°�� �̵��� Text�� �� ����Ǿ�� �� �ð� ����
                StartCoroutine(DistoryText(conversationText1, save_time)); // ���� �ð���ŭ �߰� ����ϰ� �Ҹ�
                conversationText.text = text; // ���� �޾ƿ� Text ���
                comparison_time = Time.time + Alive_time; // ���� �޾ƿ� Text �Ҹ�ð� ����
                StartCoroutine(DistoryText(conversationText, Alive_time)); // �Ҹ�ð� ����
            }

            // ����° â�� ��� ���� ���
            else if (conversationText.text != null && conversationText1.text != null && conversationText2.text == null)
            {
                conversationText2.text = conversationText1.text; // �� ��° Text�� �� ��° Text�� �̵� 
                save_time = save_time - Time.time; // �� ��°�� �̵��� Text�� �� ����Ǿ�� �� �ð� ����
                StartCoroutine(DistoryText(conversationText2, save_time)); // ���� �ð���ŭ �߰� ����ϰ� �Ҹ�
                conversationText1.text = conversationText.text; // ù ��° Text�� �� ��° Text�� �̵�
                comparison_time = comparison_time - Time.time; // �� ��°�� �̵��� Text�� �� ����Ǿ�� �� �ð� ����
                StartCoroutine(DistoryText(conversationText1, comparison_time)); // ���� �ð���ŭ �߰� ����ϰ� �Ҹ�
                conversationText.text = text; // ���� �޾ƿ� Text ���
                comparison_time = Time.time + Alive_time; // ���� �޾ƿ� Text �Ҹ�ð� ����
                StartCoroutine(DistoryText(conversationText, Alive_time)); // �Ҹ�ð� ����
            }
        }
    }*/


    /*
    public void UpdateConversationText(string text)
    {
        // ù��° Text â�� ��� ���� ���
        if (conversationText.text == null)
        {
            conversationText.text = text; // �޾ƿ� Text ���
        }
        // �ι�° Text â�� ��� ���� ���
        else if (conversationText1.text == null)
        {
            conversationText1.text = conversationText.text; // ù ��° Text�� �� ��° Text�� �̵�
            conversationText.text = text; // ���� �޾ƿ� Text ���
        }
        // ����° Textâ�� ��� ���� ��� + ��� â�� ���� �� ���� ���
        else
        {
            conversationText2.text = conversationText1.text; // �� ��° Text�� �� ��° Text�� �̵� 
            conversationText1.text = conversationText.text; // ù ��° Text�� �� ��° Text�� �̵�
            conversationText.text = text; // ���� �޾ƿ� Text ���
        }
    }
    */

    public void UpdateCharacterText(string text, float live_time, int character_index, float delayTime) // update�� text, ����� �ð�, ĳ���� ����
    {
        this.delayTime = delayTime;
        this.character_index = character_index;
        if(string.IsNullOrEmpty(character[character_index].text)) // �ش� text �ڽ��� ������� ��� ��ȭ Update
        {
            characterTextBox[character_index].SetActive(true);
            character[character_index].text = text;
            Invoke("EmptyCharacterText", live_time);
        }
        //else // �ش� �������� ����� �� ���� ��ٷȴٰ� ��ȭ Update ���û
        //{
        //    new WaitForEndOfFrame();
        //    UpdateCharacterText(text, live_time, charcter_index);
        //}
    }

    // Invoke�� ���� �ش� ĳ������ ���â�� ���µ� �ð� ������ ����
    public void EmptyCharacterText()    
    {
        character[character_index].text = string.Empty;
        characterTextBox[character_index].SetActive(false);
        if (endAction != null) endAction(delayTime);
    }

    /*
    IEnumerator CharacterText(float live_time, int character_index) // ��ȭâ�� ���� ���� �ð�
    {
        emptyText = true;
        yield return new WaitForSeconds(live_time); // �츮���� �ð���ŭ ���α�
        if (emptyText == true)
        {
            character[character_index].text = string.Empty; // textâ �ʱ�ȭ
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
    // �ؽ�Ʈ �ʱ�ȭ �޼ҵ�(���� Text �� �ð� ȣ���Ͽ� ���)
    IEnumerator DistoryText(Text text, float time)
    {
        yield return new WaitForSeconds(time);
        text.text = string.Empty;
        mainTextBox.SetActive(false);
    }
    // ? ���� ����� WaitForSeconds�� ���߱� ���ؼ��� ��� �ҽ��� �ٲ���...
}