using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  interface IProcessor<DTO>
  {
    void SetPath(string Path);
    bool IsValid();
    DTO GetDto();
  }
}
