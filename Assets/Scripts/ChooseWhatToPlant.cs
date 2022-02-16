using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWhatToPlant : MonoBehaviour
{
    [Header("Game objects")] [Tooltip("Help panels")]
    public GameObject panel1, panel2, panel3, panel4, panel5, panelRob, panel6, panel7;

    [Tooltip("is chicken or cow squares are open")]
    public bool chickenOpen, cowOpen;


    [Header("UI objects")] [Tooltip("Button to choose active square")]
    public GameObject btnWheat, btnChicken, btnCow;

    [Tooltip("Player bank texts")] public Text txtWheat, txtChicken, txtCow, txtMoney;
    [Tooltip("What button was clicked")] public int choose;
    [Tooltip("In game audio")] public AudioSource audio;
    [Tooltip("Main camera")] public Transform camera;


    private SaveData save = new SaveData();
    private string path;
    private Vector3 wheatSquarePosition, chickenSquarePosition, cowSquarePosition, realPosition;
    private Quaternion wheatSquareRotation, chickenSquareRotation, cowSquareRotation, realRotation;
    private float downTime;
    private bool isHandled;
    private float lastClick;

    private void Start()
    {
        //define camera position
        wheatSquarePosition = new Vector3(-4.37f, 6.76f, -8.15f);
        chickenSquarePosition = new Vector3(-5.05f, 5.18f, 8.45f);
        cowSquarePosition = new Vector3(-16.242f, 6.502f, 13.5f);
        chickenSquareRotation = Quaternion.Euler(42.39f, 60.59f, 0f);
        wheatSquareRotation = Quaternion.Euler(42.608f, 0.1f, 0f);
        cowSquareRotation = Quaternion.Euler(31.614f, -44.887f, 0f);
        realPosition = wheatSquarePosition;
        realRotation = wheatSquareRotation;

        btnWheat.GetComponent<Graphic>().color = Color.red;

        var buffPath = "Save_data.json";
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
            // if game was started for the first time - show help panels
            panel1.SetActive(true);
        }
    }

    //update camera position
    private void FixedUpdate()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, realPosition, Time.deltaTime * 5);
        camera.transform.rotation =
            Quaternion.RotateTowards(camera.transform.rotation, realRotation, 140 * Time.deltaTime);
    }

    public void Exit(string gameLevel)
    {
        Application.Quit();
    }

    //close all help panels
    public void closeHelp()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        panel5.SetActive(false);
        panelRob.SetActive(false);
        panel6.SetActive(false);
        panel7.SetActive(false);
    }

    public void nextHelp(int currentPanel)
    {
        GameObject[] objects = { panel1, panel2, panel3, panel6 };
        objects[currentPanel].SetActive(false);
        objects[++currentPanel].SetActive(true);
    }

    //sell ready product and get money
    public void Sell(Text txt)
    {
        var product = int.Parse(txt.text);
        if (product > 0)
        {
            downTime = Time.time;
            isHandled = false;
            audio.Play();

            if (Time.time - lastClick < 0.3)
            {
                var money = sellAll(product, txt, true);
                txtMoney.text = money.ToString();
            }
            else
            {
                var money = sellAll(product, txt, false);
                txtMoney.text = money.ToString();
                product--;
                txt.text = product.ToString();
            }

            lastClick = Time.time;
        }
    }

    private int sellAll(int product, Text txt, bool issellAll)
    {
        var money = int.Parse(txtMoney.text);
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

// control camera position value
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
                {
                    BuySquare(100, chickenSquarePosition, chickenSquareRotation);
                }

                break;
            case 2:
                if (cowOpen)
                {
                    realPosition = cowSquarePosition;
                    realRotation = cowSquareRotation;
                    btnCow.GetComponent<Graphic>().color = Color.red;
                }
                else
                {
                    BuySquare(500, cowSquarePosition, cowSquareRotation);
                }

                break;
        }
    }

// buy new square (cow || chicken)
    private void BuySquare(int price, Vector3 position, Quaternion rotation)
    {
        audio.Play();
        var money = int.Parse(txtMoney.text);
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
            panel7.SetActive(true);
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
    public string textWheat, textChicken, textCow, textMoney;
    public bool chickenOpen, cowOpen;
}