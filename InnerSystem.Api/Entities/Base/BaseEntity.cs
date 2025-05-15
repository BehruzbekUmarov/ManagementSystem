namespace InnerSystem.Api.Entities.Base;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedDate { get; set; }
}
