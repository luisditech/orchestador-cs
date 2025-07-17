using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Sprint.Product.Queries.GetProducts;

public record GetProductsQuery : IRequest<ResponseDto<List<ProductSprintDto>>>
{
}

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
    }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ResponseDto<List<ProductSprintDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;

    public GetProductsQueryHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
    }

    public async Task<ResponseDto<List<ProductSprintDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _sprintRequestService.GetProductsAsync();
    }
}
