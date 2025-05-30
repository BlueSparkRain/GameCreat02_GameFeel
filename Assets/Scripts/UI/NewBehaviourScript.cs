using UnityEngine;

public class RuntimeLogger : MonoBehaviour
{
    string logText = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logText += logString + "\n";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 800, 800), logText);
    }
}