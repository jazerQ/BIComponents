using System.Globalization;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Scatter;

public class GetRatingAndCommentsQueryHandler(IMongoDatabase db) 
    : IRequestHandler<GetRatingAndCommentsQuery, IEnumerable<GetRatingAndCommentsDtoResponse>>
{
    public async Task<IEnumerable<GetRatingAndCommentsDtoResponse>> Handle(GetRatingAndCommentsQuery request, CancellationToken cancellationToken)
    {
        var collection = db.GetCollection<BsonDocument>("BIObjects");
        
        if (await collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken) == 0)
        {
            throw new Exception("в коллекции пусто");
        }

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Ne("ProductRating", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("ProductRating", string.Empty),
            Builders<BsonDocument>.Filter.Ne("CountOfComments", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("CountOfComments", string.Empty),
            Builders<BsonDocument>.Filter.Ne("Name", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("Name", string.Empty));
        
        return (await collection
            .Find(filter)
            .ToListAsync(cancellationToken))
            .Select(p => new GetRatingAndCommentsDtoResponse()
            {
                Id = GlobalHelper.OnlyDigits(p["Id"].AsString),
                CountOfComments = GlobalHelper.OnlyDigits(p["CountOfComments"].AsString),
                Rating = float.Parse(p["ProductRating"].AsString, CultureInfo.InvariantCulture),
                Name = p["Name"].AsString
            }).ToList();
    }
}