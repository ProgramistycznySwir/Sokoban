using UnityEngine;

public class BasicLevelData
{
    // Jeśli prawdziwe to podczas zapisu Menu.cs nadpisuje nagłówek poziomu danymi z tego BasicLevelData
    bool isDataUpdated;
    /// <summary>
    /// If true, this BasicLevelData has been updated and need to be overwritten in files
    /// </summary>
    public bool IsDataUpdated;
    // Ścieżka bezwzględna pliku .txt z poziomem
    string fullName;
    public string FullName { get { return fullName; } }
    // Właściwość która ekstraktuje nazwę mapy ze ścieżki
    public string Name { get { string[] splitedFullName = fullName.Split('\\'); return splitedFullName[splitedFullName.Length - 1].Split('.')[0]; } }
    // Czy poziom został ukończony
    bool finished;
    public bool Finished { get { return finished; } }
    // Najlepszy czas
    float bestTime;
    public float BestTime { get { return bestTime; } }

    public Transform listElement;



    public BasicLevelData(string fullName = "none", bool finished = false, float bestTime = 0f)
    {
        this.finished = finished;
        this.fullName = fullName;
        this.bestTime = bestTime;
    }

    /// <summary>
    /// Call only if level was finished, cause calling this is identical of setting finished variable to true.
    /// </summary>
    /// <param name="time"> Pass just time in which level was finished, method will sort itself out.</param>
    public void UpdateData(float time)
    {
        if (time < bestTime || !finished)
        {
            finished = true;
            bestTime = time;
            isDataUpdated = true;
        }

        if (isDataUpdated)
            UpdateListElement();
    }

    public void UpdateListElement()
    {
        listElement.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Name;
        listElement.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"{(finished ? $"<color=#009500><b>Finished</b></color>\nBest time: {bestTime.ToString("F3")}s" : "Not finished")}";
    }

    // Ta metoda służy wyłącznie do celów debugu
    public override string ToString()
    {
        return $"{Name} : {finished} : {bestTime}";
    }
}