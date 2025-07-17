using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Order.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<ResponseDto<OrderResponseDto>>
{
    public string? AttnOf { get; set; }
    public string? Telephone { get; set; }
    public string? EmailAddr { get; set; }
    public string? CompanyName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostCode { get; set; }
    public string? CountryID { get; set; }
    public string? CountryCode { get; set; }
    public string? SpecialInstructions { get; set; }
    public string? PackingNote { get; set; }
    public string? CustomerRef1 { get; set; }
    public string? CustomerRef2 { get; set; }
    public List<StockOrderItemDto>? StockOrderItems { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDto<OrderResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService, IMapper mapper)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
        _mapper = mapper;
    }

    public async Task<ResponseDto<OrderResponseDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createOrderRequest = _mapper.Map<CreateOrderRequestDto>(request);

        return await _sprintRequestService.CreateOrderAsync(createOrderRequest);
    }
}
