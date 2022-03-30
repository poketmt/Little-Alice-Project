using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Bed_Event : MonoBehaviour
{
    public SmoothLocomotion locomotion;     // SmoothLocomotion ��� ȣ��� ��ü ����
    public GameObject[] searchPoints;       // ������ ����Ʈ���� Ȱ��ȭ�� ��ġ ������ ����
    public GameObject[] items_prefab;       // ����� ������ ���� �����յ� ������ ����
    public GameObject[] blankets;           // �̺� ������Ʈ�� ������ ����
    public MeshRenderer[] blankets_Fade;    // �� �ܰ迡 ���� �̺ҵ��� ���̵���/�ƿ� ȿ�� ������ ���� ���� 
    public GameObject nyangPoint;           // ������ ��� ����(�ɳ��� ��ȯ)
    public BoxCollider area;                // ����������Ʈ�� ���� �� �������� �� ���� ����
    
    private int blankets_num;               // �̺� ������Ʈ���� ���������� ���̵���/�ƿ� ��Ű�� ���� �ε����� ����

    [Header("Object Pooling")]
    public List<GameObject> itemObjs_1;     // Ǯ�� �� �����ۿ�����Ʈ ����Ʈ 1
    public List<GameObject> itemObjs_2;     // Ǯ�� �� �����ۿ�����Ʈ ����Ʈ 2

    private ItemActiveState itemActiveState;

    // ��ġ����Ʈ �� �������� �������� �����ϱ� ���� ���� ����
    public enum ItemActiveState
    {
        None,
        Active1,
        Active2
    }

    void Start()
    {
        blankets_num = 0;
        Init(); // ������Ʈ Ǯ�� �غ�
    }

    // ������(Can)�������� �ش� �ݶ��̴� ��ġ���� ��� �� ����� ��ȯ�ϰ�
    // 3���� �޼��� ����(��ġ����Ʈ �ʱ�ȭ, ������ ���, �̺Ҿִϸ��̼� �۵�)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BodyTrigger")  // ���� Cat���� ����
        {
            locomotion.DoRigidBodyJump();            // ����� �չ� ħ�� Ÿ�� �� �÷��̾� �ٿ ȿ��
            ChangeItemActiveState();                 // Ǯ���� ������Ʈ ����Ʈ 2���� ���¸� ��� ����
            SearchPointRespawn();                    // ���ο� ������Ž�� ����Ʈ��(��ƼŬ)�� Ȱ��ȭ ��
            AnimBlanket();                           // Blankets �̺�Ʈ �޼��� 
        }
    }

    // ��ġ����Ʈ ��ġ���� �����ϴ� ��������۵��� ������Ʈ Ǯ���� ���� �ʱ⼳��
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

    // ��ȸ �ɳ���ġ���� ������� ������ ��ġ����Ʈ�� ��ġ�� �缳���ϴ� �޼���
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

    // Ǯ���Ǵ� ������ ����Ʈ 2���� �۵� ���¸� 3���� �����ϰ�, 
    // Active1,2�� �ݺ���Ŵ���μ� ���� SetItemPosition�� SearchPointRespawn�� ���� ������ ����
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

    // �������� ����Ǿ�� �� ������ ���� ����
    // -> ���� ��ġ����Ʈ�� �ִ� ������ ����Ͽ� �ش� ��ġ�� �������� �����԰� ���ÿ�
    //    ���� ��ġ����Ʈ�� ���� �����Ͽ� ���� ������ �������� ��ġ���� �̸� ����
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

    // �������� ��ġ����Ʈ�� �����(�ϰ�, ���� ��ġ����Ʈ�� �������� ����
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
    
    // �̺� �ִϸ��̼�(���̵���/�ƿ�)�� ���������� �����Ŵ
    private void AnimBlanket ()
    {
        StartCoroutine(FadeIn(blankets_num));
        blankets_num++;
        if (blankets_num >= blankets_Fade.Length) blankets_num = 0;
        StartCoroutine(FadeOut(blankets_num));
    }

    // blancket�� alpha�� 1->0     ���� �̺ҿ�����Ʈ�� ������ �����
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

    // blanket�� alpha�� 0->1     ���� �̺ҿ�����Ʈ�� ������ ��Ÿ��
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
