using UnityEngine;

public class SuperMark : MonoBehaviour
{

    public SpriteRenderer RimSprite;

    public GameObject UpArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;
    public GameObject RightArrow;

    public void SetSprite(Sprite sprite) 
    {
      RimSprite.sprite = sprite;
    }

    public void ShowColArrow() 
    {
        UpArrow.SetActive(true);
        DownArrow.SetActive(true);

    }
    public void ShowRowArrow() 
    {
        LeftArrow.SetActive(true);
        RightArrow.SetActive(true);
    } 

    public void ShowColAndRow() 
    {
        ShowColArrow();
        ShowRowArrow();
    }

}
