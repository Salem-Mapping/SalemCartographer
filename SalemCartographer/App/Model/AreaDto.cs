using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
    // transient
    public Dictionary<string, TileDto> Checksums { get; set; }
    // transient
    public List<TileDto> TileList {
      get {
        return Tiles.Values.ToList();
      }
    }

    public string DisplayString {
      get {
        return string.Format(FORMAT_TITLE, Name, Tiles.Count, GetDimensions());
      }
    }

    public string Name { get; internal set; }

    public AreaDto() {
      Tiles = new();
      Checksums = new();
    }

    public void AddTile(TileDto dto) {
      Tiles[dto.GetKey()] = dto;
      Checksums[dto.Checksum] = dto;
    }

    public string GetDimensions() {
      int posXMin = 0;
      int posXMax = 0;
      int posYMin = 0;
      int posYMax = 0;
      foreach (var Tile in Tiles.Values) {
        posXMin = Math.Min(posXMin, Tile.PosX);
        posXMax = Math.Max(posXMax, Tile.PosX);
        posYMin = Math.Min(posYMin, Tile.PosY);
        posYMax = Math.Max(posYMax, Tile.PosY);
      }
      int x = Math.Abs(posXMin - posXMax) + 1;
      int y = Math.Abs(posXMin - posXMax) + 1;
      return String.Format(FORMAT_DIMENSION, x, y);
    }

  }

}