using System.Collections;
using UnityEngine;

/// <summary>
/// ���ռ������⣩����
/// </summary>
public class CollectableSquare : SpecicalSquare
{
    private BoxCollider2D checkAera;
    public E_Collectable type;

    bool canTrigger=true;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckSelf);
    }

    /// <summary>
    /// ���ռ�������Χ������⣬�ɽ��վ���3��2.9��Ϊ�ٽ����ֵ
    /// </summary>
    /// <param name="square"></param>
    void CheckSelf(Transform square) 
    {
        Debug.Log(Vector2.Distance(square.position, transform.position));
        if (canTrigger && Vector2.Distance(square.position, transform.position) <= 3)
        {
            canTrigger = false;
            StartCoroutine(BeRemoved());
        }
    }


    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        FindAnyObjectByType<CollectablesRecorder>().GetCollectable(type);//��¼�ռ���
        if (transform.parent && transform.parent.parent.GetComponent<SquareColumn>())
        {
            transform?.parent.parent.GetComponent<SquareColumn>().AddMaxSpawnNum();//�ָ����������
            yield return transform?.parent.GetComponent<Slot>().ThrowSquare();
            //������Ч
            //��Ч
            yield return new WaitForSeconds(0.2f);
            DestroyImmediate(gameObject);
        }
    }


}
