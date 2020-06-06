using UnityEngine;


public class Crate : Movable
{
    public Place occupiedPlace;

    public new Renderer renderer;

    public bool checkIfOccupying = false;

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
        if (!isMoving && checkIfOccupying)
        {
            Occupy();
        }

        Move();
    }


    public override CollisionInfo CheckCollision(Vector3 direction)
    {
        // Musi być troche podniesiony bo wszystkie obiekty centrum mają na ziemi
        Ray ray = new Ray(transform.position + Vector3.up * 0.19f, direction/*.normalized*/);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            Crate crate = hit.transform.GetComponent<Crate>();
            // Najłatwiejszy sposób by sprawdzić czy obiekt jest 
            if (crate)
            {
                SetDestinationWithBounce(transform.position + direction * 0.2f, 0.275f / movementSpeed);
                return CollisionInfo.Crate;
            }
            SetDestinationWithBounce(transform.position + direction * 0.1f, 0.275f / movementSpeed);
            return CollisionInfo.Wall;
        }

        checkIfOccupying = true;
        // 0.175 - długość rąk
        // 0.4 - "promień" (połowa boku) skrzynki
        SetDestination(transform.position + direction, 0.275f / movementSpeed);
        return CollisionInfo.Empty;
    }


    void Occupy()
    {
        if(occupiedPlace)
        {
            occupiedPlace.isOccupied = false;
            occupiedPlace = null;
        }

        // Musi być troche podniesiony bo wszystkie obiekty centrum mają na ziemi
        Ray ray = new Ray(transform.position + Vector3.up * 0.19f, Vector3.down/*.normalized*/);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            Place place = hit.transform.GetComponent<Place>();
            if(place)
            {
                occupiedPlace = place;
                place.isOccupied = true;
            }

            Glow(place);
        }

        checkIfOccupying = false;
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
