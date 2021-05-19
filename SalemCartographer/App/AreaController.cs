using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalemCartographer.App
{
  internal abstract class AreaController
  {
    private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

    protected AreaController() {
    }

    public abstract string DirectoryPath { get; }
    public abstract AreaType Type { get; }

    public abstract event EventHandler DataChanged;

    protected void RefreshAreas(Dictionary<string, AreaDto> Areas, string DirectoryPath) {
      string[] Paths = Directory.GetDirectories(DirectoryPath);
      foreach (var AreaPath in Paths) {
        AreaDto Area = AreaProcessor.BuildDto(AreaPath);
        Area.Type = Type;
        if (Areas.ContainsKey(Area.Directory)) {
          Area = Areas[Area.Directory];
        }
        AreaProcessor.RefreshDto(Area);
        Areas[Area.Directory] = Area;
      }
    }

    protected List<AreaDto> FetchAreas() {
      return Directory.GetDirectories(DirectoryPath)
        .Select(AreaProcessor.BuildDto)
        .Select(a => {
          a.Type = Type;
          return a;
        })
        .ToList();
    }

    protected bool DeleteArea(AreaDto area) {
      if (area is not AreaDto || area.Type != Type || String.IsNullOrEmpty(area.Path)
        || !area.Path.StartsWith(DirectoryPath) || !Directory.Exists(area.Path)) {
        return false;
      }
      area.Dispose();
      Directory.Delete(area.Path, true);
      return true;
    }

    protected bool DeleteTile(TileDto tile) {
      if (tile is not TileDto || !File.Exists(tile.Path)) {
        return false;
      }
      tile.Dispose();
      File.Delete(tile.Path);
      return true;
    }

    protected string SecurePath(string PathName) {
      return new string(PathName
        .Select(ch => invalidFileNameChars.Contains(ch) ? '_' : ch)
        .ToArray());
    }
  }
}