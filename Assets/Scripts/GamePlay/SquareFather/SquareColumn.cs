using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public class SquareColumn : MonoBehaviour
{
    [Header("方块增殖器")]
    public Transform squareSpawner;
    [Header("最上层方块位序")]
    public int SquareIndex { get; private set; }
    [Header("本列方块")]
    public int SquareNum;
    [Header("玩家出生信息")]
    public PlayerBornData playerBornData;

    public List<Square> columnSquares = new List<Square>();
    public List<Slot> columnSlots = new List<Slot>();

    [Header("本列方块的移动方向")]
    public E_CustomDir  MoveDir;

    Vector3 gravityDir;
    Vector3 looseSpeed;

    SquareObjPool  squarePool;

    /// <summary>
    /// 默认松动速度
    /// </summary>
    [Header("默认松动速度")]
    public float squareLooseSpeed = 50;

    void GetLoosepeed()
    {
        switch (MoveDir)
        {
            case E_CustomDir.上:
                gravityDir = Vector3.up;
                break;
            case E_CustomDir.下:
                gravityDir = Vector3.down;
                break;
            case E_CustomDir.左:
                gravityDir = Vector3.left;
                break;
            case E_CustomDir.右:
                gravityDir = Vector3.right;
                break;
            default:
                 break;
        }
        looseSpeed = gravityDir * squareLooseSpeed;
    }


    /// <summary>
    /// 本列中容纳方块的最上层槽的序列索引
    /// </summary>
    public int FirstEmptySlotIndex;

    public bool ColFull;

    [Header("本列正在消除中")]
    public bool isRemoving;


    WaitForSeconds removeDelay = new WaitForSeconds(0.15f);


    [Header("本列色块补充最大容量")]
    public int maxSpawnNum = 8;

    /// <summary>
    /// 当场景中有提前放置在列上的色块，会影响本列的可生成色块最大容量，当可消除物被消除，恢复生成容量
    /// </summary>
    public void AddMaxSpawnNum()
    {
        maxSpawnNum++;
    }

    private void Awake()
    {
        GetLoosepeed();
        squareSpawner = transform.GetChild(8);

        squarePool=FindAnyObjectByType<SquareObjPool>();

        //预缓存槽对象，性能优化
        for (int i = 0; i < 8; i++)
        {
          columnSlots.Add(transform.GetChild(i).GetComponent<Slot>());
        }

        for (int i = 0; i <columnSlots.Count ; i++)
        {
            columnSlots[i].isDownEmpty = true;
        }
        columnSlots[7].isDownEmpty=false;
   
    }

    void Start()
    {
        for (int i = columnSlots.Count; i >= 0; i--)
        {
            if (transform.GetChild(i).childCount != 0)
            {
                maxSpawnNum--;
            }
        }
    }

    /// <summary>
    /// 随机（删组合）充满本列
    /// </summary>
    /// <param name="soList"></param>
    /// <returns></returns>
    public IEnumerator SpawneFirstColSquares(List<ColorSquareSO> soList) 
    {
        WaitForSeconds delay=  new WaitForSeconds(0.2f);
        for (int i = 0; i < maxSpawnNum; i++)
        {
            //玩家产生
            if (playerBornData.IsPlayerBornColumn && i == playerBornData.BornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), squareSpawner.position, Quaternion.identity, null);
                 StartCoroutine(ColFallOneSquare(player));
                //ColFallOneSquare(player);

                //player.GetComponent<Square>().LooseSelf();
                yield return delay;
                continue;
            }

            GameObject newSquare = squarePool.GetTargetSquare(soList[i]);
            StartCoroutine(ColFallOneSquare(newSquare));
            //ColFallOneSquare(newSquare);
            yield return delay;
        }

    }

    /// <summary>
    /// 本列开启消除任务
    /// </summary>
    public void IsColumnRemoving()
    {
        isRemoving = true;
    }

    /// <summary>
    /// 本列完成消除任务
    /// </summary>
    public void StopColumnRemoving()
    {
        isRemoving = false;
    }


    int currentSquareNum;
    /// <summary>
    /// 更新本列方块
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index"></param>
    public void UpdateColumnSquares(Square square, int index)
    {
        columnSquares[index] = square;

        for (int i = 0; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i])
            {
                //ColFull = false;
                break;
            }
            ColFull = true;
        }

        if (!ColFull)
            return;
        currentSquareNum = 0;
        for (int i = 0; i < columnSquares.Count; i++)
        {
            if (columnSquares[i])
            {
                currentSquareNum++;
            }
        }
        SquareNum = currentSquareNum;
    }

    bool canCheck = true;
    float timer;

    float removeTimer;
    float removeTimerInterval = 0.4f;//消除行为在0.5s内未重复触发时
    WaitForSeconds bornDelay = new WaitForSeconds(0.2f);
    bool canAddNeedSquare=true;

    /// <summary>
    /// 向列发送填充请求：重置消除计时器
    /// </summary>
    public void ColPrepareNeededSquare()
    {
            newRemoveBegin = true;
            needAddNum++;
            Debug.Log("消除事件重置！");
            removeTimer = removeTimerInterval;
            //Test
            canAddNeedSquare = true;
    }

    bool newRemoveOver;//一连串消除事件结束
    int needAddNum;//需要添加的方块

    /// <summary>
    /// 待最上方方块入槽后
    /// </summary>
    /// <returns></returns>
    IEnumerator BornNeedToAddSquare()
    {
        Debug.Log("开始生成需要方块："+needAddNum);
        for (int i = 0; i <= needAddNum; i++)
        {
            if(SquareNum>=8)
               yield break;  
            
            needAddNum--;
            ColumnAddOneRandomSquare();
            yield return bornDelay;
        }
        canAddNeedSquare = true;
        //每次补充完毕后
    }

    /// <summary>
    /// 松掉一个槽时，继续推迟补充时刻
    /// </summary>
    public void LooseASlot() 
    {
        Debug.Log("松槽推迟");
        //newRemoveBegin = true;
        removeTimer = removeTimerInterval;
    }

    void ColAddNeededSquare()
    {
        StartCoroutine(BornNeedToAddSquare());
    }
    bool newRemoveBegin;
    private void Update()
    {
        if (ColFull)
        {
            RemoveSquares();

            //消除事件开始 && 每次触发消除，重置计时
            if (newRemoveBegin && removeTimer >= 0)
                removeTimer -= Time.deltaTime;
            else
            {
                Debug.Log("消除事件结束");
                newRemoveOver = true;
                newRemoveBegin = false;
            }

            if (newRemoveOver && canAddNeedSquare)
            {
                newRemoveOver=false;
                if (!newRemoveBegin && canAddNeedSquare)
                {
                    canAddNeedSquare = false;
                    //列生成只触发一次，等待下次消除
                    ColAddNeededSquare();
                }

            }

        }
    }


    Coroutine emptyCheck;
    IEnumerator AddWholeSquares() 
    {        
        WaitForSeconds delay=new WaitForSeconds(2f);
        yield return delay;
        for (int i = 0; i < 8 - SquareNum; i++)
        {
           ColumnAddOneRandomSquare();
           yield return new WaitForSeconds(0.8f);;
        }
        canCheck = true;
    }
    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
        {
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
        }
    }
    public List<Square> CheckRemoveList()
    {
        int num = 1;
        bool canAdd = true;

        List<Square> toRemoveSquares = new List<Square>();
        if (!columnSquares[0] || !columnSquares[0].GetComponent<ColorSquare>() ||!columnSquares[0].GetComponent<ColorSquare>().myData)
            return null;
        E_Color firstCor = columnSquares[0].GetComponent<ColorSquare>().myData.E_Color;
        toRemoveSquares.Add(columnSquares[0]);

        for (int i = 1; i < columnSquares.Count; i++)
        {
            if (!columnSquares[i] || !columnSquares[i].GetComponent<ColorSquare>())
            {
                continue;
            }
            if (!columnSquares[i].GetComponent<ColorSquare>().myData || !columnSquares[i])
                return null;

            if (columnSquares[i] && columnSquares[i].GetComponent<ColorSquare>() && columnSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor && canAdd)
            {
                if (!toRemoveSquares.Contains(columnSquares[i]))
                {
                    num++;
                    toRemoveSquares.Add(columnSquares[i]);
                }
            }
            else
            {
                if (num < 3)
                {
                    firstCor = columnSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                    toRemoveSquares.Clear();
                    num = 1;
                    canAdd = true;
                    toRemoveSquares.Add(columnSquares[i]);
                }
                else
                {
                    canAdd = false;
                    toRemoveSquares.Remove(columnSquares[i]);
                }
            }
        }
        if (toRemoveSquares.Count >= 3)
            return toRemoveSquares;
        else
            return null;
    }

    IEnumerator CheckAndRemoveSquares(List<Square> removeLists)
    {
        if (!GetComponent<SquareRow>().isRemoving && !isRemoving)
        {
            IsColumnRemoving();

            if (removeLists.Count <= 2)
            {
            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveLine(removeLists,RemoveColLine5); 
            }
            else if (removeLists.Count >= 4)
            {
                yield return RemoveLine(removeLists, RemoveColLine4);
            }

            else if (removeLists.Count >= 3)
            {
                yield return RemoveLine(removeLists, RemoveColLine3);
            }
            StopColumnRemoving();
        }
    }

    /// <summary>
    /// 连线3消：无功能，只积分
    /// </summary>
    public void RemoveColLine3()
    {
        Debug.Log("完成3消");
    }

    /// <summary>
    /// 连线4消：消除+生成1个整列炸弹色块
    /// </summary>
    public void RemoveColLine4()
    {
        Debug.Log("完成4消");
    }

    IEnumerator RemoveLine(List<Square> toRemoveSquares,UnityAction callback=null)
    {
        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                yield return toRemoveSquares[i].BeRemoved();
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                i++;
            }
            StartCoroutine(toRemoveSquares[i].BeRemoved());

            ColPrepareNeededSquare();

            yield return removeDelay;
        }
        callback?.Invoke();
    }

  
    /// <summary>
    /// 连线5消：消除+生成色块闪电：清除所有相同颜色色块
    /// </summary>
    public void RemoveColLine5()
    {
        Debug.Log("完成5消");
    }

    /// <summary>
    /// 清除整列Square
    /// </summary>
    /// <returns></returns>
    public IEnumerator RemoveWholeColumn()
    {
        int removeIndex = columnSquares.Count - 1;

        for (int i = columnSquares.Count - 1; i >= 0; i--)
        {
            if (columnSquares[columnSquares.Count - 1].GetComponent<PlayerController>())
                removeIndex = columnSquares.Count - 2;

            yield return columnSquares[removeIndex].BeRemoved();
            ColumnAddOneRandomSquare();
            yield return new WaitForSeconds(0.02f);
        }
    }


    /// <summary>
    /// 本列生成一个新方块
    /// </summary>
    public void AppearNewSquare(GameObject square)
    {
        square.transform.position = squareSpawner.position;
        square.GetComponent<Square>().LooseSelf();
    }

    /// <summary>
    /// 本列新增加一个随机颜色色块，初始化并落下
    /// </summary>
    public void ColumnAddOneRandomSquare()
    {
        GameObject newSquare = squarePool.GetRandomSquare();
        StartCoroutine(ColFallOneSquare(newSquare));
    }

    //本列最新方块控制器
    SquareController  newSquareController;

    /// <summary>
    /// 让一个方块初始化并下坠
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    IEnumerator ColFallOneSquare(GameObject square)
    {
        //yield return new WaitForSeconds(0.1f);
        yield return null;
        newSquareController = square.GetComponent<SquareController>();
        newSquareController.InitSquare(squareSpawner.position,gravityDir,looseSpeed);
        newSquareController.SquareLoose();
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
