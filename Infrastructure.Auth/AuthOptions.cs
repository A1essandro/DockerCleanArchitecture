using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public class AuthOptions
{

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int Lifetime { get; set; } = 900;

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
    }

}