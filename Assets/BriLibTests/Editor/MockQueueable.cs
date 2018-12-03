using System;
using BriLib;

namespace BriLib.Tests
{
  public class MockQueueable : IQueueable
  {
    public Action<IQueueable> OnBegan { get; set; }
    public Action<IQueueable> OnEnded { get; set; }
    public Action<IQueueable> OnKilled { get; set; }

    public void Begin()
    {
      OnBegan.Execute(this);
    }

    public void Kill()
    {
      OnKilled.Execute(this);
    }

    public void ForceEnd()
    {
      OnEnded.Execute(this);
    }
  }
}
