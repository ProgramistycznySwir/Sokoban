using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_AnalyzeNormals : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < mesh.vertices.Length; i++)
            Debug.DrawLine(mesh.vertices[i], mesh.vertices[i] + mesh.normals[i], Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
