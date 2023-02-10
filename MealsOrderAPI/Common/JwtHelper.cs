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
        public int ExpirationTimeInSeconds { get; set; } = 86400;
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

        public string GenerateToken(string userName,int userId,string[] roles)
        {
            /**
             *  iss (Issuer) - jwt簽發者
                sub (Subject) - jwt所面向的用戶
                aud (Audience) - 接收jwt的一方
                exp (Expiration Time) - jwt的過期時間，這個過期時間必須要大於簽發時間
                nbf (Not Before) - 定義在什麼時間之前，該jwt都是不可用的
                iat (Issued At) - jwt的簽發時間
                jti (JWT ID) - jwt的唯一身份標識，主要用來作為一次性token,從而迴避重放攻擊
             */
            var issuer = settings.Issuer;
            var signKey = settings.SignKey;
            var expireTime = settings.ExpirationTimeInSeconds;
            var token = JwtBuilder.Create()
                            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                            .WithSecret(signKey)
                            // 在 RFC 7519 規格中(Section#4)，總共定義了 7 個預設的 Claims，我們應該只用的到兩種！
                            .AddClaim("jti", Guid.NewGuid().ToString()) // JWT ID
                            .AddClaim("iss", issuer)
                            .AddClaim("userId", userId) // User.Identity.Name
                            .AddClaim("sub", userName) // User.Identity.Name
                                                       //.AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(Convert.ToDouble(expireTime)))
                                                       //.AddClaim("exp", DateTime.Now.AddDays(1))
                            .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(90).ToUnixTimeSeconds())
                            .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            .AddClaim("roles", roles)
                            .AddClaim(ClaimTypes.SerialNumber, 135)
                            .AddClaim(ClaimTypes.Name, userName)
                            .Encode();
            return token;
        }
    }
}
