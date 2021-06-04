using System;

namespace SalemCartographer.App.Model
{
  public class DataEventArgs<T> : EventArgs
  {
    public T Value;

    public DataEventArgs(T value) {
      Value = value;
    }
  }
}