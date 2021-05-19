using System;

namespace SalemCartographer.App
{
  internal class Configuration
  {
    private static readonly Properties.Settings Settings = Properties.Settings.Default;

    public static string GetSessionsPath() => finalizePath(Settings.PathSession);

    public static string GetCartographerPath() => finalizePath(Settings.PathCartographer);

    public static bool IsFilterSessionActive() => Settings.FilterSessions;

    public static int GetFilterSessionTileMinCount() => Settings.FilterSessionTileMinCount;

    public static string finalizePath(string Path) {
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