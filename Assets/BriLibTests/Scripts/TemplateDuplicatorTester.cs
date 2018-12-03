using System.Collections.Generic;
using UnityEngine;

namespace BriLib.Tests
{
  public class TemplateDuplicatorTester : MonoBehaviour
  {
    public TemplateDuplicator Duplicator;

    private ObservableCollection<TestData> _collection = new ObservableCollection<TestData>();
    private System.Random _random = new System.Random();
    private List<string> _names = new List<string> { "Brian", "Ben", "Bruce", "Batman", "Beachball", "Bob", "Bean" };

    public void OnAddClicked()
    {
      _collection.Add(GetRandomData());
    }

    public void OnUpdateClicked()
    {
      _collection[_random.Next(_collection.Count)] = GetRandomData();
    }

    public void OnInsertClicked()
    {
      _collection.Insert(_random.Next(_collection.Count), GetRandomData());
    }

    public void OnRemoveClicked()
    {
      _collection.Remove(_collection[_random.Next(_collection.Count)]);
    }

    public void OnBindClicked()
    {
      Duplicator.BindOnCollection(_collection);
    }

    public void OnUnbindClicked()
    {
      Duplicator.UnbindOnCollection();
    }

    private TestData GetRandomData()
    {
      return new TestData { Text = _names[_random.Next(_names.Count)] };
    }
  }
}
