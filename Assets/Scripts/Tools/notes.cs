using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class notes
{
    public float[] MusicNotes;
    public notes(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath,fileName);
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            MusicNotes = new float[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                if (float.TryParse(lines[i], out float note))
                {
                    MusicNotes[i] = note;
                }
                else
                {
                    Debug.LogError($"�޷������� {i + 1} ������: {lines[i]}");
                }
            }
        }
        else
        {
            Debug.LogError($"�ļ�{fileName}�����ڡ�");
        }
    }

}
