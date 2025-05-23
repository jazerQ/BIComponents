using DataAccess;
using MediatR;

namespace Application.Scatter.GetRatingAndCosts;

public class GetRatingAndCostsQueryHandler(IAppDbContext db) : IRequestHandler<GetRatingAndCostsQuery, IEnumerable<GetRatingAndCostsDtoResponse>>
{
    public async Task<IEnumerable<GetRatingAndCostsDtoResponse>> Handle(GetRatingAndCostsQuery request, CancellationToken cancellationToken)
    {
        return db.Products
            .Where(p =>
                p.Defaultprice != null &&
                p.Productrating != null)
            .Select(p => new GetRatingAndCostsDtoResponse()
            {
                Id = p.Id,
                Price = p.Defaultprice!.Value,
                PriceWithCard = p.Pricewithcard!.HasValue ? p.Pricewithcard.Value : p.Defaultprice.Value,
                Rating = p.Productrating!.Value
            });
    }
}