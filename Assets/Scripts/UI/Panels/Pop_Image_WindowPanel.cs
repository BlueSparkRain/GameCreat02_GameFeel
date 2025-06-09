using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Pop_Image_WindowPanel : BasePanel
{
    [Header("ȷ�ϰ�ť")]
    public Button ConfirmButton;

    [Header("��ϢͼƬ")]
    public Image image;
    [Header("��Ƶ������")]
    public VideoPlayer videoPlayer;

    [Header("RowͼƬ")]
    public RawImage rowImage;

    [Header("ͼƬ��Ϣ")]
    public TMP_Text ImageText;
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }

    protected override void Init()
    {
        base.Init();
    }

    public void ToShow(string imageText, Sprite  image,VideoClip videoClip=null,RenderTexture rt=null)
    {
      ImageText.text=imageText;
      this.image.sprite = image;
      //֧����Ƶ����
      if (videoClip != null)
      {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
            videoPlayer.targetTexture=rt;
            rowImage.gameObject.SetActive(true);
            rowImage.texture = videoPlayer.targetTexture;
      }
      else
            rowImage.gameObject.SetActive(false);
    }

    public void OnClickConformButton() 
    {
        uiManager.HidePanel<Pop_Image_WindowPanel>();
    }
}
