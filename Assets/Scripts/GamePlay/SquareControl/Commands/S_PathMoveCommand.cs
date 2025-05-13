using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_PathMoveCommand : SquareCommand
{
    public S_PathMoveCommand(Square square,GameMap gameMap) : base(square)
    {
        this.gameMap = gameMap;
    }

    GameMap gameMap;
    Square targetSquare;
    SquareController otherSquareController;

    public void TryGetSwapTarget(E_CustomDir dir) 
    {
        if (controlSquare != null)
        {
            if(RayChecker.CheckTargetLayerObj(LayerMask.GetMask("Square"), controlSquare.transform.position, dir)!=null)
            targetSquare = RayChecker.CheckTargetLayerObj(LayerMask.GetMask("Square"), controlSquare.transform.position, dir).GetComponent<Square>();
            //Debug.Log("要交换目标:" +targetSquare +"-"+ (targetSquare as ColorSquare).myData.E_Color);
        }
    }

    public override void Excute()
    {
        base.Excute();
        if(targetSquare!=null && targetSquare.gameObject.activeInHierarchy)
        mono.StartCoroutine(Swap(targetSquare));
    }


    public IEnumerator Swap(Square otherSquare)
    {
        SquareController controller;
        controller=controlSquare.GetComponent<SquareController>();
        otherSquareController = otherSquare.GetComponent<SquareController>();
        //交换音效播放
        MusicManager.Instance.PlaySound("swap", 2);

        Transform mySlot =controlSquare.transform.parent;
        otherSquare.HasFather = false;
        controlSquare.HasFather = false;


        controlSquare.transform.SetParent(otherSquare.transform.parent);
        otherSquare.transform.SetParent(mySlot);

        if (controlSquare.transform.parent != null && controlSquare.transform.parent.GetComponent<WalkableSlot>())
            controller.SquareMoveToSlot(controlSquare.transform.parent.position);

        if (otherSquare != null && mySlot != null)
            otherSquareController.SquareMoveToSlot(mySlot.position);
       
        if (controlSquare.transform.parent != null)
        {
            mono.StartCoroutine(SlowMove(controlSquare.transform.position, controlSquare.transform.parent.position, 0.07f));

            if (controlSquare.slot)
            {
                controlSlot = controlSquare.slot;
                controlSlot.selfColumn.UpdateSubColumnSquares(controlSquare, controlSlot.transform.GetSiblingIndex());
                gameMap.UpdateRowSquares(controlSquare, controlSlot.transform.parent.parent.GetSiblingIndex(), controlSlot.mapIndex);
            }
            yield return controlSquare.SquareMoveAnim();
        }
    }


    AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    IEnumerator SlowMove(Vector3 startPos, Vector3 targetPos, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            controlSquare.transform.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer / duration));
            yield return null;
        }
    }


}
