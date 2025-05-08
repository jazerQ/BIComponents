using MediatR;

namespace Application;

public record ExampleQuery() : IRequest<int>;

public class ExampleQueryHandler : IRequestHandler<ExampleQuery, int>
{
    public async Task<int> Handle(ExampleQuery request, CancellationToken cancellationToken)
    {
        return new Random().Next(1, 100);
    }
}