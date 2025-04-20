using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTV : MonoBehaviour
{
    public E_LevelLevel level=E_LevelLevel.None;


    public bool isLocked;

    public IEnumerator  UnLockSelf()
    {
        //LevelSelectManager.Instance.GetComponent<CanvasGroup>().interactable = false;
        PlayerInputManager.Instance.SetCurrentSelectGameObj(transform.gameObject);
        yield return new WaitForSeconds(3);
        //LevelSelectManager.Instance.IntoLevelSelectScene(transform.parent.GetSiblingIndex());
        isLocked = false;
        //LevelSelectManager.Instance.GetComponentInParent<CanvasGroup>().interactable = true;
        Debug.Log("½âËø¹Ø¿¨" + transform.parent.GetSiblingIndex());
        //ChangeLeveState(0);
    }


   void OnMouseEnter() 
    {
    
    
    }


    private void OnMouseExit()
    {
            
    }
}






public enum E_LevelLevel
{
  None,B, A, S
}