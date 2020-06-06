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

        Move();
    }


    public override CollisionInfo CheckCollision(Vector3 direction)
    {
        Debug.Log("I'm hewwe uwu");

        // Musi być troche podniesiony bo wszystkie obiekty centrum 
        Ray ray = new Ray(transform.position + Vector3.up * 0.19f, direction/*.normalized*/);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            Crate crate;
            // Najłatwiejszy sposób by sprawdzić czy obiekt jest 
            if ((crate = hit.transform.GetComponent<Crate>()) != null)
            {
                SetDestinationWithBounce(transform.position + direction * 0.2f, 0.275f / movementSpeed);
                return CollisionInfo.Crate;
            }
            SetDestinationWithBounce(transform.position + direction * 0.1f, 0.275f / movementSpeed);
            return CollisionInfo.Wall;
        }
        // 0.175 - długość rąk
        // 0.4 - "promień" (połowa boku) skrzynki
        SetDestination(transform.position + direction, 0.275f / movementSpeed);
        return CollisionInfo.Empty;
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
