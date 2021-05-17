using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalemCartographer.App.Model;

namespace SalemCartographer.App.UI
{
  public partial class MainForm : Form
  {
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
      ToolSessionOpen.Click += OnToolSessionOpen;
      ToolMapsAdd.Click += OnToolMapsAddClicked;
      SessionController.Instance.SessionChanged += OnSessionDataChanged;
    }

    private void PaintArea(AreaDto Area) {
      if (Area == null) {
        return;
      }
      Console.WriteLine(String.Format("Select '{0}'", Area.DisplayString));
      Canvas.SetArea(Area);
    }

    private void OnSelectionChanged(object sender, EventArgs e) {
      ListBox List = (ListBox)sender;
      if (List.SelectedItem is not AreaDto selectedItem) {
        return;
      } 
      if (List.Name == ListSessions.Name) {
        IList<AreaDto> knownAreas = WorldController.Instance.GetKnownAreas(selectedItem);
        Debug.WriteLine("knownAreas: " + knownAreas.Count);
      }
      PaintArea(selectedItem);
    }

    protected void BuildWorldList() {
      if (WorldController.Instance.World.Areas.Count == 0) {
        WorldController.Instance.CreateAreaFromSession();
      }
      ListMaps.DataSource = WorldController.Instance.World.AreaList;
    }

    protected void BuildSessionList() {
      ListSessions.DataSource = SessionController.Instance.SessionList;
    }

    protected void OnSessionDataChanged(Object sender, StringDataEventArgs e) {
      string currentSessionName = e.Value;
      BuildSessionList();
      foreach (AreaDto session in ListSessions.Items) {
        if (currentSessionName.Equals(session.Name)) {
          ListSessions.SelectedItem = session;
          break;
        }
      }
    }

    protected void OnToolSessionOpen(Object sender, EventArgs e) {
      AreaDto area = (AreaDto)ListSessions.SelectedItem;
      Process.Start(new ProcessStartInfo {
        Arguments = area.Path,
        FileName = "explorer.exe"
      });
    }

    protected void OnToolMapsAddClicked(Object sender, EventArgs e) {
      WorldController.Instance.CreateAreaFromSession();
    }

  }
}
