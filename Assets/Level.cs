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

    public Grid grid;

    public Timer timer;

    public PauseMenu pauseMenu;

    // O - wall
    // P - Player
    // C - Crate
    // X - platform
    char[,] map;

    void Awake()
    {
        __current = this;
    }


    // Normalnie to byłby konstruktor, ale ze względu na specyfikę Unity, łatwiej jest to zrobić w ten sposób.
    /// <summary>
    /// Initializes this Level, assignes stuff, generate meshes and place objects.
    /// </summary>
    /// <param name="levelData"> Level data of level you want to initialize.</param>
    public void Initialize(BasicLevelData levelData)
    {
        // Przypisuje referencję na levelData.
        this.levelData = levelData;

        try
        {
            // Ładuje mapę z pliku do którego ścieżka jest w levelData.FullName.
            LoadMapFromFile();
        }
        catch(System.IO.FileNotFoundException exception)
        {
            // Logi przechowywane są w:
            // C:\Users\username\AppData\LocalLow\DefaultCompany\Sokoban\Player.log
            Debug.LogError("ERROR: Level.cs/Initialize() has failed to load map from file due to its inexistance.\n" + exception.ToString() +
                   "\nStopped execution of this method and given control back to Menu.cs (main menu).");
            // Dalsze wykonanie poziomu jest nie możliwe.
            pauseMenu.Exit();
            return;
        }

        // Inicjuje mapę na podstawie wcześniej załadowanego map.
        GenerateMap();
    }


    public void Win()
    {
        levelData.UpdateData(timer.Time);

        pauseMenu.Pause(true);
    }


    void LoadMapFromFile()
    {
        // Ustawia tablicę map tak by była obecnie wybraną mapą.

        string[] lines = File.ReadAllLines(levelData.FullName);

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


    void GenerateMap()
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
                        Instantiate(playerPrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    case 'C':
                        Instantiate(cratePrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    case 'X':
                        Instantiate(placePrefab, grid.CellToWorld(gridPosition), Quaternion.identity, transform);
                        break;
                    default:
                        break;
                }
            }

        // Generowanie ścian
        wall.GenerateWalls(map);

        // Tutaj wycentrowywana jest kamera
        Camera.main.transform.position = PlaceCamera(mapSize);
    }


    /// <summary>
    /// Calculates the best place for camera and then puts it there.
    /// </summary>
    public Vector3 PlaceCamera(Vector2 mapSize)
    {
        float height;

        float heightX = Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView * Camera.main.aspect) * (mapSize.x + 2);
        float heightY = Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView) * (mapSize.y + 2);

        if (heightX > heightY)
            height = heightX;
        else
            height = heightY;

        return new Vector3(mapSize.x, height, -mapSize.y) / 2f;
    }
    
}
