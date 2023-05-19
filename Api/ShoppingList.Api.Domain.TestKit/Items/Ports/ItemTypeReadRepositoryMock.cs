﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;

public class ItemTypeReadRepositoryMock : Mock<IItemTypeReadRepository>
{
    public ItemTypeReadRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindActiveByAsync(string name, StoreId storeId, int? limit, IEnumerable<ItemTypeId> excludedItemTypeIds,
        IEnumerable<(ItemId, ItemTypeId)> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, excludedItemTypeIds, limit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}