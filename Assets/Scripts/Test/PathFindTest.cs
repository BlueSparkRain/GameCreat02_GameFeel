using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           PathFindManager. Instance.GetPathFindCommands(new Vector2Int(1, 1), new Vector2Int(3, 5));


        }
    }
}
