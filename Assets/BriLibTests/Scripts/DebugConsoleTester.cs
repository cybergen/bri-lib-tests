using UnityEngine;
using BriLib;
using System;
using System.Collections.Generic;

public class DebugConsoleTester : MonoBehaviour
{
  private void Start()
  {
    DebugConsole.Instance.AddCommand("param_list", "List provided params", ParamListCmd);
  }

  private string ParamListCmd(List<string> args)
  {
    var s = String.Empty;
    foreach (var c in args)
    {
      s += c + ", ";
    }
    return s;
  }
}
