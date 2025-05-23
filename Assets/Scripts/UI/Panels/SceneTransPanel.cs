using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum E_SceneTranType
{
    ����ͼ����,
    ��������,
    δ֪,
}

/// <summary>
/// ת����壬���س���ʱ���й���
/// </summary>

public class SceneTransPanel : BasePanel
{
    public float fadeTime = 0.5f;
    public float LoadingTime = 2;

    [Header("Shaderת��Image�б�")]
    public List<Image> shaderTransImages = new List<Image>();

    [Header("Shaderת����ĻImage")]
    public Transform blackTransImage;

    int curentImageIndex = 0;

    [Header("���س���������")]
    public Image loadingBar;
    [Header("���ؽ��� %")]
    public TMP_Text loadingText;


    SceneLoadManager sceneLoadManager;

    /// <summary>
    /// ת��[����ֻ��SceneLoadManager�ṩʹ��]
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="type"></param>
    public void TransToLoadScene(int sceneIndex, E_SceneTranType type)
    {
        switch (type)
        {
            case E_SceneTranType.����ͼ����:
                Debug.Log("7");
                StartCoroutine(ShaderTransLoading(sceneIndex));
                break;
            case E_SceneTranType.��������:
                Debug.Log("8");
                StartCoroutine(BlackTransLoading(sceneIndex));
                break;
            case E_SceneTranType.δ֪:

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
    bool isSceneTransing;   //ת�����ڽ���
    float sceneTransTimer;  //ת����ʱ��
    Image currentTransImage;//������ת��ͼ

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="sceneIndex">�������</param>
    /// <param name="_dealy">��������ʱ��</param>
    /// <param name="_transTime">��������ʱ��</param>
    /// <returns></returns>
    IEnumerator BlackTransLoading(int sceneIndex, float _dealy = 2f, float _transTime = 1f)
    {
        loadingBar.transform.parent.gameObject.SetActive(false);
        delay = new WaitForSeconds(_dealy);
        //ת����ʼ
        yield return uiTweener.UIDoFade(blackTransImage, 0, 1, _transTime);
        yield return _dealy;
        //���س���
        yield return sceneLoadManager.LoadNewScene(sceneIndex);
        loadingBar.transform.parent.gameObject.SetActive(false);
        yield return _dealy;

        //ת������
        yield return uiTweener.UIDoFade(blackTransImage, 1, 0, _transTime);
        uiManager.HidePanel<SceneTransPanel>();
    }

    /// <summary>
    /// ת��ͼ����
    /// </summary>
    /// <param name="sceneIndex">�������</param>
    /// <param name="_dealy">ת��ά��ʱ��</param>
    /// <param name="_transTime">ת������ʱ��</param>
    /// <returns></returns>
    IEnumerator ShaderTransLoading(int sceneIndex, float _dealy = 0.5f, float _transTime = 1f)
    {
        if (isSceneTransing)
            yield break;
        isSceneTransing = true;
        delay = new WaitForSeconds(_dealy);

        //ת����ʼ
        currentTransImage = GetNextEffectImage();
        yield return TweenHelper.MakeLerp(0, 1, _transTime, val =>
        currentTransImage.material.SetFloat("_TranBar", val));
        //��������ʱ
        sceneTransTimer = 0;
        while (sceneTransTimer < LoadingTime * 0.89f)
        {
            float value = Mathf.Lerp(0, 1, sceneTransTimer / LoadingTime);
            sceneTransTimer += Time.deltaTime * 1.2f;
            yield return null;
            loadingBar.fillAmount = value;
            loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        }
        //���س���
        yield return sceneLoadManager.LoadNewScene(sceneIndex);
        yield return delay;
        loadingBar.fillAmount = 1;
        loadingText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
        yield return delay;
        //ת������
        StartCoroutine(TweenHelper.MakeLerp(1, 0, transTime / 2, val => currentTransImage.material.SetFloat("_TranBar", val)));
        uiManager.HidePanel<SceneTransPanel>();
    }

    /// <summary>
    /// ���ת��Image
    /// </summary>
    /// <returns></returns>
    Image GetNextEffectImage()
    {
        if (curentImageIndex + 1 < shaderTransImages.Count)
            curentImageIndex++;
        else
            curentImageIndex = 0;

        return shaderTransImages[curentImageIndex];
    }

}
