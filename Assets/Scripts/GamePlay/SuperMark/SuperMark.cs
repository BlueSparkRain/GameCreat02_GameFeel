using UnityEngine;

public class SuperMark : MonoBehaviour
{

    public GameObject UpArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;
    public GameObject RightArrow;

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
