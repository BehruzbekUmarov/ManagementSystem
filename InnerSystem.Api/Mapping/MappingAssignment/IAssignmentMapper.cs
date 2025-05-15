using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingAssignment;

public interface IAssignmentMapper
{
	Assignment MapToEntity(CreateAssignmentDto dto);
	AssignmentDto MapToDto(Assignment entity);
	void MapToEntity(UpdateAssignmentDto dto, Assignment entity);
}
