
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
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.SplitLeft = new System.Windows.Forms.SplitContainer();
      this.ListSessions = new System.Windows.Forms.ListBox();
      this.SplitRight = new System.Windows.Forms.SplitContainer();
      this.Canvas = new SalemCartographer.App.UI.MapControl();
      this.SplitRightHorizontal = new System.Windows.Forms.SplitContainer();
      this.ListMaps = new System.Windows.Forms.ListBox();
      this.ToolMaps = new System.Windows.Forms.ToolStrip();
      this.ToolMapsAdd = new System.Windows.Forms.ToolStripButton();
      this.ToolMapsDelete = new System.Windows.Forms.ToolStripButton();
      this.ListPoi = new System.Windows.Forms.ListBox();
      this.ToolPoi = new System.Windows.Forms.ToolStrip();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.ToolSessionOpen = new System.Windows.Forms.ToolStripButton();
      this.ToolSessionDelete = new System.Windows.Forms.ToolStripButton();
      ((System.ComponentModel.ISupportInitialize)(this.SplitLeft)).BeginInit();
      this.SplitLeft.Panel1.SuspendLayout();
      this.SplitLeft.Panel2.SuspendLayout();
      this.SplitLeft.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitRight)).BeginInit();
      this.SplitRight.Panel1.SuspendLayout();
      this.SplitRight.Panel2.SuspendLayout();
      this.SplitRight.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitRightHorizontal)).BeginInit();
      this.SplitRightHorizontal.Panel1.SuspendLayout();
      this.SplitRightHorizontal.Panel2.SuspendLayout();
      this.SplitRightHorizontal.SuspendLayout();
      this.ToolMaps.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusStrip1
      // 
      this.statusStrip1.Location = new System.Drawing.Point(0, 539);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
      this.statusStrip1.Size = new System.Drawing.Size(784, 22);
      this.statusStrip1.TabIndex = 0;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // menuStrip1
      // 
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
      this.menuStrip1.Size = new System.Drawing.Size(784, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // SplitLeft
      // 
      this.SplitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.SplitLeft.Location = new System.Drawing.Point(0, 24);
      this.SplitLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SplitLeft.Name = "SplitLeft";
      // 
      // SplitLeft.Panel1
      // 
      this.SplitLeft.Panel1.Controls.Add(this.toolStrip1);
      this.SplitLeft.Panel1.Controls.Add(this.ListSessions);
      this.SplitLeft.Panel1MinSize = 200;
      // 
      // SplitLeft.Panel2
      // 
      this.SplitLeft.Panel2.Controls.Add(this.SplitRight);
      this.SplitLeft.Size = new System.Drawing.Size(784, 515);
      this.SplitLeft.SplitterDistance = 200;
      this.SplitLeft.SplitterWidth = 5;
      this.SplitLeft.TabIndex = 2;
      // 
      // ListSessions
      // 
      this.ListSessions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListSessions.FormattingEnabled = true;
      this.ListSessions.ItemHeight = 20;
      this.ListSessions.Location = new System.Drawing.Point(0, 0);
      this.ListSessions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ListSessions.Name = "ListSessions";
      this.ListSessions.Size = new System.Drawing.Size(200, 515);
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
      this.SplitRight.Size = new System.Drawing.Size(579, 515);
      this.SplitRight.SplitterDistance = 367;
      this.SplitRight.SplitterWidth = 5;
      this.SplitRight.TabIndex = 0;
      // 
      // Canvas
      // 
      this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Canvas.Location = new System.Drawing.Point(0, 0);
      this.Canvas.Name = "Canvas";
      this.Canvas.Size = new System.Drawing.Size(367, 515);
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
      this.SplitRightHorizontal.Size = new System.Drawing.Size(207, 515);
      this.SplitRightHorizontal.SplitterDistance = 250;
      this.SplitRightHorizontal.TabIndex = 0;
      // 
      // ListMaps
      // 
      this.ListMaps.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListMaps.FormattingEnabled = true;
      this.ListMaps.ItemHeight = 20;
      this.ListMaps.Location = new System.Drawing.Point(0, 25);
      this.ListMaps.Name = "ListMaps";
      this.ListMaps.Size = new System.Drawing.Size(207, 225);
      this.ListMaps.TabIndex = 0;
      // 
      // ToolMaps
      // 
      this.ToolMaps.AllowMerge = false;
      this.ToolMaps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMapsAdd,
            this.ToolMapsDelete});
      this.ToolMaps.Location = new System.Drawing.Point(0, 0);
      this.ToolMaps.Name = "ToolMaps";
      this.ToolMaps.Size = new System.Drawing.Size(207, 25);
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
      this.ListPoi.Size = new System.Drawing.Size(207, 236);
      this.ListPoi.TabIndex = 1;
      // 
      // ToolPoi
      // 
      this.ToolPoi.Location = new System.Drawing.Point(0, 0);
      this.ToolPoi.Name = "ToolPoi";
      this.ToolPoi.Size = new System.Drawing.Size(207, 25);
      this.ToolPoi.TabIndex = 0;
      this.ToolPoi.Text = "toolStrip2";
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolSessionOpen,
            this.ToolSessionDelete});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(200, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "ToolSession";
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
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.AppWorkspace;
      this.ClientSize = new System.Drawing.Size(784, 561);
      this.Controls.Add(this.SplitLeft);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "MainForm";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.SplitLeft.Panel1.ResumeLayout(false);
      this.SplitLeft.Panel1.PerformLayout();
      this.SplitLeft.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SplitLeft)).EndInit();
      this.SplitLeft.ResumeLayout(false);
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
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.MenuStrip menuStrip1;
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
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton ToolSessionOpen;
    private System.Windows.Forms.ToolStripButton ToolSessionDelete;
  }
}

