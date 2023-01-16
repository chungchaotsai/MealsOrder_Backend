
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
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwt(options =>
                {
                    options.Keys = new string[] { builder.Configuration.GetValue<string>("JwtSettings:SignKey") };
                    options.VerifySignature = true;
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
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                // �@�ҥöh���N�C�� Request ���w�]�[�W�@���ݩ�
                //app.UseSerilogRequestLogging(options =>
                //{
                //    // �p�G�n�ۭq�T�����d���榡�A�i�H�ק�o�̡A���ק��ä��|�v�T���c�ưO�����ݩ�
                //    options.MessageTemplate = "Handled {RequestPath}";

                //    // �w�]��X���������Ŭ� Information�A�A�i�H�b���ק�O������
                //    // options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                //    // �A�i�H�q httpContext ���o HttpContext �U�Ҧ��i�H���o����T�I
                //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                //    {
                //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                //        diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
                //    };
                //});
                app.UseHttpsRedirection();

                app.UseAuthorization();

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