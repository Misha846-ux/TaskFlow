using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize]
public class CompanyController : ControllerBase
{
    //===========================================Get=============================================
    /// <summary>
    /// Позволяет получить обсолютно все компании которые есть в бд
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        return null;
    }

    /// <summary>
    /// Позволяет получить абсолютно все компании из бд но порциями
    /// </summary>
    /// <param name="count">Количество компаний в одной порции данных</param>
    /// <param name="side">Номер порции данных</param>
    /// <returns></returns>
    [HttpGet("Filtred")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPagination([FromQuery] int count, int side)
    {
        return null;
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
    public Task<IActionResult> GetCompanyById([FromRoute] int id)
    {
        return null;
    }

    /// <summary>
    /// Позволет пользователю получить информацию о компании к которой он пренадлежит.
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
    public Task<IActionResult> DeleteCompanyByIdForAdmin([FromRoute] int id)
    {
        return null;
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
    public Task<IActionResult> UpdateForAdmin([FromBody] CopmanyUpdateDto company)
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
    public Task<IActionResult> Update([FromBody] CopmanyUpdateDto copmany)
    {
        return null;
    }
}
