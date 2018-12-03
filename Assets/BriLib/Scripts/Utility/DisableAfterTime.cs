using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
  public float SecondsBeforeDisable;

  private float _timeElapsed;

  private void OnEnable()
  {
    _timeElapsed = 0f;
  }

  private void Update()
  {
    _timeElapsed += Time.deltaTime;
    if (_timeElapsed >= SecondsBeforeDisable) gameObject.SetActive(false);
  }
}
