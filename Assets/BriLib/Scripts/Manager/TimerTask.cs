namespace BriLib
{
  public class TimerTask : AsyncTask
  {
    public float Elapsed { get; private set; }
    public float Completion { get { return Elapsed / _duration; } }
    public float TimeLeft { get { return _duration - Elapsed; } }
    private float _duration;

    public void SetDuration(float limit)
    {
      _duration = limit;
    }

    public override void Tick(float delta)
    {
      base.Tick(delta);
      Elapsed += delta;
      if (Elapsed >= _duration) Finish();
    }

    public void ForceFail() { Fail(); }
  }
}
