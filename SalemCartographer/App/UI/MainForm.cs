using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
      ListSessions.SelectedValueChanged += OnSessioSelectionChanged;
      ListMaps.DisplayMember = AreaDto.FIELD_TITLE;
      ListMaps.SelectedValueChanged += OnSessioSelectionChanged;
      ToolMapsAdd.Click += OnToolMapsAddClicked;
    }

    private void PaintArea(AreaDto Area) {
      if (Area == null) {
        return;
      }
      Console.WriteLine(String.Format("Select '{0}'", Area.DisplayString));
      Canvas.SetArea(Area);
    }

    private void OnSessioSelectionChanged(object sender, EventArgs e) {
      ListBox List = (ListBox)sender;
      PaintArea((AreaDto)List.SelectedItem);
    }

    protected void BuildWorldList() {
      if (WorldController.Instance.World.Areas.Count == 0) {
        WorldController.Instance.CreateAreaFromSession();
      }
      ListMaps.DataSource = WorldController.Instance.World.Areas.Values.ToList();
    }

    protected void BuildSessionList() {
      ListSessions.DataSource = SessionController.Instance.SessionList;
    }

    protected void OnToolMapsAddClicked(Object sender, EventArgs e) {
      WorldController.Instance.CreateAreaFromSession();
    }

  }
}
