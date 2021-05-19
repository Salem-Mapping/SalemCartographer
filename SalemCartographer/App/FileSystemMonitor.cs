using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
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
      get => includeSubdirectories; set {
        includeSubdirectories = value;
        if (watcher != null) {
          watcher.IncludeSubdirectories = value;
        }
      }
    }

    public bool EnableRaisingEvents {
      get => enableRaisingEvents; set {
        enableRaisingEvents = value;
        if (watcher != null) {
          watcher.EnableRaisingEvents = value;
        }
      }
    }

    public NotifyFilters NotifyFilter {
      get => notifyFilter; set {
        notifyFilter = value;
        if (watcher != null) {
          watcher.NotifyFilter = value;
        }
      }
    }

    private List<FileSystemEventArgs> fileEvents;
    private ReaderWriterLockSlim rwlock;
    private Timer processTimer;
    private FileSystemWatcher watcher;
    private Double waitUntilNotify = 500;
    private string path;
    private string filter;
    private bool includeSubdirectories;
    private bool enableRaisingEvents;
    private NotifyFilters notifyFilter;

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

    private void InitFileSystemWatcher() {
      if (watcher != null) {
        watcher.Changed -= Watcher_FileChanged;
        watcher.Error -= Watcher_Error;
        watcher.Dispose();
      }
      watcher = new();
      watcher.Path = path;
      watcher.Filter = filter;
      watcher.Changed += Watcher_FileChanged;
      watcher.Error += Watcher_Error;
      watcher.IncludeSubdirectories = includeSubdirectories;
      watcher.EnableRaisingEvents = enableRaisingEvents;
      watcher.NotifyFilter = notifyFilter;
    }

    private void Watcher_FileChanged(object sender, FileSystemEventArgs e) {
      try {
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
      // Watcher crashed. Re-init.
      InitFileSystemWatcher();
    }

    private void ProcessQueue(object sender, ElapsedEventArgs args) {
      try {
        Console.WriteLine("Processing queue, " + fileEvents.Count + " files created:");
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