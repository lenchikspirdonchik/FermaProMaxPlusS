using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobChicken : MonoBehaviour
{
    private GetData save = new GetData();
    private string path;
    public bool chickenOpen, cowOpen;

    void Start()
    {
        UnityThread.initUnityThread();
        String buffPath = "Save_data.json";
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