using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ten jest o tyle specjalny bo musi odwracać się w określone strony
/// </summary>
public class PlayerMove : Move
{

    void Start()
    {
        
    }
    
    // Kod z base.Update() niestety musi zostać przepisany tutaj, ponieważ Unity narzuca że ta metoda jest metodą prywatną
    void Update()
    {
        // Jeśli osiągnął cel to nie ma sensu by dalej wykonywał obliczenia (jeszcze może z tego bug wyniknąć, a tego przecież nie chcemy)
        if (!__isMoving)
            return;

        // Odlicza czas do rozpoczęcia animacji
        if (wait > 0f)
            wait -= Time.deltaTime;

        // Transform to instancja klasy która definiuje pozycję obiektu (GameObject) w euklidesowej przestrzeni 3d, zmienna position to pozycja x,y,z.
        // transform jest zmienną dziedziczoną z klasu MonoBehaviour tak samo jak Start() i Update().
        // Funkcja Vector3.MoveTowards() w wygodny sposób określa wartość nowego wektora (na podstawie kierunku z 1wszego parametru do 2giego)z tym
        // założeniem, że odległość punktu który zwraca jest w odległości nie większej niż 3ci parametr od 1wszego parametru
        // movementSpeed jest mnożony przez Time.deltaTime by uniezależnić animację od szybkości klatek (Time.deltaTime to czas 
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

        // Sprawdza czy obiekt osiągnął cel
        if (transform.position == destination)
            __isMoving = false;
    }

    public override void SetDestination(Vector3 destination)
    {
        __isMoving = true;
        this.destination = destination;

        transform.forward = destination - transform.position;
    }
}
