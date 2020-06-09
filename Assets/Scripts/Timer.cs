using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    public Movable playerMovable;

    float time;
    public float Time { get { return time; } }



    // Update is called once per frame
    void Update()
    {
        time += UnityEngine.Time.deltaTime;
        Display();
    }

    void Display()
    {
        text.text = time.ToString("F1") + "s";
    }
}
