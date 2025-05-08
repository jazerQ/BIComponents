using Application.PieChart.GetCountOfAnyTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BIPractice.Back.Controllers;

[ApiController]
[Route("[controller]")]
public class ChartController(IMediator mediator) : ControllerBase
{
    [HttpGet("/pie")]
    public async Task<IActionResult> GetDataForPieChart()
    {
        return Ok(await mediator.Send(new GetCountOfAnyTypesQuery()));
    }
}