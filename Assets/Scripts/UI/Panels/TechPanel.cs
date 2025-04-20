using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TechPanel : BasePanel
{
    int index;
    
    //public override void GamePadClose()
    //{
    //    base.GamePadClose();
    //}

    public override void HidePanel()
    {
        base.HidePanel();
        index = 0;
        canAct = true;
     }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(UIRoot, new Vector3(-4000, 0, 0), new Vector3(-6000, 0, 0), transTime / 2);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 3);
        UIRoot.transform.position = new Vector2(4000,0);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        //StartCoroutine(Wait());
    }

    bool canAct=true;

    private void Update()
    {
        if (PlayerInputManager.Instance.ColorationRight || Keyboard.current.backspaceKey.wasPressedThisFrame)
           StartCoroutine( NextPage());
    }


    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform,0,1,transTime/3);
        yield return UITween.Instance.UIDoMove(UIRoot,new Vector3(2000,0,0),Vector3.zero,transTime/3);
    }

    IEnumerator NextPage()
    {
        if (!canAct)
             yield break;
        canAct = false;
        index++;
        if ( index > 2)
        {
            UIManager.Instance.HidePanel<TechPanel>();
            yield break ;
        }
        Debug.Log("обр╩рЁ");
        yield return UITween.Instance.UIDoLocalMove(UIRoot, new Vector3(-2000, 0, 0),transTime / 3);
        canAct=true;
    }

    protected override void Init()
    {
        base.Init();
    }
}
