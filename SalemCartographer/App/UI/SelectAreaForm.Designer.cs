
namespace SalemCartographer.App.UI
{
  partial class SelectAreaForm
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
      this.components = new System.ComponentModel.Container();
      this.LabelAreaTitle = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.TextAreaTitle = new System.Windows.Forms.TextBox();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.ButtonSubmit = new System.Windows.Forms.Button();
      this.ButtonCancel = new System.Windows.Forms.Button();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.ListSessions = new System.Windows.Forms.ListBox();
      this.Canvas = new SalemCartographer.App.UI.MapControl();
      this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // LabelAreaTitle
      // 
      this.LabelAreaTitle.AutoSize = true;
      this.LabelAreaTitle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LabelAreaTitle.Location = new System.Drawing.Point(3, 0);
      this.LabelAreaTitle.Name = "LabelAreaTitle";
      this.LabelAreaTitle.Size = new System.Drawing.Size(94, 33);
      this.LabelAreaTitle.TabIndex = 0;
      this.LabelAreaTitle.Text = "area name";
      this.LabelAreaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.LabelAreaTitle, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.TextAreaTitle, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 33);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // TextAreaTitle
      // 
      this.TextAreaTitle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TextAreaTitle.Location = new System.Drawing.Point(103, 3);
      this.TextAreaTitle.Name = "TextAreaTitle";
      this.TextAreaTitle.Size = new System.Drawing.Size(694, 27);
      this.TextAreaTitle.TabIndex = 0;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.Controls.Add(this.ButtonSubmit);
      this.flowLayoutPanel1.Controls.Add(this.ButtonCancel);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 414);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 36);
      this.flowLayoutPanel1.TabIndex = 2;
      // 
      // ButtonSubmit
      // 
      this.ButtonSubmit.AutoSize = true;
      this.ButtonSubmit.Location = new System.Drawing.Point(3, 3);
      this.ButtonSubmit.Name = "ButtonSubmit";
      this.ButtonSubmit.Size = new System.Drawing.Size(75, 30);
      this.ButtonSubmit.TabIndex = 0;
      this.ButtonSubmit.Text = "select";
      this.ButtonSubmit.UseVisualStyleBackColor = true;
      // 
      // ButtonCancel
      // 
      this.ButtonCancel.AutoSize = true;
      this.ButtonCancel.Location = new System.Drawing.Point(84, 3);
      this.ButtonCancel.Name = "ButtonCancel";
      this.ButtonCancel.Size = new System.Drawing.Size(75, 30);
      this.ButtonCancel.TabIndex = 1;
      this.ButtonCancel.Text = "cancel";
      this.ButtonCancel.UseVisualStyleBackColor = true;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(0, 33);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.ListSessions);
      this.splitContainer1.Panel1MinSize = 200;
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.Canvas);
      this.splitContainer1.Panel2MinSize = 200;
      this.splitContainer1.Size = new System.Drawing.Size(800, 381);
      this.splitContainer1.SplitterDistance = 200;
      this.splitContainer1.TabIndex = 3;
      // 
      // ListSessions
      // 
      this.ListSessions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ListSessions.FormattingEnabled = true;
      this.ListSessions.ItemHeight = 20;
      this.ListSessions.Location = new System.Drawing.Point(0, 0);
      this.ListSessions.Name = "ListSessions";
      this.ListSessions.Size = new System.Drawing.Size(200, 381);
      this.ListSessions.TabIndex = 0;
      // 
      // Canvas
      // 
      this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Canvas.Location = new System.Drawing.Point(0, 0);
      this.Canvas.Name = "Canvas";
      this.Canvas.Size = new System.Drawing.Size(596, 381);
      this.Canvas.TabIndex = 0;
      this.Canvas.Text = "mapControl1";
      // 
      // ErrorProvider
      // 
      this.ErrorProvider.ContainerControl = this;
      // 
      // SelectAreaForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "SelectAreaForm";
      this.Text = "Form1";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label LabelAreaTitle;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TextBox TextAreaTitle;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button ButtonSubmit;
    private System.Windows.Forms.Button ButtonCancel;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ListBox ListSessions;
    private MapControl Canvas;
    private System.Windows.Forms.ErrorProvider ErrorProvider;
  }
}