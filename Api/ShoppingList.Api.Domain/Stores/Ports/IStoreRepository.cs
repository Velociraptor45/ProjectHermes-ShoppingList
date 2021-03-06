﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Ports
{
    public interface IStoreRepository
    {
        Task<IEnumerable<IStore>> GetAsync(CancellationToken cancellationToken);

        Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken);

        Task<IStore> StoreAsync(IStore store, CancellationToken cancellationToken);

        Task<IStore> FindActiveByAsync(StoreId id, CancellationToken cancellationToken);
        Task<IEnumerable<IStore>> FindByAsync(IEnumerable<StoreId> ids, CancellationToken cancellationToken);
    }
}