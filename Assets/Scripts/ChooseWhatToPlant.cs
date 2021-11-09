using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChooseWhatToPlant : MonoBehaviour
{
    [Header("Game objects")] [Tooltip("Help panels")]
    public GameObject panel1, panel2, panel3, panel4, panelRob;

    [Tooltip("is chicken or cow squares are open")]
    public bool chickenOpen, cowOpen;


    [Header("UI objects")] [Tooltip("Button to choose active square")]
    public GameObject btnWheat, btnChicken, btnCow;

    [Tooltip("Player bank texts")] public Text txtWheat, txtChicken, txtCow, txtMoney;
    [Tooltip("What button was clicked")] public int choose = 0;
    [Tooltip("In game audio")] public AudioSource audio;
    [Tooltip("Main camera")] public Transform camera;


    private SaveData save = new SaveData();
    private string path;
    private Vector3 wheatSquarePosition, chickenSquarePosition, cowSquarePosition, realPosition;
    private Quaternion wheatSquareRotation, chickenSquareRotation, cowSquareRotation, realRotation;
    private float downTime;
    private bool isHandled = false;
    private float lastClick = 0;

    private void Start()
    {
        btnWheat.GetComponent<Graphic>().color = Color.red;

        String buffPath = "Save_data.json";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, buffPath);
#else
        path = Path.Combine(Application.dataPath, buffPath);
#endif
        if (File.Exists(path))
        {
            save = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            txtChicken.text = save.textChicken;
            txtCow.text = save.textCow;
            txtWheat.text = save.textWheat;
            txtMoney.text = save.textMoney;
            chickenOpen = save.chickenOpen;
            cowOpen = save.cowOpen;
        }
        else
        {
            panel1.SetActive(true);
            panel2.SetActive(true);
            panel3.SetActive(true);
        }

        wheatSquarePosition = new Vector3(-4.37f, 6.76f, -8.15f);
        chickenSquarePosition = new Vector3(-5.05f, 5.18f, 8.45f);
        cowSquarePosition = new Vector3(-16.242f, 6.502f, 13.5f);
        chickenSquareRotation = Quaternion.Euler(42.39f, 60.59f, 0f);
        wheatSquareRotation = Quaternion.Euler(42.608f, 0.1f, 0f);
        cowSquareRotation = Quaternion.Euler(31.614f, -44.887f, 0f);
        realPosition = wheatSquarePosition;
        realRotation = wheatSquareRotation;
        
    }

    private void FixedUpdate()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, realPosition, Time.deltaTime * 5);
        camera.transform.rotation =
            Quaternion.RotateTowards(camera.transform.rotation, realRotation, 140 * Time.deltaTime);
        
    }

    public void Exit(String gameLevel)
    {
        Application.Quit();
    }

    public void closeHelp()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        panelRob.SetActive(false);
    }

    public void Sell(Text txt)
    {
        int product = int.Parse(txt.text);
        if (product > 0)
        {
            downTime = Time.time;
            isHandled = false;
            audio.Play();

            if (Time.time - lastClick < 0.3)
            {
                int money = sellAll(product, txt, true);
                txtMoney.text = money.ToString();
            }
            else
            {
                int money = sellAll(product, txt, false);
                txtMoney.text = money.ToString();
                product--;
                txt.text = product.ToString();
            }

            lastClick = Time.time;
        }
    }

    private int sellAll(int product, Text txt, bool issellAll)
    {
        int money = int.Parse(txtMoney.text);
        if (issellAll)
            txt.text = "0";
        else
            product = 1;

        switch (txt.name)
        {
            case "txtWheat":
                money += 1 * product;
                break;
            case "txtChicken":
                money += 2 * product;
                break;
            case "txtCow":
                money += 3 * product;
                break;
        }

        return money;
    }


    public void Input(int input)
    {
        audio.Play();
        choose = input;
        btnWheat.GetComponent<Graphic>().color = Color.white;
        btnChicken.GetComponent<Graphic>().color = Color.white;
        btnCow.GetComponent<Graphic>().color = Color.white;
        switch (choose)
        {
            case 0:
                realPosition = wheatSquarePosition;
                realRotation = wheatSquareRotation;
                btnWheat.GetComponent<Graphic>().color = Color.red;
                break;
            case 1:
                if (chickenOpen)
                {
                    realPosition = chickenSquarePosition;
                    realRotation = chickenSquareRotation;
                    btnChicken.GetComponent<Graphic>().color = Color.red;
                }
                else
                    BuySquare(100, chickenSquarePosition, chickenSquareRotation);

                break;
            case 2:
                if (cowOpen)
                {
                    realPosition = cowSquarePosition;
                    realRotation = cowSquareRotation;
                    btnCow.GetComponent<Graphic>().color = Color.red;
                }
                else
                    BuySquare(500, cowSquarePosition, cowSquareRotation);

                break;
        }
    }

    private void service()
    {
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(45000);
                UnityThread.executeInUpdate(() =>
                {
                    var money = int.Parse(txtMoney.text);

                    if (money > 0)
                    {
                        money = (int)(money * 0.87);
                    }
                    else if (money == 0)
                    {
                        money = -13;
                    }
                    else
                    {
                        money = (int)(money / 0.87);
                    }

                    txtMoney.text = money.ToString();
                });
            }
        });
        thread.Start();
    }

    private void robSquare()
    {
        Random rnd = new Random();
        Thread t = new Thread(() =>
        {
            int chislo = rnd.Next(0, 25000);
            if (chislo == 19)
            {
                UnityThread.executeInUpdate(() =>
                {
                    panelRob.SetActive(true);
                    chislo = rnd.Next(0, 3);
                    switch (chislo)
                    {
                        case 0:
                            txtWheat.text = "0";
                            break;
                        case 1:
                            txtChicken.text = "0";
                            break;
                        case 2:
                            txtCow.text = "0";
                            break;
                        case 3:
                            txtMoney.text = "0";
                            break;
                    }
                });
            }
        });
        t.Start();
    }

    private void BuySquare(int price, Vector3 position, Quaternion rotation)
    {
        audio.Play();
        int money = int.Parse(txtMoney.text);
        if (money > price)
        {
            money -= price;
            switch (price)
            {
                case 100:
                    chickenOpen = true;
                    btnChicken.GetComponent<Graphic>().color = Color.red;
                    break;
                case 500:
                    cowOpen = true;
                    btnCow.GetComponent<Graphic>().color = Color.red;
                    break;
            }

            realPosition = position;
            realRotation = rotation;
        }
        else
        {
            panel4.SetActive(true);
        }

        txtMoney.text = money.ToString();
    }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        save.textChicken = txtChicken.text;
        save.textCow = txtCow.text;
        save.textWheat = txtWheat.text;
        save.textMoney = txtMoney.text;
        save.chickenOpen = chickenOpen;
        save.cowOpen = cowOpen;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
#endif
    private void OnApplicationQuit()
    {
        save.textChicken = txtChicken.text;
        save.textCow = txtCow.text;
        save.textWheat = txtWheat.text;
        save.textMoney = txtMoney.text;
        save.chickenOpen = chickenOpen;
        save.cowOpen = cowOpen;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
}

[Serializable]
public class SaveData
{
    public String textWheat, textChicken, textCow, textMoney;
    public bool chickenOpen, cowOpen;
}