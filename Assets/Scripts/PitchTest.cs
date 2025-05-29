using System.Collections;
using UnityEngine;
public class PitchTest : MonoBehaviour
{
    public float cPitch = 1;
    float timer;
    public float interval = 0.5f;

    private void Start()
    {
        GamepadShakeManager.Instance.IMInit();
        //StartCoroutine(StartComboMusic());
    }

    IEnumerator StartComboMusic() 
    {
      yield return new WaitForSeconds(0.3f);
        //MusicManager.Instance.PlayBKMusic("Bob");
    }

    private void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = interval;
            if (cPitch > 1)
                cPitch -= 0.1f;
        }
    }

    public void DingDong()
    {
        cPitch += 0.1f;
        //MusicManager.Instance.PlaySound("remove", cPitch);
        MusicManager.Instance.PlaySound("remove");
    }

}
