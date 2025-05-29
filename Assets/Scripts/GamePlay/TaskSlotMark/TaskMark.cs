using System.Collections;
using UnityEngine;

public class TaskMark : MonoBehaviour
{
    EventCenter evcenter;
    [Header("�������������")]
    public int taskIndex;

    bool canTrigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTrigger && other.GetComponent<PlayerController>())
        {
            Debug.Log("��Ұ�����");
            canTrigger = false;
            evcenter.EventTrigger(E_EventType.E_TaskTrigger, taskIndex);
            StartCoroutine(SquareRemoveAnim());
        }
    }
    public IEnumerator TaskAppear() 
    {
        yield return new WaitForSeconds(6);
        //transform.position = pos;
        yield return TweenHelper.MakeLerp(Vector3.zero,Vector3.one*0.5f,0.5f,val=> transform.localScale=val);
        canTrigger = true;
    }

    private void Awake()
    {
        transform.localScale=Vector3.zero;
    }
    void Start()
    {
        evcenter = EventCenter.Instance;

        StartCoroutine(TaskAppear());
    }

    /// <summary>
    /// ɫ���ڱ�����ʱ�Ķ���
    /// </summary>
    /// <returns></returns>
    public IEnumerator SquareRemoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.10f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f, 2.5f, 1.6f), 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.2f, 0.03f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.zero, 0.02f, val => transform.localScale = val);
    }
}
