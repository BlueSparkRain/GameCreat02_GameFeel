using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Techer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() 
    {
        yield return new WaitForSeconds(1.5f);
        //UIManager.Instance.ShowPanel<TechPanel>(null);

    }

}
