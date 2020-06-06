using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    static List<Place> places = new List<Place>();

    private bool __isOccupied;
    public bool isOccupied { set { if (value == true) CheckIfAllOccupied(); __isOccupied = value; } }
    
    void Start()
    {
        places.Add(this);
    }

    void OnDestroy()
    {
        places.Remove(this);
    }

    public bool CheckIfAllOccupied()
    {
        foreach (Place place in places)
            if (!place.__isOccupied)
                return false;
        return true;
    }
}
