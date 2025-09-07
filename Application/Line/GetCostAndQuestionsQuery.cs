using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Line;

public class GetCostAndQuestionsQuery : IRequest<IEnumerable<GetCostAndQuestionsDtoResponse>>;

public class GetCostAndQuestionsQueryHandler(IMongoDatabase db) : IRequestHandler<GetCostAndQuestionsQuery, IEnumerable<GetCostAndQuestionsDtoResponse>>
{
    public async Task<IEnumerable<GetCostAndQuestionsDtoResponse>> Handle(
        GetCostAndQuestionsQuery request, 
        CancellationToken cancellationToken)
    {
        var collection = db.GetCollection<BsonDocument>("BIObjects");

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Ne("DefaultPrice", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("DefaultPrice", ""), // не пустая строка
            Builders<BsonDocument>.Filter.Ne("CountOfQuestions", BsonNull.Value),
            Builders<BsonDocument>.Filter.Ne("CountOfQuestions", "") // не пустая строка
        );
        var documents = await collection.Find(filter)
            .ToListAsync(cancellationToken);

        var result = documents.Select(p => new GetCostAndQuestionsDtoResponse
            {
                Cost = GlobalHelper.OnlyDigits(p.GetValue("DefaultPrice", "").AsString),
                CountOfQuestions = GlobalHelper.OnlyDigits(p.GetValue("CountOfQuestions", "").AsString)
            })
            .OrderByDescending(p => p.Cost)
            .ToList();

        return result;
    }
}