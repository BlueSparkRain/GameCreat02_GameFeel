using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class PitchTest : MonoSingleton<PitchTest>
{

    public float cPitch=1;

    float timer;
    public  float interval = 0.5f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cPitch += 0.1f;
            MusicManager.Instance.PlaySound("Broke",cPitch);
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = interval;
            if(cPitch>1)
            cPitch -= 0.1f;
        }
    }

    public void DingDong() 
    {
        cPitch += 0.1f;
        MusicManager.Instance.PlaySound("Broke", cPitch);
    }

}
