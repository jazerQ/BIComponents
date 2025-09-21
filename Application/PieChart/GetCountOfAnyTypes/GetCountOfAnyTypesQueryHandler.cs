using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.PieChart.GetCountOfAnyTypes;

public class GetCountOfAnyTypesQueryHandler(IMongoDatabase db) 
    : IRequestHandler<GetCountOfAnyTypesQuery, IEnumerable<GetCountOfAnyTypesDtoResponse>>
{
    public async Task<IEnumerable<GetCountOfAnyTypesDtoResponse>> Handle(GetCountOfAnyTypesQuery request, CancellationToken cancellationToken)
    {
        var collection = db.GetCollection<BsonDocument>("BIObjects");
        
        if (await collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken) == 0)
        {
            throw new Exception("в коллекции пусто");
        }

        var list = await collection
            .Aggregate()
            .Group(new BsonDocument
            {
                { "_id", "$Type" },
                { "count", new BsonDocument("$sum", 1) }
            })
            .ToListAsync(cancellationToken);
        
        return list.Select(i => new GetCountOfAnyTypesDtoResponse
        {
            Count = i["count"].AsInt32,
            Name = i["_id"].AsString
        });
    }
}