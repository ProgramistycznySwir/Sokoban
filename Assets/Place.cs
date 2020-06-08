using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    static List<Place> places = new List<Place>();

    private bool __isOccupied;
    public bool isOccupied { get { return __isOccupied; } set { __isOccupied = value; if (value) CheckIfAllOccupied(); } }
    
    void Start()
    {
        places.Add(this);
    }

    void OnDestroy()
    {
        places.Remove(this);
    }

    public void CheckIfAllOccupied()
    {
        foreach (Place place in places)
            if (!place.__isOccupied)
                return;
        Debug.Log("Game has been finished!");
        Level.current.Win();
    }
}
