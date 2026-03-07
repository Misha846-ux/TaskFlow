using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize(Roles = "Admin")]
public class CompanyController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyById([FromRoute]int id)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompanyById([FromRoute] int id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyPostDto company)
    {
            return Ok();
    }
}
