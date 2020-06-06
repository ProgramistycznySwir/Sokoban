using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Movable
{
    Vector2 movementInput = Vector2.zero;

    public Transform armsModel;

    public bool dontTurn;

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

        if (!isMoving && movementInput != Vector2.zero)
        {
            Vector3 correctedMovementInput;

            if (Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x))
                correctedMovementInput = Vector3.forward * (movementInput.y < 0 ? -1f : 1f);
            else
                correctedMovementInput = Vector3.right * (movementInput.x < 0 ? -1f : 1f);


            CollisionInfo collisionInfo = CheckCollision(correctedMovementInput);

            if (collisionInfo != CollisionInfo.Crate)
                RaiseHands(false);
            else
                RaiseHands(true);

            if(collisionInfo == CollisionInfo.Empty)
            {
                SetDestination(transform.position + correctedMovementInput);
            }
            else if (collisionInfo == CollisionInfo.Crate)
            {
                SetDestination(transform.position + correctedMovementInput);
            }
            else if (collisionInfo == CollisionInfo.Wall)
            {
                SetDestinationWithBounce(transform.position + correctedMovementInput * 0.3f);
            }
        }
        else if (!isMoving)
            RaiseHands(false);

        if (dontTurn == true)
            dontTurn = false;

        Move();
    }

    public override void SetDestination(Vector3 destination)
    {
        if (bounce)
        {
            dontTurn = true;
            transform.forward = destination - transform.position;
        }
        else if (!dontTurn)
            transform.forward = destination - transform.position;

        base.SetDestination(destination);
    }

    public void RaiseHands(bool raiseHands)
    {
        if (raiseHands)
            armsModel.localEulerAngles = Vector3.zero;
        else
            armsModel.localEulerAngles = new Vector3(90f, 0f, 0f);
    }
}
