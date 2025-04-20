using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSaver
{
    //<fileName,path>
    private Dictionary<string, string> pathDic = new Dictionary<string, string>();
    //将可以被序列化的数据结构转换为json字符串文件存储于特殊文件夹persistentData内

    /// <summary>
    /// 外界将所需要存储的数据整合整一个类（或结构体）
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
            Debug.Log($"成功于路径：{path}储存文件:{fileName}");
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    //将特定文件名称路径下存储的文件的json字符串反序列化为对应的数据结构并返回
    public static T LoadFromJson<T>(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
#if UNITY_EDITOR
            Debug.Log($"成功读取文件:{fileName}");
#endif
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //删除已存储的对应名称的json文件
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
    /// 删除所有存档文件
    /// </summary>
    public void DeleteAllSaveFile()
    {

        foreach (var item in pathDic)
        {
            File.Delete(item.Value);
            Debug.Log("已删除所有存档!");
        }
    }

}
/// <summary>
/// 需要记录的Json文件名
/// </summary>
public class JsonFileName
{
    public static string Profile1 = "Profile1";
    public static string Profile2 = "Profile2";
    public static string Profile3 = "Profile3";
    //public static string Profile4 = "Profile4";
}
