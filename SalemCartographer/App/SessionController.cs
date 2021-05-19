using SalemCartographer.App.Enum;
using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SalemCartographer.App
{
  internal class SessionController : AreaController
  {
    private const string CurrentSessionFile = ApplicationConstants.CurrentSessionFile;
    private static SessionController _Instance;

    public static SessionController Instance {
      get {
        if (_Instance == null) {
          _Instance = new SessionController();
        }
        return _Instance;
      }
    }

    public override event EventHandler DataChanged;

    public event EventHandler<StringDataEventArgs> SessionChanged;

    public override string DirectoryPath => Configuration.GetSessionsPath();
    public override AreaType Type => AreaType.Session;
    public string CurrentSession { get; private set; }
    public Dictionary<string, AreaDto> Sessions { get; private set; }

    public List<AreaDto> SessionList {
      get {
        return Sessions.Values.ToList();
      }
    }

    private FileSystemMonitor watcher;

    private SessionController() {
      Sessions = new();
      SetupWatcher();
      Refresh();
    }

    public void Refresh() {
      string path = DirectoryPath;
      if (!Directory.Exists(path)) {
        return;
      }
      ReadCurrentSessionName();
      RefreshAreas(Sessions, path);
      DataChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool Delete(AreaDto session) {
      if (Sessions.ContainsKey(session.Directory)) {
        Sessions.Remove(session.Directory);
      }
      bool result = DeleteArea(session);
      DataChanged?.Invoke(this, EventArgs.Empty);
      return result;
    }

    private void OnSessionFilesChanged(object source, DataEventArgs<List<FileSystemEventArgs>> events) {
      bool refreshNeeded = false;
      foreach (var e in events.Value) {
        if (!File.Exists(e.FullPath)) {
          continue;
        }
        if (CurrentSessionFile.Equals(e.Name)) {
          ReadCurrentSessionName();
          SessionChanged?.Invoke(this, new StringDataEventArgs(CurrentSession));
          Debug.WriteLine(String.Format("Session changed to: {0} -> {1}", e.Name, CurrentSession));
        } else {
          Debug.WriteLine(String.Format("File: {0} -> {1}", e.Name, e.ChangeType));
          refreshNeeded = true;
        }
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
      watcher = new(WatchedPath, "*.*");
      watcher.IncludeSubdirectories = true;
      watcher.NotifyFilter = NotifyFilters.LastWrite;
      watcher.ChangedBulk += OnSessionFilesChanged;
      watcher.EnableRaisingEvents = true;
      Debug.WriteLine(String.Format("setup watcher for {0}", WatchedPath));
    }

    protected string ReadCurrentSessionName() {
      string SessionsPath = DirectoryPath;
      if (File.Exists(SessionsPath + CurrentSessionFile)) {
        string Text = File.ReadAllText(SessionsPath + CurrentSessionFile);
        MatchCollection Matches = ApplicationConstants.RegexJsVar.Matches(Text);
        foreach (Match Match in Matches) {
          if (ApplicationConstants.CurrentSessionVar
            .Equals(Match.Groups[ApplicationConstants.RegexJsVar_Name].Value)) {
            CurrentSession = Match.Groups[ApplicationConstants.RegexJsVar_Value].Value;
            break;
          }
        }
      }
      return CurrentSession;
    }
  }
}