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

    // (개미집과 퀘스트앤트 둘 다 사탕에 반응해야 하는데 개미집은 애니메이션 동작이 없으므로 2개로 나눔) 
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

    // 애니메이터의 애니메이션이 종료되었을 때 아이템 비활성화 시키기 위함
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
