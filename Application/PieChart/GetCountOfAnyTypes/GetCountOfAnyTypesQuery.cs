using MediatR;

namespace Application.PieChart.GetCountOfAnyTypes;

public record GetCountOfAnyTypesQuery() : IRequest<IEnumerable<GetCountOfAnyTypesDtoResponse>>;