using SalemCartographer.App.Model;
using SalemCartographer.App.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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
    private readonly Dictionary<String, WeakList<AreaDto>> KnownFileHashes;

    WorldController() : base() {
      World = new();
      KnownFileHashes = new();
      Refresh();
    }

    public void Refresh() {
      string path = Configuration.GetCartographerPath();
      if (!Directory.Exists(path)) {
        return;
      }
      RefreshAreas(World.Areas, path);
      RefreshHashes();
    }

    protected void RefreshHashes() {
      KnownFileHashes.Clear();
      foreach (var area in World.AreaList) {
        foreach (var tile in area.TileList) {
          if (!KnownFileHashes.ContainsKey(tile.Checksum)) {
            KnownFileHashes.Add(tile.Checksum, new WeakList<AreaDto>());
          }
          if (!KnownFileHashes[tile.Checksum].Contains(area)) {
            KnownFileHashes[tile.Checksum].Add(area);
          }
        }
      }
    }

    public IList<AreaDto> GetKnownAreas(AreaDto area) {
      List<AreaDto> matchingAreas = new();
      ISet<AreaDto> knownAreas = new HashSet<AreaDto>();
      foreach (var item in area.TileList) {
        if (KnownFileHashes.ContainsKey(item.Checksum)) {
          knownAreas.UnionWith(KnownFileHashes[item.Checksum]);
        }
      }
      if (knownAreas.Count == 0) {
        return matchingAreas;
      }
      foreach (var knownArea in knownAreas) {
        bool matches = true;
        try {
          TileDto tileSource1 = area.TileList
            .Where(t => knownArea.Checksums.TryGetValue(t.Checksum, out var target))
            .First();
          TileDto tileMatch1 = knownArea.Checksums[tileSource1.Checksum];
          Point Diff = new(tileMatch1.PosX - tileSource1.PosX, tileMatch1.PosY - tileSource1.PosY);
          foreach (var tileSource in area.TileList) {
            Point p = tileSource.Coordinate;
            p.Offset(Diff);
            string key = String.Format(TileDto.KEY_FORMAT, p.X, p.Y);
            TileDto tileMatch = knownArea.Tiles[key];
            if (tileMatch != null && !tileSource.Checksum.Equals(tileSource.Checksum)) {
              throw new Exception("missmatching");
            }
          }
        } catch (Exception e) {
          Debug.WriteLine(e);
          matches = false;
        }
        if (matches) {
          matchingAreas.Add(knownArea);
        }
      }
      return matchingAreas;
    }

    public bool CreateAreaFromSession() {
      SelectAreaForm selectForm = new(SessionController.Instance.SessionList);
      DialogResult result = selectForm.ShowDialog();
      switch (result) {
        case DialogResult.OK:
        case DialogResult.Yes:
          Create(selectForm.AreaName, selectForm.Selected);
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

    protected void Create(string areaName, AreaDto newArea) {
      string worldPath = Configuration.GetCartographerPath();
      string worldAreaPath = Configuration.finalizePath(worldPath + SecurePath(areaName));
      if (!Directory.Exists(worldAreaPath)) {
        Directory.CreateDirectory(worldAreaPath);
      }
      foreach (var tile in newArea.Tiles.Values) {
        string newFilePath = worldAreaPath + tile.FileName;
        File.Copy(tile.Path, newFilePath);
      }
      Refresh();
      DataChanged.Invoke(this, new EventArgs());
    }
  }
}
