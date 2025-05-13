using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRiser : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.PlayBKMusic("BK1");
        StartCoroutine(WaitOpen());
    }
    IEnumerator WaitOpen()
    {
        yield return new WaitForSeconds(2.6f);
        //UIManager.Instance.ShowPanel<MenuPanel>(null);
        UIManager.Instance.ShowPanel<NewMenuPanel>(null);
    }

}
