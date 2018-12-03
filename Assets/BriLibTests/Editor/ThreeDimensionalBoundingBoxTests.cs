using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  public class ThreeDimensionalBoundingBoxTests
  {
    private ThreeDimensionalBoundingBox _box;

    [SetUp]
    public void Setup()
    {
      _box = new ThreeDimensionalBoundingBox(10, 10, 10, 10);
    }

    [Test]
    public void BoundsPointIntersection3D()
    {
      Assert.True(_box.Intersects(5, 5, 5));
      Assert.True(_box.Intersects(15, 15, 15));
    }

    [Test]
    public void BoundsPointNonIntersection3D()
    {
      Assert.False(_box.Intersects(-1, -1, -1));
      Assert.False(_box.Intersects(21, 21, 21));
    }

    [Test]
    public void BoundsBoundsEdgeIntersection3D()
    {
      var newBox = new ThreeDimensionalBoundingBox(15, 15, 15, 10);
      Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsInnerIntersection3D()
    {
      var newBox = new ThreeDimensionalBoundingBox(11, 11, 11, 2);
      Assert.True(_box.Intersects(newBox));
      Assert.True(newBox.Intersects(_box));
    }

    [Test]
    public void BoundsBoundsNonIntersections3D()
    {
      var newBox = new ThreeDimensionalBoundingBox(-5, -5, -5, 3);
      Assert.False(_box.Intersects(newBox));
    }

    [Test]
    public void DistanceAlongStraightPath3D()
    {
      var newBox = new ThreeDimensionalBoundingBox(2, 2, 2, 2);
      var dist = newBox.BoundsDistance(6, 2, 2);
      Assert.AreEqual(2, dist);
    }

    [Test]
    public void DistancePointInside3D()
    {
      var newBox = new ThreeDimensionalBoundingBox(2, 2, 2, 2);
      var dist = newBox.BoundsDistance(2, 4, 4);
      Assert.AreEqual(-2, dist);
    }

    [Test]
    public void DistanceFlatPlaneXZPlane3D()
    {
      var hypotenuse = (float)(2f.Sq() + 2d.Sq()).Sqrt();
      var newBox = new ThreeDimensionalBoundingBox(2, 2, 2, 2);
      var dist = newBox.BoundsDistance(2, 6, 6);
      Assert.AreEqual(hypotenuse, dist);
    }

    [Test]
    public void DistanceFlatPlaneYZPlane3D()
    {
      var hypotenuse = (float)(2f.Sq() + 2d.Sq()).Sqrt();
      var newBox = new ThreeDimensionalBoundingBox(2, 2, 2, 2);
      var dist = newBox.BoundsDistance(6, 2, 6);
      Assert.AreEqual(hypotenuse, dist);
    }

    [Test]
    public void DistanceAlongDiagonalYXPlane3D()
    {
      var hypotenuse = (float)(2f.Sq() + 2d.Sq()).Sqrt();
      var newBox = new ThreeDimensionalBoundingBox(2, 2, 2, 2);
      var dist = newBox.BoundsDistance(6, 6, 2);
      Assert.AreEqual(hypotenuse, dist);
    }

    [Test]
    public void ProblemPoints()
    {
      var box = new ThreeDimensionalBoundingBox(-0.7f, 0.043f, -0.762f, 3f);
      Assert.True(box.Intersects(0.278f, 0.3895f, -0.558f));
      Assert.True(box.Intersects(0.2765f, 0.1045f, 0.5804999f));
      Assert.True(box.Intersects(0.233f, 1.4965f, 0.5249999f));
    }

    [Test]
    public void ProblemBoxes()
    {
      var box = new ThreeDimensionalBoundingBox(-0.7f, 0.043f, -0.762f, 3f);

      var badBoxOne = new ThreeDimensionalBoundingBox(0.625f, 1.875f, 0.625f, 0.625f);
      Assert.True(box.Intersects(badBoxOne));
      Assert.True(badBoxOne.Intersects(box));

      var badBoxTwo = new ThreeDimensionalBoundingBox(5f, 5f, -5f, 5f);
      Assert.True(box.Intersects(badBoxTwo));
      Assert.True(badBoxTwo.Intersects(box));

      var badBoxThree = new ThreeDimensionalBoundingBox(0.625f, 0.625f, 0.625f, 0.625f);
      Assert.True(box.Intersects(badBoxThree));
      Assert.True(badBoxThree.Intersects(box));

      var badBoxFour = new ThreeDimensionalBoundingBox(0, 0, 0, 10);
      Assert.True(box.Intersects(badBoxFour));
      Assert.True(badBoxFour.Intersects(box));

      var badBoxFive = new ThreeDimensionalBoundingBox(5, 5, 5, 5);
      Assert.True(box.Intersects(badBoxFive));
      Assert.True(badBoxFive.Intersects(box));

      var badBoxSix = new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f);
      Assert.True(box.Intersects(badBoxSix));
      Assert.True(badBoxSix.Intersects(box));

      var badBoxSeven = new ThreeDimensionalBoundingBox(1.25f, 1.25f, 1.25f, 1.25f);
      Assert.True(box.Intersects(badBoxSeven));
      Assert.True(badBoxSeven.Intersects(box));
    }
  }
}
