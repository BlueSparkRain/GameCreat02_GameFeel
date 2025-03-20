using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRiser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel<MenuPanel>(null);
        MusicManager.Instance.PlayBKMusic("BK1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
