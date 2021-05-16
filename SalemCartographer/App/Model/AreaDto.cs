using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SalemCartographer.App.Model
{

  public class AreaDto
  {
    public const string FIELD_TITLE = "DisplayString";
    public const string FORMAT_TITLE = "{0} ({1}) [{2}]";
    public const string FORMAT_DIMENSION = "{0}x{1}";

    public string Path { get; set; }
    public string Directory { get; set; }
    public string Hash { get; set; }
    public Dictionary<string, TileDto> Tiles { get; set; }
    public string DisplayString {
      get {
        return string.Format(FORMAT_TITLE, Directory, Tiles.Count, GetDimensions());
      }
    }

    public AreaDto() {
      Tiles = new Dictionary<string, TileDto>();
    }

    public void AddTile(TileDto Dto) {
      if (Tiles == null) {
        Tiles = new Dictionary<string, TileDto>();
      }
      string Key = Dto.GetKey();
      if (!Tiles.ContainsKey(Key)) {
        Tiles.Add(Key, Dto);
      }
    }

    public string GetDimensions() {
      int PosXMin = 0;
      int PosXMax = 0;
      int PosYMin = 0;
      int PosYMax = 0;
      foreach (var Tile in Tiles.Values) {
        PosXMin = Math.Min(PosXMin, Tile.PosX);
        PosXMax = Math.Max(PosXMax, Tile.PosX);
        PosYMin = Math.Min(PosYMin, Tile.PosY);
        PosYMax = Math.Max(PosYMax, Tile.PosY);
      }
      int X = Math.Abs(PosXMin - PosXMax) + 1;
      int Y = Math.Abs(PosXMin - PosXMax) + 1;
      return String.Format(FORMAT_DIMENSION, X, Y);
    }

  }

}