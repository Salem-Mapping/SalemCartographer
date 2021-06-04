using SalemCartographer.App.Utils;
using System;

namespace SalemCartographer.App
{
  internal class Configuration
  {
    private static readonly Properties.Settings Settings = Properties.Settings.Default;

    public static string GetSessionsPath() => PathUtils.FinalizePath(Settings.PathSession);

    public static string GetCartographerPath() => PathUtils.FinalizePath(Settings.PathCartographer);

    public static double GetAutoMergeMinScore() => 3;

    public static double GetAutoMergeMinScoreNormalized() => 0.8;

    public static bool LiveMergeEnabled() => true;

    public static bool ShouldDeleteMineTiles() => true;

    public static bool IsFilterSessionActive() => Settings.FilterSessions;

    public static int GetFilterSessionTileMinCount() => Settings.FilterSessionTileMinCount;

  }
}