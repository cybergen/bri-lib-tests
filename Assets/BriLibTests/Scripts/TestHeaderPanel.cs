using BriLib.UI;

public class TestHeaderPanel : Panel
{
  public void OnShowStringPanel()
  {
    UIManager.Body.ShowPanel<TestStringPanel, string>(System.Guid.NewGuid().ToString());
  }

  public void OnShowFloatPanel()
  {
    UIManager.Body.ShowPanel<TestFloatPanel, float>(new System.Random().Next(5000));
  }
}
