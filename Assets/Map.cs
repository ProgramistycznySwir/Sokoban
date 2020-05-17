using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // <NOTE> To będzie można sparametryzować i wrzucić do ustawień
    public static float wallHeight = 1f;

    public GameObject playerPrefab;
    public GameObject cratePrefab;
    public GameObject placePrefab;
    public GameObject wallPrefab;
    public MeshFilter wallsMeshFilter;

    public Grid grid__;
    Grid __grid;
    public Grid grid { get { return __grid; } }


    // Musi być Vector3 bo dużo metod unity używa właśnie Vector3 zamiast Vector2
    public Vector3Int playerPosition;

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

        //map = new char[,]{ { ' ', ' ', ' ', ' ' },
        //                   { ' ', 'O', 'O', ' ' },
        //                   { ' ', 'O', 'O', ' ' },
        //                   { ' ', 'O', 'O', ' ' },
        //                   { ' ', ' ', ' ', ' ' }};
    }

    void Start()
    {
        GenerateWalls();
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum CollisionInfo { Wall, CrateWall, Crate, Free}

    /// <summary>
    /// Only dirrection is needed cause map stores player position anyways
    /// </summary>
    /// <returns>Whether object can perform movement or not</returns>
    public CollisionInfo AttemptMovement(Vector2Int dirrection) //<NOT IMPLEMENTED>
    {
        return CollisionInfo.Free;
    }

    public void EnlistCrate(Crate crate)
    {
        crates.Add(crate);
    }

    public void GenerateMap() //<NOT IMPLEMENTED>
    {
        // Cała magia dzieje się tutaj, ustawiane są skrzynie, platformy, gracz, oraz generowany jest mesh ścian
        Vector3Int gridPosition;
        Vector2Int mapSize = new Vector2Int(map.GetUpperBound(1), map.GetUpperBound(0));
        for (int y = 0; y <= mapSize.y; y++)
            for (int x = 0; x <= mapSize.x; x++)
            {
                gridPosition = new Vector3Int(x, -y, 0);
                switch(map[y, x])
                {
                    case 'O':
                        Instantiate<GameObject>(wallPrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    case 'P':
                        Instantiate<GameObject>(playerPrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    case 'C':
                        Instantiate<GameObject>(cratePrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    case 'X':
                        Instantiate<GameObject>(placePrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    default:
                        break;
                }
            }

        // Tutaj wycentrowywana jest kamera
        Camera.main.transform.position = new Vector3(mapSize.x / 2f, 6, -mapSize.y / 2f);
    }

    public void LoadMapFromFile() //<NOT IMPLEMENTED>
    {
        // Ustawia tablicę map tak by była obecnie wybraną mapą.
    }

    // Mógłbym w tym miejscu posilić się o greedy-mesh, ale sądzę że nawet jeśli będą obecne zbędne współliniowe wierzchołki to dalej będzie to
    // bardziej optymalne od tego żeby każda ściana była odrębnym obiektem.
    // Update: o ile współliniowe wierzchołki chyba nikomu nie przeszkadzają, ale takie rzeczy jak kilka vertexów w tym samym miejscu już 
    // robią, że oświetlenie obiektu szaleje ;/
    // Update: udało mi się jako tako naprawić oświetlenie i jest... ummm... stylistyczne? Znaczy no, nie jest źle, ale niektóre krawędzie
    // szczególnie te dobrze doświetlone są troche rozmazane.
    public void GenerateWalls()
    {
        Mesh wallsMesh = new Mesh();

        // Vertex'y - punkty na których później się "rozpina" trójkąty z których składa się siatka obiektu
        List<Vector3> verts = new List<Vector3>();

        // Pierwszy submesh - podłoga
        List<int> tris0 = new List<int>();
        // Drugi submesh - ściany
        List<int> tris1 = new List<int>();

        // Normal'e - kierunki w które skierowane są powierzchnie trójkątów, potrzebne są do odpowiedniego oświetlenia obiektu
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        //int wallsCount = 0;
        Vector2Int mapSize = new Vector2Int(map.GetUpperBound(1), map.GetUpperBound(0));
        // Najpierw dodawane są vertexy, a dopiero w kolejnej pętli dodawane są trójkąty
        for (int y = 0; y <= mapSize.y + 1; y++)
            for(int x = 0; x <= mapSize.x + 1; x++)
            {
                // Vertexy niżej są parzyste, a wyżej nie parzyste
                // Powoduje to wystąpienie sporej ilości (n <= (mapSize.x+1)*(mapSize.y+1)) nie potrzebnych punktów, ale jest to rozwiązanie
                // wystarczająco optymalne (pojedyńcza skrzynka posiada nieco mniej vertex'ów co mapa 14x14).
                verts.Add(new Vector3(x, 0, -y));
                verts.Add(new Vector3(x, wallHeight, -y));

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                uvs.Add(new Vector2(x, -y));
                uvs.Add(new Vector2(x, -y + 1));
            }
        for (int y = 0; y <= mapSize.y; y++)
            for (int x = 0; x <= mapSize.x; x++)
            {
                if (map[y, x] == 'O')
                {
                    // Górna
                    tris1.Add((y * (mapSize.x + 2) + x) * 2 + 1);
                    tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                    tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2 + 1);

                    tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                    tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                    tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2 + 1);

                    // Przednia
                    if(y == mapSize.y || map[y+1, x] != 'O')
                    {
                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2 + 1);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2);

                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2);
                    }

                    // Tylna
                    if (y == 0 || map[y - 1, x] != 'O')
                    {
                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                        tris1.Add((y * (mapSize.x + 2) + x) * 2 + 1);
                        tris1.Add((y * (mapSize.x + 2) + x) * 2);

                        tris1.Add((y * (mapSize.x + 2) + x) * 2);
                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2);
                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                    }

                    // Prawa
                    if (x == mapSize.x || map[y, x + 1] != 'O')
                    {
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2);

                        tris1.Add((y * (mapSize.x + 2) + (x + 1)) * 2);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2 + 1);
                    }

                    // Lewa
                    if (x == 0 || map[y, x - 1] != 'O')
                    {
                        tris1.Add((y * (mapSize.x + 2) + x) * 2 + 1);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2 + 1);
                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2);

                        tris1.Add(((y + 1) * (mapSize.x + 2) + x) * 2);
                        tris1.Add((y * (mapSize.x + 2) + x) * 2);
                        tris1.Add((y * (mapSize.x + 2) + x) * 2 + 1);
                    }
                }
                else
                {
                    // Dolna
                    tris0.Add( (y * (mapSize.x + 2) + x ) * 2 );
                    tris0.Add( (y * (mapSize.x + 2) + (x + 1)) * 2 );
                    tris0.Add( ((y + 1) * (mapSize.x + 2) + x) * 2 );

                    tris0.Add((y * (mapSize.x + 2) + (x + 1)) * 2);
                    tris0.Add(((y + 1) * (mapSize.x + 2) + (x + 1)) * 2);
                    tris0.Add(((y + 1) * (mapSize.x + 2) + x) * 2);
                }
            }

        // Aplikowanie wygenerowanej siatki
        wallsMesh.subMeshCount = 2;
        wallsMesh.vertices = verts.ToArray();
        wallsMesh.SetTriangles(tris0, 0);
        wallsMesh.SetTriangles(tris1, 1);

        // Dla oświetlenia
        //wallsMesh.RecalculateNormals();
        // Jak nie prośbą to groźbą...
        //wallsMesh.SetNormals(normals);

        wallsMesh.RecalculateNormals();
        Vector3[] test = wallsMesh.normals;

        for (int i = 0; i < test.Length; i++)
        {
            //if(i % 2 == 1)
            //    test

            if (test[i].x > 0)
                test[i].x = 1;
            else if (test[i].x < 0)
                test[i].x = -1;

            if (test[i].y > 0)
                test[i].y = 1;
            else if (test[i].y < 0)
                test[i].y = -1;

            if (test[i].z > 0)
                test[i].z = 1;
            else if (test[i].z < 0)
                test[i].z = -1;
        }

        wallsMesh.SetNormals(normals);

        for (int i = 0; i < test.Length; i++)
            Debug.DrawLine(verts[i], verts[i] + test[i], Color.red);

        //wallsMesh.SetUVs(0, normals);

        // Aplikowanie przygotowanej siatki do sceny
        wallsMeshFilter.mesh = wallsMesh;
    }
}
