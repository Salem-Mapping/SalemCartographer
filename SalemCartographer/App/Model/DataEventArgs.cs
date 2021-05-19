using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App.Model
{
  class DataEventArgs<T> : EventArgs
  {
    public T Value;

    public DataEventArgs(T value) {
      Value = value;
    }

  }
}
