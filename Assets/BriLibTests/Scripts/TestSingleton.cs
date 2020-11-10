using UnityEngine;
using BriLib;

public class TestSingleton : Singleton<TestSingleton>
{
  public int TestValue = 4;

  private bool _onCreateCalled;
  private bool _onBeginCalled;
  private bool _onEndCalled;

  public override void OnCreate()
  {
    base.OnCreate();
    _onCreateCalled = true;
  }

  public override void Begin()
  {
    base.Begin();
    _onBeginCalled = true;
  }

  public override void End()
  {
    base.End();
    _onEndCalled = true;
  }

  public void LogState()
  {
    Debug.Log(string.Format("Current state TestValue:{0}, OnCreateCalled:{1}, BeginCalled:{2}, EndCalled:{3}",
      TestValue, _onCreateCalled, _onBeginCalled, _onEndCalled));
  }
}
