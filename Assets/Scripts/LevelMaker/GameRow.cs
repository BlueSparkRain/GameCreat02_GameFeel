using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRow : MonoBehaviour
{
    public List<Square> rowSquares;
    GameCol col;

    List<Square> toRemoveSquares = new List<Square>();

    public void RemoveWholeRow()
    {
        if (!isRemoving)
        {
            PostProcessManager.Instance.LenDistortionFlash();
            NewRemove();
            StartCoroutine(RemoveSquares(rowSquares, true));
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
        SuperSquare.controller.GetSuperMarkPower(superType, false,true);
    }

    public void UpdateRowSquare(Square square, int index)
    {
        if (rowSquares != null)
        {
            rowSquares[index] = square;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotCapcity"></param>
    public void InitRow(int slotCapcity) 
    {
        rowSquares = new List<Square>();
        col=GetComponent<GameCol>();
        for (int i = 0; i < slotCapcity; i++)
            rowSquares.Add(null);
    }

    //�������
    WaitForSeconds removeDelay = new WaitForSeconds(0.1f);
    //����������
    public bool isRemoving;
    //��������ʱ
    float checkRemoveTimer;
    float checkRemoveInterval=0.25f;//ÿ0.2s����Ƿ��п������������
    //��������ʱ
    bool canCheckRemove;
    //�����м��
    bool canStartCheck=false;

    IEnumerator CheckStart() 
    {
        yield return new WaitForSeconds(4);
        canStartCheck = true;
    
    }
    private void Start()
    {
        StartCoroutine(CheckStart());
    }

    float removingTimer;
    float removingInterval=0.25f;
    

    void Update()
    {


        if(!canStartCheck)
            return;
        if (!canCheckRemove && checkRemoveTimer >= 0)
            checkRemoveTimer -= Time.deltaTime;
        else
            canCheckRemove = true;

        if (canCheckRemove && rowSquares != null)
        {
            CheckToRemoveSquares();
            canCheckRemove=false;
            checkRemoveTimer = checkRemoveInterval;
        }


        if (isRemoving && removingTimer >= 0) 
        {
            removingTimer -= Time.deltaTime;
        }
        else
            isRemoving = false;
    }

    public void CheckToRemoveSquares()
    {
        if (CheckRemoveList() != null && !isRemoving)
        {
            NewRemove();
            StartCoroutine(RemoveSquares(CheckRemoveList()));
        }
    }

    int firstIndex;
    E_Color firstCor;

    int num = 1;
    public List<Square> CheckRemoveList()
    {
        toRemoveSquares.Clear();
        num = 1;
        bool canAdd = true;


        for (int i = 0; i < rowSquares.Count; ++i)
        {
            if (rowSquares[i] == null ||
                !rowSquares[i].GetComponent<ColorSquare>() ||
                !rowSquares[i].GetComponent<ColorSquare>().myData)
                continue; 
           firstCor=rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
           firstIndex = i;
           break; 
        }

        //�������ʣ����������ɫ���ˣ�һ���޷�����������
        if (firstIndex >= rowSquares.Count - 2)
            return null;

        //��¼����������
        toRemoveSquares.Add(rowSquares[firstIndex]);

        for (int i = firstIndex + 1; i < rowSquares.Count; ++i)
        { 
            if (!rowSquares[i] || !rowSquares[i].GetComponent<ColorSquare>())
            {
                if (toRemoveSquares.Count >= 3)
                    return toRemoveSquares;
                num = 0;
                continue;
            }

            if (rowSquares[i].GetComponent<ColorSquare>().myData != null)
            {

                if (rowSquares[i].GetComponent<ColorSquare>().myData.E_Color == firstCor
                    && canAdd)
                {
                    if (!toRemoveSquares.Contains(rowSquares[i]))
                    {
                        toRemoveSquares.Add(rowSquares[i]);
                        num++;
                    }
                }
                else
                {
                    if (num < 3)
                    {
                        firstCor = rowSquares[i].GetComponent<ColorSquare>().myData.E_Color;
                        toRemoveSquares.Clear();
                        canAdd = true;
                        toRemoveSquares.Add(rowSquares[i]);
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

    IEnumerator WaitRowRemove()
    {
        IsRowRemoving();
        yield return new WaitForSeconds(0.6f);
        StopRowRemoving();
    }

    void NewRemove() 
    {
        isRemoving = true;
        removingTimer = removingInterval;
    }



    IEnumerator RemoveSquares(List<Square> removeLists,bool isSuperPower=false)
    {        
         if (isSuperPower) 
         {
            yield return RemoveRowLine(removeLists, isSuperPower, RemoveWholeRow666);
            yield break;
         }   

         if (removeLists.Count <= 2)
         {
            yield break;
         }
         else if (removeLists.Count >= 5)
         {
             yield return RemoveRowLine(removeLists, isSuperPower, RemoveRowLine5);
             yield break;
         }
         else if (removeLists.Count >= 4)
         {
             yield return RemoveRowLine(removeLists, isSuperPower, RemoveRowLine4);
            yield break;
         }
        
         else if (removeLists.Count >= 3)
         {
             yield return RemoveRowLine(removeLists, isSuperPower, RemoveRowLine3);
            yield break;
         }
        
    }

    /// <summary>
    /// ���п�ʼ��������
    /// </summary>
    void IsRowRemoving()
    {
        isRemoving = true;
    }
    /// <summary>
    /// ���������������
    /// </summary>
    void StopRowRemoving()
    {
        isRemoving = false;
    }

    /// <summary>
    /// ����3�����޹��ܣ�ֻ����
    /// </summary>
    void RemoveRowLine3()
    {
        Debug.Log("���3��");

        //5������
    }


    /// <summary>
    /// ����4��������+����1������ը��ɫ��
    /// </summary>
    void RemoveRowLine4()
    {
        Debug.Log("���4��");

        //5������
    }


    /// <summary>
    /// ����5��������+����ɫ�����磺���������ͬ��ɫɫ��
    /// </summary>
    void RemoveRowLine5()
    {
        Debug.Log("���5��");

        //5������
    }

    void RemoveWholeRow666() 
    {
        Debug.Log("�����������");
    }

    IEnumerator RemoveRowLine(List<Square> toRemoveSquares,bool isSuperPwer=false, UnityAction callback = null)
    {
        Square SuperSquare = null;
        int superSquareIndex=0;
        E_SuperMarkType superType = E_SuperMarkType.����or����;

        if (!isSuperPwer)
        {
            if (toRemoveSquares.Count >= 4)
            {
                superSquareIndex = Random.Range(0, toRemoveSquares.Count);
                while (toRemoveSquares[superSquareIndex].GetComponent<PlayerController>())
                {
                    superSquareIndex = Random.Range(1, toRemoveSquares.Count);
                }
                SuperSquare = toRemoveSquares[superSquareIndex];
            }

            if (toRemoveSquares.Count >= 5)
            {
                superType = E_SuperMarkType.����And����;

            }
            else if (toRemoveSquares.Count >= 4)
            {
                superType = E_SuperMarkType.����or����;
            }
        }

        for (int i = 0; i < toRemoveSquares.Count; i++)
        {
            if(toRemoveSquares[i]==null)
                continue;

            if (toRemoveSquares[i].GetComponent<PlayerController>())
            {
                StartCoroutine(toRemoveSquares[i].BeRemoved());
                if (i + 1 >= toRemoveSquares.Count)
                    yield break;
                continue;
            }

            if (!isSuperPwer)
            {
                if (toRemoveSquares.Count >= 4 && i == superSquareIndex)
                {
                    SubCol targetCol = toRemoveSquares[i].transform.GetComponentInParent<SubCol>();
                    targetCol?.ColDelayBorn();
                    continue;
                }
            }


            if (toRemoveSquares[i].transform.parent != null)
            {

                SubCol targetCol = toRemoveSquares[i].transform.GetComponentInParent<SubCol>(); // parent?.parent?.GetComponent<GameCol>();

                if (toRemoveSquares[i] != SuperSquare)
                {
                    StartCoroutine(toRemoveSquares[i].BeRemoved());
                    //������
                    NewRemove();

                    targetCol?.ColAddPrepareNeededSquare();
                    targetCol?.NewRemove();
                    //StartCoroutine(WaitColRemove(targetCol));
                    yield return removeDelay;
                }
            }

            if (SuperSquare != null)
            {
                Debug.Log(SuperSquare);
                StartCoroutine(MakeSuperSquare(SuperSquare, superType));
            }

        }
        callback?.Invoke();
    }

    //IEnumerator WaitColRemove(SubCol targetCol) 
    //{
    //    targetCol?.IsColumnRemoving();
    //    yield return new WaitForSeconds(0.4f);
    //    targetCol?.StopColumnRemoving();
    //}
}
