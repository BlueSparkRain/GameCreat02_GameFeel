using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnKnowLevel : MonoBehaviour
{
    public Button MenuButton;
    void Start()
    {
        StartCoroutine(SetGameReload());
        MenuButton.interactable = false;
    }

    IEnumerator SetGameReload()
    {
        yield return new WaitForSeconds(2);
        MenuButton.interactable = true;
        PlayerInputManager.Instance.SetCurrentSelectGameObj(MenuButton.gameObject);
        MenuButton.onClick.AddListener(
       () =>
        {
            UIManager.Instance.ShowPanel<SceneTransPanel>(panel =>
            {
                //panel.SceneLoadingTrans(0);
                //DestroyImmediate( LevelSelectManager.Instance.gameObject);
            });

            //PlayerInputManager.Instance.SetCurrentSelectGameObj(MenuButton.gameObject);

            //StartCoroutine(ClearAllPanels());
        });
    }


    //ÆúÓÃ
    // IEnumerator ClearAllPanels() 
    //{
    //    yield return new WaitForSeconds(3);
    //    UIManager.Instance.DestoryAllPanels();
    //}
}
