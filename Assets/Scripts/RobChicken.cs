using System;
using System.IO;
using UnityEngine;

public class RobChicken : MonoBehaviour
{
    public bool chickenOpen, cowOpen;

    private string path;

    // under construction. you don't see anything here
    private GetData save = new GetData();

    private void Start()
    {
        UnityThread.initUnityThread();
        var buffPath = "Save_data_RobChicken.json";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, buffPath);
#else
        path = Path.Combine(Application.dataPath, buffPath);
#endif
        if (File.Exists(path))
        {
            save = JsonUtility.FromJson<GetData>(File.ReadAllText(path));
            chickenOpen = save.chickenOpen;
            cowOpen = save.cowOpen;
        }
    }
}

[Serializable]
public class GetData
{
    public bool chickenOpen, cowOpen;
}