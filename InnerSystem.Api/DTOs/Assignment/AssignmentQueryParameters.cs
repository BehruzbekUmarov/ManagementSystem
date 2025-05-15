namespace InnerSystem.Api.DTOs.Assignment;

public class AssignmentQueryParameters
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string? SearchTerm { get; set; }
	public bool SortDescending { get; set; } = false;
}
