using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Netsuit.Fulfill.Commands.CreateFulfill;
using TropicFeel.Domain.Dtos.Netsuit;

namespace TropicFeel.Application.Sprint.TrackingSprint.Queries.GetTracking;

public record GetTrackingQuery(int Id) : IRequest<TrackingVm>
{
}

public class GetTrackingQueryValidator : AbstractValidator<GetTrackingQuery>
{
    public GetTrackingQueryValidator()
    {
    }
}

public class GetTrackingQueryHandler : IRequestHandler<GetTrackingQuery, TrackingVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;
    private readonly INetsuitClientService _netsuitClientService;

    public GetTrackingQueryHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService,INetsuitClientService netsuitClientService)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
        _netsuitClientService = netsuitClientService;
    }

    public async Task<TrackingVm?> Handle(GetTrackingQuery request, CancellationToken cancellationToken)
    {
        var tracking = await _sprintRequestService.GetTracking(request.Id);
        
        return tracking != null
            ? new TrackingVm()
            {
                Count = tracking.Count,
                Message = tracking.Message,
                Result = tracking.Result,
                Status = tracking.Status
            }
            : null;
    }
}
