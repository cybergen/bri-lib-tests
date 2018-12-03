using System;

namespace BriLib
{
  public class EaseWrapper
  {
    public enum Direction
    {
      Forward,
      Backward,
    }

    private float _currentTime = 0f;
    private Direction _currentDirection = Direction.Forward;

    private float _duration;
    private float _startValue;
    private float _endValue;
    private Easing.Method _easeType;
    private Action<float> _onUpdate;

    private Action _onFinish;
    private Action _onCancel;
    private bool _easing;

    public EaseWrapper(
      float duration, 
      float startValue, 
      float endValue, 
      Easing.Method easeType, 
      Action<float> onUpdate)
    {
      _duration = duration;
      _startValue = startValue;
      _endValue = endValue;
      _easeType = easeType;
      _onUpdate = onUpdate;
    }

    public void SetEase(Direction direction, Action onFinish, Action onCancel)
    {
      if (_easing) _onCancel.Execute();

      _currentDirection = direction;
      _easing = true;
      _onFinish = onFinish;
      _onCancel = onCancel;
    }

    public void Tick(float delta)
    {
      if (!_easing) return;

      if (_currentDirection == Direction.Forward)
      {
        _currentTime += delta;
        if (_currentTime >= _duration)
        {
          _currentTime = _duration;
          _easing = false;
          _onFinish.Execute();
        }
      }
      else
      {
        _currentTime -= delta;
        if (_currentTime <= 0)
        {
          _currentTime = 0f;
          _easing = false;
          _onFinish.Execute();
        }
      }

      _onUpdate.Execute(Easing.Ease(_startValue, _endValue, 0f, _duration, _currentTime, _easeType));
    }
  }
}