using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cratePrefab;
    public GameObject placePrefab;
    public MeshFilter wallsMeshFilter;

    public Grid grid__;
    Grid __grid;


    // Musi być Vector3 bo dużo metod unity używa właśnie Vector3 zamiast Vector2
    public Vector3Int playerPosition;

    public Transform test;

    // O - wall
    // P - Player
    // C - Crate
    // X - platform
    static char[,] map;

    static List<Crate> crates = new List<Crate>();

    void Awake()
    {
        __grid = grid__;

        map = new char[,]{ { 'O', 'O', 'O', 'O' },
            { 'O', 'P', 'X', 'O' },
            { 'O', ' ', 'C', 'O' },
            { 'O', ' ', ' ', 'O' },
            { 'O', 'O', 'O', 'O' }};
    }

    void Start()
    {
        GenerateWalls();
    }

    // Update is called once per frame
    void Update()
    {
        test.position = __grid.CellToWorld(playerPosition);
    }

    public enum CollisionInfo { Cannot, Crate, Nothing}

    /// <summary>
    /// Only dirrection is needed cause map stores player position anyways
    /// </summary>
    /// <returns>Whether object can perform movement or not</returns>
    public CollisionInfo AttemptMovement(Vector2Int dirrection) //<NOT IMPLEMENTED>
    {
        return CollisionInfo.Nothing;
    }

    public void EnlistCrate(Crate crate)
    {
        crates.Add(crate);
    }

    public void GenerateMap() //<NOT IMPLEMENTED>
    {
        // Cała magia dzieje się tutaj, ustawiane są skrzynie, platformy, gracz, oraz generowany jest mesh ścian
    }

    public void LoadMapFromFile() //<NOT IMPLEMENTED>
    {
        // Ustawia tablicę map tak by była obecnie wybraną mapą.
    }

    // Mógłbym w tym miejscu posilić się o greedy-mesh, ale sądzę że nawet jeśli będą obecne zbędne współliniowe wierzchołki to dalej będzie to
    // bardziej optymalne od tego żeby każda ściana była odrębnym obiektem.
    // Update: o ile współliniowe wierzchołki chyba nikomu nie przeszkadzają, ale takie rzeczy jak kilka vertexów w tym samym miejscu już 
    // robią, że oświetlenie obiektu szleje
    public void GenerateWalls()
    {
        Mesh wallsMesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        
        int wallsCount = 0;
        for(int y = 0; y <= map.GetUpperBound(0); y++)
            for(int x = 0; x <= map.GetUpperBound(1); x++)
            {
                if (map[y, x] != 'O')
                    continue;

                // Góra - zgodnie do wskazówek zegara zaczynając od lewego bliższego rogu
                verts.Add(new Vector3(x, 0, y));
                verts.Add(new Vector3(x, 0, y + 1));
                verts.Add(new Vector3(x + 1, 0, y + 1));
                verts.Add(new Vector3(x + 1, 0, y));

                // Dół - zgodnie do wskazówek zegara zaczynając od lewego bliższego rogu
                // Te -2 definiuje że ściana będzie się chować jeszcze 1 unit pod powierzchnię planszy.
                // Nie wiem czy to jest potrzebne, ale nie wiem czy nie zaczną się dziwne zachowania cieni jeśli ściana będzie choć odrobinę wystawać.
                verts.Add(new Vector3(x, -2, y));
                verts.Add(new Vector3(x, -2, y + 1));
                verts.Add(new Vector3(x + 1, -2, y + 1));
                verts.Add(new Vector3(x + 1, -2, y));

                // Górna ściana
                // Tutaj ustawiane są indeksy wierzchołków trójkątów (3 kolejne int'y w liście), żeby trójkąt miał normalną skierowaną w naszą stronę
                // wierzchołki muszą być ustawione w kierunku wskazówek zegara
                tris.Add(wallsCount);
                tris.Add(wallsCount + 1);
                tris.Add(wallsCount + 2);

                tris.Add(wallsCount + 2);
                tris.Add(wallsCount + 3);
                tris.Add(wallsCount);

                // Lewa ściana
                if (!(x > 0 && map[y, x - 1] == 'O'))
                {
                    tris.Add(wallsCount);
                    tris.Add(wallsCount + 4);
                    tris.Add(wallsCount + 5);

                    tris.Add(wallsCount + 5);
                    tris.Add(wallsCount + 1);
                    tris.Add(wallsCount);
                }
                // Prawa ściana
                if (!(x < map.GetUpperBound(1) && map[y, x + 1] == 'O'))
                {
                    tris.Add(wallsCount + 2);
                    tris.Add(wallsCount + 6);
                    tris.Add(wallsCount + 7);

                    tris.Add(wallsCount + 7);
                    tris.Add(wallsCount + 3);
                    tris.Add(wallsCount + 2);
                }
                // Bliska ściana
                if (!(y > 0 && map[y - 1, x] == 'O'))
                {
                    tris.Add(wallsCount);
                    tris.Add(wallsCount + 3);
                    tris.Add(wallsCount + 7);

                    tris.Add(wallsCount + 7);
                    tris.Add(wallsCount + 4);
                    tris.Add(wallsCount);
                }
                // Dalsza ściana
                if (!(y < map.GetUpperBound(0) && map[y + 1, x] == 'O'))
                {
                    tris.Add(wallsCount + 1);
                    tris.Add(wallsCount + 5);
                    tris.Add(wallsCount + 6);

                    tris.Add(wallsCount + 6);
                    tris.Add(wallsCount + 2);
                    tris.Add(wallsCount + 1);
                }

                tris.Add(wallsCount + 4);
                tris.Add(wallsCount + 7);
                tris.Add(wallsCount + 6);

                tris.Add(wallsCount + 6);
                tris.Add(wallsCount + 5);
                tris.Add(wallsCount + 4);

                wallsCount += 8;
            }

        wallsMesh.vertices = verts.ToArray();
        wallsMesh.triangles = tris.ToArray();

        // Dla oświetlenia
        wallsMesh.RecalculateNormals();

        wallsMeshFilter.mesh = wallsMesh;
    }
}
