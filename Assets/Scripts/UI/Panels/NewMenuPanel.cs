using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMenuPanel : BasePanel
{

    public MenuCustomButtonElement TitleElement;
    public Transform buttonFather;
    List<MenuCustomButtonElement> menuCustomButtonElements=new List<MenuCustomButtonElement>();

    //private void Start()
    //{
    //    //uiTweener.UIDoMove(continueGame,)
    //    Init();

    //}
    WaitForSeconds delay=new   WaitForSeconds(0.2f);
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
       yield return null;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
       yield return null;
       TitleElement.SelfAppear();
       StartCoroutine(ShowButtons());
    }

    void ShowFirstTitle() 
    {
    
    }
    IEnumerator ShowButtons() 
    {
        for (int i = 0; i < menuCustomButtonElements.Count; i++) 
        {
            yield return delay;
            menuCustomButtonElements[i].SelfAppear();
        }
    }

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < buttonFather.childCount; i++) 
        {
            menuCustomButtonElements.Add(buttonFather.GetChild(i).GetComponent<MenuCustomButtonElement>());
        }
    }
}
