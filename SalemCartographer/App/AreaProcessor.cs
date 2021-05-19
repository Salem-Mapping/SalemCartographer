using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SalemCartographer.App.Model;

namespace SalemCartographer.App
{
  internal class AreaProcessor : IProcessor<AreaDto>
  {
    protected string AreaPath { get; set; }
    protected bool Valid { get; set; }

    public AreaProcessor() {
    }

    public AreaProcessor(string path) {
      SetPath(path);
    }

    public void SetPath(string path) {
      AreaPath = path;
      Validate();
    }

    private void Validate() {
      Valid = Directory.Exists(AreaPath);
    }

    public bool IsValid() {
      return Valid;
    }

    public AreaDto GetDto() {
      return BuildDto(AreaPath);
    }

    public static AreaDto BuildDto(String areaPath) {
      string name = areaPath != null ? Path.GetFileName(areaPath) : areaPath;
      AreaDto Area = new() {
        Path = Configuration.finalizePath(areaPath),
        Name = name,
        Directory = name,
      };
      return Area;
    }

    public static void RefreshDto(AreaDto area) {
      if (!Directory.Exists(area.Path)) {
        return;
      }
      TileProcessor tileProcessor = new();
      string[] files = Directory.GetFiles(area.Path);
      foreach (var file in files) {
        tileProcessor.SetPath(file);
        if (tileProcessor.IsValid()) {
          area.AddTile(tileProcessor.GetDto());
        }
      }
    }
  }
}