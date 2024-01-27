using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToContract;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ShoppingLists.Converters.ToContract;

public class ShoppingListConverterTests
    : ToContractConverterTestBase<Domain.ShoppingLists.Models.ShoppingList, Repositories.ShoppingLists.Entities.ShoppingList, ShoppingListConverter>
{
    protected override ShoppingListConverter CreateSut()
    {
        return new ShoppingListConverter();
    }

    protected override void AddMapping(IMappingExpression<Domain.ShoppingLists.Models.ShoppingList,
        Repositories.ShoppingLists.Entities.ShoppingList> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletionDate))
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId.Value))
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.ItemsOnList, opt => opt.MapFrom(src => Convert(src)));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<(Domain.ShoppingLists.Models.ShoppingList, IShoppingListSection, ShoppingListItem), ItemsOnList>(MemberList.Destination)
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(si => si.Item3.Id.Value))
            .ForMember(dest => dest.ItemTypeId, opt => opt.MapFrom(si => si.Item3.TypeId.HasValue ? si.Item3.TypeId.Value : (Guid?)null))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(si => si.Item3.Quantity.Value))
            .ForMember(dest => dest.InBasket, opt => opt.MapFrom(si => si.Item3.IsInBasket))
            .ForMember(dest => dest.SectionId, opt => opt.MapFrom(si => si.Item2.Id.Value))
            .ForMember(dest => dest.ShoppingListId, opt => opt.MapFrom(si => si.Item1.Id.Value))
            .ForMember(dest => dest.ShoppingList, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }

    private static IEnumerable<(Domain.ShoppingLists.Models.ShoppingList, IShoppingListSection, ShoppingListItem)> Convert(
        Domain.ShoppingLists.Models.ShoppingList src)
    {
        List<(Domain.ShoppingLists.Models.ShoppingList, IShoppingListSection, ShoppingListItem)> list = new();
        foreach (var section in src.Sections)
        {
            foreach (var item in section.Items)
            {
                list.Add((src, section, item));
            }
        }
        return list;
    }
}