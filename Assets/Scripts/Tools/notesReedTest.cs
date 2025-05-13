using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notesReedTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        notes level1 = new notes("level1note(Test).txt");

        //for(int i = 0; i < level1.MusicNotes.Length; i++)
        //    Debug.LogError(level1.MusicNotes[i]);
    }
}
