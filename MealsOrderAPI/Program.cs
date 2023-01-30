
using MealsOrderAPI.Common;
using MealsOrderAPI.Context;
using MealsOrderAPI.Logger;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository;
using MealsOrderAPI.Repository.Interface;
using MealsOrderAPI.Settings;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Json;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace MealsOrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Json("appsettings.json")
                   .CreateLogger();
                Log.Information("Starting web host");

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.Configure<JwtSettingsOption>(builder.Configuration.GetSection("JwtSettings"));
                builder.Services.AddOptions<JwtSettingsOption>("JwtSettings");
                builder.Services.AddSingleton<JwtHelper>();
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

                        // 通常不太需要驗證 Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true,

                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" 應該從 IConfiguration 取得
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
                    };
                });

                builder.Services.AddAuthorization();



                // https://blog.miniasp.com/post/2021/11/29/How-to-use-Serilog-with-NET-6
                // Serilog setting https://serilog.net/


                // Hide connection info in secrets.json
                var DatabaseSettings =
                    builder.Configuration.GetSection("MealsOrder").Get<DatabaseSettings>();
                // https://learn.microsoft.com/zh-tw/ef/core/dbcontext-configuration/
                builder.Services.AddDbContext<MealsOrderContext>(x => x.UseSqlServer(DatabaseSettings.ConnectionString));
                builder.Services.AddScoped<IUsersRepository, UserRepository>();
                // Add services to the container.
                builder.Services.AddControllers()
                    .AddOData(opt => opt
                        .Select()
                        .Filter()
                        .OrderBy()
                        .Count()
                        .SetMaxTop(100)
                        .AddRouteComponents("odata", GetEdmModel()).Filter().Select().Expand());
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MealsOrderAPI", Version = "v1" });
                });
                builder.Host.UseSerilog();
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                builder.Services.AddHttpContextAccessor();
                builder.Services.AddTransient<ClaimsPrincipal>(s =>
    s.GetService<IHttpContextAccessor>().HttpContext.User);

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                // 一勞永逸的將每個 Request 都預設加上一些屬性
                //app.UseSerilogRequestLogging(options =>
                //{
                //    // 如果要自訂訊息的範本格式，可以修改這裡，但修改後並不會影響結構化記錄的屬性
                //    options.MessageTemplate = "Handled {RequestPath}";

                //    // 預設輸出的紀錄等級為 Information，你可以在此修改記錄等級
                //    // options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                //    // 你可以從 httpContext 取得 HttpContext 下所有可以取得的資訊！
                //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                //    {
                //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                //        diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
                //    };
                //});
                app.UseHttpsRedirection();

                app.UseAuthorization();
                app.UseAuthentication();

                app.MapControllers();
                app.UseSerilogRequestLogging();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            //odataBuilder.EnableLowerCamelCase(); //CamelEntityNaming

            //odataBuilder.EntitySet<User>("Users");
            odataBuilder.EntitySet<UserDto>("Users");

            return odataBuilder.GetEdmModel();
        }


    }
}