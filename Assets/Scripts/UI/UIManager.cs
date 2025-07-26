using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
public enum E_UILayer
{
    Bottom,
    Middle,
    Top,
    System,
}

/// <summary>
/// ��������UI���
/// ע�⣺Ԥ����������������豣��һ��
/// </summary>
public class UIManager : BaseSingleton<UIManager>
{
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    /// <summary>
    /// ���򿪵����
    /// </summary>
    public BasePanel currentPanel;


    //private List<BasePanel> HistoryPanels=new List<BasePanel>();

    /// <summary>
    /// ��ȡ��Ŀ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public BasePanel GetPanel<T>()
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName];
        }
        Debug.Log("δ��ѯ��Ŀ�����");
        return null;
    }


    /// <summary>
    /// ʹ��UI��������Ŀ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callBack">ί�лص��ַ�</param>
    /// <param name="isSubPanel">����壨�������ʱ����岻��رգ�</param>
    /// <param name="layer">Ŀ�����ɲ�</param>
    /// <param name="isSync">ʹ���첽��δ������</param>
    public void ShowPanel<T>(UnityAction<T> callBack, bool isSubPanel = false, bool isPopWindow=false) where T : BasePanel
    {
       

        //��ȡ�������Ԥ����������������豣��һ��
        string panelName = typeof(T).Name;
        BasePanel panel;
        //�������
        if (panelDic.ContainsKey(panelName))
        {
            panel = panelDic[panelName];
            if (!panel.gameObject.activeSelf)
                panel.gameObject.SetActive(true);

            //����Ѿ����ڸ���壬�䷢����������ָ��
            //if(isSubPanel)
                //HistoryPanels.Add(currentPanel);

            if(!isPopWindow)
                currentPanel = panel;
            


            panel.ShowPanel();
            MonoManager.Instance.StartCoroutine(panel.ShowPanelTweenEffect());
            Debug.Log("����Ѵ���:" + panelName);
            callBack?.Invoke(panelDic[panelName] as T);
            return;
        }
        //��������壬�ȼ�����Դ
        GameObject panelobj = Resources.Load<GameObject>("Prefab/UIPanel/" + panelName);

        //�����Ԥ�Ƽ���������Ӧ��layer�£�������ԭ�������Ŵ�С
        panelobj = GameObject.Instantiate(panelobj);
        panelobj.transform.SetAsFirstSibling();

        //��ȡ��ӦUI�������
        panel = panelobj.GetComponent<BasePanel>();


        //if (isSubPanel)
            //HistoryPanels.Add(currentPanel);

        if (!isPopWindow)
            currentPanel = panel;

        panel.ShowPanel();
        MonoManager.Instance.StartCoroutine(panel.ShowPanelTweenEffect());

        Debug.Log("����壡" + panelName);
        callBack?.Invoke(panel as T);
        //�洢panel
        if (!panelDic.ContainsKey(panelName))
        {
            panelDic.Add(panelName, panel);
        }
    }

    public void HidePanel<T>(bool isDestroy = false) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isDestroy)
            {
                //ѡ������ʱ�Ż��Ƴ��ֵ�
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
            else //��ʧ��
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
