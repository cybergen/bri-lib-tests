using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  [TestFixture]
  public class ObservablePropertyTests
  {
    private ObservableProperty<MockQueueable> _prop;

    [SetUp]
    public void Setup()
    {
      _prop = new ObservableProperty<MockQueueable>();
    }

    [Test]
    public void OnPropChanged()
    {
      ObservableProperty<MockQueueable> callback = null;
      MockQueueable mockObj = new MockQueueable();
      _prop.OnChanged += (obj) => { callback = obj as ObservableProperty<MockQueueable>; };
      Assert.AreEqual(null, callback, "Callback not yet triggered");
      Assert.AreEqual(null, _prop.Value, "Object not yet assigned");
      _prop.Value = mockObj;
      Assert.AreEqual(_prop, callback, "Callback triggered");
      Assert.AreEqual(mockObj, _prop.Value, "New object assigned");
    }

    [Test]
    public void OnNonGenericChanged()
    {
      ObservableProperty<MockQueueable> callback = null;
      object mockObj = new MockQueueable();
      (_prop as IObservable).OnChanged += (obj) => { callback = obj as ObservableProperty<MockQueueable>; };
      Assert.AreEqual(null, callback, "Callback not yet triggered");
      Assert.AreEqual(null, _prop.Value, "Object not yet assigned");
      (_prop as IObservable).Value = mockObj;
      Assert.AreEqual(_prop, callback, "Callback triggered");
      Assert.AreEqual(mockObj, _prop.Value, "New object assigned");
    }
  }
}
