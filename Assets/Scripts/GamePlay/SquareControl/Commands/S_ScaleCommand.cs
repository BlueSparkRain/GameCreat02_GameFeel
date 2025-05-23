using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ScaleCommand : SquareCommand
{
    Vector3 startValue;
    Vector3 endValue;
    float tranTine = 0.3f;
    public S_ScaleCommand(Square square) : base(square)
    {

    }

    public void GetScaleTask(Vector3 startValue,Vector3 endValue,float tranTine = 0.3f) 
    {
        this.startValue = startValue;
        this.endValue = endValue;
        this.tranTine = tranTine;
    }

    public override void Excute()
    {
        base.Excute();
        mono.StartCoroutine(DoScale());

    }

    IEnumerator DoScale() 
    {
       yield return TweenHelper.MakeLerp(startValue*0.6f,endValue*1.2f, tranTine, (val)=>controlSelf.localScale=val);
       yield return TweenHelper.MakeLerp(controlSelf.localScale, endValue, tranTine, (val)=>controlSelf.localScale=val);
    }
}
