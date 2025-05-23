using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    UIManager uiManager;

    protected override void InitSelf()
    {
        base.InitSelf();
        uiManager ??= UIManager.Instance;
    }

    /// <summary>
    /// 加载新场景，选择过渡模式
    /// </summary>
    /// <param name="sceneIndex">要加载的场景序号</param>
    /// <param name="type">过渡模式</param>
    public void TransToLoadScene(int sceneIndex, E_SceneTranType type) 
    {
        uiManager.ShowPanel<SceneTransPanel>(panel => panel.TransToLoadScene(sceneIndex,type),false,true);
    }

    /// <summary>
    /// 卸载当前场景并加载新场景(建议外界禁止调用)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator LoadNewScene(int index)
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(index);
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
     
    }

    IEnumerator LoadLevel(int unloadSceneIndex, int sceneIndex)
    {
        yield return SceneManager.UnloadSceneAsync(unloadSceneIndex);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
