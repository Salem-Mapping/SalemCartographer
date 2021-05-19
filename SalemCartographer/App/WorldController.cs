using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using SalemCartographer.App.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SalemCartographer.App
{
  internal class WorldController : AreaController
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

    public override string DirectoryPath => Configuration.GetCartographerPath();
    public override AreaType Type => AreaType.World;
    public WorldDto World;
    private readonly Dictionary<String, WeakList<AreaDto>> KnownFileHashes;

    private WorldController() : base() {
      World = new();
      KnownFileHashes = new();
      Refresh();
    }

    public void Refresh() {
      string path = DirectoryPath;
      if (!Directory.Exists(path)) {
        return;
      }
      RefreshAreas(World.Areas, path);
      RefreshHashes();
      DataChanged?.Invoke(this, EventArgs.Empty);
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

    public List<MatchedAreaDto> GetKnownAreas(AreaDto area) {
      List<MatchedAreaDto> matchingAreas = new();
      ISet<AreaDto> knownAreas = new HashSet<AreaDto>();
      foreach (var item in area.TileList) {
        if (KnownFileHashes.ContainsKey(item.Checksum)) {
          knownAreas.UnionWith(KnownFileHashes[item.Checksum]);
        }
      }
      if (knownAreas.Count == 0) {
        Debug.WriteLine(String.Format("Area '{0}' => no matching areas found", area.Directory));
        return matchingAreas;
      }
      foreach (var knownArea in knownAreas) {
        MatchedAreaDto match = new(knownArea);
        float score = 0;
        int count = 0;
        try {
          TileDto tileSource1 = area.TileList
            .Where(t => knownArea.Checksums.TryGetValue(t.Checksum, out var target))
            .First();
          TileDto tileMatch1 = knownArea.Checksums[tileSource1.Checksum];
          Point Offset = new(tileMatch1.PosX - tileSource1.PosX, tileMatch1.PosY - tileSource1.PosY);
          match.Offset = Offset;
          foreach (var tileSource in area.TileList) {
            try {
              Point p = tileSource.Coordinate;
              p.Offset(Offset);
              string key = String.Format(TileDto.KEY_FORMAT, p.X, p.Y);
              if (!knownArea.Tiles.ContainsKey(key)) {
                continue;
              }
              TileDto matchTile = knownArea.Tiles[key];
              count++;
              float sourceTile = TileComparator.Compare(tileSource, matchTile);
              score += sourceTile;
              match.AddTile(matchTile, sourceTile);
            } catch (Exception e) {
              Debug.WriteLine(e);
            }
          }
        } catch (Exception e) {
          Debug.WriteLine(e);
          score = 0;
        }
        float normalized = score / count;
        Debug.WriteLine(String.Format("Area '{5}' => '{0}' {1} ({2}/{3}) -> {4}", knownArea.Directory, normalized, score, count, match.Offset, area.Directory));
        if (score > 3 && normalized > 0.8) {
          match.Score = score;
          match.ScoreNormalized = normalized;
          matchingAreas.Add(match);
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

    protected bool Create(string areaName, AreaDto newArea) {
      string worldPath = DirectoryPath;
      string worldAreaPath = Configuration.FinalizePath(worldPath + SecurePath(areaName));
      if (newArea.Type == Type || newArea.Path.StartsWith(worldPath)) {
        return false;
      }
      if (!Directory.Exists(worldAreaPath)) {
        Directory.CreateDirectory(worldAreaPath);
      }
      foreach (var tile in newArea.Tiles.Values) {
        string newFilePath = worldAreaPath + tile.FileName;
        File.Copy(tile.Path, newFilePath);
      }
      Refresh();
      DataChanged.Invoke(this, new EventArgs());
      return true;
    }

    public bool Merge(AreaDto sourceArea, AreaDto targetArea, Point offset) {
      Stopwatch watch = new();
      watch.Start();
      bool result = false;
      if (sourceArea.Type == Type || targetArea.Type != Type
        || !World.Areas.ContainsKey(targetArea.Directory)) {
        return result;
      }
      AreaDto orginalArea = World.Areas[targetArea.Directory];
      string targetPath = orginalArea.Path;

      try {
        foreach (var sourceTile in sourceArea.TileList) {
          TileDto newTile = new(sourceTile);
          Point coord = sourceTile.Coordinate;
          coord.Offset(offset);
          newTile.Coordinate = coord;
          if (!File.Exists(sourceTile.Path)) {
            continue;
          }
          TileDto orginalTile = null;
          if (orginalArea.Tiles.ContainsKey(newTile.GetKey())) {
            orginalTile = orginalArea.Tiles[newTile.GetKey()];
            orginalTile.Dispose();
          }
          newTile.FileName = TileProcessor.GenerateFileName(newTile);
          newTile.Path = targetPath + newTile.FileName;
          File.Copy(sourceTile.Path, newTile.Path, true);
          if (orginalTile != null) {
            orginalArea.RemoveTile(orginalTile);
          }
          orginalArea.AddTile(newTile);
          result = true;
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
      }
      if (result) {
        Refresh();
        if (sourceArea.Type == AreaType.Session) {
          SessionController.Instance.Delete(sourceArea);
        }
      }
      Debug.WriteLine("merge finished after {0} ms", watch.ElapsedMilliseconds);
      watch.Stop();
      return result;
    }

    public bool Delete(AreaDto area) {
      if (World.Areas.ContainsKey(area.Directory)) {
        World.Areas.Remove(area.Directory);
      }
      return DeleteArea(area);
    }
  }
}