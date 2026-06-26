namespace piedteam_net1_2_hocmienphi.service.Utils.JwtService;

public class JwtOptions
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string ExpirationMinutes { get; set; }
}