using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu main;

    public static string levelsDirectory = "C:\\Programowanie Obiektowe - Projekt\\Sokoban\\Levels\\";
    static string defaultLevelsDirectory;

    public GameObject levelPrefab;

    BasicLevelData choosenLevel;
    //bool isLevelChoosen;
    public TMPro.TextMeshProUGUI choosenLevelText;
    public Button playButton;

    public List<BasicLevelData> levels = new List<BasicLevelData>();

    public RectTransform levelListContent;
    public GameObject levelListElement;

    void Awoke()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelList();
        main = this;
    }

    // Metoda Unity wzywana kiedy obiekt jest niszczony.
    // W tym przypadku jest to najlepszy sposób by zapewnić by postęp gracza był zapisany nie wiadomo co.
    void OnDestroy()
    {
        SaveLevelList();
    }


    public void Return(bool restart)
    {
        SaveLevelList();
        Destroy(Level.current.gameObject);
        if (restart)
            Play();
        else
            gameObject.SetActive(true);
    }


    public void Play()
    {
        GameObject newLevel = Instantiate(levelPrefab);
        newLevel.GetComponent<Level>().Initialize(choosenLevel);
        gameObject.SetActive(false);
    }


    public void Exit()
    {
        Application.Quit();
    }


    public void ChooseLevel(int index)
    {
        choosenLevel = levels[index];
        playButton.interactable = true;
        choosenLevelText.text = $"{choosenLevel.Name}\n{(choosenLevel.Finished ? $"<color=#009500><b>Finished</b></color>\nBest time: {choosenLevel.BestTime.ToString("F3")}s" : "Not finished")}";
    }


    void LoadLevelList()
    {
        string[] filesInDirectory = Directory.GetFiles(levelsDirectory);

        string[] line_;
        BasicLevelData data_;
        int i = 0;
        foreach(string file in filesInDirectory)
        {
            // Loading data to list
            line_ = File.ReadAllLines(file)[0].Split(' ');
            if (line_.Length < 2)
                data_ = new BasicLevelData(file);
            else
                data_ = new BasicLevelData(file, (line_[0] == "y" ? true : false), System.Convert.ToSingle(line_[1]));
            
            levels.Add(data_);

            // Placing list elements and updating data
            GameObject newListElement = Instantiate(levelListElement, levelListContent);
            // Pozycjonowanie elementów troche podobne do CSS
            newListElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(40 + i * 80));
            // Dodawanie delegata do listy funkcji jaką ma wykonać przycisk po wciśnięciu w menu
            // <NOTE> delegat mi tu w obu przypadkach przyjmuje 2 w obu przypadkach
            int _index = i;
            newListElement.transform.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(_index); });
            //newListElement.transform.GetComponent<Button>().onClick.AddListener(() => ChooseLevel(i));
            Debug.Log(newListElement.transform.GetComponent<Button>().onClick.GetPersistentMethodName(0));
            data_.listElement = newListElement.transform;
            data_.UpdateListElement();

            i++;
        }
        
        levelListContent.sizeDelta = new Vector2(0, 80 * levels.Count);
    }

    void SaveLevelList()
    {
        foreach (BasicLevelData level in levels)
            if(level.IsDataUpdated)
            {
                string[] lines = File.ReadAllLines(level.FullName);
                lines[0] = (level.Finished) ? $"y {level.BestTime}" : "";
                File.WriteAllLines(level.FullName, lines);
            }
    }
}
