using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    public Movable playerMovable;

    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Display();
    }

    void Display()
    {
        text.text = time.ToString("F0") + "s";
    }
}
