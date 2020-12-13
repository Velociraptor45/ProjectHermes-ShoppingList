﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports
{
    public interface IShoppingListRepository
    {
        Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<Models.ShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<Models.ShoppingList>> FindActiveByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DomainException"></exception>
        Task<Models.ShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        Task<IEnumerable<Models.ShoppingList>> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DomainException"></exception>
        Task StoreAsync(Models.ShoppingList shoppingList, CancellationToken cancellationToken);
    }
}