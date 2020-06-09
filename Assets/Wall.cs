using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    /// <summary>
    /// Height of the wall model
    /// </summary>
    // To musi być readonly bo struktury nie mogą mieć prefixu "const".
    public static readonly Vector2 wallHeightRange = new Vector2(0.2f, 1f);
    static float __wallHeight = 1f;
    public static float wallHeight { get { return __wallHeight; } set { __wallHeight = Mathf.Clamp(value, wallHeightRange.x, wallHeightRange.y); } }

    public MeshFilter meshFilter;
    public new MeshCollider collider;
    
    

    // Mógłbym w tym miejscu posilić się o greedy-mesh, ale sądzę że nawet jeśli będą obecne zbędne współliniowe wierzchołki to dalej będzie to
    // bardziej optymalne od tego żeby każda ściana była odrębnym obiektem.
    // Update: o ile współliniowe wierzchołki chyba nikomu nie przeszkadzają, ale takie rzeczy jak kilka vertexów w tym samym miejscu już 
    // robią, że oświetlenie obiektu szaleje ;/
    // Update: udało mi się jako tako naprawić oświetlenie i jest... ummm... stylistyczne? Znaczy no, nie jest źle, ale niektóre krawędzie
    // szczególnie te dobrze doświetlone są troche rozmazane.
    // Update: ...tyle zachodu na marne... okazuje się że Unity do każdej ściany wykorzystuje nowe vertex'y (ew w przypadku współpłaszczyźnianych je dzieli)
    public void GenerateWalls(char[,] map)
    {
        Mesh mesh = new Mesh();
        
        // Vertex'y - punkty na których później się "rozpina" trójkąty z których składa się siatka obiektu
        List<Vector3> verts = new List<Vector3>();

        // Pierwszy submesh - podłoga
        List<int> tris0 = new List<int>();
        // Drugi submesh - ściany
        List<int> tris1 = new List<int>();

        // Normal'e - kierunki w które skierowane są powierzchnie trójkątów, potrzebne są do odpowiedniego oświetlenia obiektu
        //List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        
        // Dla wygody
        Vector2Int mapSize = new Vector2Int(map.GetUpperBound(1), map.GetUpperBound(0));
        
        int vertsCount = 0;
        for (int y = 0; y <= mapSize.y; y++)
            for (int x = 0; x <= mapSize.x; x++)
            {
                if (map[y, x] == 'O')
                {
                    // Górna
                    verts.Add(new Vector3(x, wallHeight, -y)); // 0
                    verts.Add(new Vector3(x, wallHeight, -y + 1)); // 1
                    verts.Add(new Vector3(x + 1, wallHeight, -y + 1)); // 2
                    verts.Add(new Vector3(x + 1, wallHeight, -y)); // 3

                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0, 1));
                    uvs.Add(new Vector2(1, 1));
                    uvs.Add(new Vector2(1, 0));

                    tris1.Add(vertsCount);
                    tris1.Add(vertsCount + 1);
                    tris1.Add(vertsCount + 2);

                    tris1.Add(vertsCount + 2);
                    tris1.Add(vertsCount + 3);
                    tris1.Add(vertsCount);

                    vertsCount += 4;

                    // Przednia
                    if (y == mapSize.y || map[y + 1, x] != 'O')
                    {
                        verts.Add(new Vector3(x, wallHeight, -y)); // 0
                        verts.Add(new Vector3(x + 1, wallHeight, -y)); // 3
                        verts.Add(new Vector3(x + 1, 0, -y)); // 7
                        verts.Add(new Vector3(x, 0, -y)); // 4

                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(1, 0));

                        tris1.Add(vertsCount);
                        tris1.Add(vertsCount + 1);
                        tris1.Add(vertsCount + 2);

                        tris1.Add(vertsCount + 2);
                        tris1.Add(vertsCount + 3);
                        tris1.Add(vertsCount);

                        vertsCount += 4;
                    }

                    // Tylna
                    if (y == 0 || map[y - 1, x] != 'O')
                    {
                        verts.Add(new Vector3(x + 1, wallHeight, -y + 1)); // 2
                        verts.Add(new Vector3(x, wallHeight, -y + 1)); // 1
                        verts.Add(new Vector3(x, 0, -y + 1)); // 5
                        verts.Add(new Vector3(x + 1, 0, -y + 1)); // 6

                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(1, 0));

                        tris1.Add(vertsCount);
                        tris1.Add(vertsCount + 1);
                        tris1.Add(vertsCount + 2);

                        tris1.Add(vertsCount + 2);
                        tris1.Add(vertsCount + 3);
                        tris1.Add(vertsCount);

                        vertsCount += 4;
                    }

                    // Prawa
                    if (x == mapSize.x || map[y, x + 1] != 'O')
                    {
                        verts.Add(new Vector3(x + 1, wallHeight, -y)); // 3
                        verts.Add(new Vector3(x + 1, wallHeight, -y + 1)); // 2
                        verts.Add(new Vector3(x + 1, 0, -y + 1)); // 6
                        verts.Add(new Vector3(x + 1, 0, -y)); // 7

                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(1, 0));

                        tris1.Add(vertsCount);
                        tris1.Add(vertsCount + 1);
                        tris1.Add(vertsCount + 2);

                        tris1.Add(vertsCount + 2);
                        tris1.Add(vertsCount + 3);
                        tris1.Add(vertsCount);

                        vertsCount += 4;
                    }

                    // Lewa
                    if (x == 0 || map[y, x - 1] != 'O')
                    {
                        verts.Add(new Vector3(x, wallHeight, -y)); // 0
                        verts.Add(new Vector3(x, 0, -y)); // 4
                        verts.Add(new Vector3(x, 0, -y + 1)); // 5
                        verts.Add(new Vector3(x, wallHeight, -y + 1)); // 1

                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(1, 0));

                        tris1.Add(vertsCount);
                        tris1.Add(vertsCount + 1);
                        tris1.Add(vertsCount + 2);

                        tris1.Add(vertsCount + 2);
                        tris1.Add(vertsCount + 3);
                        tris1.Add(vertsCount);

                        vertsCount += 4;
                    }
                }
                else
                {
                    // Dolna
                    verts.Add(new Vector3(x, 0, -y)); // 4
                    verts.Add(new Vector3(x, 0, -y + 1)); // 5
                    verts.Add(new Vector3(x + 1, 0, -y + 1)); // 6
                    verts.Add(new Vector3(x + 1, 0, -y)); // 7

                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0, 1));
                    uvs.Add(new Vector2(1, 1));
                    uvs.Add(new Vector2(1, 0));

                    tris0.Add(vertsCount);
                    tris0.Add(vertsCount + 1);
                    tris0.Add(vertsCount + 2);

                    tris0.Add(vertsCount + 2);
                    tris0.Add(vertsCount + 3);
                    tris0.Add(vertsCount);

                    vertsCount += 4;
                }
            }

        // Aplikowanie wygenerowanej siatki
        mesh.subMeshCount = 2;
        mesh.vertices = verts.ToArray();
        mesh.SetTriangles(tris0, 0);
        mesh.SetTriangles(tris1, 1);

        // Dla oświetlenia
        mesh.RecalculateNormals();

        mesh.SetUVs(0, uvs);
        
        // Aplikowanie przygotowanej siatki do sceny
        meshFilter.mesh = mesh;
        // oraz do silnika fizycznego
        collider.sharedMesh = mesh;
    }
}