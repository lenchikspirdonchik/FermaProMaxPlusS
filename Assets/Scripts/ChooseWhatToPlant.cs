using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChooseWhatToPlant : MonoBehaviour
{
    public int choose = 0;
    public GameObject btnWheat, btnChicken, btnCow;
    public Text txtWheat, txtChicken, txtCow, txtMoney;
    public AudioSource audio;
    public Transform camera;
    private SaveData save = new SaveData();
    private string path;
    public bool chickenOpen, cowOpen;
    private Vector3 wheatSquarePosition, chickenSquarePosition, cowSquarePosition;
    private Quaternion wheatSquareRotation, chickenSquareRotation, cowSquareRotation;
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

        wheatSquarePosition = new Vector3(-4.37f, 6.76f, -8.15f);
        chickenSquarePosition = new Vector3(-5.05f, 5.18f, 8.45f);
        cowSquarePosition = new Vector3(-16.242f, 6.502f, 13.5f);
        chickenSquareRotation = Quaternion.Euler(42.39f, 60.59f, 0f);
        wheatSquareRotation = Quaternion.Euler(42.608f, 0.1f, 0f);
        cowSquareRotation = Quaternion.Euler(31.614f, -44.887f, 0f);
    }


    public void Exit(String gameLevel)
    {
        Application.Quit();
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
                money += 2 * product;
                break;
            case "txtChicken":
                money += 3 * product;
                break;
            case "txtCow":
                money += 4 * product;
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
                camera.position = wheatSquarePosition;
                camera.rotation = wheatSquareRotation;
                btnWheat.GetComponent<Graphic>().color = Color.red;
                break;
            case 1:
                if (chickenOpen)
                {
                    camera.position = chickenSquarePosition;
                    camera.rotation = chickenSquareRotation;
                    btnChicken.GetComponent<Graphic>().color = Color.red;
                }
                else
                    BuySquare(100, chickenSquarePosition, chickenSquareRotation);

                break;
            case 2:
                if (cowOpen)
                {
                    camera.position = cowSquarePosition;
                    camera.rotation = cowSquareRotation;
                    btnCow.GetComponent<Graphic>().color = Color.red;
                }
                else
                    BuySquare(500, cowSquarePosition, cowSquareRotation);

                break;
        }
    }

    private void BuySquare(int price, Vector3 position, Quaternion rotation)
    {
        if (EditorUtility.DisplayDialog("Домик заблокирован",
            "Нужно разблокировать домик (" + price + "$)", "Разблокировать", "Отмена"))
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

                camera.position = position;
                camera.rotation = rotation;
            }

            txtMoney.text = money.ToString();
        }
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