﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.TestTools.Extensions;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services.Conversion.ItemSearchReadModels;

public class ItemSearchReadModelConversionServiceMock : Mock<IItemSearchReadModelConversionService>
{
    public ItemSearchReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IEnumerable<IItem> items, IStore store,
        IEnumerable<SearchItemForShoppingResultReadModel> returnValue)
    {
        Setup(m => m.ConvertAsync(
                It.Is<IEnumerable<IItem>>(i => i.IsEquivalentTo(items)),
                store,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupConvertAsync(IEnumerable<ItemWithMatchingItemTypeIds> mappings, IStore store,
        IEnumerable<SearchItemForShoppingResultReadModel> returnValue)
    {
        Setup(m => m.ConvertAsync(
                It.Is<IEnumerable<ItemWithMatchingItemTypeIds>>(maps => maps.IsEquivalentTo(mappings)),
                store,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}