using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioVisualization1 : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject[] images;
    private float[] samples=new float [512];
    private AudioSource audioSource;
    public float changeSpeed = 0.05f;
    public float reMapParam = 0.15f;
    void Start()
    {
        audioSource=GameObject.Find("BKMusic").GetComponent<AudioSource>();
        images = new GameObject[6];
       
        for (int i = 0; i < transform.childCount; i++)
        {
            images[i] = transform.GetChild(i).gameObject;
        }
        InvokeRepeating("ScaleLength", 0, changeSpeed);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void ScaleLength()
    {
        if (audioSource)
        {
            audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
            for (int i = 0; i < images.Length; i++)
            {
                float hight = Mathf.Max(Mathf.Min(2 * (float)ExponentialMapping(samples[Mathf.Min((int)(512 / 2 / images.Length) * i, 511)], reMapParam), 1) - 0.1f, 0);
                //images[i].transform.localScale = new Vector3(1, 1 + hight, 1);
                images[i].GetComponent<RectTransform>().localScale = new Vector3(1, hight, 1);
            }

        }
        
    }
    public static double ExponentialMapping(float x, float a)
    {
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
}
