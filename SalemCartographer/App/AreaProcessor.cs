using SalemCartographer.App.Model;
using SalemCartographer.App.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace SalemCartographer.App
{
  public class AreaProcessor : IProcessor<AreaDto>
  {
    protected AreaDto area;
    protected bool Valid { get; set; }

    public AreaProcessor() {
    }

    public AreaProcessor(string path) {
      SetPath(path);
    }

    public void SetDto(AreaDto dto) {
      area = dto;
      Validate();
    }

    public void SetPath(string path) {
      area = BuildDto(path);
      Validate();
    }

    private void Validate() {
      Valid = Directory.Exists(area.Path);
      RefreshDto(area);
    }

    public bool IsValid() {
      return Valid;
    }

    public AreaDto GetDto() {
      return area;
    }

    public static AreaDto BuildDto(String areaPath) {
      string path = areaPath.EndsWith(Path.DirectorySeparatorChar) ? areaPath[0..^1] : areaPath;
      string finalizedPath = PathUtils.FinalizePath(areaPath);
      AreaDto area = null;
      if (Directory.Exists(path)) {
        area = Load(path);
      }
      if (area == null) {
        string name = Path.GetFileName(path);
        area = new() {
          Path = finalizedPath,
          Name = name,
          Directory = name,
        };
      }
      else if (!finalizedPath.Equals(area.Path)) {
        area.Path = finalizedPath;
        RefreshDto(area);
      }
      return area;
    }

    public static void RefreshDto(AreaDto area) {
      if (!Directory.Exists(area.Path)) {
        return;
      }
      TileProcessor tileProcessor = new();
      string[] files = Directory.GetFiles(area.Path, AppConstants.TileSearchFilter);
      foreach (var file in files) {
        TileDto tile = area.GetTile(TileProcessor.ParseFileName(file));
        if (tile != null && tile is TileDto) {
          tile.Path = file;
          tileProcessor.SetDto(tile);
        }
        else {
          tileProcessor.SetPath(file);
        }
        if (tileProcessor.IsValid()) {
          area.AddTile(tileProcessor.GetDto());
        }
      }
      Store(area);
    }

    public static void Store(AreaDto area) {
      try {
        AreaDto data = new(area);
        string dataFile = PathUtils.FinalizePath(area.Path) + AppConstants.WorldFileName;
        string json = JsonSerializer.Serialize(area);
        if (!String.IsNullOrWhiteSpace(json)) {
          File.WriteAllText(dataFile, json);
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
      }
    }

    public static AreaDto Load(string dictonaryPath) {
      try {
        string dataFile = PathUtils.FinalizePath(dictonaryPath) + AppConstants.WorldFileName;
        if (File.Exists(dataFile)) {
          string json = File.ReadAllText(dataFile);
          if (!String.IsNullOrWhiteSpace(json)) {
            return JsonSerializer.Deserialize<AreaDto>(json);
          }
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
      }
      return null;
    }

  }
}