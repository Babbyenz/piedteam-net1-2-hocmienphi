namespace piedteam_net1_2_hocmienphi.service.Utils.JwtService;

public class JwtOptions
{
    public string Issuer { get; set; } // nguoi ky 
    public string Audience { get; set; } //nguoi nhan token
    public string SecretKey { get; set; } //key de tao ra chuoi token 
    public int ExpirationMinutes { get; set; } //han token 
}