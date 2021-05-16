using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  class ImageComparator
  {

    public static float Compare(Image bm1, Image bm2) {
      return Compare(new Bitmap(bm1), new Bitmap(bm2));
    }

    public static float Compare(Bitmap bm1, Bitmap bm2) {
      // Make a difference image.
      int wid = Math.Min(bm1.Width, bm2.Width);
      int hgt = Math.Min(bm1.Height, bm2.Height);
      Bitmap bm3 = new (wid, hgt);

      // Create the difference image.
      Color eq_color = Color.White;
      Color ne_color = Color.Red;
      for (int x = 0; x < wid; x++) {
        for (int y = 0; y < hgt; y++) {
          if (bm1.GetPixel(x, y).Equals(bm2.GetPixel(x, y)))
            bm3.SetPixel(x, y, eq_color);
          else {
            bm3.SetPixel(x, y, ne_color);
          }
        }
      }
      return 0;
    }

  }
}
