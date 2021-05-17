using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SalemCartographer.App.Model;

namespace SalemCartographer.App
{

  class SessionController : AreaController
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

    public string CurrentSession { get; private set; }
    public Dictionary<string, AreaDto> Sessions { get; private set; }
    public List<AreaDto> SessionList {
      get {
        return Sessions.Values.ToList();
      }
    }

    private FileSystemWatcher Watcher;

    SessionController() {
      Sessions = new();
      SetupWatcher();
      Refresh();
    }

    public void Refresh() {
      string Path = Configuration.GetSessionsPath();
      if (!Directory.Exists(Path)) {
        return;
      }
      ReadCurrentSessionName();
      RefreshAreas(Sessions, Path);
    }

    private void OnSessionFilesChanged(object source, FileSystemEventArgs e) {
      if (!File.Exists(e.FullPath)) {
        return;
      }
      if (CurrentSessionFile.Equals(e.Name)) {
        ReadCurrentSessionName();
        SessionChanged?.Invoke(this, new StringDataEventArgs(CurrentSession));
        Debug.WriteLine(String.Format("Session changed to: {0} -> {1}", e.Name, CurrentSession));
      } else {
        Debug.WriteLine(String.Format("File: {0} -> {1}", e.Name, e.ChangeType));
      }
      // DataChanged?.Invoke(this, e);
      Refresh();
    }

    protected void SetupWatcher() {
      string WatchedPath = Configuration.GetSessionsPath();
      if (Watcher != null) {
        Watcher.Changed -= OnSessionFilesChanged;
        Watcher.Dispose();
      }
      Watcher = new(WatchedPath, "*.*");
      Watcher.IncludeSubdirectories = true;
      Watcher.NotifyFilter = NotifyFilters.LastWrite;
      Watcher.Changed += OnSessionFilesChanged;
      Watcher.EnableRaisingEvents = true;
      Debug.WriteLine(String.Format("setup watcher for {0}", WatchedPath));
    }

    protected string ReadCurrentSessionName() {
      string SessionsPath = Configuration.GetSessionsPath();
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
