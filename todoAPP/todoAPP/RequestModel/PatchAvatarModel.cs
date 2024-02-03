namespace todoAPP.RequestModel
{
  public class PatchAvatarModel
  {
    public Guid UserId { get; set; }
    public IFormFile File { get; set; } = null!;
  }
}