using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  public class ExtensionTests
  {
    [Test]
    public void RangeMap()
    {
      var value = 5f.MapRange(0, 10, 0, 1);
      Assert.AreEqual(0.5f, value, "5 out of 10 should map to 0.5 on scale of 0 to 1");
    }

    [Test]
    public void RangeMapFlipSign()
    {
      var value = 5f.MapRange(0, 10, -10, -6);
      Assert.AreEqual(-8f, value, "5 out of 10 should map to -8 on a scale of -10 to -6");
    }
  }
}
