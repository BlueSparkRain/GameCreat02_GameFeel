using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRiser : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.PlayBKMusic("LEVEL-水滴");
        StartCoroutine(WaitOpen());
    }
    void ConformAcion() {
    InitButton();   
    }
    void DisposeAcion() {
    
    }
    IEnumerator WaitOpen()
    {
        yield return new WaitForSeconds(0.6f);
        UIManager.Instance.ShowPanel<Pop_Confirm_WindowPanel>(panel => panel.ToConfirm("首次游戏", ConformAcion, DisposeAcion));
        yield return new WaitForSeconds(2.0f);

        UIManager.Instance.ShowPanel<NewMenuPanel>(null);
    }

    public void InitButton()
    {
        DataSaver.SaveByJson(JsonFileName.Profile1, new ProfileSaveData(JsonFileName.Profile1));
        DataSaver.SaveByJson(JsonFileName.Profile2, new ProfileSaveData(JsonFileName.Profile2));
        DataSaver.SaveByJson(JsonFileName.Profile3, new ProfileSaveData(JsonFileName.Profile3));
    }
}
