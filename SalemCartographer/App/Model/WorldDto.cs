using System.Collections.Generic;
using System.Linq;

namespace SalemCartographer.App.Model
{
  public class WorldDto
  {
    public Dictionary<string, AreaDto> Areas { get; set; }

    public List<AreaDto> AreaList {
      get {
        return Areas.Values.ToList();
      }
    }

    public WorldDto() {
      Areas = new();
    }

    public void SetAreas(List<AreaDto> NewAreas) {
      Areas = NewAreas.Distinct().ToDictionary(x => x.DisplayString, x => x);
    }

    public void AddArea(AreaDto NewArea) {
      if (Areas.ContainsKey(NewArea.DisplayString)) {
        Areas.Remove(NewArea.DisplayString);
      }
      Areas.Add(NewArea.DisplayString, NewArea);
    }
  }
}