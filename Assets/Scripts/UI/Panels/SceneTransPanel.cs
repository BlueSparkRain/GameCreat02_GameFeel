using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum E_SceneTranType
{
    过场图过渡,
    黑屏过渡,
    未知,
}

/// <summary>
/// 转场面板，加载场景时进行过渡
/// </summary>

public class SceneTransPanel : BasePanel
{
    public float fadeTime = 0.5f;
    public float LoadingTime = 2;

    [Header("Shader转场Image列表")]
    public List<Image> shaderTransImages = new List<Image>();

    [Header("Shader转场黑幕Image")]
    public Transform blackTransImage;

    int curentImageIndex = 1;

    [Header("加载场景进度条")]
    public Image loadingBar;
    [Header("加载进度 %")]
    public TMP_Text loadingText;


    SceneLoadManager sceneLoadManager;

    /// <summary>
    /// 转场[建议只对SceneLoadManager提供使用]
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="type"></param>
    public void TransToLoadScene(int sceneIndex, E_SceneTranType type)
    {
        switch (type)
        {
            case E_SceneTranType.过场图过渡:
                StartCoroutine(ShaderTransLoading(sceneIndex));
                break;
            case E_SceneTranType.黑屏过渡:
                StartCoroutine(BlackTransLoading(sceneIndex));
                break;
            case E_SceneTranType.未知:

                break;
        }
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, 0.8f);

        yield return new WaitForSeconds(0.5f);
        blackTransImage.GetComponent<CanvasGroup>().alpha = 0;
        loadingBar.transform.parent.gameObject.SetActive(true);
        isSceneTransing = false;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, 0.4f);
    }

    protected override void Init()
    {
        base.Init();
        if (!sceneLoadManager)
            sceneLoadManager = SceneLoadManager.Instance;
        loadingBar.fillAmount = 0;
    }

    WaitForSeconds delay;
    bool isSceneTransing;   //转场正在进行
    float sceneTransTimer;  //转场计时器
    Image currentTransImage;//黑屏或转场图

    /// <summary>
    /// 黑屏过渡
    /// </summary>
    /// <param name="sceneIndex">场景序号</param>
    /// <param name="_dealy">黑屏持续时长</param>
    /// <param name="_transTime">黑屏过渡时长</param>
    /// <returns></returns>
    IEnumerator BlackTransLoading(int sceneIndex, float _delay = 1f, float _transTime = 1f)
    {
        loadingBar.transform.parent.gameObject.SetActive(false);
        delay = new WaitForSeconds(_delay);
        //转场开始
        yield return uiTweener.UIDoFade(blackTransImage, 0, 1, _transTime);
        yield return delay;
        //加载场景
        yield return sceneLoadManager.LoadNewScene(sceneIndex);
        loadingBar.transform.parent.gameObject.SetActive(false);
        yield return delay;
       
        //转场结束
        //yield return uiTweener.UIDoFade(blackTransImage, 1, 0, _transTime);
        uiManager.HidePanel<SceneTransPanel>();
    }

    /// <summary>
    /// 转场图过渡
    /// </summary>
    /// <param name="sceneIndex">场景序号</param>
    /// <param name="_dealy">转场维持时长</param>
    /// <param name="_transTime">转场过渡时长</param>
    /// <returns></returns>
    IEnumerator ShaderTransLoading(int sceneIndex, float _dealy = 0.25f, float _transTime = 1f)
    {
        if (isSceneTransing)
            yield break;
        isSceneTransing = true;
        delay = new WaitForSeconds(_dealy);

        //转场开始
        currentTransImage = GetNextEffectImage();
        yield return TweenHelper.MakeLerp(0, 1, _transTime, val =>
        currentTransImage.material.SetFloat("_TranBar", val));
        //进度条计时
        sceneTransTimer = 0;
        while (sceneTransTimer < LoadingTime * 0.89f)
        {
            float value = Mathf.Lerp(0, 1, sceneTransTimer / LoadingTime);
            sceneTransTimer += Time.deltaTime * 1.2f;
            yield return null;
            loadingBar.fillAmount = value;
            loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        }
        //加载场景
        yield return sceneLoadManager.LoadNewScene(sceneIndex);
        yield return delay;
        loadingBar.fillAmount = 1;
        loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        yield return delay;
        //转场结束
        StartCoroutine(TweenHelper.MakeLerp(1, 0, transTime / 2, val => currentTransImage.material.SetFloat("_TranBar", val)));
        uiManager.HidePanel<SceneTransPanel>();
    }

    /// <summary>
    /// 获得转场Image
    /// </summary>
    /// <returns></returns>
    Image GetNextEffectImage()
    {
        if (curentImageIndex + 1 > shaderTransImages.Count)
            curentImageIndex++;
        else
            curentImageIndex = 0;

        return shaderTransImages[curentImageIndex];
    }

}
