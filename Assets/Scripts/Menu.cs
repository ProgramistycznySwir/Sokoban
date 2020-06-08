using System.IO;
using System.Collections.Generic;
using UnityEngine;

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


    void LoadLevelList()
    {
        // Loading data to list
        string[] filesInDirectory = Directory.GetFiles(levelsDirectory);
        string[] _line;
        int i = 0;
        foreach(string file in filesInDirectory)
        {
            Debug.Log($"File: {file}");
            _line = File.ReadAllLines(file)[0].Split(' ');
            if (_line.Length < 2)
                levels.Add(new BasicLevelData(file));
            else
                levels.Add(new BasicLevelData(file, (_line[0] == "y" ? true : false), System.Convert.ToSingle(_line[1])));
            Debug.Log($"Data: {levels[i++]}");
        }

        // Placing
        levelListContent.sizeDelta = new Vector2(0, 80 * levels.Count);
        i = 0;
        foreach(BasicLevelData data in levels)
        {
            GameObject newElement = Instantiate(LevelListElement, levelListContent);
            //newElement.GetComponent<RectTransform>().localPosition = new Vector3(0, -(40 + i++ * 80), 0);
            newElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(40 + i++ * 80));
            newElement.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = data.name;
            newElement.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"{(data.finished ? "<color=#009500><b>Finished</b></color>" : "Not finished")}\nBest time: {data.bestTime}";
        }
    }
}
