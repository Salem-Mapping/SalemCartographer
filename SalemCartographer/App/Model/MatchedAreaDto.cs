using System.Drawing;

namespace SalemCartographer.App.Model
{
  public class MatchedAreaDto : AreaDto
  {
    public Point Offset;
    public float Score;
    public float ScoreNormalized;

    public MatchedAreaDto(AreaDto dto) : base(dto) {
      if (dto is MatchedAreaDto matched) {
        Score = matched.Score;
        ScoreNormalized = matched.ScoreNormalized;
      }
    }

    public void AddTile(TileDto dto, float? score) {
      MatchedTileDto matchDto = (dto is not MatchedTileDto) ? new(dto) : (MatchedTileDto)dto;
      matchDto.Score = score;
      base.AddTile(matchDto);
    }

    public override void AddTile(TileDto dto) {
      base.AddTile((dto is not MatchedTileDto) ? new(dto) : (MatchedTileDto)dto);
    }

    public override bool RemoveTile(TileDto dto) {
      bool removedBase = base.RemoveTile(dto);
      bool removed = Tiles.Remove(dto.GetKey());
      return removedBase || removed;
    }
  }
}