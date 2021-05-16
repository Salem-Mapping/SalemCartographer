using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SalemCartographer.App.Model;

namespace SalemCartographer.App
{
  class AreaProcessor : IProcessor<AreaDto>
  {
    protected string AreaPath { get; set; }
    protected bool Valid { get; set; }

    public AreaProcessor() {
    }

    public AreaProcessor(string Path) {
      SetPath(Path);
    }

    public void SetPath(string Path) {
      AreaPath = Path;
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

    public static AreaDto BuildDto(String AreaPath) {
      AreaDto Area = new() {
        Path = AreaPath,
        Directory = AreaPath != null ? Path.GetFileName(AreaPath) : AreaPath,
      };
      if (!Directory.Exists(AreaPath)) {
        return Area;
      }
      TileProcessor TileProcessor = new TileProcessor();
      string[] Files = Directory.GetFiles(Area.Path);
      foreach (var File in Files) {
        TileProcessor.SetPath(File);
        if (TileProcessor.IsValid()) {
          Area.AddTile(TileProcessor.GetDto());
        }
      }
      return Area;
    }

  }
}
