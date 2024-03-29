﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypes;

public class AllQuantityTypesQueryHandler : IQueryHandler<AllQuantityTypesQuery, IEnumerable<QuantityTypeReadModel>>
{
    private readonly IQuantitiesQueryService _quantitiesQueryService;

    public AllQuantityTypesQueryHandler(IQuantitiesQueryService quantitiesQueryService)
    {
        _quantitiesQueryService = quantitiesQueryService;
    }

    public Task<IEnumerable<QuantityTypeReadModel>> HandleAsync(AllQuantityTypesQuery query,
        CancellationToken cancellationToken)
    {
        var result = _quantitiesQueryService.GetAllQuantityTypes();

        return Task.FromResult(result);
    }
}