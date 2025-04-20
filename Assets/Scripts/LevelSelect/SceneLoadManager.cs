using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    UIManager uiManager;
    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    /// <summary>
    /// �����³�����ѡ�����ģʽ
    /// </summary>
    /// <param name="sceneIndex">Ҫ���صĳ������</param>
    /// <param name="type">����ģʽ</param>
    public void TransToLoadScene(int sceneIndex, E_SceneTranType type) 
    {
        uiManager.ShowPanel<SceneTransPanel>(panel => panel.TransToLoadScene(sceneIndex,type),false);
    }

    /// <summary>
    /// ж�ص�ǰ�����������³���
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator LoadNewScene(int index)
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(index);

        yield  break;
        if (index == 0)
        {
            if (SceneManager.GetSceneAt(0) != SceneManager.GetSceneByBuildIndex(1))
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            SceneManager.LoadScene(index);
            yield break;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene(index);
            yield break;
        }

        if (index == 1)//Ҫ�ص�1��������Ҫж�ص�ǰ����������1
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        if (index != 1 && SceneManager.GetSceneByBuildIndex(index) != SceneManager.GetActiveScene())
            SceneManager.LoadScene(index, LoadSceneMode.Additive);

        //if(SceneManager.GetSceneByBuildIndex(index) != SceneManager.GetSceneAt(0))
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
        //StartCoroutine( LevelSelectManager.Instance.HideLevelSelector());

        //uiInstance.ShowPanel<SceneTransPanel>(panel =>  panel.SceneLoadingTrans(sceneIndex));
    }

    public void EndOneLevel(int starNum)
    {
        //uiInstance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(1));

        //LevelSelectManager.Instance.EndCurrentLevel(starNum);
        //StartCoroutine(LevelSelectManager.Instance.ShowLevelSelector());
    }

    IEnumerator LoadLevel(int unloadSceneIndex, int sceneIndex)
    {
        yield return SceneManager.UnloadSceneAsync(unloadSceneIndex);

        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
