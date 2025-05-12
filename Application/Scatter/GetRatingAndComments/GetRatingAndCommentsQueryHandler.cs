using DataAccess;
using MediatR;

namespace Application.Scatter;

public class GetRatingAndCommentsQueryHandler(IAppDbContext db) 
    : IRequestHandler<GetRatingAndCommentsQuery, IEnumerable<GetRatingAndCommentsDtoResponse>>
{
    public async Task<IEnumerable<GetRatingAndCommentsDtoResponse>> Handle(GetRatingAndCommentsQuery request, CancellationToken cancellationToken)
    {
        return db.Products
            .Where(p => 
                p.Productrating != null &&
                p.Countofcomments != null &&
                p.Name != null)
            .Select(p => new GetRatingAndCommentsDtoResponse()
        {
            Id = p.Id,
            CountOfComments = p.Countofcomments!.Value,
            Rating = p.Productrating!.Value,
            Name = p.Name!
        });
    }
}