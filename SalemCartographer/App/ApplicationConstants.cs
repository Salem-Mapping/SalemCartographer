using System.Text.RegularExpressions;

namespace SalemCartographer.App
{
  internal class ApplicationConstants
  {
    public const string ProductName = "Salem Cartographer";
    public const string CurrentSessionFile = "currentsession.js";
    public const string CurrentSessionVar = "currentSession";

    public const char TileDivider = (char)95;
    public const string TileFormat = "tile_{0}_{1}.png";
    public const int TileSize = 100;

    public const string RegexJsVarString = "(?:^|\n)(?:var|let)\\s*(?<var>[\\w\\d]+)\\s*=\\s*(['\"])(?<value>.*?)\\1(?:;|$)";
    public static readonly Regex RegexJsVar = new(RegexJsVarString, RegexOptions.Multiline);
    public const string RegexJsVar_Name = "var";
    public const string RegexJsVar_Value = "value";
  }
}