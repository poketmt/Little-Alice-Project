using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyItem : MonoBehaviour
{
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // (�������� ����Ʈ��Ʈ �� �� ������ �����ؾ� �ϴµ� �������� �ִϸ��̼� ������ �����Ƿ� 2���� ����) 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            gameObject.SetActive(false);
        }
        else if (other.gameObject.name == "Quest_Ant")
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            StartCoroutine(OnFrameAnimatorCheck(other));
        }
    }

    // �ִϸ������� �ִϸ��̼��� ����Ǿ��� �� ������ ��Ȱ��ȭ ��Ű�� ����
    IEnumerator OnFrameAnimatorCheck(Collider other)
    {
        Animator anim = other.gameObject.GetComponent<Animator>();
        yield return new WaitForSeconds(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("eat"))
        {
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
