using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Crate : Movable
{
    public new Renderer renderer;

    public bool willAchivePlace = false;

    public bool glowing = false;
    
    void Start()
    {
        // Podczas kompilowania gry silnik optymalizuje build zawierając tylko elementy które są obecne w jakiś sposób na scenie
        // czyli żeby Unity zawarło moduł świecenia dla materiału Crate_Inner, musi być on aktywny na początku, więc jeśli jest
        // tworzony obiekt to musi mieć ręcznie wyłączone świecenie :/.
        Glow(false);
    }
    
    void Update()
    {
        if (!glowing && !isMoving && willAchivePlace)
        {
            glowing = true;
            Glow(true);
        }
    }

    void Glow(bool glow)
    {
        Material material = renderer.materials[2];
        if (glow)
            material.SetColor("_EmissionColor", Color.green);
        else
            material.SetColor("_EmissionColor", Color.black);
    }
}
