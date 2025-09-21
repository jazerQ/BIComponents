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
        
        if (await collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken) == 0)
        {
            throw new Exception("в коллекции пусто");
        }
        
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Ne("DefaultPrice", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("DefaultPrice", string.Empty),
            Builders<BsonDocument>.Filter.Ne("ProductRating", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("ProductRating", string.Empty));
        
        return (await collection
            .Find(filter)
            .ToListAsync(cancellationToken))
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