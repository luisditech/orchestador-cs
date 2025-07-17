using TropicFeel.Application.Common.Interfaces;

namespace TropicFeel.Application.Sprint.FulfillSprint.Queries.GetFulfill;

public record GetFulfillQuery(int id) : IRequest<FulfillVm?>
{
}

public class GetFulfillQueryValidator : AbstractValidator<GetFulfillQuery>
{
    public GetFulfillQueryValidator()
    {
    }
}

public class GetFulfillQueryHandler : IRequestHandler<GetFulfillQuery, FulfillVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly ISprintRequestService _sprintRequestService;
    public GetFulfillQueryHandler(IApplicationDbContext context, ISprintRequestService sprintRequestService)
    {
        _context = context;
        _sprintRequestService = sprintRequestService;
    }

    public async Task<FulfillVm?> Handle(GetFulfillQuery request, CancellationToken cancellationToken)
    {
        var sprintFulfill = await _sprintRequestService.GetFulfill(request.id);
        if (sprintFulfill != null)
        {
            return new FulfillVm()
            {
                Status = sprintFulfill.Status,
                Result = sprintFulfill.Result,
                Count = sprintFulfill.Count,
                Message = sprintFulfill.Message

            };
        }
        else
        {
            return null;
        }
    }
}
