using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider slider;

    private void Update()
    {
        slider.value = RobWheat.chance;
    }
}