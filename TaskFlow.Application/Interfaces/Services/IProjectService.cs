using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs.CommonDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.UpdateProjectDTOs;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Создаёт новый проект.
        /// </summary>
        /// <param name="dto">DTO с данными проекта.</param>
        Task<int?> CreateProjectAsync(CreateProjectDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Получает проект по Id.
        /// Доступно только если пользователь является участником проекта.
        /// </summary>
        /// <param name="id">Id проекта.</param>
        /// <param name="userId">Id текущего пользователя.</param>
        Task<ProjectDetailsDto?> GetProjectByIdAsync(int id, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает проект по Id без ограничений доступа (для Админа).
        /// </summary>
        /// <param name="id">Id проекта.</param>
        Task<ProjectDetailsDto?> GetProjectByIdAdminAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список проектов с фильтрацией и пагинацией.
        /// </summary>
        /// <param name="filter">Параметры фильтрации и пагинации.</param>
        /// <param name="userId">Id пользователя.</param>
        Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAsync(ProjectFilterDto filter, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список всех проектов с фильтрацией и пагинацией (для Админа).
        /// </summary>
        /// <param name="filter">Параметры фильтрации и пагинации.</param>
        Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAdminAsync(ProjectFilterDto filter, CancellationToken cancellationToken);

        /// <summary>
        /// Выполняет поиск проектов по названию.
        /// </summary>
        /// <param name="name">Строка для поиска.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        Task<PagedResponseDto<ProjectListItemDto>> GetProjectsByNameAsync(string name, int userId, int page, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Выполняет поиск проектов по названию среди всех проектов (для Админа).
        /// </summary>
        /// <param name="name">Строка для поиска.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        Task<PagedResponseDto<ProjectListItemDto>> GetProjectsByNameAdminAsync(string name, int page, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список пользователей проекта.
        /// </summary>
        /// <param name="projectId">Id проекта.</param>
        /// <param name="userId">Id пользователя.</param>
        Task<ICollection<ProjectUserListItemDto>> GetProjectUsersAsync(int projectId, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список пользователей проекта без ограничений (для Админа).
        /// </summary>
        /// <param name="projectId">Id проекта.</param>
        Task<ICollection<ProjectUserListItemDto>> GetProjectUsersAdminAsync(int projectId, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет пользователя в проект.
        /// </summary>
        /// <param name="projectId">Id проекта.</param>
        /// <param name="dto">DTO с данными пользователя и ролью.</param>
        Task<int?> AddUserToProjectAsync(int projectId, AddUserToProjectDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет пользователя из проекта.
        /// </summary>
        /// <param name="projectUserId">Id связи пользователя с проектом.</param>
        Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные проекта.
        /// Доступно только пользователям, имеющим доступ к проекту.
        /// </summary>
        /// <param name="id">Id проекта.</param>
        /// <param name="dto">DTO с новыми данными.</param>
        /// <param name="userId">Id пользователя.</param>
        Task UpdateProjectAsync(int id, UpdateProjectDto dto, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные проекта без проверки доступа (для Админа).
        /// </summary>
        /// <param name="id">Id проекта.</param>
        /// <param name="dto">DTO с новыми данными.</param>
        Task UpdateProjectAdminAsync(int id, UpdateProjectDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет проект.
        /// Доступно только пользователям, имеющим доступ к проекту.
        /// </summary>
        /// <param name="id">Id проекта.</param>
        /// <param name="userId">Id пользователя.</param>
        Task<int?> DeleteProjectAsync(int id, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет проект без проверки доступа (для Админа).
        /// </summary>
        /// <param name="id">Id проекта.</param>
        Task<int?> DeleteProjectAdminAsync(int id, CancellationToken cancellationToken);
    }
}
