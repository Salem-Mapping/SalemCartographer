using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using SalemCartographer.App.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  public class SessionController : AbstractAreaController
  {
    private const string CurrentSessionFile = AppConstants.CurrentSessionFile;

    private static SessionController _Instance;
    private static readonly object _IntanceLock = new();
    public static SessionController Instance {
      get {
        lock (_IntanceLock) {
          if (_Instance == null) {
            _Instance = new SessionController();
          }
        }
        return _Instance;
      }
    }

    public override string DirectoryPath => Configuration.GetSessionsPath();
    public override Dictionary<string, AreaDto> Areas => Sessions;
    public override AreaType Type => AreaType.Session;
    public string CurrentSession { get; private set; }
    public Dictionary<string, AreaDto> Sessions { get; private set; }
    public List<AreaDto> SessionList => Sessions.Values.ToList();
    private Point? LastPosition = null;

    public event EventHandler<StringDataEventArgs> SessionChanged;
    public event EventHandler<PositionEventArgs> PositionChanged;

    private FileSystemMonitor watcher;
    private CancellationTokenSource cancelMatching;

    private SessionController() {
      if (!Directory.Exists(DirectoryPath)) {
        return;
      }
      Sessions = new();
      RefreshAreasAsync();
      SetupWatcher();
    }

    public void Refresh() {
      ReadCurrentSessionName();
      RefreshAreasAsync();
    }

    protected override void RefreshAreas() {
      base.RefreshAreas();
      RefreshMatchesAsync();
    }

    protected void RefreshMatches() {
      Stopwatch w = new();
      w.Start();
      foreach (var session in AreaList) {
        if (cancelMatching != null && cancelMatching.Token.IsCancellationRequested) {
          Debug.WriteLine("====================================================");
          Debug.WriteLine(String.Format("Matching Areas canceled: {0}, after: {1} ms", Type, w.ElapsedMilliseconds));
          cancelMatching.Token.ThrowIfCancellationRequested();
        }
        if (session.MatchingAreas.Any()) {
          continue;
        }
        if (session.MatchingAreas != null && session.MatchingAreas.Count == 0) {
          session.MatchingAreas = WorldController.Instance.GetKnownAreas(session);
        }
      }
      //    Debug.WriteLine("====================================================");
      //    Debug.WriteLine(String.Format("Matching Areas complete: {0}, after: {1} ms", Type, w.ElapsedMilliseconds));
      cancelMatching = null;
    }
    protected async void RefreshMatchesAsync() {
      CancelMatchAreasAsync();
      cancelMatching = new();
      try {
        await Task.Run(() => RefreshMatches(), cancelMatching.Token);
      } catch (Exception) { }
    }

    protected void CancelMatchAreasAsync() {
      if (cancelMatching != null) {
        cancelMatching.Cancel();
        cancelMatching = null;
      }
    }

    public bool Delete(AreaDto session) {
      return DeleteArea(session);
    }

    private void OnSessionFilesChanged(object source, DataEventArgs<List<FileSystemEventArgs>> events) {
      bool refreshNeeded = false;
      List<String> files = new();
      foreach (var e in events.Value) {
        if (!File.Exists(e.FullPath)) {
          continue;
        }
        if (CurrentSessionFile.Equals(e.Name)) {
          ReadCurrentSessionName();
          SessionChanged?.Invoke(this, new StringDataEventArgs(CurrentSession));
          Debug.WriteLine(String.Format("Session changed to: {0} -> {1}", e.Name, CurrentSession));
        }
        else {
          Debug.WriteLine(String.Format("File: {0} -> {1}", e.Name, e.ChangeType));
          refreshNeeded = true;
          switch (e.ChangeType) {
            case WatcherChangeTypes.Created:
            case WatcherChangeTypes.Changed:
              files.Add(e.FullPath);
              break;
            case WatcherChangeTypes.Deleted:
            case WatcherChangeTypes.Renamed:
            default:
              break;
          }
        }
      }
      if (files.Any()) {
        LiveMerge(files.Distinct());
      }
      if (refreshNeeded) {
        Refresh();
      }
    }

    protected void SetupWatcher() {
      string WatchedPath = DirectoryPath;
      if (watcher != null) {
        watcher.ChangedBulk -= OnSessionFilesChanged;
        watcher.Dispose();
      }
      watcher = new(WatchedPath, AppConstants.TileSearchFilter);
      watcher.ChangedBulk += OnSessionFilesChanged;
      watcher.IncludeSubdirectories = true;
      //watcher.NotifyFilter = NotifyFilters.FileName;
      watcher.EnableRaisingEvents = true;
      Debug.WriteLine(String.Format("setup watcher for {0}", WatchedPath));
    }

    protected string ReadCurrentSessionName() {
      string SessionsPath = DirectoryPath;
      if (File.Exists(SessionsPath + CurrentSessionFile)) {
        string Text = File.ReadAllText(SessionsPath + CurrentSessionFile);
        MatchCollection Matches = AppConstants.RegexJsVar.Matches(Text);
        foreach (Match Match in Matches) {
          if (AppConstants.CurrentSessionVar
            .Equals(Match.Groups[AppConstants.RegexJsVar_Name].Value)) {
            CurrentSession = Match.Groups[AppConstants.RegexJsVar_Value].Value;
            break;
          }
        }
      }
      return CurrentSession;
    }

    protected void LiveMerge(IEnumerable<string> files) {
      bool changed = false;
      AreaDto currArea = null;
      Point? currPos = null;
      if (!Configuration.LiveMergeEnabled()) {
        return;
      }
      lock (Areas) {
        Debug.WriteLine("start Live-Merge");
        var directories = files.Select(Path.GetDirectoryName).Select(Path.GetFileName).Distinct();
        foreach (var directory in directories) {
          AreaDto area = GetArea(directory);
          if (area == null) {
            continue;
          }
          if (currArea != area) {
            LastPosition = null;
          }
          currArea ??= area;
          // calc position
          var newFiles = files.Where(s => s.StartsWith(PathUtils.FinalizePath(area.Path))).Select(Path.GetFileName);
          var postions = newFiles.Select(TileProcessor.ParseFileName);
          if (LastPosition.HasValue) {
            postions.Concat(new List<Point>() { LastPosition.Value });
          }
          Point min = new(postions.Select(p => p.X).Min(), postions.Select(p => p.Y).Min());
          Point max = new(postions.Select(p => p.X).Max(), postions.Select(p => p.Y).Max());
          int width = max.X - min.X;
          int height = max.Y - min.Y;
          currPos = new(min.X + width / 2, min.Y + height / 2);
          LastPosition = currPos;
          // find map
          if (!area.MatchingAreas.Any()) {
            area.MatchingAreas = WorldController.Instance.GetKnownAreas(area);
          }
          if (!area.MatchingAreas.Any()) {
            continue;
          }
          AreaDto matchingArea = area.MatchingAreas.OrderByDescending(a => (a.Score.HasValue) ? a.Score : 0).First();
          if (matchingArea == null || !matchingArea.Offset.HasValue
            || matchingArea.Score < Configuration.GetAutoMergeMinScore()
            || matchingArea.ScoreNormalized < Configuration.GetAutoMergeMinScoreNormalized()) {
            continue;
          }
          AreaDto targetArea = WorldController.Instance.GetArea(matchingArea.Directory);
          if (targetArea == null) {
            continue;
          }
          DateTime dt = DateTime.Now.AddSeconds(-30);
          IEnumerable<TileDto> tilesScored = targetArea.TileList.Where(t => t.Score.HasValue).Where(t => t.Date < dt);
          foreach (TileDto tile in tilesScored) { tile.Score = null; }
          //foreach (string file in newFiles) {
          foreach (TileDto sourceTile in area.TileList) {
            try {
              //TileDto sourceTile = area.GetTile(TileProcessor.ParseFileName(file));
              TileDto newTile = WorldController.Instance.MergeTile(sourceTile, targetArea, matchingArea.Offset.Value);
              changed = changed || newTile != null;
              if (newTile != null) {
                TileDto matchingTile = matchingArea.GetTileByKey(newTile.Key);
                newTile.Score = (matchingTile != null && matchingTile.Score.HasValue) ? matchingTile.Score : 1;
              }
            } catch (Exception e) {
              Debug.WriteLine(this.GetType().Name + ": " + e);
            }
          }
          if (currPos.HasValue) {
            Point value = currPos.Value;
            value.Offset(matchingArea.Offset.Value);
            currPos = value;
          }
          currArea = targetArea;
        }
        Debug.WriteLine("finish Live-Merge Process: " + currPos.Value);
        if (changed) {
          InvokeDataChanged();
        }
        if (currArea != null && currPos.HasValue) {
          InvokePositionChange(currArea, currPos.Value);
        }
      }
    }

    public void InvokePositionChange(AreaDto area, Point position) {
      PositionChanged?.Invoke(this, new(area, new(position.X, position.Y)));
    }

    public class PositionEventArgs : EventArgs
    {
      public AreaDto Area;
      public Point Position;

      internal PositionEventArgs() { }
      internal PositionEventArgs(AreaDto area, Point position) : this() {
        Area = area;
        Position = position;
      }
    }

  }
}
