﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IItemRepository
    {
        Task<IStoreItem> FindByAsync(StoreItemId storeItemId, ShoppingListStoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindByAsync(ShoppingListStoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindPermanentByAsync(IEnumerable<ShoppingListStoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken);

        Task<IEnumerable<IStoreItem>> FindActiveByAsync(string searchInput, ShoppingListStoreId storeId, CancellationToken cancellationToken);

        Task<IStoreItem> StoreAsync(IStoreItem storeItem, CancellationToken cancellationToken);
        Task<IEnumerable<IStoreItem>> FindActiveByAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);
    }
}