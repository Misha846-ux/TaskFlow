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
    Task<ICollection<CompanyEntity>?> GetAllCompaniesAsync();
    Task<ICollection<CompanyEntity>?> GetCompaniesPaginationAsync(int count, int side);
    Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdAsync(int userId); // We take userId from Access or Refresh token and find companies by it. I think it is better than by email because email can be changed but userId is unchangeable
    Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdPaginationAsync(int userId, int count, int side);
    Task<CompanyEntity?> GetCompanyByIdAsync(int id);
    Task<int?> AddCompanyAsync(CompanyEntity company);
    Task<int?> DeleteCompanyByIdAsync(int id);
    Task<int?> UpdateCompanyByIdAsync();


    Task<ICollection<CompanyUserEntity>> GetAllEmploersAsync();
    Task<ICollection<CompanyUserEntity>> GetAllCompanyEmploersAsync(int companyId);
    Task<CompanyUserEntity> GetCompanyUserByIdAsync(int id);
    Task<int?> AddEmployeeToCompanyAsync(CompanyUserEntity companyUserEntity);
    Task<int?> DeleteEmployeeByIdAsync(int id);
    Task<int?> UpdateEmployeeAsync();

}
