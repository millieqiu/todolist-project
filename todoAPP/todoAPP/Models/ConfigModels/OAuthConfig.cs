namespace todoAPP.Models.ConfigModels;

public class OAuthConfig
{
    public const string SectionName = "OAuth";
	public OAuthGoogle Google { get; set; } = null!;
}

public class OAuthGoogle
{
	public string ClientId { get; set; } = string.Empty;
	public string ClientSecret { get; set; } = string.Empty;
}