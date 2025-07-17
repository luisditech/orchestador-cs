using TropicFeel.Application.Common.Interfaces;

namespace TropicFeel.Application.Netsuit.Order.Queries;



public record GetNetSuiteOrderQuery(string OrderReference) : IRequest<NetSuiteJlpVm?>
{

}

public class GetNetSuiteOrderQueryValidator : AbstractValidator<GetNetSuiteOrderQuery>
{
    public GetNetSuiteOrderQueryValidator()
    {
    }
}

public class GetNetSuiteOrderQueryHandler : IRequestHandler<GetNetSuiteOrderQuery, NetSuiteJlpVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly IOAuthAuthenticationService _authAuthenticationService;
    

    public GetNetSuiteOrderQueryHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService, 
        IOAuthAuthenticationService oAuthAuthenticationService)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
        _authAuthenticationService = oAuthAuthenticationService;
        
    }

    public  async Task<NetSuiteJlpVm?> Handle(GetNetSuiteOrderQuery request, CancellationToken cancellationToken)
    {
       
        var orderReference =  await _jlpRequestService.GetOrdersByIdReference(request.OrderReference);
        if (orderReference != null)
        {
            return new NetSuiteJlpVm()
            {
                Count = orderReference.Count,
                Previous = orderReference.Previous,
                Next = orderReference.Next,
                Results = orderReference.Results
            };
        }
        return null;

    }
}
