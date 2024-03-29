﻿using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreForItemContractConverter : IToContractConverter<IStore, StoreForItemContract>
{
    private readonly IToContractConverter<ISection, SectionForItemContract> _sectionConverter;

    public StoreForItemContractConverter(
        IToContractConverter<ISection, SectionForItemContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreForItemContract ToContract(IStore source)
    {
        var sectionsToConvert = source.Sections.Where(s => !s.IsDeleted);
        return new StoreForItemContract(
            source.Id,
            source.Name,
            sectionsToConvert.Select(_sectionConverter.ToContract));
    }
}