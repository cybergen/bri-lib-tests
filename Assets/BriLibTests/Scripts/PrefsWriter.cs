using UnityEngine;
using System.Threading.Tasks;
using BriLib;

public class PrefsWriter : MonoBehaviour
{
  async void Start()
  {
    await Task.Delay(5000);
    PreferencesManager.Instance.SetBool("WritesCorrectly", true);
    LogManager.Info("Wrote correctly: " + PreferencesManager.Instance.GetBool("WritesCorrectly"));
  }
}
