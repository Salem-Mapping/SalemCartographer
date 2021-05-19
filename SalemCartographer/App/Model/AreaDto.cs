using SalemCartographer.App.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalemCartographer.App.Model
{
  public class AreaDto : IDisposable
  {
    public const string FIELD_TITLE = "DisplayString";
    public const string FORMAT_TITLE = "{0} ({1}) [{2}]";
    public const string FORMAT_DIMENSION = "{0}x{1}";

    public string Path { get; set; }
    public string Directory { get; set; }
    public string Hash { get; set; }
    public Dictionary<string, TileDto> Tiles { get; protected set; }

    // transient
    public AreaType Type { get; set; }

    // transient
    public Dictionary<string, TileDto> Checksums { get; protected set; }

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

    public AreaDto(AreaType type) {
      Tiles = new();
      Checksums = new();
      Type = type;
    }

    public AreaDto() : this(AreaType.Preview) {
    }

    public AreaDto(AreaDto dto) : this(dto.Type) {
      Path = dto.Path;
      Name = dto.Name;
      Directory = dto.Directory;
      Hash = dto.Hash;
      Type = dto.Type;
      Tiles = dto.Tiles.Values.ToDictionary(t => t.GetKey(), t => new TileDto(t));
    }

    public virtual void AddTile(TileDto dto) {
      string key = dto.GetKey();
      if (Tiles.ContainsKey(key)) {
        RemoveTile(dto);
      }
      Tiles.Add(key, dto);
      if (dto.Checksum != null) {
        Checksums[dto.Checksum] = dto;
      }
    }

    public virtual bool RemoveTile(TileDto dto) {
      if (dto.Checksum != null) {
        Checksums.Remove(dto.Checksum);
      }
      return Tiles.Remove(dto.GetKey());
    }

    public void ClearTiles() {
      Tiles.Clear();
      Checksums.Clear();
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
      int y = Math.Abs(posYMin - posYMax) + 1;
      return String.Format(FORMAT_DIMENSION, x, y);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        foreach (var tile in TileList) {
          tile.Dispose();
        }
      }
    }

    public void Dispose() {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    public override bool Equals(object obj) {
      if (obj == null || obj is not AreaDto area) {
        return false;
      }
      return Path.Equals(area.Path);
    }

    public override int GetHashCode() {
      return Path.GetHashCode();
    }
  }
}