using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// A form of singleton where we expect some prefab or game object to exist in world. If the game object does not yet exist,
  /// Instance returns null (does not attempt to create the singleton for us). If the game object DOES exist, we bail out,
  /// as we should not attempt to create more than one of a singleton
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class GOSingleton<T> : MonoBehaviour, ISingleton where T : Component, ISingleton
  {
    public static T Instance { get; private set; }

    private void Awake()
    {
      if (Instance != null)
      {
        throw new DuplicateInstanceException("Attempted to initialize already initialized singleton " + typeof(T));
      }
      Instance = gameObject.GetComponent<T>();
      Instance.OnCreate();
    }

    private void Start()
    {
      Begin();
    }

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
