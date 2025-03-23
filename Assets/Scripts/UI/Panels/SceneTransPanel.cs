using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransPanel : BasePanel
{
    public float fadeTime = 0.5f;
    public float LoadingTime = 2;

    [Header("Shader转场Image列表")]
    public List<Image> shaderTransImages = new List<Image>();
    int curentImageIndex = 0;

    [Header("加载场景进度条")]
    public Image loadingBar;
    [Header("加载进度 %")]
    public TMP_Text loadingText;
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, 0.8f);
        yield return new WaitForSeconds(0.5f);
 
        isTransing = false;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return  UITween.Instance.UIDoFade(transform, 0, 1, 0.4f);
    }

    protected override void Init()
    {
        base.Init();
        loadingBar.fillAmount = 0;
    }

    public void SceneLoadingTrans(int index)
    {
        PlayerInputManager.Instance.SetCurrentSelectGameObj(null);
        StartCoroutine(LoadingBar(index));
    }

    IEnumerator ShaderTrans()
    {
        yield return null;
    }

    bool isTransing;

    private IEnumerator LoadingBar(int index)
    {
        if (isTransing)
            yield break;
        isTransing = true;
        //PlayerInputManager.Instance.SetCurrentSelectGameObj(null);

        Image tranImg = NextEffect();
        float timer = 0;
        yield return TweenHelper.MakeLerp(0, 1, transTime, val => tranImg.GetComponent<Image>().material.SetFloat("_TranBar", val)); 
        while (timer < LoadingTime * 0.89f)
        {
            float value = Mathf.Lerp(0, 1, timer / LoadingTime);
            timer += Time.deltaTime*1.2f;
            yield return null;
            loadingBar.fillAmount = value;
            loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        }

        yield return SceneLoadManager.Instance.LoadNewScene(index);

        yield return new WaitForSeconds(0.5f);
        loadingBar.fillAmount = 1;
        loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        yield return new WaitForSeconds(fadeTime);
        StartCoroutine( TweenHelper.MakeLerp(1, 0, transTime / 2, val => tranImg.GetComponent<Image>().material.SetFloat("_TranBar", val))) ;
        UIManager.Instance.HidePanel<SceneTransPanel>();
    }

    Image NextEffect()
    {
        if (curentImageIndex + 1 < shaderTransImages.Count)
            curentImageIndex++;
        else
            curentImageIndex = 0;

        Debug.Log("赶");
        return shaderTransImages[curentImageIndex];
    }

    public override void GamePadClose()
    {
        base.GamePadClose();
    }
}
