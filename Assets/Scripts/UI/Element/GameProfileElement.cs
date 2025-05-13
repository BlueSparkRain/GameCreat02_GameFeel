using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProfileElement : MonoBehaviour
{
    //������ȡ��ʷ�浵����ʾ������Ϣ
    [Header("�浵����ı�")]
    public TMP_Text ProfileIDText;
    private int profileID;

    [Header("ʱ����ı�")]
    public TMP_Text TimeText;

    [Header("��ǰ�ؿ��ı�")]
    public TMP_Text CurrentLevelText;

    [Header("��ǰ�浵�ǳ��ı�")]
    public TMP_Text ProfileNameText;

    [Header("�����ı�")]
    public TMP_Text TechText;

    [Header("���ش浵��ť")]
    public Button LoadProfileButton;

    [Header("ɾ���浵��ť")]
    public Button ClearProfileButton;

    [Header("�浵����")]
    [SerializeField] private ProfileSaveData profileSaveData;

    GameProfileSaveManager gameProfileSaveManager;
    UIManager uiManager;

    WaitForSeconds delay = new WaitForSeconds(0.3f);
    CanvasGroup canvasGroup;

    private void Start()
    {
        LoadProfileButton.onClick.AddListener(() =>
        {
            //���ش浵��Ϣ������ؿ�ѡ����桾��֧�ֹؿ��ڽ��ȴ浵��������

            //if (canOpenWindow)
            //{
            //    canOpenWindow = false;
            //}

            LoadCurrentProfile();
        });

        ClearProfileButton.onClick.AddListener(() =>
        {
            //�û�����ɾ���浵��Ϣ
            CallClearConfirm();
        });

        gameProfileSaveManager = GameProfileSaveManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
        uiManager = UIManager.Instance;
    }

    /// <summary>
    /// ��ȡ�԰��۵Ĵ浵����
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    public void GetProfileData(ProfileSaveData data, int index)
    {
        profileSaveData = data;
        ProfileIDText.text = index.ToString();
        Debug.Log("��ȡ���浵��" + ProfileIDText.text);
        profileID = index;
        UpdateView();
    }

    /// <summary>
    /// ���ݵ�ǰ�浵����UI������ʾ
    /// </summary>
    public void UpdateView()
    {
        if (!profileSaveData.isNewProfile)
        {
            TechText.text = "���������Ϸ";
            //�浵����
            ProfileNameText.text = profileSaveData.profileName;
            //�浵����
            TimeText.text = profileSaveData.saveDate
                            + " " +
                            profileSaveData.saveTime;
            //�浵���¹ؿ�
            CurrentLevelText.text = profileSaveData.currentLevel.ToString();
        }
        else
        {
            TechText.text = "�봴���´浵";
            ProfileNameText.text = "�������´浵������";
            TimeText.text = "������???������";
            CurrentLevelText.text = "0";
        }
    }

    /// <summary>
    ///���ص�ǰ�浵[������´浵����Ҫ������浵����]
    /// </summary>
    void LoadCurrentProfile()
    {
        transform.parent.GetComponentInParent<GameProfilePanel>().SetElementName(this);

        if (profileSaveData.isNewProfile)
        {
            (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).LockAllElemnets();

            //�����뵯�������������浵���ƺ�д��json�ļ�
            uiManager.ShowPanel<Pop_InputField_WindowPanel>(panel => panel.InputToConfirm(OnProfileNameSetValueChange, ProfileNameSetUpAction), true, true);
            //��ǰ�浵��Ϊ���ĵ�
            profileSaveData.isNewProfile = false;
        }
        else //��ʷ�浵
        {
            //�������ؿ�ѡ��
            Debug.Log("������ʷ�浵");
            gameProfileSaveManager.SelectNewProfile(profileSaveData);
            //ȷ�ϵ�ǰ�浵Ϊȫ��
            SceneLoadManager.Instance.TransToLoadScene(1,E_SceneTranType.����ͼ����);
        }
    }

    bool canOpenWindow = true;

    /// <summary>
    /// ����ȷ�ϵ�����ֹ�����
    /// </summary>
    void CallClearConfirm()
    {
        if (canOpenWindow)
        {
            canOpenWindow = false;
            (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).LockAllElemnets();
            uiManager.ShowPanel<Pop_Confirm_WindowPanel>(panel => panel.ToConfirm("�Ƿ�ɾ���浵", ClearCurrentProfile, ClearDisposeAction), true, true);
        }
    }
    /// <summary>
    /// �����ǰ�浵���ɿհ״浵��
    /// </summary>
    void ClearCurrentProfile()
    {
        Debug.Log("ע�⣺" + profileSaveData.ProfileID + "�ѱ���գ�");
        ProfileSaveData newData = new ProfileSaveData(profileSaveData.ProfileID);
        DataSaver.SaveByJson(profileSaveData.ProfileID, newData);
        GetProfileData(newData, profileID);
        //���������浵Ԫ��
        canOpenWindow = true;

        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
    }

    /// <summary>
    /// ȡ��ɾ���浵ָ��
    /// </summary>
    void ClearDisposeAction()
    {
        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
    }

    /// <summary>
    /// �û�ÿ���޸Ĵ浵����
    /// </summary>
    /// <param name="str">��������Ĵ浵����</param>
    void OnProfileNameSetValueChange(String str)
    {
        Debug.Log(profileSaveData.ProfileID + "�õ�������" + str);
        profileSaveData.GetProfileName(str);
        DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
    }
    /// <summary>
    /// �޸���ϣ����ô浵����
    /// </summary>
    void ProfileNameSetUpAction()
    {
        profileSaveData.GetTime();
        DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
        UpdateView();

        canOpenWindow = true;
        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
        //StartCoroutine(LockALittle());
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    public IEnumerator LockSelf()
    {
        yield return delay;
        canvasGroup.interactable = false;

    }
    /// <summary>
    /// �ⶳ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator UnLockSelf()
    {
        yield return delay;
        canvasGroup.interactable = true;
    }

    /// <summary>
    /// ����رգ��Զ�д��浵�ļ�
    /// </summary>
    private void OnApplicationQuit()
    {
        if (profileSaveData != null)
        {
            profileSaveData.GetTime();
            DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
        }
    }
}
