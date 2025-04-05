using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features;
public class JwtOptions
{
    public string Key { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int AccessTokenExpiryMinutes { get; set; }

    public int RefreshTokenExpiryDays { get; set; }
}
