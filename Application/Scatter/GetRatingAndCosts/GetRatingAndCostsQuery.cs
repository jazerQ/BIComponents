using MediatR;

namespace Application.Scatter.GetRatingAndCosts;

public record GetRatingAndCostsQuery() : IRequest<IEnumerable<GetRatingAndCostsDtoResponse>>;