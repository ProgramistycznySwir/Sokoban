public class BasicLevelData
{
    public bool finished;
    public string name;
    public float bestTime;

    public BasicLevelData(bool finished = false, string name = "none", float bestTime = 0f)
    {
        this.finished = finished;
        this.name = name;
        this.bestTime = bestTime;
    }
}