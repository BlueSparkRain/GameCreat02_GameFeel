using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectManager : MonoSingleton<LevelSelectManager>
{
    public List<MylevelSelectButton> levelSelectButtons = new List<MylevelSelectButton>();
    public Transform buttonContainer;
    public Transform Root;
    public int currentLevelIndex;
    Transform VCam;

    public GameObject FirstSelectButton;
    public Transform LockBoard;//当前关卡未解锁，选择将弹窗

    bool isLockReminging;

    public  IEnumerator LockRemindShow() 
    {
       if(isLockReminging)
            yield break;
        isLockReminging = true;
        StartCoroutine( Shake());
        yield return UITween.Instance.UIDoLocalMove(LockBoard,new Vector2(0,-450),0.3f);
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 5.8f), 0.08f, val => LockBoard.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -5.8f), new Vector3(0, 0, 5.8f), 0.08f, val => LockBoard.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 5.8f), Vector3.zero, 0.08f, val => LockBoard.eulerAngles = val);
        yield return LockRemindClose();
        isLockReminging = false;
    }

    IEnumerator Shake()
    {
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 5.8f), 0.08f, val => VCam.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -5.8f), new Vector3(0, 0, 5.8f), 0.08f, val => VCam.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 5.8f), Vector3.zero, 0.08f, val => VCam.eulerAngles = val);
    }

    IEnumerator LockRemindClose()
    {
        yield return UITween.Instance.UIDoLocalMove(LockBoard, new Vector2(0, 450), 0.3f);
    }

    public IEnumerator ShowLevelSelector() 
    {
        yield return UITween.Instance.UIDoFade(transform,0,1,0.5f);
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(0,2000),0.3f);

        transform.GetComponent<CanvasGroup>().interactable = true;
        if (FirstSelectButton)
        {
            //EventSystem.current.SetSelectedGameObject(FirstSelectButton);
            PlayerInputManager.Instance.SetCurrentSelectGameObj(FirstSelectButton);
        }
    } 
    public IEnumerator HideLevelSelector() 
    {
        transform.GetComponent<CanvasGroup>().interactable = false;
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(0, -2000), 0.3f);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, 0.5f);
    }

    void Start()
    {
        VCam = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().transform;
        //注册所有按钮
        for (int i = 0; i < buttonContainer.childCount; i++)
        {
           levelSelectButtons.Add(buttonContainer.GetChild(i).GetComponentInChildren<MylevelSelectButton>());
        }
   
        StartCoroutine(IntoLevelSelectScene());
    }

    IEnumerator IntoLevelSelectScene() 
    {
        yield return new WaitForSeconds(2);

        levelSelectButtons[0].UnLockSelf();

        currentLevelIndex = 0;
        PlayerInputManager.Instance.SetCurrentSelectGameObj(FirstSelectButton);
    }




    /// <summary>
    /// 完成当前关卡
    /// </summary>
    /// <param name="score"></param>
    public void EndCurrentLevel(int  starNum) 
    {
        levelSelectButtons[currentLevelIndex].ChangeLeveState(starNum);
        UnLockNewLevel();

    }

    /// <summary>
    /// 解锁新关卡
    /// </summary>
    /// <param name="Index"></param>
    public void UnLockNewLevel()
    {
        currentLevelIndex++;
        levelSelectButtons[currentLevelIndex].UnLockSelf();
    }

    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneLoadManager.Instance.EndOneLevel();
        }
    }

}
