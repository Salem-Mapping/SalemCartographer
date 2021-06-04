using SalemCartographer.App.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;

namespace SalemCartographer.App.Model
{
  public class AreaDto : AbstractModel
  {
    public const string FIELD_TITLE = "DisplayString";
    public const string FORMAT_TITLE = "{0} ({1}) [{2}] {3}";
    public const string FORMAT_DIMENSION = "{0}x{1}";

    // serialize
    [JsonInclude]
    public string Name;
    [JsonInclude]
    public string Path;
    [JsonInclude]
    public string Directory;
    [JsonInclude]
    public string Hash;
    [JsonInclude]
    public Dictionary<string, TileDto> Tiles { get => tiles ??= new(); protected set => tiles = value; }

    // transient
    [JsonIgnore]
    public Point? LastLocation;
    [JsonIgnore]
    private Dictionary<string, TileDto> tiles;
    [JsonIgnore]
    public AreaType Type { get; set; }
    [JsonIgnore]
    public Point? Offset;
    [JsonIgnore]
    public float? Score;
    [JsonIgnore]
    public float? ScoreNormalized;
    [JsonIgnore]
    public List<AreaDto> MatchingAreas;
    [JsonIgnore]
    public Dictionary<string, TileDto> Hashes { get; protected set; }
    [JsonIgnore]
    public List<TileDto> TileList {
      get {
        lock (Tiles) {
          return Tiles.Values.ToList();
        }
      }
    }
    [JsonIgnore]
    public string DisplayString => string.Format(FORMAT_TITLE, Name, Tiles.Count, GetDimensions(), ScoreNormalized);

    public AreaDto(AreaType type) {
      Tiles = new();
      Hashes = new();
      MatchingAreas = new();
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
      Score = dto.Score;
      ScoreNormalized = dto.ScoreNormalized;
      lock (dto.Tiles) {
        Tiles = dto.Tiles.Values.ToDictionary(t => t.Key, t => new TileDto(t));
        if (Tiles == null) {
          Tiles = new();
        }
      }
    }

    public virtual void AddTile(TileDto dto) {
      if (dto == null) {
        return;
      }
      string key = dto.Key;
      if (Tiles.ContainsKey(key)) {
        RemoveTile(dto);
      }
      lock (Tiles) {
        Tiles[key] = dto;
      }
      if (dto.Hash != null) {
        lock (Hashes) {
          Hashes[dto.Hash] = dto;
        }
      }
    }

    public void AddTile(TileDto dto, float? score) {
      if (dto == null) {
        return;
      }
      dto.Score = score;
      AddTile(dto);
    }

    public virtual bool RemoveTile(TileDto dto) {
      if (dto.Hash != null) {
        lock (Hashes) {
          Hashes.Remove(dto.Hash);
        }
      }
      lock (Tiles) {
        return Tiles.Remove(dto.Key);
      }
    }

    public virtual TileDto GetTile(Point pos) {
      return GetTile(pos.X, pos.Y);
    }

    public virtual TileDto GetTile(int x, int y) {
      return GetTileByKey(String.Format(AppConstants.TileKeyFormat, x, y));
    }

    public virtual TileDto GetTileByKey(string key) {
      lock (Tiles) {
        if (Tiles.ContainsKey(key)) {
          return Tiles[key];
        }
      }
      return null;
    }

    public void ClearTiles() {
      lock (Tiles) {
        Tiles.Clear();
      }
      lock (Hashes) {
        Hashes.Clear();
      }
    }

    public string GetDimensions() {
      int posXMin = 0;
      int posXMax = 0;
      int posYMin = 0;
      int posYMax = 0;
      lock (Tiles) {
        foreach (var Tile in Tiles.Values) {
          posXMin = Math.Min(posXMin, Tile.X);
          posXMax = Math.Max(posXMax, Tile.X);
          posYMin = Math.Min(posYMin, Tile.Y);
          posYMax = Math.Max(posYMax, Tile.Y);
        }
      }
      int x = Math.Abs(posXMin - posXMax) + 1;
      int y = Math.Abs(posYMin - posYMax) + 1;
      return String.Format(FORMAT_DIMENSION, x, y);
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