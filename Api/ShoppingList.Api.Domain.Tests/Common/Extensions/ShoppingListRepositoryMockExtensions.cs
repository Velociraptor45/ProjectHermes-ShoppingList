﻿using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ShoppingListRepositoryMockExtensions
    {
        public static void SetupFindActiveByAsync(this Mock<IShoppingListRepository> mock, StoreItemId storeItemId,
            IEnumerable<IShoppingList> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreItemId>(id => id == storeItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public static void SetupFindActiveByAsync(this Mock<IShoppingListRepository> mock, StoreId storeId,
            IShoppingList returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public static void SetupFindByAsync(this Mock<IShoppingListRepository> mock, ShoppingListId shoppingListId,
            IShoppingList returnValue)
        {
            mock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == shoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingList>(returnValue));
        }
    }
}