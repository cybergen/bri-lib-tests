using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// A singleton that you can easily initialize direct from code. Does not expect any existing game object in scene to initialize it,
  /// nor any serialized/initiailized fields set by editor
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Singleton<T> : MonoBehaviour, ISingleton where T : Component, ISingleton
  {
    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          var obj = new GameObject(typeof(T).GetType().ToString());
          obj.AddComponent<T>();
          _instance = obj.GetComponent<T>();
          _instance.OnCreate();
          _instance.Begin();
        }
        return _instance;
      }
      private set
      {
        _instance = value;
      }
    }

    private static T _instance;

    public virtual void OnCreate()
    {
      DontDestroyOnLoad(gameObject);
    }

    public virtual void Begin() { }

    public virtual void End()
    {
      Instance = null;
      Destroy(gameObject);
    }
  }
}
