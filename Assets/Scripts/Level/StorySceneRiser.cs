using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneRiser : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowPanel<BlackPanel>(null);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        yield return new WaitForSeconds(2);
        DialogueManager.Instance.CreatNewUnInteractableDialogue(1, new Vector2(-150, 400), Vector3.one*1.5f);
        yield return new WaitForSeconds(2.5f);
        DialogueManager.Instance.CreatNewUnInteractableDialogue(2, new Vector2(250, -350), Vector3.one *2.0f);
        yield return new WaitForSeconds(1);
        DialogueManager.Instance.CreatNewUnInteractableDialogue(3, new Vector2(100, -100), Vector3.one*1.8f);
        yield return new WaitForSeconds(2.5f);
        UIManager.Instance.HidePanel<BlackPanel>();
    }
}
