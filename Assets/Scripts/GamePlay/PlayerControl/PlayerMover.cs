using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public AnimationCurve moveCurve;
    Square square;
    // Start is called before the first frame update
    void Start()
    {
        square = GetComponent<Square>();
    }

    void Update()
    {

        MoveOnce();
        ColorationOnce();
    }
    public void MoveOnce() 
    {
        //移动的目标Slot
        SquareColumn targetColumn;


        if (InputManager.Instance.MoveUp)
        {

        }
        else if (InputManager.Instance.MoveDown){ }
        else if (InputManager.Instance.MoveLeft){ }
        else if (InputManager.Instance.MoveRight){ }
    }

    public void ColorationOnce() 
    {
        if (InputManager.Instance.ColorationUp){ }
        else if (InputManager.Instance.ColorationDown){ }
        else if (InputManager.Instance.ColorationLeft){ }
        else if (InputManager.Instance.ColorationRight){ }
    }


    public void Swap() 
    {
    
    }


   
}
