using SalemCartographer.App.Model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace SalemCartographer.App
{
  internal class TileProcessor : IProcessor<TileDto>
  {
    public TileDto Tile { get; protected set; }
    public bool Valid { get; protected set; }

    public TileProcessor() {
    }

    public TileProcessor(string path) {
      SetPath(path);
    }

    public void SetPath(string path) {
      Tile = BuildDto(path);
      Validate();
      RefreshDto(Tile);
    }

    public void SetDto(TileDto dto) {
      Tile = dto;
      Validate();
      RefreshDto(Tile);
    }

    private void Validate() {
      Valid = File.Exists(Tile.Path);
    }

    public bool IsValid() {
      return Valid;
    }

    public TileDto GetDto() {
      return Tile;
    }

    public static TileDto BuildDto(string path) {
      TileDto tile = new() { Path = path };
      tile.FileName = tile.Path != null ? Path.GetFileName(tile.Path) : tile.Path;
      tile.Coordinate = ParseFileName(tile.FileName);
      return tile;
    }

    public TileDto RefreshDto(TileDto tile) {
      if (!File.Exists(tile.Path)) {
        return tile;
      }
      FileInfo FileInfo = new(Tile.Path);
      if (!tile.Date.Equals(FileInfo.LastWriteTime) || tile.Size != FileInfo.Length || String.IsNullOrWhiteSpace(tile.Hash)) {
        tile.Size = FileInfo.Length;
        tile.Date = FileInfo.LastWriteTime;
        tile.Hash = TileComparator.Hash(tile);
      }
      return tile;
    }

    public static Point ParseFileName(string fileName) {
      string FileName = Path.GetFileNameWithoutExtension(fileName);
      String[] FileParts = FileName.Split(AppConstants.TileDivider);
      if (FileParts.Length >= 3) {
        return new(int.Parse(FileParts[1]), int.Parse(FileParts[2]));
      }
      throw new Exception("Filename is not valid");
    }

    public static string GenerateFileName(TileDto newTile) {
      return String.Format(AppConstants.TileFormat, newTile.X, newTile.Y);
    }

  }
}