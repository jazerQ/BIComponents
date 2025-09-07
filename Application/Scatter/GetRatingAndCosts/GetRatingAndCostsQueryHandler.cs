using System.Globalization;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Scatter.GetRatingAndCosts;

public class GetRatingAndCostsQueryHandler(IMongoDatabase db) : IRequestHandler<GetRatingAndCostsQuery, IEnumerable<GetRatingAndCostsDtoResponse>>
{
    public async Task<IEnumerable<GetRatingAndCostsDtoResponse>> Handle(GetRatingAndCostsQuery request, CancellationToken cancellationToken)
    {
        var collection = db.GetCollection<BsonDocument>("BIObjects");
        
        return collection
            .AsQueryable()    
            .Where(p =>
                string.IsNullOrWhiteSpace(p["DefaultPrice"].AsString) &&
                string.IsNullOrWhiteSpace(p["ProductRating"].AsString))
            .Select(p => new GetRatingAndCostsDtoResponse()
            {
                Id = GlobalHelper.OnlyDigits(p["Id"].AsString),
                Price = GlobalHelper.OnlyDigits(p["DefaultPrice"].AsString),
                PriceWithCard = !string.IsNullOrWhiteSpace(p["PriceWithCard"].AsString) 
                    ? GlobalHelper.OnlyDigits(p["PriceWithCard"].AsString) 
                    : GlobalHelper.OnlyDigits(p["DefaultPrice"].AsString),
                Rating = float.Parse(p["ProductRating"].AsString, CultureInfo.InvariantCulture)
            }).ToList();
    }
}