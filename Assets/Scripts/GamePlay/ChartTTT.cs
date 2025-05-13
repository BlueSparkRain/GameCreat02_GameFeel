using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChartTTT : MonoBehaviour
{
   public   List<float> floatLists = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());


    }

    IEnumerator Wait() 
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(ChartCheckManager.Instance.SetUpNewMusic(floatLists));
        yield return new WaitForSeconds(10);
        StartCoroutine(ChartCheckManager.Instance.SetUpNewMusic(floatLists));        

    }

    // Update is called once per frame
    void Update()
    {
    }
}
