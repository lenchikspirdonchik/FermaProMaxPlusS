using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChooseWhatToPlant : MonoBehaviour
{
    public int choose = 0;
    public GameObject btnWheat, btnChicken, btnCow, btnDelete, btnGet;
    public Text txtWheat, txtChicken, txtCow, txtMoney;
    public AudioSource audio;
    private SaveData save = new SaveData();
    private string path;

    private void Start()
    {
        btnWheat.GetComponent<Graphic>().color = Color.red;

        String buffPath = "Save_data.json";
#if UNITY_ANDROID && !UNITY_EDITOR
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
        }
    }


    public void Exit(String gameLevel)
    { 
        Application.Quit();
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevel);
    }

    public void Sell(Text txt)
    {
        audio.Play();
        int product = int.Parse(txt.text);
        if (product > 0)
        {
            product--;
            txt.text = product.ToString();

            int money = int.Parse(txtMoney.text);

            switch (txt.name)
            {
                case "txtWheat":
                    money += 2;
                    break;
                case "txtChicken":
                    money += 3;
                    break;
                case "txtCow":
                    money += 4;
                    break;
            }

            txtMoney.text = money.ToString();
        }
    }

    public void Input(int input)
    {
        audio.Play();
        choose = input;
        btnWheat.GetComponent<Graphic>().color = Color.white;
        btnChicken.GetComponent<Graphic>().color = Color.white;
        btnCow.GetComponent<Graphic>().color = Color.white;
        btnDelete.GetComponent<Graphic>().color = Color.white;
        btnGet.GetComponent<Graphic>().color = Color.white;
        switch (choose)
        {
            case 0:
                btnWheat.GetComponent<Graphic>().color = Color.red;
                break;
            case 1:
                btnChicken.GetComponent<Graphic>().color = Color.red;
                break;
            case 2:
                btnCow.GetComponent<Graphic>().color = Color.red;
                break;
            case 3:
                btnDelete.GetComponent<Graphic>().color = Color.red;
                break;
            case 4:
                btnGet.GetComponent<Graphic>().color = Color.red;
                break;
            default:
                btnWheat.GetComponent<Graphic>().color = Color.red;
                break;
        }
    }
    
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        save.textChicken = txtChicken.text;
        save.textCow = txtCow.text;
        save.textWheat = txtWheat.text;
        save.textMoney = txtMoney.text;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
#endif
    private void OnApplicationQuit()
    {
        save.textChicken = txtChicken.text;
        save.textCow = txtCow.text;
        save.textWheat = txtWheat.text;
        save.textMoney = txtMoney.text;
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
   
}

[Serializable]
public class SaveData
{
    public String textWheat, textChicken, textCow, textMoney;
}