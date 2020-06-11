using UnityEngine;


public class Player : Movable
{
    public Transform armsModel;

    // Dla animacji podniesienia rąk
    public bool dontTurn;

    // Żeby dać skrzyniom czas na wykonanie pełnych animacji
    float inputWait;
    float inputDelay;


    void Start()
    {
        // By nie liczyć tego za każdym razem
        inputDelay = 0.3f / movementSpeed;
    }
    

    Vector2 movementInput = Vector2.zero;
    void Update()
    {
        // Input.GetAxisRaw() to funkcja Unity która odpowiada za zwrócenie wartości osi wejścia z kontrolera w przypadku PC jest to klawiatura.
        // "Horizontal' to klawisze 'a' (dla wartości -1f) i 'd' (dla wartości 1f), odpowiednio "Vertical" jeśli nie zostanie zmienione w opcjach.
        // Ta funkcja zwraca float, ponieważ współdziała z analogowymi gałkami kontrolerów.
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        
        
        if (!isMoving && movementInput != Vector2.zero && inputWait < 0f)
        {
            Vector3 correctedMovementInput;

                if (Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x))
                correctedMovementInput = Vector3.forward * (movementInput.y < 0 ? -1f : 1f);
            else
                correctedMovementInput = Vector3.right * (movementInput.x < 0 ? -1f : 1f);


            CollisionInfo collisionInfo = CheckCollision(correctedMovementInput);

            if (collisionInfo == CollisionInfo.Crate || collisionInfo == CollisionInfo.CrateCrate || collisionInfo == CollisionInfo.CrateWall)
                RaiseHands(true);
            else
                RaiseHands(false);

            switch(collisionInfo)
            {
                case CollisionInfo.Empty:
                    SetDestination(transform.position + correctedMovementInput);
                    inputWait = 0;
                    break;
                case CollisionInfo.Crate:
                    SetDestination(transform.position + correctedMovementInput);
                    inputWait = inputDelay;
                    break;
                case CollisionInfo.CrateCrate:
                    SetDestinationWithBounce(transform.position + correctedMovementInput * 0.475f);
                    inputWait = inputDelay;
                    break;
                case CollisionInfo.CrateWall:
                    SetDestinationWithBounce(transform.position + correctedMovementInput * 0.375f);
                    inputWait = 0;
                    break;
                case CollisionInfo.Wall:
                    SetDestinationWithBounce(transform.position + correctedMovementInput * 0.3f);
                    inputWait = inputDelay;
                    break;
            }
        }
        else if (!isMoving)
            RaiseHands(false);


        if (!isMoving)
            inputWait -= Time.deltaTime;

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


    public override CollisionInfo CheckCollision(Vector3 direction)
    {
        // Musi być troche podniesiony bo wszystkie obiekty centrum 
        Ray ray = new Ray(transform.position + Vector3.up * 0.19f, direction/*.normalized*/);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            Crate crate;
            // Najłatwiejszy sposób by sprawdzić czy obiekt jest 
            if ((crate = hit.transform.GetComponent<Crate>()) != null)
            {
                CollisionInfo collisionInfo = crate.CheckCollision(direction);
                if (collisionInfo == CollisionInfo.Empty)
                    return CollisionInfo.Crate;
                else if (collisionInfo == CollisionInfo.Crate)
                    return CollisionInfo.CrateCrate;
                else
                    return CollisionInfo.CrateWall;
            }

            return CollisionInfo.Wall;
        }

        return CollisionInfo.Empty;
    }


    public void RaiseHands(bool raiseHands)
    {
        if (raiseHands)
            armsModel.localEulerAngles = Vector3.zero;
        else
            armsModel.localEulerAngles = new Vector3(90f, 0f, 0f);
    }
}
