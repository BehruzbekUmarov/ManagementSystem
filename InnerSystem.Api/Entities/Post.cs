using InnerSystem.Api.Entities.Base;
using InnerSystem.Api.Enums;
using InnerSystem.Identity.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnerSystem.Api.Entities;

public class Post : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
    public string? Image { get; set; }
	public PostStatus Status { get; set; } = PostStatus.Draft;

	public Guid AuthorId { get; set; }

	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
