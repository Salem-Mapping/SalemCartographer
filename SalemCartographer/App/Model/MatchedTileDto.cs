namespace SalemCartographer.App.Model
{
  public class MatchedTileDto : TileDto
  {
    public float? Score;

    public MatchedTileDto(TileDto dto) : base(dto) {
      if (dto is MatchedTileDto matchedTile) {
        Score = matchedTile.Score;
      }
    }
  }
}