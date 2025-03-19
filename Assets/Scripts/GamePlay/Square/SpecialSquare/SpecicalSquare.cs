using System.Collections;
using UnityEngine;

/// <summary>
/// ���ⷽ��
/// </summary>
public class SpecicalSquare : Square, ICanSpecical
{
    [Header("����ؿ龫��")]
    public Sprite SpecicalSprite;

    [Header("���ؿ�ɴݻ�")]
    bool canRemoved;

    protected override void Awake()
    {
        base.Awake();
        MyAppear();
    }

    /// <summary>
    /// ����������������
    /// </summary>
    void MyAppear()
    {
        transform.GetComponent<SpriteRenderer>().sprite = SpecicalSprite;
    }

    public override void DoSelfEffect()
    {
     
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        if(!canRemoved)
            yield break;

    }

    public virtual void DoSelfSpecical()
    {
        Debug.Log("����ؿ����⹦��");
    }

   
}
