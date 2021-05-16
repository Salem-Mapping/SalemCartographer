using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App.Model
{
  class StringDataEventArgs : EventArgs
  {
    public string Value;

    public StringDataEventArgs(string Value) {
      this.Value = Value;
    }
  }
}
