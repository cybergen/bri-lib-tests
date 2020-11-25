using System;
using UnityEngine.UI;
using BriLib;
using BriLib.UI;

public class TestStringPanel : FadingPanel<string>
{
  public Text DisplayText;

  public override void Show(string data, Action onShowAnimationFinish = null)
  {
    DisplayText.text = data;
    base.Show(data, onShowAnimationFinish);
  }

  public async void FadeOut()
  {
    await DisplayText.gameObject.Fade(0.75f, 0f, Easing.Method.ElasticOut);
  }

  public async void FadeIn()
  {
    await DisplayText.gameObject.Fade(0.75f, 1f, Easing.Method.ElasticOut);
  }

  public void FadeAfterFive()
  {
    AsyncMethods.DoAfterTime(5f, FadeOut);
  }
}
