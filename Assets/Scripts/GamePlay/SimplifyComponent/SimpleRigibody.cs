using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleRigibody : MonoBehaviour
{
    // �ٶȺͼ��ٶ�
    public Vector3 velocity;
    public Vector3 acceleration;
    public bool useGravity = true; 
    private Quaternion rotation;
  
    void Update()
    {
        if (useGravity)
        {
            HandleManualMovement();
        }
    }
    public void GetSlot() 
    {
        velocity = Vector3.zero;
        useGravity = false;
    }

    private void HandleManualMovement()
    {
        // ���������λ�á���ת��
        // ���磺�����û������������ʽ�������ٶȡ���ת��
        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        //transform.rotation = rotation;
    }

    /// <summary>
    /// �Զ����˶����ٶ�
    /// </summary>
    /// <param name="accelerationDir">��һ������</param>
    /// <param name="accelerationIntensity">���ٶȾ���ֵ��С</param>
    public void SetCustomAcceleration(Vector3 accelerationDir)
    {
        this.acceleration = accelerationDir * (9.8f);
    }

    /// <summary>
    /// ���ø����ٶ�
    /// </summary>
    public void SetZeroSpeed()
    {
        velocity = Vector3.zero;
    }


    /// <summary>
    /// ���ø����ٶ�
    /// </summary>
    /// <param name="newVelocity"></param>
    public void SetLooseVelocity(Vector3 newVelocity)
    {
        useGravity = true;
        velocity = newVelocity;
    }

    /// <summary>
    /// ������ת
    /// </summary>
    /// <param name="newRotation"></param>
    public void SetRotation(Quaternion newRotation)
    {
        rotation = newRotation;
    }
}
