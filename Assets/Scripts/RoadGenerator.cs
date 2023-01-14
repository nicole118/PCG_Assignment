using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public float roadWidth = 10;
    public float curveAmount = 1;
    public int numLanes = 2;

    void Start()
    {
        // Generate a noise map with a given width and height
        float[,] noiseMap = Noise.GenerateNoiseMap(256, 256, seed: 0, scale: 1, octaves: 1, persistance: 1, lacunarity: 1);

        // Create a smooth spline curve using the noise map
        Vector3[] curvePoints = CreateCurve(noiseMap, curveAmount);

        // Use the curve to generate a road mesh
        Mesh roadMesh = MarchingSquares.CreateMesh(curvePoints, roadWidth, numLanes);

        // Add the road mesh to a new GameObject
        GameObject road = new GameObject("Road");
        MeshFilter mf = road.AddComponent<MeshFilter>();
        mf.mesh = roadMesh;
    }

    Vector3[] CreateCurve(float[,] noiseMap, float curveAmount)
    {
        // Create an array of points that follow the noise map
        Vector3[] points = new Vector3[noiseMap.GetLength(0)];
        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            points[x] = new Vector3(x, noiseMap[x, 0] * curveAmount, 0);
        }

        // Use a spline curve to smooth out the points
        return Spline.Curve(points, smoothness: 0.5f);
    }
}


// Utility class for generating noise maps
public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[width, height];

        // Use the given seed for the random number generator
        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            // Generate random offsets for each octave
            float offsetX = rng.Next(-100000, 100000) + rng.Next();
            float offsetY = rng.Next(-100000, 100000) + rng.Next();
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        // Generate the noise map
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    // Sample noise at different frequencies and amplitudes
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    // Increase frequency and decrease amplitude for each successive octave
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // Keep track of the min and max noise values
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize the noise map to a range of 0-1
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}

// Utility class for creating smooth spline curves
public static class Spline
{
    public static Vector3[] Curve(Vector3[] points, float smoothness)
    {
        Vector3[] curvePoints = new Vector3[points.Length * 2 - 2];
        for (int i = 0; i < points.Length - 1; i++)
        {
            // Set the start point for the curve
            curvePoints[i * 2] = points[i];

            // Calculate the control point for the curve
            Vector3 controlPoint = (points[i] + points[i + 1]) / 2;
            controlPoint += (controlPoint - (points[i] + controlPoint) / 2) * smoothness;
            curvePoints[i * 2 + 1] = controlPoint;
        }

        // Set the end point for the curve
        curvePoints[curvePoints.Length - 1] = points[points.Length - 1];

        return curvePoints;
    }
}


// Utility class for generating meshes using the Marching Squares algorithm
public static class MarchingSquares
{
    public static Mesh CreateMesh(Vector3[] points, float roadWidth, int numLanes)
    {
        // Calculate the number of vertices and triangles needed for the mesh
        int numVertices = points.Length * 2 + 2;
        int numTriangles = (points.Length - 1) * 2 * 3;

        // Create arrays for the vertices, triangles, and UV coordinates of the mesh
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];
        Vector2[] uv = new Vector2[numVertices];

        // Set the starting vertex and UV coordinates for the mesh
        vertices[0] = points[0] - Vector3.right * roadWidth / 2;
        uv[0] = Vector2.zero;

        // Set the other vertices and UV coordinates for the mesh
        for (int i = 0; i < points.Length - 1; i++)
        {
            // Set the left and right vertex for each point
            vertices[i * 2 + 1] = points[i] - Vector3.right * roadWidth / 2;
            vertices[i * 2 + 2] = points[i] + Vector3.right * roadWidth / 2;

            // Set the UV coordinates for each vertex
            uv[i * 2 + 1] = new Vector2((float)i / (points.Length - 1), 0);
            uv[i * 2 + 2] = new Vector2((float)i / (points.Length - 1), 1);

            // Set the triangles for the left and right sides of the road
            triangles[i * 6] = i * 2 + 1;
            triangles[i * 6 + 1] = i * 2 + 2;
            triangles[i * 6 + 2] = i * 2 + 3;
            triangles[i * 6 + 3] = i * 2 + 1;
            triangles[i * 6 + 4] = i * 2 + 3;
            triangles[i * 6 + 5] = i * 2 + 2;
        }

        // Set the ending vertex and UV coordinates for the mesh
        vertices[vertices.Length - 1] = points[points.Length - 1] - Vector3.right * roadWidth / 2;
        uv[uv.Length - 1] = Vector2.one;

        // Set the triangles for the bottom of the road
        for (int i = 0; i < points.Length - 1; i++)
        {
            triangles[numTriangles - i * 6 - 1] = vertices.Length - 1;
            triangles[numTriangles - i * 6 - 2] = vertices.Length - 2 - i * 2;
            triangles[numTriangles - i * 6 - 3] = vertices.Length - 3 - i * 2;
        }

        // Create a new mesh and set the vertices, triangles, and UV coordinates
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // Calculate the normals for the mesh
        mesh.RecalculateNormals();

        // Create a new GameObject for the road
        GameObject road = new GameObject("Road");

        // Add a MeshFilter component to the road and set the mesh to the one we created
        MeshFilter meshFilter = road.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Add a MeshRenderer component to the road and set the material to a default white material
        MeshRenderer meshRenderer = road.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Return the mesh
        return mesh;
    }
}

