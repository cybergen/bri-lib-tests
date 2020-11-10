using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonTester : MonoBehaviour
{
  public void OnLogStatePressed()
  {
    TestSingleton.Instance.LogState();
  }

  public void OnLoadSceneCalled()
  {
    SceneManager.LoadScene("SingletonTestScene2");
  }
}
