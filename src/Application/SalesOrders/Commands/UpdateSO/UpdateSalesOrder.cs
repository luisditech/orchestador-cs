using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.TodoItems.Commands.UpdateTodoItem;

namespace TropicFeel.Application.SalesOrders.Commands.UpdateSO;


public record UpdateSalesOrderCommand : IRequest
{

    public required string OrderReference { get; init; }
    public DateTime OrderDate { get; init; }
}


public class UpdateSaleOrderCommandHandler : IRequestHandler<UpdateSalesOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSaleOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SalesOrder
            .FindAsync(new object[] { request.OrderReference }, cancellationToken);

        Guard.Against.NotFound(request.OrderReference, entity);

        entity.OrderDateJlp = request.OrderDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
