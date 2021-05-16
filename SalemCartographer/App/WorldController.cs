using SalemCartographer.App.Model;
using SalemCartographer.App.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalemCartographer.App
{
  class WorldController : AreaController
  {
    private static WorldController _Instance;
    public static WorldController Instance {
      get {
        if (_Instance == null) {
          _Instance = new WorldController();
        }
        return _Instance;
      }
    }

    public override event EventHandler DataChanged;

    public WorldDto World;
    private Dictionary<String, List<String>> KnownFileHashes;

    WorldController() : base() {
      World = new();
      Refresh();
    }

    public void Refresh() {
      string Path = Configuration.GetCartographerPath();
      if (!Directory.Exists(Path)) {
        Debug.WriteLine(String.Format("Directory '{0}' does not exist", Path));
        return;
      }
      World.SetAreas(FetchAreas(Path));
      
    }

    public bool CreateAreaFromSession() {
      SelectAreaForm SelectForm = new(SessionController.Instance.SessionList);
      DialogResult Result = SelectForm.ShowDialog();
      switch (Result) {
        case DialogResult.OK:
        case DialogResult.Yes:
          Create(SelectForm.AreaName, SelectForm.Selected);
          return true;
        case DialogResult.Retry:
        case DialogResult.Cancel:
        case DialogResult.Abort:
        case DialogResult.Ignore:
        case DialogResult.No:
        case DialogResult.None:
        default:
          break;
      }
      return false;
    }

    protected void Create(string AreaName, AreaDto NewArea) {
      string WorldPath = Configuration.GetCartographerPath();
      string WorldAreaPath = Configuration.finalizePath(WorldPath + SecurePath(AreaName));
      if (!Directory.Exists(WorldAreaPath)) {
        Directory.CreateDirectory(WorldAreaPath);
      }
      foreach (var Tile in NewArea.Tiles.Values) {
        string NewFilePath = WorldAreaPath + Tile.FileName;
        File.Copy(Tile.Path, NewFilePath);
      }
      Refresh();
      DataChanged.Invoke(this, new EventArgs());
    }
  }
}
