using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  class ApplicationConstants
  {
    public const string ProductName = "Salem Cartographer";
    public const char TileDivider = (char)95;
    public const int TileSize = 100;
    public const string CurrentSessionFile = "currentsession.js";
    public const string CurrentSessionVar = "currentSession";
    public static readonly Regex RegexJsVar = new (
      "(?:^|\n)(?:var|let)\\s*(?<var>[\\w\\d]+)\\s*=\\s*(['\"])(?<value>.*?)\\1(?:;|$)",
      RegexOptions.Multiline);
    public const string RegexJsVar_Name = "var";
    public const string RegexJsVar_Value = "value";
  }
}
