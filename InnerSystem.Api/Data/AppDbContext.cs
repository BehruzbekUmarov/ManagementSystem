using InnerSystem.Api.Entities;
using InnerSystem.Api.Entities.Base;
using InnerSystem.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InnerSystem.Api.Data;

public class AppDbContext : DbContext
{
	public DbSet<Assignment> Assignments { get; set; }
	public DbSet<Post> Posts { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<Comment> Comments { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<BaseEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedDate = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedDate = DateTime.UtcNow;
					break;
			}
		}

		return await base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// BaseEntity configuration (no table needed unless used directly)
		//modelBuilder.Entity<BaseEntity>().UseTptMappingStrategy();

		// ----------------- Assignment -----------------
		modelBuilder.Entity<Assignment>(entity =>
		{
			entity.ToTable("Assignments");

			entity.Property(e => e.Title)
				  .IsRequired()
				  .HasMaxLength(200);

			entity.Property(e => e.Description)
				  .IsRequired();

			entity.Property(e => e.Status)
				  .HasConversion<string>()  // store enum as string
				  .IsRequired();

			entity.Property(e => e.AssignedToId).IsRequired();
			entity.Property(e => e.CreatedById).IsRequired();
		});

		// ----------------- Post -----------------
		modelBuilder.Entity<Post>(entity =>
		{
			entity.ToTable("Posts");

			entity.Property(e => e.Title)
				  .IsRequired()
				  .HasMaxLength(200);

			entity.Property(e => e.Body).IsRequired();

			entity.Property(e => e.Status)
				  .HasConversion<string>()
				  .IsRequired();

			entity.Property(e => e.AuthorId).IsRequired();

			entity.HasMany(e => e.Comments)
				  .WithOne(c => c.Post)
				  .HasForeignKey(c => c.PostId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		// ----------------- Comment -----------------
		modelBuilder.Entity<Comment>(entity =>
		{
			entity.ToTable("Comments");

			entity.Property(e => e.Content)
				  .IsRequired()
				  .HasMaxLength(1000);

			entity.Property(e => e.AuthorId).IsRequired();
			entity.Property(e => e.PostId).IsRequired();
		});

		// ----------------- Notification -----------------
		modelBuilder.Entity<Notification>(entity =>
		{
			entity.ToTable("Notifications");

			entity.Property(e => e.Title)
				  .IsRequired()
				  .HasMaxLength(200);

			entity.Property(e => e.Description).IsRequired();

			entity.Property(e => e.IsRead)
				  .HasDefaultValue(false);

			entity.Property(e => e.UserId).IsRequired();
		});
	}




}
