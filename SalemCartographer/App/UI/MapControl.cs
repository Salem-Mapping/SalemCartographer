using SalemCartographer.App.Model;
using SalemCartographer.App.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace SalemCartographer.App.UI
{
  public partial class MapControl : Control
  {
    // definitions
    private const float TILE_SIZE = AppConstants.TileSize;
    private const float ZOOM_FACOTR = 0.2F;
    private const GraphicsUnit UNIT = GraphicsUnit.Pixel;
    private const int SPACING_X = 10;
    private const int SPACING_Y = 10;

    // control settings
    public int FpsLock = 30;

    public Color BackgroundColor = Color.Black;
    public bool GridShow = true;
    public Color GridColor = Color.White;
    public Color NullmeridianColor = Color.Blue;
    public Color MatchColor = Color.Green;
    public float MatchScore = 1F;
    public Color PartialMatchColor = Color.Yellow;
    public float PartialScore = 0.75F;
    public Color DismatchColor = Color.Red;
    public int Zoom;
    public Point? SelectedTile {
      get => selectedTile; set {
        selectedTile = value;
        if (value.HasValue) {
          CenteredTile = value.Value;
        }
        else {
          Invalidate();
        }
      }
    }

    public Point CenteredTile {
      get => CalcPos(PointUtils.Invert(areaCenter));
      set {
        areaCenter = PointUtils.Invert(CalcCoord(value));
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
    private int SizeZoomed => (int)(TILE_SIZE * Math.Pow(1 + ZOOM_FACOTR, Zoom));
    private Size TileSize => new(SizeZoomed, SizeZoomed);
    private Point? selectedTile;
    private readonly Dictionary<string, Image> imageCache = new();
    private Point MouseLast;

    public MapControl() {
      InitializeComponent();
      Initalize();
    }

    protected override void OnPaint(PaintEventArgs e) {
      try {
        base.OnPaint(e);
        Graphics graphics = e.Graphics;
        if (area == null) {
          return;
        }
        if ((DateTime.Now - LastPaint).TotalMilliseconds < FpsLock / 1000) {
          Invalidate();
          return;
        }
        LastPaint = DateTime.Now;

        graphics.Clear(BackgroundColor);
        PaintTiles(graphics);
        PaintGrid(graphics);
        // PrintFps();
      } finally {
        e.Dispose();
      }
    }

    protected void PaintTiles(Graphics graphics) {
      RectangleF srcRect = new(new(0, 0), new(TILE_SIZE, TILE_SIZE));
      var mode = graphics.CompositingMode;
      graphics.CompositingMode = CompositingMode.SourceCopy;
      foreach (var tile in area.TileList) {
        Point position = CalcCanvasCoordinates(tile.Coordinate);
        if (position.X <= -SizeZoomed || Width + SizeZoomed <= position.X
          || position.Y <= -SizeZoomed || Height + SizeZoomed <= position.Y) {
          continue;
        }
        lock (tile) {
          RectangleF destRect = new(position, new(SizeZoomed, SizeZoomed));
          Image image = GetImage(tile);
          if (image == null) { continue; }
          graphics.DrawImage(image, destRect, srcRect, UNIT);
        }
      }
      graphics.CompositingMode = mode;
    }

    private Image GetImage(TileDto tile) {
      Image image = null;
      if (!String.IsNullOrWhiteSpace(tile.Hash) && imageCache.ContainsKey(tile.Hash)) {
        image = imageCache[tile.Hash];
      }
      if (image == null) { image = tile.Image; }
      return image;
    }

    protected void ClearImageCache() {
      foreach (var img in imageCache.Values) {
        img.Dispose();
      }
      imageCache.Clear();
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    protected void PaintGrid(Graphics graphics) {
      try {
        Point center = CalcCenter();
        Pen gridPen = new(GridColor, 1);
        SolidBrush fontBrush = new(GridColor);
        if (GridShow && Zoom >= -5) {
          Point offset = new(center.X % SizeZoomed, center.Y % SizeZoomed);
          for (int x = offset.X; x < Width; x += SizeZoomed) {
            graphics.DrawLine(gridPen, new(x, 0), new(x, Height));
          }
          for (int y = offset.Y; y < Height; y += SizeZoomed) {
            graphics.DrawLine(gridPen, new(0, y), new(Width, y));
          }

          Rectangle nullmeridian = new(center, TileSize);
          if (nullmeridian.IntersectsWith(ControlCanvas)) {
            gridPen.Color = NullmeridianColor;
            graphics.DrawRectangle(gridPen, nullmeridian);
          }

          try {
            StringFormat drawFormat = new() { Alignment = StringAlignment.Center };
            PointF legendX = new(
              offset.X > SizeZoomed / 2 ? offset.X - (SizeZoomed / 2) : offset.X + (SizeZoomed / 2),
              SPACING_Y);
            legendX.X = ((legendX.X < 0) ? SizeZoomed : 0) + legendX.X;
            legendX.Y = ((legendX.Y < 0) ? SizeZoomed : 0) + legendX.Y;
            for (float x = legendX.X; x < Width; x += SizeZoomed) {
              Point pos = CalcCanvasPosition(new Point((int)x, (int)legendX.Y));
              graphics.DrawString("" + pos.X, Font, fontBrush, x, legendX.Y, drawFormat);
            }
            drawFormat.Alignment = StringAlignment.Near;
            PointF legendY = new(((offset.X > 40F) ? 0F : offset.X) + SPACING_X, offset.Y + (SizeZoomed / 2) - Font.Size / 2);
            legendY.X = ((legendY.X < 0) ? SizeZoomed : 0) + legendY.X;
            legendY.Y = ((legendY.Y < 0) ? SizeZoomed : 0) + legendY.Y;
            for (float y = legendY.Y; y < Width; y += SizeZoomed) {
              Point pos = CalcCanvasPosition(new Point((int)legendX.Y, (int)y));
              graphics.DrawString("" + pos.Y, Font, fontBrush, legendY.X, y, drawFormat);
            }
          } catch (Exception e) {
            Debug.WriteLine(e);
          }
        }

        AreaDto matchedArea = area;
        if (area.MatchingAreas.Any()) {
          try {
            matchedArea = area.MatchingAreas.OrderByDescending(a => a.ScoreNormalized.HasValue ? a.ScoreNormalized : 0).First();
          } catch (Exception ex) {
            Debug.WriteLine(ex);
          }
        }
        foreach (TileDto tile in area.TileList) {
          try {
            if (!tile.Score.HasValue) {
              continue;
            }
            Color color;
            if (MatchScore <= tile.Score.Value) {
              color = MatchColor;
            }
            else if (PartialScore <= tile.Score.Value) {
              color = PartialMatchColor;
            }
            else {
              color = DismatchColor;
            }
            Point SelectedCoords = CalcCanvasCoordinates(tile.Coordinate);
            Rectangle MatchGrid = new(SelectedCoords, TileSize);
            gridPen.Color = color;
            graphics.DrawRectangle(gridPen, MatchGrid);
          } catch (Exception ex) {
            Debug.WriteLine(ex);
          }
        }

        if (selectedTile.HasValue) {
          Point SelectedCoords = CalcCanvasCoordinates(selectedTile.Value);
          Rectangle Selection = new(SelectedCoords, TileSize);
          if (Selection.IntersectsWith(ControlCanvas)) {
            gridPen.Color = SelectionColor;
            graphics.DrawRectangle(gridPen, Selection);
          }
        }
      } catch (Exception e) {
        Debug.WriteLine(e);
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
          SelectTile(CalcCanvasPosition(e.Location));
          break;
        case MouseButtons.Middle:
          ResetZoom();
          break;
        case MouseButtons.Right:
        case MouseButtons.XButton1:
        case MouseButtons.XButton2:
        case MouseButtons.None:
        default:
          break;
      }
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e) {
      base.OnMouseDoubleClick(e);
      switch (e.Button) {
        case MouseButtons.Left:
          DeselectTile();
          break;
        case MouseButtons.None:
        case MouseButtons.Right:
        case MouseButtons.Middle:
        case MouseButtons.XButton1:
        case MouseButtons.XButton2:
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
      switch (e.Button) {
        case MouseButtons.Left:
          MoveMap(e.Location);
          break;
        case MouseButtons.Right:
        case MouseButtons.Middle:
        case MouseButtons.XButton1:
        case MouseButtons.XButton2:
        case MouseButtons.None:
        default:
          break;
      }
      MouseLast = e.Location;
    }

    private void MoveMap(Point p) {
      if (area == null) {
        return;
      }
      if (MouseLast.X != p.X || MouseLast.Y != p.Y) {
        areaCenter.Offset(p.X - MouseLast.X, p.Y - MouseLast.Y);
        area.LastLocation = CenteredTile;
        Debug.WriteLine("Last: " + area.LastLocation);
      }
      Invalidate();
    }

    private void ResetZoom() {
      CenteredTile = SelectedTile ?? new Point(0, 0);
      Zoom = 0;
      Invalidate();
    }

    protected override void OnMouseWheel(MouseEventArgs e) {
      base.OnMouseWheel(e);
      ChangeZoomBy(e.Delta / SystemInformation.MouseWheelScrollDelta);
    }

    private void ChangeZoomBy(int amont) {
      Point p = CenteredTile;
      Zoom += amont;
      Debug.WriteLine("Zoom: " + Zoom);
      CenteredTile = p;
      Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e) {
      base.OnMouseDown(e);
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
      ClearImageCache();
      area = Area;
      SelectedTile = null;
      ResetArea();
      if (area.LastLocation.HasValue) {
        CenteredTile = area.LastLocation.Value;
      }
      Invalidate();
    }

    protected void SelectTile(Point coord) {
      if (AllowSelection && area != null) {
        selectedTile = coord;
        Invalidate();
        SelectionChanged?.Invoke(this, EventArgs.Empty);
        string key = String.Format("{0}_{1}", coord.X, coord.Y);
        if (area.Tiles.ContainsKey(key)) {
          var tile = area.Tiles[key];
          if (Debugger.IsAttached) {
            Debug.WriteLine(String.Format("tile[{0}/{1}]: {2} ", tile.X, tile.Y, tile.Date));
          }
        }
      }
    }

    protected void DeselectTile() {
      selectedTile = null;
      Invalidate();
      SelectionChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void ResetArea() {
      areaCenter = new(0, 0);
      Zoom = 0;
      Invalidate();
    }

    protected Point CalcCenter() {
      int x = ControlCenter.X - (SizeZoomed / 2) + areaCenter.X;
      int y = ControlCenter.Y - (SizeZoomed / 2) + areaCenter.Y;
      return new(x, y);
    }

    protected Point CalcCanvasPosition(Point coordinate) => CalcPos(PointUtils.Substract(coordinate, CalcCenter()));
    protected Point CalcCanvasCoordinates(Point position) => PointUtils.Add(CalcCoord(position), CalcCenter());
    protected Point CalcPos(Point p) => PointUtils.DivideAwaiZero(p, SizeZoomed);
    protected Point CalcCoord(Point p) => PointUtils.Multiply(p, SizeZoomed);

  }
}