using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  abstract class AreaController
  {
    static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

    protected AreaController() {

    }

    public abstract event EventHandler DataChanged;

    protected static void RefreshAreas(Dictionary<string, AreaDto> Areas, string DirectoryPath) {
      string[] Paths = Directory.GetDirectories(DirectoryPath);
      foreach (var AreaPath in Paths) {
        AreaDto Area = AreaProcessor.BuildDto(AreaPath);
        if (Areas.ContainsKey(Area.Name)) {
          Area = Areas[Area.Name];
        }
        AreaProcessor.RefreshDto(Area);
        Areas[Area.Name] = Area;
      }
    }

    protected static List<AreaDto> FetchAreas(string Path) {
      return Directory.GetDirectories(Path).Select(AreaProcessor.BuildDto).ToList();
    }

    protected static string SecurePath(string PathName) {
      return new string(PathName
        .Select(ch => invalidFileNameChars.Contains(ch) ? '_' : ch)
        .ToArray());
    }

  }
}
