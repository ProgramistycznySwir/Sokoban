using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move))]
public class Crate : MonoBehaviour
{
    public Move movement;

    public new Renderer renderer;

    public bool willAchivePlace = false;

    public bool glowing = false;

    // Start is called before the first frame update
    void Start()
    {
        Material material = renderer.materials[2];
        material.SetColor("_EmissionColor", Color.green);
        renderer.materials[2] = material;
    }

    // Update is called once per frame
    void Update()
    {
        if(!glowing && !movement.isMoving && willAchivePlace)
        {
            glowing = true;
            renderer.materials[2].SetColor("_EmissionColor", Color.red);
        }
    }
}
