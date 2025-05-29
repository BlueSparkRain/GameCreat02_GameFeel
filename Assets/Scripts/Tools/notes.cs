using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class notes
{
    public List<float> MusicNotes=new List<float>();
    public notes(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath,fileName+".txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            //MusicNotes = new float[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                if (float.TryParse(lines[i], out float noteSampleValue))
                {
                    //MusicNotes[i] = note;
                    MusicNotes.Add(noteSampleValue);
                }
                else
                {
                    Debug.LogError($"无法解析第 {i + 1} 行内容: {lines[i]}");
                }
            }
        }
        else
        {
            Debug.LogError($"文件{fileName}不存在。");
        }
    }

}
