using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App.Utils
{
  class PathUtils
  {
    private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

    public static string SecureFileName(string PathName) {
      return new string(PathName
        .Select(ch => invalidFileNameChars.Contains(ch) ? '_' : ch)
        .ToArray());
    }

    public static string FinalizePath(string Path) {
      if (Path.Contains("%")) {
        Path = Environment.ExpandEnvironmentVariables(Path);
      }
      if (!Path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString())) {
        Path += System.IO.Path.DirectorySeparatorChar;
      }
      return Path;
    }
  }
}
