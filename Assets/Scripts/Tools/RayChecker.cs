using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class RayChecker
{
    //static  float targetChecckDistance = 1.4f;
    
   static List<RaycastHit2D> hits = new List<RaycastHit2D>();

    public static GameObject CheckTargetLayerObj(LayerMask layer,float targetChecckDistance, Vector3 checkOriginPos, E_CustomDir targetDir) 
    {
        hits.Clear();
        switch (targetDir)
        {
            case E_CustomDir.上:
                hits =  Physics2D.RaycastAll(checkOriginPos, Vector2.up, targetChecckDistance, layer).ToList();
                if(hits.Count>0)
                return hits.Last().collider.gameObject;
                else return null;
            case E_CustomDir.下:
                        hits = Physics2D.RaycastAll(checkOriginPos, Vector2.down, targetChecckDistance, layer).ToList();
                if (hits.Count > 0)
                    return hits.Last().collider.gameObject;
                else return null;
           
            case E_CustomDir.左:
                        hits = Physics2D.RaycastAll(checkOriginPos, Vector2.left, targetChecckDistance, layer).ToList();
                if (hits.Count > 0)
                    return hits.Last().collider.gameObject;
                else return null;
         
            case E_CustomDir.右:
                        hits = Physics2D.RaycastAll(checkOriginPos, Vector2.right, targetChecckDistance, layer).ToList();
                if (hits.Count > 0)
                    return hits.Last().collider.gameObject;
                else return null;
            default:
                        return null;
                    }
    }
}
