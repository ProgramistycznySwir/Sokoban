using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu main;

    public static string levelsDirectory = "Levels\\";

    public GameObject levelPrefab;

    BasicLevelData choosenLevel;
    public TMPro.TextMeshProUGUI choosenLevelText;
    public Button playButton;

    public List<BasicLevelData> levels = new List<BasicLevelData>();

    public RectTransform levelListContent;
    public GameObject levelListElement;

    public Slider wallHeightSlider;
    public TMPro.TextMeshProUGUI wallHeightText;
    public Slider movementSpeedSlider;
    public TMPro.TextMeshProUGUI movementSpeedText;



    void Awoke()
    {
        main = this;
    }

    void Start()
    {
        // Po to żeby suwaki na początku były ustawione w domyślnych wartościach
        wallHeightSlider.value = Mathf.InverseLerp(Wall.wallHeightRange.x, Wall.wallHeightRange.y, Wall.wallHeight);
        movementSpeedSlider.value = Mathf.InverseLerp(Movable.movementSpeedRange.x, Movable.movementSpeedRange.y, Movable.movementSpeed);

        LoadLevelList();
        main = this;
    }

    // Metoda Unity wzywana kiedy obiekt jest niszczony.
    // W tym przypadku jest to najlepszy sposób by zapewnić by postęp gracza był zapisany nie wiadomo co się stanie z programem.
    void OnApplicationQuit()
    {
        SaveLevelList();
    }

    public void Return(bool restart)
    {
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


    #region >>> Settings Menu <<<

    public void WallHeightSlider()
    {
        float value = Mathf.Lerp(Wall.wallHeightRange.x, Wall.wallHeightRange.y, wallHeightSlider.value);
        wallHeightText.text = value.ToString("F1");
        Wall.wallHeight = value;
    }
    public void MovementSpeedSlider()
    {
        float value = Mathf.Lerp(Movable.movementSpeedRange.x, Movable.movementSpeedRange.y, movementSpeedSlider.value);
        movementSpeedText.text = value.ToString("F1");
        Movable.movementSpeed = value;
    }

    #endregion


    #region >>> Files <<<

    /// <summary>
    /// 
    /// </summary>
    void LoadLevelList()
    {
        string[] filesInDirectory = new string[0];
        try
        {
            filesInDirectory = Directory.GetFiles(levelsDirectory);
        }
        catch (System.Exception exception)
        {
            Debug.LogError("ERROR: Menu.cs/LoadLevelList() failed to find levels folder: " + levelsDirectory + "\n" + exception.ToString() +
               "\nFurther programm behaviour: Doesn't loaded any levels soo game is basicly useless without fixing this problem.");
        }

        string[] line_;
        BasicLevelData data_;
        int i = 0;
        foreach (string file in filesInDirectory)
        {
            try
            {
                // Loading data to list
                line_ = File.ReadAllLines(file)[0].Split(' ');
                if (line_.Length < 2)
                    data_ = new BasicLevelData(file);
                else
                    data_ = new BasicLevelData(file, (line_[0] == "y" ? true : false), System.Convert.ToSingle(line_[1]));

                // Placing list elements and updating data
                GameObject newListElement = Instantiate(levelListElement, levelListContent);
                // Pozycjonowanie elementów troche podobne do CSS
                newListElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(40 + i * 80));
                // Dodawanie delegata do listy funkcji jaką ma wykonać przycisk po wciśnięciu w menu.
                // Ten _index jest niezbędny bo delegaty przyjmują referencję na zmienną, nie jej kopię.
                int _index = i;
                newListElement.transform.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(_index); });
                // By data_ mógł później własnoręcznie aktualizować dane w menu
                data_.listElement = newListElement.transform;
                data_.UpdateListElement();


                levels.Add(data_);

                i++;
            }
            catch (System.Exception exception)
            {
                Debug.LogError("ERROR: Menu.cs/LoadLevelList() failed to load data from file: " + file + "\n" + exception.ToString() +
                   "\nFurther programm behaviour: Avoided any other atempts of loading this level, but continues to try load rest of them.");
            }
        }

        levelListContent.sizeDelta = new Vector2(0, 80 * levels.Count);
    }

    void SaveLevelList()
    {
        foreach (BasicLevelData level in levels)
            if (level.IsDataUpdated)
            {
                string[] lines = File.ReadAllLines(level.FullName);
                lines[0] = (level.Finished) ? $"y {level.BestTime}" : "";
                File.WriteAllLines(level.FullName, lines);
            }
    }



    #endregion
}

public class UnasignedBasicLevelDataException : System.Exception
{
    public string message;

    public UnasignedBasicLevelDataException(string message)
    {

    }
}