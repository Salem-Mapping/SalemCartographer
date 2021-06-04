using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace SalemCartographer.App.Model
{
  public class TileDto : AbstractModel
  {
    private const string FormatKey = AppConstants.TileKeyFormat;

    // serialize
    [JsonInclude]
    public string Path { get; set; }
    [JsonInclude]
    public int X { get; set; }
    [JsonInclude]
    public int Y { get; set; }
    [JsonInclude]
    public string Hash { get; set; }
    [JsonInclude]
    public string FileName { get; set; }
    [JsonInclude]
    public long Size { get; set; }
    [JsonInclude]
    public DateTime Date { get; set; }

    // transient
    [JsonIgnore]
    public string Key => String.Format(FormatKey, X, Y);
    [JsonIgnore]
    public float? Score;
    [JsonIgnore]
    public Point Coordinate {
      get => new(X, Y);
      set {
        X = value.X;
        Y = value.Y;
      }
    }
    //[JsonIgnore]
    //private Image image;
    [JsonIgnore]
    public Image Image {
      get {
        Image image = null;
        if (image == null && File.Exists(this.Path)) {
          lock (this) {
            try {
              var memStream = new MemoryStream();
              using FileStream fileStream = new(this.Path, FileMode.Open, FileAccess.Read, FileShare.Delete);
              fileStream.CopyTo(memStream);
              image = Image.FromStream(memStream);
            } catch (Exception) { }
          }
        }
        return image;
      }
    }

    public TileDto() {
    }

    public TileDto(TileDto dto) : this() {
      Path = dto.Path;
      X = dto.X;
      Y = dto.Y;
      Hash = dto.Hash;
      FileName = dto.FileName;
      Size = dto.Size;
      Date = dto.Date;
      Score = dto.Score;
    }
  }
}