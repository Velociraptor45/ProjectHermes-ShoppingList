﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput, CancellationToken cancellationToken);

        Task<IItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken);

        Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids, CancellationToken cancellationToken);

        Task<IEnumerable<IItemCategory>> FindByAsync(bool includeDeleted, CancellationToken cancellationToken);

        Task<IItemCategory> StoreAsync(IItemCategory model, CancellationToken cancellationToken);
    }
}