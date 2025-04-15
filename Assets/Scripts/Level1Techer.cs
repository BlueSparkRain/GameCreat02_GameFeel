using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Techer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() 
    {
        yield return new WaitForSeconds(1.5f);
        //UIManager.Instance.ShowPanel<TechPanel>(null);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
