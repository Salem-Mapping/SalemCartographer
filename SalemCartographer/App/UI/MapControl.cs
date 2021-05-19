using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalemCartographer.App.UI
{
  public partial class MapControl : Control
  {
    // definitions
    private const int SIZE_TILE = ApplicationConstants.TileSize;

    private const GraphicsUnit UNIT = GraphicsUnit.Pixel;

    // control settings
    public int FpsLock = 30;

    public Color BackgroundColor = Color.Black;
    public bool GridShow = true;
    public Color GridColor = Color.White;
    public Color NullmeridianColor = Color.Blue;
    public Color MatchColor = Color.Green;
    public float MatchScore = 0.9F;
    public Color PartialMatchColor = Color.Yellow;
    public float PartialScore = 0.5F;
    public Color DismatchColor = Color.Red;

    public Point? SelectedTile {
      get => selectedTile; set {
        selectedTile = value;
        if (value.HasValue) {
          CenteredTile = value.Value;
        } else {
          Invalidate();
        }
      }
    }

    public Point CenteredTile {
      get => CalcTilePosition(new(areaCenter.X * -1, areaCenter.Y * -1)); set {
        areaCenter = CalcTileCoordinates(new(value.X * -1, value.Y * -1));
        Invalidate();
      }
    }

    // selection
    public bool AllowSelection = true;

    public Color SelectionColor = Color.DarkViolet;

    public event EventHandler SelectionChanged;

    // control vars
    private Rectangle ControlCanvas;

    private Point ControlCenter;
    private DateTime LastPaint;

    // area vars
    private AreaDto area;

    private Point areaCenter;
    private Size tileSize = new(SIZE_TILE, SIZE_TILE);
    private Rectangle imageRect = new(new(0, 0), new(SIZE_TILE, SIZE_TILE));
    private IEnumerable<MatchedAreaDto> matchedAreas = Enumerable.Empty<MatchedAreaDto>();
    private Point? selectedTile;

    // status vars
    private MouseButtons MouseButton;

    private Point MouseLast;

    public MapControl() {
      InitializeComponent();
      Initalize();
    }

    public void AddMatchedAreas(IEnumerable<MatchedAreaDto> areas) {
      matchedAreas = areas;
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e) {
      try {
        base.OnPaint(e);
        if (area == null) {
          return;
        }
        if ((DateTime.Now - LastPaint).TotalMilliseconds < FpsLock / 1000) {
          Invalidate();
          return;
        }
        LastPaint = DateTime.Now;

        e.Graphics.Clear(BackgroundColor);
        PaintTiles(e);
        PaintGrid(e);
        // PrintFps();
      } finally {
        e.Dispose();
      }
    }

    protected void PaintTiles(PaintEventArgs e) {
      foreach (var Tile in area.Tiles.Values) {
        Point Position = CalcTileCoordinates(Tile.Coordinate);
        if (Position.X <= -SIZE_TILE || Width + SIZE_TILE <= Position.X
          || Position.Y <= -SIZE_TILE || Height + SIZE_TILE <= Position.Y) {
          continue;
        }

        Image image = Tile.GetImage();
        e.Graphics.DrawImage(image, Position.X, Position.Y, imageRect, UNIT);
      }
    }

    protected void PaintGrid(PaintEventArgs e) {
      Point Center = CalcCenter();
      Point Offset = new(Center.X % SIZE_TILE, Center.Y % SIZE_TILE);
      Pen GridPen = new(GridColor, 1);
      for (int x = Offset.X; x < Width; x += SIZE_TILE) {
        e.Graphics.DrawLine(GridPen, new(x, 0), new(x, Height));
      }
      for (int y = Offset.Y; y < Height; y += SIZE_TILE) {
        e.Graphics.DrawLine(GridPen, new(0, y), new(Width, y));
      }

      Rectangle Nullmeridian = new(Center, tileSize);
      if (Nullmeridian.IntersectsWith(ControlCanvas)) {
        GridPen.Color = NullmeridianColor;
        e.Graphics.DrawRectangle(GridPen, Nullmeridian);
      }

      AreaDto matchedArea = area;
      if (matchedAreas.Any()) {
        try {
          matchedArea = matchedAreas.OrderByDescending(a => a.ScoreNormalized).First();
        } catch (Exception ex) {
          Debug.WriteLine(ex);
        }
      }
      foreach (TileDto tile in area.Tiles.Values) {
        try {
          if (tile is not MatchedTileDto matchTile || !matchTile.Score.HasValue) {
            continue;
          }
          Color color;
          if (MatchScore < matchTile.Score.Value) {
            color = MatchColor;
          } else if (PartialScore < matchTile.Score.Value) {
            color = PartialMatchColor;
          } else {
            color = DismatchColor;
          }
          Point SelectedCoords = CalcTileCoordinates(tile.Coordinate);
          Rectangle MatchGrid = new(SelectedCoords, tileSize);
          GridPen.Color = color;
          e.Graphics.DrawRectangle(GridPen, MatchGrid);
        } catch (Exception ex) {
          Debug.WriteLine(ex);
        }
      }

      if (SelectedTile.HasValue) {
        Point SelectedCoords = CalcTileCoordinates(SelectedTile.Value);
        Rectangle Selection = new(SelectedCoords, tileSize);
        if (Selection.IntersectsWith(ControlCanvas)) {
          GridPen.Color = SelectionColor;
          e.Graphics.DrawRectangle(GridPen, Selection);
        }
      }
    }

    private DateTime date = DateTime.UtcNow;
    private long times = 0;

    protected void PrintFps() {
      //Required to tell me the framerate
      if ((DateTime.UtcNow - date).TotalSeconds > 1) {
        date = DateTime.UtcNow;
        Debug.WriteLine(times);
        times = 0;
      }
      times++;
    }

    protected override void OnMouseClick(MouseEventArgs e) {
      base.OnMouseClick(e);
      switch (e.Button) {
        case MouseButtons.Left:
          if (AllowSelection) {
            SelectedTile = CalcTilePosition(e.Location);
            Invalidate();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
          }
          break;

        case MouseButtons.None:
          break;

        case MouseButtons.Right:
          break;

        case MouseButtons.Middle:
          ResetAreaCenter();
          break;

        case MouseButtons.XButton1:
          break;

        case MouseButtons.XButton2:
          break;

        default:
          break;
      }
    }

    protected override void OnMouseEnter(EventArgs e) {
      base.OnMouseEnter(e);
    }

    protected override void OnMouseHover(EventArgs e) {
      base.OnMouseHover(e);
    }

    protected override void OnMouseLeave(EventArgs e) {
      base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e) {
      base.OnMouseMove(e);

      switch (MouseButton) {
        case MouseButtons.Left:
          break;

        case MouseButtons.Right:
          if (MouseLast.X != e.X || MouseLast.Y != e.Y) {
            areaCenter.Offset(e.X - MouseLast.X, e.Y - MouseLast.Y);
            Invalidate();
          }
          break;

        case MouseButtons.Middle:
          break;

        case MouseButtons.XButton1:
          break;

        case MouseButtons.XButton2:
          break;

        case MouseButtons.None:
          break;

        default:
          break;
      }
      MouseLast.X = e.X;
      MouseLast.Y = e.Y;
    }

    protected override void OnMouseWheel(MouseEventArgs e) {
      base.OnMouseWheel(e);
      Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);
      MouseButton = MouseButtons.None;
    }

    protected override void OnMouseDown(MouseEventArgs e) {
      base.OnMouseDown(e);
      MouseButton = e.Button;
    }

    protected override void OnResize(EventArgs e) {
      ControlCenter = new Point(Width / 2, Height / 2);
      ControlCanvas = new Rectangle(0, 0, Width, Height);
      Invalidate();
    }

    protected void Initalize() {
      DoubleBuffered = true;
    }

    public void SetArea(AreaDto Area) {
      area = Area;
      matchedAreas = Enumerable.Empty<MatchedAreaDto>();
      SelectedTile = null;
      MouseButton = MouseButtons.None;
      ResetAreaCenter();
      Invalidate();
    }

    protected void ResetAreaCenter() {
      areaCenter = new(0, 0);
      Invalidate();
    }

    protected Point CalcCenter() {
      int x = ControlCenter.X - (SIZE_TILE / 2) + areaCenter.X;
      int y = ControlCenter.Y - (SIZE_TILE / 2) + areaCenter.Y;
      return new(x, y);
    }

    private Point CalcTilePosition(Point Coordinate) {
      Point Center = CalcCenter();
      int x = (Coordinate.X - Center.X);
      int y = (Coordinate.Y - Center.Y);
      return new((x >= 0 ? x : x - SIZE_TILE) / SIZE_TILE, (y >= 0 ? y : y - SIZE_TILE) / SIZE_TILE);
    }

    protected Point CalcTileCoordinates(Point Position) {
      Point Center = CalcCenter();
      return new(Center.X + (Position.X * SIZE_TILE), Center.Y + (Position.Y * SIZE_TILE));
    }
  }
}