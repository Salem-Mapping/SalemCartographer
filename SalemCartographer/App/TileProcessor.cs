using SalemCartographer.App.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace SalemCartographer.App
{
  internal class TileProcessor : IProcessor<TileDto>
  {
    private static readonly SHA256 Sha256 = SHA256.Create();
    protected string TilePath { get; set; }
    protected bool Valid { get; set; }

    public TileProcessor() {
    }

    public TileProcessor(string Path) {
      SetPath(Path);
    }

    public void SetPath(string Path) {
      TilePath = Path;
      Validate();
    }

    private void Validate() {
      Valid = File.Exists(TilePath);
    }

    public bool IsValid() {
      return Valid;
    }

    public TileDto GetDto() {
      if (!IsValid()) {
        return null;
      }
      return BuildDto(TilePath);
    }

    private static TileDto BuildDto(String TilePath) {
      TileDto Tile = new() {
        Path = TilePath,
        FileName = TilePath != null ? Path.GetFileName(TilePath) : TilePath
      };
      string FileName = Path.GetFileNameWithoutExtension(Tile.FileName);
      String[] FileParts = FileName.Split(ApplicationConstants.TileDivider);
      if (FileParts.Length >= 3) {
        try {
          Tile.PosX = int.Parse(FileParts[1]);
          Tile.PosY = int.Parse(FileParts[2]);
        } catch (Exception e) {
          Debug.WriteLine(e);
        }
      }
      if (!File.Exists(TilePath)) {
        return Tile;
      }
      FileInfo FileInfo = new(TilePath);
      if (!Tile.Date.Equals(FileInfo.LastWriteTime)
        || Tile.Size != FileInfo.Length) {
        Tile.Size = FileInfo.Length;
        Tile.Date = FileInfo.LastWriteTime;
        Tile.Checksum = GetChecksum(TilePath);
      }
      return Tile;
    }

    public static string GetChecksum(string TilePath) {
      try {
        return BytesToString(GetHashSha256(TilePath));
      } catch (Exception e) {
        Console.WriteLine(e);
        return null;
      }
    }

    private static byte[] GetHashSha256(string filename) {
      using FileStream stream = File.OpenRead(filename);
      return Sha256.ComputeHash(stream);
    }

    private static string BytesToString(byte[] bytes) {
      string result = "";
      foreach (byte b in bytes) result += b.ToString("x2");
      return result;
    }

    internal static string GenerateFileName(TileDto newTile) {
      return String.Format(ApplicationConstants.TileFormat, newTile.PosX, newTile.PosY);
    }
  }
}