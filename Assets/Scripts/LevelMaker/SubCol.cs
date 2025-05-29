using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 子列：以孵化器和障碍物为界限控制本子列
/// </summary>
public class SubCol : MonoBehaviour
{
    /// <summary>
    /// 本子列的槽
    /// </summary>
    public List<Slot> subSlots = new List<Slot>();

    /// <summary>
    /// 本子列的色块
    /// </summary>
    public List<Square> subColSquares = new List<Square>();

    /// <summary>
    /// 首次充满子列
    /// </summary>
    public bool subColFull;

    [Header("本列色块补充最大容量")]
    public int maxSpawnSquareNum;

    SpawnerSlot spawner;

    WalkableSlot newUsableSlot;
    WalkableSlot upSlot;
    WalkableSlot downSlot;
    GameCol selfCol;

    public void RemoveWholeSubCol() 
    {
        if (!isRemoving)
        {
            PostProcessManager.Instance.LenDistortionFlash();
            StartCoroutine(RemoveSquares(subColSquares,true));
        }
    }

    /// <summary>
    /// 在目标槽处产生特殊方块
    /// </summary>
    /// <param name="so"></param>
    /// <param name="slotIndex"></param>
   IEnumerator MakeSuperSquare(Square SuperSquare, E_SuperMarkType superType)
   {
        yield return SuperSquare.SquareMoveAnim();
        SuperSquare.controller.GetSuperMarkPower(superType,true,false);
   }

    public void Init(Vector3 gravityDir, Vector3 looseSpeed)
    {
        spawner = subSlots[0].GetComponent<SpawnerSlot>();
        selfCol=GetComponentInParent<GameCol>();

        spawner.Init(gravityDir, looseSpeed);
        maxSquareNum = subSlots.Count - 1;

        maxSpawnSquareNum = maxSquareNum;

        for (int i = 0; i < subSlots.Count; ++i)
        {
            subSlots[i].Init(this);
            if (subSlots[i].transform.childCount != 0)
                maxSpawnSquareNum--;

            if (i >= 1)//除开首位孵化槽,逐一初始化槽
            {
                newUsableSlot = subSlots[i].GetComponent<WalkableSlot>();
                //第二位起计上槽
                if (i > 1)
                    upSlot = subSlots[i-1].GetComponent<WalkableSlot>();
                else upSlot = null;
                //第低位前计下槽
                if (i < maxSquareNum)
                    downSlot = subSlots[i+1].GetComponent<WalkableSlot>();
                else
                    downSlot = null;
              
                 newUsableSlot.InitUp_DownSlot(upSlot,downSlot);
            };
        }

        for (int i = 0; i < maxSquareNum; ++i)
        {
            subColSquares.Add(null);
        }
        //将子列最下方的槽设置为底
        subSlots[maxSpawnSquareNum].GetComponent<WalkableSlot>().isDownEmpty=false;
    }

    int currentSquareNum;

    [Header("本列方块最大数")]
    public int SquareNum;

    public int maxSquareNum;

    /// <summary>
    /// 更新本子列方块
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index">槽在子列中的序号</param>
    public void UpdateSubColumnSquares(Square square, int index)
    {
        subColSquares[index - 1] = square;

        for (int i = 0; i < subColSquares.Count; ++i)
        {
            if (!subColSquares[i])
            {
                break;
            }
            subColFull = true;
        }

        if (!subColFull)
            return;
        currentSquareNum = 0;
        for (int i = 0; i < subColSquares.Count; ++i)
        {
            if (subColSquares[i])
            {
                currentSquareNum++;
            }
        }
        SquareNum = currentSquareNum;
    }
    public void IsPlayerCol(int playerIndex)
    {
        playerBorn = true;
        this.playerBornIndex = playerIndex;
    }

    public void GetNewSlot(Slot slot)
    {
        slot.transform.SetParent(transform);
        subSlots.Add(slot);
    }

    public void LooseASlot()
    {
       delayBornTimer = delayBornTimerInterval;
    }

    float checkRemoveTimer;
    float checkRemoveInterval = 0.25f;
    bool canCheckRemove;

    float removingTimer;
    float removingInterval = 0.25f;

    /// <summary>
    /// 在子列中检测子列是否可消除
    /// </summary>
    private void Update()
    {
        if (!canCheckRemove && checkRemoveTimer >= 0)
            checkRemoveTimer -= Time.deltaTime;
        else
            canCheckRemove = true;

        if (isRemoving && removingTimer >= 0)
        {
            removingTimer -= Time.deltaTime;
        }
        else
            isRemoving = false;

        if (subColFull)
        {
            if (canCheckRemove && subColSquares != null)
            {
                canCheckRemove=false;
                CheckToRemoveSquares();
                checkRemoveTimer=checkRemoveInterval;
            }
            //消除事件开始 && 每次触发消除，重置计时
            if (newRemoveBegin && delayBornTimer >= 0)  
                delayBornTimer -= Time.deltaTime;
            else
            {
                newRemoveOver = true;
                newRemoveBegin = false;
            }

            if (newRemoveOver && canAddNeedSquare)
            {
                newRemoveOver = false;
                if (!newRemoveBegin && canAddNeedSquare)
                {
                    canAddNeedSquare = false;
                    ColAddNeededSquare();
                    //列生成只触发一次，等待下次消除
                }
            }
        }
    }

    void ColAddNeededSquare()
    {
        StartCoroutine(BornNeedToAddSquare());
    }

    /// <summary>
    /// 待最上方方块入槽后
    /// </summary>
    /// <returns></returns>
    IEnumerator BornNeedToAddSquare()
    {
        for (int i = 0; i < maxSpawnSquareNum-SquareNum; ++i)
        {
            if (SquareNum > maxSpawnSquareNum)
            {
                canAddNeedSquare = true;
                yield break;
            }

            needAddNum--;

            //在第三关，会掉落音符块干扰

            if (GameLevelCheckManager.Instance.currentLevelIndex == 3)
                spawner.SubColAddSpecialSquare(E_SpecialSquareType.消融收集);
            else
            spawner.SubColAddOneRandomSquare();

            yield return bornDelay;
        }
        canAddNeedSquare = true;
        //每次补充完毕后
    }
    bool playerBorn;
    int playerBornIndex;


    int currentSpecialIndex;
    /// <summary>
    /// 随机（删组合）充满本子列
    /// </summary>
    /// <param name="colorSOList">子列颜色信息</param>
    /// <param name="bornPlayer">是否玩家出生列</param>
    /// <param name="playerIndex">出生位数</param>
    /// <returns></returns>
    public IEnumerator SpawneFirstColSquares(List<ColorSquareSO> colorSOList, List<SubcolCustomSpecialSquare> specialTypesList)
    {
        //currentSpecialIndex = specialTypesList.Count-1;
        currentSpecialIndex = 0;
        //WaitForSeconds delay = new WaitForSeconds(0.35f);
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        for (int i = maxSpawnSquareNum-1; i >=0; i--)
        {
            //玩家产生
            if (playerBorn && i == playerBornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), spawner.transform.position, Quaternion.identity, null);
                spawner.ColFallOneSquare(player);
                yield return delay;
                continue;
            }       
            if (currentSpecialIndex < specialTypesList.Count)
            {
                if (specialTypesList.Count > 0 && i == specialTypesList[currentSpecialIndex].index)
                {
                    //Debug.Log("列：" + transform.parent.GetSiblingIndex() + "-我有" + specialTypesList.Count + "个特殊快");
                    //Debug.Log("替换为特殊块-位数：" + i + "类型：" + specialTypesList[currentSpecialIndex].specialType);
                    NewSpecialSquare(specialTypesList[currentSpecialIndex].specialType, specialTypesList[currentSpecialIndex].taskTriggerIndex);
                    yield return delay;
                    continue;
                }
            }
            spawner.SubColAddTargetColorSquare(colorSOList[i]);
            yield return delay;
        }
    }

    void NewSpecialSquare(E_SpecialSquareType specialType,int taskIndex)
    { 
        spawner.SubColAddSpecialSquare(specialType,taskIndex);
        currentSpecialIndex++;
    }

    public void GetTargetSlotNewSquare(WalkableSlot slot) 
    {
       StartCoroutine(spawner.BornSquareToTargetSlot(slot));
    }

    void CheckToRemoveSquares()
    {
        if (CheckRemoveList() != null)
        {
            StartCoroutine(RemoveSquares(CheckRemoveList()));
        }
    }

    public void NewColRemove()
    {
        isRemoving = true;
        removingTimer = removingInterval;
    }

    IEnumerator RemoveSquares(List<Square> removeLists,bool isSuperPower=false)
    {
        if (!isRemoving)
        {
            NewColRemove();
            if (isSuperPower) 
            {
                yield return RemoveLine(removeLists, isSuperPower, RemoveColLine5);
            }

            if (removeLists.Count <= 2)
            {

            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveLine(removeLists, isSuperPower, RemoveColLine5);
            }
            else if (removeLists.Count >= 4)
            {
                yield return RemoveLine(removeLists, isSuperPower,RemoveColLine4);
            }
            else if (removeLists.Count >= 3)
            {
                yield return RemoveLine(removeLists, isSuperPower, RemoveColLine3);
            }
        }
    }

    [Header("本列正在消除中")]
    public bool isRemoving;

    int superSquareIndex;

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

    /// <summary>
    /// 连线5消：消除+生成色块闪电：清除所有相同颜色色块
    /// </summary>
    void RemoveColLine5()
    {
        Debug.Log("完成5消");
    }
    
    void RemoveWholeSubCol666() 
    {
       
    }


    WaitForSeconds removeDelay = new WaitForSeconds(0.1f);
    IEnumerator RemoveLine(List<Square> toRemoveSquares, bool isSuperPower = false, UnityAction callback = null)
    {
        //canRemoveLine = false;
        Square SuperSquare=null;
        int superSquareIndex=0;
        E_SuperMarkType superType= E_SuperMarkType.整行or整列;

        if (!isSuperPower)
        {
            if (toRemoveSquares.Count >= 4)
            {
                superSquareIndex = Random.Range(0, toRemoveSquares.Count);
                
                if ((toRemoveSquares[superSquareIndex] == null))
                     yield break;
                    

                while (toRemoveSquares[superSquareIndex].GetComponent<PlayerController>())
                {
                    superSquareIndex = Random.Range(1, toRemoveSquares.Count);
                }

                SuperSquare = toRemoveSquares[superSquareIndex];
            }

            if (toRemoveSquares.Count >= 5)
            {
             superType=E_SuperMarkType.整行And整列;
                
            }
            else if (toRemoveSquares.Count >= 4) 
            {
             superType=E_SuperMarkType.整行or整列;
             }
        }

        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if(toRemoveSquares[i]==null)
                continue;

            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                yield return toRemoveSquares[i].BeRemoved();
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                continue;
            }

            if (!isSuperPower)
            {
                if (toRemoveSquares.Count >= 4 && i == superSquareIndex)
                {
                    ColDelayBorn();
                    continue;
                }
            }

            //非玩家且非特殊
            if (toRemoveSquares[i] != SuperSquare)
            {
                StartCoroutine(toRemoveSquares[i].BeRemoved());
                NewColRemove();

                ColAddPrepareNeededSquare();
                yield return removeDelay;
            }
        }
        //????
        if (SuperSquare!=null)
        {
            StartCoroutine(MakeSuperSquare(SuperSquare, superType));
        }
        callback?.Invoke();
    }

    float delayBornTimer;
    float delayBornTimerInterval = 0.5f;//消除行为在0.4s内未重复触发时
    WaitForSeconds bornDelay = new WaitForSeconds(0.25f);

    bool canAddNeedSquare = true;
    bool newRemoveBegin;//新的消除行为触发
    bool newRemoveOver;//一连串消除事件结束
    int needAddNum;//需要添加的方块


    /// <summary>
    /// 延迟产生方块
    /// </summary>
    public void ColDelayBorn() 
    {
        newRemoveBegin = true;
        delayBornTimer = delayBornTimerInterval;
        //canAddNeedSquare = true;

    }

    /// <summary>
    /// 向列发送填充请求：重置消除计时器
    /// </summary>
    public void ColAddPrepareNeededSquare()
    {
        needAddNum++;
        newRemoveBegin = true;
        delayBornTimer = delayBornTimerInterval;
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

    public List<Square> toRemoveSquares = new List<Square>();

    int firstIndex;
    E_ColorSquareType firstCor;

        int num = 1;
    public List<Square> CheckRemoveList()
    {
        toRemoveSquares.Clear();
        num = 1;
        bool canAdd = true;

        //从第一个槽寻找开始方块
        for (int i = 0; i < subColSquares.Count; ++i)
        {
            if (subColSquares[i] == null ||
                !subColSquares[i].GetComponent<ColorSquare>() ||
                !subColSquares[i].GetComponent<ColorSquare>().myData)
                continue;

            firstCor = subColSquares[i].GetComponent<ColorSquare>().myData.E_Color;
            firstIndex = i;
            break;
        }

        //记录消除检测起点
        toRemoveSquares.Add(subColSquares[firstIndex]);

        //从开始的下一位开始查找色块
        for (int i = firstIndex + 1; i < subColSquares.Count; ++i)
        {
            //如果遇到非色块，重新计数 并 跳过下一个
            if (!subColSquares[i] || !subColSquares[i].GetComponent<ColorSquare>())
            {
                if (toRemoveSquares.Count >= 3)
                    return toRemoveSquares;
                num = 0;
                continue;
            }

            if (subColSquares[i].GetComponent<ColorSquare>().myData != null)
            {

                if (subColSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor
                && canAdd)
                {
                    if (!toRemoveSquares.Contains(subColSquares[i]))
                    {
                        toRemoveSquares.Add(subColSquares[i]);
                        num++;
                    }
                }
                else
                {
                    if (num < 3)
                    {
                        firstCor = subColSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                        toRemoveSquares.Clear();
                        canAdd = true;
                        toRemoveSquares.Add(subColSquares[i]);
                        num = 1;
                    }
                    else
                    {
                        canAdd = false;
                    }
                }
            }
        }

        if (toRemoveSquares.Count >= 3)
            return toRemoveSquares;
        else
            return null;
    }
}



