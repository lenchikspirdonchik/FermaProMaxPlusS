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
    private int whatIsActive = 0;
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
        
    }

    private void Delete()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        whatIsActive = 0;
    }

    private void Add(GameObject obj)
    {
        if (transform.childCount == 0)
        {
            var position = transform.position;
            Vector3 vector3 = new Vector3(position.x, position.y + 0.5f, position.z);
            Instantiate(obj, vector3, Quaternion.identity, transform);
            whatIsActive = choose.choose;
            PlantWheat();
        }
    }

    private void PlantWheat()
    {
        Thread t = new Thread(new ThreadStart(() =>
        {
            Thread.Sleep(10000);
            isReady = true;
            Debug.Log("Is Ready");
            UnityThread.executeInUpdate(() =>
            {
                GetComponent<Renderer>().material.color = new Color32(194, 124, 0, 255);
            });
        }));
        t.Start();
        
       
    }
}