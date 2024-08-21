namespace todoAPP.Models.DTO;

public class PatchUserTagDTO
{
    public Guid UserId { get; set; }
    public IEnumerable<PatchUserTagNameDTO> TagList { get; set; } = new List<PatchUserTagNameDTO>();
}

public class PatchUserTagNameDTO
{
    public Guid Uid { get; set; }
    public string Name { get; set; } = string.Empty;
}