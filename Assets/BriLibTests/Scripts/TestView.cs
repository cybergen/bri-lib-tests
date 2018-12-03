using UnityEngine;
using UnityEngine.UI;

namespace BriLib.Tests
{
  public class TestView : MonoBehaviour, IView
  {
    public GameObject GameObject { get; private set; }
    public object Data { get; private set; }
    public Text Text;

    public void ApplyData(object data)
    {
      GameObject = gameObject;
      Data = data;
      var castData = data as TestData;
      Text.text = castData.Text;

    }
  }
}
