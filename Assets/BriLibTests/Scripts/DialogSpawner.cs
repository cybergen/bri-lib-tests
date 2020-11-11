using UnityEngine;
using BriLib;

public class DialogSpawner : Panel
{
  public GameObject Template;

  public void SpawnDialog()
  {
    DialogManager.Instance.ShowDialog(Template, "Title", "Body", () => DialogManager.Instance.HideDialog(Template), new ButtonData("Hide", null, true ));
  }
}
