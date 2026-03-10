using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<int?> AddProject(ProjectEntity project);
    Task<ProjectEntity?> GetProjectById(int id);
    Task<ICollection<ProjectEntity>?> GetAllProjects();
    Task<ICollection<ProjectEntity>?> GetProjectsByUserId(int userId);
    Task<int?> UpdateProject(int id, ProjectEntity project);
    Task<int?> DeleteProjectById(int id);
}
