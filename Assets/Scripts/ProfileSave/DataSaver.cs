using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSaver
{
    //<fileName,path>
    private Dictionary<string, string> pathDic = new Dictionary<string, string>();
    //�����Ա����л������ݽṹת��Ϊjson�ַ����ļ��洢�������ļ���persistentData��

    /// <summary>
    /// ��罫����Ҫ�洢������������һ���ࣨ��ṹ�壩
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    public static void SaveByJson(string fileName, object data)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        string json = JsonUtility.ToJson(data);

        try
        {
            if (json != null) File.WriteAllText(path, json);
#if UNITY_EDITOR
            Debug.Log($"�ɹ���·����{path}�����ļ�:{fileName}");
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    //���ض��ļ�����·���´洢���ļ���json�ַ��������л�Ϊ��Ӧ�����ݽṹ������
    public static T LoadFromJson<T>(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
#if UNITY_EDITOR
            Debug.Log($"�ɹ���ȡ�ļ�:{fileName}");
#endif
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //ɾ���Ѵ洢�Ķ�Ӧ���Ƶ�json�ļ�
    public static void DeleteSavedFile(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <summary>
    /// ɾ�����д浵�ļ�
    /// </summary>
    public void DeleteAllSaveFile()
    {

        foreach (var item in pathDic)
        {
            File.Delete(item.Value);
            Debug.Log("��ɾ�����д浵!");
        }
    }

}
/// <summary>
/// ��Ҫ��¼��Json�ļ���
/// </summary>
public class JsonFileName
{
    public static string Profile1 = "Profile1";
    public static string Profile2 = "Profile2";
    public static string Profile3 = "Profile3";
    //public static string Profile4 = "Profile4";
}
