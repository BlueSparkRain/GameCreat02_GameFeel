using UnityEngine;

public class StoryTVScreen : MonoBehaviour
{

    private void OnMouseUpAsButton()
    {
        SceneLoadManager.Instance.TransToLoadScene(2, E_SceneTranType.ºÚÆÁ¹ý¶É);
    }

}
