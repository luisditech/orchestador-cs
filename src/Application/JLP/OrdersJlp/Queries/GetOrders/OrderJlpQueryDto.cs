using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;
public class OrderJlpQueryDto
{
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
}

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<OrderJlpDto, OrderJlpQueryDto>();
    }
}
