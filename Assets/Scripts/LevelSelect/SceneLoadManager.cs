using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{

    public IEnumerator LoadNewScene(int index)
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) 
        {
            Debug.Print("niin");
          yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
          SceneManager.LoadScene(index);
          yield break;
        }

        if (index==1)//要回到1场景。需要卸载当前，但不加载1
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        
        if (index != 1 && SceneManager.GetSceneByBuildIndex(index)!= SceneManager.GetActiveScene())
        SceneManager.LoadScene(index,LoadSceneMode.Additive);



          //if(SceneManager.GetSceneByBuildIndex(index) != SceneManager.GetSceneAt(0))

    }


    public IEnumerable UnLoadScene()
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }


    /// <summary>
    /// 从LevelSelect场景进入关卡
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadNewLevel(int sceneIndex) 
    {
       StartCoroutine( LevelSelectManager.Instance.HideLevelSelector());
        UIManager.Instance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(sceneIndex));
    }

    public void EndOneLevel()
    {
        LevelSelectManager.Instance.EndCurrentLevel(2);//2星通过
        UIManager.Instance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(1));
        StartCoroutine(LevelSelectManager.Instance.ShowLevelSelector());
    }

    IEnumerator LoadLevel(int unloadSceneIndex,int sceneIndex) 
    {
       yield return SceneManager.UnloadSceneAsync(unloadSceneIndex);
       
       SceneManager.LoadSceneAsync(sceneIndex);
    }
}
