using SalemCartographer.App.Model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SalemCartographer.App
{
  internal class TileComparator
  {

    public static string Hash(TileDto tile) {
      try {
        lock (tile) {
          return Hash(tile.Image);
        }
      } catch (Exception e) {
        Console.WriteLine(e);
      }
      return null;
    }

    public static string Hash(Image image) => image != null ? Hash(new Bitmap(image)) : null;

    public static string Hash(Bitmap bitmap) {
      try {
        ImageConverter converter = new();
        //create 2 byte arrays, one for each image
        byte[] imgBytes1 = new byte[1];

        //convert images to byte array
        imgBytes1 = (byte[])converter.ConvertTo(bitmap, imgBytes1.GetType());
        bitmap.Dispose();

        //now compute a hash for each image from the byte arrays
        SHA256Managed sha = new();
        byte[] imgHash1 = sha.ComputeHash(imgBytes1);
        //string result = Encoding.Default.GetString(imgHash1);
        string result = BytesToString(imgHash1);
        return result;
      } catch (Exception ex) {
        Debug.WriteLine("TileComparator: " + ex);
      }
      return null;
    }

    public static float Compare(TileDto t1, TileDto t2) {
      float tileScore;
      if (t1.Hash != null && t2.Hash != null && t1.Hash.Equals(t2.Hash)) {
        tileScore = 1;
      } else {
        lock (t1) lock (t2) using (Bitmap bm1 = new(t1.Path), bm2 = new(t2.Path)) {
              tileScore = Compare(bm1, bm2);
            }
      }
      return tileScore;
    }

    public static float Compare(Image img1, Image img2) {
      float score;
      lock (img1) lock (img2) using (Bitmap bm1 = new(img1), bm2 = new(img2)) {
            score = Compare(bm1, bm2);
          }
      return score;
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
      bm1.Dispose();
      bm2.Dispose();
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

    private static readonly SHA256 sha256 = SHA256.Create();
    public static byte[] GetHashSha256(Stream stream) {
      try {
        return sha256.ComputeHash(stream);
      } catch (Exception e) {
        Debug.WriteLine("TileComparator: " + e);
      } finally {
        stream.Dispose();
      }
      return Array.Empty<byte>();
    }

    public static string BytesToString(byte[] bytes) {
      string result = "";
      foreach (byte b in bytes) result += b.ToString("x2");
      return result;
    }

  }
}