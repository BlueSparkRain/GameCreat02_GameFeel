using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRiser : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.PlayBKMusic("5-π»”Í");
        StartCoroutine(WaitOpen());
    }
    IEnumerator WaitOpen()
    {
        yield return new WaitForSeconds(2.6f);
        UIManager.Instance.ShowPanel<NewMenuPanel>(null);
    }

    public void InitButton()
    {
        DataSaver.SaveByJson(JsonFileName.Profile1, new ProfileSaveData(JsonFileName.Profile1));
        DataSaver.SaveByJson(JsonFileName.Profile2, new ProfileSaveData(JsonFileName.Profile2));
        DataSaver.SaveByJson(JsonFileName.Profile3, new ProfileSaveData(JsonFileName.Profile3));
    }
}
