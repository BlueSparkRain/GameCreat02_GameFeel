using System.Collections;
using Unity.Content;
using UnityEngine;

/// <summary>
/// ���ռ������⣩����
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    private BoxCollider2D checkAera;
    public E_Collectable type;
    [Header("��Ҫ�����Ĵ���")]
    public int moveTime=1;

    bool canTrigger=true;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }
    bool canMinus=true;

    /// <summary>
    /// ���ռ�������Χ������⣬�ɽ��վ���3��2.9��Ϊ�ٽ����ֵ
    /// </summary>
    /// <param name="square"></param>
    void CheckSelf(Transform square) 
    {
        if (canTrigger && canMinus && Vector2.Distance(square.position, transform.position) <= 3)
        {
             StartCoroutine(MinusCheck());
            if (moveTime <= 0)
            {
                canTrigger = false;
                StartCoroutine(BeRemoved());
            }
        }
    }

    IEnumerator MinusCheck() 
    {
       canMinus = false; 
       moveTime -= 1;
       yield return new WaitForSeconds(1);
       canMinus = true;
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        FindAnyObjectByType<CollectablesRecorder>().GetCollectable(type);//��¼�ռ���
        if (transform.parent && transform.parent.parent.GetComponent<SquareColumn>())
        {
            transform?.parent.parent.GetComponent<SquareColumn>().AddMaxSpawnNum();//�ָ����������
            transform?.parent.GetComponent<Slot>().ThrowSquare();
            //������Ч
            //��Ч
            yield return new WaitForSeconds(0.2f);
            DestroyImmediate(gameObject);
        }
    }
}
