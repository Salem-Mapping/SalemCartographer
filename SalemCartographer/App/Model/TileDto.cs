using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SalemCartographer.App.Model
{

  public class TileDto
  {
    // permanent
    public int PosX { get; set; }
    public int PosY { get; set; }
    public string Checksum { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }
    // transient
    public Point Position => new(PosX, PosY);
    public string Path { get; set; }
    private Image Img;

    public TileDto() {

    }

    public Image GetImage() {
      if (Img == null && File.Exists(Path)) {
        Img = Image.FromFile(Path);
      }
      return Img;
    }

    public string GetKey() {
      return String.Format("{0}_{1}", PosX, PosY);
    }

  }

}