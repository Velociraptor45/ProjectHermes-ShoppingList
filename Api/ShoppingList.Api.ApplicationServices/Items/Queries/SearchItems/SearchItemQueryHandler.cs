﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItems;

public class SearchItemQueryHandler : IQueryHandler<SearchItemQuery, IEnumerable<SearchItemResultReadModel>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemQueryServiceDelegate;

    public SearchItemQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemQueryServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> HandleAsync(SearchItemQuery query,
        CancellationToken cancellationToken)
    {
        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchAsync(query.SearchInput);
    }
}