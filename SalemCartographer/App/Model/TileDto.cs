using System;
using System.Drawing;

namespace SalemCartographer.App.Model
{
  public class TileDto : IDisposable
  {
    public const string KEY_FORMAT = "{0}_{1}";

    // permanent
    public string Path { get; set; }

    public int PosX { get; set; }
    public int PosY { get; set; }
    public string Checksum { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }

    // transient
    public Point Coordinate {
      get => new(PosX, PosY);
      set {
        PosX = value.X;
        PosY = value.Y;
      }
    }

    private WeakReference<Image> _Img;

    public TileDto() {
    }

    public TileDto(TileDto dto) : this() {
      Path = dto.Path;
      PosX = dto.PosX;
      PosY = dto.PosY;
      Checksum = dto.Checksum;
      FileName = dto.FileName;
      Size = dto.Size;
      Date = dto.Date;
      _Img = dto._Img;
    }

    public Image GetImage() {
      if (_Img == null || !_Img.TryGetTarget(out var target)) {
        target = Image.FromFile(Path);
        _Img = new(target);
      }
      return target;
    }

    public string GetKey() {
      return String.Format(KEY_FORMAT, PosX, PosY);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        if (_Img != null && _Img.TryGetTarget(out Image image)) {
          image.Dispose();
        }
        _Img = null;
      }
    }

    public void Dispose() {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}