using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize]
public class CompanyController : ControllerBase
{
    /// <summary>
    /// Return all companies
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllCompanies()
    {
        return Ok();
    }
    /// <summary>
    /// Allow to get company by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Company</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyById([FromRoute]int id)
    {
        return Ok();
    }
    /// <summary>
    /// Allow to delet company by id, only for admin role
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompanyById([FromRoute] int id)
    {
        return Ok();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyPostDto company)
    {
            return Ok();
    }
    [HttpPatch]
    public async Task<IActionResult> UpdateCompanyById([FromBody] CompanyUpdateDto company)
    {
        return Ok();
    }
}
