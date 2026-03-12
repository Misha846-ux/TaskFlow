using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<ICollection<CompanyEntity>?> GetAllCompaniesAsync(CancellationToken cancellationToken);
    Task<ICollection<CompanyEntity>?> GetCompaniesPaginationAsync(int count, int side, CancellationToken cancellationToken);
    Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdAsync(int userId, CancellationToken cancellationToken); // We take userId from Access or Refresh token and find companies by it. I think it is better than by email because email can be changed but userId is unchangeable
    Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);
    Task<CompanyEntity?> GetCompanyByIdAsync(int id, CancellationToken cancellationToken);
    Task<int?> AddCompanyAsync(CompanyEntity company, int ownerId, CancellationToken cancellationToken);
    Task<int?> DeleteCompanyByIdAsync(int id, CancellationToken cancellationToken);


    Task<ICollection<CompanyUserEntity>> GetAllCompanyEmploersAsync(int companyId, CancellationToken cancellationToken);
    Task<ICollection<CompanyUserEntity>> GetCompanyEmploersByRoleAsync(int companyId, CompanyRole role, CancellationToken cancellationToken);
    Task<CompanyUserEntity> GetCompanyUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<int?> AddEmployeeToCompanyAsync(CompanyUserEntity companyUserEntity, CancellationToken cancellationToken);
    Task<int?> DeleteEmployeeByIdAsync(int id, CancellationToken cancellationToken);
    Task UpdateChangesAsync(CancellationToken cancellationToken);

}
