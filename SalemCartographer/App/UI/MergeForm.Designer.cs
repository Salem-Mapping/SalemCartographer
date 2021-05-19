
namespace SalemCartographer.App.UI
{
  partial class MergeForm
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
      this.SplitHorizontal = new System.Windows.Forms.SplitContainer();
      this.SplitVertical = new System.Windows.Forms.SplitContainer();
      this.ButtonMerge = new System.Windows.Forms.Button();
      this.PanelBottom = new System.Windows.Forms.FlowLayoutPanel();
      this.ButtonCancel = new System.Windows.Forms.Button();
      this.PanelTop = new System.Windows.Forms.TableLayoutPanel();
      this.LabelSelection = new System.Windows.Forms.Label();
      this.ComboBoxAreas = new System.Windows.Forms.ComboBox();
      this.CanvasOther = new SalemCartographer.App.UI.MapControl();
      this.CanvasThis = new SalemCartographer.App.UI.MapControl();
      this.CanvasMerge = new SalemCartographer.App.UI.MapControl();
      ((System.ComponentModel.ISupportInitialize)(this.SplitHorizontal)).BeginInit();
      this.SplitHorizontal.Panel1.SuspendLayout();
      this.SplitHorizontal.Panel2.SuspendLayout();
      this.SplitHorizontal.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SplitVertical)).BeginInit();
      this.SplitVertical.Panel1.SuspendLayout();
      this.SplitVertical.Panel2.SuspendLayout();
      this.SplitVertical.SuspendLayout();
      this.PanelBottom.SuspendLayout();
      this.PanelTop.SuspendLayout();
      this.SuspendLayout();
      // 
      // SplitHorizontal
      // 
      this.SplitHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitHorizontal.Location = new System.Drawing.Point(0, 0);
      this.SplitHorizontal.Name = "SplitHorizontal";
      this.SplitHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // SplitHorizontal.Panel1
      // 
      this.SplitHorizontal.Panel1.Controls.Add(this.SplitVertical);
      // 
      // SplitHorizontal.Panel2
      // 
      this.SplitHorizontal.Panel2.Controls.Add(this.CanvasMerge);
      this.SplitHorizontal.Size = new System.Drawing.Size(1155, 549);
      this.SplitHorizontal.SplitterDistance = 274;
      this.SplitHorizontal.TabIndex = 0;
      // 
      // SplitVertical
      // 
      this.SplitVertical.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitVertical.Location = new System.Drawing.Point(0, 0);
      this.SplitVertical.Name = "SplitVertical";
      // 
      // SplitVertical.Panel1
      // 
      this.SplitVertical.Panel1.Controls.Add(this.CanvasThis);
      // 
      // SplitVertical.Panel2
      // 
      this.SplitVertical.Panel2.Controls.Add(this.CanvasOther);
      this.SplitVertical.Size = new System.Drawing.Size(1155, 274);
      this.SplitVertical.SplitterDistance = 577;
      this.SplitVertical.TabIndex = 0;
      // 
      // ButtonMerge
      // 
      this.ButtonMerge.AutoSize = true;
      this.ButtonMerge.Location = new System.Drawing.Point(3, 3);
      this.ButtonMerge.Name = "ButtonMerge";
      this.ButtonMerge.Size = new System.Drawing.Size(75, 30);
      this.ButtonMerge.TabIndex = 0;
      this.ButtonMerge.Text = "Merge";
      this.ButtonMerge.UseVisualStyleBackColor = true;
      // 
      // PanelBottom
      // 
      this.PanelBottom.AutoSize = true;
      this.PanelBottom.Controls.Add(this.ButtonMerge);
      this.PanelBottom.Controls.Add(this.ButtonCancel);
      this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.PanelBottom.Location = new System.Drawing.Point(0, 513);
      this.PanelBottom.Name = "PanelBottom";
      this.PanelBottom.Size = new System.Drawing.Size(1155, 36);
      this.PanelBottom.TabIndex = 1;
      // 
      // ButtonCancel
      // 
      this.ButtonCancel.AutoSize = true;
      this.ButtonCancel.Location = new System.Drawing.Point(84, 3);
      this.ButtonCancel.Name = "ButtonCancel";
      this.ButtonCancel.Size = new System.Drawing.Size(75, 30);
      this.ButtonCancel.TabIndex = 1;
      this.ButtonCancel.Text = "Cancel";
      this.ButtonCancel.UseVisualStyleBackColor = true;
      // 
      // PanelTop
      // 
      this.PanelTop.AutoSize = true;
      this.PanelTop.ColumnCount = 2;
      this.PanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
      this.PanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86F));
      this.PanelTop.Controls.Add(this.LabelSelection, 0, 0);
      this.PanelTop.Controls.Add(this.ComboBoxAreas, 1, 0);
      this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.PanelTop.Location = new System.Drawing.Point(0, 0);
      this.PanelTop.Name = "PanelTop";
      this.PanelTop.RowCount = 1;
      this.PanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.PanelTop.Size = new System.Drawing.Size(1155, 34);
      this.PanelTop.TabIndex = 2;
      // 
      // LabelSelection
      // 
      this.LabelSelection.AutoSize = true;
      this.LabelSelection.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LabelSelection.Location = new System.Drawing.Point(3, 0);
      this.LabelSelection.Name = "LabelSelection";
      this.LabelSelection.Size = new System.Drawing.Size(155, 34);
      this.LabelSelection.TabIndex = 0;
      this.LabelSelection.Text = "merge with";
      this.LabelSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CheckboxAreas
      // 
      this.ComboBoxAreas.FormattingEnabled = true;
      this.ComboBoxAreas.Location = new System.Drawing.Point(164, 3);
      this.ComboBoxAreas.Name = "CheckboxAreas";
      this.ComboBoxAreas.Size = new System.Drawing.Size(304, 28);
      this.ComboBoxAreas.TabIndex = 1;
      // 
      // CanvasOther
      // 
      this.CanvasOther.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CanvasOther.Location = new System.Drawing.Point(0, 0);
      this.CanvasOther.Name = "CanvasOther";
      this.CanvasOther.Size = new System.Drawing.Size(574, 274);
      this.CanvasOther.TabIndex = 0;
      // 
      // CanvasThis
      // 
      this.CanvasThis.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CanvasThis.Location = new System.Drawing.Point(0, 0);
      this.CanvasThis.Name = "CanvasThis";
      this.CanvasThis.Size = new System.Drawing.Size(577, 274);
      this.CanvasThis.TabIndex = 0;
      // 
      // CanvasMerge
      // 
      this.CanvasMerge.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CanvasMerge.Location = new System.Drawing.Point(0, 0);
      this.CanvasMerge.Name = "CanvasMerge";
      this.CanvasMerge.Size = new System.Drawing.Size(1155, 271);
      this.CanvasMerge.TabIndex = 0;
      // 
      // MergeForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1155, 549);
      this.Controls.Add(this.PanelTop);
      this.Controls.Add(this.PanelBottom);
      this.Controls.Add(this.SplitHorizontal);
      this.Name = "MergeForm";
      this.Text = "MergeForm";
      this.SplitHorizontal.Panel1.ResumeLayout(false);
      this.SplitHorizontal.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SplitHorizontal)).EndInit();
      this.SplitHorizontal.ResumeLayout(false);
      this.SplitVertical.Panel1.ResumeLayout(false);
      this.SplitVertical.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SplitVertical)).EndInit();
      this.SplitVertical.ResumeLayout(false);
      this.PanelBottom.ResumeLayout(false);
      this.PanelBottom.PerformLayout();
      this.PanelTop.ResumeLayout(false);
      this.PanelTop.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.SplitContainer SplitHorizontal;
    private System.Windows.Forms.SplitContainer SplitVertical;
    private System.Windows.Forms.Button ButtonMerge;
    private System.Windows.Forms.FlowLayoutPanel PanelBottom;
    private System.Windows.Forms.Button ButtonCancel;
    private System.Windows.Forms.TableLayoutPanel PanelTop;
    private System.Windows.Forms.Label LabelSelection;
    private System.Windows.Forms.ComboBox ComboBoxAreas;
    private MapControl CanvasThis;
    private MapControl CanvasOther;
    private MapControl CanvasMerge;
  }
}