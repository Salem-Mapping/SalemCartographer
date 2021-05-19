using System;

namespace SalemCartographer.App.Model
{
  public class StringDataEventArgs : EventArgs
  {
    public string Value;

    public StringDataEventArgs(string Value) {
      this.Value = Value;
    }
  }
}