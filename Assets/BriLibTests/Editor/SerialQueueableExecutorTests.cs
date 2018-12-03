using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  [TestFixture]
  public class SerialQueueableExecutorTests
  {
    private SerialQueueableExecutor _executor;
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
      _executor = new SerialQueueableExecutor();
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
    public void BeginTriggersExecutorCallback()
    {
      Assert.AreEqual(0, _executorBeginCallCount, "Executor begin callback should not yet have been triggered");
      _executor.Begin();
      Assert.AreEqual(1, _executorBeginCallCount, "Executor begin callback was triggered once on executor.Begin");
    }

    [Test]
    public void BeginStartsQueuedEntry()
    {
      Assert.AreEqual(0, _queueOneBeginCallCount, "Queueable begin callback should not yet have triggered");
      _executor.Queue(_queueableOne);
      Assert.AreEqual(0, _queueOneBeginCallCount, "Queueable begin callback should not yet have triggered");
      _executor.Begin();
      Assert.AreEqual(1, _queueOneBeginCallCount, "Queueable begin callback should have been called once");
    }

    [Test]
    public void KillTriggersExecutorKillCallback()
    {
      Assert.AreEqual(0, _executorKillCallCount, "Kill callback on executor not yet triggered");
      _executor.Queue(_queueableOne);
      _executor.Begin();
      Assert.AreEqual(0, _executorKillCallCount, "Kill callback on executor not yet triggered");
      _executor.Kill();
      Assert.AreEqual(1, _executorKillCallCount, "Kill callback on executor should have triggered once");
    }

    [Test]
    public void KillKillsQueuedEntry()
    {
      Assert.AreEqual(0, _queueOneKillCallCount, "Kill callback on queueable not yet triggered");
      _executor.Queue(_queueableOne);
      _executor.Begin();
      Assert.AreEqual(0, _queueOneKillCallCount, "Kill callback on queueable not yet triggered");
      _executor.Kill();
      Assert.AreEqual(1, _queueOneKillCallCount, "Kill callback on queueable should have triggered once");
    }

    [Test]
    public void BeginDoesntStartSecondEntry()
    {
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 not yet triggered");
      _executor.Queue(_queueableOne);
      _executor.Queue(_queueableTwo);
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 not yet triggered");
      _executor.Begin();
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 should still not trigger");
    }

    [Test]
    public void KillDoesntKillUnstartedEntry()
    {
      Assert.AreEqual(0, _queueTwoKillCallCount, "Kill callback on queueable 2 not yet triggered");
      _executor.Queue(_queueableOne);
      _executor.Queue(_queueableTwo);
      _executor.Begin();
      Assert.AreEqual(0, _queueTwoKillCallCount, "Kill callback on queueable 2 not yet triggered");
      _executor.Kill();
      Assert.AreEqual(0, _queueTwoKillCallCount, "Kill callback on queueable 2 should not trigger");
    }

    [Test]
    public void FinishEntryAdvancesQueue()
    {
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 not yet triggered");
      _executor.Queue(_queueableOne);
      _executor.Queue(_queueableTwo);
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 not yet triggered");
      _executor.Begin();
      Assert.AreEqual(0, _queueTwoBeginCallCount, "Begin callback on queueable 2 not yet triggered");
      _queueableOne.ForceEnd();
      Assert.AreEqual(1, _queueTwoBeginCallCount, "Begin callback on queueable 2 should have triggered once when queueable 1 finished");
    }

    [Test]
    public void FinishAllEntries()
    {
      Assert.AreEqual(0, _executorEndCallCount, "Executor should not yet have triggered end callback");
      _executor.Queue(_queueableOne);
      _executor.Queue(_queueableTwo);
      Assert.AreEqual(0, _executorEndCallCount, "Executor should not yet have triggered end callback");
      _executor.Begin();
      Assert.AreEqual(0, _executorEndCallCount, "Executor should not yet have triggered end callback");
      _queueableOne.ForceEnd();
      Assert.AreEqual(0, _executorEndCallCount, "Executor should not yet have triggered end callback");
      _queueableTwo.ForceEnd();
      Assert.AreEqual(1, _executorEndCallCount, "Executor should have triggered end callback");
    }

    [Test]
    public void KillCurrentPlaying()
    {
      Assert.AreEqual(0, _executorKillCallCount, "Executor kill should not yet have triggered");
      _executor.Queue(_queueableOne);
      _executor.Begin();
      Assert.AreEqual(0, _executorKillCallCount, "Executor kill should not yet have triggered");
      _queueableOne.Kill();
      Assert.AreEqual(1, _executorKillCallCount, "Executor kill should have triggered");
    }
  }
}
