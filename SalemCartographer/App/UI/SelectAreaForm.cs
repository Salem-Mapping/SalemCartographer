using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalemCartographer.App.UI
{
  public partial class SelectAreaForm : Form
  {
    public AreaDto Selected;
    public string AreaName;

    public SelectAreaForm(List<AreaDto> DataSource) {
      InitializeComponent();
      Initialize();
      ListSessions.DataSource = DataSource;
    }

    protected void OnSelect(object sender, EventArgs e) {
      Canvas.SetArea((AreaDto)ListSessions.SelectedItem);
    }

    protected void OnSubmit(object sender, EventArgs e) {
      if (TextAreaTitle.TextLength <= 0) {
        // LabelAreaTitle TextAreaTitle
        ErrorProvider.SetIconAlignment(TextAreaTitle, ErrorIconAlignment.MiddleLeft);
        ErrorProvider.SetError(TextAreaTitle, "must not be empty!");
        return;
      }
      AreaName = TextAreaTitle.Text;
      Selected = (AreaDto)ListSessions.SelectedItem;
      DialogResult = DialogResult.OK;
      Close();
    }

    protected void OnCancel(object sender, EventArgs e) {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    protected void Initialize() {
      ListSessions.DisplayMember = AreaDto.FIELD_TITLE;
      ListSessions.SelectedValueChanged += OnSelect;
      ButtonSubmit.Click += OnSubmit;
      ButtonCancel.Click += OnCancel;
    }
  }
}