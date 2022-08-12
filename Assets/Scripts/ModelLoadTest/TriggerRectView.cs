using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRectView : MonoBehaviour
{
    public bool sceneTest = false;
    
    public Bounds currentBounds ;

    public Color lineColor = Color.cyan;
    //画线用的材质球
    Material lineMat;

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        if (lineMat == null)
        {
            Material Mat = Resources.Load<Material>("LineMat");
            lineMat = new Material(Mat);
            lineMat.color = lineColor;
        }

        if (sceneTest)
        {
            currentBounds = BoundsCalculator.CalculateBounds(transform, out bool foundCanvas, BoundsCalculator.BoundsCalculationMethod.RendererOverCollider,
                true, false);  
        }

        CalcBoxColliderPositons();
    }

    void OnRenderObject()
    {
        CalcBoxColliderPositons();
        lineMat.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(v3FrontTopLeft);
        GL.Vertex(v3FrontTopRight);

        GL.Vertex(v3FrontTopRight);
        GL.Vertex(v3FrontBottomRight);

        GL.Vertex(v3FrontBottomRight);
        GL.Vertex(v3FrontBottomLeft);

        GL.Vertex(v3FrontBottomLeft);
        GL.Vertex(v3FrontTopLeft);

        GL.Vertex(v3BackTopLeft);
        GL.Vertex(v3BackTopRight);

        GL.Vertex(v3BackTopRight);
        GL.Vertex(v3BackBottomRight);

        GL.Vertex(v3BackBottomRight);
        GL.Vertex(v3BackBottomLeft);

        GL.Vertex(v3BackBottomLeft);
        GL.Vertex(v3BackTopLeft);

        GL.Vertex(v3FrontTopLeft);
        GL.Vertex(v3BackTopLeft);

        GL.Vertex(v3FrontTopRight);
        GL.Vertex(v3BackTopRight);

        GL.Vertex(v3FrontBottomRight);
        GL.Vertex(v3BackBottomRight);

        GL.Vertex(v3FrontBottomLeft);
        GL.Vertex(v3BackBottomLeft);
        GL.End();
    }

    void CalcBoxColliderPositons()
    {
        //Bounds bounds = GetComponent<BoxCollider>().bounds;
        //Bounds bounds = currentBounds;

        Vector3 v3Center = currentBounds.center;
        ;// - transform.position; //此处减去GameObject本身坐标是为了在物体移动时，绘制的Box也跟着一起移动
        Vector3 v3Extents = currentBounds.extents;

        v3FrontTopLeft =
            new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y,
                v3Center.z - v3Extents.z); // Front top left corner
        v3FrontTopRight =
            new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y,
                v3Center.z - v3Extents.z); // Front top right corner
        v3FrontBottomLeft =
            new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y,
                v3Center.z - v3Extents.z); // Front bottom left corner
        v3FrontBottomRight =
            new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y,
                v3Center.z - v3Extents.z); // Front bottom right corner
        v3BackTopLeft =
            new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y,
                v3Center.z + v3Extents.z); // Back top left corner
        v3BackTopRight =
            new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y,
                v3Center.z + v3Extents.z); // Back top right corner
        v3BackBottomLeft =
            new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y,
                v3Center.z + v3Extents.z); // Back bottom left corner
        v3BackBottomRight =
            new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y,
                v3Center.z + v3Extents.z); // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
    }
}