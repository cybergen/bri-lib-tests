using UnityEngine;
using BriLib;

namespace BriLib.Tests
{
  public class QuadtreeTester : TextureWriteTester
  {
    public int PointSize;
    public Color PointColor;
    public Color BoundsColor;

    private Quadtree<EmptyPoint> _tree;

    private void Start()
    {
      Initialize();
    }

    protected override void OnMouseClick(int x, int y)
    {
      base.OnMouseClick(x, y);
      _tree.Insert(x, y, new EmptyPoint());
    }

    protected override void Initialize()
    {
      base.Initialize();
      _tree = new Quadtree<EmptyPoint>(Width / 2, Height / 2, Width / 2, 3);
      UpdateTexture();
    }

    protected override void UpdateTexture()
    {
      DrawBoundingBoxes();
      DrawPoints();
      base.UpdateTexture();
    }

    private void DrawBoundingBoxes()
    {
      foreach (var box in _tree.RecursiveBounds)
      {
        DrawBoundingBox(box);
      }
    }

    private void DrawPoints()
    {
      foreach (var point in _tree.GetPointRange(Width / 2, Height / 2, Width / 2))
      {
        DrawPoint((int)point.X, (int)point.Y, PointSize, PointColor);
      }
    }

    private void DrawBoundingBox(TwoDimensionalBoundingBox bounds)
    {
      var startX = bounds.X - bounds.Radius;
      var endX = bounds.X + bounds.Radius;
      var startY = bounds.Y - bounds.Radius;
      var endY = bounds.Y + bounds.Radius;

      for (var x = startX; x <= endX; x++)
      {
        for (var subX = -1; subX < 2; subX++)
        {
          _texture.SetPixel((int)x, (int)startY + subX, BoundsColor);
          _texture.SetPixel((int)x, (int)endY + subX, BoundsColor);
        }
      }

      for (var y = startY; y <= endY; y++)
      {
        for (var subY = -1; subY < 2; subY++)
        {
          _texture.SetPixel((int)startX + subY, (int)y, BoundsColor);
          _texture.SetPixel((int)endX + subY, (int)y, BoundsColor);
        }
      }
    }

    private void OnGUI()
    {
      var startX = Screen.width / 2 - 50;
      if (GUI.Button(new Rect(startX, 20, 100, 30), "Initialize"))
      {
        Initialize();
      }
    }

    private class EmptyPoint { }
  }
}
