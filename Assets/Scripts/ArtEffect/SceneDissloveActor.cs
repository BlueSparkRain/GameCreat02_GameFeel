using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SceneDissloveActor : MonoBehaviour
{
    public bool canDisslove;

    public Transform center;

    public List<Material> dissloveMats;

    // Update is called once per frame
    void Update()
    {
        if(center!=null && dissloveMats.Count > 0) 
        {

            for (int i = 0; i < dissloveMats.Count; i++)
            {
                dissloveMats[i].SetVector("_DissloveCenter", center.position);
            
            }
        }

    }
}
