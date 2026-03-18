using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TaskFlow.Application.DTOs.ComapniesDTOs;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyController(ICompanyService _companyService) : ControllerBase
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
                return StatusCode(500, "An error occurred while processing your request.");
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Позволяет получить все компании которые пренадлежат пользователю. Информацию о пользователе берём из
        /// Access или Refresh токена
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyCompanies")]
        public async Task<IActionResult> GetAllMyComapnies()
        {
            return null;
        }

        /// <summary>
        /// Позволяет получить все компании которые пренадлежат порционно. Информацию о пользователе берём из
        /// Access или Refresh токена
        /// </summary>
        /// <param name="count">Количество компаний в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <returns></returns>
        [HttpGet("Filtred/MyCompanies")]
        public async Task<IActionResult> GetMyCompaniesPagination([FromQuery] int count, int side)
        {
            return null;
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Позволет пользователю получить информацию о компании к которой он пренадлежит. Информацию о пользователе берём из Access или Refresh токена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MyCompanyById")]
        public Task<IActionResult> GetMyCompanyById([FromRoute] int id)
        {
            return null;
        }

        //===========================================Post============================================

        /// <summary>
        /// Позволяет пользователю создать новую компанию в которой он буедт владельцем
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost("AddCompany")]
        public Task<IActionResult> AddCompany([FromBody] CompanyPostDto company)
        {
            return null;
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Позволяет пользователю удалить компанию в которой он являеться владельцом
        /// </summary>
        /// <param name="id">id компании которая будет удалена</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public Task<IActionResult> DeleteCompanyById([FromRoute] int id)
        {
            return null;
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
            return null;
        }


        /// <summary>
        /// Позволяет пользователю обновить данные о компании в которой он присутствует и в которой у него есть на это 
        /// право.
        /// </summary>
        /// <param name="copmany"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public Task<IActionResult> Update([FromBody] CompanyUpdateDto copmany)
        {
            return null;
        }
    }
}
