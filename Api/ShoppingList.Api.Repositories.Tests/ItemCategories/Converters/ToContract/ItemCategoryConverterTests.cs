using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToContract;
using ItemCategory = ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ItemCategories.Converters.ToContract;

public class ItemCategoryConverterTests : ToContractConverterTestBase<IItemCategory, ItemCategory, ItemCategoryConverter>
{
    protected override ItemCategoryConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<IItemCategory, ItemCategory> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }
}