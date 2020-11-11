using System;
using UnityEngine.UI;
using BriLib;

public class TestStringPanel : FadingPanel<string>
{
  public Text DisplayText;

  public override void Show(string data, Action onShowAnimationFinish = null)
  {
    DisplayText.text = data;
    base.Show(data, onShowAnimationFinish);
  }
}
