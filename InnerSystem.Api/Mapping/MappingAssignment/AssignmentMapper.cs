using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;
using InnerSystem.Identity.Abstract;

namespace InnerSystem.Api.Mapping.MappingAssignment;

public class AssignmentMapper(IEnvironmentAccessor _environmentAccessor) : IAssignmentMapper
{
	public void MapToEntity(UpdateAssignmentDto dto, Assignment entity)
	{
		entity.Title = dto.Title;
		entity.Description = dto.Description;
		entity.Status = dto.Status;
		entity.AssignedToId = dto.AssignedToId;
	}
	public AssignmentDto MapToDto(Assignment entity)
	{
		return new AssignmentDto
		{
			Id = entity.Id,
			Title = entity.Title,
			Description = entity.Description,
			Status = entity.Status,
			AssignedToId = entity.AssignedToId,
			CreatedById = entity.CreatedById,
			IsDeleted = entity.IsDeleted,
			CreatedDate = entity.CreatedDate,
			UpdatedDate = entity.UpdatedDate
		};
	}

	public Assignment MapToEntity(CreateAssignmentDto dto)
	{
		if (dto == null) throw new ArgumentNullException(nameof(dto));
		return new Assignment
		{
			Title = dto.Title,
			Description = dto.Description,
			AssignedToId = dto.AssignedToId,
			Status = Enums.TaskStatus.Pending, 
		};
	}

	public List<AssignmentDto> MapToDtoList(IEnumerable<Assignment> comments)
	{
		return comments.Select(MapToDto).ToList();
	}

}
