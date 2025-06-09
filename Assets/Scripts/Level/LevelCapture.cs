using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCapture : MonoBehaviour
{
    public int currentLevel=1;

    public List<TV3DFloatTextContainer> tV3DFloatTextContainers;
    TV3DFloatTextContainer currentText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<LevelSelectScreen>()) 
        {
            if(currentText!=null)
            StartCoroutine(currentText.ChangeTextsVislble(false));
            currentLevel = other.GetComponent<LevelSelectScreen>().levelNumber;
            currentText = tV3DFloatTextContainers[currentLevel - 1];
            StartCoroutine(tV3DFloatTextContainers[currentLevel - 1].ChangeTextsVislble(true));
        }
    }
}
