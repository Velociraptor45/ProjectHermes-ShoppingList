﻿using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeContractConverter : IToContractConverter<QuantityTypeReadModel, QuantityTypeContract>
{
    public QuantityTypeContract ToContract(QuantityTypeReadModel source)
    {
        return new QuantityTypeContract(
            source.Id,
            source.Name,
            source.DefaultQuantity,
            source.PriceLabel,
            source.QuantityLabel,
            source.QuantityNormalizer);
    }
}