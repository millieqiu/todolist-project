﻿namespace todoAPP.Models.ViewModel;

public class OAuthTokenViewModel
{
    public string access_token { get; set; } = null!;
    public int expires_in { get; set; }
    public string scope { get; set; } = null!;
    public string token_type { get; set; } = null!;
    public string id_token { get; set; } = null!;
}

