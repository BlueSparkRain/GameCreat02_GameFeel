using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Search.Providers;
using UnityEngine;

public class SquareColumn : MonoBehaviour
{
    [Header("方块增殖器")]
    public Transform squareSpawner;
    [Header("最上层方块位序")]
    public int SquareIndex {  get; private set; }
    [Header("本列方块")]
    public int SquareNum;
    [Header("玩家出生信息")]
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
        Debug.Log("需要生成"+(8 - SquareNum)+"个方块");
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
    /// 当一个槽被填满，解锁上方的一个槽
    /// </summary>
    public void ActiveNewSlot()
    {
        if (SquareIndex - 1 < 0)
            return;
        SquareIndex--;
        transform.GetChild(SquareIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// 本列生成一个新方块
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
            //先让所有方块变成动态
            StartCoroutine(SpawnRestColumn());
            StartCoroutine(LooseRestSquare(looseIndex));
        }
    }
}

[Serializable]
public class PlayerBornData 
{
    [Header("玩家此列出生?")]
    public bool IsPlayerBornColumn;
    [Header("玩家出生位置【越靠列下方值越大】")]
    public int BornIndex;
}
