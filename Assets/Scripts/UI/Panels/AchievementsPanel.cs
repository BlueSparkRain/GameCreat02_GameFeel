using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPanel : BasePanel
{
    [Header("成就data列表")]
    public  List<AchievementDataSO> AchievementsDatas;
    [Header("成就Element父")]
    public Transform AchievementElementsFather;

    private List<AchievementIElement> AchievementElements=new List<AchievementIElement>();

    public int currentPage;

    private int maxPage;

    [Header("左页按钮")]
    public Button leftPageButton;
    [Header("右页按钮")]
    public Button rightPageButton;
    [Header("返回按钮")]
    public Button returnButton;

    bool init;

    protected override void Init()
    {
        base.Init();
        if (!init)
        {
            init = true;

            //计算最大成就页数
            maxPage = AchievementsDatas.Count % 6 != 0 ? 
                AchievementsDatas.Count / 6 + 1 :
                AchievementsDatas.Count / 6;

            //预缓存
            for (int i = 0; i < AchievementElementsFather.childCount; i++)
            {
                AchievementElements.Add(AchievementElementsFather.GetChild(i).
                    GetComponent<AchievementIElement>());
            }


            returnButton.onClick.AddListener(() => { uiManager.HidePanel<AchievementsPanel>(); }); 

        }
        //初始化第一页成就
        ResetPage();
    }


    /// <summary>
    /// 左页：T  右页：F
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
    /// 左页
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
    ///右页
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
        yield return  uiTweener.UIEaseInFrom(E_Dir.下,transform,UIRoot,transTime);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.上,transform,UIRoot,transTime);
    }

}
