using InnerSystem.Identity.Abstract;
using InnerSystem.Identity.AccessConfigurations;
using InnerSystem.Identity.ClaimsPrincipalFactory;
using InnerSystem.Identity.Email;
using InnerSystem.Identity.Models;
using InnerSystem.Identity.Services.Auth;
using InnerSystem.Identity.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnerSystem.Identity;

public static class IdentityModule
{
	public static IServiceCollection RegisterIdentityModule(this IServiceCollection services, IConfiguration configuration)
	{
		//var connectionString = configuration.GetConnectionString("ManagementSystemDb");
		//services.AddDbContext<ManagementSIdentityDbContext>(options => options.UseNpgsql(connectionString));
		var connectionString = GetConnectionString();
		services.AddDbContext<ManagementSIdentityDbContext>(options =>
	    options.UseSqlServer(connectionString));

		string GetConnectionString()
		{
			// Check if we're on Render (DATABASE_URL will be set in the environment)
			var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
			if (!string.IsNullOrEmpty(databaseUrl))
			{
				var uri = new Uri(databaseUrl);
				var userInfo = uri.UserInfo.Split(':');
				return $"Host={uri.Host};Port={uri.Port};Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')};SSL Mode=Require;Trust Server Certificate=true";
			}

			// Fall back to appsettings.json for local dev
			return configuration.GetConnectionString("ManagementSystemDb");
		}

		services.AddIdentity<User, Role>(option =>
		{
			option.Password.RequiredLength = 8;
			option.Password.RequireNonAlphanumeric = false;
			option.Password.RequireLowercase = true;
			option.Password.RequireUppercase = false;
			option.Password.RequireDigit = true;
			option.SignIn.RequireConfirmedPhoneNumber = true;
		}).AddRoles<Role>()
		  .AddUserManager<UserManager<User>>()
		  .AddRoleManager<RoleManager<Role>>()
		  .AddEntityFrameworkStores<ManagementSIdentityDbContext>()
		  .AddClaimsPrincipalFactory<InnerSystemClaimsPrincipalFactory>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.SaveToken = true;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidAudience = configuration["AccessConfiguration:Audience"],
				ValidIssuer = configuration["AccessConfiguration:Issuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.TheSecretKey))
			};
		});

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IMappingService, MappingService>();
		services.AddScoped<IEnvironmentAccessor, EnvironmentAccessor>();

		services.Configure<EmailClientOptions>(
	    configuration.GetSection(EmailClientOptions.EmailSectionName));
		services.AddScoped<IEmailSender, EmailSender>();

		services.AddAutoMapper(typeof(MappingProfile));


		return services;
	}

	
}
