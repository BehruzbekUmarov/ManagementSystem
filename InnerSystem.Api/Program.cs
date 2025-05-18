using InnerSystem.Api.Data;
using InnerSystem.Api.Helper;
using InnerSystem.Api.Mapping;
using InnerSystem.Api.Mapping.MappingAssignment;
using InnerSystem.Api.Mapping.MappingComment;
using InnerSystem.Api.Mapping.MappingNotification;
using InnerSystem.Api.Mapping.MappingPost;
using InnerSystem.Api.Repositories;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity;
using InnerSystem.Identity.AccessConfigurations;
using InnerSystem.Identity.Middleware;
using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc().AddJsonOptions(_ => _.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<AccessConfiguration>(builder.Configuration.GetSection("AccessConfiguration"));

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Management System Api", Version = "v1" });
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
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
			new string[]{}
		}
	});

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration.GetConnectionString("ManagementSystemDb");
builder.Services.AddDbContext<AppDbContext>(options 
	=> options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAssignmentMapper, AssignmentMapper>();
builder.Services.AddScoped<ICommentMapper, CommentMapper>();
builder.Services.AddScoped<IPostMapper, PostMapper>();
builder.Services.AddScoped<IPostImageSaveHelper, PostImageSaveHelper>();
builder.Services.AddScoped<INotificationMapper, NotificationMapper>();
builder.Services.RegisterIdentityModule(builder.Configuration);

//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowFrontend", policy =>
//	{
//		policy.WithOrigins("http://localhost:3000", "https://imsbackend.com") // update with real URLs
//			  .AllowAnyHeader()
//			  .AllowAnyMethod();
//	});
//});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var roleManager = services.GetRequiredService<RoleManager<Role>>();
	var userManager = services.GetRequiredService<UserManager<User>>();

	await RoleInitializer.SeedRolesAsync(roleManager);
	await RoleInitializer.SeedAdminAsync(userManager);
}

//app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
