using System.Text;
using Application.Horizontal;
using Application.Line;
using Application.PieChart.GetCountOfAnyTypes;
using Application.Scatter;
using Application.Scatter.GetRatingAndCosts;
using Aspose.Cells;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BIPractice.Back.Controllers;

[ApiController]
[Route("[controller]")]
public class ChartController(IMediator mediator, IMongoDatabase db) : ControllerBase
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

    [HttpPost("/upload-file")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var collection = db.GetCollection<BsonDocument>("BIObjects");
            
            var wb = new Workbook(stream, new TxtLoadOptions(LoadFormat.Csv)
            {
                Separator = ',',
                Encoding = Encoding.UTF8,
                HasFormula = false
            });

            var sheet = wb.Worksheets.FirstOrDefault() ?? throw new NullReferenceException("Данные не смог забрать");

            for (int row = 1; row < sheet.Cells.MaxDataRow; row++)
            {
                var doc = new BsonDocument();
                for (int col = 0; col < sheet.Cells.MaxDataColumn; col++)
                {
                    var header = sheet.Cells[0, col].Value.ToString();
                    doc[header] = sheet.Cells[row, col].StringValue;
                }
                
                await collection.InsertOneAsync(doc);
            }
        }

        return Ok();
    }
    
}