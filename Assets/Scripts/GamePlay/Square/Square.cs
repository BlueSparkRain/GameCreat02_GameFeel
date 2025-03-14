using UnityEngine;

public class Square : MonoBehaviour
{
    public E_SquareColor color;
    Rigidbody2D rb;
    public bool HasFather { get; private set; }


    protected virtual  void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    public virtual void MoveToSlot(Vector3 slotPos)
    {
       rb.isKinematic=true;
       rb.velocity = Vector3.zero;
       StartCoroutine(TweenHelper.MakeLerp(transform.position, slotPos, 0.1f, val => transform.position = val));
       HasFather = true;
    }
    /// <summary>
    /// 松掉本方块
    /// </summary>
    public void LooseSelf() 
    {
       rb.bodyType = RigidbodyType2D.Dynamic;
        HasFather= false;
    }

    protected virtual void BeRemoved()
    {

    }
}


public enum E_SquareColor 
{
    红色,蓝色,黄色,紫色
}
