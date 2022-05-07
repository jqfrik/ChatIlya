using Microsoft.AspNetCore.Mvc;

namespace Chat.Web_Api.Controllers;

public class Risk
{
    public string Name { get; set; }
    
    public Guid Id { get; set; }
}

public class RiskRequest
{
    public string Name { get; set; }
    
    public Guid Id { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private static List<Risk> risks = new List<Risk>();

    [HttpGet]
    public async Task<IActionResult> GetRisks()
    {
        return Ok(risks);
    }

    [HttpPost]
    public async Task<IActionResult> AddRisk(RiskRequest request)
    {
        if (request.Id != Guid.Empty && request.Name != "")
        {
            var risk = new Risk()
            {
                Id = request.Id,
                Name = request.Name
            };
            risks.Add(risk);
            return Ok(new { Success = true });
        }

        return BadRequest(new {Success = false});
    }
}