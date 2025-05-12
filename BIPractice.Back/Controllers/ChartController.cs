using Application.Horizontal;
using Application.PieChart.GetCountOfAnyTypes;
using Application.Scatter;
using Application.Scatter.GetRatingAndCosts;
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

    [HttpGet("/horizontal")]
    public async Task<IActionResult> GetDataForHorizontal()
    {
        return Ok(await mediator.Send(new GetTopFiveQuery(IsBest: true)));
    }

    [HttpGet("/simple")]
    public async Task<IActionResult> GetDataForSimpleBarChart()
    {
        return Ok(await mediator.Send(new GetTopFiveQuery(IsBest: false)));
    }

    [HttpGet("/scatter")]
    public async Task<IActionResult> GetDataForScatterChart()
    {
        return Ok(await mediator.Send(new GetRatingAndCommentsQuery()));
    }

    [HttpGet("/scattercosts")]
    public async Task<IActionResult> GetDataForScatter()
    {
        return Ok(await mediator.Send(new GetRatingAndCostsQuery()));
    }
}