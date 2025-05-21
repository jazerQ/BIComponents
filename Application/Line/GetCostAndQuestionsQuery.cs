using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Line;

public class GetCostAndQuestionsQuery : IRequest<IEnumerable<GetCostAndQuestionsDtoResponse>>;

public class GetCostAndQuestionsQueryHandler(IAppDbContext context) : IRequestHandler<GetCostAndQuestionsQuery, IEnumerable<GetCostAndQuestionsDtoResponse>>
{
    public async Task<IEnumerable<GetCostAndQuestionsDtoResponse>> Handle(GetCostAndQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .Where(p => 
                p.Defaultprice != null &&
                p.Countofquestions != null) 
            .OrderBy(p => p.Defaultprice)
            .Select(p => new GetCostAndQuestionsDtoResponse()
            {
                Cost = p.Defaultprice!.Value,
                CountOfQuestions = p.Countofquestions!.Value
            }).ToListAsync(cancellationToken);
    }
}