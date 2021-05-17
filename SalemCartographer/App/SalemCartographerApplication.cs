using SalemCartographer.App.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalemCartographer.App
{
  class SalemCartographerApplication : ApplicationContext
  {
    public SalemCartographerApplication() : base() {
      MainForm = new UI.MainForm();
    }

  }
}
