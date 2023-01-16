using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject buildingsInWorld;
    public GameObject building;

    List<GameObject> buildingList = new List<GameObject>();
    GameObject[] buildingsArray;

    int buildingsAmount = 5;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            buildingList.Add(Instantiate<GameObject>(building));
            buildingsArray = buildingList.ToArray();
            buildingsArray[i].transform.position = new Vector3(Random.Range(16, 19), 5, Random.Range(-25, 5));
            buildingsArray[i].transform.parent = buildingsInWorld.transform;
        }
    }
}
