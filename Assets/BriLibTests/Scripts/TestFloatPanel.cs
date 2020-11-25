using System;
using UnityEngine;
using UnityEngine.UI;
using BriLib.UI;

public class TestFloatPanel : Panel<float>
{
  public Text DisplayText;

  public override void Show(float data, Action onShowAnimationFinish = null)
  {
    base.Show(data, onShowAnimationFinish);
    DisplayText.text = data.ToString();
  }

  public void ShrinkWindow()
  {
    var rect = GetComponent<RectTransform>();
    var startLeft = rect.offsetMin.x;
    var startRight = -rect.offsetMax.x;
    var startTop = -rect.offsetMax.y;
    var startBottom = rect.offsetMin.y;

    rect.SetLeft(startLeft + 10);
    rect.SetRight(startRight + 10);
    rect.SetTop(startTop + 10);
    rect.SetBottom(startBottom + 10);
  }
}
