using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;

namespace SalemCartographer.App.Model
{
  public class WorldDto : AbstractModel
  {
    // serialize
    [JsonInclude]
    public SortedDictionary<string, AreaDto> Areas { get; set; }

    // transient
    [JsonIgnore]
    public List<AreaDto> AreaList => Areas.Values.ToList();

    public WorldDto() {
      Areas = new();
    }

    public WorldDto(WorldDto dto) : this() {
      foreach (var old in dto.Areas) {
        Areas[old.Key] = new(old.Value);
      }
    }

    public void AddArea(AreaDto NewArea) {
      if (Areas.ContainsKey(NewArea.DisplayString)) {
        Areas.Remove(NewArea.DisplayString);
      }
      Areas.Add(NewArea.DisplayString, NewArea);
    }
  }
}