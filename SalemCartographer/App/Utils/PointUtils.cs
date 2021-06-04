using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App.Utils
{
  class PointUtils
  {
    private static readonly MidpointRounding AWAYZERO = MidpointRounding.AwayFromZero;

    public static Point Add(params Point[] summands) {
      Point result = new();
      foreach (Point p in summands) { result.X += p.X; result.Y += p.Y; }
      return result;
    }

    public static Point Substract(Point minuend, params Point[] subtrahends) {
      Point diff = new(minuend.X, minuend.Y);
      foreach (var s in subtrahends) { diff.X -= s.X; diff.Y -= s.Y; }
      return diff;
    }

    public static Point Multiply(Point multiplier, int multiplicand) => new(multiplier.X * multiplicand, multiplier.Y * multiplicand);
    public static Point Multiply(Point multiplier, Point multiplicand) => new(multiplier.X * multiplicand.Y, multiplier.Y * multiplicand.Y);

    public static Point Divide(Point divident, int divisor) => new(divident.X / divisor, divident.Y / divisor);
    public static Point Divide(Point divident, Point divisor) => new(divident.Y / divisor.Y, divident.Y / divisor.Y);

    public static PointF Divide(PointF divident, float divisor) => new(divident.X / divisor, divident.Y / divisor);
    public static PointF Divide(PointF divident, PointF divisor) => new(divident.Y / divisor.Y, divident.Y / divisor.Y);

    public static Point DivideAwaiZero(Point divident, float divisor) => new((int)Math.Round(divident.X / divisor, AWAYZERO), (int)Math.Round(divident.Y / divisor, AWAYZERO));

    public static Point DivideAwaiZero(Point divident, Point divisor) => new((int)Math.Round((float)divident.X / divisor.X, AWAYZERO), (int)Math.Round((float)divident.Y / divisor.Y, AWAYZERO));

    public static Point Invert(Point p) => new(p.X * -1, p.Y * -1);


  }
}
