using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRiser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.PlayBKMusic("BK1");
        UIManager.Instance.ShowPanel<MenuPanel>(null);
    }
}
