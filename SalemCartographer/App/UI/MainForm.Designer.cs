
namespace SalemCartographer.App.UI
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.Status = new System.Windows.Forms.StatusStrip();
      this.SplitLeft = new System.Windows.Forms.SplitContainer();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.ToolSession = new System.Windows.Forms.ToolStrip();
      this.ToolSessionMerge = new System.Windows.Forms.ToolStripButton();
      this.ToolSessionOpen = new System.Windows.Forms.ToolStripButton();
      this.ToolSessionDelete = new System.Windows.Forms.ToolStripButton();
      this.ListSessions = new System.Windows.Forms.ListBox();
      this.SplitRight = new System.Windows.Forms.SplitContainer();
      this.Canvas = new SalemCartographer.App.UI.MapControl();
      this.SplitRightHorizontal = new System.Windows.Forms.SplitContainer();
      this.ListMaps = new System.Windows.Forms.ListBox();
      this.ToolMaps = new System.Windows.Forms.ToolStrip();
      this.ToolMapsAdd = new System.Windows.Forms.ToolStripButton();
      this.ToolMapsOpen = new System.Windows.Forms.ToolStripButton();
      this.ToolMapsDelete = new System.Windows.Forms.ToolStripButton();
      this.ListPoi = new System.Windows.Forms.ListBox();
      this.ToolPoi = new System.Windows.Forms.ToolStrip();
      this.menuStrip2 = new System.Windows.Forms.MenuStrip();
      this.ToolAutoMerge = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.SplitLeft)).BeginInit();
      this.SplitLeft.Panel1.SuspendLayout();
      this.SplitLeft.Panel2.SuspendLayout();
      this.SplitLeft.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.ToolSession.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitRight)).BeginInit();
      this.SplitRight.Panel1.SuspendLayout();
      this.SplitRight.Panel2.SuspendLayout();
      this.SplitRight.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitRightHorizontal)).BeginInit();
      this.SplitRightHorizontal.Panel1.SuspendLayout();
      this.SplitRightHorizontal.Panel2.SuspendLayout();
      this.SplitRightHorizontal.SuspendLayout();
      this.ToolMaps.SuspendLayout();
      this.menuStrip2.SuspendLayout();
      this.SuspendLayout();
      // 
      // Status
      // 
      this.Status.Location = new System.Drawing.Point(0, 539);
      this.Status.Name = "Status";
      this.Status.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
      this.Status.Size = new System.Drawing.Size(784, 22);
      this.Status.TabIndex = 0;
      this.Status.Text = "statusStrip1";
      // 
      // SplitLeft
      // 
      this.SplitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.SplitLeft.Location = new System.Drawing.Point(0, 28);
      this.SplitLeft.Margin = new System.Windows.Forms.Padding(0);
      this.SplitLeft.Name = "SplitLeft";
      // 
      // SplitLeft.Panel1
      // 
      this.SplitLeft.Panel1.Controls.Add(this.tableLayoutPanel1);
      this.SplitLeft.Panel1MinSize = 200;
      // 
      // SplitLeft.Panel2
      // 
      this.SplitLeft.Panel2.Controls.Add(this.SplitRight);
      this.SplitLeft.Size = new System.Drawing.Size(784, 511);
      this.SplitLeft.SplitterDistance = 200;
      this.SplitLeft.SplitterWidth = 5;
      this.SplitLeft.TabIndex = 2;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.ToolSession, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.ListSessions, 0, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 511);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // ToolSession
      // 
      this.ToolSession.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolSessionMerge,
            this.ToolSessionOpen,
            this.ToolSessionDelete});
      this.ToolSession.Location = new System.Drawing.Point(0, 0);
      this.ToolSession.Name = "ToolSession";
      this.ToolSession.Size = new System.Drawing.Size(200, 25);
      this.ToolSession.TabIndex = 1;
      // 
      // ToolSessionMerge
      // 
      this.ToolSessionMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolSessionMerge.Image = global::SalemCartographer.Properties.Resources.object_group_regular_24_black;
      this.ToolSessionMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolSessionMerge.Name = "ToolSessionMerge";
      this.ToolSessionMerge.Size = new System.Drawing.Size(23, 22);
      // 
      // ToolSessionOpen
      // 
      this.ToolSessionOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolSessionOpen.Image = global::SalemCartographer.Properties.Resources.folder_open_solid;
      this.ToolSessionOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolSessionOpen.Name = "ToolSessionOpen";
      this.ToolSessionOpen.Size = new System.Drawing.Size(23, 22);
      // 
      // ToolSessionDelete
      // 
      this.ToolSessionDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolSessionDelete.Image = global::SalemCartographer.Properties.Resources.trash_solid_24_black;
      this.ToolSessionDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolSessionDelete.Name = "ToolSessionDelete";
      this.ToolSessionDelete.Size = new System.Drawing.Size(23, 22);
      // 
      // ListSessions
      // 
      this.ListSessions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListSessions.FormattingEnabled = true;
      this.ListSessions.ItemHeight = 20;
      this.ListSessions.Location = new System.Drawing.Point(0, 25);
      this.ListSessions.Margin = new System.Windows.Forms.Padding(0);
      this.ListSessions.Name = "ListSessions";
      this.ListSessions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.ListSessions.Size = new System.Drawing.Size(200, 486);
      this.ListSessions.TabIndex = 0;
      // 
      // SplitRight
      // 
      this.SplitRight.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.SplitRight.Location = new System.Drawing.Point(0, 0);
      this.SplitRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SplitRight.Name = "SplitRight";
      // 
      // SplitRight.Panel1
      // 
      this.SplitRight.Panel1.Controls.Add(this.Canvas);
      // 
      // SplitRight.Panel2
      // 
      this.SplitRight.Panel2.Controls.Add(this.SplitRightHorizontal);
      this.SplitRight.Panel2MinSize = 200;
      this.SplitRight.Size = new System.Drawing.Size(579, 511);
      this.SplitRight.SplitterDistance = 359;
      this.SplitRight.SplitterWidth = 5;
      this.SplitRight.TabIndex = 0;
      // 
      // Canvas
      // 
      this.Canvas.BackColor = System.Drawing.SystemColors.WindowText;
      this.Canvas.CenteredTile = new System.Drawing.Point(-95, -158);
      this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Canvas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.Canvas.ForeColor = System.Drawing.SystemColors.Control;
      this.Canvas.Location = new System.Drawing.Point(0, 0);
      this.Canvas.Name = "Canvas";
      this.Canvas.SelectedTile = null;
      this.Canvas.Size = new System.Drawing.Size(359, 511);
      this.Canvas.TabIndex = 0;
      this.Canvas.Text = "graphicCanvas1";
      // 
      // SplitRightHorizontal
      // 
      this.SplitRightHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitRightHorizontal.Location = new System.Drawing.Point(0, 0);
      this.SplitRightHorizontal.Name = "SplitRightHorizontal";
      this.SplitRightHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // SplitRightHorizontal.Panel1
      // 
      this.SplitRightHorizontal.Panel1.Controls.Add(this.ListMaps);
      this.SplitRightHorizontal.Panel1.Controls.Add(this.ToolMaps);
      this.SplitRightHorizontal.Panel1MinSize = 100;
      // 
      // SplitRightHorizontal.Panel2
      // 
      this.SplitRightHorizontal.Panel2.Controls.Add(this.ListPoi);
      this.SplitRightHorizontal.Panel2.Controls.Add(this.ToolPoi);
      this.SplitRightHorizontal.Panel2MinSize = 100;
      this.SplitRightHorizontal.Size = new System.Drawing.Size(215, 511);
      this.SplitRightHorizontal.SplitterDistance = 247;
      this.SplitRightHorizontal.TabIndex = 0;
      // 
      // ListMaps
      // 
      this.ListMaps.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListMaps.FormattingEnabled = true;
      this.ListMaps.ItemHeight = 20;
      this.ListMaps.Location = new System.Drawing.Point(0, 25);
      this.ListMaps.Name = "ListMaps";
      this.ListMaps.Size = new System.Drawing.Size(215, 222);
      this.ListMaps.TabIndex = 0;
      // 
      // ToolMaps
      // 
      this.ToolMaps.AllowMerge = false;
      this.ToolMaps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMapsAdd,
            this.ToolMapsOpen,
            this.ToolMapsDelete});
      this.ToolMaps.Location = new System.Drawing.Point(0, 0);
      this.ToolMaps.Name = "ToolMaps";
      this.ToolMaps.Size = new System.Drawing.Size(215, 25);
      this.ToolMaps.TabIndex = 0;
      this.ToolMaps.Text = "toolStrip1";
      // 
      // ToolMapsAdd
      // 
      this.ToolMapsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolMapsAdd.Image = global::SalemCartographer.Properties.Resources.plus_solid_24_black;
      this.ToolMapsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolMapsAdd.Name = "ToolMapsAdd";
      this.ToolMapsAdd.Size = new System.Drawing.Size(23, 22);
      // 
      // ToolMapsOpen
      // 
      this.ToolMapsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolMapsOpen.Image = global::SalemCartographer.Properties.Resources.folder_open_solid;
      this.ToolMapsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolMapsOpen.Name = "ToolMapsOpen";
      this.ToolMapsOpen.Size = new System.Drawing.Size(23, 22);
      // 
      // ToolMapsDelete
      // 
      this.ToolMapsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ToolMapsDelete.Image = global::SalemCartographer.Properties.Resources.trash_solid_24_black;
      this.ToolMapsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ToolMapsDelete.Name = "ToolMapsDelete";
      this.ToolMapsDelete.Size = new System.Drawing.Size(23, 22);
      // 
      // ListPoi
      // 
      this.ListPoi.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListPoi.FormattingEnabled = true;
      this.ListPoi.ItemHeight = 20;
      this.ListPoi.Location = new System.Drawing.Point(0, 25);
      this.ListPoi.Name = "ListPoi";
      this.ListPoi.Size = new System.Drawing.Size(215, 235);
      this.ListPoi.TabIndex = 1;
      // 
      // ToolPoi
      // 
      this.ToolPoi.Location = new System.Drawing.Point(0, 0);
      this.ToolPoi.Name = "ToolPoi";
      this.ToolPoi.Size = new System.Drawing.Size(215, 25);
      this.ToolPoi.TabIndex = 0;
      this.ToolPoi.Text = "toolStrip2";
      // 
      // menuStrip2
      // 
      this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolAutoMerge});
      this.menuStrip2.Location = new System.Drawing.Point(0, 0);
      this.menuStrip2.Name = "menuStrip2";
      this.menuStrip2.Size = new System.Drawing.Size(784, 28);
      this.menuStrip2.TabIndex = 3;
      this.menuStrip2.Text = "menuStrip2";
      // 
      // ToolAutoMerge
      // 
      this.ToolAutoMerge.Name = "ToolAutoMerge";
      this.ToolAutoMerge.Size = new System.Drawing.Size(102, 24);
      this.ToolAutoMerge.Text = "Auto-Merge";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.AppWorkspace;
      this.ClientSize = new System.Drawing.Size(784, 561);
      this.Controls.Add(this.SplitLeft);
      this.Controls.Add(this.Status);
      this.Controls.Add(this.menuStrip2);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "MainForm";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.SplitLeft.Panel1.ResumeLayout(false);
      this.SplitLeft.Panel1.PerformLayout();
      this.SplitLeft.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SplitLeft)).EndInit();
      this.SplitLeft.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ToolSession.ResumeLayout(false);
      this.ToolSession.PerformLayout();
      this.SplitRight.Panel1.ResumeLayout(false);
      this.SplitRight.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SplitRight)).EndInit();
      this.SplitRight.ResumeLayout(false);
      this.SplitRightHorizontal.Panel1.ResumeLayout(false);
      this.SplitRightHorizontal.Panel1.PerformLayout();
      this.SplitRightHorizontal.Panel2.ResumeLayout(false);
      this.SplitRightHorizontal.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitRightHorizontal)).EndInit();
      this.SplitRightHorizontal.ResumeLayout(false);
      this.ToolMaps.ResumeLayout(false);
      this.ToolMaps.PerformLayout();
      this.menuStrip2.ResumeLayout(false);
      this.menuStrip2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip Status;
    private System.Windows.Forms.SplitContainer SplitLeft;
    private System.Windows.Forms.ListBox ListSessions;
    private System.Windows.Forms.SplitContainer SplitRight;
    private MapControl Canvas;
    private System.Windows.Forms.SplitContainer SplitRightHorizontal;
    private System.Windows.Forms.ListBox ListMaps;
    private System.Windows.Forms.ToolStrip ToolMaps;
    private System.Windows.Forms.ListBox ListPoi;
    private System.Windows.Forms.ToolStrip ToolPoi; 
    private System.Windows.Forms.ToolStripButton ToolMapsAdd;
    private System.Windows.Forms.ToolStripButton ToolMapsDelete;
    private System.Windows.Forms.ToolStrip ToolSession;
    private System.Windows.Forms.ToolStripButton ToolSessionOpen;
    private System.Windows.Forms.ToolStripButton ToolSessionDelete;
    private System.Windows.Forms.ToolStripButton ToolMapsOpen;
    private System.Windows.Forms.ToolStripButton ToolSessionMerge;
    private System.Windows.Forms.MenuStrip menuStrip2;
    private System.Windows.Forms.ToolStripMenuItem ToolAutoMerge;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
  }
}

