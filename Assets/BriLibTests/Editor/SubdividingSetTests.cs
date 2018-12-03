using System.Collections.Generic;
using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  public class SubdividingSetTests
  {
    private SubdividingSet<TestObject> _setList;
    private int _bucketSize = 2;
    private TestObject _objectOne = new TestObject();
    private TestObject _objectTwo = new TestObject();
    private TestObject _objectThree = new TestObject();
    private TestObject _objectFour = new TestObject();
    private TestObject _objectFive = new TestObject();

    [SetUp]
    public void Setup()
    {
      var list = new List<TestObject>();
      list.Add(_objectOne);
      list.Add(_objectTwo);
      _setList = new SubdividingSet<TestObject>(list, _bucketSize);
    }

    [Test]
    public void PointsInSetThatFitBucket()
    {
      Assert.AreEqual(2, _setList.Count);
      Assert.AreEqual(null, _setList.Next);
    }

    [Test]
    public void PointOrder()
    {
      Assert.AreEqual(_objectOne, _setList.Entries[0]);
      Assert.AreEqual(_objectTwo, _setList.Entries[1]);
    }

    [Test]
    public void SetSubdividesOnceBucketSizes()
    {
      var list = new List<TestObject>();
      list.Add(_objectOne);
      list.Add(_objectTwo);
      list.Add(_objectThree);
      _setList = new SubdividingSet<TestObject>(list, _bucketSize);

      Assert.AreEqual(1, _setList.Count);
      Assert.AreNotEqual(null, _setList.Next);
      Assert.AreEqual(2, _setList.Next.Count);
      Assert.AreEqual(_objectOne, _setList.Entries[0]);
      Assert.AreEqual(_objectTwo, _setList.Next.Entries[0]);
      Assert.AreEqual(_objectThree, _setList.Next.Entries[1]);
      Assert.AreEqual(null, _setList.Next.Next);
    }

    [Test]
    public void SetSubdividesTwice()
    {
      var list = new List<TestObject>();
      list.Add(_objectOne);
      list.Add(_objectTwo);
      list.Add(_objectThree);
      list.Add(_objectFour);
      list.Add(_objectFive);
      _setList = new SubdividingSet<TestObject>(list, _bucketSize);

      Assert.AreEqual(2, _setList.Count);
      Assert.AreNotEqual(null, _setList.Next);
      Assert.AreEqual(1, _setList.Next.Count);
      Assert.AreNotEqual(null, _setList.Next.Next);
      Assert.AreEqual(2, _setList.Next.Next.Count);
      Assert.AreEqual(null, _setList.Next.Next.Next);
    }

    [Test]
    public void PointsRetainOrderOnMultipleSubdivide()
    {
      var list = new List<TestObject>();
      list.Add(_objectOne);
      list.Add(_objectTwo);
      list.Add(_objectThree);
      list.Add(_objectFour);
      list.Add(_objectFive);
      _setList = new SubdividingSet<TestObject>(list, _bucketSize);

      Assert.AreEqual(_objectOne, _setList.Entries[0]);
      Assert.AreEqual(_objectTwo, _setList.Entries[1]);
      Assert.AreEqual(_objectThree, _setList.Next.Entries[0]);
      Assert.AreEqual(_objectFour, _setList.Next.Next.Entries[0]);
      Assert.AreEqual(_objectFive, _setList.Next.Next.Entries[1]);
    }

    private class TestObject { }
  }
}
