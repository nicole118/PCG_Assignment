using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public TerrainGenerator terrain;
    public GameObject objectsInWorld;

    public GameObject tree;
    public GameObject plant;
    public GameObject flower;

    int treesAmount = 20;
    int plantsAmount = 40;
    int flowersAmount = 8;

    List<GameObject> treesList = new List<GameObject>();
    GameObject[] treesArray;

    List<GameObject> plantsList = new List<GameObject>();
    GameObject[] plantsArray;

    List<GameObject> flowerList = new List<GameObject>();
    GameObject[] flowerArray;

    List<Vector3> vertices = new List<Vector3>();
    public void createTrees()
    {
        for (int i = 0; i <= treesAmount; i++)
        {
            vertices = terrain.vertices.ToList();
            Vector3 vertex = vertices[Random.Range(0, vertices.Count)]; // get a random vertex from the vertices list

            treesList.Add(Instantiate<GameObject>(tree)); 
            treesArray = treesList.ToArray();
            treesArray[i].transform.position = new Vector3(vertex.x, vertex.y, vertex.z);
            treesArray[i].transform.parent = objectsInWorld.transform;
        }
    }

    public void createPlants()
    {
        for (int i = 0; i <= plantsAmount; i++)
        {
            vertices = terrain.vertices.ToList();
            Vector3 vertex = vertices[Random.Range(0, vertices.Count)]; // get a random vertex from the vertices list

            plantsList.Add(Instantiate<GameObject>(plant));
            plantsArray = plantsList.ToArray();
            plantsArray[i].transform.position = new Vector3(vertex.x, vertex.y, vertex.z);
            plantsArray[i].transform.parent = objectsInWorld.transform;
        }
    }

    public void createFlowers()
    {
        for (int i = 0; i < flowersAmount; i++)
        {
            vertices = terrain.vertices.ToList();
            Vector3 vertex = vertices[Random.Range(0, vertices.Count)]; // get a random vertex from the vertices list

            flowerList.Add(Instantiate<GameObject>(flower));
            flowerArray = flowerList.ToArray();
            flowerArray[i].transform.position = new Vector3(vertex.x +5f, vertex.y, vertex.z -3f);
            flowerArray[i].transform.parent = objectsInWorld.transform;
        }
    }
}
