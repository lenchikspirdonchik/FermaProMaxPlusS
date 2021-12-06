using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class CreateSquare : MonoBehaviour
{
    public GameObject wheat, chicken, cow;
    public Text txtWheat, txtChicken, txtCow, txtMoney;
    public AudioSource audioChicken, audioCow;
    public int activeSquare;

    private int whatIsActive = 3, chickenEgg = 0, cowMilk = 0;
    private bool isReady;
    private Save save = new Save();
    private string path;
    private float lastClick = 0;
    private float waitTime = 1.0f;
    private float downTime;
    private bool isHandled = false;
    private int chanceDrought = 1, chanceCombine = 2, chanceBird = 5000, chanceBadGood = 3, chanceWolf = 3000;


    private void Start()
    {
        UnityThread.initUnityThread();
        String buffPath = "Save_" + transform.name + ".json";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            path = Path.Combine(Application.persistentDataPath, buffPath);
#else
        path = Path.Combine(Application.dataPath, buffPath);
#endif
        if (File.Exists(path))
        {
            save = JsonUtility.FromJson<Save>(File.ReadAllText(path));
            whatIsActive = save.whatIsActive;
            chickenEgg = save.chickenEgg;
            cowMilk = save.cowMilk;
            isReady = save.isReady;


            var position = transform.position;
            Vector3 vector3 = new Vector3(position.x, position.y + 0.5f, position.z);
            switch (whatIsActive)
            {
                case 0:
                    Instantiate(wheat, vector3, Quaternion.identity, transform);
                    if (isReady)
                        GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    else PlantWheat();
                    break;
                case 1:
                    Instantiate(chicken, vector3, Quaternion.identity, transform);
                    if (isReady)
                        GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    else PlantChicken();
                    break;
                case 2:
                    Instantiate(cow, vector3, Quaternion.identity, transform);
                    if (isReady)
                        GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    else PlantCow();
                    break;
            }

            foo();
        }
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            isReady = false;
            GetComponent<Renderer>().material.color = new Color32(84, 53, 13, 255);
            whatIsActive = 3;
        }
    }

    void OnMouseDown()
    {
        downTime = Time.time;
        isHandled = false;

        //if (Time.time - lastClick < 0.3) double clicked the target

        lastClick = Time.time;
        if (isReady) Get();
        else
        {
            switch (activeSquare)
            {
                case 0:
                    Add(wheat);
                    break;
                case 1:
                    Add(chicken);
                    break;
                case 2:
                    Add(cow);
                    break;
            }
        }
    }

    void OnMouseDrag()
    {
        if ((Time.time > downTime + waitTime) && !isHandled)
        {
            isHandled = true;
            Delete();
        }
    }

    private void Get()
    {
        GetComponent<Renderer>().material.color = new Color32(84, 53, 13, 255);
        if (transform.childCount != 0)
        {
            if (activeSquare == 0)
            {
                var counter = int.Parse(txtWheat.text);
                counter++;
                txtWheat.text = counter.ToString();
                PlantWheat();
            }

            if (activeSquare == 1)
            {
                var counter = int.Parse(txtChicken.text);
                counter++;
                txtChicken.text = counter.ToString();
                PlantChicken();
            }

            if (activeSquare == 2)
            {
                var counter = int.Parse(txtCow.text);
                counter++;
                txtCow.text = counter.ToString();
                PlantCow();
            }

            isReady = false;
        }
    }

    private void Delete()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GetComponent<Renderer>().material.color = new Color32(84, 53, 13, 255);
        isReady = false;
        whatIsActive = 3;
    }

    private void Add(GameObject obj)
    {
        int money = int.Parse(txtMoney.text);
        int wheatCount = int.Parse(txtWheat.text);
        Delete();

        var position = transform.position;
        Vector3 vector3 = new Vector3(position.x, position.y + 0.5f, position.z);
        if (activeSquare == 0 && money > 1)
        {
            money -= 2;
            Instantiate(obj, vector3, Quaternion.identity, transform);
            PlantWheat();
        }

        if (activeSquare == 1 && money > 2 && wheatCount > 2)
        {
            money -= 3;
            Instantiate(obj, vector3, Quaternion.identity, transform);
            audioChicken.Play();
            PlantChicken();
        }

        if (activeSquare == 2 && money > 3 && wheatCount > 3)
        {
            money -= 4;
            Instantiate(obj, vector3, Quaternion.identity, transform);
            audioCow.Play();
            PlantCow();
        }

        whatIsActive = activeSquare;
        txtMoney.text = money.ToString();
    }

    private void PlantCow()
    {
        var counter = int.Parse(txtWheat.text);
        Thread t = new Thread(() =>
        {
            while (activeSquare != 2 && counter < 3)
            {
                Thread.Sleep(1500);
                UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
            }

            UnityThread.executeInUpdate(() =>
            {
                counter -= 3;
                txtWheat.text = counter.ToString();
            });

            Thread.Sleep(40000);


            if (activeSquare == 2)
            {
                isReady = true;
                cowMilk++;
                UnityThread.executeInUpdate(() =>
                {
                    GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    foo();
                });
            }


            UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
        });
        t.Start();
    }

    private void PlantChicken()
    {
        var counter = int.Parse(txtWheat.text);
        Thread t = new Thread(() =>
        {
            while (activeSquare != 1 && counter < 2)
            {
                Thread.Sleep(1500);
                UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
            }

            UnityThread.executeInUpdate(() =>
            {
                if (chickenEgg % 3 == 0)
                {
                    counter--;
                    txtWheat.text = counter.ToString();
                    chickenEgg = 0;
                }
            });

            Thread.Sleep(30000);


            if (activeSquare == 1)
            {
                isReady = true;
                chickenEgg++;
                UnityThread.executeInUpdate(() =>
                {
                    GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    foo();
                });
            }


            UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
        });
        t.Start();
    }

    private void PlantWheat()
    {
        Thread t = new Thread(() =>
        {
            Thread.Sleep(20000);
            UnityThread.executeInUpdate(() =>
            {
                if (activeSquare == 0)
                {
                    isReady = true;

                    GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                    foo();
                }
            });
        });
        t.Start();
    }


    private void foo()
    {
        Thread timeAfterReady = new Thread(() =>
        {
            int time = 0;
            while (time < 90)
            {
                time++;
                Thread.Sleep(1001);
                if (activeSquare == 3 || !isReady)
                    break;
            }

            UnityThread.executeInUpdate(() =>
            {
                if (activeSquare != 3 && isReady)
                {
                    Debug.Log("delete foo: transform = " + transform.name);
                    GetComponent<Renderer>().material.color = new Color32(240, 10, 10, 255);
                    isReady = false;
                }
            });
        });
        timeAfterReady.Start();
    }


#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private void OnApplicationPause(bool pause)
        {
            save.whatIsActive = whatIsActive;
            save.chickenEgg = chickenEgg;
            save.cowMilk = cowMilk;
            save.isReady = isReady;
             File.WriteAllText(path, JsonUtility.ToJson(save));
        }
#endif
    public void OnApplicationQuit()
    {
        save.whatIsActive = whatIsActive;
        save.chickenEgg = chickenEgg;
        save.cowMilk = cowMilk;
        save.isReady = isReady;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
}

[Serializable]
public class Save
{
    public int whatIsActive = 0, chickenEgg, cowMilk;
    public bool isReady;
}