using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
    private void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        mesh.vertices = new Vector3[]
        {
            Vector3.zero, Vector3.right, Vector3.up, new Vector3(1f, 1f)
            //new Vector3(1.1f, 0f), new Vector3(0f, 1.1f), new Vector3(1.1f, 1.1f)
        };
        mesh.triangles = new int[]
        {
            //数字顺序会影响三角形的可视角度
            0, 2, 1, 1, 2, 3
        };
        mesh.normals = new Vector3[]
        {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back
            //Vector3.back, Vector3.back, Vector3.back
        };
        mesh.uv = new Vector2[]
        {
            Vector2.zero, Vector2.right, Vector2.up, Vector2.one
            //Vector2.right, Vector2.up, Vector2.one
            //Vector2.zero, Vector2.right, Vector2.one
        };
        mesh.tangents = new Vector4[]
        {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            //new Vector4(1f, 0f, 0f, -1f),
            //new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f)
        };
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
