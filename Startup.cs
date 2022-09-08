using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using petsLighthouseAPI.petsLighthouseAPI.Options;
using petsLighthouseAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petsLighthouseAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string cors = "cors";

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<petDBContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"));
                opt.EnableSensitiveDataLogging();
            });

            services.AddCors(option => option.AddPolicy(name: cors, builder =>
            {
                builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>())
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
            }));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostPetService, PostpetService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                ProfilesLocation = @"d:/DZHosts/LocalUser/erex/Protected.petslighthouse.somee.com/.aws/credentials"
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "petsLighthouseAPI", Version = "v1" });
            });

            var jwt = Configuration.GetSection("Jwt");
            services.Configure<JwtOptions>(jwt);

            var jwtOptions = jwt.Get<JwtOptions>();
            var Key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);
            services.AddAuthentication(d =>
            {
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(d =>
            {
                d.RequireHttpsMetadata = false;
                d.SaveToken = true;
                d.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "petsLighthouseAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(cors);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
