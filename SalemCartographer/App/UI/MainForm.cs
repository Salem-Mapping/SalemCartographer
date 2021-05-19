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
    private List<MatchedAreaDto> matchedAreas = new();

    public MainForm() {
      InitializeComponent();
      Initalize();
      BuildSessionList();
      BuildWorldList();
    }

    protected void Initalize() {
      Text = ApplicationConstants.ProductName;
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
      SessionController.Instance.SessionChanged += OnSessionDataChanged;
      SessionController.Instance.DataChanged += OnSessionDataChanged;
      WorldController.Instance.DataChanged += OnSessionDataChanged;
    }

    protected void OnWorldDataChanged(object sender, EventArgs e) {
      BuildWorldList();
    }

    protected void BuildWorldList() {
      Debug.WriteLine("BuildWorldList");
      if (WorldController.Instance.World.Areas.Count == 0) {
        WorldController.Instance.CreateAreaFromSession();
      }
      ListMaps.DataSource = new List<AreaDto>(WorldController.Instance.World.AreaList);
    }

    protected void OnSessionDataChanged(object sender, EventArgs e) {
      BuildSessionList();
    }

    protected void BuildSessionList() {
      Debug.WriteLine("BuildSessionList");
      ListSessions.DataSource = new List<AreaDto>(SessionController.Instance.SessionList);
    }

    private void OnSelectionChanged(object sender, EventArgs e) {
      ListBox list = (ListBox)sender;
      AreaDto selectedItem = GetSelectedArea(list);
      if (selectedItem == null) {
        return;
      }
      Canvas.SetArea(selectedItem);
      if (list.Name == ListSessions.Name) {
        matchedAreas = WorldController.Instance.GetKnownAreas(selectedItem);
        foreach (var item in WorldController.Instance.World.AreaList) {
          if (!matchedAreas.Contains(item)) {
            matchedAreas.Add(new(item));
          }
        }
        Canvas.AddMatchedAreas(matchedAreas);
      }
    }

    protected void OnSessionDataChanged(Object sender, StringDataEventArgs e) {
    }

    protected void OnToolSessionMerge(Object sender, EventArgs e) {
      AreaDto sourceArea = GetSelectedSession();
      if (sourceArea == null) {
        MessageBox.Show("select a area to merge, first!");
        return;
      }
      MergeForm form = new(sourceArea, matchedAreas);
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

#nullable enable

    private AreaDto? GetSelectedSession() {
      return GetSelectedArea(ListSessions);
    }

    private AreaDto? GetSelectedMap() {
      return GetSelectedArea(ListMaps);
    }

    private AreaDto? GetSelectedArea(ListBox list) {
      try {
        if (list.SelectedItems.Count > 0) {
          return (AreaDto)list.SelectedItems[0];
        }
        return ((AreaDto)list.SelectedItem);
      } catch (Exception e) {
        Debug.WriteLine(e);
        return null;
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
      AreaDto area = null;
      if (sender == ToolSessionDelete) {
        area = GetSelectedSession();
      } else if (sender == ToolMapsDelete) {
        area = GetSelectedMap();
      }
      if (area == null) {
        MessageBox.Show("select a area, first!");
        return;
      }
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

    protected void OnToolMapsAddClicked(Object sender, EventArgs e) {
      WorldController.Instance.CreateAreaFromSession();
    }
  }
}