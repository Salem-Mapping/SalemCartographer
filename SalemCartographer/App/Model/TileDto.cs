using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SalemCartographer.App.Model
{

  public class TileDto
  {
    public const string KEY_FORMAT = "{0}_{1}";

    // permanent
    public int PosX { get; set; }
    public int PosY { get; set; }
    public string Checksum { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }
    // transient
    public Point Coordinate => new(PosX, PosY);
    public string Path { get; set; }

    private WeakReference<Image> _Img;

    public TileDto() {

    }

    public Image GetImage() {
      if (_Img ==null || !_Img.TryGetTarget(out var target)) {
        target = Image.FromFile(Path);
        _Img = new(target);
      }
      return target;
    }

    public string GetKey() {
      return String.Format(KEY_FORMAT, PosX, PosY);
    }

  }

}