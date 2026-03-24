
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class light : MonoBehaviour
{
    public LayerMask Layers;
    public Vector2[] poses;
    [Range(0f, 360f)]
    public float degree;
    public int quality;
    int posesCount;
    public int distance;
    public Color color;
    public string sortingLayerName = "Default";
    public int sortingOrder = 0;
    Material newMat;
    mirror mRef = null;
    private void Start() 
    {
        Material mat = GetComponent<MeshRenderer>().material;
        newMat = new Material(mat);
        GetComponent<MeshRenderer>().material = newMat;
        newMat.color = color;
    }
    void Update()
    {
        degree = (degree%(2*quality) == 0)? degree : (int)(degree / (2 * quality)) * (2 * quality);
        float baseRot = (360-degree)/2 - 180;
        newMat.color = color;
        posesCount = (posesCount %2 == 0) ? (int)(degree / quality): (int)(degree / quality)-1;
        poses = new Vector2[posesCount];
        GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        List<fromToDistance> mirrorList = new List<fromToDistance>();
        
        for (int i = 0; i < posesCount; i++)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, myMath.DirFromRot((quality * i ) + transform.localEulerAngles.z + baseRot), distance, Layers);
            if (hit)
            {
                if (hit.collider.tag == "Mirror")
                {
                    if (mRef != null)
                    {
                        if (mRef != hit.collider.transform.parent.GetComponent<mirror>())
                        {
                            mRef.AddVisualDot(null);
                            mRef = null;
                        }
                    }
                    mRef = hit.collider.transform.parent.GetComponent<mirror>();
                    float lastDist = distance - Vector2.Distance(transform.position, hit.point);
                    mirrorList.Add(new fromToDistance(transform.position, hit.point, lastDist));
                }
                

                objects o;
                if (hit.collider.TryGetComponent<objects>(out o))
                {
                    o.open();
                }
                poses[i] = hit.point - (Vector2)transform.position;
            }
            else
            {
                poses[i] =  myMath.DirFromRot((quality * i) + transform.localEulerAngles.z + baseRot) * distance;
            }

            
            
        }
        if (mRef != null && mirrorList.Count == 0)
        {
            mRef.AddVisualDot(null);
            mRef = null;
        }
        if (mRef != null)
        {
            mRef.AddVisualDot(mirrorList);
        }
        List<Vector2> positions = new List<Vector2>(0);
        if (poses != null)
        {
            for (int i = 0; i < posesCount-1; i++)
            {
                positions.Add(Vector2.zero);
                positions.Add(poses[i]);
                positions.Add(poses[i+1]);
            }
        }
        DrawTriangles(positions);
    }

    void DrawTriangles(List<Vector2> positions)
    {
        if (positions.Count % 3 != 0)
        {
            Debug.LogError("Noktaların sayısı 3'ün katı olmalı! (Her üç nokta bir üçgen oluşturur)");
            return;
        }
        if (degree == 360)
        {
            positions.Add(poses[0]);
            positions.Add(Vector2.zero);
            positions.Add(poses[poses.Length-1]);
        }
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[positions.Count];
        int[] triangles = new int[positions.Count];

        for (int i = 0; i < positions.Count; i++)
        {
            vertices[i] = new Vector3(positions[i].x, positions[i].y, 0);
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        RotateMesh(-transform.localEulerAngles.z);
    }
    void RotateMesh(float angle)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Quaternion rotation = Quaternion.Euler(0, 0, angle); // Z ekseninde döndür

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation * vertices[i]; // Her vertexi döndür
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = mesh;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (poses != null)
        {
            for (int i = 1; i < poses.Length; i++)
            {
                Gizmos.DrawLine(poses[i - 1] + (Vector2)transform.position, poses[i] + (Vector2)transform.position);
            }
        }
    }
}
