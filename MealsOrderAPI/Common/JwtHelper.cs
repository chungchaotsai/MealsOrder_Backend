using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using MealsOrderAPI.Extensions;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using static MealsOrderAPI.Program;

namespace MealsOrderAPI.Common
{

    public class JwtSettingsOption
    {
        public string Issuer { get; set; } = "";
        public string SignKey { get; set; } = "";
    }
    /// <summary>
    /// Ref: https://github.com/doggy8088/AspNetCore6JwtNetAuthN/blob/main/Program.cs
    /// </summary>
    public class JwtHelper {
        private readonly JwtSettingsOption settings;

        public JwtHelper(IOptions<JwtSettingsOption> settings)
        {
            this.settings = settings.Value;
        }

        public string GenerateToken(string userName,string[] roles, int expireMinutes = 3000)
        {
            var issuer = settings.Issuer;
            var signKey = settings.SignKey;

            var token = JwtBuilder.Create()
                            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                            .WithSecret(signKey)
                            // 在 RFC 7519 規格中(Section#4)，總共定義了 7 個預設的 Claims，我們應該只用的到兩種！
                            .AddClaim("jti", Guid.NewGuid().ToString()) // JWT ID
                            .AddClaim("iss", issuer)
                            // .AddClaim("nameid", userName) // User.Identity.Name
                            .AddClaim("sub", userName) // User.Identity.Name
                            .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds())
                            .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            .AddClaim("roles", roles)
                            .AddClaim(ClaimTypes.Name, userName)
                            .Encode();
            return token;
        }
    }
}
