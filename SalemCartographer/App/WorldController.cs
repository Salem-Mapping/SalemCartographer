using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using SalemCartographer.App.UI;
using SalemCartographer.App.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalemCartographer.App
{
  internal class WorldController : AbstractAreaController
  {
    private static WorldController instance;
    private static readonly object intanceLock = new();
    public static WorldController Instance {
      get {
        lock (intanceLock) {
          if (instance == null) {
            instance = new WorldController();
          }
        }
        return instance;
      }
    }

    public static string worldMapsDirectoryName = "maps";

    public override IDictionary<string, AreaDto> Areas => World.Areas;
    public override string DirectoryPath => PathUtils.FinalizePath(Configuration.GetCartographerPath() + worldMapsDirectoryName);
    public override AreaType Type => AreaType.World;
    public WorldDto World;

    private readonly string DataFile = Configuration.GetCartographerPath() + AppConstants.WorldFileName;
    private readonly Dictionary<String, WeakList<AreaDto>> knownFileHashes;

    private WorldController() : base() {
      if (!Directory.Exists(DirectoryPath)) {
        return;
      }
      LoadData();
      if (World == null) {
        World = new();
      }
      knownFileHashes = new();
      RefreshAreasAsync();
    }

    public void Refresh() {
      RefreshAreasAsync();
    }

    protected override void RefreshAreas() {
      base.RefreshAreas();
      RefreshHashes();
      StoreData();
    }

    protected void RefreshHashes() {
      lock (knownFileHashes) {
        knownFileHashes.Clear();
        foreach (var area in World.AreaList) {
          foreach (var tile in area.TileList) {
            if (!knownFileHashes.ContainsKey(tile.Hash)) {
              knownFileHashes.Add(tile.Hash, new WeakList<AreaDto>());
            }
            if (!knownFileHashes[tile.Hash].Contains(area)) {
              knownFileHashes[tile.Hash].Add(area);
            }
          }
        }
      }
    }

    public List<AreaDto> GetKnownAreas(AreaDto area) {
      List<AreaDto> matchingAreas = new();
      ISet<AreaDto> knownAreas = new HashSet<AreaDto>();
      lock (knownFileHashes) {
        if (!knownFileHashes.Any()) {
          return matchingAreas;
        }
        foreach (var item in area.TileList) {
          if (item.Hash == null) { continue; }
          if (knownFileHashes.ContainsKey(item.Hash)) {
            knownAreas.UnionWith(knownFileHashes[item.Hash]);
          }
        }
      }
      if (knownAreas.Count == 0) {
   //     Debug.WriteLine(String.Format("Area '{0}' => no matching areas found", area.Directory));
        return matchingAreas;
      }
      foreach (var knownArea in knownAreas) {
        AreaDto match = new(knownArea);
        float score = 0;
        int count = 0;
        try {
          TileDto tileSource1 = area.TileList
            .Where(t => knownArea.Hashes.TryGetValue(t.Hash, out var target))
            .First();
          TileDto tileMatch1 = knownArea.Hashes[tileSource1.Hash];
          Point Offset = new(tileMatch1.X - tileSource1.X, tileMatch1.Y - tileSource1.Y);
          match.Offset = Offset;
          foreach (var tileSource in area.TileList) {
            try {
              Point p = tileSource.Coordinate;
              p.Offset(Offset);
              string key = String.Format(AppConstants.TileKeyFormat, p.X, p.Y);
              if (!knownArea.Tiles.ContainsKey(key)) {
                continue;
              }
              TileDto matchTile = new(knownArea.Tiles[key]);
              count++;
              float sourceTile = TileComparator.Compare(tileSource, matchTile);
              score += sourceTile;
              match.AddTile(matchTile, sourceTile);
            } catch (Exception e) {
              Debug.WriteLine(this.GetType().Name + ": " + e);
            }
          }
        } catch (Exception e) {
          Debug.WriteLine(this.GetType().Name + ": " + e);
          score = 0;
        }
        float normalized = score / count;
        Debug.WriteLine(String.Format("Area '{5}' => '{0}' {1} ({2}/{3}) -> {4}", knownArea.Directory, normalized, score, count, match.Offset, area.Directory));
        if (score > 0.3 && normalized > 0.3) {
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
      string worldAreaPath = PathUtils.FinalizePath(worldPath + PathUtils.SecureFileName(areaName));
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
      return true;
    }

    public void AutoMerge(IEnumerable<AreaDto> sessions) {
      double minScore = Configuration.GetAutoMergeMinScore();
      double minScoreNormalized = Configuration.GetAutoMergeMinScoreNormalized();
      foreach (AreaDto session in sessions) {
        if (session.Type == Type) {
          continue;
        }

        if (!session.MatchingAreas.Any()) {
          session.MatchingAreas = GetKnownAreas(session);
        }
        var filteredAreas = session.MatchingAreas
             .Where(m => m.Score > minScore)
             .Where(m => m.ScoreNormalized > minScoreNormalized);
        if (filteredAreas.Count() == 1) {
          AreaDto area = filteredAreas.Single();
          Merge(session, area, area.Offset.Value);
        }
      }
    }

    private CancellationTokenSource cancelAutoMerge;
    public async void AutoMergeAsync(IEnumerable<AreaDto> sessions) {
      CancelAutoMergeAsync();
      cancelAutoMerge = new();
      try {
        await Task.Run(() => AutoMerge(sessions), cancelAutoMerge.Token);
      } catch (Exception) { }
    }
    protected void CancelAutoMergeAsync() {
      if (cancelAutoMerge != null) {
        cancelAutoMerge.Cancel();
        cancelAutoMerge = null;
      }
    }

    public bool Merge(AreaDto sourceArea, AreaDto targetArea, Point offset) {
      Stopwatch watch = new();
      watch.Start();
      bool changed = false;
      if (sourceArea.Type == Type || !World.Areas.ContainsKey(targetArea.Directory)) {
        return changed;
      }
      AreaDto orginalArea = World.Areas[targetArea.Directory];
      try {
        foreach (var sourceTile in sourceArea.TileList) {
          TileDto newTile = MergeTile(sourceTile, orginalArea, offset);
          if (newTile != null) { newTile.Score = null; }
          changed = changed || newTile != null;
        }
      } catch (Exception e) {
        Debug.WriteLine(this.GetType().Name + ": " + e);
      }
      if (changed) {
        Refresh();
      }
      Debug.WriteLine("merge finished after {0} ms", watch.ElapsedMilliseconds);
      watch.Stop();
      return changed;
    }

    public TileDto MergeTile(TileDto sourceTile, AreaDto targetArea, Point offset) {
      TileDto newTile = new(sourceTile);
      Point coord = sourceTile.Coordinate;
      coord.Offset(offset);
      newTile.Coordinate = coord;
      if (!File.Exists(sourceTile.Path)) {
        return null;
      }
      TileDto orginalTile = targetArea.GetTileByKey(newTile.Key);
      if (orginalTile != null && sourceTile.Date <= orginalTile.Date) {
        return null;
      }
      newTile.FileName = TileProcessor.GenerateFileName(newTile);
      newTile.Path = PathUtils.FinalizePath(targetArea.Path) + newTile.FileName;
      lock (sourceTile) {
        File.Copy(sourceTile.Path, newTile.Path, true);
      }
      if (orginalTile != null) {
        targetArea.RemoveTile(orginalTile);
      }
      targetArea.AddTile(newTile);
      return newTile;
    }

    public bool Delete(AreaDto area) {
      return DeleteArea(area);
    }

    protected void StoreData() {
      try {
        WorldDto data = new(World);
        string json = JsonSerializer.Serialize(World);
        if (!String.IsNullOrWhiteSpace(json)) {
          File.WriteAllText(DataFile, json);
          Debug.WriteLine("===> data stored");
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
      }
    }

    protected void LoadData() {
      try {
        if (File.Exists(DataFile)) {
          string json = File.ReadAllText(DataFile);
          if (!String.IsNullOrWhiteSpace(json)) {
            WorldDto data = JsonSerializer.Deserialize<WorldDto>(json);
            if (data != null) {
              World = data;
            }
          }
        }
        if (World == null) {
          World = new();
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
      }
    }

  }
}