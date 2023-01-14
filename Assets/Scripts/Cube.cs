using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Cube : MonoBehaviour
{
    public Material newMaterialRef;

    [SerializeField]
    private Vector3 triangleSize = Vector3.one;

    [SerializeField]
    private int submeshCount = 6;

    [SerializeField]
    private int submeshTopIndex = 0;
    
     [SerializeField]
    private int submeshBottomIndex = 1;
    
     [SerializeField]
    private int submeshBackIndex = 2;
    
     [SerializeField]
    private int submeshLefttIndex = 3;
        
     [SerializeField]    
    private int submeshRightIndex = 4;
        
     [SerializeField]
    private int submeshFrontIndex = 5;

    // Start is called before the first frame update
    void Start()
    {
        CreateCube(); 
    }

    public Vector3 CubeSize(){
        return triangleSize * 2;
    }

    private void CreateCube(){
        
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshBuilder meshBuilder  = new MeshBuilder(submeshCount);

        //------- Points -------

        // TOP
        Vector3 t0 = new Vector3(triangleSize.x, triangleSize.y, -triangleSize.z);
        Vector3 t1 = new Vector3(-triangleSize.x, triangleSize.y, -triangleSize.z);
        Vector3 t2 = new Vector3(-triangleSize.x, triangleSize.y, triangleSize.z);
        Vector3 t3 = new Vector3(triangleSize.x, triangleSize.y, triangleSize.z);

        // BOTTOM
        Vector3 b0 = new Vector3(triangleSize.x, -triangleSize.y, -triangleSize.z);
        Vector3 b1 = new Vector3(-triangleSize.x, -triangleSize.y, -triangleSize.z);
        Vector3 b2 = new Vector3(-triangleSize.x, -triangleSize.y, triangleSize.z);
        Vector3 b3 = new Vector3(triangleSize.x, -triangleSize.y, triangleSize.z);


        //-------Triangles -------

        //TOP SQUARE
        meshBuilder.TriangleBuilder(t0,t1,t2, submeshTopIndex);
        meshBuilder.TriangleBuilder(t0,t2,t3, submeshTopIndex);

        //BOTTOM SQUARE
        meshBuilder.TriangleBuilder(b2,b1,b0, submeshBottomIndex);
        meshBuilder.TriangleBuilder(b3,b2,b0, submeshBottomIndex);

        //BACK SQUARE
        meshBuilder.TriangleBuilder(b0, t1, t0, submeshBackIndex);
        meshBuilder.TriangleBuilder(b0, b1, t1, submeshBackIndex);

        //LEFT-SIDE SQUARE
        meshBuilder.TriangleBuilder(b1, t2, t1, submeshLefttIndex);
        meshBuilder.TriangleBuilder(b1, b2, t2, submeshLefttIndex);

        //RIGHT-SIDE SQUARE
        meshBuilder.TriangleBuilder(b2, t3, t2, submeshRightIndex);
        meshBuilder.TriangleBuilder(b2, b3, t3, submeshRightIndex);

        //FRONT SQUARE
        meshBuilder.TriangleBuilder(b3,t0,t3, submeshFrontIndex);
        meshBuilder.TriangleBuilder(b3,b0,t0, submeshFrontIndex);

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        GetComponent<Renderer>().material = newMaterialRef;
    }

}
