using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  [TestFixture]
  public class ParallelQueueableExecutorTests
  {
    private ParallelQueueableExecutor _executor;
    private MockQueueable _queueableOne;
    private MockQueueable _queueableTwo;

    private int _executorBeginCallCount;
    private int _executorKillCallCount;
    private int _executorEndCallCount;
    private int _queueOneBeginCallCount;
    private int _queueOneKillCallCount;
    private int _queueOneEndCallCount;
    private int _queueTwoBeginCallCount;
    private int _queueTwoKillCallCount;
    private int _queueTwoEndCallCount;

    [SetUp]
    public void Setup()
    {
      _executor = new ParallelQueueableExecutor();
      _executor.OnBegan += (ex) => _executorBeginCallCount++;
      _executor.OnKilled += (ex) => _executorKillCallCount++;
      _executor.OnEnded += (ex) => _executorEndCallCount++;

      _queueableOne = new MockQueueable();
      _queueableOne.OnBegan += (queue) => _queueOneBeginCallCount++;
      _queueableOne.OnKilled += (queue) => _queueOneKillCallCount++;
      _queueableOne.OnEnded += (queue) => _queueOneEndCallCount++;

      _queueableTwo = new MockQueueable();
      _queueableTwo.OnBegan += (queue) => _queueTwoBeginCallCount++;
      _queueableTwo.OnKilled += (queue) => _queueTwoKillCallCount++;
      _queueableTwo.OnEnded += (queue) => _queueTwoEndCallCount++;

      _executorBeginCallCount = 0;
      _executorKillCallCount = 0;
      _executorEndCallCount = 0;
      _queueOneBeginCallCount = 0;
      _queueOneKillCallCount = 0;
      _queueOneEndCallCount = 0;
      _queueTwoBeginCallCount = 0;
      _queueTwoKillCallCount = 0;
      _queueTwoEndCallCount = 0;
    }

    [Test]
    public void BeginStartsAll()
    {
      _executor.AddQueueable(_queueableOne);
      _executor.AddQueueable(_queueableTwo);
      Assert.AreEqual(0, _queueOneBeginCallCount, "Queueable one not yet begun");
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Queueable two not yet begun");
      Assert.AreEqual(0, _executorBeginCallCount, "Executor has not yet begun");
      _executor.Begin();
      Assert.AreEqual(1, _queueOneBeginCallCount, "Queueable one has begun");
      Assert.AreEqual(1, _queueTwoBeginCallCount, "Queueable two has begun");
      Assert.AreEqual(1, _executorBeginCallCount, "Executor has yet begun");
    }

    [Test]
    public void KillKillsAll()
    {
      _executor.AddQueueable(_queueableOne);
      _executor.AddQueueable(_queueableTwo);
      Assert.AreEqual(0, _queueOneKillCallCount, "Queueable one not yet killed");
      Assert.AreEqual(0, _queueTwoKillCallCount, "Queueable two not yet killed");
      Assert.AreEqual(0, _executorKillCallCount, "Executor was not yet killed");
      _executor.Begin();
      Assert.AreEqual(0, _queueOneKillCallCount, "Queueable one not yet killed");
      Assert.AreEqual(0, _queueTwoKillCallCount, "Queueable two not yet killed");
      Assert.AreEqual(0, _executorKillCallCount, "Executor was not yet killed");
      _executor.Kill();
      Assert.AreEqual(1, _queueOneKillCallCount, "Queueable one was killed");
      Assert.AreEqual(1, _queueTwoKillCallCount, "Queueable two was killed");
      Assert.AreEqual(1, _executorKillCallCount, "Executor was killed");
    }

    [Test]
    public void EndOneDoesntEndExecutor()
    {
      _executor.AddQueueable(_queueableOne);
      _executor.AddQueueable(_queueableTwo);
      _executor.Begin();
      Assert.AreEqual(0, _executorEndCallCount, "Executor not yet ended");
      _queueableOne.ForceEnd();
      Assert.AreEqual(0, _executorEndCallCount, "Executor not yet ended");
    }

    [Test]
    public void EndAllEndsExecutor()
    {
      _executor.AddQueueable(_queueableOne);
      _executor.AddQueueable(_queueableTwo);
      _executor.Begin();
      Assert.AreEqual(0, _executorEndCallCount, "Executor not yet ended");
      _queueableTwo.ForceEnd();
      Assert.AreEqual(0, _executorEndCallCount, "Executor not yet ended");
      _queueableOne.ForceEnd();
      Assert.AreEqual(1, _executorEndCallCount, "Executor has ended");
    }

    [Test]
    public void KillOneKillsExecutor()
    {
      _executor.AddQueueable(_queueableOne);
      _executor.AddQueueable(_queueableTwo);
      _executor.Begin();
      Assert.AreEqual(0, _executorKillCallCount, "Executor not yet ended");
      _queueableTwo.Kill();
      Assert.AreEqual(1, _executorKillCallCount, "Executor has ended");
    }
  }
}
