using Application.Horizontal;
using Application.Line;
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
        try
        {
            return Ok(await mediator.Send(new GetCountOfAnyTypesQuery()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/horizontal")]
    public async Task<IActionResult> GetDataForHorizontal()
    {
        try
        {
            return Ok(await mediator.Send(new GetTopFiveQuery(IsBest: true)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/simple")]
    public async Task<IActionResult> GetDataForSimpleBarChart()
    {
        try
        {
            return Ok(await mediator.Send(new GetTopFiveQuery(IsBest: false)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/scatter")]
    public async Task<IActionResult> GetDataForScatterChart()
    {
        try
        {
            return Ok(await mediator.Send(new GetRatingAndCommentsQuery()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/scattercosts")]
    public async Task<IActionResult> GetDataForScatter()
    {
        try
        {
            return Ok(await mediator.Send(new GetRatingAndCostsQuery()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/line")]
    public async Task<IActionResult> GetDataForLineChart()
    {
        try
        {
            return Ok(await mediator.Send(new GetCostAndQuestionsQuery()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}