using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static string levelsDirectory = "C:\\Programowanie Obiektowe - Projekt\\Sokoban\\Levels\\";
    static string defaultLevelsDirectory;

    public Level levelPrefab;

    int choosenLevel;
    bool isLevelChoosen;
    public List<BasicLevelData> levels = new List<BasicLevelData>();

    public RectTransform levelListContent;
    public GameObject LevelListElement;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Exit()
    {
        Application.Quit();
    }


    public void ChooseLevel(int index)
    {
        Debug.Log(index);
    }


    void LoadLevelList()
    {
        string[] filesInDirectory = Directory.GetFiles(levelsDirectory);

        string[] _line;
        BasicLevelData _data;
        int i = 0;
        foreach(string file in filesInDirectory)
        {
            // Loading data to list
            _line = File.ReadAllLines(file)[0].Split(' ');
            if (_line.Length < 2)
                _data = new BasicLevelData(file);
            else
                _data = new BasicLevelData(file, (_line[0] == "y" ? true : false), System.Convert.ToSingle(_line[1]));
            
            levels.Add(_data);

            // Placing list elements and updating data
            GameObject newListElement = Instantiate(LevelListElement, levelListContent);
            // Pozycjonowanie elementów troche podobne do CSS
            newListElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(40 + i * 80));
            // Dodawanie delegata do listy funkcji jaką ma wykonać przycisk po wciśnięciu w menu
            // <NOTE> delegat mi tu w obu przypadkach przyjmuje 2 w obu przypadkach
            newListElement.transform.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(delegate { ChooseLevel(i); }));
            _data.listElement = newListElement.transform;
            _data.UpdateListElement();

            i++;
        }
        
        levelListContent.sizeDelta = new Vector2(0, 80 * levels.Count);
    }
}
