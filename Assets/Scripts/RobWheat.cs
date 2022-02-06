using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RobWheat : MonoBehaviour
{
    private int chance = 5000;
    public Text txtmoney, txtWheat;
    public GameObject panelBadWheat, panelDrought, birdImg;
    private SaveRobWheat save = new SaveRobWheat();
    private string path;

    private void Start()
    {
        UnityThread.initUnityThread();
        var buffPath = "Save_data_RobWheat.json";
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

        //three threads - three different ways to steal wheat from the user
        var threadBird = new Thread(() =>
        {
            while (true)
            {
                var rnd = new Random();
                var chislo = rnd.Next(1, chance);

                if (chislo == 1)
                {
                    chislo = rnd.Next(0, 64);
                    UnityThread.executeInUpdate(() =>
                    {
                        transform.GetChild(chislo).GetComponent<Renderer>().material.color =
                            new Color32(84, 53, 13, 255);
                        foreach (Transform eachChild in transform.GetChild(chislo))
                            if (eachChild.name == "P_AncientRuins_Plants20(Clone)")

                                Destroy(eachChild.gameObject);

                        birdImg.SetActive(true);
                    });
                    Thread.Sleep(2000);
                    UnityThread.executeInUpdate(() => { birdImg.SetActive(false); });
                }

                Thread.Sleep(10);
                if (chance > 10000) chance--;
            }
        });

        var threadBadWheat = new Thread(() =>
        {
            while (true)
            {
                var rnd = new Random();
                var chislo = rnd.Next(1, 95000);

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

        var threadDrought = new Thread(() =>
        {
            while (true)
            {
                var rnd = new Random();
                var chislo = rnd.Next(1, 75000);

                if (chislo == 3)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        for (var i = 0; i < 64; i += 3)
                        {
                            transform.GetChild(chislo).GetComponent<Renderer>().material.color =
                                new Color32(84, 53, 13, 255);
                            foreach (Transform eachChild in transform.GetChild(i))
                            {
                                if (eachChild.name == "P_AncientRuins_Plants20(Clone)")
                                    Destroy(eachChild.gameObject);
                                panelDrought.SetActive(true);
                            }
                        }
                    });


                    Thread.Sleep(100);
                }

                Thread.Sleep(10);
                if (chance > 5000) chance--;
            }
        });

        threadBird.Start();
        threadBadWheat.Start();
        threadDrought.Start();
    }

// make the user pay us
    public void increaseChance()
    {
        var money = int.Parse(txtmoney.text);
        if (money > 110)
        {
            chance += 30000;
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