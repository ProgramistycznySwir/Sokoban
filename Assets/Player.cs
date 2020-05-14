using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bez skryptu Move ani rusz (dosłownie obiekt nie będzie się poruszał)
[RequireComponent(typeof(Move))]
public class Player : MonoBehaviour
{

    public Move movement;

    public Grid grid;


    Vector2 movementInput = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxisRaw() to funkcja Unity która odpowiada za zwrócenie wartości osi wejścia z kontrolera w przypadku PC jest to klawiatura.
        // "Horizontal' to klawisze 'a' (dla wartości -1f) i 'd' (dla wartości 1f), odpowiednio "Vertical" jeśli nie zostanie zmienieone w opcjach.
        // Ta funkcja zwraca float, ponieważ współdziała z gałkami analogowymi kontrolerów.
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        if(!movement.isMoving && movementInput != Vector2.zero)
        {
            Vector3 correctedMovementInput;

            if (Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x))
                correctedMovementInput = Vector3.forward * (movementInput.y < 0 ? -1f : 1f);
            else
                correctedMovementInput = Vector3.right * (movementInput.x < 0 ? -1f : 1f);

            movement.SetDestination(transform.position + correctedMovementInput);
        }        
    }
}
