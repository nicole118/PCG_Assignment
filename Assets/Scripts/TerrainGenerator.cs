using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    int[] triangles;

    public TreeGenerator treeGenerator;
    public GameObject PlayerController;

    public Vector3[] vertices;
    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateShape();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                //height map
                float y = Mathf.PerlinNoise(x * .4f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        treeGenerator.createTrees();
        treeGenerator.createPlants();
        treeGenerator.createFlowers();

        GameObject player = Instantiate<GameObject>(PlayerController);
        player.transform.position = new Vector3(Random.Range(30, 50), 2, Random.Range(10, 20));

        //triangle array - depends on the size of the quads
        triangles = new int[xSize * zSize * 6];

        int vertex = 0;
        int tri = 0;

        //generating triangles
        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tri + 0] = vertex + 0;
                triangles[tri + 1] = vertex + xSize + 1;
                triangles[tri + 2] = vertex + 1;
                triangles[tri + 3] = vertex + 1;
                triangles[tri + 4] = vertex + xSize + 1;
                triangles[tri + 5] = vertex + xSize + 2;

                vertex++;
                tri += 6;
            }
            vertex++;
        }
    }

    void UpdateShape()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
