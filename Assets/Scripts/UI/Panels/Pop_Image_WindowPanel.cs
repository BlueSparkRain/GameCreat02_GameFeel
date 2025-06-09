using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Pop_Image_WindowPanel : BasePanel
{
    [Header("确认按钮")]
    public Button ConfirmButton;

    [Header("信息图片")]
    public Image image;
    [Header("视频播放器")]
    public VideoPlayer videoPlayer;

    [Header("Row图片")]
    public RawImage rowImage;

    [Header("图片信息")]
    public TMP_Text ImageText;
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
    }

    protected override void Init()
    {
        base.Init();
    }

    public void ToShow(string imageText, Sprite  image,VideoClip videoClip=null,RenderTexture rt=null)
    {
      ImageText.text=imageText;
      this.image.sprite = image;
      //支持视频播放
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
