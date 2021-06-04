namespace SalemCartographer.App
{
  public interface IProcessor<DTO>
  {
    void SetPath(string path);

    void SetDto(DTO dto);

    bool IsValid();

    DTO GetDto();
  }
}