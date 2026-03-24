
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class mirror : MonoBehaviour
{
    public LayerMask Layers;
    public float rotation;
    List<fromTo> dots;
    Material newMat;
    public Color color;
    private void Start()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        newMat = new Material(mat);
        GetComponent<MeshRenderer>().material = newMat;
        newMat.color = color;
    }
    public void Update()
    {
        transform.Find("model").rotation = Quaternion.Euler(0,0,rotation);
        if (dots != null)
        {
            if (dots.Count > 0)
            {
                newMat.color = color;
                List<Vector2> dotss = new List<Vector2>();
                for (int i = 0; i < dots.Count - 1; i++)
                {
                    dotss.Add(dots[i].from - (Vector2)transform.position);
                    dotss.Add(dots[i].to - (Vector2)transform.position);
                    dotss.Add(dots[i+1].to - (Vector2)transform.position);
                    dotss.Add(dots[i].from - (Vector2)transform.position);
                    dotss.Add(dots[i+1].from - (Vector2)transform.position);
                    dotss.Add(dots[i+1].to - (Vector2)transform.position);
                }
                DrawTriangles(dotss);







            }
            else
            {
                newMat.color = Color.clear;
            }
        }
        else
        {
            newMat.color = Color.clear;
        }
    }

    public void AddVisualDot(List<fromToDistance> ftd)
    {
        dots = new List<fromTo>();
        if (ftd != null)
        {
            List<fromToDistance> mirrorList = new List<fromToDistance>();
            
            foreach (fromToDistance f in ftd)
            {
                
                Vector2 visualDot = Vector2.zero;
                Vector2 dif = f.to - f.from;
                RaycastHit2D hit;
                Vector2 direction = Vector2.one;
                if (rotation == 90)
                {
                    direction = new Vector2(-dif.x, dif.y).normalized;
                }
                else if(rotation ==0)
                {
                    direction = new Vector2(dif.x, -dif.y).normalized;
                }
                else if (rotation == 45)
                {
                    direction = new Vector2(dif.y, dif.x).normalized;
                }
                else if(rotation == 135)
                {
                    direction = new Vector2(-dif.y, -dif.x).normalized;
                }
                hit = Physics2D.Raycast(f.to, direction, f.distance, Layers);
                if (hit)
                {
                    visualDot = hit.point;
                    objects o;
                    bossBody bo;
                    if (hit.collider.TryGetComponent<objects>(out o))
                    {
                        o.open();
                    }
                    if (hit.collider.TryGetComponent<bossBody>(out bo))
                    {
                        bo.open();
                    }
                }
                else
                {
                    visualDot = f.to + direction * f.distance;
                }

                dots.Add(new fromTo(f.to, visualDot));
            }
        }
    }
    void DrawTriangles(List<Vector2> positions)
    {
        if (positions.Count % 3 != 0)
        {
            Debug.LogError("Noktaların sayısı 3'ün katı olmalı! (Her üç nokta bir üçgen oluşturur)");
            return;
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
    }


}

public class fromToDistance
{
    public Vector2 from;
    public Vector2 to;
    public float distance;
    public fromToDistance(Vector2 from, Vector2 to, float distance)
    {
        this.from = from;
        this.to = to;
        this.distance = distance;
    }
}

public class fromTo
{
    public Vector2 from;
    public Vector2 to;
    public fromTo(Vector2 from, Vector2 to)
    {
        this.from = from;
        this.to = to;
    }
}