﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQueryHandler :
    IQueryHandler<ItemCategorySearchQuery, IEnumerable<ItemCategoryReadModel>>
{
    private readonly Func<CancellationToken, IItemCategoryQueryService> _itemCategoryQueryServiceDelegate;

    public ItemCategorySearchQueryHandler(
        Func<CancellationToken, IItemCategoryQueryService> itemCategoryQueryServiceDelegate)
    {
        _itemCategoryQueryServiceDelegate = itemCategoryQueryServiceDelegate;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(ItemCategorySearchQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var service = _itemCategoryQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.SearchInput);
    }
}