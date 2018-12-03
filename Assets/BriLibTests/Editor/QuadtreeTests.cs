using System.Linq;
using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  public class QuadtreeTests
  {
    private Quadtree<TestObject> _tree;
    private TwoDimensionalBoundingBox _box;
    private TestObject _fiveFiveObject = new TestObject();
    private TestObject _twoTwoObject = new TestObject();
    private TestObject _threeThreeObject = new TestObject();
    private TestObject _fourFourObject = new TestObject();
    private TestObject _fifteenFifteenObject = new TestObject();
    private TestObject _twentyFiveObject = new TestObject();
    private TestObject _negativeOneObject = new TestObject();

    [SetUp]
    public void Setup()
    {
      _box = new TwoDimensionalBoundingBox(10, 10, 10);
      _tree = new Quadtree<TestObject>(_box, 2);
    }

    [Test]
    public void GetOneObjectInRange()
    {
      _tree.Insert(5, 5, _fiveFiveObject);
      var objects = _tree.GetRange(_box).ToList();
      Assert.AreEqual(1, objects.Count());
      Assert.AreEqual(_fiveFiveObject, objects[0]);
    }

    [Test]
    public void GetOneObjectInSubRange()
    {
      _tree.Insert(5, 5, _fiveFiveObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(3, 3, 5)).ToList();
      Assert.AreEqual(1, objects.Count());
      Assert.AreEqual(_fiveFiveObject, objects[0]);
    }

    [Test]
    public void GetNoObjectsInRange()
    {
      _tree.Insert(-1, -1, _negativeOneObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(3, 3, 2)).ToList();
      Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void GetNoObjectsInSubRange()
    {
      _tree.Insert(5, 5, _fiveFiveObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(15, 15, 5)).ToList();
      Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void GetObjectsAfterSubDivide()
    {
      _tree.Insert(4, 4, _fourFourObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(3, objects.Count);
      Assert.True(objects.Contains(_fourFourObject));
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void GetObjectsAcrossMultipleZonesAfterSubdivide()
    {
      _tree.Insert(15, 15, _fifteenFifteenObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var objects = _tree.GetRange(_box).ToList();
      Assert.AreEqual(3, objects.Count);
      Assert.True(objects.Contains(_fifteenFifteenObject));
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void ObjectsAfterSubDivideExcludeSome()
    {
      _tree.Insert(15, 15, _fifteenFifteenObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(2, objects.Count);
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void RemoveObject()
    {
      _tree.Insert(5, 5, _fiveFiveObject);
      var objects = _tree.GetRange(_box).ToList();
      Assert.AreEqual(1, objects.Count());
      Assert.AreEqual(_fiveFiveObject, objects[0]);
      _tree.Remove(_fiveFiveObject);
      objects = _tree.GetRange(_box).ToList();
      Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void RemoveObjectAfterSubdivide()
    {
      _tree.Insert(4, 4, _fourFourObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(3, objects.Count);
      Assert.True(objects.Contains(_fourFourObject));
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
      _tree.Remove(_fourFourObject);
      objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(2, objects.Count);
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void RemoveCausingSubdivideRemovalThenAdd()
    {
      _tree.Insert(4, 4, _fourFourObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(3, objects.Count);
      Assert.True(objects.Contains(_fourFourObject));
      Assert.True(objects.Contains(_twoTwoObject));
      Assert.True(objects.Contains(_threeThreeObject));
      _tree.Remove(_fourFourObject);
      _tree.Remove(_twoTwoObject);
      _tree.Remove(_threeThreeObject);
      objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(0, objects.Count);
      _tree.Insert(4, 4, _fourFourObject);
      objects = _tree.GetRange(new TwoDimensionalBoundingBox(2.5f, 2.5f, 2.5f)).ToList();
      Assert.AreEqual(1, objects.Count);
      Assert.True(objects.Contains(_fourFourObject));
    }

    [Test]
    public void GetSingleObject()
    {
      _tree.Insert(4, 4, _fourFourObject);
      var neighbor = _tree.GetNearestObject(2, 2);
      Assert.AreEqual(neighbor, _fourFourObject);
    }

    [Test]
    public void GetNearestOutOfTwo()
    {
      _tree.Insert(4, 4, _fourFourObject);
      _tree.Insert(2, 2, _twoTwoObject);
      var neighbor = _tree.GetNearestObject(5, 5);
      Assert.AreEqual(neighbor, _fourFourObject);
    }

    [Test]
    public void GetNearestOutOfMany()
    {
      _tree.Insert(15, 15, _fifteenFifteenObject);
      _tree.Insert(5, 5, _fiveFiveObject);
      _tree.Insert(4, 4, _fourFourObject);
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(3, 3, _threeThreeObject);
      var neighbor = _tree.GetNearestObject(5, 5);
      Assert.AreEqual(neighbor, _fiveFiveObject);
    }

    [Test]
    public void GetNearestWithSeparateOctantMostProximalPoint()
    {
      var _nineFourPoint = new TestObject();
      var _sixNinePoint = new TestObject();
      _tree.Insert(2, 2, _twoTwoObject);
      _tree.Insert(6, 9, _sixNinePoint);
      _tree.Insert(9, 4, _nineFourPoint);
      var neighbor = _tree.GetNearestObject(9, 7);
      Assert.AreEqual(_nineFourPoint, neighbor);
    }

    private class TestObject { }
  }
}
