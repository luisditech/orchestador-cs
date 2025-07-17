using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;
using TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;

namespace TropicFeel.Application.JLP.OrdersJlp.Queries.GetProducts;
 public class GetProductsPartNumber
{

}
public record ProductPartNumber(string part_number) : IRequest<ProductVm?>
{

}

public class ProductPartNumberValidator : AbstractValidator<ProductPartNumber>
{
    public ProductPartNumberValidator()
    {
    }
}

public class ProductPartNumberHandler : IRequestHandler<ProductPartNumber, ProductVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;

    public ProductPartNumberHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
    }

    public async Task<ProductVm?> Handle(ProductPartNumber request, CancellationToken cancellationToken)
    {
        var products = await _jlpRequestService.GetProductsByPartNumber(request.part_number);
        if (products != null)
        {
            return new ProductVm()
            {
                Count = products.count,
                Previous = products.previous,
                Next = products.next,
                results = products.results
            };
        }
        return null;

    }
}
