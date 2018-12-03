using UnityEngine;

namespace BriLib.Tests
{
    public class PerlinTester : MonoBehaviour
    {
        public bool GenerateTexture = false;
        public int TextureSize = 1024;
        public Material Material;

        private void Update()
        {
            if (GenerateTexture)
            {
                GenerateTexture = false;
                var tex = new Texture2D(TextureSize, TextureSize, TextureFormat.RGB24, true);
                tex.name = "Test";
                for (int y = 0; y < TextureSize; y++)
                {
                    for (int x = 0; x < TextureSize; x++)
                    {
                        var lightness = PerlinNoise.Random01(y * TextureSize + x);
                        tex.SetPixel(x, y, new Color(lightness, lightness, lightness));
                    }
                }
                tex.Apply();
                Material.mainTexture = tex;
            }
        }
    }
}
