namespace todoAPP.Models.ViewModel;

public class OAuthUserinfoViewModel
{
    public string id { get; set; } = null!;
    public string email { get; set; } = null!;
    public bool verified_email { get; set; }
    public string name { get; set; } = null!;
    public string picture { get; set; } = null!;
}

