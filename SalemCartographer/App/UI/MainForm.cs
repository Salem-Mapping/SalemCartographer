using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SalemCartographer.App.UI
{
  public partial class MainForm : Form
  {
    public MainForm() {
      InitializeComponent();
      Initalize();
    }

    protected void Initalize() {
      this.Load += OnFormLoad;
      Text = AppConstants.ProductName;
      ListSessions.DisplayMember = AreaDto.FIELD_TITLE;
      ListSessions.SelectedValueChanged += OnSelectionChanged;
      ListMaps.DisplayMember = AreaDto.FIELD_TITLE;
      ListMaps.SelectedValueChanged += OnSelectionChanged;
      ToolSessionMerge.Click += OnToolSessionMerge;
      ToolSessionOpen.Click += OnToolOpen;
      ToolSessionDelete.Click += OnToolDelete;
      ToolMapsOpen.Click += OnToolOpen;
      ToolMapsAdd.Click += OnToolMapsAddClicked;
      ToolMapsDelete.Click += OnToolDelete;
      ToolAutoMerge.Click += OnToolAutoMergeClicked;
      SessionController.Instance.SessionChanged += OnSessionDataChanged;
      SessionController.Instance.DataChanged += OnSessionDataChanged;
      SessionController.Instance.PositionChanged += OnSessionPositionChanged;
      WorldController.Instance.DataChanged += OnWorldDataChanged;
    }

    protected void OnFormLoad(object sender, EventArgs e) {
      BuildWorldList();
      BuildSessionList();
    }

    protected void OnWorldDataChanged(object sender, EventArgs e) {
      BuildWorldList();
    }

    protected void BuildWorldList() {
      Debug.WriteLine("====================================================");
      IList<AreaDto> areaList = WorldController.Instance.AreaList;
      Debug.WriteLine("BuildWorldList: " + areaList.Count);
      if (areaList.Count == 0) {
        WorldController.Instance.CreateAreaFromSession();
      }
      RefreshList(ListMaps, WorldController.Instance.World.AreaList);
    }

    protected void OnSessionDataChanged(object sender, EventArgs e) {
      BuildSessionList();
    }

    protected void BuildSessionList() {
      Debug.WriteLine("====================================================");
      IList<AreaDto> areaList = SessionController.Instance.AreaList;
      Debug.WriteLine("BuildSessionList: " + areaList.Count);
      RefreshList(ListSessions, areaList.Reverse()); 
    } 

    private bool changingSource;
    private void RefreshList(ListBox list, IEnumerable<AreaDto> data) {
      try {
        list.Invoke(new Action(() => {
          changingSource = true;
          List<AreaDto> areaDtos = new(data);
          list.DataSource = areaDtos;
          list.Refresh();
          changingSource = false;
        }));

      } catch (Exception) { }
    }

    private void SelectItem(ListBox list, AreaDto item) {
      try {
        list.Invoke(new Action(() => {
          if (!ListSessions.Equals(list)) {
            ListSessions.SelectedItem = null;
          } else if (!ListMaps.Equals(list)) {
            ListMaps.SelectedItem = null;
          }
          list.SelectedItem = item;
        }));
      } catch (Exception) { }
    }

    private void OnSelectionChanged(object sender, EventArgs e) {
      if (changingSource) {
        return;
      }
      ListBox list = (ListBox)sender;
      AreaDto selectedItem = GetSelectedArea(list);
      if (selectedItem == null) {
        return;
      }
      switch (selectedItem.Type) {
        case Enum.AreaType.World:
          ListSessions.SelectedItem = null;
          break;
        case Enum.AreaType.Session:
          ListMaps.SelectedItem = null;
          break;
      }
      if (selectedItem.Type == Enum.AreaType.Session) {
        if (!selectedItem.MatchingAreas.Any()) {
          selectedItem.MatchingAreas = WorldController.Instance.GetKnownAreas(selectedItem);
        }
        Debug.WriteLine(String.Format("Area '{0}' matches {1} times", selectedItem.Name, selectedItem.MatchingAreas.Count));
      }
      Canvas.SetArea(selectedItem);
    }

    protected void OnSessionPositionChanged(Object sender, SessionController.PositionEventArgs e) {
      if (e.Area == null) {
        return;
      }
      try {
        ListBox list;
        switch (e.Area.Type) {
          case Enum.AreaType.World:
            list = ListMaps;
            break;
          case Enum.AreaType.Session:
            list = ListSessions;
            break;
          case Enum.AreaType.Preview:
          default:
            return;
        }
        SelectItem(list, e.Area);
        Canvas.Invoke(new Action(() => {
          Canvas.SetArea(e.Area);
          Canvas.CenteredTile = e.Position;
          Canvas.SelectedTile = e.Position;
        }));
      } catch (Exception) { }
    }

    protected void OnSessionDataChanged(Object sender, StringDataEventArgs e) {
    }

    protected void OnToolSessionMerge(Object sender, EventArgs ev) {
      AreaDto sourceArea = GetSelectedSession();
      if (sourceArea == null) {
        MessageBox.Show("select a area to merge, first!");
        return;
      }
      AreaDto targetArea = null;
      if (!sourceArea.MatchingAreas.Any()) {
        sourceArea.MatchingAreas = WorldController.Instance.GetKnownAreas(sourceArea);
      }
      if (sourceArea.MatchingAreas.Any()) {
        try {
          targetArea = sourceArea.MatchingAreas.OrderByDescending(a => (a.Score.HasValue) ? a.Score : 0).First();
        } catch (Exception e) {
          Debug.WriteLine(this.GetType().Name + ": " + e);
        }
      }

      List<AreaDto> targetAreas = new(sourceArea.MatchingAreas);
      foreach (var map in WorldController.Instance.AreaList) {
        if (!targetAreas.Contains(map)) {
          targetAreas.Add(map);
        }
      }

      MergeForm form = new(sourceArea, targetAreas);
      var result = form.ShowDialog();
      switch (result) {
        case DialogResult.OK:
        case DialogResult.Yes:
          WorldController.Instance.Merge(form.SourceArea, form.TargetArea, form.Offset);
          break;
        case DialogResult.None:
        case DialogResult.Cancel:
        case DialogResult.Abort:
        case DialogResult.Retry:
        case DialogResult.Ignore:
        case DialogResult.No:
        default:
          break;
      }
    }

    private IEnumerable<AreaDto> GetSelectedSessions() => GetSelectedAreas(ListSessions);

    private IEnumerable<AreaDto> GetSelectedMaps() => GetSelectedAreas(ListMaps);

#nullable enable

    private AreaDto? GetSelectedSession() => GetSelectedArea(ListSessions);

    private AreaDto? GetSelectedMap() => GetSelectedArea(ListMaps);

    private AreaDto? GetSelectedArea(ListBox list) {
      try {
        if (list.SelectedItems.Count > 0) {
          return (AreaDto)list.SelectedItems[0];
        }
        return ((AreaDto)list.SelectedItem);
      } catch (Exception e) {
        Debug.WriteLine(this.GetType().Name + ": " + e);
        return null;
      }
    }

    private IEnumerable<AreaDto> GetSelectedAreas(ListBox list) {
      try {
        return list.SelectedItems.Cast<AreaDto>();
      } catch (Exception e) {
        Debug.WriteLine(this.GetType().Name + ": " + e);
        return Enumerable.Empty<AreaDto>();
      }
    }
#nullable disable

    protected void OnToolOpen(Object sender, EventArgs e) {
      AreaDto area = null;
      if (sender == ToolSessionOpen) {
        area = GetSelectedSession();
      } else if (sender == ToolMapsOpen) {
        area = GetSelectedMap();
      }
      if (area == null) {
        MessageBox.Show("select a area, first!");
        return;
      }
      Process.Start(new ProcessStartInfo {
        Arguments = area.Path,
        FileName = "explorer.exe"
      });
    }

    protected void OnToolDelete(Object sender, EventArgs e) {
      IEnumerable<AreaDto> col = Enumerable.Empty<AreaDto>();
      if (sender == ToolSessionDelete) {
        col = GetSelectedSessions();
      } else if (sender == ToolMapsDelete) {
        col = GetSelectedMaps();
      }
      if (col == null || !col.Any()) {
        MessageBox.Show("select a area, first!");
        return;
      }
      foreach (var area in col) {
        switch (area.Type) {
          case Enum.AreaType.World:
            WorldController.Instance.Delete(area);
            break;
          case Enum.AreaType.Session:
            SessionController.Instance.Delete(area);
            break;
          case Enum.AreaType.Preview:
          default:
            break;
        }
      }
    }

    public void OnToolAutoMergeClicked(object sender, EventArgs e) => WorldController.Instance.AutoMergeAsync(SessionController.Instance.SessionList);

    protected void OnToolMapsAddClicked(object sender, EventArgs e) => WorldController.Instance.CreateAreaFromSession();

    private void MainForm_Load(object sender, EventArgs e) {

    }

  }
}