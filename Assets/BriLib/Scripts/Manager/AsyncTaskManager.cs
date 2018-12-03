using UnityEngine;
using System;
using System.Collections.Generic;

namespace BriLib
{
  public class AsyncTaskManager : GOSingleton<AsyncTaskManager>
  {
    private List<AsyncTask> _tasks = new List<AsyncTask>();
    private List<AsyncTask> _removeList = new List<AsyncTask>();

    public T StartTask<T>(Action onFinish = null, Action onFailed = null) where T : AsyncTask, new()
    {
      var task = new T();
      task.SetCallbacks(onFinish, onFailed);
      _tasks.Add(task);
      task.Start();
      return task;
    }

    private void Update()
    {
      var delta = Time.deltaTime;

      for (int i = 0; i < _tasks.Count; i++)
      {
        if (_tasks[i].Finished) _removeList.Add(_tasks[i]);
        else _tasks[i].Tick(delta);
      }

      for (int i = 0; i < _removeList.Count; i++)
      {
        _tasks.Remove(_removeList[i]);
      }

      _removeList.Clear();
    }
  }
}
