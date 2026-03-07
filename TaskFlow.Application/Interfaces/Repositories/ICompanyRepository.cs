using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<ICollection<CompanyEntity>?> GetAllCompaniesAsync();
    Task<CompanyEntity?> GetCompanyByIdAsync(int id);
    Task<int?> DeleteCompanyByIdAsync(int id);
    Task<CompanyEntity> UpdateCompanyByIdAsync(int id, CompanyEntity company);
    Task<int?> AddCompanyAsync(CompanyEntity company);
}
