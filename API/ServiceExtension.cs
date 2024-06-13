using API;
using Application.Mapping;
using Application.Services;
using Contracts;
using Domain.Entities;
using Domain.Models;
using Infrastructure.DataConnection;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<ApplicationDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options =>
                {
                    options.MigrationsAssembly("API");
                }));


        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {

            var issuer = config.GetValue<string>("ApiSettings:JwtOptions:Issuer");
            var audience = config.GetValue<string>("ApiSettings:JwtOptions:Audience");
            string TokenKey = config.GetValue<string>("ApiSettings:JwtOptions:Secret");

            services.AddIdentity<User, Role>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;


            }).AddEntityFrameworkStores<ApplicationDataContext>().AddDefaultTokenProviders();




            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
        public static IServiceCollection AddScopedServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }

        public static void ConfigureBackGroundService(this IServiceCollection services) =>
    services.AddHostedService<StatusBackgroundService>();


        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }


        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string example: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });

            });
        }

        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: builder.Configuration.GetValue<string>("Cors:AllowOrigin"), policy =>
                {
                    //policy.WithOrigins("http://example.com");
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }
    }
}
