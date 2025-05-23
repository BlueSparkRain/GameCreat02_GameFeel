using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RayChecker
{
    //static  float targetChecckDistance = 1.4f;

    public static GameObject CheckTargetLayerObj(LayerMask layer,float targetChecckDistance, Vector3 checkOriginPos, E_CustomDir targetDir) 
    {

        switch (targetDir)
        {
            case E_CustomDir.ио:
                return (Physics2D.Raycast(checkOriginPos, Vector2.up, targetChecckDistance, layer).collider?.gameObject);
                
            case E_CustomDir.об:
                return (Physics2D.Raycast(checkOriginPos, Vector2.down, targetChecckDistance, layer).collider?.gameObject);
            case E_CustomDir.вС:
                return (Physics2D.Raycast(checkOriginPos, Vector2.left, targetChecckDistance, layer).collider?.gameObject);
            case E_CustomDir.ср:
                return (Physics2D.Raycast(checkOriginPos, Vector2.right, targetChecckDistance, layer).collider?.gameObject);
            default:
                return null;
        }
    }
}
