using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class ShoppingListItemConverterTests
    : ToDomainConverterTestBase<ItemsOnList, ShoppingListItem, ShoppingListItemConverter>
{
    public override ShoppingListItemConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<ItemsOnList, ShoppingListItem> mapping)
    {
        mapping
            .ForCtorParam(nameof(ShoppingListItem.Id), opt => opt.MapFrom(src => src.ItemId))
            .ForCtorParam(nameof(ShoppingListItem.TypeId), opt => opt.MapFrom(src => new ItemTypeId(src.ItemTypeId!.Value)))
            .ForCtorParam(nameof(ShoppingListItem.IsInBasket), opt => opt.MapFrom(src => src.InBasket))
            .ForCtorParam(nameof(ShoppingListItem.Quantity), opt => opt.MapFrom(src => src.Quantity));
    }

    protected override ItemsOnList CreateSource()
    {
        return new ItemsOnListEntityBuilder().Create();
    }
}