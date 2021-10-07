
using UnityEngine;

public class CreateSquare : MonoBehaviour
{
    public GameObject wheat;
    public GameObject chicken;
    public GameObject cow;
    public ChooseWhatToPlant choose;
    private int whatIsActive = 0;


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
            default:
                Add(wheat);
                break;
        }
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
        }
    }
}