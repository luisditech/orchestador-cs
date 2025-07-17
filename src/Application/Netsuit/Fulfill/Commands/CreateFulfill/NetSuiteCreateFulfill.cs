using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Order.Commands.NetSuitCreateOrder;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Netsuit.Fulfill.Commands.CreateFulfill;


public record NetSuiteCreateFulfillCommand(int Id) : IRequest<int>
{
    //public int Id { get; init; }
}

public class NetSuiteCreateFulfillCommandValidator : AbstractValidator<NetSuiteCreateFulfillCommand>
{
    public NetSuiteCreateFulfillCommandValidator()
    {
    }
}

public class NetSuiteCreateFulfillCommandHandler : IRequestHandler<NetSuiteCreateFulfillCommand,int>
{
    private readonly IApplicationDbContext _context;
    private readonly INetsuitClientService _netsuitClientService;
    private readonly ISprintRequestService _printRequestService;

    public NetSuiteCreateFulfillCommandHandler(IApplicationDbContext context, INetsuitClientService netsuitClientService, 
        ISprintRequestService printRequestService)
    {
        _context = context;
        _netsuitClientService = netsuitClientService;
        _printRequestService = printRequestService;
    }

    public async Task<int> Handle(NetSuiteCreateFulfillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            

            var tracking = await _printRequestService.GetTracking(request.Id);
            
                var createFulfill = new FulfillDto()
                {
                    Action = "transform",
                    RecordType = "SalesOrder",
                    Id = int.TryParse(tracking?.Result.HAWB, out int hawbInt) ? hawbInt : 0,
                    ToType = "itemfulfillment",
                    CustBody_Pwk_Tracking_Codes = tracking?.Result.TrackingLink,
                    CustBody_Tf_Sprint_Fulfill_Hawb = tracking?.Result.HAWB,
                    Item = _context.Items.Where(x => x.OrderId == request.Id)
                    .Select(x => new ItemFulfill()
                    {
                        //Quantity = x.Quantity != null ? x.Quantity : string.Empty,
                        Item = x.ItemReference != null ? x.ItemReference : string.Empty
                    }).ToList()

                };


                //await _netsuitClientService.CreateFulfill(createFulfill);
                return 1;

            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
            
        }
    }

}
