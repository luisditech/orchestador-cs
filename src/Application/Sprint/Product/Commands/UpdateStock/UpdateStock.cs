using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;
namespace TropicFeel.Application.Product.Commands.UpdateStock;

public record UpdateStockCommand : IRequest<ResponseDto<ProductSprintDto>>
{
    public string? WarehouseLocationId { get; set; }
    public string? Sku { get; set; }
    public string? Quantity { get; set; }
}

public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
{
    public UpdateStockCommandValidator()
    {
    }
}

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, ResponseDto<ProductSprintDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;
    private readonly IMapper _mapper;

    public UpdateStockCommandHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService, IMapper mapper)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
        _mapper = mapper;
    }

    public async Task<ResponseDto<ProductSprintDto>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var update = _mapper.Map<UpdateStockDto>(request);
        
        var response = await _sprintRequestService.UpdateStockAsync(update);
        return response;
    }
}
