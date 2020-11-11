using System;
using UnityEngine.UI;
using BriLib;

public class TestFloatPanel : Panel<float>
{
  public Text DisplayText;

  public override void Show(float data, Action onShowAnimationFinish = null)
  {
    base.Show(data, onShowAnimationFinish);
    DisplayText.text = data.ToString();
  }
}
