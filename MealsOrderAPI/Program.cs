
using MealsOrderAPI.Context;
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
using System.Data;

namespace MealsOrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Hide connection info in secrets.json
            var DatabaseSettings =
                builder.Configuration.GetSection("MealsOrder").Get<DatabaseSettings>();
            // https://learn.microsoft.com/zh-tw/ef/core/dbcontext-configuration/
            builder.Services.AddDbContext<MealsOrderContext>(x => x.UseSqlServer(DatabaseSettings.ConnectionString));
            builder.Services.AddScoped<IUsersRepository, UserRepository>();
            // Add services to the container.

            builder.Services.AddControllers()
                .AddOData(opt => opt
                .AddRouteComponents("v1", GetEdmModel())
                .Filter()
                .Select()
                .SetMaxTop(20)
                .OrderBy()
                .Expand());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MealsOrderAPI", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<User>("Users");
            return builder.GetEdmModel();
        }
    }
}