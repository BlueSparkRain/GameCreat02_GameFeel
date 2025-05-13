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

    /// <summary>
    /// ��Ŀ��۴��������ⷽ��
    /// </summary>
    /// <param name="so"></param>
    /// <param name="slotIndex"></param>
   IEnumerator MakeSuperSquare(Square SuperSquare)
    {
        yield return  new WaitForSeconds(0.3f);
        yield return SuperSquare.SquareMoveAnim();
      //yield return SuperSquare.SquareMoveAnim();
      var superEffect= Instantiate(Resources.Load<GameObject>("Prefab/SuperMark/SuperMark"));
      superEffect.transform.SetParent(SuperSquare.transform);
      superEffect.transform.localPosition = Vector3.zero; 
    }


    public void Init(Vector3 gravityDir, Vector3 looseSpeed)
    {
        spawner = subSlots[0].GetComponent<SpawnerSlot>();
        spawner.Init(gravityDir, looseSpeed);
        maxSquareNum = subSlots.Count - 1;
        //Test
        //Debug.Log("���ж�ȡ���");

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
        //Debug.Log("�ɲ��Ƴ�");
        removeTimer = removeTimerInterval;
    }

    float checkRemoveTimer;
    float checkRemoveInterval = 0.35f;
    bool canCheckRemove;

    /// <summary>
    /// �������м�������Ƿ������
    /// </summary>
    private void Update()
    {
        if (!canCheckRemove && checkRemoveTimer >= 0)
            checkRemoveTimer -= Time.deltaTime;
        else
            canCheckRemove = true;

        if (subColFull)
        {
            if (canCheckRemove && subColSquares != null)
            {
                RemoveSquares();
                canCheckRemove=false;
                checkRemoveTimer=checkRemoveInterval;
            }
            //�����¼���ʼ && ÿ�δ������������ü�ʱ
            if (newRemoveBegin && removeTimer >= 0)
                removeTimer -= Time.deltaTime;
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

                    if (needAddNum == 0 && SquareNum < maxSpawnSquareNum)
                        needAddNum = 1;


                    //������ֻ����һ�Σ��ȴ��´�����
                    ColAddNeededSquare();
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
        //Test
        //Debug.Log("��ʼ������Ҫ���飺" + needAddNum);

        for (int i = 0; i < needAddNum; ++i)
        {
            if (SquareNum > maxSpawnSquareNum)
            {
                //Test
                //Debug.Log("��");
                yield break;
            }

            needAddNum--;

            spawner.SubColAddOneRandomSquare();
            yield return bornDelay;
        }
        canAddNeedSquare = true;
        //ÿ�β�����Ϻ�
    }
    bool playerBorn;
    int playerBornIndex;

    /// <summary>
    /// �����ɾ��ϣ�����������
    /// </summary>
    /// <param name="soList">������ɫ��Ϣ</param>
    /// <param name="bornPlayer">�Ƿ���ҳ�����</param>
    /// <param name="playerIndex">����λ��</param>
    /// <returns></returns>
    public IEnumerator SpawneFirstColSquares(List<ColorSquareSO> soList)
    {
        WaitForSeconds delay = new WaitForSeconds(0.25f);
        for (int i = maxSpawnSquareNum-1; i >=0; i--)
        {
            //��Ҳ���
            if (playerBorn && i == playerBornIndex)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSquare"), spawner.transform.position, Quaternion.identity, null);
                StartCoroutine(spawner.ColFallOneSquare(player));

                yield return delay;
                continue;
            }
            spawner.SubColAddTargetSquare(soList[i]);
            yield return delay;
        }
    }

    public void RemoveSquares()
    {
        if (CheckRemoveList() != null)
        {
            StartCoroutine(CheckAndRemoveSquares(CheckRemoveList()));
        }
    }

    IEnumerator CheckAndRemoveSquares(List<Square> removeLists)
    {
        if (!GetComponentInParent<GameRow>().isRemoving && !isRemoving)
        {
            IsColumnRemoving();

            if (removeLists.Count <= 2)
            {

            }
            else if (removeLists.Count >= 5)
            {
                yield return RemoveLine(removeLists, RemoveColLine5);
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


    //WaitForSeconds removeDelay = new WaitForSeconds(0.12f);
    WaitForSeconds removeDelay = new WaitForSeconds(0.1f);
    IEnumerator RemoveLine(List<Square> toRemoveSquares, UnityAction callback = null)
    {
        Square SuperSquare=null;
        int superSquareIndex=0;
        if (toRemoveSquares.Count >= 3)
        {
            superSquareIndex = Random.Range(0, toRemoveSquares.Count);
            while (toRemoveSquares[superSquareIndex].GetComponent<PlayerController>()) 
            {
               superSquareIndex = Random.Range(1, toRemoveSquares.Count);
            }

            SuperSquare = toRemoveSquares[superSquareIndex];
        }

        for (int i = 0; i < toRemoveSquares.Count; ++i)
        {
            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                yield return toRemoveSquares[i].BeRemoved();
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                continue;
                //i++;
            }

            if (toRemoveSquares.Count >= 3 && i == superSquareIndex)
            {
                newRemoveBegin = true;
                removeTimer = removeTimerInterval;
                canAddNeedSquare = true;
                Debug.Log("̫����"+superSquareIndex+"-"+SuperSquare);
                //yield return removeDelay;
                continue;
            }

            //������ҷ�����
            StartCoroutine(toRemoveSquares[i].BeRemoved());
            ColPrepareNeededSquare();
            yield return removeDelay;
        }

        //????
        if (SuperSquare!=null)
        {
        Debug.Log(SuperSquare);
            StartCoroutine(MakeSuperSquare(SuperSquare));
        }



        callback?.Invoke();
    }

    float removeTimer;
    float removeTimerInterval = 0.4f;//������Ϊ��0.4s��δ�ظ�����ʱ
    WaitForSeconds bornDelay = new WaitForSeconds(0.2f);

    bool canAddNeedSquare = true;
    bool newRemoveBegin;//�µ�������Ϊ����
    bool newRemoveOver;//һ���������¼�����
    int needAddNum;//��Ҫ��ӵķ���

    /// <summary>
    /// ���з��������������������ʱ��
    /// </summary>
    public void ColPrepareNeededSquare()
    {
        needAddNum++;
        newRemoveBegin = true;
        //Test
        //Debug.Log("�����¼����ã�");
        removeTimer = removeTimerInterval;
        canAddNeedSquare = true;
    }

    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    public void RemoveColLine5()
    {
        Debug.Log("���5��");
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
    E_Color firstCor;

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
