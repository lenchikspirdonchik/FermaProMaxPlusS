using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SC_BackgroundScaler : MonoBehaviour
{
    private Image backgroundImage;
    private float ratio;
    private RectTransform rt;

    // Start is called before the first frame update
    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        rt = backgroundImage.rectTransform;
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!rt)
            return;

        //Scale image proportionally to fit the screen dimensions, while preserving aspect ratio
        if (Screen.height * ratio >= Screen.width)
            rt.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        else
            rt.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
    }
}