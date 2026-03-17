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
    private readonly ICachingService _cachingService;
    public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ICachingService cachingService)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
        _cachingService = cachingService;
    }
    public async Task<int?> CreateCompanyAsync(CompanyPostDto dto, int ownerUserId, CancellationToken cancellationToken)
    {
        try
        {
            // Delete the cache for companies if it exists
            if (await _cachingService.GetAsync<ICollection<CompanyGetDto>>("Companies") != null)
            {
                await _cachingService.RemoveAsync("Companies");
            }
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var company = _mapper.Map<CompanyEntity>(dto);

            return await _companyRepository.AddCompanyAsync(company, ownerUserId, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the company: {ex.Message}");
            return null;
        }
    }

    public async Task<ICollection<CompanyGetDto>> GetCompaniesPaginationAsync(int count, int side, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _companyRepository.GetCompaniesPaginationAsync(count, side, cancellationToken);
            var companies = _mapper.Map<ICollection<CompanyGetDto>>(data);
            return companies;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting pagination companies: {ex.Message}");
            return new List<CompanyGetDto>();
        }
    }

    public async Task<ICollection<CompanyGetDto>> GetUsersCompaniesAsync(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _companyRepository.GetCompaniesByUserIdAsync(userId, cancellationToken);
            var companies = _mapper.Map<ICollection<CompanyGetDto>>(data);
            return companies;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting user's companies: {ex.Message}");
            return new List<CompanyGetDto>();
        }
    }

    public async Task<ICollection<CompanyGetDto>> GetUsersCompaniesPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _companyRepository.GetCompaniesByUserIdPaginationAsync(userId, count, side, cancellationToken);
            var companies = _mapper.Map<ICollection<CompanyGetDto>>(data);
            return companies;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting user's pagination companies: {ex.Message}");
            return new List<CompanyGetDto>();
        }
    }

    public async Task<CompanyGetDto?> GetCompanyByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);

            if (data == null) return null;

            var company = _mapper.Map<CompanyGetDto>(data);

            return company;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An error occurred while getting the company by id: {id}; with an exception: {ex.Message}");
            return null;
        }
    }

    public async Task<CompanyGetDto?> GetUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
            if (data == null) return null;

            if(data.Users.Any(u => u.UserId == userId))
            {
                var company = _mapper.Map<CompanyGetDto>(data);
                return company;
            }
            else
            {
                Console.WriteLine($"User with id: {userId} is not in the company with id: {id}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting the user's company by id: {id}; with an exception: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> DeleteCompanyByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            if(await _cachingService.GetAsync<ICollection<CompanyGetDto>>("Companies") != null)
            {
                await _cachingService.RemoveAsync("Companies");
            }

            return await _companyRepository.DeleteCompanyByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the company by id: {id}; with an exception: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> DeleteUsersCompanyByIdAsync(int id, int userId, CancellationToken cancellationToken)
    {
        try
        {
            var company = await _companyRepository.GetCompanyByIdAsync(id, cancellationToken);
            if (company == null) return null;

            if (company.Users.Any(u =>u.UserId == userId && u.CompanyRole.ToString() == "Owner"))
            {

                if (await _cachingService.GetAsync<ICollection<CompanyGetDto>>("Companies") != null)
                {
                    await _cachingService.RemoveAsync("Companies");
                }

                return await _companyRepository.DeleteCompanyByIdAsync(id, cancellationToken);
            }
            else
            {
                Console.WriteLine($"User with id: {userId} can not delete the company with id: {id}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the user's company by id: {id}; with an exception: {ex.Message}");
            return null;
        }
    }

    public async Task<ICollection<CompanyGetDto>> GetAllCompaniesAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (await _cachingService.GetAsync<ICollection<CompanyGetDto>>("Companies") != null)
            {
                return await _cachingService.GetAsync<ICollection<CompanyGetDto>>("Companies");
            }
            else
            {
                var data = await _companyRepository.GetAllCompaniesAsync(cancellationToken);
                var companies = _mapper.Map<ICollection<CompanyGetDto>>(data);
                await _cachingService.SetAsync("Companies", companies, TimeSpan.FromMinutes(20));
                return companies;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting all companies: {ex.Message}");
            return new List<CompanyGetDto>();
        }
    }

    public Task<int?> UpdateCompanyByIdAsync(int id, CompanyUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUsersCompanyByIdAsync(int id, CompanyUpdateDto dto, int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

