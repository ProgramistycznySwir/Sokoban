using System.IO;
using UnityEngine;


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

public class Level : MonoBehaviour
{
    static Level __current;
    static public Level current { get { return __current; } }

    BasicLevelData levelData;

    public GameObject playerPrefab;
    public GameObject cratePrefab;
    public GameObject placePrefab;
    public Wall wall;

    public Grid grid__;
    Grid __grid;
    public Grid grid { get { return __grid; } }

    public Timer timer;

    public PauseMenu pauseMenu;

    // O - wall
    // P - Player
    // C - Crate
    // X - platform
    char[,] map;

    void Awake()
    {
        __grid = grid__;
        __current = this;
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
                switch (map[y, x])
                {
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

        // Generowanie ścian
        wall.GenerateWalls(map);

        // Tutaj wycentrowywana jest kamera
        Camera.main.transform.position = new Vector3(mapSize.x / 2f, 6, -mapSize.y / 2f);
    }


    public void Initialize(BasicLevelData levelData)
    {
        this.levelData = levelData;

        LoadMapFromFile();

        GenerateMap();
    }


    public void Win()
    {
        levelData.UpdateData(timer.Time);

        pauseMenu.Pause(true);
    }


    public void LoadMapFromFile()
    {
        // Ustawia tablicę map tak by była obecnie wybraną mapą.

        string[] lines = File.ReadAllLines(levelData.FullName);

        foreach (string line in lines)
            Debug.Log(line);

        int longestLine = 0;
        for (int i = 1; i < lines.Length; i++)
            //foreach (string line in lines)
            if (lines[i].Length > longestLine)
                longestLine = lines[i].Length;
        map = new char[lines.Length - 1, longestLine];

        for(int y = 1; y < lines.Length; y++)
            for(int x = 0; x < longestLine; x++)
            {
                if (x >= lines[y].Length)
                    map[y - 1, x] = ' ';
                else
                    map[y - 1, x] = lines[y][x];
            }
    }

}
