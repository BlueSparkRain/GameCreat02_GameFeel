using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollectablesViewer : MonoBehaviour
{
    [Header("��ǰ���ռ�����Text")]
    public TMP_Text currentCollectNum;
    [Header("Ŀ���ռ�����Text")]
    public TMP_Text targetCollectNum;

    [Header("Ŀ���ռ���Image")]
    public Image collectImage;

    /// <summary>
    /// CollectableUIObj��ʼ��
    /// </summary>
    /// <param name="targetNum"></param>
    public void InitSelf(int targetNum, Sprite sprite)
    {
        currentCollectNum.text = "0";
        targetCollectNum.text = targetNum.ToString();
        collectImage.sprite = sprite;
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void UpdateSelf(int newCurrentNum)
    {
        currentCollectNum.text = newCurrentNum.ToString();
    }
}
