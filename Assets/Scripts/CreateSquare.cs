using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CreateSquare : MonoBehaviour
{
    public GameObject wheat;
    public GameObject chicken;
    public GameObject cow;
    public ChooseWhatToPlant choose;
    public Text txtWheat, txtChicken, txtCow, txtMoney;
    private int whatIsActive = 3, chickenEgg = 0, cowMilk = 0;
    private bool isReady = false;

    void Awake()
    {
        UnityThread.initUnityThread();
    }

    private void OnMouseDown()
    {
        switch (choose.choose)
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
            case 3:
                Delete();
                break;
            case 4:
                Get();
                break;
            default:
                Add(wheat);
                break;
        }
    }

    private void Get()
    {
        if (isReady)
        {
            GetComponent<Renderer>().material.color = new Color32(84, 53, 13, 255);

            if (whatIsActive == 0)
            {
                var counter = int.Parse(txtWheat.text);
                counter++;
                txtWheat.text = counter.ToString();
                PlantWheat();
            }

            if (whatIsActive == 1)
            {
                var counter = int.Parse(txtChicken.text);
                counter += chickenEgg;
                chickenEgg = 0;
                txtChicken.text = counter.ToString();
                PlantChicken();
            }

            if (whatIsActive == 2)
            {
                var counter = int.Parse(txtCow.text);
                counter += cowMilk;
                cowMilk = 0;
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
        if (transform.childCount == 0)
        {
            var position = transform.position;
            Vector3 vector3 = new Vector3(position.x, position.y + 0.5f, position.z);
            if (choose.choose == 0 && money > 0)
            {
                money--;
                Instantiate(obj, vector3, Quaternion.identity, transform);
                whatIsActive = choose.choose;
                PlantWheat();
            }

            if (choose.choose == 1 && money > 1)
            {
                money -= 2;
                Instantiate(obj, vector3, Quaternion.identity, transform);
                whatIsActive = choose.choose;
                PlantChicken();
            }

            if (choose.choose == 2 && money > 2)
            {
                money -= 3;
                Instantiate(obj, vector3, Quaternion.identity, transform);
                whatIsActive = choose.choose;
                PlantCow();
            }
            txtMoney.text = money.ToString();
        }
    }

    private void PlantCow()
    {
        var counter = int.Parse(txtWheat.text);
        Thread t = new Thread(() =>
        {
            while (whatIsActive == 2)
            {
                if (counter > 1)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        counter--;
                        txtWheat.text = counter.ToString();
                    });

                    Thread.Sleep(20000);
                    if (whatIsActive == 2)
                    {
                        isReady = true;
                        cowMilk++;
                        UnityThread.executeInUpdate(() =>
                        {
                            GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                        });
                    }
                }

                UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
            }
        });
        t.Start();
    }

    private void PlantChicken()
    {
        var counter = int.Parse(txtWheat.text);
        Thread t = new Thread(() =>
        {
            while (whatIsActive == 1)
            {
                if (counter > 1)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        if (chickenEgg % 3 == 0)
                        {
                            //Debug.Log(chickenEgg +" % 3 = "+chickenEgg % 3);
                            counter--;
                            txtWheat.text = counter.ToString();
                        }
                    });

                    Thread.Sleep(10000);
                    if (whatIsActive == 1)
                    {
                        isReady = true;
                        chickenEgg++;
                        UnityThread.executeInUpdate(() =>
                        {
                            GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                        });
                    }
                }

                UnityThread.executeInUpdate(() => { counter = int.Parse(txtWheat.text); });
            }
        });
        t.Start();
    }

    private void PlantWheat()
    {
        Thread t = new Thread(() =>
        {
            Thread.Sleep(10000);

            if (whatIsActive == 0)
            {
                isReady = true;
                UnityThread.executeInUpdate(() =>
                {
                    GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                });
            }
        });
        t.Start();
    }
}