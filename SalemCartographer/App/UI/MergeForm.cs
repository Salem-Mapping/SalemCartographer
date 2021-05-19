using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalemCartographer.App.UI
{
  public partial class MergeForm : Form
  {
    public AreaDto SourceArea { get; protected set; }
    public AreaDto TargetArea => (AreaDto)ComboBoxAreas.SelectedItem;

    public Point Offset {
      get {
        Point offsetThis = CanvasThis.SelectedTile ?? (new());
        Point offsetOther = CanvasOther.SelectedTile ?? (new());
        return new(offsetOther.X - offsetThis.X, offsetOther.Y - offsetThis.Y);
      }
    }

    private readonly IEnumerable<AreaDto> targetAreas;

    public MergeForm(AreaDto sourceArea, IEnumerable<AreaDto> targetAreas) {
      InitializeComponent();
      this.Load += OnLoad;
      this.SourceArea = sourceArea;
      this.targetAreas = targetAreas;
    }

    protected void OnLoad(Object sender, EventArgs ev) {
      if (targetAreas.Count() == 0) {
        DialogResult = DialogResult.Abort;
        Close();
        return;
      }
      Init();
    }

    protected void CalculateMerged() {
      if (ComboBoxAreas.SelectedItem is not AreaDto area) {
        return;
      }
      bool calcScore = false;
      Point offset = Offset;
      if (area is not MatchedAreaDto matchedArea || !offset.Equals(matchedArea.Offset)) {
        calcScore = true;
      }
      AreaDto mergeArea = new(area) {
        Type = Enum.AreaType.Preview,
        Path = String.Empty,
        Directory = String.Empty
      };
      foreach (var sourceTile in SourceArea.TileList) {
        MatchedTileDto tile = new(sourceTile);
        Point coord = tile.Coordinate;
        coord.Offset(offset);
        tile.Coordinate = coord;
        if (area.Tiles.ContainsKey(tile.GetKey())) {
          var orgTile = area.Tiles[tile.GetKey()];
          if (!calcScore && orgTile is MatchedTileDto matchedTile) {
            tile.Score = matchedTile.Score;
          } else {
            tile.Score = TileComparator.Compare(sourceTile, orgTile);
          }
        }
        mergeArea.AddTile(tile);
      }
      CanvasMerge.SetArea(mergeArea);
      CanvasMerge.CenteredTile = offset;
    }

    protected void OnAreaChanged(Object sender, EventArgs ev) {
      AreaDto area = (AreaDto)ComboBoxAreas.SelectedItem;
      CanvasOther.SetArea(area);
      CanvasThis.SelectedTile = new();
      if (area is MatchedAreaDto match) {
        CanvasOther.SelectedTile = match.Offset;
      }
      CalculateMerged();
    }

    protected void OnTileChanged(Object sender, EventArgs ev) {
      CalculateMerged();
    }

    protected void OnSubmit(Object sender, EventArgs ev) {
      DialogResult = DialogResult.OK;
      Close();
    }

    protected void OnCancel(Object sender, EventArgs ev) {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    protected void Init() {
      ComboBoxAreas.DropDownStyle = ComboBoxStyle.DropDownList;
      ComboBoxAreas.DisplayMember = AreaDto.FIELD_TITLE;
      ComboBoxAreas.DataSource = targetAreas;
      ComboBoxAreas.SelectedValueChanged += OnAreaChanged;

      CanvasOther.AllowSelection = true;
      CanvasOther.SelectionChanged += OnTileChanged;

      CanvasThis.AllowSelection = true;
      CanvasThis.SelectionChanged += OnTileChanged;
      CanvasThis.SetArea(SourceArea);

      CanvasMerge.AllowSelection = false;

      ButtonMerge.Click += OnSubmit;
      ButtonCancel.Click += OnSubmit;

      // set data
      ComboBoxAreas.SelectedItem = targetAreas.OrderByDescending(a => (a is MatchedAreaDto m) ? m.Score : 0).First();
      OnAreaChanged(this, EventArgs.Empty);
    }
  }
}