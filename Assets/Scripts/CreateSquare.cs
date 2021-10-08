using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CreateSquare : MonoBehaviour
{
    public GameObject wheat;
    public GameObject chicken;
    public GameObject cow;
    public ChooseWhatToPlant choose;
    public Text txtWheat, txtChicken, txtCow;
    private int whatIsActive = 3;
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
            if (whatIsActive == 0)
            {
                GetComponent<Renderer>().material.color = new Color32(84, 53, 13, 255);
                var counter = int.Parse(txtWheat.text);
                counter++;
                txtWheat.text = counter.ToString();
                isReady = false;
                PlantWheat();
            }
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
        if (transform.childCount == 0)
        {
            var position = transform.position;
            Vector3 vector3 = new Vector3(position.x, position.y + 0.5f, position.z);
            Instantiate(obj, vector3, Quaternion.identity, transform);
            whatIsActive = choose.choose;
            if (whatIsActive == 0) PlantWheat();
            //if (whatIsActive == 1) PlantChicken();
            //if (whatIsActive == 2) PlantCow();
        }
    }

    private void PlantWheat()
    {
        Thread t = new Thread(() =>
        {
            Thread.Sleep(10000);
            isReady = true;
            if (whatIsActive != 3)
            {
                UnityThread.executeInUpdate(() =>
                {
                    GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
                });
            }
        });
        t.Start();
    }
}