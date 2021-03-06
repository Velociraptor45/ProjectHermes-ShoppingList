﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels
{
    public interface IItemSearchReadModelConversionService
    {
        Task<IEnumerable<ItemSearchReadModel>> ConvertAsync(IEnumerable<IStoreItem> items, IStore store, CancellationToken cancellationToken);
    }
}