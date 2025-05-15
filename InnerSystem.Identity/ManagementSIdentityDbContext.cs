using InnerSystem.Identity.Constants;
using InnerSystem.Identity.Email;
using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Identity;

public class ManagementSIdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ManagementSIdentityDbContext(DbContextOptions<ManagementSIdentityDbContext> options) : base(options)
    {
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
	}

    public DbSet<EmailToken> EmailTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Role>(x =>
		{
			x.HasData(new Role(RoleNames.User));
			x.HasData(new Role(RoleNames.Admin));
			x.HasData(new Role(RoleNames.Manager));
		});

		base.OnModelCreating(builder);
	}
}
