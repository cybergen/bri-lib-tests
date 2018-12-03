using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BriLib;
using BriLib.Delaunay;
using System.Collections;

namespace BriLib.Tests
{
  public class ShatterTester : TextureWriteTester
  {
    public int VoronoiCount;
    public int IntersectionPointSize;
    public Color CircleColor;
    public Color IntersectionColor;
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    public float FragmentDisplaceDistance;
    public float EaseTime;

    private VoronoiDiagram _voronoi;
    private Triangle _initial;
    private Dictionary<Vector3, Color> _pointColors = new Dictionary<Vector3, Color>();
    private System.Random _rand = new System.Random();

    private void OnGUI()
    {
      var width = 100;
      var startY = Screen.height - 30;
      var height = 30;
      var gap = 20;
      var x = Screen.width / 2 - (gap * 4) - width * 3;

      if (GUI.Button(new Rect(x, startY, width, height), "Initialize"))
      {
        Initialize();
      }

      x += width + gap;
      if (GUI.Button(new Rect(x, startY, width, height), "Toggle Points"))
      {
        drawPoints = !drawPoints;
      }

      x += width + gap;
      if (GUI.Button(new Rect(x, startY, width, height), "Toggle Voronoi"))
      {
        drawVoronoi = !drawVoronoi;
      }

      x += width + gap;
      if (GUI.Button(new Rect(x, startY, width, height), "Toggle Delaunay"))
      {
        drawLines = !drawLines;
      }

      x += width + gap;
      if (GUI.Button(new Rect(x, startY, width, height), "Apply Voronoi to Texture"))
      {
        ApplyVoronoiToTexture();
      }

      x += width + gap;
      if (GUI.Button(new Rect(x, startY, width, height), "Shatter Mesh"))
      {
        SeparateMesh();
      }
    }

    protected override void OnMouseClickWorld(Vector3 point)
    {
      base.OnMouseClickWorld(point);

      if (_voronoi == null) return;

      var localPoint = transform.InverseTransformPoint(point);
      _voronoi.AddFacePoint(new Vector3(localPoint.x, localPoint.y, localPoint.z));
      _pointColors.Add(localPoint, GetRandomColor());
    }

    private Color GetRandomColor()
    {
      var r = _rand.Next(256) / 256f;
      var g = _rand.Next(256) / 256f;
      var b = _rand.Next(256) / 256f;
      return new Color(r, g, b);
    }

    protected override void Initialize()
    {
      base.Initialize();
      _voronoi = new VoronoiDiagram(MeshFilter, transform);
      _pointColors.Clear();

      ClearTris();
      ClearLines();
      ClearPoints();
      UpdateTexture();
    }

    protected override void UpdateTexture()
    {
      DrawBackground();
      DrawPoints();
      DrawVoronoi();
      DrawTriangles();
      base.UpdateTexture();
    }

    private void DrawVoronoi()
    {
      if (_voronoi == null) return;

      ClearTris();
      foreach (var cell in _voronoi.Cells)
      {
        var color = GetRandomColor();
        foreach (var tri in cell.Triangles)
        {
          var vectors = new[] { tri.Item1, tri.Item2, tri.Item3 };
          DrawTriangle(vectors, color);
        }
      }
    }

    private void DrawPoints()
    {
      if (_voronoi == null) return;
      ClearPoints();

      foreach (var point in _pointColors.Keys)
      {
        DrawGLPoint(point, _pointColors[point]);
      }
    }

    private void DrawTriangles()
    {
      if (_voronoi == null) return;
      ClearLines();

      foreach (var triangle in _voronoi.DelaunayTris)
      {
        Debug.Log("Got triangle: " + triangle);
        var enumerator = triangle.GetEnumerator();
        if (!enumerator.MoveNext()) continue;
        var old = enumerator.Current;
        var first = old;
        while (enumerator.MoveNext())
        {
          var newPoint = enumerator.Current;
          MakeDrawLine(old, newPoint);
          old = newPoint;
        }
        if (first != null && old != null)
        {
          MakeDrawLine(old, first);
        }
      }
    }

    private void MakeDrawLine(Pnt old, Pnt newPoint)
    {
      var oldPoint = transform.TransformPoint(new Vector3((float)old[0], 0.01f, (float)old[1]));
      var newP = transform.TransformPoint(new Vector3((float)newPoint[0], 0.01f, (float)newPoint[1]));
      DrawLine(oldPoint, newP);
    }

    private void ApplyVoronoiToTexture()
    {
      if (_voronoi == null) return;

      //for (int y = 0; y < Height; y++)
      //{
      //    for (int x = 0; x < Width; x++)
      //    {
      //        var color = _colorTree.GetNearestNeighbor((float)x, (float)y).Color;
      //        _texture.SetPixel(x, y, color);
      //    }
      //}
      //_texture.Apply();
    }

    private void SeparateMesh()
    {
      if (_voronoi == null) return;

      var minX = float.MaxValue;
      var maxX = float.MinValue;
      var minY = float.MaxValue;
      var maxY = float.MinValue;
      var minZ = float.MaxValue;
      var maxZ = float.MinValue;

      foreach (var vert in MeshFilter.mesh.vertices)
      {
        minX = Mathf.Min(minX, vert.x);
        maxX = Mathf.Max(maxX, vert.x);
        minY = Mathf.Min(minY, vert.y);
        maxY = Mathf.Max(maxY, vert.y);
        minZ = Mathf.Min(minZ, vert.z);
        maxZ = Mathf.Max(maxZ, vert.z);
      }

      Debug.Log("MinX: " + minX + ", MaxX: " + maxX + ", MinY: " + minY + ", MaxY: " + maxY + ", MinZ: " + minZ + ", MaxZ: " + maxZ);

      var frags = _voronoi.Fragments;
      foreach (var frag in frags)
      {
        //Copy this object
        var newGo = Instantiate(gameObject);
        newGo.GetComponent<ShatterTester>().enabled = false;
        var mesh = newGo.GetComponent<MeshFilter>().mesh;

        //Apply verts, tris, and uvs
        mesh.Clear();
        mesh.vertices = frag.Vertices;
        mesh.triangles = frag.Triangles;
        mesh.uv = frag.UVs;

        //Animate it outwards based on direction and magnitude from 
        var xDir = (float)(_rand.Next(200) - 100) / 100f;
        var yDir = (float)(_rand.Next(200) - 100) / 100f;
        var start = newGo.transform;
        StartCoroutine(BreakCoroutine(new Vector3(xDir, start.position.y, yDir), start, EaseTime));
      }
      Renderer.enabled = false;
    }

    private IEnumerator BreakCoroutine(Vector3 vector3, Transform start, float easeTime)
    {
      var startTime = Time.time;
      var endTime = Time.time + easeTime;
      var elapsed = 0f;
      var startPosition = start.position;

      while (elapsed < easeTime)
      {
        elapsed = Time.time - startTime;
        start.position = startPosition + Easing.ElasticEaseOut(elapsed / easeTime) * vector3 * FragmentDisplaceDistance;
        yield return null;
      }
    }

    private class ColorWrapper
    {
      public Color Color;
      public ColorWrapper(Color color) { Color = color; }
    }
  }
}
