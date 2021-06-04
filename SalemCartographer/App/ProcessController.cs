using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  public class ProcessController : AbstractController
  {
    private static ProcessController _Instance;
    private static readonly object _IntanceLock = new();
    public static ProcessController Instance {
      get {
        lock (_IntanceLock) {
          if (_Instance == null) {
            _Instance = new ProcessController();
          }
        }
        return _Instance;
      }
    }

    public List<ProcessDto> ActiveProcesses => activeProcesses;
    private readonly List<ProcessDto> activeProcesses;

    private ProcessController() : base() {
      activeProcesses = new();
    }


  }
}
