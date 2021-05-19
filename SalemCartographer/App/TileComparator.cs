using SalemCartographer.App.Model;
using System;
using System.Drawing;

namespace SalemCartographer.App
{
  internal class TileComparator
  {
    public static float Compare(TileDto t1, TileDto t2) {
      float tileScore;
      if (t1.Checksum.Equals(t2.Checksum)) {
        tileScore = 1;
      } else {
        tileScore = Compare(t1.GetImage(), t2.GetImage());
      }
      return tileScore;
    }

    public static float Compare(Image bm1, Image bm2) {
      return Compare(new Bitmap(bm1), new Bitmap(bm2));
    }

    public static float Compare(Bitmap bm1, Bitmap bm2) {
      float score = 0;
      int widthMin = Math.Min(bm1.Width, bm2.Width);
      int widthMax = Math.Max(bm1.Width, bm2.Width);
      int heightMin = Math.Min(bm1.Height, bm2.Height);
      int heightMax = Math.Max(bm1.Height, bm2.Height);
      int count = widthMax * heightMax;
      for (int x = 0; x < widthMin; x++) {
        for (int y = 0; y < heightMin; y++) {
          Color c1 = bm1.GetPixel(x, y);
          Color c2 = bm2.GetPixel(x, y);
          //int eq = c1.Equals(c2) ? 1 : 0;
          //matches += eq;
          if (c1.Equals(c2)) {
            score += 1;
            //} else {
            //  float v = (float)(1 - ColourDistance(c1, c2));
            //  score += v;
          }
        }
      }
      float normalized = score / count;
      return normalized;
    }

    public static double ColourDistance(Color e1, Color e2) {
      var h1 = e1.GetHue();
      var h2 = e2.GetHue();
      var s1 = e1.GetSaturation();
      var s2 = e2.GetSaturation();
      var v1 = e1.GetBrightness();
      var v2 = e2.GetBrightness();

      return Math.Pow(Math.Sin(h1) * s1 * v1 - Math.Sin(h2) * s2 * v2, 2)
          + Math.Pow(Math.Cos(h1) * s1 * v1 - Math.Cos(h2) * s2 * v2, 2)
          + Math.Pow(v1 - v2, 2);
    }
  }
}