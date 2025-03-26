using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float len =50;
    public int imageCount = 15;
    public GameObject[] images;
    private float[] samples=new float [512];
    private AudioSource audioSource;
    public float changeSpeed = 0.05f;
    public GameObject imagePrefab;
    public float reMapParam = 0.15f;
    void Start()
    {
        //return;

        audioSource = GameObject.Find("BKMusic").GetComponent<AudioSource>();
        images = new GameObject[imageCount];
        for (int i = 0; i < imageCount; i++)
        {
            images[i] = Instantiate(imagePrefab, transform, false);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            images[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < images.Length; i++)
        {
            
            float x, y, z;
            x = images[i].GetComponent<RectTransform>().anchoredPosition.x;
            y = images[i].GetComponent<RectTransform>().anchoredPosition.y;
            images[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x-len*Mathf.Sin((i*360/images.Length)*Mathf.PI/180), y+len*Mathf.Cos((i*360/images.Length)*Mathf.PI/180));
            images[i].transform.rotation=Quaternion.Euler(0,0,i*360/images.Length);
        }
        InvokeRepeating("ScaleLength", 0, changeSpeed);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void ScaleLength()
    {
        if(audioSource)
        {
            audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
            for (int i = 0; i < images.Length; i++)
            {
                float hight = Mathf.Max(Mathf.Min(2 * (float)ExponentialMapping(samples[Mathf.Min((int)(512 / 2 / images.Length) * i, 511)], reMapParam), 1) - 0.1f, 0);
                //images[i].transform.localScale = new Vector3(1, 1 + hight, 1);
                images[i].GetComponent<Image>().fillAmount = hight;
            }
        }
        
    }
    public static double ExponentialMapping(float x, float a)
    {
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
}
