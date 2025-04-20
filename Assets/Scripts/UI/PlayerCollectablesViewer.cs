using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollectablesViewer : MonoBehaviour
{
    [Header("当前已收集个数Text")]
    public TMP_Text currentCollectNum;
    [Header("目标收集个数Text")]
    public TMP_Text targetCollectNum;

    [Header("目标收集物Image")]
    public Image collectImage;

    /// <summary>
    /// CollectableUIObj初始化
    /// </summary>
    /// <param name="targetNum"></param>
    public void InitSelf(int targetNum, Sprite sprite)
    {
        currentCollectNum.text = "0";
        targetCollectNum.text = targetNum.ToString();
        collectImage.sprite = sprite;
    }

    /// <summary>
    /// 更新自身数据
    /// </summary>
    public void UpdateSelf(int newCurrentNum)
    {
        currentCollectNum.text = newCurrentNum.ToString();
    }
}
