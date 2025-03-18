using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RemoveTest : MonoBehaviour
{
    public LayerMask targetLayer; // 目标物体的层掩码
    public GameObject selectedObj; // 击中效果 prefab
    public float 射线长度 = 100f; // 射线的最大检测距离

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

        //// 检测鼠标左键点击
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // 获取鼠标点击的屏幕位置
        //    Vector2 mousePosition = Input.mousePosition;

        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //    // 发射射线
        //    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, 100, targetLayer);

        //    // 如果检测到物体
        //    if (canAct&& hit.collider != null)
        //    {
        //        // 获取被击中的物体
        //        selectedObj = hit.collider.gameObject;
        //        StartCoroutine(selectedObj.GetComponent<Square>().BeRemoved());
        //        canAct = false;
        //        timer = timerInterval;
        //    }
        //    else
        //        Debug.Log("无目标");
        //}
    }

}
