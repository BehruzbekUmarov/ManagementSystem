namespace InnerSystem.Identity.DTOs;

public class RoleDto
{
	public RoleDto(string name)
	{
		Name = name;
	}
	public string Name { get; set; }
}
