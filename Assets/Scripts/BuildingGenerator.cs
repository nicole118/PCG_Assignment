using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    float xPos, size;

    ArrayList buildingsList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            float randomx = Random.Range(0, 30); // Random generated X
            Building newBuilding = new Building(xPos, 0, size);

            buildingsList.Add(newBuilding);
        }
        building();

    }

    void building()
    {
        foreach (Building b in buildingsList)
        {
            b.CreateCube();
        }
    }
}
