using NUnit.Framework;
using BriLib;

namespace BriLib.Tests
{
  [TestFixture]
  public class PerlinNoiseTests
  {
    [Test]
    public void Repeatability()
    {
      var valueOne = PerlinNoise.Random(0);
      var valueTwo = PerlinNoise.Random(2);
      var valueOneB = PerlinNoise.Random(0);
      var valueTwoB = PerlinNoise.Random(2);

      Assert.AreEqual(valueOne, valueOneB, "For identical inputs, values should be the same");
      Assert.AreEqual(valueTwo, valueTwoB, "For identical inputs, values should be the same");
    }

    [Test]
    public void ScaledRepeatability()
    {
      var scale = 5;
      var valueOne = PerlinNoise.ScaledRandom(0, scale);
      var valueTwo = PerlinNoise.ScaledRandom(2, scale);
      var valueOneB = PerlinNoise.ScaledRandom(0, scale);
      var valueTwoB = PerlinNoise.ScaledRandom(2, scale);

      Assert.AreEqual(valueOne, valueOneB, "For identical inputs, values should be the same");
      Assert.AreEqual(valueTwo, valueTwoB, "For identical inputs, values should be the same");
    }

    [Test]
    public void ChangedSeed()
    {
      PerlinNoise.Seed = 222222f;
      var valueOne = PerlinNoise.Random(0);
      var valueTwo = PerlinNoise.Random(2);

      PerlinNoise.Seed = 7f;
      var valueOneB = PerlinNoise.Random(0);
      var valueTwoB = PerlinNoise.Random(2);

      Assert.AreNotEqual(valueOne, valueOneB, "Values should probably vary with different seeds");
      Assert.AreNotEqual(valueTwo, valueTwoB, "Values should probably vary with different seeds");
    }
  }
}
