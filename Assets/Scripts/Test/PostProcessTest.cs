using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PostProcessManager.Instance.OpenVignetteIdle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
