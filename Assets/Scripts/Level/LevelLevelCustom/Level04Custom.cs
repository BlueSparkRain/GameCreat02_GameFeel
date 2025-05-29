using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level04Custom : MonoBehaviour
{
    public int bossCol;
    public int bossRow;
    GameMap map;
    [Header("Boss击杀要求")]
    public int BossKillTask=1;
    [Header("当前Boss击杀数")]
    public int BossKillNum;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_KillABoss,KillABoss);
    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_KillABoss, KillABoss);
    }

    void KillABoss() 
    {
        BossKillNum ++;

        if (BossKillNum >= BossKillTask) 
        {
            //EventCenter.Instance.EventTrigger(E_EventType.E_TaskTrigger,1);
            StartCoroutine(BossBoom());
        }
    }

    IEnumerator BossBoom() 
    {
        yield return new WaitForSeconds(1);
        EventCenter.Instance.EventTrigger(E_EventType.E_TaskTrigger, 1);

    }

    void Start()
    {
        map = FindAnyObjectByType<GameMap>();
       StartCoroutine(CreateEnemy( 8, bossCol, bossRow));
    }

    IEnumerator CreateEnemy(float delay, int colIndex, int rowIndex)
    {
        yield return new WaitForSeconds(delay);
        map.CreatBoss(colIndex, rowIndex);
    }
}
