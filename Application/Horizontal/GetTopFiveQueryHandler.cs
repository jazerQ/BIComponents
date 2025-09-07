using System.Globalization;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Horizontal;

public class GetTopFiveQueryHandler(IMongoDatabase db) : IRequestHandler<GetTopFiveQuery, IEnumerable<GetTopFiveDtoResponse>>
{
    public async Task<IEnumerable<GetTopFiveDtoResponse>> Handle(GetTopFiveQuery request, CancellationToken cancellationToken)
    {
        var collection = db.GetCollection<BsonDocument>("BIObjects");

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Ne("ProductRating", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("ProductRating", "")
        );

        var sort = request.IsBest
            ? Builders<BsonDocument>.Sort.Descending("ProductRating")
            : Builders<BsonDocument>.Sort.Ascending("ProductRating");

        var documents = await collection.Find(filter)
            .Sort(sort)
            .Limit(5)
            .ToListAsync(cancellationToken);

        var result = documents.Select(p => new GetTopFiveDtoResponse
        {
            Id = int.Parse(p.GetValue("Id", "0").AsString),
            Name = p.GetValue("Name", "").AsString,
            Rating = float.Parse(p.GetValue("ProductRating", "0").AsString, CultureInfo.InvariantCulture)
        }).ToList();

        return result;
    }
}