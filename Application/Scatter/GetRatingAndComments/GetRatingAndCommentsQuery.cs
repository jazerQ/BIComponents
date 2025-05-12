using MediatR;

namespace Application.Scatter;

public record GetRatingAndCommentsQuery() : IRequest<IEnumerable<GetRatingAndCommentsDtoResponse>>;