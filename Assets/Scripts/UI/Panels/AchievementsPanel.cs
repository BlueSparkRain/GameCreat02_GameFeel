using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPanel : BasePanel
{
    [Header("�ɾ�data�б�")]
    public  List<AchievementDataSO> AchievementsDatas;
    [Header("�ɾ�Element��")]
    public Transform AchievementElementsFather;

    private List<AchievementIElement> AchievementElements=new List<AchievementIElement>();

    public int currentPage;

    private int maxPage;

    [Header("��ҳ��ť")]
    public Button leftPageButton;
    [Header("��ҳ��ť")]
    public Button rightPageButton;
    [Header("���ذ�ť")]
    public Button returnButton;

    bool init;

    protected override void Init()
    {
        base.Init();
        if (!init)
        {
            init = true;

            //�������ɾ�ҳ��
            maxPage = AchievementsDatas.Count % 6 != 0 ? 
                AchievementsDatas.Count / 6 + 1 :
                AchievementsDatas.Count / 6;

            //Ԥ����
            for (int i = 0; i < AchievementElementsFather.childCount; i++)
            {
                AchievementElements.Add(AchievementElementsFather.GetChild(i).
                    GetComponent<AchievementIElement>());
            }


            returnButton.onClick.AddListener(() => { uiManager.HidePanel<AchievementsPanel>(); }); 

        }
        //��ʼ����һҳ�ɾ�
        ResetPage();
    }


    /// <summary>
    /// ��ҳ��T  ��ҳ��F
    /// </summary>
    /// <param name="index"></param>
    public void OnClickPageButton(bool isLeftPage) 
    {
        if (isLeftPage) 
           leftPage();
        else 
           RightPage();
    }

    void ResetPage() 
    {
        currentPage = 1;
        FreezeButton(leftPageButton);
        GetWholePageData(currentPage);
    }

    /// <summary>
    /// ��ҳ
    /// </summary>
    void leftPage() 
    {
        currentPage --;

        if (currentPage == maxPage-1)
            UnFreezeButton(rightPageButton);
        else if(currentPage == 1)
        FreezeButton(leftPageButton);

        GetWholePageData(currentPage);
    }

    /// <summary>
    ///��ҳ
    /// </summary>
    void RightPage() 
    {
        currentPage++;

        if (currentPage == 2)
            UnFreezeButton(leftPageButton);
        else if(currentPage==maxPage)
            FreezeButton(rightPageButton);

        GetWholePageData(currentPage);
    }


    void GetWholePageData(int page) 
    {
        int firstSO = (page-1)*6;
        for (int i = 0; i < AchievementElements.Count; i++) 
        {
            if (firstSO + i<AchievementsDatas.Count)
            {
                AchievementDataSO data = AchievementsDatas[firstSO + i];
                AchievementElements[i].UpdateAchievementData(data);        
            }
        }
    }


    void FreezeButton(Button button) 
    {
      button.interactable = false;
    }

    void UnFreezeButton(Button button) 
    {
      button.interactable = true;
    }


    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return  uiTweener.UIEaseInFrom(E_Dir.��,transform,UIRoot,transTime);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��,transform,UIRoot,transTime);
    }

}
