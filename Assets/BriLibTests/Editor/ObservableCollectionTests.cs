using System;
using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  [TestFixture]
  public class ObservableCollectionTests
  {
    private ObservableCollection<TestObject> _list;
    private ObservableCollection<TestObject> _listTwo;
    private TestObject _objOne;
    private TestObject _objTwo;
    private TestObject _objThree;
    private TestObject _objFour;
    private TestObject _objFive;
    private TestObject _objSix;

    private object _addedObj;
    private int _addedObjIndex;

    [SetUp]
    public void Setup()
    {
      _list = new ObservableCollection<TestObject>();
      _listTwo = new ObservableCollection<TestObject>();
      _objOne = new TestObject { Value = 1 };
      _objTwo = new TestObject { Value = 2 };
      _objThree = new TestObject { Value = 3 };
      _objFour = new TestObject { Value = 4 };
      _objFive = new TestObject { Value = 5 };
      _objSix = new TestObject { Value = 6 };

    }

    [Test]
    public void AddItem()
    {
      TestObject addedItem = null;
      int index = -1;
      _list.OnAdded += (ind, obj) => { index = ind; addedItem = obj; };
      _list.Add(_objOne);
      Assert.AreEqual(0, index, "Item added to index 0");
      Assert.AreEqual(_objOne, addedItem, "Added item was item one");
    }

    [Test]
    public void AddNonGeneric()
    {
      var list = _list as IObservableCollection;
      TestObject addedItem = null;
      int index = -1;
      list.OnAddedNonGeneric += (ind, obj) => { index = ind; addedItem = obj as TestObject; };
      list.Add(_objOne);
      Assert.AreEqual(0, index, "Item added to index 0");
      Assert.AreEqual(_objOne, addedItem, "Added item was item one");
    }

    [Test]
    public void AddIncorrectTypeNonGeneric()
    {
      var list = _list as IObservableCollection;
      list.Add(_objOne);
      list.Add(_objTwo);
      bool excepted = false;
      try
      {
        list.Add(new WrappedTestObject());
      }
      catch (Exception e)
      {
        excepted = true;
      }
      Assert.AreEqual(2, list.Count, "Count should only be two");
      Assert.True(excepted, "Should get exception on type mismatch");
    }

    [Test]
    public void GetEnumerator()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      int passes = _list.Count;
      Assert.AreEqual(2, passes, "Should get enumerator and make two passes through loop");
    }

    [Test]
    public void GetEnumeratorNonGeneric()
    {
      var list = _list as IObservableCollection;
      list.Add(_objOne);
      list.Add(_objTwo);
      int passes = _list.Count;
      Assert.AreEqual(2, passes, "Should get enumerator and make two passes through loop");
    }

    [Test]
    public void MembersPresent()
    {
      Assert.AreEqual(0, _list.Count, "Count should still be 0");
      _list.Add(_objOne);
      Assert.AreEqual(1, _list.Count, "Count should be 1");
      Assert.True(_list.Contains(_objOne), "Object one should be in list");
      _list.Add(_objTwo);
      Assert.AreEqual(2, _list.Count, "Count should become 2");
      Assert.True(_list.Contains(_objTwo), "Object two should be in list");
      Assert.True(_list.Contains(_objOne), "Object one should be in list");
    }

    [Test]
    public void MembersPresentNonGeneric()
    {
      var list = _list as IObservableCollection;
      Assert.AreEqual(0, list.Count, "Count should still be 0");
      list.Add(_objOne);
      Assert.AreEqual(1, list.Count, "Count should be 1");
      Assert.True(list.Contains(_objOne), "Object one should be in list");
      list.Add(_objTwo);
      Assert.AreEqual(2, list.Count, "Count should become 2");
      Assert.True(list.Contains(_objTwo), "Object two should be in list");
      Assert.True(list.Contains(_objOne), "Object one should be in list");
    }

    [Test]
    public void RemoveItem()
    {
      TestObject removedItem = null;
      int index = -1;
      _list.Add(_objTwo);
      _list.Add(_objOne);
      _list.Add(_objThree);
      _list.OnRemoved += (ind, obj) => { index = ind; removedItem = obj; };
      Assert.True(_list.Contains(_objOne), "Object one still in list");
      Assert.AreEqual(3, _list.Count, "Count should be three");
      _list.Remove(_objOne);
      Assert.AreEqual(1, index, "Object should be removed from index 1");
      Assert.AreEqual(removedItem, _objOne, "Object one should be removed");
    }

    [Test]
    public void RemoveItemNonGeneric()
    {
      var list = _list as IObservableCollection;
      TestObject removedItem = null;
      int index = -1;
      list.Add(_objTwo);
      list.Add(_objOne);
      list.Add(_objThree);
      list.OnRemovedNonGeneric += (ind, obj) => { index = ind; removedItem = obj as TestObject; };
      Assert.True(list.Contains(_objOne), "Object one still in list");
      Assert.AreEqual(3, list.Count, "Count should be three");
      list.Remove(_objOne);
      Assert.AreEqual(1, index, "Object should be removed from index 1");
      Assert.AreEqual(removedItem, _objOne, "Object one should be removed");
    }

    [Test]
    public void UpdateItem()
    {
      var index = -1;
      TestObject obj = null;
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.OnReplaced += (ind, old, newObj) => { index = ind; obj = newObj; };
      _list[1] = _objThree;
      Assert.AreEqual(1, index, "Index replaced should be 1");
      Assert.AreEqual(obj, _objThree, "Object two should be replaced with obj three");
    }

    [Test]
    public void UpdateItemNonGeneric()
    {
      var list = _list as IObservableCollection;
      var index = -1;
      TestObject obj = null;
      list.Add(_objOne);
      list.Add(_objTwo);
      list.OnReplacedNonGeneric += (ind, old, newObj) => { index = ind; obj = newObj as TestObject; };
      list[1] = _objThree;
      Assert.AreEqual(1, index, "Index replaced should be 1");
      Assert.AreEqual(obj, _objThree, "Object two should be replaced with obj three");
    }

    [Test]
    public void ClearItem()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.Add(_objThree);
      Assert.AreEqual(3, _list.Count, "Count should be three");
      bool cleared = false;
      _list.OnCleared += () => cleared = true;
      _list.Clear();
      Assert.AreEqual(0, _list.Count, "Count should be 0");
      Assert.False(_list.Contains(_objTwo), "Object two should be removed");
      Assert.False(_list.Contains(_objThree), "Object three should be removed");
      Assert.False(_list.Contains(_objOne), "Object one should be removed");
      Assert.True(cleared, "OnClear callback should have triggered");
    }

    [Test]
    public void ClearItemNonGeneric()
    {
      var list = _list as IObservableCollection;
      list.Add(_objOne);
      list.Add(_objTwo);
      list.Add(_objThree);
      Assert.AreEqual(3, list.Count, "Count should be three");
      bool cleared = false;
      list.OnCleared += () => cleared = true;
      list.Clear();
      Assert.AreEqual(0, list.Count, "Count should be 0");
      Assert.False(list.Contains(_objTwo), "Object two should be removed");
      Assert.False(list.Contains(_objThree), "Object three should be removed");
      Assert.False(list.Contains(_objOne), "Object one should be removed");
      Assert.True(cleared, "OnClear callback should have triggered");
    }

    [Test]
    public void GetMappedCollection()
    {
      Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.Add(_objThree);
      var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
      Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
      bool hadOne = false;
      bool hadTwo = false;
      bool hadThree = false;
      mappedList.ForEach(entry =>
      {
        hadOne |= entry.Object == _objOne;
        hadTwo |= entry.Object == _objTwo;
        hadThree |= entry.Object == _objThree;
      });

      Assert.True(hadOne, "Mapped list should have contained item one");
      Assert.True(hadTwo, "Mapped list should have contained item two");
      Assert.True(hadThree, "Mapped list should have contained item three");
    }

    [Test]
    public void MappedCollectionUnderlyingAdd()
    {
      Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
      Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
      WrappedTestObject test = null;
      int testIndex = -1;
      mappedList.OnAdded += (ind, item) => { testIndex = ind; test = item; };
      _list.Add(_objThree);
      Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
      Assert.AreEqual(2, testIndex, "New entry should be at index 2");
      Assert.AreEqual(_objThree, test.Object, "Object three should have been added");
    }

    [Test]
    public void MappedCollectionUnderlyingRemove()
    {
      Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.Add(_objThree);
      var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
      Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
      WrappedTestObject test = null;
      int testIndex = -1;
      mappedList.OnRemoved += (ind, item) => { testIndex = ind; test = item; };
      _list.Remove(_objTwo);
      Assert.AreEqual(1, testIndex, "Removed entry should be at index 1");
      Assert.AreEqual(_objTwo, test.Object, "Object two should have been removeed");
      Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
    }

    [Test]
    public void MappedCollUnderlyingReplace()
    {
      Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
      Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
      WrappedTestObject newObj = null;
      WrappedTestObject oldObj = null;
      int testIndex = -1;
      mappedList.OnReplaced += (ind, item, itemTwo) => { testIndex = ind; oldObj = item; newObj = itemTwo; };
      _list[1] = _objThree;
      Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
      Assert.AreEqual(1, testIndex, "New entry should be at index 1");
      Assert.AreEqual(_objThree, newObj.Object, "Object three should have been added");
      Assert.AreEqual(_objTwo, oldObj.Object, "Object two should have been swapped out");
    }

    [Test]
    public void MappedCollUnderlyingClear()
    {
      Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
      Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
      bool clearCalled = true;
      mappedList.OnCleared += () => { clearCalled = true; };
      _list.Clear();
      Assert.AreEqual(0, mappedList.Count, "Mapped list should have 0 entries");
      Assert.True(clearCalled, "Clear should have been called");
    }

    [Test]
    public void GetMappedNonGeneric()
    {
      Func<object, object> map = (entry) => { return new WrappedTestObject { Object = (TestObject)entry }; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.Add(_objThree);
      var mappedList = _list.Map(map);
      Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
      bool hadOne = false;
      bool hadTwo = false;
      bool hadThree = false;
      mappedList.ForEach(entry =>
      {
        hadOne |= (entry as WrappedTestObject).Object == _objOne;
        hadTwo |= (entry as WrappedTestObject).Object == _objTwo;
        hadThree |= (entry as WrappedTestObject).Object == _objThree;
      });

      Assert.True(hadOne, "Mapped list should have contained item one");
      Assert.True(hadTwo, "Mapped list should have contained item two");
      Assert.True(hadThree, "Mapped list should have contained item three");
    }

    [Test]
    public void ReduceExecute()
    {
      Func<TestObject, int, int> add = (obj, index) => { return index + obj.Value; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      int start = _list.Reduce(0, add);
      Assert.AreEqual(3, start, "Seed value should have incremented to 3");
    }

    [Test]
    public void ReduceNonGeneric()
    {
      Func<object, int, int> add = (obj, index) => { return index + (obj as TestObject).Value; };
      _list.Add(_objOne);
      _list.Add(_objTwo);
      int start = _list.ReduceNonGeneric(0, add);
      Assert.AreEqual(3, start, "Seed value should have incremented to 3");
    }

    [Test]
    public void GetFilteredCollection()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
    }

    [Test]
    public void FilteredCollNonGeneric()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var filtered = _list.FilterNonGeneric((obj) => { return (obj as TestObject).Value == 1; });
      Assert.AreEqual(1, filtered.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, filtered[0], "Object at position 0 should be objOne");
    }

    [Test]
    public void FilteredCollAddMember()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list.Add(_objThree);
      Assert.AreEqual(2, cast.Count, "Filtered list should have additional object");
      Assert.AreEqual(_objThree, cast[1], "Object three should be at position 1");
    }

    [Test]
    public void FilteredCollAddFilteredOutMember()
    {
      _list.Add(_objOne);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list.Add(_objTwo);
      Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
      Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
    }

    [Test]
    public void FilteredCollReplaceMember()
    {
      _list.Add(_objOne);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list[0] = _objThree;
      Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
      Assert.AreEqual(_objThree, cast[0], "Object three should be at position 0");
    }

    [Test]
    public void FilteredCollReplaceNonpresentMember()
    {
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(0, cast.Count, "Filtered list should be empty");
      _list[0] = _objThree;
      Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
      Assert.AreEqual(_objThree, cast[0], "Object three should be at position 0");
    }

    [Test]
    public void FilteredCollReplacePresentMemberWithFilteredOutMember()
    {
      _list.Add(_objOne);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list[0] = _objTwo;
      Assert.AreEqual(0, cast.Count, "Filtered list should not have any objects");
    }

    [Test]
    public void FilteredCollReplaceNonPresentWithNonPresent()
    {
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(0, cast.Count, "Filtered list should be empty");
      _list[0] = new TestObject() { Value = 4 };
      Assert.AreEqual(0, cast.Count, "Filtered list should not have any objects");
    }

    [Test]
    public void FilteredCollRemoveMember()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list.Remove(_objOne);
      Assert.AreEqual(0, cast.Count, "Filtered list should have no objects");
    }

    [Test]
    public void FilteredRemoveNonPresent()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      _list.Remove(_objTwo);
      Assert.AreEqual(1, cast.Count, "Filtered list should have one object");
      Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
    }

    [Test]
    public void FilteredClear()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _list.Add(_objThree);
      var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
      var cast = filtered as ObservableCollection<TestObject>;
      Assert.AreEqual(2, cast.Count, "Filtered list should have just one object");
      Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
      Assert.AreEqual(_objThree, cast[1], "Object three should be at position 1");
      bool clearCalled = false;
      filtered.OnCleared += () => clearCalled = true;
      _list.Clear();
      Assert.True(clearCalled, "Clear callback should have triggered");
      Assert.AreEqual(0, cast.Count, "Filtered list should become empty");
    }

    [Test]
    public void GetSortedCollection()
    {
      _list.Add(_objThree);
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var sorted = _list.Sort((obj, obj2) => { return obj.Value - obj2.Value; });
      var cast = sorted as ObservableCollection<TestObject>;
      Assert.AreEqual(3, cast.Count, "Sorted list count should be 3");
      Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
      Assert.AreEqual(_objTwo, cast[1], "Object two should be at position 1");
      Assert.AreEqual(_objThree, cast[2], "Object three should be at position 2");
    }

    [Test]
    public void GetSortedGeneric()
    {
      _list.Add(_objThree);
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
      Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
      Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
      Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
      Assert.AreEqual(_objThree, sorted[2], "Object three should be at position 2");
    }

    [Test]
    public void SortedAddMember()
    {
      _list.Add(_objThree);
      _list.Add(_objTwo);
      var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
      Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
      Assert.AreEqual(_objTwo, sorted[0], "Object two should be at position 1");
      Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 2");
      _list.Add(_objOne);
      Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
      Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
    }

    [Test]
    public void SortedRemoveMember()
    {
      _list.Add(_objThree);
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
      Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
      Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
      Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
      Assert.AreEqual(_objThree, sorted[2], "Object three should be at position 2");
      _list.Remove(_objTwo);
      Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
      Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
      Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 1");
    }

    [Test]
    public void SortedReplace()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
      Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
      Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
      Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
      _list[0] = _objThree;
      Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
      Assert.AreEqual(_objTwo, sorted[0], "Object two should be at position 0");
      Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 1");
    }

    [Test]
    public void SortedClear()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
      Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
      bool clearCalled = false;
      sorted.OnCleared += () => clearCalled = true;
      _list.Clear();
      Assert.True(clearCalled, "Clear should have been called");
      Assert.AreEqual(0, sorted.Count, "Sorted list should now be empty");
    }

    [Test]
    public void GetUnion()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
    }

    [Test]
    public void GetUnionNonGeneric()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
    }

    [Test]
    public void UnionAddLeft()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
      _list.Add(_objSix);
      Assert.AreEqual(5, union.Count, "Unioned list should be at 5 objects now");
      Assert.AreEqual(_objSix, union[2], "Object six should be at position 2");
    }

    [Test]
    public void UnionAddRight()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
      _listTwo.Add(_objFive);
      Assert.AreEqual(5, union.Count, "Unioned list should be at 5 objects now");
      Assert.AreEqual(_objFive, union[4], "Object five should be at position 4");
    }

    [Test]
    public void UnionRemoveLeft()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
      _list.Remove(_objTwo);
      Assert.AreEqual(3, union.Count, "Unioned list should be at 3 objects now");
      Assert.AreEqual(_objOne, union[0], "Object one should be at position 0");
      Assert.AreEqual(_objThree, union[1], "Object three should be at position 1");
    }

    [Test]
    public void UnionRemoveRight()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      Assert.AreEqual(_objThree, union[2], "Entry 2 in list should be object three");
      Assert.AreEqual(_objFour, union[3], "Entry 3 in list should be object four");
      _listTwo.Remove(_objFour);
      Assert.AreEqual(3, union.Count, "Unioned list should be at 3 objects now");
      Assert.AreEqual(_objTwo, union[1], "Object two should be at position 1");
      Assert.AreEqual(_objThree, union[2], "Object three should be at position 2");
    }

    [Test]
    public void UnionReplaceLeft()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      _list[1] = _objFive;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objFive, union[1], "Entry 1 in list should be object five, now");
    }

    [Test]
    public void UnionReplaceRight()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
      _listTwo[1] = _objFive;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      Assert.AreEqual(_objFive, union[3], "Entry 3 in list should be object five, now");
    }

    [Test]
    public void UnionClearLeft()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      _list.Clear();
      Assert.AreEqual(2, union.Count, "Unioned list should have 2 objects remaining");
      Assert.AreEqual(_objThree, union[0], "Entry 0 in list should be object three");
      Assert.AreEqual(_objFour, union[1], "Entry 1 in list should be object four");
    }

    [Test]
    public void UnionClearRight()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      _listTwo.Clear();
      Assert.AreEqual(2, union.Count, "Unioned list should have 2 objects remaining");
      Assert.AreEqual(_objOne, union[0], "Entry 0 in list should be object one");
      Assert.AreEqual(_objTwo, union[1], "Entry 1 in list should be object two");
    }

    [Test]
    public void UnionClearBoth()
    {
      _list.Add(_objOne);
      _list.Add(_objTwo);
      _listTwo.Add(_objThree);
      _listTwo.Add(_objFour);
      var union = _list.Union(_listTwo) as ObservableCollection<TestObject>;
      Assert.AreEqual(4, union.Count, "Unioned list should have 4 objects");
      _list.Clear();
      _listTwo.Clear();
      Assert.AreEqual(0, union.Count, "Unioned list should have 0 remaining objects");
    }

    private class WrappedTestObject
    {
      public TestObject Object;
    }

    private class TestObject
    {
      public int Value;
    }
  }
}
