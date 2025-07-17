using Microsoft.Extensions.DependencyInjection.Mappings;
using TropicFeel.Application.Common.Models;
using TropicFeel.Application.JLP.OrderJlpLists.Commands;
using TropicFeel.Domain.Dtos.JLP;

namespace TropicFeel.Application.Common.Mappings;

public static class MappingExtensions 
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    //public class MappingProfile : AutoMapper.Profile
    //{
    //    public MappingProfile()
    //    {
    //        CreateMap<CreateOrderJlpCommand, OrderRequestJlpDto>().ReverseMap();
    //    }
    //}
}
