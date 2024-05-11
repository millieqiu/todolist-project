namespace todoAPP.Models;

public class GetUserTagListDTO
{
    public Guid UserId { get; set; }
}

public class PatchUserTagNameDTO
{
    public Guid UserTagId { get; set; }
    public string Name { get; set; } = string.Empty;
}