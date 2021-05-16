using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalemCartographer.App.UI
{ 
  public partial class MapControl : Control
  {
    // definitions
    private const int SIZE_TILE = ApplicationConstants.TileSize;
    private const GraphicsUnit UNIT = GraphicsUnit.Pixel;
    private RectangleF IMAGE_RECT = new RectangleF(0.0F, 0.0F, SIZE_TILE, SIZE_TILE);

    // control settings
    public int FpsLock = 30;
    public Color BackgroundColor = Color.Black;
    public bool GridShow = true;
    public Color GridColor = Color.White;
    public Color NullmeridianColor = Color.Yellow;
    public Color SelectionColor = Color.Red;

    // control vars
    private Rectangle ControlCanvas;
    private Point ControlCenter;
    private DateTime LastPaint;

    // area vars
    private AreaDto Area;
    private Point AreaCenter;
    private Point? SelectedTile;

    // status vars
    private MouseButtons MouseButton;
    private Point MouseLast;

    public MapControl() {
      InitializeComponent();
      Initalize();
    }

    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      if (Area == null) {
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
    }

    protected void PaintTiles(PaintEventArgs e) {
      foreach (var Tile in Area.Tiles.Values) {
        Point Position = CalcTileCoordinates(Tile.Position);
        if (Position.X <= -SIZE_TILE || Width + SIZE_TILE <= Position.X
          || Position.Y <= -SIZE_TILE || Height + SIZE_TILE <= Position.Y) {
          continue;
        }
        e.Graphics.DrawImage(Tile.GetImage(), Position.X, Position.Y, IMAGE_RECT, UNIT);
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
      Rectangle Nullmeridian = new Rectangle(Center, new(SIZE_TILE, SIZE_TILE));
      if (Nullmeridian.IntersectsWith(ControlCanvas)) {
        Pen NullmeridianPen = new(NullmeridianColor, 1);
        e.Graphics.DrawRectangle(NullmeridianPen, Nullmeridian);
      }
      if (SelectedTile.HasValue) {
        Point SelectedCoords = CalcTileCoordinates(SelectedTile.Value);
        Rectangle Selection = new Rectangle(SelectedCoords, new(SIZE_TILE, SIZE_TILE));
        if (Selection.IntersectsWith(ControlCanvas)) {
          Pen SelectionPen = new(SelectionColor, 1);
          e.Graphics.DrawRectangle(SelectionPen, Selection);
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
          SelectedTile = CalcTilePosition(e.Location);
          Invalidate();
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
            AreaCenter.Offset(e.X - MouseLast.X, e.Y - MouseLast.Y);
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
      this.Area = Area;
      MouseButton = MouseButtons.None;
      ResetAreaCenter();
      Invalidate();
    }

    protected void ResetAreaCenter() {
      AreaCenter = new(0, 0);
      Invalidate();
    }

    protected Point CalcCenter() {
      int x = ControlCenter.X - (SIZE_TILE / 2) + AreaCenter.X;
      int y = ControlCenter.Y - (SIZE_TILE / 2) + AreaCenter.Y;
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
