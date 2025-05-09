using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Horizontal;

public class GetTopFiveQueryHandler(IAppDbContext db) : IRequestHandler<GetTopFiveQuery, IEnumerable<GetTopFiveDtoResponse>>
{
    public async Task<IEnumerable<GetTopFiveDtoResponse>> Handle(GetTopFiveQuery request, CancellationToken cancellationToken)
    {
        return request.IsBest
            ? db.Products
                .AsNoTracking()
                .Where(p => p.Productrating != null)
                .OrderByDescending(p => p.Productrating)
                .Take(5)
                .Select(p => new GetTopFiveDtoResponse()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Rating = p.Productrating
                })
            : db.Products
                .AsNoTracking()
                .Where(p => p.Productrating != null)
                .OrderBy(p => p.Productrating)
                .Take(5)
                .Select(p => new GetTopFiveDtoResponse()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Rating = p.Productrating
                });
    }
}