using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ComapniesDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<int?> CreateCompanyAsync(CompanyPostDto dto, int ownerUserId, CancellationToken cancellationToken)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<CompanyEntity>(dto);
        try
        {
            var createdId = await _companyRepository.AddCompanyAsync(entity, ownerUserId, cancellationToken);
            return createdId;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<int?> DeleteCompanyByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _companyRepository.DeleteCompanyByIdAsync(id, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Удаляет компанию только если userId является владельцем (owner).
    /// </summary>
    public async Task<int?> DeleteUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken)
    {
        // Получаем компанию вместе с CompanyUsers
        var company = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
        if (company == null) return null;

        // Проверяем, есть ли у пользователя роль Owner
        var isOwner = company.CompanyUsers?.Any(u => u.UserId == userId && u.CompanyRole == CompanyRole.Owner) ?? false;
        if (!isOwner)
        {
            // Можно бросить UnauthorizedAccessException или вернуть null
            throw new UnauthorizedAccessException("User is not owner of the company.");
        }

        // Удаляем
        return await _companyRepository.DeleteCompanyByIdAsync(id, cancellationToken);
    }

    public async Task<ICollection<CompanyGetDto>> GetAllCompaniesAsync(CancellationToken cancellationToken)
    {
        var entities = await _companyRepository.GetAllCompaniesAsync(cancellationToken);
        if (entities == null) return new List<CompanyGetDto>();

        return _mapper<CompanyGetDto>(entities);
    }

    public async Task<ICollection<CompanyGetDto>> GetCompaniesPaginationAsync(int count, int side, CancellationToken cancellationToken)
    {
        var entities = await _companyRepository.GetCompaniesPaginationAsync(count, side, cancellationToken);
        if (entities == null) return new List<CompanyGetDto>();

        return entities.Select(MapToDto).ToList();
    }

    public async Task<CompanyGetDto?> GetCompanyByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
        if (entity == null) return null;
        return MapToDto(entity);
    }

    public async Task<ICollection<CompanyGetDto>> GetUsersCompaniesAsync(int userId, CancellationToken cancellationToken)
    {
        var entities = await _companyRepository.GetCompaniesByUserIdAsync(userId, cancellationToken);
        if (entities == null) return new List<CompanyGetDto>();
        return entities.Select(MapToDto).ToList();
    }

    public async Task<ICollection<CompanyGetDto>> GetUsersCompaniesPaginationAsync(int userId, int side, int count, CancellationToken cancellationToken)
    {
        var entities = await _companyRepository.GetCompaniesByUserIdPaginationAsync(userId, count, side, cancellationToken);
        if (entities == null) return new List<CompanyGetDto>();
        return entities.Select(MapToDto).ToList();
    }

    public async Task<CompanyGetDto?> GetUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
        if (company == null) return null;

        var isMember = company.CompanyUsers?.Any(u => u.UserId == userId) ?? false;
        if (!isMember)
        {
            throw new UnauthorizedAccessException("User is not a member of this company.");
        }

        return MapToDto(company);
    }

    public async Task<int?> UpdateCompanyByIdAsync(int id, CompanyUpdateDto dto, CancellationToken cancellationToken)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        // Создаём CompanyEntity с Id и полями из dto — репозиторий сам обновит только непустые поля
        var newEntity = new CompanyEntity
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description
        };

        var updated = await _companyRepository.UpdateCompanyAsync(newEntity, cancellationToken);
        return updated?.Id;
    }

    public async Task UpdateUsersCompanyByIdAsync(int id, CompanyUpdateDto dto, int userId, CancellationToken cancellationToken)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var company = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
        if (company == null) throw new InvalidOperationException("Company not found.");

        // проверка прав — допускаем владельца или администратора компании
        var hasRight = company.CompanyUsers?.Any(u =>
            u.UserId == userId && (u.CompanyRole == CompanyRole.Owner || u.CompanyRole == CompanyRole.Admin)) ?? false;

        if (!hasRight)
            throw new UnauthorizedAccessException("User has no rights to update this company.");

        var newEntity = new CompanyEntity
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description
        };

        await _companyRepository.UpdateCompanyAsync(newEntity, cancellationToken);
    }
}
