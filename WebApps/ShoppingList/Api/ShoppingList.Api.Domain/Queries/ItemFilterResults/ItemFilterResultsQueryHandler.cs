﻿using ShoppingList.Api.Domain.Converters;
using ShoppingList.Api.Domain.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.ItemFilterResults
{
    public class ItemFilterResultsQueryHandler : IQueryHandler<ItemFilterResultsQuery, IEnumerable<ItemFilterResultReadModel>>
    {
        private readonly IItemRepository itemRepository;

        public ItemFilterResultsQueryHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemFilterResultReadModel>> HandleAsync(
            ItemFilterResultsQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new System.ArgumentNullException(nameof(query));
            }

            var storeItems = await itemRepository.FindByAsync(query.StoreIds, query.ItemCategoriesIds, query.ManufacturerIds,
                cancellationToken);

            return storeItems.Select(model => model.ToReadModel());
        }
    }
}