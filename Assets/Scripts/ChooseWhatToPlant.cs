using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChooseWhatToPlant : MonoBehaviour
{
    public int choose = 0;
    public GameObject btnWheat, btnChicken, btnCow, btnDelete, btnGet;


    private void Start()
    {
        btnWheat.GetComponent<Graphic>().color = Color.red;
    }

    public void Input(int input)
    {
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
}