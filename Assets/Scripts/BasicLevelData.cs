public class BasicLevelData
{
    public string fullName;
    public string name { get { string[] splitedFullName = fullName.Split('\\'); return splitedFullName[splitedFullName.Length - 1].Split('.')[0]; } }
    public bool finished;
    public float bestTime;

    public BasicLevelData(string fullName = "none", bool finished = false, float bestTime = 0f)
    {
        this.finished = finished;
        this.fullName = fullName;
        this.bestTime = bestTime;
    }

    // Ta metoda służy wyłącznie do celów debugu
    public override string ToString()
    {
        return $"{name} : {finished} : {bestTime}";
    }
}