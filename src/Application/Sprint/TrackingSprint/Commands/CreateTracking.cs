using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Sprint.TrackingSprint.Commands;
public record CreateTrackingCommand : IRequest<ResponseDto<List<TrackingResponseDto>>>
{
    public string? AWB { get; set; }
    public string? Status { get; set; }
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
    public string? Carrier { get; set; }
    public string? Tracking { get; set; }
    public List<StockOrderItemDto>? StockOrderItems { get; set; }
}

public class CreateTrackingCommandValidator : AbstractValidator<CreateTrackingCommand>
{
    public CreateTrackingCommandValidator() { }
}

public class CreateTrackingCommandHandler : IRequestHandler<CreateTrackingCommand, ResponseDto<List<TrackingResponseDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;
    private readonly IMapper _mapper;

    public CreateTrackingCommandHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService, IMapper mapper)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<TrackingResponseDto>>> Handle(CreateTrackingCommand request, CancellationToken cancellationToken)
    {
        var createtrackingRequest = _mapper.Map<CreateTrackingRequestDto>(request);

        return await _sprintRequestService.CreateTracking(createtrackingRequest);
    }
}
