using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���շ����������������
/// </summary>
public class GameLevelCheckManager :MonoSingleton<GameLevelCheckManager>
{
    //[Header("���յ÷�")]
    public int currentFinalScore;

    public int currentLevelIndex;

    E_LevelLevel  currentLevelLevel;

    public void SetCurrentLevel(int index) 
    {
        currentLevelIndex = index;
    }

    public void EndLevel(int currentFinalScore) 
    {
        this.currentFinalScore = currentFinalScore;
        UIManager.Instance.ShowPanel<GameOverPanel>(panel=>panel.GetPlayerData(currentLevelLevel, currentFinalScore));
        GetLevelSave(currentLevelIndex);
    }

    public void  CalculateLevel(int finalScore,int S_levelScore) 
    {
       if(finalScore >= S_levelScore) 
       {
            currentLevelLevel = E_LevelLevel.S;
       }
       else if (finalScore >= S_levelScore / 2) 
        {
            currentLevelLevel = E_LevelLevel.A;
            EventCenter.Instance.EventTrigger(E_EventType.E_GetALevel);
        } 
        else if (finalScore >= S_levelScore / 4 *3) 
        {

        }
        else 
        {
            currentLevelLevel = E_LevelLevel.C;

        }
    }

    void GetLevelSave(int levelIndex) 
    {
        Debug.Log("����"+ currentLevelLevel);
        //����ؿ�
        GameProfileSaveManager.Instance.SetProfileLevelData(currentLevelIndex,currentLevelLevel,currentFinalScore);
    }
   
}
