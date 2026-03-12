using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly TaskFlowDbContext _context;
        public CompanyRepository(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task<int?> AddCompanyAsync(CompanyEntity company, int ownerId, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Companies.AddAsync(company, cancellationToken);
                await _context.CompanyUsers.AddAsync(new CompanyUserEntity
                {
                    CompanyId = company.Id,
                    UserId = ownerId,
                    CompanyRole = CompanyRole.Owner
                }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return company.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: AddCompanyAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with AddCompanyAsync");
            }
        }

        public async Task<int?> AddEmployeeToCompanyAsync(CompanyUserEntity companyUserEntity, CancellationToken cancellationToken)
        {
            try
            {
                await _context.CompanyUsers.AddAsync(companyUserEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return companyUserEntity.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: AddEmployeeToCompanyAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with AddEmployeeToCompanyAsync");
            }
        }

        public async Task<int?> DeleteCompanyByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.Companies.Remove(new CompanyEntity { Id = id });
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: DeleteCompanyByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with DeleteCompanyByIdAsync");
            }
        }

        public async Task<int?> DeleteEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.CompanyUsers.Remove(new CompanyUserEntity { Id = id });
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: DeleteEmployeeByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with DeleteEmployeeByIdAsync");
            }
        }

        public async Task<ICollection<CompanyEntity>?> GetAllCompaniesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Companies
                    .Include(c => c.Projects)
                    .Include(c => c.CompanyUsers)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetAllCompaniesAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetAllCompaniesAsync");
            }
        }

        public async Task<ICollection<CompanyUserEntity>> GetAllCompanyEmploersAsync(int companyId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.CompanyUsers
                    .Include(e => e.Company)
                    .Include(e => e.User)
                    .Where(c => c.CompanyId == companyId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetAllCompanyEmploersAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetAllCompanyEmploersAsync");
            }
        }

        public async Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Companies
                    .Include(c => c.Projects)
                    .Include(c => c.CompanyUsers)
                    .Where(c => c.CompanyUsers.Any(u => u.Id == userId))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompaniesByUserIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompaniesByUserIdAsync");
            }
        }

        public async Task<ICollection<CompanyEntity>?> GetCompaniesByUserIdPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if (side < 1)
                {
                    side = 1;
                }
                return await _context.Companies
                    .Include(c => c.Projects)
                    .Include(c => c.CompanyUsers)
                    .OrderBy(c => c.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .Where (c => c.CompanyUsers.Any(u => u.Id == userId))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompaniesByUserIdPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompaniesByUserIdPaginationAsync");
            }
        }

        public async Task<ICollection<CompanyEntity>?> GetCompaniesPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Companies
                    .Include(c => c.Projects)
                    .Include(c => c.CompanyUsers)
                    .OrderBy(c => c.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompaniesPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompaniesPaginationAsync");
            }
        }

        public async Task<CompanyEntity?> GetCompanyByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Companies
                    .Include(c => c.Projects)
                    .Include(c => c.CompanyUsers)
                    .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompanyByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompanyByIdAsync");
            }
        }

        public async Task<ICollection<CompanyUserEntity>> GetCompanyEmploersByRoleAsync(int companyId, CompanyRole role, CancellationToken cancellationToken)
        {
            try
            {
                return (await GetAllCompanyEmploersAsync(companyId, cancellationToken))
                    .Where(u => u.CompanyRole == role)
                    .ToList();
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompanyEmploersByRoleAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompanyEmploersByRoleAsync");
            }
        }

        public async Task<CompanyUserEntity> GetCompanyUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.CompanyUsers
                    .Include(u => u.Company)
                    .Include(u => u.User)
                    .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: GetCompanyUserByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with GetCompanyUserByIdAsync");
            }
        }

        public async Task UpdateChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Company Repository: UpdateChangesAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Company Repository: Problem with UpdateChangesAsync");
            }
        }
    }
}
