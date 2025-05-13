using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class S_ParticalExplodeCommand : SquareCommand
{
    E_ParticalType particalType;
    WholeObjPoolManager wholeObjPoolManager;
    ParticleSystem particle;
    GameObject particalObj;

    public S_ParticalExplodeCommand(Square square) : base(square)
    {
        wholeObjPoolManager = WholeObjPoolManager.Instance;

    }
    public void GetParticalType(E_ParticalType particalType) 
    {
    this.particalType = particalType;
    }

    public override void Excute()
    {
       base.Excute();
       particalObj = wholeObjPoolManager.GetTargetPartical(particalType);
       particle= particalObj.GetComponent<ParticleSystem>();
       particle.transform.position=controlSquare.transform.position;
       particle.textureSheetAnimation.SetSprite(0, controlSquare.GetComponent<SpriteRenderer>().sprite);
       particle.Play();
       mono.StartCoroutine(WaitToReturnPool());
    }

    WaitForSeconds returnDelay=new WaitForSeconds(2);
    IEnumerator WaitToReturnPool() 
    {
        yield return  returnDelay;
        wholeObjPoolManager.ReturnPool(E_ObjectPoolType.色块消除爆炸池,particalObj);
    
    }
}

public enum E_ParticalType
{
    玩家卡点完美,
    玩家卡点优秀,
    玩家卡点好,
    色块消除爆炸,

}