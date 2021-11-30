using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using SalemCartographer.App.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SalemCartographer.App
{
  public abstract class AbstractAreaController : AbstractController
  {
    protected AbstractAreaController() : base() {

    }

    public abstract IDictionary<string, AreaDto> Areas { get; }
    public abstract string DirectoryPath { get; }
    public abstract AreaType Type { get; }
    public IList<AreaDto> AreaList => Areas.Values.ToList();
    public event EventHandler<DataEventArgs<Dictionary<string, AreaDto>>> DataChanged;

    private CancellationTokenSource cancelRefresh;

    protected virtual void RefreshAreas() {
      Stopwatch w = new();
      w.Start();
      foreach (var path in Directory.GetDirectories(DirectoryPath)) {
        if (cancelRefresh != null && cancelRefresh.Token.IsCancellationRequested) {
          Debug.WriteLine("====================================================");
          Debug.WriteLine(String.Format("Refresh Areas canceled: {0}, after: {1} ms", Type, w.ElapsedMilliseconds));
          cancelRefresh.Token.ThrowIfCancellationRequested();
        }
        AreaDto Area = AreaProcessor.BuildDto(path);
        if (Areas.TryGetValue(Area.Directory, out var existingArea) && existingArea != null) {
          Area = existingArea;
          Area.Path = path;
        }
        Area.Type = Type;
        AreaProcessor.RefreshDto(Area);
        Areas[Area.Directory] = Area;
      }
      Debug.WriteLine("====================================================");
      Debug.WriteLine(String.Format("Refresh Areas complete: {0}, after: {1} ms", Type, w.ElapsedMilliseconds));
      InvokeDataChanged();
      cancelRefresh = null;
    }

    protected async void RefreshAreasAsync() {
      CancelRefreshAreasAsync();
      cancelRefresh = new();
      try {
        await Task.Run(() => RefreshAreas(), cancelRefresh.Token);
      } catch (Exception) { }
    }

    protected void CancelRefreshAreasAsync() {
      if (cancelRefresh != null) {
        cancelRefresh.Cancel();
        cancelRefresh = null;
      }
    }

    public AreaDto GetArea(string directory) {
      AreaDto area = null;
      bool isPath = directory.Contains(Path.DirectorySeparatorChar);
      string path = isPath ? directory : PathUtils.FinalizePath(DirectoryPath + directory);
      if (!Directory.Exists(path)) {
        return null;
      }
      string directoryName = isPath ? Path.GetFileName(directory) : directory;
      if (Areas.ContainsKey(directoryName)) {
        area = Areas[directoryName];
      }
      if (area == null) {
        area = AreaProcessor.BuildDto(path);
      }
      area.Type = Type;
      AreaProcessor.RefreshDto(area);
      Areas[area.Directory] = area;
      return area;
    }

    protected bool DeleteArea(AreaDto area, bool notify = true) {
      if (area is not AreaDto || area.Type != Type || String.IsNullOrEmpty(area.Path)
        || !area.Path.StartsWith(DirectoryPath) || !Directory.Exists(area.Path)) {
        return false;
      }
      Directory.Delete(area.Path, true);
      if (Areas.ContainsKey(area.Directory)) {
        Areas.Remove(area.Directory);
        if (notify) {
          InvokeDataChanged();
        }
      }
      return true;
    }

    protected bool DeleteTile(TileDto tile) {
      if (tile is not TileDto || !File.Exists(tile.Path)) {
        return false;
      }
      File.Delete(tile.Path);
      return true;
    }

    private readonly ReaderWriterLockSlim rwlock = new();
    private Timer processTimer;

    protected void InvokeDataChanged() {
      try {
        rwlock.EnterWriteLock();
        if (processTimer == null) {
          processTimer = new(500);
          processTimer.AutoReset = false;
          processTimer.Elapsed += (object s, ElapsedEventArgs e) => {
            DataChanged?.Invoke(this, new(new(Areas)));
          };
          processTimer.Start();
        }
        else {
          processTimer.Stop();
          processTimer.Start();
        }
      } finally {
        rwlock.ExitWriteLock();
      }
    }

  }
}