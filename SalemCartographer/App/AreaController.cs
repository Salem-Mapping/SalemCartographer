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

    protected List<AreaDto> FetchAreas(string Path) {
      return Directory.GetDirectories(Path).Select(AreaProcessor.BuildDto).ToList();
    }

    protected static string SecurePath(string PathName) {
      return new string(PathName
        .Select(ch => invalidFileNameChars.Contains(ch) ? '_' : ch)
        .ToArray());
    }

  }
}
