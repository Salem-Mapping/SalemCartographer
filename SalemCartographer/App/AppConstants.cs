using System.Text.RegularExpressions;

namespace SalemCartographer.App
{
  internal class AppConstants
  {
    public const string ProductName = "Salem Cartographer";
    public const string WorldFileName = "cartographer.json";

    public const string CurrentSessionFile = "currentsession.js";
    public const string CurrentSessionVar = "currentSession";

    public const char TileDivider = (char)95;
    public const string TileSearchFilter = "*.png";
    public const string TileFormat = "tile_{0}_{1}.png";
    public const int TileSize = 100;
    public const string TileKeyFormat = "{0}_{1}";

    public const string RegexJsVarString = "(?:^|\n)(?:var|let)\\s*(?<var>[\\w\\d]+)\\s*=\\s*(['\"])(?<value>.*?)\\1(?:;|$)";
    public static readonly Regex RegexJsVar = new(RegexJsVarString, RegexOptions.Multiline);
    public const string RegexJsVar_Name = "var";
    public const string RegexJsVar_Value = "value";

    public static readonly string[] MineTiles = new string[] {
      "50c0eec955907b2e1a6f233b8537551676be61b441360e0430afaef5a138e930"
    };

  }
}