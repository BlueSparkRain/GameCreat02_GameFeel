using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���У��Է��������ϰ���Ϊ���޿��Ʊ�����
/// </summary>
public class SubCol : MonoBehaviour
{
    /// <summary>
    /// �����еĲ�
    /// </summary>
    public List<Slot> subSlots = new List<Slot>();

    /// <summary>
    /// �����е�ɫ��
    /// </summary>
    public List<Square> subColSquares = new List<Square>();

    /// <summary>
    /// �״γ�������
    /// </summary>
    public bool subColFull;

    [Header("����ɫ�鲹���������")]
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
    /// ��Ŀ��۴��������ⷽ��
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

            if (i >= 1)//������λ������,��һ��ʼ����
            {
                newUsableSlot = subSlots[i].GetComponent<WalkableSlot>();
                //�ڶ�λ����ϲ�
                if (i > 1)
                    upSlot = subSlots[i-1].GetComponent<WalkableSlot>();
                else upSlot = null;
                //�ڵ�λǰ���²�
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
        //���������·��Ĳ�����Ϊ��
        subSlots[maxSpawnSquareNum].GetComponent<WalkableSlot>().isDownEmpty=false;
    }

    int currentSquareNum;

    [Header("���з��������")]
    public int SquareNum;

    public int maxSquareNum;

    /// <summary>
    /// ���±����з���
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index">���������е����</param>
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
    /// �������м�������Ƿ������
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
            //�����¼���ʼ && ÿ�δ������������ü�ʱ
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
                    //������ֻ����һ�Σ��ȴ��´�����
                }
            }
        }
    }

    void ColAddNeededSquare()
    {
        StartCoroutine(BornNeedToAddSquare());
    }

    /// <summary>
    /// �����Ϸ�������ۺ�
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

            //�ڵ����أ���������������

            if (GameLevelCheckManager.Instance.currentLevelIndex == 3)
                spawner.SubColAddSpecialSquare(E_SpecialSquareType.�����ռ�);
            else
            spawner.SubColAddOneRandomSquare();

            yield return bornDelay;
        }
        canAddNeedSquare = true;
        //ÿ�β�����Ϻ�
    }
    bool playerBorn;
    int playerBornIndex;


    int currentSpecialIndex;
    /// <summary>
    /// �����ɾ��ϣ�����������
    /// </summary>
    /// <param name="colorSOList">������ɫ��Ϣ</param>
    /// <param name="bornPlayer">�Ƿ���ҳ�����</param>
    /// <param name="playerIndex">����λ��</param>
    /// <returns></returns>
    public IEnumerator SpawneFirstColSquares(List<ColorSquareSO> colorSOList, List<SubcolCustomSpecialSquare> specialTypesList)
    {
        //currentSpecialIndex = specialTypesList.Count-1;
        currentSpecialIndex = 0;
        //WaitForSeconds delay = new WaitForSeconds(0.35f);
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        for (int i = maxSpawnSquareNum-1; i >=0; i--)
        {
            //��Ҳ���
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
                    //Debug.Log("�У�" + transform.parent.GetSiblingIndex() + "-����" + specialTypesList.Count + "�������");
                    //Debug.Log("�滻Ϊ�����-λ����" + i + "���ͣ�" + specialTypesList[currentSpecialIndex].specialType);
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

    [Header("��������������")]
    public bool isRemoving;

    int superSquareIndex;

    /// <summary>
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    public void RemoveColLine3()
    {
        Debug.Log("���3��");
    }

    /// <summary>
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    public void RemoveColLine4()
    {
        Debug.Log("���4��");
      
    }

    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    void RemoveColLine5()
    {
        Debug.Log("���5��");
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
        E_SuperMarkType superType= E_SuperMarkType.����or����;

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
             superType=E_SuperMarkType.����And����;
                
            }
            else if (toRemoveSquares.Count >= 4) 
            {
             superType=E_SuperMarkType.����or����;
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

            //������ҷ�����
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
    float delayBornTimerInterval = 0.5f;//������Ϊ��0.4s��δ�ظ�����ʱ
    WaitForSeconds bornDelay = new WaitForSeconds(0.25f);

    bool canAddNeedSquare = true;
    bool newRemoveBegin;//�µ�������Ϊ����
    bool newRemoveOver;//һ���������¼�����
    int needAddNum;//��Ҫ��ӵķ���


    /// <summary>
    /// �ӳٲ�������
    /// </summary>
    public void ColDelayBorn() 
    {
        newRemoveBegin = true;
        delayBornTimer = delayBornTimerInterval;
        //canAddNeedSquare = true;

    }

    /// <summary>
    /// ���з��������������������ʱ��
    /// </summary>
    public void ColAddPrepareNeededSquare()
    {
        needAddNum++;
        newRemoveBegin = true;
        delayBornTimer = delayBornTimerInterval;
    }

    /// <summary>
    /// ���п�����������
    /// </summary>
    public void IsColumnRemoving()
    {
        isRemoving = true;
    }

    /// <summary>
    /// ���������������
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

        //�ӵ�һ����Ѱ�ҿ�ʼ����
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

        //��¼����������
        toRemoveSquares.Add(subColSquares[firstIndex]);

        //�ӿ�ʼ����һλ��ʼ����ɫ��
        for (int i = firstIndex + 1; i < subColSquares.Count; ++i)
        {
            //���������ɫ�飬���¼��� �� ������һ��
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



