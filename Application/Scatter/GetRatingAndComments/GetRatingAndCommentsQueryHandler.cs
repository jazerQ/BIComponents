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
        
        return collection
            .AsQueryable()
            .Where(p => 
                !string.IsNullOrWhiteSpace(p["ProductRating"].AsString) &&
                !string.IsNullOrWhiteSpace(p["CountOfComments"].AsString) &&
                !string.IsNullOrEmpty(p["Name"].AsString))
            .Select(p => new GetRatingAndCommentsDtoResponse()
        {
            Id = GlobalHelper.OnlyDigits(p["Id"].AsString),
            CountOfComments = GlobalHelper.OnlyDigits(p["CountOfComments"].AsString),
            Rating = float.Parse(p["ProductRating"].AsString, CultureInfo.InvariantCulture),
            Name = p["Name"].AsString
        }).ToList();
    }
}