using System.Windows.Forms;

namespace SalemCartographer.App
{
  internal class SalemCartographerApplication : ApplicationContext
  {
    public SalemCartographerApplication() : base() {
      MainForm = new UI.MainForm();
    }
  }
}