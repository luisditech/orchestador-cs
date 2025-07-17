using TropicFeel.Application.JLP.OrderJlpLists.Commands;
using TropicFeel.Application.Order.Commands.CreateOrder;
using TropicFeel.Application.Product.Commands.UpdateStock;
using TropicFeel.Application.Sprint.TrackingSprint.Commands;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Sprint;

namespace Microsoft.Extensions.DependencyInjection.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrderCommand, CreateOrderRequestDto>();
        CreateMap<UpdateStockCommand, UpdateStockDto>();
        CreateMap<CreateTrackingCommand, CreateTrackingRequestDto>().ReverseMap();
        CreateMap<CreateOrderJlpCommand, OrderRequestJlpDto>().ReverseMap();
    }
}
