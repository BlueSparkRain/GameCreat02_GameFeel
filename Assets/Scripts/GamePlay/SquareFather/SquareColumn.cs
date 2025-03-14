using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Search.Providers;
using UnityEngine;

public class SquareColumn : MonoBehaviour
{
    [Header("������ֳ��")]
    public Transform squareSpawner;
    [Header("���ϲ㷽��λ��")]
    public int SquareIndex {  get; private set; }
    [Header("���з���")]
    public int SquareNum;
    [Header("��ҳ�����Ϣ")]
    public PlayerBornData playerBornData;

    public GameObject prefab;

    void Start()
    {
        SquareIndex = 7;
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(SquareIndex).gameObject.SetActive(true);

        squareSpawner = transform.GetChild(8);
    }

     IEnumerator SpawnRestColumn() 
    {
        yield return  new WaitForSeconds(0.1f);
        Debug.Log("��Ҫ����"+(8 - SquareNum)+"������");
        for (int i = 0; i <= 8-SquareNum; i++) 
        {
            SpawneNewSquare();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator SpawnFirstColumn() 
    {
        for (int i = 0; i < 8; i++) 
        {
            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -70);
            }
            else
            {
                SpawneNewSquare();
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    /// <summary>
    /// ��һ���۱������������Ϸ���һ����
    /// </summary>
    public void ActiveNewSlot()
    {
        if (SquareIndex - 1 < 0)
            return;
        SquareIndex--;
        transform.GetChild(SquareIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// ��������һ���·���
    /// </summary>
    void SpawneNewSquare()
    {
       if (SquareNum + 1 <= 8)
       {
           GameObject newSquare = Instantiate(prefab, squareSpawner.position, Quaternion.identity, null);
           newSquare.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -70);
           SquareNum++;
       }
    }

    IEnumerator LooseRestSquare(int looseIndex) 
    {
        for (int i = looseIndex; i >=0; i--)
        {
            transform.GetChild(i).GetComponent<Slot>().LooseSelf();
            yield return  new WaitForSeconds(0.1f);
        }    
    }
    public void SquareThrow(int looseIndex) 
    {
        SquareNum--;
        if (SquareNum < 8)
        {
            //�������з����ɶ�̬
            StartCoroutine(SpawnRestColumn());
            StartCoroutine(LooseRestSquare(looseIndex));
        }
    }
}

[Serializable]
public class PlayerBornData 
{
    [Header("��Ҵ��г���?")]
    public bool IsPlayerBornColumn;
    [Header("��ҳ���λ�á�Խ�����·�ֵԽ��")]
    public int BornIndex;
}
