using DemoRollGame.Models.Configuration;
using DemoRollGame.Models.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.DbCore.Repositories.Base;
using DemoRollGame.DbCore.UoW;
using Microsoft.OpenApi.Models;
using DemoRollGame.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DemoRollGame.Service.Cron;

namespace DemoRollGame
{
    public class Startup
    {
        private readonly string DEV_CORS = "DEV_CORS";
        private readonly string VUE_DEV_SERVER_URL = "http://localhost:3000/";
        private readonly string VUE_DEV_SERVER_URL_ALT = "http://localhost:3000";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(DEV_CORS,
                builder =>
                {
                    builder.AllowCredentials();
                    builder.WithOrigins(VUE_DEV_SERVER_URL, VUE_DEV_SERVER_URL_ALT);
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            services.Configure<ConnectionSettings>(Configuration.GetSection("ConnectionStrings"));
            services.AddDbContext<demo_roll_gameContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DemoRollGame"));
            }
            );

            services.AddScoped<demo_roll_gameContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UserService>();
            services.AddScoped<MatchService>();
            services.AddScoped<PlayService>();

            services.Configure<CronServiceConfiguration>(Configuration.GetSection("CronServiceConfiguration"));
            services.Configure<MatchMakerConfiguration>(Configuration.GetSection("MatchMakerConfiguration"));

            services.AddHttpClient();
            services.AddHostedService<CronService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoRollGame", Version = "v1" });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoRollGame v1"));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(DEV_CORS);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
