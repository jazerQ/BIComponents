using MediatR;

namespace Application.Horizontal;

public record GetTopFiveQuery(bool IsBest) : IRequest<IEnumerable<GetTopFiveDtoResponse>>;