﻿using UnityEngine;


// Convention note:
// __name - pole
// name_ - zmienna tymczasowa
// name__ - zmienna dla Unity (by była widoczna w inspectorze)
//
// propertyName - jeśli jest to jedynie podanie przesłoniętego pola
// PropertyName - jeśli jest to jakaś formuła, prawie metoda
//
// Nasuwa się pytanie dlaczego nazwa właściwości jest pisana z małej litery?
// Taka jest konwencja Unity.

// Najlepiej jeśli będzie abstract, bo nie będzie żadnego obiektu tego typu w grze
/// <summary>
/// Script responsible for movement of objects
/// </summary>
public abstract class Movable : MonoBehaviour
{
    /// <summary>
    /// How fast every movable object traverses
    /// </summary>
    public static readonly Vector2 movementSpeedRange = new Vector2(1f, 10f);
    protected static float __movementSpeed = 3f;
    public static float movementSpeed { get { return __movementSpeed; } set { __movementSpeed = Mathf.Clamp(value, movementSpeedRange.x, movementSpeedRange.y); } }

    /// <summary>
    /// If object reached destination is false
    /// Player script makes use of it to determine if next move is possible
    /// </summary>
    protected bool __isMoving = false;
    public bool isMoving { get { return __isMoving; } }

    /// <summary>
    /// How much time (in seconds) does this object have to wait till it starts animation
    /// </summary>
    public float wait = 0f;

    protected Vector3 destination;
    protected Vector3 destinationBounce;

    protected bool bounce;

    void Update()
    {
        // Po to wyrzuciłem to do odrębnej metody bo MonoBehaviour.Update() nie jest dziedziczony.
        Move();
    }

    public void SetDestinationWithBounce(Vector3 destination, float delay = 0f)
    {
        bounce = true;
        destinationBounce = transform.position;
        // Odwołanie do metody aby uniknąć powtarzania kodu
        SetDestination(destination, delay);
    }

    public void SetDestination(Vector3 destination, float delay)
    {
        wait = delay;
        // Odwołanie do metody aby uniknąć powtarzania kodu
        SetDestination(destination);
    }

    public virtual void SetDestination(Vector3 destination)
    {
        __isMoving = true;
        this.destination = destination;
    }

    protected void Move()
    {
        // Jeśli osiągnął cel to nie ma sensu by dalej wykonywał obliczenia.
        if (!__isMoving)
            return;

        // Odlicza czas do rozpoczęcia animacji
        if (wait > 0f)
        {
            wait -= Time.deltaTime;
            return;
        }

        // Transform to instancja klasy która definiuje pozycję obiektu (GameObject) w euklidesowej przestrzeni 3d, zmienna position to pozycja x,y,z.
        // transform jest zmienną dziedziczoną z klasu MonoBehaviour tak samo jak Start() i Update().
        // Funkcja Vector3.MoveTowards() w wygodny sposób określa wartość nowego wektora (na podstawie kierunku z 1wszego parametru do 2giego) z tym
        // założeniem, że odległość punktu który zwraca jest w odległości nie większej niż 3ci parametr od 1wszego parametru
        // movementSpeed jest mnożony przez Time.deltaTime by uniezależnić animację od szybkości klatek (Time.deltaTime to czas) 
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

        // Sprawdza czy obiekt osiągnął cel
        if (transform.position == destination && bounce)
        {
            bounce = false;
            destination = destinationBounce;
        }
        else if (transform.position == destination)
            __isMoving = false;
    }

    public enum CollisionInfo { Empty, Crate, Wall, CrateCrate, CrateWall}
    // Jest to metoda abstrakcyjna bo i Crate i Player muszą definiować 2 odrębne logiki związane z tą metodą
    public abstract CollisionInfo CheckCollision(Vector3 direction);


    public static void SetMovementSpeed(float speed)
    {
        if (speed < 0.5f)
            speed = 0.5f;
        __movementSpeed = speed;
    }
}
