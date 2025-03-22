using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum E_UILayer
{
    Bottom,
    Middle,
    Top,
    System,
}

/// <summary>
/// 管理所有UI面板
/// 注意：预制体名和面板类名需保持一致
/// </summary>
public class UIManager : BaseSingleton<UIManager>
{

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    public void ShowPanel<T>(UnityAction<T> callBack, E_UILayer layer = E_UILayer.Middle, bool isSync = false) where T : BasePanel
    {
        //获取面板名，预制体名和面板类名需保持一致
        string panelName = typeof(T).Name;
        BasePanel panel;
        //存在面板
        if (panelDic.ContainsKey(panelName))
        {
            panel = panelDic[panelName];
            if (!panel.gameObject.activeSelf)
                panel.gameObject.SetActive(true);

            panel.ShowPanel();
            MonoManager.Instance.StartCoroutine(panel.ShowPanelTweenEffect());
            Debug.Log("面板已存在:" + panelName);
            callBack?.Invoke(panelDic[panelName] as T);
            return;
        }
        //不存在面板，先加载资源
        GameObject panelobj = Resources.Load<GameObject>("Prefab/UIPanel/" + panelName);

        //将面板预制件创建到对应父layer下，并保持原本的缩放大小
        panelobj = GameObject.Instantiate(panelobj);
        panelobj.transform.SetAsFirstSibling();

        //获取对应UI组件返回
        panel = panelobj.GetComponent<BasePanel>();
        panel.ShowPanel();
        MonoManager.Instance.StartCoroutine(panel.ShowPanelTweenEffect());

        Debug.Log("新面板！" + panelName);
        callBack?.Invoke(panel as T);
        //存储panel
        if (!panelDic.ContainsKey(panelName))
        {
            panelDic.Add(panelName, panel);
        }
    }

    public void HidePanel<T>(bool isDestroy = false)
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isDestroy)
            {
                //选择销毁时才会移出字典
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
            else //仅失活
                MonoManager.Instance.StartCoroutine(PanelHideEnd(panelDic[panelName]));
        }
    }
    IEnumerator PanelHideEnd(BasePanel panel)
    {
        panel.HidePanel();
        yield return panel.HidePanelTweenEffect();
        panel.gameObject.SetActive(false);
    }
}
