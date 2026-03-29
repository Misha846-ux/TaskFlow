using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TaskFlow.Application.DTOs.ComapniesDTOs;
using TaskFlow.Application.Interfaces.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyController(ICompanyService _companyService, IJwtService _jwtService) : ControllerBase
    {
        //===========================================Get=============================================
        /// <summary>
        /// Позволяет получить обсолютно все компании которые есть в бд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync(cancellationToken);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in controller while getting all companies with an exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Позволяет получить абсолютно все компании из бд но порциями
        /// </summary>
        /// <param name="count">Количество компаний в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <returns></returns>
        [HttpGet("Filtred")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPagination([FromQuery] int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var companies = await _companyService.GetCompaniesPaginationAsync(count, side, cancellationToken);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in controller while getting companies paggination with an exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Позволяет получить все компании которые пренадлежат пользователю. Информацию о пользователе берём из
        /// Access или Refresh токена
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyCompanies")]
        public async Task<IActionResult> GetAllMyComapnies(CancellationToken cancellationToken)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var token = authHeader["Bearer ".Length..].Trim();

                var userIdString = _jwtService.GetTokenUsersId(token);
                if (!int.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user id in token.");

                var companies = await _companyService.GetUsersCompaniesAsync(userId, cancellationToken);

                return Ok(companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while getting user's companies: {ex.Message}");
                return BadRequest("An error occurred while getting the company.");
            }
        }

        /// <summary>
        /// Позволяет получить все компании которые пренадлежат порционно. Информацию о пользователе берём из
        /// Access или Refresh токена
        /// </summary>
        /// <param name="count">Количество компаний в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <returns></returns>
        [HttpGet("Filtred/MyCompanies")]
        public async Task<IActionResult> GetMyCompaniesPagination([FromQuery] int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var token = authHeader["Bearer ".Length..].Trim();

                var userIdString = _jwtService.GetTokenUsersId(token);
                if (!int.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user id in token.");

                var companies = await _companyService.GetUsersCompaniesPaginationAsync(userId, count, side, cancellationToken);

                return Ok(companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while getting user's companies pagination: {ex.Message}");
                return BadRequest("An error occurred while getting the company.");
            }
        }

        /// <summary>
        /// Позволяет админу получить информацию о любой компании по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCompanyById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var companies = await _companyService.GetCompanyByIdAsync(id, cancellationToken);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in controller while getting company by id with an exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Позволет пользователю получить информацию о компании к которой он пренадлежит. Информацию о пользователе берём из Access или Refresh токена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MyCompanyById")]
        public async Task<IActionResult> GetMyCompanyById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var token = authHeader["Bearer ".Length..].Trim();

                var userIdString = _jwtService.GetTokenUsersId(token);
                if (!int.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user id in token.");

                var company = await _companyService.GetUsersCompanyByIdAsync(id, userId, cancellationToken);

                return Ok(company);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while getting user's company by id: {ex.Message}");
                return BadRequest("An error occurred while getting the company.");
            }
        }

        //===========================================Post============================================

        /// <summary>
        /// Позволяет пользователю создать новую компанию в которой он буедт владельцем
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] CompanyPostDto company, CancellationToken cancellationToken)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var token = authHeader["Bearer ".Length..].Trim();

                var userIdString = _jwtService.GetTokenUsersId(token);
                if (!int.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user id in token.");

                await _companyService.CreateCompanyAsync(company, userId, cancellationToken);

                return Ok(company);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while adding user's company: {ex.Message}");
                return BadRequest("An error occurred while creating the company.");
            }
        }

        //===========================================Delete==========================================

        /// <summary>
        /// Позволяет админу удалить любую компанию по id
        /// </summary>
        /// <param name="id">id компании которая будет удалена</param>
        /// <returns></returns>
        [HttpDelete("DeleteForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCompanyByIdForAdmin([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _companyService.DeleteCompanyByIdAsync(id, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in controller while deleting company by id with an exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Позволяет пользователю удалить компанию в которой он являеться владельцом
        /// </summary>
        /// <param name="id">id компании которая будет удалена</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCompanyById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var token = authHeader["Bearer ".Length..].Trim();

                var userIdString = _jwtService.GetTokenUsersId(token);
                if (!int.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user id in token.");

                await _companyService.DeleteUsersCompanyByIdAsync(id, userId, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while deleting user's company: {ex.Message}");
                return BadRequest("An error occurred while deliting the company.");
            }
        }
        //===========================================Update==========================================


        /// <summary>
        /// Позволяет админу изменить анфирмацию о любой компании
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPut("UpdateForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateForAdmin([FromBody] CompanyUpdateDto company, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = await _companyService.UpdateCompanyByIdAsync(company, cancellationToken);
                return Ok(companyId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while updating the company: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Позволяет пользователю обновить данные о компании в которой он присутствует и в которой у него есть на это 
        /// право.
        /// </summary>
        /// <param name="copmany"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CompanyOfUserUpdateDto company, CancellationToken cancellationToken)
        {
            try
            {
                await _companyService.UpdateUsersCompanyByIdAsync(company, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CompanyController while updating the company: {ex.Message}");
                return null;
            }
        }
    }
}
