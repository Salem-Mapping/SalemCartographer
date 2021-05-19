namespace SalemCartographer.App
{
  internal interface IProcessor<DTO>
  {
    void SetPath(string Path);

    bool IsValid();

    DTO GetDto();
  }
}