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
    /// �����³�����ѡ�����ģʽ
    /// </summary>
    /// <param name="sceneIndex">Ҫ���صĳ������</param>
    /// <param name="type">����ģʽ</param>
    public void TransToLoadScene(int sceneIndex, E_SceneTranType type) 
    {
        uiManager.ShowPanel<SceneTransPanel>(panel => panel.TransToLoadScene(sceneIndex,type),false,true);
    }

    /// <summary>
    /// ж�ص�ǰ�����������³���(��������ֹ����)
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
    /// ��LevelSelect��������ؿ�
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
