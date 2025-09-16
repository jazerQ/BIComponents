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
        
        return collection
            .AsQueryable()
            .AsNoTracking()
            .GroupBy(p => p["Type"].AsString)
            .Select(g => new GetCountOfAnyTypesDtoResponse()
            {
                Name = g.Key,
                Count = g.Count()
            }).ToList();
    }
}