using System;

namespace SalemCartographer.App.Model
{
  internal class DataEventArgs<T> : EventArgs
  {
    public T Value;

    public DataEventArgs(T value) {
      Value = value;
    }
  }
}