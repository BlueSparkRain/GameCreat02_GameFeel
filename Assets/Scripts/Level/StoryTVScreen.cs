using UnityEngine;

public class StoryTVScreen : MonoBehaviour
{
    bool canInto;
    void StoryOver() 
    {
     canInto = true;
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_StoryOver,StoryOver);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_StoryOver,StoryOver);
    }
    private void OnMouseUpAsButton()
    {
        if (canInto)
        {
            SceneLoadManager.Instance.TransToLoadScene(2, E_SceneTranType.ºÚÆÁ¹ý¶É);
        }
    }
}
