using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RemoveTest : MonoBehaviour
{
    public LayerMask targetLayer; // Ŀ������Ĳ�����
    public GameObject selectedObj; // ����Ч�� prefab
    public float ���߳��� = 100f; // ���ߵ���������

    float timer;
    public float timerInterval = 0.25f;
    bool canAct;

    private void Start()
    {
        timer=timerInterval;
    }

    void Update()
    {
        //if(timer>=0)
        //    timer-=Time.deltaTime;
        //else
        // canAct = true;

        //// ������������
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // ��ȡ���������Ļλ��
        //    Vector2 mousePosition = Input.mousePosition;

        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //    // ��������
        //    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, 100, targetLayer);

        //    // �����⵽����
        //    if (canAct&& hit.collider != null)
        //    {
        //        // ��ȡ�����е�����
        //        selectedObj = hit.collider.gameObject;
        //        StartCoroutine(selectedObj.GetComponent<Square>().BeRemoved());
        //        canAct = false;
        //        timer = timerInterval;
        //    }
        //    else
        //        Debug.Log("��Ŀ��");
        //}
    }

}
