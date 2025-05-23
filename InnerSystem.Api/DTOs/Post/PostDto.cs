﻿using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Enums;
using System.Text.Json.Serialization;

namespace InnerSystem.Api.DTOs.Post;

public class PostDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public string? Image { get; set; }
	public PostStatus Status { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<CommentForPostDto> Comments { get; set; } = new();

	public Guid AuthorId { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}
