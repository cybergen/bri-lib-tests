using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  public class TwoDimensionalBoundingBoxTests
  {
    private TwoDimensionalBoundingBox _box;

    [SetUp]
    public void Setup()
    {
      _box = new TwoDimensionalBoundingBox(10, 10, 10);
    }

    [Test]
    public void BoundsPointIntersection2D()
    {
      Assert.True(_box.Intersects(5, 5));
      Assert.True(_box.Intersects(15, 15));
    }

    [Test]
    public void BoundsPointNonIntersection2D()
    {
      Assert.False(_box.Intersects(-1, -1));
      Assert.False(_box.Intersects(21, 21));
    }

    [Test]
    public void BoundsBoundsEdgeIntersection2D()
    {
      var newBox = new TwoDimensionalBoundingBox(15, 15, 10);
      Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsInnerIntersection2D()
    {
      var newBox = new TwoDimensionalBoundingBox(11, 11, 2);
      Assert.True(_box.Intersects(newBox));
      Assert.True(newBox.Intersects(_box));
    }

    [Test]
    public void BoundsBoundsNonIntersections2D()
    {
      var newBox = new TwoDimensionalBoundingBox(-5, -5, 3);
      Assert.False(_box.Intersects(newBox));
    }

    [Test]
    public void DistanceAlongStraightPath()
    {
      var newBox = new TwoDimensionalBoundingBox(2, 2, 2);
      var dist = newBox.BoundsDistance(6, 2);
      Assert.AreEqual(2, dist);
    }

    [Test]
    public void DistanceAlongDiagonal()
    {
      var hypotenuse = (float)(2f.Sq() + 2d.Sq()).Sqrt();
      var newBox = new TwoDimensionalBoundingBox(2, 2, 2);
      var dist = newBox.BoundsDistance(6, 6);
      Assert.AreEqual(hypotenuse, dist);
    }

    [Test]
    public void DistancePointInside()
    {
      var newBox = new TwoDimensionalBoundingBox(2, 2, 2);
      var dist = newBox.BoundsDistance(2, 4);
      Assert.AreEqual(-2, dist);
    }
  }
}
