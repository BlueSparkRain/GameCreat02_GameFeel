using UnityEditor;
public class ProfileEditor : Editor
{
    [MenuItem("MyTools/�����հ״浵json�ļ�")]
    static void CreatEmptyJsonProfile() 
    {
        DataSaver.SaveByJson(JsonFileName.Profile1,new ProfileSaveData(JsonFileName.Profile1));
        DataSaver.SaveByJson(JsonFileName.Profile2,new ProfileSaveData(JsonFileName.Profile2));
        DataSaver.SaveByJson(JsonFileName.Profile3,new ProfileSaveData(JsonFileName.Profile3));
    }
}
