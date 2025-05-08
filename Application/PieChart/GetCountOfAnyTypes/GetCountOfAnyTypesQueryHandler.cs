using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PieChart.GetCountOfAnyTypes;

public class GetCountOfAnyTypesQueryHandler(IAppDbContext db) 
    : IRequestHandler<GetCountOfAnyTypesQuery, IEnumerable<GetCountOfAnyTypesDtoResponse>>
{
    public async Task<IEnumerable<GetCountOfAnyTypesDtoResponse>> Handle(GetCountOfAnyTypesQuery request, CancellationToken cancellationToken)
    {
        return db.Models
            .AsNoTracking()
            .GroupBy(p => p.TypeId)
            .Select(g => new GetCountOfAnyTypesDtoResponse()
            {
                Id = g.Key,
                Count = g.Count()
            });
    }
}