using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SalemCartographer.App
{
  internal class FileSystemMonitor : IDisposable
  {
    public event EventHandler<DataEventArgs<List<FileSystemEventArgs>>> ChangedBulk;

    public Double WaitUntilNotify {
      get => waitUntilNotify;
      set {
        waitUntilNotify = value;
        if (processTimer != null) {
          processTimer.Interval = value;
        }
      }
    }

    public string Path {
      get => path; set {
        path = value;
        if (watcher != null) {
          watcher.Path = value;
        }
      }
    }

    public string Filter {
      get => filter; set {
        filter = value;
        if (watcher != null) {
          watcher.Filter = value;
        }
      }
    }

    public bool IncludeSubdirectories {
      get => includeSubdirectories.GetValueOrDefault(); set {
        includeSubdirectories = value;
        if (watcher != null) {
          watcher.IncludeSubdirectories = value;
        }
      }
    }

    public bool EnableRaisingEvents {
      get => enableRaisingEvents.GetValueOrDefault(); set {
        enableRaisingEvents = value;
        if (watcher != null) {
          watcher.EnableRaisingEvents = value;
        }
      }
    }

    public NotifyFilters NotifyFilter {
      get => notifyFilter.GetValueOrDefault(); set {
        notifyFilter = value;
        if (watcher != null) {
          watcher.NotifyFilter = value;
        }
      }
    }

    private string path;
    private string filter;
    private bool? includeSubdirectories;
    private bool? enableRaisingEvents;
    private NotifyFilters? notifyFilter;

    private List<FileSystemEventArgs> fileEvents;
    private ReaderWriterLockSlim rwlock;
    private Timer processTimer;
    private FileSystemWatcher watcher;
    private Double waitUntilNotify = 500;

    public FileSystemMonitor(string path) : this(path, "*") {

    }

    public FileSystemMonitor(string path, string filter) {
      this.path = path;
      this.filter = filter;
      Initalize();
    }

    private void Initalize() {
      rwlock = new ReaderWriterLockSlim();
      fileEvents = new();
      InitFileSystemWatcher();
    }

    private bool isInInit = false;
    private void InitFileSystemWatcher() {
      if (watcher != null) {
        watcher.Changed -= Watcher_FileChanged;
        watcher.Error -= Watcher_Error;
        watcher.Dispose();
      }
      try {
        if (!Directory.Exists(path)) {
          throw new Exception(String.Format("Path {0} does not exist!", path));
        }
        isInInit = true;
        watcher = new FileSystemWatcher(path, string.IsNullOrWhiteSpace(filter) ? "*" : filter);
        watcher.Changed += Watcher_FileChanged;
        watcher.Error += Watcher_Error;
        if (includeSubdirectories.HasValue) {
          watcher.IncludeSubdirectories = includeSubdirectories.Value;
        } else {
          includeSubdirectories = watcher.IncludeSubdirectories;
        }
        if (enableRaisingEvents.HasValue) {
          watcher.EnableRaisingEvents = enableRaisingEvents.Value;
        } else {
          enableRaisingEvents = watcher.EnableRaisingEvents;
        }
        if (notifyFilter.HasValue) {
          watcher.NotifyFilter = notifyFilter.Value;
        } else {
          notifyFilter = watcher.NotifyFilter;
        }
        isInInit = false;
      } catch (Exception e) {
        System.Diagnostics.Debug.WriteLine(e);
      }
    }

    private void Watcher_FileChanged(object sender, FileSystemEventArgs e) {
      try {
        System.Diagnostics.Debug.WriteLine("add queue: " + e.FullPath);
        rwlock.EnterWriteLock();
        fileEvents.Add(e);
        if (processTimer == null) {
          // First file, start timer.
          processTimer = new Timer(2000);
          processTimer.Elapsed += ProcessQueue;
          processTimer.Start();
        } else {
          // Subsequent file, reset timer.
          processTimer.Stop();
          processTimer.Start();
        }
      } finally {
        rwlock.ExitWriteLock();
      }
    }

    private void Watcher_Error(object sender, ErrorEventArgs e) {
      Debug.WriteLine(e.GetException());
      if (!isInInit) {
        InitFileSystemWatcher();
      } else {
        throw e.GetException();
      }
    }

    private void ProcessQueue(object sender, ElapsedEventArgs args) {
      try {
        System.Diagnostics.Debug.WriteLine("Processing queue, " + fileEvents.Count + " files created:");
        rwlock.EnterReadLock();
        ChangedBulk?.Invoke(this, new(new(fileEvents)));
        fileEvents.Clear();
      } finally {
        if (processTimer != null) {
          processTimer.Stop();
          processTimer.Dispose();
          processTimer = null;
        }
        rwlock.ExitReadLock();
      }
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        if (rwlock != null) {
          rwlock.Dispose();
          rwlock = null;
        }
        if (watcher != null) {
          watcher.EnableRaisingEvents = false;
          watcher.Dispose();
          watcher = null;
        }
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}