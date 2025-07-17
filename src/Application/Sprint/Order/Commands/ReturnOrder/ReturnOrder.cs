using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Domain.Events;

namespace TropicFeel.Application.Order.Commands.ReturnOrder;

public record ReturnOrderCommand : IRequest<string>
{
    public required string JlpSalesOrder { get; set; }
}

public class ReturnOrderCommandValidator : AbstractValidator<ReturnOrderCommand>
{
    public ReturnOrderCommandValidator()
    {
    }
}

public class ReturnOrderCommandHandler : IRequestHandler<ReturnOrderCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public ReturnOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<string> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newEvent = new SprintReturnOrderEnvent(new ReturnDto()
            {
                JlpSalesOrder = request.JlpSalesOrder,
            });
            await _mediator.Publish(newEvent, cancellationToken);
            
            return "Order returned successfully.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
