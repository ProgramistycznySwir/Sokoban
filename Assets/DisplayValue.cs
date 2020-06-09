using UnityEngine;

public class DisplayValue : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public TMPro.TextMeshProUGUI text;

    public void UpdateText()
    {
        float value = slider.value;
        text.text = value.ToString("F1");
    }
}
