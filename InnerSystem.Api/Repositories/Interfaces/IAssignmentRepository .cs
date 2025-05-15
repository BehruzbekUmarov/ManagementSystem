using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Repositories.Interfaces;

public interface IAssignmentRepository : IGenericRepository<Assignment>
{
	Task<IEnumerable<Assignment>> GetAssignmentsByUserAsync(Guid userId);
	Task<IEnumerable<Assignment>> GetAssignmentsWithStatusAsync(InnerSystem.Api.Enums.TaskStatus status);
	Task<IEnumerable<Assignment>> GetAllPaginatedAsync(AssignmentQueryParameters parameters);
}
