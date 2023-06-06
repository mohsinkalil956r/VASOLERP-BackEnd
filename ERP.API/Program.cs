using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ERP.API.Auth;
using ERP.API.Infrastructure;
using ERP.DAL.DB;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories;
using ERP.DAL.Repositories.Abstraction;
using System.Security.Claims;
using System.Text;

namespace ERP.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ERPContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ERPContext")));

        builder.Services.AddIdentity<SystemUser, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<ERPContext>();

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            options.Events = new JwtBearerEvents();
            options.Events.OnTokenValidated = async ctx =>
            {
                var username = ctx.Principal.GetUsername();

                var db = ctx.HttpContext.RequestServices.GetRequiredService<ERPContext>();

                var permissons = await db.Users.Where(u => u.UserName == username).Select(u => u.Permissions).FirstOrDefaultAsync();
                var claims = permissons?.Select(p => new Claim(p.Name, "")) ?? new List<Claim>();

                ctx.Principal.AddIdentity(new ClaimsIdentity(claims));
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("", policy =>
            {
                policy.RequireAssertion(ctx =>
                {
                    return ctx.User.IsInRole("SystemUser") && ctx.User.HasClaim(c => c.Type == "") || ctx.User.IsInRole("Admin");
                });
            });
        });


        builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton<IJWTManager, JWTManager>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IEmployeeContactRepository, EmployeeContactRepository>();

        builder.Services.AddAutoMapper(typeof(MapperProfile));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP APIs", Version = "v1" });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{ }
                }
            });
        });

        var app = builder.Build();

        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });

        app.UseStaticFiles(new StaticFileOptions()
        {
            OnPrepareResponse = async (ctx) =>
            {
                if (!ctx.Context.User.Identity.IsAuthenticated)
                {
                    ctx.Context.Response.StatusCode = 401;
                    return;
                }
            }
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
