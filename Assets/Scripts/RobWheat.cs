using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = System.Random;

public class RobWheat : MonoBehaviour
{
    private int chance = 5000, chanceDrought = 1;
    public Text txtmoney, txtWheat;
    public GameObject panelBadWheat;
    private SaveRobWheat save = new SaveRobWheat();
    private string path;

    private void Start()
    {
        UnityThread.initUnityThread();
        String buffPath = "Save_data_RobWheat.json";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            path = Path.Combine(Application.persistentDataPath, buffPath);
#else
        path = Path.Combine(Application.dataPath, buffPath);
#endif
        if (File.Exists(path))
        {
            save = JsonUtility.FromJson<SaveRobWheat>(File.ReadAllText(path));
            if (save.chance > 0)
                chance = save.chance;
        }

        Thread threadBird = new Thread(() =>
        {
            while (true)
            {
                Random rnd = new Random();
                int chislo = rnd.Next(1, chance);

                if (chislo == chanceDrought)
                {
                    chislo = rnd.Next(0, 64);
                    UnityThread.executeInUpdate(() =>
                    {
                        transform.GetChild(chislo).GetComponent<Renderer>().material.color =
                            new Color32(84, 53, 13, 255);
                        foreach (Transform eachChild in transform.GetChild(chislo))
                        {
                            if (eachChild.name == "P_AncientRuins_Plants20(Clone)")

                                Destroy(eachChild.gameObject);
                        }
                    });
                    Thread.Sleep(100);
                }

                Thread.Sleep(10);
                if (chance > 5000) chance--;
            }
        });

        Thread threadBadWheat = new Thread(() =>
        {
            while (true)
            {
                Random rnd = new Random();
                int chislo = rnd.Next(1, 65000);

                if (chislo == 2)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        txtWheat.text = "0";
                        panelBadWheat.SetActive(true);
                    });
                    Thread.Sleep(100);
                }

                Thread.Sleep(2);
            }
        });

        threadBird.Start();
        threadBadWheat.Start();
    }

    public void increaseChance()
    {
        int money = int.Parse(txtmoney.text);
        if (money > 150)
        {
            chance += 10000;
            money -= 100;
            txtmoney.text = money.ToString();
        }
    }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private void OnApplicationPause(bool pause)
        {
        save.chance = chance;
        File.WriteAllText(path, JsonUtility.ToJson(save));
        }
#endif
    public void OnApplicationQuit()
    {
        save.chance = chance;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
}

public class SaveRobWheat
{
    public int chance;
}