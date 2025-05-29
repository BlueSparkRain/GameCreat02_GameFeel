using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level02Custom : MonoBehaviour
{
    public List<EnemyCreateSetting> enemySetting = new List<EnemyCreateSetting>();
    EnemyCreateSetting current;
    GameMap map;
    
    void Start()
    {
        map = FindAnyObjectByType<GameMap>();
        if (enemySetting.Count > 0)
        {
            for (int i = 0; i < enemySetting.Count; i++)
            {
                current=enemySetting[i];
                StartCoroutine(CreateEnemy(current.delay,current.colIndex,current.rowIndex));
            }
        }
    }

    IEnumerator CreateEnemy(float delay, int colIndex, int rowIndex) 
    {                       
        yield return new WaitForSeconds(delay);
        map.CreateEnemy(colIndex,rowIndex);
    }
}
[Serializable]
public class EnemyCreateSetting 
{
    public float delay;
    public int colIndex;
    public int rowIndex;

}
