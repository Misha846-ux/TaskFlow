using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ComapniesDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.CommonDTOs;

namespace TaskFlow.Application.Interfaces.Services;

public interface ICompanyService
{
    /// <summary>
    /// Возвращает все компании
    /// </summary>
    Task<ICollection<CompanyGetDto>?> GetAllCompaniesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает компании порционно 
    /// </summary>
    /// <param name="page">Номер страницы </param>
    /// <param name="pageSize">Размер страницы</param>
    Task<ICollection<CompanyGetDto>?> GetCompaniesPaginationAsync(int count, int side, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает список компаний, к которым принадлежит пользователь 
    /// </summary>
    Task<ICollection<CompanyGetDto>?> GetUsersCompaniesAsync(int userId, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает компании пользователя порционно 
    /// </summary>
    /// <param name="userId">Id текущего пользователя.</param>
    /// <param name="side">Номер страницы</param>
    /// <param name="count">Размер страницы</param>
    Task<ICollection<CompanyGetDto>?> GetUsersCompaniesPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);

    /// <summary>
    /// Получить информацию о компании по Id
    /// </summary>
    Task<CompanyGetDto?> GetCompanyByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить информацию о компании по Id для пользователя (доступно только если пользователь связан с компанией)
    /// </summary>
    Task<CompanyGetDto?> GetUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт новую компанию. Создающий пользователь автоматически становится владельцем/администратором.
    /// </summary>
    /// <param name="dto">DTO с данными компании.</param>
    /// <param name="ownerUserId">Id пользователя, создающего компанию.</param>
    Task<int?> CreateCompanyAsync(CompanyPostDto dto, int ownerUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет компанию по Id (для Admin — без проверки прав).
    /// </summary>
    Task<int?> DeleteCompanyByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет компанию по Id (только если пользователь — владелец/имеет право).
    /// </summary>
    Task<int?> DeleteUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные компании (для Admin — без проверки прав).
    /// </summary>
    Task<int?> UpdateCompanyByIdAsync(int id, CompanyUpdateDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные компании (только для пользователей, имеющих право в этой компании).
    /// </summary>
    Task UpdateUsersCompanyByIdAsync(int id, CompanyUpdateDto dto, int userId, CancellationToken cancellationToken);
}
